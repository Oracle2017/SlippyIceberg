using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump : MonoBehaviour {
	Rigidbody2D rb;
	[HideInInspector] public static float startJumpY;
	[HideInInspector] public static float endJumpY;
	[HideInInspector] public static bool isJumping;

	[Header("JUMP SETTINGS")]
	[Range(0.0f, 1.0f)]
	[SerializeField] float jumpSpeed;
	[SerializeField] int jumpHeight;
	[SerializeField] float landingSpeed = 1;

	bool isScriptEnabled;


	// Use this for initialization
	void OnEnable () {
		rb = GetComponent<Rigidbody2D>();
		startJumpY = transform.position.y - jumpHeight;
		endJumpY = transform.position.y;
		isScriptEnabled = true;
	}

	void Update()
	{
		
	}

	void FixedUpdate()
	{
		if (!isScriptEnabled)
			return;

		if (isJumping && rb.position.y <= endJumpY)
		{
			float newPosY = Mathf.Lerp(rb.position.y, endJumpY, 1 - jumpSpeed);
			rb.position = new Vector2(rb.position.x, newPosY);

			if (rb.position.y + 0.5f >= endJumpY)
			{
				rb.gravityScale = landingSpeed;
				isJumping = false;
			}
		}
	}

	void Jump()
	{
		if (!isScriptEnabled)
			return;

		if (isJumping)
			return;

		rb.gravityScale = 0;
		startJumpY = rb.position.y;
		endJumpY = rb.position.y + jumpHeight;
		rb.velocity = new Vector2(rb.velocity.x, 0);
		isJumping = true;
	}
		

	/*void Jump()
	{
		if (rb.velocity.y > 0)
			return;

		Vector2 jump = new Vector2(0, jumpHeight);
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
			//rb.velocity += jump; 
	}*/

	void OnCollisionEnter2D()
	{
		// TODO: check for platform tag
		Jump();
	}

	void OnTriggerEnter2D()
	{
		// TODO: check for platform tag
		Jump();
	}

	void OnDisable()
	{
		isScriptEnabled = false;
	}
}
