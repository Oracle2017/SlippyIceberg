#if ((UNITY_IPHONE || UNITY_ANDROID) && (!UNITY_EDITOR))
#define MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement2 : MonoBehaviour {
	Rigidbody2D rb;

	[Header("MOVE SETTINGS")]

	[SerializeField] int moveSpeed;
	[SerializeField] float moveSpeedLimit;
	// Additional move speed. min = 0°, max = 90°.
	float minSlipVelocity;
	float maxSlipVelocity;
	[SerializeField] int landSpeed;

	[Space(10)]




	[Header("PHONE SETTINGS")]

	///<summary>
	///Force of the phone tilt
	///</summary>
	[SerializeField] float accelerometerMultiplier;

	///<summary>
	///Until what accelerometer value should it start impacting the movement speed. Value between 0 and 1.
	///</summary>
	[Range(0, 1)]
	[SerializeField] float speedDeadZone;

	///<summary>
	///Clamping the accelerometer value at this limit. Value between 0 and 1. 1 means no limit.
	///</summary>
	[Range(0, 1)]
	[SerializeField] float tiltLimit;

	[Space(10)]





	[Header("RAYCASTING SETTINGS")]
	[SerializeField] LayerMask collisionMask;
	[SerializeField] LayerMask waterMask;
	Vector2 rayOriginTopMax;
	float rayLength;

	// Necessary for collision detection variables.
	SpriteRenderer spriteRenderer;
	float spriteWidth;
	float spriteHeight;
	float obstacleSpriteHeight;
	Vector3 obstacleScale;

	///<summary>
	/// Keeps data about player collisions.
	/// </summary>
	CollisionInfo collisionInfo;

	[Space(10)]




	[Header("PARTICLE EFFECTS")]

	[SerializeField] ParticleSystem dustTrails;




	// Smooth damping rotation of iceberg.
	float currentVelocity;




	// Reset Variables
	Vector3 startPos;
	[HideInInspector] public static bool isDead;





	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();

		minSlipVelocity = 0;
		maxSlipVelocity = moveSpeed;

		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = spriteRenderer.sprite.bounds.size.x;
		spriteHeight = spriteRenderer.sprite.bounds.size.y;
		// TODO: obstacle must be instantiated
		obstacleSpriteHeight = PerlinNoise.platformSingleton.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		obstacleScale = PerlinNoise.platformSingleton.transform.localScale;

		startPos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		if (isDead)
			return;

		//TODO: must not be parented
		// Parenting to obstacle for getting the rotation
		transform.parent = PerlinNoise.platformSingleton.transform;
		transform.localRotation = Quaternion.identity;


		WaterCollisionCheck();

		PlatformCollisionCheck();

		Move();

		if (collisionInfo.touchingObstacle)
		{
			SlipPlayer();
		}
			
		DebugRays();
	}




	/// <summary>
	/// Check if the player is completely under water. If it is then it resets the player.
	/// </summary>
	void WaterCollisionCheck()
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
			isDead = true;
			//TODO: the Reset() function should be triggered from another script and the code under here should be removed
			transform.position = startPos;
		}
	}




	/// <summary>
	/// Check if the player is touching a platform. If yes then it readjusts the position.
	/// </summary>
	void PlatformCollisionCheck()
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


		

	void Move()
	{
		float hypothenus = (moveSpeed) * Time.deltaTime;
		float angle = -1 * Vector2.SignedAngle(transform.right, Vector2.right);
		float x = Mathf.Cos(angle * Mathf.Deg2Rad) * hypothenus;
		float y = Mathf.Sin(angle * Mathf.Deg2Rad) * hypothenus;
		Vector3 movePosition = new Vector3(x, y, 0);

		Debug.DrawRay(transform.position, movePosition, Color.red);
		Debug.DrawRay(transform.position, -movePosition, Color.yellow);


		#if MOBILE

			float accelerationX = Mathf.Clamp(Input.acceleration.x * accelerometerMultiplier, -tiltLimit, tiltLimit);
			accelerationX = (Mathf.Abs(accelerationX) < speedDeadZone)? 0: accelerationX;

			if (collisionInfo.touchingObstacle)
			{
				transform.position += Vector3.ClampMagnitude(accelerationX * movePosition, moveSpeedLimit);

				if (accelerationX < 0)
				{
					spriteRenderer.flipX = true;
					dustTrails.transform.localScale = (dustTrails.transform.localScale.x < 0)? 
						dustTrails.transform.localScale: new Vector3(-dustTrails.transform.localScale.x, dustTrails.transform.localScale.y, dustTrails.transform.localScale.z);


				}

				else 
				{
					spriteRenderer.flipX = false;
					dustTrails.transform.localScale = (dustTrails.transform.localScale.x >= 0)? 
						dustTrails.transform.localScale: new Vector3(-dustTrails.transform.localScale.x, dustTrails.transform.localScale.y, dustTrails.transform.localScale.z);

				}
			}

			else 
			{
				transform.position += Vector3.ClampMagnitude(Vector3.right * accelerationX * moveSpeed * Time.deltaTime, moveSpeedLimit);
			}

			return;


		#else

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				dustTrails.transform.localScale = (dustTrails.transform.localScale.x < 0)? 
				dustTrails.transform.localScale: new Vector3(-dustTrails.transform.localScale.x, dustTrails.transform.localScale.y, dustTrails.transform.localScale.z);
				
				spriteRenderer.flipX = true;

				if (collisionInfo.touchingObstacle)
				{
					
					transform.position -= movePosition;
				}

				else 
				{
					transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
				}
			}

			else if (Input.GetKey(KeyCode.RightArrow))
			{
				dustTrails.transform.localScale = (dustTrails.transform.localScale.x >= 0)? 
				dustTrails.transform.localScale: new Vector3(-dustTrails.transform.localScale.x, dustTrails.transform.localScale.y, dustTrails.transform.localScale.z);

				spriteRenderer.flipX = false;

				if (collisionInfo.touchingObstacle)
				{
					transform.position += movePosition;
				}

				else 
				{
					transform.position += Vector3.right * moveSpeed * Time.deltaTime;
				}
			}

		#endif

	}




	//TODO: I think this should be ostacle independent
	public void SlipPlayer()
	{
		collisionInfo.slopeAngle = PerlinNoise.platformSingleton.transform.rotation.eulerAngles.z;

		float angle = -1 * Vector2.SignedAngle(transform.right, Vector2.right);

		if (angle < 0)
		{
			collisionInfo.slopeAngle = 90 - Mathf.Abs(collisionInfo.slopeAngle % 90);
		}

		float slipVelocity = Utils.Map(Mathf.Abs(collisionInfo.slopeAngle % 90), 0, 89, minSlipVelocity, maxSlipVelocity);

		float hypothenus = slipVelocity * Time.deltaTime;
		float x = Mathf.Cos(angle * Mathf.Deg2Rad) * hypothenus;
		float y = Mathf.Sin(angle * Mathf.Deg2Rad) * hypothenus;
		Vector3 movePosition = new Vector3(x, y, 0);


		if (angle < 0)
		{
			transform.position += movePosition;
		}

		else 
		{
			transform.position -= movePosition;
		}

	}




	/// <summary>
	/// Debugging player raycasts in the Unity Editor.
	/// </summary>
	void DebugRays()
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




	public void Reset()
	{
		isDead = false;
		transform.position = startPos;
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
