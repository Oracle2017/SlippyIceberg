using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
	[SerializeField] public Transform levelCoins;
	[HideInInspector] public GameObject coinPrefab;

	protected Transform coin; 
	protected GameObject currentCoin;
	protected int previousAmountOfLevelCoins;
	protected int index = 1;

	protected float waitTimer;

	public virtual void StartSettings () {
		
	}
	
	// Update is called once per frame
	public virtual void UpdateSettings () {
		if (IsWaiting(GameManager.waitTime))
		{
			return;
		}

		CheckAllCoinsCollected();
	}

	void CheckAllCoinsCollected()
	{
		if (GameManager.amountOfLevelCoins != previousAmountOfLevelCoins)
		{
			print("instantiate new coin!");
			coin = levelCoins.GetChild(index);
			//currentCoin = Instantiate(coin.gameObject, coin.position, coin.rotation);
			currentCoin = Instantiate(coinPrefab, coin.position, Quaternion.identity);
			index = (index + 1) % levelCoins.childCount;


		}

		previousAmountOfLevelCoins = GameManager.amountOfLevelCoins;
	}

	protected bool IsWaiting(float _waitTimeLimit)
	{
		if (waitTimer < _waitTimeLimit)
		{
			waitTimer += Time.deltaTime;
			//GameManager.currentPlatform.stop = true;
			return true;
		}

		else 
		{
			print("not waiting");
			return false;
		}
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
			
		waitTimer = 0;

		GameManager.currentPlatform.shouldStabilize = true;
	}
}
