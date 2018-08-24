using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : Level {

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
		/*if (currentCoins.Count > 0)
		{
			for (int i = 0; i < currentCoins.Count; i++)
			{
				if (currentCoins[i] != null)
					Destroy(currentCoins[i]);
			}

			currentCoins.Clear();
		}*/
		shouldStabilize = true;
		shouldCenterPosition = true;
		shouldScale = true;

	}
}
