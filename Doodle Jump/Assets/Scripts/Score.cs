﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour {
	[HideInInspector] public static int currentLvl;
	[SerializeField] Text currentLvlText;
	[HideInInspector] public static int totalAmountOfCoins;
	[HideInInspector] public static int currentAmountOfCoins;
	[SerializeField] Text amountOfCoinsText;

	[HideInInspector] public static double currentTime;
	//[SerializeField] Text currentTimeText;

	// Use this for initialization
	public void StartSettings () {
		Reset();
	}


	public void UpdateScores()
	{
		currentLvlText.text = "STAGE " + currentLvl.ToString();
		amountOfCoinsText.text = (currentAmountOfCoins + totalAmountOfCoins).ToString();

		currentTime += Time.deltaTime;
		//currentTimeText.text = ((int) currentTime).ToString();
	}

	public void Reset()
	{
		currentLvl = 0;
		totalAmountOfCoins = PlayerPrefs.GetInt("amountOfDiamonds"); 
		currentAmountOfCoins = 0;
		print( PlayerPrefs.GetInt("amountOfDiamonds"));

		currentTime = 0f;
	}
		
}
