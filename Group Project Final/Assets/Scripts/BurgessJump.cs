using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgessJump : MonoBehaviour {

	public float speed;
	public float jumpSpeed;
	Vector2 movement;

	public float fallingGravityModifier;
	public int jumpHoldFrames;
	int currentJumpHoldFrames;

	Rigidbody2D rb;
	//Animator anim;
	//SpriteRenderer sr;

	bool onGround = true;
	bool isJumping = false;
	

	KeyCode jumpKey;

	void Start () {
		// assigning components to our global variables
		rb = GetComponent<Rigidbody2D>();
		//anim = GetComponent<Animator>();
		//sr = GetComponent<SpriteRenderer>();

		jumpKey = KeyCode.Space;
		currentJumpHoldFrames = jumpHoldFrames;
	}
	
	void Update () {
		MovePlayer();
		//HandleSpriteDirection();
		//SetAnimationParameters();
	}

	void FixedUpdate () {
		// using the movement vector that gets assigned in update here in fixedupdate to apply force to the rigidbody
		if (isJumping) {
			rb.AddForce(Vector2.up * jumpSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
			isJumping = false;
		}
		else if (!onGround && Input.GetKey(jumpKey) && currentJumpHoldFrames > 0) {
			rb.AddForce(Vector2.up * jumpSpeed * 0.5f * Time.fixedDeltaTime, ForceMode2D.Impulse);
		}


		if (!onGround && rb.velocity.y < 0) {
			// this means we're in the air and falling. i want to apply some additional gravity in this case;
			Vector2 v = rb.velocity;
			v.y -= fallingGravityModifier * Time.fixedDeltaTime;
			rb.velocity = v;			
		}


		if (onGround) {
			rb.AddForce(movement * speed * Time.fixedDeltaTime);	
		}
		else {
			rb.AddForce(movement * speed * 0.5f * Time.fixedDeltaTime);
		}
	}

	void MovePlayer () {
		// assigning our desired movement vector based on player input in the update loop
		movement = Vector2.zero;

		if (Input.GetKey(KeyCode.LeftArrow)) {
			movement += Vector2.left;
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			movement += Vector2.right;
		}

		if (Input.GetKeyDown(jumpKey) && onGround) {
			isJumping = true;
		}

		if (Input.GetKeyUp(jumpKey) && !onGround) {
			currentJumpHoldFrames = 0;
		}

		if (Input.GetKey(jumpKey) && !onGround) {
			currentJumpHoldFrames--;
		}

	}



	void OnCollisionEnter2D (Collision2D c) {
		if (c.gameObject.tag == "ground") {
			//Debug.Log(c.contacts[0].normal);
			onGround = true;
			currentJumpHoldFrames = jumpHoldFrames;
		}
	}

	void OnCollisionExit2D (Collision2D c) {
		if (c.gameObject.tag == "ground") {
			onGround = false;
		}	
	}



	// void HandleSpriteDirection () {
	// 	if (Mathf.Abs(rb.velocity.x) > 1) {
	// 		sr.flipX = (rb.velocity.x < 0);
	// 	}
	// }

	// void SetAnimationParameters () {
	// 	bool isRunning = (movement.x != 0);
	// 	anim.SetBool("running", isRunning);
	// 	anim.SetBool("jumping", !onGround);
	// }
}
