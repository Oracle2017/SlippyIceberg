using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour {
	[HideInInspector] public static int currentLvl;
	[SerializeField] Text currentLvlText;
	[HideInInspector] public static int amountOfCoins;
	[SerializeField] Text amountOfCoinsText;

	[HideInInspector] public static double currentTime;
	//[SerializeField] Text currentTimeText;

	// Use this for initialization
	void Start () {


		Reset();
	}


	public void UpdateScores()
	{
		currentLvlText.text = "STAGE " + currentLvl.ToString();
		amountOfCoinsText.text = amountOfCoins.ToString();

		currentTime += Time.deltaTime;
		//currentTimeText.text = ((int) currentTime).ToString();
	}

	public void Reset()
	{
		currentLvl = 0;
		amountOfCoins = 0;

		currentTime = 0f;
	}
		
}
