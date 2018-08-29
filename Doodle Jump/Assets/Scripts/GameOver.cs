using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	[SerializeField] Text timeScoreText;
	//[SerializeField] Text timeHighscoreText;
	[SerializeField] Text coinsScoreText;
	//[SerializeField] Text coinsHighscoreText;
	[SerializeField] Text stageScoreText;
	[SerializeField] Text stageHighscoreText;
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

		/*int currentTime =  (int) Score.currentTime;
		timeScoreText.text = currentTime.ToString();

		int currentLvl = Score.currentLvl;
		int amountOfCoins = Score.amountOfCoins;

		// Save data
		if (PlayerPrefs.GetInt("Highscore") < currentTime)
		{
			PlayerPrefs.SetInt("Highscore", currentTime);
		}

		timeHighscoreText.text = "HIGHSCORE\t\t" + PlayerPrefs.GetInt("Highscore");*/

		SetScore(Score.currentLvl, "stageHighscore", stageScoreText, stageHighscoreText);
		coinsScoreText.text = Score.amountOfCoins.ToString(); //SetScore(Score.amountOfCoins, "coinsHighscore", coinsScoreText, coinsHighscoreText);
		timeScoreText.text = ((int )Score.currentTime).ToString() + " s"; //SetScore((int) Score.currentTime, "timeHighscore", timeScoreText, timeHighscoreText);

	}

	void SetScore(int _score, string _highscoreName, Text _scoreText, Text _highscoreText, string _highscorePrefix = "")
	{
		if (PlayerPrefs.GetInt(_highscoreName) < _score)
		{
			PlayerPrefs.SetInt(_highscoreName, _score);
		}

		_scoreText.text = _score.ToString();
		_highscoreText.text = _highscorePrefix + PlayerPrefs.GetInt(_highscoreName);
	}

	public void CloseWindow()
	{
		image.enabled = false;
		rectTransform.localScale = Vector3.zero;
	}
}
