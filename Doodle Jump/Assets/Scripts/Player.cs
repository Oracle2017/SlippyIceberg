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
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	public void UpdatePlayer () {

		if (isDead)
			return;

		//TODO: must not be parented
		// Parenting to obstacle for getting the rotation
		transform.parent = PerlinNoise.platformSingleton.transform;
		transform.localRotation = Quaternion.identity;

		if (playerCollision.WaterCollisionCheck())
		{
			isDead = true;
		}

		playerCollision.PlatformCollisionCheck();

		playerMovement.Move(playerCollision.collisionInfo.touchingObstacle);

		if (playerCollision.collisionInfo.touchingObstacle)
		{
			playerCollision.collisionInfo.slopeAngle = PerlinNoise.platformSingleton.transform.rotation.eulerAngles.z;
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
