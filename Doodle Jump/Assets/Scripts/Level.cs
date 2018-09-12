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

	protected bool shouldStabilize;
	protected bool shouldCenterPosition;
	protected bool shouldScale;
	protected bool isPrepared;
	protected Vector3 currentMoveVelocity;
	[SerializeField] protected float scaleXTarget;

	// TODO reset all this!

	public virtual void StartSettings () {
		
	}
	
	// Update is called once per frame
	public virtual void UpdateSettings () {
		if (!isPrepared)
		{
			Settings();
			return;
		}

		CheckAllCoinsCollected();
	}

	void Settings()
	{
		if (shouldStabilize && !(
			GameManager.currentPlatform.currentRotation >= -0.1f &&
			GameManager.currentPlatform.currentRotation <= 0.1f))
		{
			return;
		}

		if (shouldCenterPosition && !(
			GameManager.currentPlatform.transform.position.x >= GameManager.currentPlatform.startPos.x - 0.05f &&
			GameManager.currentPlatform.transform.position.x <= GameManager.currentPlatform.startPos.x + 0.05f)) 
		{
			CenterPlatform();
			return;
		}

		if (shouldScale)
		{
			ScalePlatform(scaleXTarget);
			shouldScale = false;
		}
			

		if (GameManager.currentPlatform.transform.localScale.x - scaleXTarget <= 0.05f && 
			GameManager.currentPlatform.transform.localScale.x - scaleXTarget >= -0.05f)
		{
			if (IsWaiting(GameManager.waitTime))
			{
				return;
			}

			// Start settings
			if (!shouldStabilize)
			{
				GameManager.currentPlatform.shouldStabilize = false;
				GameManager.currentPlatform.swingTimer = 0f;
				GameManager.currentPlatform.currentRotationVelocity = 0f;
			}

			isPrepared = true;
		}

	}

	void CenterPlatform()
	{
		GameManager.currentPlatform.transform.position = Vector3.SmoothDamp(
			GameManager.currentPlatform.transform.position,
			GameManager.currentPlatform.startPos,
			ref currentMoveVelocity, 
			0.65f);

		// TODO: this is only centering on the x axis

		/*if (Mathf.Abs(Mathf.Abs(GameManager.currentPlatform.transform.position.x) - Mathf.Abs(GameManager.currentPlatform.startPos.x)) <= (GameManager.currentPlatform.moveSpeed * Time.deltaTime))
		{
			print("dist from center = " + (Mathf.Abs(GameManager.currentPlatform.transform.position.x) - Mathf.Abs(GameManager.currentPlatform.startPos.x)));
			print("dist per frame = " + (GameManager.currentPlatform.moveSpeed * Time.deltaTime));
			Debug.Break();
			GameManager.currentPlatform.transform.position = GameManager.currentPlatform.startPos;
		}

		else if (GameManager.currentPlatform.transform.position.x < 0)
		{
			GameManager.currentPlatform.transform.position += new Vector3(GameManager.currentPlatform.moveSpeed * Time.deltaTime, 0);
		}

		else 
		{
			GameManager.currentPlatform.transform.position -= new Vector3(GameManager.currentPlatform.moveSpeed * Time.deltaTime, 0);
		}*/
	}

	protected void ScalePlatform(float _xTarget)
	{
		Platform_ScaleChanger _platformScaleChanger = GameManager.currentPlatform.GetComponent<Platform_ScaleChanger>();
		_platformScaleChanger.ChangeScaleTo(_xTarget, GameManager.currentPlatform.scaleSpeed, false);
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
			return false;
		}
	}

	public virtual void Reset()
	{
		// Coin stuff
		index = 0;
		GameManager.amountOfLevelCoins = 5;//levelCoins.childCount;
		previousAmountOfLevelCoins = -1;

		if (currentCoin != null)
		{
			Destroy(currentCoin);
		}
			
		// Other
		waitTimer = 0;
		currentMoveVelocity = Vector3.zero;

		GameManager.currentPlatform.shouldStabilize = true;

		isPrepared = false;

	}
}
