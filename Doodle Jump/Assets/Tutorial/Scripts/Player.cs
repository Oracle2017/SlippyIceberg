using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborn = .2f;
	float accelerationTimeGrounded = .1f;

	Controller2D controller;
	Vector3 velocity;
	float moveSpeed = 6;
	float jumpVelocity;
	float gravityForce;
	float velocityXSmoothing;



	void Start()
	{
		controller = GetComponent<Controller2D>();

		gravityForce = -( 2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravityForce) * timeToJumpApex;

	}

	void Update()
	{
		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.Set(velocity.x, 0, velocity.z);
		}

		if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
		{
			velocity.Set(velocity.x, jumpVelocity, velocity.z);
		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		float targetVelocityX = input.x * moveSpeed;
		velocity = new Vector3(Mathf.SmoothDamp(velocity.x, targetVelocityX /*+ controller.SlipVelocityX()*/, ref velocityXSmoothing, (controller.collisions.below? accelerationTimeGrounded: accelerationTimeAirborn)), velocity.y, velocity.z);
		Vector3 gravity = new Vector3(0, gravityForce * Time.deltaTime, 0);
		velocity += gravity;
		controller.Move(velocity * Time.deltaTime);
		controller.ReadjustPlayer();
	}



}
