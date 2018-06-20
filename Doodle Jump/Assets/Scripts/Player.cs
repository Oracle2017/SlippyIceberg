using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement2))]
[RequireComponent(typeof(PlayerCollision))]
public class Player : MonoBehaviour {

	[SerializeField] PlayerMovement2 playerMovement;
	[SerializeField] PlayerCollision playerCollision;

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

		//TODO: must not be parented
		// Parenting to obstacle for getting the rotation
		transform.parent = GameManager.currentPlatform.transform;
		transform.localRotation = Quaternion.identity;

		if (playerCollision.WaterCollisionCheck())
		{
			isDead = true;
		}

		playerCollision.PlatformCollisionCheck();

		playerMovement.Move(playerCollision.collisionInfo.touchingObstacle);

		if (playerCollision.collisionInfo.touchingObstacle)
		{
			playerCollision.collisionInfo.slopeAngle = GameManager.currentPlatform.transform.rotation.eulerAngles.z;
			playerMovement.SlipPlayer(playerCollision.collisionInfo.slopeAngle);
		}

		playerCollision.DebugRays();
	}

	public void Reset()
	{
		isDead = false;
		transform.position = startPos;
	}
}
