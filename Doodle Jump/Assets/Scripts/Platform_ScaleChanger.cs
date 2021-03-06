﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_ScaleChanger : MonoBehaviour {

	Vector3 currentScaleVelocity;
	Vector3 currentLocalScale;
	//Vector3 currentScaleTarget;
	bool isScaling;
	Vector3 newScale;
	float transitionSpeed;
	SpriteRenderer spriteRenderer;
	[SerializeField] Color scalingColor;
	[SerializeField] float waitTimeLimit = 1;
	float waitTimer = 0;
	bool isWaiting;
	Color startColor;

	public void StartSettings()
	{
		currentLocalScale = transform.localScale;
		spriteRenderer = GetComponent<SpriteRenderer>();
		startColor = spriteRenderer.color;
	}

	public void UpdateSettings()
	{
		if (isScaling)
		{
			Vector3 _obstacleSpriteSize = GameManager.currentPlatform.obstacleSpriteSize;
			Vector3 _guiScalingStartDist = new Vector3(_obstacleSpriteSize.x * transform.localScale.x / 2, 0, 0);
			//print("_guiScalingStartDist = " + _guiScalingStartDist);
			Vector3 _guiScalingEndDist =  new Vector3(_obstacleSpriteSize.x * newScale.x / 2, 0, 0);
			GUI_Manager.guiScaling.UpdateSettings(transform.position, _guiScalingStartDist, _guiScalingEndDist);

		}

		if (isWaiting)
		{
			Wait();
			return;
		}

		if (isScaling)
		{
			//print("_guiScalingEndDist = " + _guiScalingEndDist);


			transform.localScale = Vector3.SmoothDamp(transform.localScale, newScale, ref currentScaleVelocity, transitionSpeed);

			if (transform.localScale.x - newScale.x <= 0.05f && 
				transform.localScale.x - newScale.x >= -0.05f)
			{
				spriteRenderer.color = startColor;
				GUI_Manager.guiScaling.gameObject.SetActive(false);
				if (transform.localScale.x - newScale.x <= 0.01f && 
					transform.localScale.x - newScale.x >= -0.01f)
				{
					currentLocalScale = transform.localScale;
					isScaling = false;
				}
			}
		}
	}

	void Wait()
	{
		if (waitTimer >= waitTimeLimit)
		{
			isWaiting = false;
			return;
		}

		waitTimer += Time.deltaTime;
	}

	public void ChangeScaleTo(float _newScaleX, float _transitionSpeed, bool _platformStop)
	{
		GUI_Manager.guiScaling.gameObject.SetActive(true);
		GUI_Manager.guiScaling.StartSettings();

		newScale = new Vector3(_newScaleX, transform.localScale.y, transform.localScale.z);
		transitionSpeed = _transitionSpeed;
		isScaling = true;

		isWaiting = true;
		waitTimer = 0;
		spriteRenderer.color = scalingColor;
		//GetComponent<Platform>().stop = _platformStop;
	}
		
}
