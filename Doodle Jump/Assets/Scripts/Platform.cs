using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {


	public int scale = 10;
	public float offset = 100f;

	[HideInInspector] public float rotationZChange;
	float currentVelocity;

	// For resetting values when player replays
	float startRotationSpeed;
	float startRotationMultiplier;

	float rotationSpeed;
	float rotationMultiplier;
	bool isChangingSpeed;
	bool isChangingAmplitude;
	bool isStarting;

	[HideInInspector] public float obstacleSpriteHeight;
	[HideInInspector] public Vector3 obstacleScale;

	public void StartSettings()
	{
		offset = Random.Range(0, 100000);
		rotationSpeed = 0.1f;
		rotationMultiplier = 1;

		startRotationSpeed = rotationSpeed;
		startRotationMultiplier = rotationMultiplier;

		obstacleSpriteHeight = transform.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		obstacleScale = transform.localScale;
	}


	public void UpdatePlatform()
	{
		if (GameManager.currentPlayer.isDead)
		{
			Reset();
			return;
		}


		if (!isStarting)
		{
			StartCoroutine(StartWait(2f));
			return;
		}

		offset += Time.deltaTime * rotationSpeed;
		//transform.localRotation = Quaternion.Euler(0f, 0f, RotationTarget());
		//print(transform.right.normalized);*/


		float currentRotation = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.z, RotationTarget(), ref currentVelocity, 0.2f);
		transform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
		Debug.DrawRay(transform.position, transform.up.normalized, Color.green);

		if (rotationSpeed < 1f)
		{
			StartCoroutine(ChangeSpeed(8f));
		}

		/*if (rotationMultiplier < 2)
		{
			
			StartCoroutine(ChangeRotationAmplitude(5f));
		}*/
	}

	float RotationTarget()
	{
		float zRotation = scale + offset;
		float sample = Mathf.PerlinNoise(zRotation, 0);
		sample = Mathf.Clamp(sample, 0, 1);
		float rotationTarget = Utils.Map(sample * rotationMultiplier, 0, 1, -90, 90);

		//rotationZChange = (transform.rotation.z > rotationZ)? transform.rotation.eulerAngles.z - rotationZ: (rotationZ - transform.rotation.eulerAngles.z) * -1;
		//print(rotationZChange);

		float rotationZ360 = (Mathf.Sign(rotationTarget) == -1)? rotationTarget + 360: rotationTarget;
		if (transform.rotation.z > rotationZ360)
		{
			rotationZChange = transform.rotation.eulerAngles.z - rotationZ360;
		}

		else 
		{
			//print("transform.rotation.z <= rotationZ");
			//print(rotationZ - transform.rotation.eulerAngles.z);
			rotationZChange = rotationTarget - transform.rotation.eulerAngles.z;
		}

		return rotationTarget;

	}

	IEnumerator ChangeSpeed(float waitTime)
	{
		if (!isChangingSpeed)
		{
			isChangingSpeed = true;
			yield return new WaitForSeconds(waitTime);
			rotationSpeed *= 2;
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

	IEnumerator StartWait(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		isStarting = true;
	}

	public void Reset()
	{
		transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		rotationSpeed = startRotationSpeed;
		rotationMultiplier = startRotationMultiplier;
		isStarting = false;
		offset = Random.Range(0, 100000);
	}
}
