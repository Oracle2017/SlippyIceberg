using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : Level {
	List<GameObject> currentCoins = new List<GameObject>();
	bool isGrowing;
	bool isPlatformCentered;
	Vector3 currentMoveVelocity;
	[SerializeField] float scaleXTarget = 1.6f;

	// Use this for initialization
	public override void StartSettings () {
		Reset();
	}
	
	// Update is called once per frame
	public override void UpdateSettings () {

		if (!(
			GameManager.currentPlatform.currentRotation >= -0.1f &&
			GameManager.currentPlatform.currentRotation <= 0.1f))
		{
			return;
		}

		CenterPlatform();

		if (!isGrowing &&
			GameManager.currentPlatform.transform.position.x >= GameManager.currentPlatform.startPos.x - 0.1f &&
			GameManager.currentPlatform.transform.position.x <= GameManager.currentPlatform.startPos.x + 0.1f) 
		{
			LevelGrow();
			isGrowing = true;
		}

		base.UpdateSettings();
	}
		

	void CenterPlatform()
	{
		GameManager.currentPlatform.transform.position = Vector3.SmoothDamp(
			GameManager.currentPlatform.transform.position,
			GameManager.currentPlatform.startPos,
			ref currentMoveVelocity, 
			1f);
	}

	void LevelGrow()
	{
		print("Level Grow!");
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(scaleXTarget, 1.5f, false);
		//print("start scale = " + GameManager.currentPlatform.obstacleStartScale.x);

		/*for (int i = 0; i < levelCoins.childCount; i++)
		{
			Transform coin = levelCoins.GetChild(i);
			GameObject currentCoin = Instantiate(coin.gameObject, coin.position, coin.rotation);
			currentCoins.Add(currentCoin);
		}*/
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

		isGrowing = false;
		currentMoveVelocity = Vector3.zero;
	}
}
