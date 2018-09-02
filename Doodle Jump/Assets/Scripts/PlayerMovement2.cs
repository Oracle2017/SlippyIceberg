#if ((UNITY_IPHONE || UNITY_ANDROID) && (!UNITY_EDITOR))
#define MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerCollision))]
public class PlayerMovement2 : MonoBehaviour {
	

	[Header("MOVE SETTINGS")]

	[SerializeField] int moveSpeed = 8;
	[SerializeField] float moveSpeedLimit = 8;
	// Additional move speed. min = 0°, max = 90°.
	float minSlipVelocity;
	float maxSlipVelocity;

	[Space(10)]




	[Header("PHONE SETTINGS")]

	///<summary>
	///Force of the phone tilt
	///</summary>
	[SerializeField] float accelerometerMultiplier = 3.3f;

	///<summary>
	///Until what accelerometer value should it start impacting the movement speed. Value between 0 and 1.
	///</summary>
	[Range(0, 1)]
	[SerializeField] float speedDeadZone = 0.02f;

	///<summary>
	///Clamping the accelerometer value at this limit. Value between 0 and 1. 1 means no limit.
	///</summary>
	[Range(0, 1)]
	[SerializeField] float tiltLimit = 1;

	[Space(10)]





	[Header("PARTICLE EFFECTS")]

	[SerializeField] ParticleSystem dustTrails;
	[SerializeField] float emissionLimit = 10;



	// Flipping the sprite when player changes direction
	SpriteRenderer spriteRenderer;




	// Smooth damping rotation of iceberg.
	float currentVelocity;




	// Use this for initialization
	public void StartSettings () {

		minSlipVelocity = 0;
		maxSlipVelocity = moveSpeed;

		spriteRenderer = GetComponent<SpriteRenderer>();


	}

	public void Move(bool _isTouchingObstacle)
	{
		float hypothenus = (moveSpeed) * Time.deltaTime;
		// TODO: FIX THIS SHIT HERE UNDER
		float angle = transform.rotation.eulerAngles.z;// GameManager.currentPlatform.currentRotation;// -1 * Vector2.SignedAngle(transform.right, Vector2.right);
		//print("movement angle = " + angle);
		float x = Mathf.Cos(angle * Mathf.Deg2Rad) * hypothenus;
		float y = Mathf.Sin(angle * Mathf.Deg2Rad) * hypothenus;
		Vector3 movePosition = new Vector3(x, y, 0);

		Debug.DrawRay(transform.position, movePosition, Color.red);
		Debug.DrawRay(transform.position, -movePosition, Color.yellow);

		var _emission = dustTrails.emission;
		_emission.enabled = true;

		#if MOBILE

			float accelerationX = Mathf.Clamp(Input.acceleration.x * accelerometerMultiplier, -tiltLimit, tiltLimit);
			accelerationX = (Mathf.Abs(accelerationX) < speedDeadZone)? 0: accelerationX;
			_emission.rateOverTime = Math.Abs(accelerationX) * emissionLimit;

			if (_isTouchingObstacle)
			{
				if (!dustTrails.isPlaying)
					dustTrails.Play();

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

				if (_isTouchingObstacle)
				{
					transform.position -= movePosition;
					_emission.rateOverTime = emissionLimit;
					if (!dustTrails.isPlaying)
						dustTrails.Play();
				}

				else 
				{
					transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
					_emission.rateOverTime = 0;
				}
			}

			else if (Input.GetKey(KeyCode.RightArrow))
			{
				dustTrails.transform.localScale = (dustTrails.transform.localScale.x >= 0)? 
				dustTrails.transform.localScale: new Vector3(-dustTrails.transform.localScale.x, dustTrails.transform.localScale.y, dustTrails.transform.localScale.z);

				spriteRenderer.flipX = false;

				if (_isTouchingObstacle)
				{
				
					transform.position += movePosition;
					_emission.rateOverTime = emissionLimit;
					if (!dustTrails.isPlaying)
						dustTrails.Play();

				}

				else 
				{
					transform.position += Vector3.right * moveSpeed * Time.deltaTime;
					_emission.rateOverTime = 0;
				}
			}

			else 
			{
				_emission.rateOverTime = 0;
			}

		#endif

	}




	//TODO: I think this should be ostacle independent
	public void SlipPlayer(float _slopeAngle)
	{

		/*print("regular slope angle = " + _slopeAngle);
		float angle = -1 * Vector2.SignedAngle(transform.right, Vector2.right);

		if (angle < 0)
		{
			_slopeAngle = 90 - Mathf.Abs(_slopeAngle % 90);
		}

		print("recalculated slope angle = " + _slopeAngle);

		float slipVelocity = Utils.Map(Mathf.Abs(_slopeAngle % 90), 0, 90, minSlipVelocity, maxSlipVelocity);

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
		}*/

		if (Mathf.Abs(_slopeAngle) > 90)
		{
			_slopeAngle = _slopeAngle - Mathf.Sign(_slopeAngle) * 180;//- Mathf.Sign(_slopeAngle) * 90 + (_slopeAngle % 90);
		}

		float slipVelocity = -1 * Utils.Map(_slopeAngle, -90, 90, -maxSlipVelocity, maxSlipVelocity);
		//print ("regular slope angle = " + _slopeAngle + " - " + "slip velocity = " + slipVelocity);

		float hypothenus = slipVelocity * Time.deltaTime;
		float x = Mathf.Cos(_slopeAngle * Mathf.Deg2Rad) * hypothenus;
		float y = Mathf.Sin(_slopeAngle * Mathf.Deg2Rad) * hypothenus;
		Vector3 movePosition = new Vector3(x, y, 0);

		transform.position += movePosition;


	}



}
