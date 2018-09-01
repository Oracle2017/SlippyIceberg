﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[Header("Prefabs")]
	[SerializeField] Player playerPrefab; // Just used as prefab
	[SerializeField] Platform platformPrefab;
	public static Player currentPlayer;
	public static Platform currentPlatform;
	[SerializeField] Score score;
	[SerializeField] GameOver gameOver;
	[SerializeField] GameObject coinPrefab;
	[SerializeField] CosmeticsTab cosmeticsTab;

	[SerializeField] Transform[] levelCoins;
	[SerializeField] Level[] levels;

	[Space(10)]
	[Header("Stage Transition Settings")]
	[SerializeField] float startWaitTime = 3; 
	[SerializeField] float waitTimeDecreaseSpeed = 0.6f;
	[SerializeField] float waitTimeLimit = 0.5f;
	[HideInInspector] public static float waitTime;

	[HideInInspector] public static int amountOfLevelCoins;
	[HideInInspector] public static int levelCoinIndex; // amount of coins catched in a level
	int currentLevel;
	int amountOfLevelsPassed;

	[HideInInspector] public static GameObject GUICanvas;

	[HideInInspector] public static float screenHeight;
	 

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
		score.StartSettings();
		gameOver.StartSettings();
		cosmeticsTab.StartSettings();


		// GUI Canvas instantiation
		GUICanvas = Instantiate(new GameObject("GUI Canvas"), Vector3.zero, Quaternion.identity);
		Canvas _canvas = GUICanvas.AddComponent<Canvas>();
		_canvas.renderMode = RenderMode.ScreenSpaceCamera;
		_canvas.worldCamera = Camera.main;


		// Every level gets assigned the coin prefab
		for (int i = 0; i < levels.Length; i++)
		{
			levels[i].coinPrefab = coinPrefab;
		}

		// Settings
		waitTime = startWaitTime;
		currentLevel = -1;
		screenHeight = Camera.main.orthographicSize * 2.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Space))
		{
			PlayerPrefs.DeleteAll();
		}
		
		if (!currentPlayer.isDead)
		{
			LevelSwitcher();
			levels[currentLevel].UpdateSettings();

			currentPlatform.UpdatePlatform();
			currentPlayer.UpdatePlayer();
			score.UpdateScores();
		}

		else 
		{
			currentPlayer.gameObject.SetActive(false);
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
			Score.currentLvl = amountOfLevelsPassed;
			currentLevel = (currentLevel + 1) % levels.Length;
			levelCoinIndex = 0;

			if (amountOfLevelsPassed > 1 && 
				amountOfLevelsPassed % levels.Length == 1)
			{
				// Decrease rotation speed
				float temp_rotationSpeed = GameManager.currentPlatform.rotationSpeed * GameManager.currentPlatform.SpeedIncrease; 
				GameManager.currentPlatform.rotationSpeed = (temp_rotationSpeed < GameManager.currentPlatform.rotationSpeedLimit)? 
					temp_rotationSpeed: 
					GameManager.currentPlatform.rotationSpeed;

				// Decrease move speed
				float temp_moveSpeed = GameManager.currentPlatform.moveSpeed * GameManager.currentPlatform.moveSpeedIncrease; 
				GameManager.currentPlatform.moveSpeed = (temp_moveSpeed < GameManager.currentPlatform.moveSpeedLimit)? 
					temp_moveSpeed: 
					GameManager.currentPlatform.moveSpeed;

				// Decrease move speed
				float temp_scaleSpeed = GameManager.currentPlatform.scaleSpeed * (1/GameManager.currentPlatform.scaleSpeedIncrease); 
				GameManager.currentPlatform.scaleSpeed = (temp_scaleSpeed > GameManager.currentPlatform.scaleSpeedLimit)? 
					temp_scaleSpeed: 
					GameManager.currentPlatform.scaleSpeed;

				// Decrease waiting transitions
				float temp_waitLimit = waitTime * waitTimeDecreaseSpeed; 
				waitTime = (temp_waitLimit > waitTimeLimit)? 
					temp_waitLimit: 
					waitTime;
			}
		}

		levels[currentLevel].StartSettings();
	}

	public void RestartGame()
	{
		amountOfLevelsPassed = 0;

		currentPlayer.gameObject.SetActive(true);
		currentPlayer.Reset();
		currentPlatform.Reset();
		gameOver.CloseWindow();
		score.Reset();
		levels[currentLevel].Reset();
		currentLevel = -1;
		amountOfLevelCoins = 0;
		GameManager.currentPlatform.moveSpeed = GameManager.currentPlatform.startMoveSpeed;
		GameManager.currentPlatform.rotationSpeed = GameManager.currentPlatform.startRotationSpeed;
		waitTime = startWaitTime;


		print("reset");
	}
}
