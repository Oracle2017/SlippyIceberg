using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : Level {
	// Platform vars
	float platformMoveTimer;
	[SerializeField] float scaleXTarget = 0.25f;
	float currentVelocity;

	// Use this for initialization
	public override void StartSettings () {
		Reset();
	}

	public override void UpdateSettings () {
		
		// Wait til stabilizing is done
		if (!(
			GameManager.currentPlatform.currentRotation >= -0.1f &&
			GameManager.currentPlatform.currentRotation <= 0.1f))
		{
			return;
		}

		ShrinkPlatform();

		// Wait til scaling is done
		if (!(GameManager.currentPlatform.transform.localScale.x >= scaleXTarget - 0.1f &&
			GameManager.currentPlatform.transform.localScale.x <= scaleXTarget + 0.1f))
		{
			return;
		}
			
		base.UpdateSettings();

		if (!IsWaiting(GameManager.waitTime))
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

	void ShrinkPlatform()
	{
		// Shrink and stop
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(scaleXTarget, 1.5f, true);
	}

	public override void Reset()
	{
		base.Reset();

		platformMoveTimer = 0;
		currentVelocity = 0;
	}
}
