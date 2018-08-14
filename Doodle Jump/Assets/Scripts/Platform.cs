using UnityEngine;
using System.Collections;

public abstract class Platform : MonoBehaviour {

	[HideInInspector] public float rotationZChange;
	[HideInInspector] public float currentVelocity;
	[HideInInspector] public float currentRotation;

	// For resetting values when player replays
	float startRotationSpeed;
	float startRotationMultiplier;
	[HideInInspector] public Vector3 startPos;

	[Header("Speed of the rotation")]
	[SerializeField] protected float rotationSpeed = 0.1f;
	[SerializeField] float IncreasePerSecond = 10;
	[SerializeField] float SpeedIncrease = 1.5f;
	[SerializeField] float rotationSpeedLimit = 1.5f;
	[HideInInspector] public float rotationVelocity; // TODO: useless?
	public bool stop;
	[HideInInspector] public bool shouldStabilize; // rotation to 0
	[HideInInspector] public float swingTimer;
	[Space(10)]

	[Header("Force of the rotation")]
	[SerializeField] protected float rotationMultiplier = 1;
	bool isChangingSpeed;
	bool isChangingAmplitude;
	[HideInInspector] public bool platformShouldWait;
	float roundTimeLimit;
	float roundTimer;

	[HideInInspector] public float obstacleSpriteHeight;
	[HideInInspector] public Vector3 obstacleStartScale;

	public virtual void StartSettings()
	{
		platformShouldWait = true;

		startPos = transform.position;

		startRotationSpeed = rotationSpeed;
		startRotationMultiplier = rotationMultiplier;

		obstacleSpriteHeight = transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		obstacleStartScale = transform.localScale;

		roundTimeLimit = 2f;
	}


	public virtual void UpdatePlatform()
	{
		rotationVelocity = 0f;

		if (Input.GetMouseButtonDown(1))
		{
			stop = !stop;
		}

		if (stop)
			return;

		//print("roundstart = " + roundStart);

		if (platformShouldWait)
		{
			print(" platform is waiting ");
			StartWait(roundTimeLimit);
			return;
		}
			
		//print("swing ? " + !shouldStabilize);

		if (shouldStabilize)
		{
			currentRotation =  Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, 0f, ref currentVelocity, 1f);
		}

		else 
		{
			currentRotation = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, RotationTarget(), ref currentVelocity, 0.2f);
		}
		//rotationVelocity = Mathf.DeltaAngle(transform.localRotation.eulerAngles.z, currentRotation);
		transform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
		//print("platform angle = " + currentRotation);

		Debug.DrawRay(transform.position, transform.up.normalized, Color.green);

		if (rotationSpeed < rotationSpeedLimit)
		{
			StartCoroutine(ChangeSpeed(IncreasePerSecond));
		}

		/*if (rotationMultiplier < 2)
		{
			
			StartCoroutine(ChangeRotationAmplitude(10f));
		}*/
	}

	protected abstract float RotationTarget();


	protected float RegulateAngle(float _rotationTarget)
	{
		float rotationZ360 = (Mathf.Sign(_rotationTarget) == -1)? _rotationTarget + 360: _rotationTarget;
		if (transform.rotation.z > rotationZ360)
		{
			rotationZChange = transform.rotation.eulerAngles.z - rotationZ360;
		}

		else 
		{
			rotationZChange = _rotationTarget - transform.rotation.eulerAngles.z;
		}

		return _rotationTarget;
	}

	IEnumerator ChangeSpeed(float waitTime)
	{
		if (!isChangingSpeed)
		{
			isChangingSpeed = true;
			yield return new WaitForSeconds(waitTime);
			rotationSpeed *= SpeedIncrease;
			isChangingSpeed = false;
		}
	}

	IEnumerator ChangeRotationAmplitude(float waitTime)
	{
		if (!isChangingAmplitude)
		{
			print("rotationMultiplier = " + rotationMultiplier.ToString());
			isChangingAmplitude = true;
			yield return new WaitForSeconds(waitTime);
			rotationMultiplier += 0.5f;
			isChangingAmplitude = false;
		}
	}

	void StartWait(float _waitTime)
	{
		if (roundTimer < roundTimeLimit)
		{
			roundTimer += Time.deltaTime;
			return;
		}

		else 
		{
			platformShouldWait = false;
			roundTimer = 0;
		}
	}

	public virtual void Reset()
	{
		transform.position = startPos;
		currentVelocity = 0f;
		rotationVelocity = 0f;
		transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		rotationSpeed = startRotationSpeed;
		rotationMultiplier = startRotationMultiplier;
		platformShouldWait = true;
		roundTimer = 0;
		transform.localScale = obstacleStartScale;
	}
}
