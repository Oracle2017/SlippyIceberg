using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_ScaleChanger : MonoBehaviour {

	Vector3 currentScaleVelocity;
	Vector3 currentLocalScale;
	//Vector3 currentScaleTarget;
	bool isScaling;
	Vector3 newScale;
	float transitionSpeed;
	bool platformStop;

	void Start()
	{
		currentLocalScale = transform.localScale;
	}

	void Update()
	{
		if (isScaling)
		{
			transform.localScale = Vector3.SmoothDamp(transform.localScale, newScale, ref currentScaleVelocity, transitionSpeed);
			if (transform.localScale.x - newScale.x <= 0.1f && 
				transform.localScale.x - newScale.x >= -0.1f)
			{
				currentLocalScale = transform.localScale;
				isScaling = false;
				GetComponent<Platform>().stop = platformStop;
			}
		}
	}

	public void ChangeScaleTo(float _newScaleX, float _transitionSpeed, bool _platformStop)
	{
		newScale = new Vector3(_newScaleX, transform.localScale.y, transform.localScale.z);
		transitionSpeed = _transitionSpeed;
		isScaling = true;
		platformStop = _platformStop;
	}
		
}
