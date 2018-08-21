using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement2))]
[RequireComponent(typeof(PlayerCollision))]
public class Player : MonoBehaviour {

	[SerializeField] PlayerMovement2 playerMovement;
	[SerializeField] PlayerCollision playerCollision;
	[SerializeField] PlayerJump playerJump;
	[SerializeField] bool slip;

	// Reset Variables
	Vector3 startPos;
	[HideInInspector] public bool isDead;

	// Use this for initialization
	public void StartSettings () {
		startPos = transform.position;

		playerMovement.StartSettings();
		playerCollision.StartSettings();
	}
	
	// Update is called once per frame
	public void UpdatePlayer () {

		if (isDead)
			return;

		//transform.localRotation = Quaternion.identity;

		if (playerCollision.WaterCollisionCheck())
		{
			isDead = true;
		}


		playerCollision.PlatformCollisionCheck(playerJump.landingSpeed);
		playerCollision.DebugRays();

		playerMovement.Move(playerCollision.collisionInfo.touchingObstacle);

		if (playerCollision.collisionInfo.touchingObstacle && slip)
		{
			playerCollision.collisionInfo.slopeAngle = 
				playerCollision.collisionInfo.currentPlatform.transform.rotation.eulerAngles.z;
			// Vector2.SignedAngle(Vector2.up, playerCollision.collisionInfo.currentPlatform.transform.rotation.eulerAngles);//
			playerCollision.collisionInfo.slopeAngle = Mathf.DeltaAngle(0f, playerCollision.collisionInfo.slopeAngle);

			//print("initial slope angle = " + playerCollision.collisionInfo.slopeAngle);
			playerMovement.SlipPlayer(playerCollision.collisionInfo.slopeAngle);
		}

		playerJump.UpdateSettings(playerCollision.collisionInfo.touchingObstacle);


	}

	public void Reset()
	{
		isDead = false;
		transform.position = startPos;
		playerCollision.Reset();
		playerJump.Reset();
	}
}
