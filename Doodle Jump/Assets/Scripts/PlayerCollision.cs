using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
	[Header("RAYCASTING SETTINGS")]
	[SerializeField] LayerMask collisionMask;
	[SerializeField] LayerMask waterMask;
	Vector2 rayOriginTopMax;
	float rayLength;
	float skinWidth = 0.15f;
	float totalRotationVelocity;


	// Necessary for collision detection variables.
	float spriteWidth;
	float spriteHeight;



	///<summary>
	/// Keeps data about player collisions.
	/// </summary>
	public CollisionInfo collisionInfo;



	[SerializeField] float landSpeed = 8f;

	// Use this for initialization
	public void StartSettings () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		// max width and height of a sprite of the animation.
		spriteWidth = 2.40f;//spriteRenderer.sprite.bounds.size.x;
		spriteHeight = 2.92f;//spriteRenderer.sprite.bounds.size.y;
		print(spriteWidth);
		print(spriteHeight);

	}
	

	/// <summary>
	/// Check if the player is completely under water. If it is then it resets the player.
	/// </summary>
	public bool WaterCollisionCheck()
	{
		// The Ray is on the top side
		// TODO hit distance should be close to 0!
		float _topRayLength = 0.1f;

		// extendX is the distance between center and right edge of the player.
		// extendY is the distance between center and top edge of the player
		Vector3 extendX =  transform.right * spriteWidth * GameManager.currentPlatform.obstacleScale.x * transform.localScale.x / 2;
		Vector3 extendY = transform.up * spriteHeight * GameManager.currentPlatform.obstacleScale.y * transform.localScale.y / 2;
		Vector2 _rayOriginTopLeft = transform.position + -extendX + extendY;
		Vector2 _rayOriginTopRight = transform.position + extendX + extendY;
		rayOriginTopMax = (_rayOriginTopLeft.y >= _rayOriginTopRight.y)? _rayOriginTopLeft: _rayOriginTopRight;
		RaycastHit2D _hitTop = Physics2D.Raycast(rayOriginTopMax, Vector2.up * -1, _topRayLength, waterMask);

		if (_hitTop)
		{
			return true;
		}

		else 
		{
			return false;
		}
	}




	/// <summary>
	/// Check if the player is touching a platform. If yes then it readjusts the position.
	/// </summary>
	public void PlatformCollisionCheck()
	{

		rayLength = Mathf.Abs(landSpeed * Time.deltaTime);


		// Double Raycasting (both down sides). Because we want to see if the player is fully not touching the obstacle.
		// TODO: not sure if raycasts are even needed here
		for (int i = 0; i < 3; i++)
		{
			int direction = -1 + i; // same but more expensive: (i == 0)? -1: 1; 
			// TODO: put some variables at Start() to make it less computer epensive.
			Vector3 parentScale = Vector3.one;//(transform.parent == GameManager.currentPlatform.transform)? GameManager.currentPlatform.obstacleScale: Vector3.one;
			Vector2 _rayOrigin = transform.position + direction * transform.right * spriteWidth / 2 * transform.localScale.x * parentScale.x - transform.up * spriteHeight / 2 * transform.localScale.y * parentScale.y;
			RaycastHit2D _hit = Physics2D.Raycast(_rayOrigin, Vector2.up * -1, rayLength, collisionMask);

			if (_hit)
			{
				if (collisionInfo.touchingObstacle)
				{
					float rotationVelocity = GameManager.currentPlatform.rotationVelocity;
					totalRotationVelocity += rotationVelocity;
					print("totalRotationVelocity = " + rotationVelocity.ToString());
					transform.RotateAround(GameManager.currentPlatform.transform.position, Vector3.forward, rotationVelocity);
				}

				if (!collisionInfo.touchingObstacle)
				{
					transform.position -= new Vector3(0, _hit.distance, 0);
					collisionInfo.touchingObstacle = true;
					transform.parent = null;//GameManager.currentPlatform.transform;
					transform.rotation = GameManager.currentPlatform.transform.rotation;
				}

				break;
			}

			else if (i == 2)
			{
				// Both raycasts don't make a hit
				collisionInfo.touchingObstacle = false;
				//transform.position -= new Vector3(0, landSpeed * Time.deltaTime, 0);
				transform.parent = null;
				//transform.rotation = Quaternion.identity;
			}

		}
	}

	/// <summary>
	/// Debugging player raycasts in the Unity Editor.
	/// </summary>
	public void DebugRays()
	{
		// TODO: not totally correct, they should be recalculated. They are 1 frame behind.
		for (int i = 0; i < 3; i++)
		{
			int direction = -1 + i; // same but more expensive: (i == 0)? -1: 1; 
			// TODO: put some variables at Start() to make it less computer epensive.
			Vector3 parentScale = Vector3.one;//(transform.parent == GameManager.currentPlatform.transform)? GameManager.currentPlatform.obstacleScale: Vector3.one;
			Vector2 _rayOrigin = transform.position + direction * transform.right * spriteWidth / 2 * transform.localScale.x * parentScale.x - transform.up * spriteHeight / 2 * transform.localScale.y * parentScale.y;
			Debug.DrawRay(_rayOrigin, Vector2.up * -1 * rayLength, Color.red);
		}

		Debug.DrawRay(rayOriginTopMax, Vector2.up * -1 * 0.1f, Color.cyan);
	}

	public struct CollisionInfo {
		public bool touchingObstacle;
		public GameObject collider;
		public float slopeAngle;

		public void Reset() {
			touchingObstacle = false;
			collider = null;
		}
	}
}
