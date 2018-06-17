using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour {
	[HideInInspector] public static double currentScore;
	int highScore;
	Text uiCurrentScoreText;

	// Use this for initialization
	void Start () {
		uiCurrentScoreText = GetComponent<Text>();
		Reset();
	}


	public void UpdateScore()
	{
		currentScore += Time.deltaTime;

		uiCurrentScoreText.text = ((int) currentScore).ToString();
	}

	public void Reset()
	{
		currentScore = 0f;
	}
		
}
