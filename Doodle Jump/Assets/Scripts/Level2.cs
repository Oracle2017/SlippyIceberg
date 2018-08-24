using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : Level {
	// Platform vars
	float platformMoveTimer;
	float currentVelocity;

	// Use this for initialization
	public override void StartSettings () {
		Reset();
	}

	public override void UpdateSettings () {
		base.UpdateSettings();

		if (isPrepared)
		{
			MoveLeftAndRight(2.45f);
		}
	}

	void MoveLeftAndRight(float _dist)
	{
		float sample = Mathf.Sin(Mathf.Deg2Rad * platformMoveTimer);
		platformMoveTimer+= Time.deltaTime * GameManager.currentPlatform.moveSpeed;
		sample = Mathf.Clamp(sample, -1, 1);
		float moveTarget = Utils.Map(sample, -1, 1, -_dist, _dist);

		float platformPosX = Mathf.SmoothDampAngle(GameManager.currentPlatform.transform.position.x, moveTarget, ref currentVelocity, 0.2f);
		GameManager.currentPlatform.transform.position = new Vector3(platformPosX, GameManager.currentPlatform.transform.position.y);
	}

	public override void Reset()
	{
		base.Reset();
		shouldStabilize = true;
		shouldCenterPosition = false;
		shouldScale = true;


		platformMoveTimer = 0;
		currentVelocity = 0;
	}
}
