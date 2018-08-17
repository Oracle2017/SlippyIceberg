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
	[SerializeField] GameObject coinPrefab;


	[SerializeField] Transform[] levelCoins;
	[HideInInspector] public static int amountOfLevelCoins;
	[HideInInspector] public static int levelCoinIndex; // amount of coins catched in a level
	int currentLevel;

	[SerializeField] Level[] levels;

	int amountOfLevelsPassed;

	[HideInInspector] public static GameObject GUICanvas;
	 

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

		GUICanvas = Instantiate(new GameObject("GUI Canvas"), Vector3.zero, Quaternion.identity);
		Canvas _canvas = GUICanvas.AddComponent<Canvas>();
		_canvas.renderMode = RenderMode.ScreenSpaceCamera;
		_canvas.worldCamera = Camera.main;

		for (int i = 0; i < levels.Length; i++)
		{
			levels[i].coinPrefab = coinPrefab;
		}
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
			amountOfLevelsPassed++;
			currentLevel = (currentLevel + 1) % levels.Length;
			print("currentLevel = " + currentLevel);
			levelCoinIndex = 0;

			if (amountOfLevelsPassed > 1 && 
				amountOfLevelsPassed % levels.Length == 1)
			{
				float temp_rotationSpeed = GameManager.currentPlatform.rotationSpeed * GameManager.currentPlatform.SpeedIncrease; 
				GameManager.currentPlatform.rotationSpeed = (temp_rotationSpeed < GameManager.currentPlatform.rotationSpeedLimit)? 
					temp_rotationSpeed: 
					GameManager.currentPlatform.rotationSpeed;
				
				float temp_moveSpeed = GameManager.currentPlatform.moveSpeed * GameManager.currentPlatform.moveSpeedIncrease; 
				GameManager.currentPlatform.moveSpeed = (temp_moveSpeed < GameManager.currentPlatform.moveSpeedLimit)? 
					temp_moveSpeed: 
					GameManager.currentPlatform.moveSpeed;
			}
		}

		/*if (levels[currentLevel].isLevelFinished || currentLevel == -1)
		{
			currentLevel = (currentLevel + 1) % levels.Length;
			print("currentLevel = " + currentLevel);
			levelCoinIndex = 0;
		}*/

		levels[currentLevel].StartSettings();
	}

	public void RestartGame()
	{
		amountOfLevelsPassed = 0;
		currentPlayer.Reset();
		currentPlatform.Reset();
		gameOver.CloseWindow();
		score.Reset();
		levels[currentLevel].Reset();
		currentLevel = -1;
		amountOfLevelCoins = 0;
		GameManager.currentPlatform.moveSpeed = GameManager.currentPlatform.startMoveSpeed;
		GameManager.currentPlatform.rotationSpeed = GameManager.currentPlatform.startRotationSpeed;
		print("reset");
	}
}
