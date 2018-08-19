using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	public Text ScoreText;
	public Text HighscoreText;
	Vector3 targetScale;
	Vector3 scalingVelocity;
	RectTransform rectTransform;
	Image image;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();

		image = GetComponent<Image>();


		targetScale = rectTransform.localScale;

		CloseWindow();
	}


	public void DisplayWindow()
	{
		image.enabled = true;
		rectTransform.localScale = Vector3.SmoothDamp(rectTransform.localScale, targetScale, ref scalingVelocity, 0.2f);

		int currentTime =  (int) Score.currentTime;
		ScoreText.text = currentTime.ToString();
		int currentLvl = Score.currentLvl;
		int amountOfCoins = Score.amountOfCoins;

		// Save data
		if (PlayerPrefs.GetInt("Highscore") < currentTime)
		{
			PlayerPrefs.SetInt("Highscore", currentTime);
		}

		HighscoreText.text = "HIGHSCORE\t\t" + PlayerPrefs.GetInt("Highscore");
	}

	public void CloseWindow()
	{
		image.enabled = false;
		rectTransform.localScale = Vector3.zero;
	}
}
