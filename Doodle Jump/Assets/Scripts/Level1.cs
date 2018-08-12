using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : Level {
	Transform coin; 
	GameObject currentCoin;
	int previousAmountOfLevelCoins;
	int index = 1;

	// Use this for initialization
	public override void StartSettings () {
		Reset();
		LevelShrink();
	}

	public override void UpdateSettings () {
		if (GameManager.amountOfLevelCoins != previousAmountOfLevelCoins)
		{
			print("instantiate new coin!");
			coin = levelCoins.GetChild(index);
			currentCoin = Instantiate(coin.gameObject, coin.position, coin.rotation);
			index = (index + 1) % 2;
		}

		previousAmountOfLevelCoins = GameManager.amountOfLevelCoins;
	}
	
	void LevelShrink()
	{
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(0.6f, 1.5f, false);
		GameManager.amountOfLevelCoins = 2;
		previousAmountOfLevelCoins = -1;
		/*for (int i = 0; i < levelCoins.childCount; i++)
		{
			Transform coin = levelCoins.GetChild(i);
			Instantiate(coin.gameObject, coin.position, coin.rotation);
			print("coin instantiated");
		}*/
	}

	void Reset()
	{
		index = 1;

		if (currentCoin != null)
		{
			Destroy(currentCoin);
		}
	}
		
}
