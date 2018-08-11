using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_ScaleChanger : MonoBehaviour {

	Vector3 currentScaleVelocity;
	Vector3 currentLocalScale;
	Vector3 currentScaleTarget;
	bool isScaling;

	void Start()
	{
		currentLocalScale = transform.localScale;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
			isScaling = true;
			currentScaleTarget = new Vector3(3 * currentLocalScale.x, transform.localScale.y, transform.localScale.z);
			print("current local scale x = " + transform.localScale.x);
			print("target scale x = " + 3 * currentLocalScale.x);
		}

		if (isScaling)
		{
			ChangeScaleTo(currentScaleTarget, 10f);
			if (transform.localScale.x == currentScaleTarget.x)
			{
				currentLocalScale = transform.localScale;
				isScaling = false;
			}
		}
	}

	void ChangeScaleTo(Vector3 _newScale, float _transitionSpeed)
	{
		transform.localScale = Vector3.SmoothDamp(transform.localScale, _newScale, ref currentScaleVelocity, _transitionSpeed);
	}
		
}
