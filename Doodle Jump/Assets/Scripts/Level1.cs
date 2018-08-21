using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : Level {
	[SerializeField] float scaleXTarget = 0.6f;

	// Use this for initialization
	public override void StartSettings () {
		Reset();
		LevelShrink();
	}

	public override void UpdateSettings () {
		if (GameManager.currentPlatform.transform.localScale.x >= scaleXTarget - 0.1f &&
			GameManager.currentPlatform.transform.localScale.x <= scaleXTarget + 0.1f)
		{
			base.UpdateSettings();

			if (!IsWaiting(GameManager.waitTime))
			{
				GameManager.currentPlatform.shouldStabilize = false;
			}
		}
	}
	
	void LevelShrink()
	{
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(scaleXTarget, 1.5f, false);
		//print("can swing ? " + !GameManager.currentPlatform.shouldStabilize);
	}

	public override void Reset()
	{
		base.Reset();

		// Should swing and not stabilize anymore, TODO: find better way to do this
		GameManager.currentPlatform.swingTimer = 0f;
		GameManager.currentPlatform.currentRotationVelocity = 0f;
		//GameManager.currentPlatform.platformShouldWait = true;
	}
		
}
