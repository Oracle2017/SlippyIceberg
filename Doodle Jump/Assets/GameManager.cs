using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] Player playerPrefab; // Just used as prefab
	[SerializeField] Platform platformPrefab;
	public static Player currentPlayer;
	public static Platform currentPlatform;
	[SerializeField] Score score;
	[SerializeField] GameOver gameOver;


	[SerializeField] Transform[] levelCoins;
	[HideInInspector] public static int amountOfLevelCoins;
	[HideInInspector] public static int levelCoinIndex; // amount of coins catched in a level
	int currentLevel;

	[SerializeField] Level[] levels;
	 

	// Use this for initialization
	void Start () {

		// TODO: remove this in the end, just for debugging
		currentPlatform = GameObject.FindObjectOfType<Platform>() as Platform;
		currentPlayer = GameObject.FindObjectOfType<Player>() as Player;

		if (currentPlatform == null)
		{
			currentPlatform = Instantiate(platformPrefab.gameObject, platformPrefab.transform.position, platformPrefab.transform.rotation).GetComponent<Platform>();
		}


		if (currentPlayer == null)
		{
			currentPlayer = Instantiate(playerPrefab.gameObject, playerPrefab.transform.position, playerPrefab.transform.rotation).GetComponent<Player>();
		}
			

		currentPlatform.StartSettings();
		currentPlayer.StartSettings();

		currentLevel = -1;

	}
	
	// Update is called once per frame
	void Update () {
		
		if (!currentPlayer.isDead)
		{
			LevelSwitcher();
			levels[currentLevel].UpdateSettings();
			currentPlatform.UpdatePlatform();
			currentPlayer.UpdatePlayer();
			score.UpdateScore();
		}

		else 
		{
			gameOver.DisplayWindow();
		}
	}

	void LevelSwitcher()
	{
		//print("amount of coins = " + amountOfLevelCoins);

		if (amountOfLevelCoins > 0)
		{
			return;
		}
			

		if (amountOfLevelCoins <= 0)
		{
			currentLevel = (currentLevel + 1) % levels.Length;
			print("currentLevel = " + currentLevel);
			levelCoinIndex = 0;
		}

		switch (currentLevel)
		{
			case 0:
				levels[0].StartSettings();
				break;
			case 1:
				levels[1].StartSettings();
				break;
		}
	}

	public void RestartGame()
	{
		currentPlayer.Reset();
		currentPlatform.Reset();
		gameOver.CloseWindow();
		score.Reset();
		currentLevel = -1;
		amountOfLevelCoins = 0;
		print("reset");
	}
}
