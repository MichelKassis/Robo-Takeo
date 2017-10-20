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
		float moveVertical = Input.GetAxis("Vertical");
		//bool playerAttack = Input.GetKeyUp ("Fire1");
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		rb2d.AddForce (movement * playerSpeed);
		flip (moveHorizontal);

		myAnimator.SetFloat ("playerSpeed", Mathf.Abs (moveHorizontal));

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

	private void HandleAttacks(){

		if (playerStrike) {
		
			myAnimator.SetTrigger ("playerStrike");
		}
	}

	private void HandleInput(){
	
		if (Input.GetKeyDown (KeyCode.RightShift)) {
		
			playerStrike = true;
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
	
	}





}