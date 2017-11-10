using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	public float playerSpeed;
	public Text countText;
	public Text winText;
	public bool playerDie;
	public bool playerStrike;
	private Rigidbody2D rb2d;
	private int count;
	private Animator myAnimator;
	private bool isGrounded;
	private bool canRun;
	private bool jump;
	[SerializeField]
	private bool airControl;

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;


	[SerializeField]
	private LayerMask whatIsGround;





	private bool facingRight;

	void Start()
	{
		facingRight = true;
		rb2d = GetComponent<Rigidbody2D> ();
		playerDie = false;
		myAnimator = GetComponent <Animator> ();
	}

	void Update (){
	
		HandleInput ();

	}

	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis("Horizontal");
		//float moveVertical = Input.GetAxis("Vertical");
		isGrounded = isGroundedFunc();
		canRun = isGroundedFunc ();

		HandleMovement (moveHorizontal, 0);
		HandleAttacks ();
		resetValues ();
	}




	private void flip(float moveHorizontal){
		if (moveHorizontal > 0 && !facingRight || moveHorizontal < 0 && facingRight ) {
		
			facingRight = !facingRight;

			Vector2 theScale = transform.localScale;

			theScale.x *= -1;
			transform.localScale = theScale;

		}
	
	}

	private void HandleMovement(float moveHorizontal, float moveVertical){

			//bool playerAttack = Input.GetKeyUp ("Fire1");
			rb2d.velocity = new Vector2 (moveHorizontal * playerSpeed, rb2d.velocity.y);
	
		//rb2d.AddForce (movement * playerSpeed);
		flip (moveHorizontal);

		if (isGrounded && jump){

			isGrounded = false;

			rb2d.AddForce (new Vector2 (0, jumpForce));

		}

		myAnimator.SetFloat ("playerSpeed", Mathf.Abs (moveHorizontal));

	}

	private void HandleAttacks(){

		if (playerStrike) {
		
			myAnimator.SetTrigger ("playerStrike");
		}
	}

	//input of game
	private void HandleInput(){
	
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
		
			playerStrike = true;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
		
			jump = true;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("PickUp")) {
			
			other.gameObject.SetActive (false);
			count = count + 1;
		}

		if (other.gameObject.CompareTag ("Enemy")){

			if (this.myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("playerStrike")) {
			
				other.gameObject.SetActive (false);
			} else {

				playerDie = true;
				gameObject.SetActive (false);
			}
		}
	}

	private void resetValues(){
		playerStrike = false;
		jump = false;
	
	}

	private bool isGroundedFunc(){
		if (rb2d.velocity.y <= 0)
		{
		
			foreach (Transform point in groundPoints) 
			{
			
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);

				for (int i = 0; i < colliders.Length; i++) 
				{
				
					if (colliders [i].gameObject != gameObject) 
					{
					
						return true;
					}
				}
			}
		
		}
		return false;
	
	}





}