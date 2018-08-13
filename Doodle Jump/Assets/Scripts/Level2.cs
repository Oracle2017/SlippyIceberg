using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : Level {
	float platformMoveSpeed = 10;
	float platformMoveTimer;
	float platformScaleTarget = 0.25f;
	float currentVelocity;
	bool shouldMoveLeftAndRight;
	bool isPlatformScaling;

	// Use this for initialization
	public override void StartSettings () {
		Reset();
		StabilizePlatform();
	}

	public override void UpdateSettings () {
		if (!isPlatformScaling &&
			GameManager.currentPlatform.currentRotation >= -0.1f &&
			GameManager.currentPlatform.currentRotation <= 0.1f)
		{
			ShrinkPlatform();

			shouldMoveLeftAndRight = true;

			if (GameManager.currentPlatform.transform.localScale.x >= platformScaleTarget - 0.01f &&
				GameManager.currentPlatform.transform.localScale.x <= platformScaleTarget + 0.01f)
			{
				shouldMoveLeftAndRight = true;
				isPlatformScaling = true;
			}
		}

		else if (shouldMoveLeftAndRight)
		{
			//GameManager.currentPlatform.platformShouldWait = true;
			MoveLeftAndRight(2.45f);
		}
	}

	void MoveLeftAndRight(float _dist)
	{
		float sample = Mathf.Sin(Mathf.Deg2Rad * platformMoveTimer);
		platformMoveTimer+= Time.deltaTime * platformMoveSpeed;
		sample = Mathf.Clamp(sample, -1, 1);
		float moveTarget = Utils.Map(sample, -1, 1, -_dist, _dist);

		float platformPosX = Mathf.SmoothDampAngle(GameManager.currentPlatform.transform.position.x, moveTarget, ref currentVelocity, 0.2f);
		GameManager.currentPlatform.transform.position = new Vector3(platformPosX, GameManager.currentPlatform.transform.position.y);
	}

	void StabilizePlatform()
	{
		GameManager.currentPlatform.shouldStabilize = true;
	}

	void ShrinkPlatform()
	{
		// Shrink and stop
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(platformScaleTarget, 1.5f, true);
		GameManager.currentPlatform.stop = true;
	}

	void Reset()
	{
		GameManager.amountOfLevelCoins = 9999;
		shouldMoveLeftAndRight = false;
		isPlatformScaling = false;
		platformMoveTimer = 0;
		currentVelocity = 0;
		GameManager.currentPlatform.shouldStabilize = false;
	}
}
