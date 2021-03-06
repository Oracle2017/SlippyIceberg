﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
	[SerializeField] GUI_ControlsHelper guiControlsHelper;
	[SerializeField] GUI_LevelStatus guiLevelStatus;
	[SerializeField] LevelTransitor levelTransitor;
	//[SerializeField] GameObject guiHelperPrefab;

	[SerializeField] Transform[] levelCoins;
	[SerializeField] Level[] levels;
	[HideInInspector] public static int levelsLength;

	[Space(10)]
	[Header("Stage Transition Settings")]
	[SerializeField] float startWaitTime = 3; 
	[SerializeField] float waitTimeDecreaseSpeed = 0.6f;
	[SerializeField] float waitTimeLimit = 0.5f;
	[HideInInspector] public static float waitTime;

	[HideInInspector] public static int amountOfLevelCoins;
	[HideInInspector] public static int levelCoinIndex; // amount of coins catched in a level
	int currentLevel;
	[HideInInspector] public static int amountOfLevelsPassed;

	[HideInInspector] public static GameObject GUICanvas;

	[HideInInspector] public static float screenHeight;

	public static bool isPausing;
	 

	/*[MenuItem("Edit/Reset Playerprefs")] public static void DeletePlayerPrefs() { PlayerPrefs.DeleteAll(); }*/

	// Use this for initialization
	void Start () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			PlayerPrefs.DeleteAll();
		}

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
		guiLevelStatus.StartSettings();
		gameOver.StartSettings();
		cosmeticsTab.StartSettings();
		levelTransitor.StartSettings();
		levelsLength = levels.Length;


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
		/*if (Input.GetMouseButtonDown(0))
		{
			isPausing = !isPausing;
		}*/


		if (isPausing)
		{
			return;
		}
			
		
		if (!currentPlayer.isDead)
		{
			guiLevelStatus.FillDiamond(levelCoinIndex - 1);
			LevelSwitcher();
			levels[currentLevel].UpdateSettings();
			/*if (guiHelper != null)
			{
				guiHelper.UpdateSettings();
			}*/

			if (Score.currentLvl == 1)
			{
				guiControlsHelper.UpdateSettings();
			}

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
			if (amountOfLevelsPassed > 0)
			{
				levelTransitor.StageCompleted();
			}

			//guiLevelStatus.Reset();
			amountOfLevelsPassed++;
			Score.currentLvl = amountOfLevelsPassed;
			currentLevel = (currentLevel + 1) % levels.Length;
			levelCoinIndex = 0;

			/*if (guiHelper == null && Score.currentLvl < 2)
			{
				GameObject _guiHelperGameObject = Instantiate(guiHelperPrefab, guiHelperPrefab.transform.position, Quaternion.identity, GUICanvas.transform);
				guiHelper = _guiHelperGameObject.GetComponent<GUI_Helper>();
				guiHelper.StartSettings();
			}*/

			//print("current level = " + Score.currentLvl);
			if (Score.currentLvl == 1)
			{
				guiControlsHelper.gameObject.SetActive(true);
				guiControlsHelper.StartSettings();
			}

			else 
			{
				guiControlsHelper.gameObject.SetActive(false);
			}


			if (amountOfLevelsPassed > 1 &&
				amountOfLevelsPassed % 4 == 0)
			{
				Score.coinIncrementer *= 2;
				levelTransitor.Diamonds2X();
			}


			if (amountOfLevelsPassed > 1 && 
				amountOfLevelsPassed % levels.Length == 1)
			{
				// Decrease rotation speed
				float temp_rotationSpeed = currentPlatform.rotationSpeed * currentPlatform.SpeedIncrease; 
				currentPlatform.rotationSpeed = (temp_rotationSpeed < currentPlatform.rotationSpeedLimit)? 
					temp_rotationSpeed: 
					currentPlatform.rotationSpeed;

				// Decrease move speed
				float temp_moveSpeed = currentPlatform.moveSpeed * currentPlatform.moveSpeedIncrease; 
				GameManager.currentPlatform.moveSpeed = (temp_moveSpeed < currentPlatform.moveSpeedLimit)? 
					temp_moveSpeed: 
					GameManager.currentPlatform.moveSpeed;

				// Decrease move speed
				float temp_scaleSpeed = currentPlatform.scaleSpeed * (1/currentPlatform.scaleSpeedIncrease); 
				currentPlatform.scaleSpeed = (temp_scaleSpeed > currentPlatform.scaleSpeedLimit)? 
					temp_scaleSpeed: 
					currentPlatform.scaleSpeed;

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
		waitTime = startWaitTime;
		guiLevelStatus.Reset();
		levelTransitor.Reset();
		cosmeticsTab.CheckNewVisible();
		levelCoinIndex = 0;


		print("reset");
	}
}
