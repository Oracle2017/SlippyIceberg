using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : Level {
	[SerializeField] float smallScaleXtarget;
	float bigScaleXtarget;
	float currentScaleXTarget;

	// Use this for initialization
	public override void StartSettings () {
		Reset();
	}

	public override void UpdateSettings () {
		base.UpdateSettings();

		if (!isPrepared)
			return;

		if (GameManager.currentPlatform.transform.localScale.x - currentScaleXTarget <= 0.05f && 
			GameManager.currentPlatform.transform.localScale.x - currentScaleXTarget >= -0.05f)
		{
			currentScaleXTarget = (currentScaleXTarget == smallScaleXtarget)? bigScaleXtarget: smallScaleXtarget;
			ScalePlatform(currentScaleXTarget);
		}
	}

	public override void Reset()
	{
		base.Reset();
		shouldStabilize = true;
		shouldCenterPosition = true;
		shouldScale = true;
		bigScaleXtarget = scaleXTarget;
		currentScaleXTarget = smallScaleXtarget;
	}
}
