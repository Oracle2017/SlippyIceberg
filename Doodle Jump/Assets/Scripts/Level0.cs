﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : Level {
	List<GameObject> currentCoins = new List<GameObject>();

	// Use this for initialization
	public override void StartSettings () {
		Reset();
		LevelStart();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LevelStart()
	{
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(GameManager.currentPlatform.obstacleStartScale.x, 1.5f, true);
		print("start scale = " + GameManager.currentPlatform.obstacleStartScale.x);

		//print("level coins = " + levelCoins);
		GameManager.currentPlatform.stop = true;
		GameManager.amountOfLevelCoins = levelCoins.childCount;

		for (int i = 0; i < levelCoins.childCount; i++)
		{
			Transform coin = levelCoins.GetChild(i);
			GameObject currentCoin = Instantiate(coin.gameObject, coin.position, coin.rotation);
			currentCoins.Add(currentCoin);
		}
	}

	void Reset()
	{
		if (currentCoins.Count > 0)
		{
			for (int i = 0; i < currentCoins.Count; i++)
			{
				if (currentCoins[i] != null)
					Destroy(currentCoins[i]);
			}

			currentCoins.Clear();
		}
	}
}