using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
	[SerializeField] public Transform levelCoins;

	protected Transform coin; 
	protected GameObject currentCoin;
	protected int previousAmountOfLevelCoins;
	protected int index = 1;
	[HideInInspector] public bool isLevelFinished;

	public virtual void StartSettings () {
		
	}
	
	// Update is called once per frame
	public virtual void UpdateSettings () {
		CheckAllCoinsCollected();
	}

	void CheckAllCoinsCollected()
	{
		if (GameManager.amountOfLevelCoins != previousAmountOfLevelCoins)
		{
			print("instantiate new coin!");
			coin = levelCoins.GetChild(index);
			currentCoin = Instantiate(coin.gameObject, coin.position, coin.rotation);
			index = (index + 1) % levelCoins.childCount;
		}

		previousAmountOfLevelCoins = GameManager.amountOfLevelCoins;
	}

	public virtual void Reset()
	{
		index = 1;

		GameManager.amountOfLevelCoins = levelCoins.childCount;
		previousAmountOfLevelCoins = -1;

		if (currentCoin != null)
		{
			Destroy(currentCoin);
		}

		isLevelFinished = false;
	}
}
