#if ((UNITY_IPHONE || UNITY_ANDROID) && (!UNITY_EDITOR))
#define MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement2 : MonoBehaviour {
	Rigidbody2D rb;

	[Header("MOVE SETTINGS")]
	// not using this atm
	[SerializeField] int moveSpeed;
	[SerializeField] float moveSpeedLimit;

	// Until what accelerometer value should it start impacting the movement speed. Value between 0 and 1.
	[SerializeField] float speedDeadZone;

	//  Clamping the accelerometer value at this limit. Value between 0 and 1. 1 means no limit
	[SerializeField] float tiltLimit;

	[SerializeField] int landSpeed;
	[SerializeField] LayerMask collisionMask;
	[SerializeField] LayerMask waterMask;
	SpriteRenderer spriteRenderer;
	float spriteWidth;
	float spriteHeight;
	float obstacleSpriteHeight;
	[Space(10)]

	CollisionInfo collisionInfo;
	float minSlipVelocity;
	float maxSlipVelocity;
	[SerializeField] bool phoneInput;
	float currentVelocity;
	float accelerationX;
	[SerializeField] float accelerometerMultiplier;
	Vector3 startPos;

	[HideInInspector] public static bool isDead;

	[SerializeField] ParticleSystem dustTrails;
	int dustTrailDirection = 0;




	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();

		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		spriteHeight = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		obstacleSpriteHeight = PerlinNoise.platformSingleton.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

		minSlipVelocity = 0;
		maxSlipVelocity = moveSpeed;
		startPos = transform.position;

		//transform.localScale = Vector2.Scale(transform.localScale, new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1));
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead)
			return;

		// Parenting to obstacle for getting the rotation
		transform.parent = PerlinNoise.platformSingleton.transform;
		transform.localRotation = Quaternion.identity;
		Vector3 obstacleScale = PerlinNoise.platformSingleton.transform.localScale;
		//transform.localScale = new Vector3(0.15f / obstacleScale.x, 0.15f / obstacleScale.y, 1);


		// TOP RAY
		float _rayLength = Mathf.Abs(landSpeed * Time.deltaTime);
		float _topRayLength = 0.1f;

		Vector2 _rayOriginTopLeft = transform.position + -1 * transform.right * spriteWidth * obstacleScale.x * transform.localScale.x / 2 + transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
		Vector2 _rayOriginTopRight = transform.position + -1 * transform.right * spriteWidth * obstacleScale.x * transform.localScale.x / 2 + transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
		Vector2 _rayOriginTop = (_rayOriginTopLeft.y >= _rayOriginTopRight.y)? _rayOriginTopLeft: _rayOriginTopRight;
		//float _rayLength = Mathf.Abs(landSpeed * Time.deltaTime);
		RaycastHit2D _hitTop = Physics2D.Raycast(_rayOriginTop, Vector2.up * -1, _topRayLength, waterMask);

		if (_hitTop)
		{
			print("touching water");
			isDead = true;
			transform.position = startPos;
		}


		// Double Raycasting
		for (int i = 0; i < 2; i++)
		{
			int direction = (i == 0)? -1: 1;
			Vector2 _rayOrigin = transform.position + direction * transform.right * spriteWidth * obstacleScale.x * transform.localScale.x / 2 - transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
			//float _rayLength = Mathf.Abs(landSpeed * Time.deltaTime);
			RaycastHit2D _hit = Physics2D.Raycast(_rayOrigin, Vector2.up * -1, _rayLength, collisionMask);


			if (_hit)
			{
				if (!collisionInfo.touchingObstacle)
				{
					transform.position -= new Vector3(0, _hit.distance, 0);
					collisionInfo.touchingObstacle = true;
					collisionInfo.collider = _hit.collider.gameObject;

					/*if (_hit.collider.tag == "DeadZone")
					{
						print("touching water");
						transform.position = startPos;
					}*/
				}
					
				//print(_hit.collider.tag + "LOOOOOOK HERE");
				break;
			}

			else if (i == 1)
			{
				collisionInfo.collider = null;
				collisionInfo.touchingObstacle = false;
				transform.position -= new Vector3(0, landSpeed * Time.deltaTime, 0);
				transform.parent = null;
			}
				
		}

		collisionInfo.slopeAngle = PerlinNoise.platformSingleton.transform.rotation.eulerAngles.z;
		//print("slope Angle = " + collisionInfo.slopeAngle);


		Move();

		if (collisionInfo.touchingObstacle)
			SlipPlayer();

		// Just for debugging the ray
		/*rayOrigin = transform.position - transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
		Debug.DrawRay(rayOrigin, Vector2.up * -1 * rayLength, Color.red);*/
		for (int i = 0; i < 2; i++)
		{
			int direction = (i == 0)? -1: 1;
			Vector2 _rayOrigin = transform.position + direction * transform.right * spriteWidth * obstacleScale.x * transform.localScale.x / 2 - transform.up * spriteHeight * obstacleScale.y * transform.localScale.y / 2;
			Debug.DrawRay(_rayOrigin, Vector2.up * -1 * _rayLength, Color.green);
		}

		Debug.DrawRay(_rayOriginTop, Vector2.up * -1 * 0.1f, Color.cyan);
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


		/*Vector3 targetPosition;

		targetPosition = transform.position;*/

		#if MOBILE

			accelerationX = Mathf.Clamp(Input.acceleration.x * accelerometerMultiplier, -tiltLimit, tiltLimit);
			accelerationX = (Mathf.Abs(accelerationX) < speedDeadZone)? 0: accelerationX;

			if (collisionInfo.touchingObstacle)
			{
				transform.position += Vector3.ClampMagnitude(accelerationX * movePosition, moveSpeedLimit);

				if (accelerationX < 0)
				{
					dustTrailDirection = 180;
					spriteRenderer.flipX = true;
					dustTrails.transform.localScale = (dustTrails.transform.localScale.x < 0)? 
						dustTrails.transform.localScale: new Vector3(-dustTrails.transform.localScale.x, dustTrails.transform.localScale.y, dustTrails.transform.localScale.z);


				}

				else 
				{
					dustTrailDirection = 0;
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
				dustTrailDirection = 180;
				//targetPosition = transform.position - movePosition;
				if (collisionInfo.touchingObstacle)
				{
					
					transform.position -= movePosition;
				}

				else 
				{
					transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
					print("move real left");
				}
			}

			else if (Input.GetKey(KeyCode.RightArrow))
			{
				dustTrails.transform.localScale = (dustTrails.transform.localScale.x >= 0)? 
		dustTrails.transform.localScale: new Vector3(-dustTrails.transform.localScale.x, dustTrails.transform.localScale.y, dustTrails.transform.localScale.z);


				spriteRenderer.flipX = false;
				dustTrailDirection = 0;

				if (collisionInfo.touchingObstacle)
				{
					transform.position += movePosition;
				}

				else 
				{
					transform.position += Vector3.right * moveSpeed * Time.deltaTime;
					print("move real right");
				}
			}

		#endif


		//dustTrails.transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.z + dustTrailDirection, -90, 0);
		//dustTrails.transform.localRotation = Quaternion.Euler(transform.right
		//Debug.DrawRay(dustTrails.transform.position, new Vector3(0, 0, transform.rotation.eulerAngles.z + dustTrailDirection) * 100, Color.white);
		//dustTrails.transform.LookAt(-transform.right);
		//dustTrails.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(dustTrails.transform.rotation.eulerAngles, transform.right * -1, 90, 90));
		//transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, Time.deltaTime * 10);
	}

	public void SlipPlayer()
	{

		float angle = -1 * Vector2.SignedAngle(transform.right, Vector2.right);

		if (angle < 0)
		{
			collisionInfo.slopeAngle = 90 - Mathf.Abs(collisionInfo.slopeAngle % 90);
		}

		float slipVelocity = Utils.Map(Mathf.Abs(collisionInfo.slopeAngle % 90), 0, 89, minSlipVelocity, maxSlipVelocity);

		float hypothenus = slipVelocity * Time.deltaTime;
		//angle = (angle < 0)? angle + 90: angle;
		//print("angle = " + angle);
		float x = Mathf.Cos(angle * Mathf.Deg2Rad) * hypothenus;
		float y = Mathf.Sin(angle * Mathf.Deg2Rad) * hypothenus;
		Vector3 movePosition = new Vector3(x, y, 0);
		//print("velocity slip = " + slipVelocity);


		//Debug.DrawRay(transform.position, movePosition, Color.red);
		//Debug.DrawRay(transform.position, -movePosition, Color.yellow);



		if (angle < 0)
		{
			transform.position += movePosition;
		}

		else 
		{
			transform.position -= movePosition;
		}

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
