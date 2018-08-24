using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : Level {

	// Use this for initialization
	public override void StartSettings () {
		Reset();
	}

	public override void UpdateSettings () {
		base.UpdateSettings();
	}

	public override void Reset()
	{
		base.Reset();
		shouldStabilize = false;
		shouldCenterPosition = true;
		shouldScale = true;

	}
		
}
