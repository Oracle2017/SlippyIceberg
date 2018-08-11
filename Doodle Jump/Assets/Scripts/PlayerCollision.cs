using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
	[Header("RAYCASTING SETTINGS")]
	[SerializeField] LayerMask collisionMask;
	[SerializeField] LayerMask waterMask;
	Vector2 rayOriginTopMax;
	float rayLength;
	[SerializeField] float skinWidth;
	float totalRotationVelocity;
	bool touchedObstacleOnce;


	// Necessary for collision detection variables.
	float spriteWidth;
	float spriteHeight;



	///<summary>
	/// Keeps data about player collisions.
	/// </summary>
	public CollisionInfo collisionInfo;

	[SerializeField] GameObject UIHelper;
	float platformPreviousRotZ;

	int[] raycastOrder;


	// Use this for initialization
	public void StartSettings () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		// max width and height of a sprite of the animation.
		spriteWidth = 2.40f;//spriteRenderer.sprite.bounds.size.x;
		spriteHeight = 2.92f;//spriteRenderer.sprite.bounds.size.y;
		raycastOrder = new int[] { 0, -1, 1 };
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
		Vector3 extendX =  transform.right * spriteWidth  * transform.localScale.x / 2;
		Vector3 extendY = transform.up * spriteHeight * transform.localScale.y / 2;
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
	public void PlatformCollisionCheck(float _landSpeed)
	{

		rayLength = Mathf.Abs(_landSpeed * Time.deltaTime) + 1;


		// Double Raycasting (both down sides). Because we want to see if the player is fully not touching the obstacle.
		// TODO: not sure if raycasts are even needed here
		for (int i = 0; i < 3; i++)
		{
			int direction = raycastOrder[i];//-1 + i; // same but more expensive: (i == 0)? -1: 1; 
			//int direction = -1 + i;
			// TODO: put some variables at Start() to make it less computer epensive.

			//- (direction * transform.right * skinWidth)
			Vector2 _rayOrigin = transform.position + 
				direction * (transform.right * spriteWidth / 2 * transform.localScale.x) - 
				(transform.up * spriteHeight / 2 * transform.localScale.y);
			RaycastHit2D _hit = Physics2D.Raycast(_rayOrigin + Vector2.up, Vector2.up * -1, rayLength, collisionMask);

			if (_hit)
			{
				touchedObstacleOnce = true;
				float rotationVelocity = Vector3.SignedAngle(Vector3.up, _hit.normal, Vector3.forward) - platformPreviousRotZ;
				if (Mathf.Abs(rotationVelocity) >= 2)
				{
					collisionInfo.touchingObstacle = false;
				}

				if (collisionInfo.touchingObstacle)
				{
					//collisionInfo.currentPlatform.rotationVelocity;
					platformPreviousRotZ = Vector3.SignedAngle(Vector3.up, _hit.normal, Vector3.forward);
					print("rotation velocity = " + rotationVelocity);
					print("hit distance = " + _hit.distance);
					totalRotationVelocity += rotationVelocity;
					//print("totalRotationVelocity = " + rotationVelocity.ToString());
					transform.RotateAround(collisionInfo.currentPlatform.transform.position, Vector3.forward, rotationVelocity);
					return;
				}

				else if (!collisionInfo.touchingObstacle)
				{
					float playerRotationZ = Vector3.SignedAngle(Vector3.up, _hit.normal, Vector3.forward);
					platformPreviousRotZ = playerRotationZ;

					collisionInfo.currentPlatform = _hit.transform.GetComponent<Platform>();
					collisionInfo.touchingObstacle = true;

					transform.rotation = Quaternion.Euler(0, 0, playerRotationZ);


					transform.position = (Vector3) _hit.point + 
						-direction * (transform.right * spriteWidth / 2 * transform.localScale.x) +
						transform.up * spriteHeight / 2 * transform.localScale.y;

					print("hit distance = " + _hit.distance);
					//transform.rotation = Quaternion.Euler(0, 0, playerRotationZ);//collisionInfo.currentPlatform.transform.rotation;
					//Debug.Break();
					return;
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

				if (touchedObstacleOnce)
				{
					//Debug.Break();
				}
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
			int direction = raycastOrder[i];
			//int direction = -1 + i; // same but more expensive: (i == 0)? -1: 1; 
			// TODO: put some variables at Start() to make it less computer epensive.
			Vector2 _rayOrigin = transform.position + 
				direction * (transform.right * spriteWidth / 2 * transform.localScale.x) - 
				(transform.up * spriteHeight / 2 * transform.localScale.y) + Vector3.up;
			
			Debug.DrawRay(_rayOrigin, Vector2.up * -1 * rayLength, Color.red);
		}

		Debug.DrawRay(rayOriginTopMax, Vector2.up * -1 * 0.1f, Color.cyan);
	}

	public struct CollisionInfo {
		public Platform currentPlatform;
		public bool touchingObstacle;
		public float slopeAngle;

		public void Reset() {
			touchingObstacle = false;
			currentPlatform = null;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Coin")
		{
			print("touched coin");
			Destroy(col.gameObject);
		}
	}

	public void Reset()
	{
		transform.rotation = Quaternion.identity;
		collisionInfo.Reset();
		touchedObstacleOnce = false;
	}
}
