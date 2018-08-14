using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : Level {

	// Use this for initialization
	public override void StartSettings () {
		Reset();
		LevelShrink();
	}

	public override void UpdateSettings () {
		base.UpdateSettings();
	}
	
	void LevelShrink()
	{
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(0.6f, 1.5f, false);
		//print("can swing ? " + !GameManager.currentPlatform.shouldStabilize);

	}

	public override void Reset()
	{
		base.Reset();

		// Should swing and not stabilize anymore, TODO: find better way to do this
		GameManager.currentPlatform.swingTimer = 0f;
		GameManager.currentPlatform.currentVelocity = 0f;
		GameManager.currentPlatform.platformShouldWait = true;


	}
		
}
