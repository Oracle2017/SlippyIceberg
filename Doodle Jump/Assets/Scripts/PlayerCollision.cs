using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
	[Header("RAYCASTING SETTINGS")]
	[SerializeField] LayerMask collisionMask;
	[SerializeField] LayerMask waterMask;
	Vector2 rayOriginTopMax;
	float rayLength;


	// Necessary for collision detection variables.
	float spriteWidth;
	float spriteHeight;
	float obstacleSpriteHeight;
	Vector3 obstacleScale;


	///<summary>
	/// Keeps data about player collisions.
	/// </summary>
	public CollisionInfo collisionInfo;



	[SerializeField] int landSpeed = 8;

	// Use this for initialization
	void Start () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = spriteRenderer.sprite.bounds.size.x;
		spriteHeight = spriteRenderer.sprite.bounds.size.y;
		// TODO: obstacle must be instantiated
		obstacleSpriteHeight = PerlinNoise.platformSingleton.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		obstacleScale = PerlinNoise.platformSingleton.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
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
		Vector3 extendX =  transform.right * spriteWidth * obstacleScale.x * transform.localScale.x / 2;
		Vector3 extendY = transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
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
		for (int i = 0; i < 2; i++)
		{
			int direction = -1 + 2 * i; // same but more expensive: (i == 0)? -1: 1; 
			// TODO: put some variables at Start() to make it less computer epensive.
			Vector2 _rayOrigin = transform.position + direction * transform.right * spriteWidth * obstacleScale.x * transform.localScale.x / 2 - transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
			RaycastHit2D _hit = Physics2D.Raycast(_rayOrigin, Vector2.up * -1, rayLength, collisionMask);

			if (_hit)
			{
				if (!collisionInfo.touchingObstacle)
				{
					transform.position -= new Vector3(0, _hit.distance, 0);
					collisionInfo.touchingObstacle = true;
				}

				break;
			}

			else if (i == 1)
			{
				// Both raycasts don't make a hit
				collisionInfo.touchingObstacle = false;
				transform.parent = null;
				transform.position -= new Vector3(0, landSpeed * Time.deltaTime, 0);
			}

		}
	}

	/// <summary>
	/// Debugging player raycasts in the Unity Editor.
	/// </summary>
	public void DebugRays()
	{
		// TODO: not totally correct, they should be recalculated. They are 1 frame behind.
		for (int i = 0; i < 2; i++)
		{
			int direction = (i == 0)? -1: 1;
			Vector2 _rayOrigin = transform.position + direction * transform.right * spriteWidth * obstacleScale.x * transform.localScale.x / 2 - transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
			Debug.DrawRay(_rayOrigin, Vector2.up * -1 * rayLength, Color.green);
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
