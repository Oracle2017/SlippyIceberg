using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cosmetic : MonoBehaviour {
	[SerializeField] Image currentImage;
	[SerializeField] Sprite unlockedSprite;
	[SerializeField] string description;
	[SerializeField] GameObject diamondImage;
	bool isVisible;
	bool isUnlocked;

	[Header("Conditions")]
	[SerializeField] int amountOfDiamonsNeeded;
	[SerializeField] Text amountOfDiamondsNeededText;


	public void OnTap()
	{
		string _description = description;

		if (isVisible && Score.totalAmountOfCoins >= amountOfDiamonsNeeded)
		{
			if (!isUnlocked)
			{
				PlayerPrefs.SetInt("unlockedCosmetics_"+name, 1);
				diamondImage.SetActive(false);
				amountOfDiamondsNeededText.text = "";

				Score.totalAmountOfCoins -= amountOfDiamonsNeeded;
				PlayerPrefs.SetInt("amountOfDiamonds", Score.totalAmountOfCoins);
				Cosmetic_Previewer.singleton.amountOfCoins.text = Score.totalAmountOfCoins.ToString();
				//GameManager.score.

				isUnlocked = true;
			}

			GameManager.currentPlayer.spriteRenderer.sprite = unlockedSprite;

			Player_Mirror playerMirror = GameManager.currentPlayer.GetComponent<Player_Mirror>();

			if (playerMirror != null)
			{
				playerMirror.reflectionSprite.sprite = unlockedSprite;
			}
				
			print("unlocked");
		}

		else if (!isUnlocked && Score.totalAmountOfCoins < amountOfDiamonsNeeded)
		{
			_description = "You don't have enough diamonds. You need " + (amountOfDiamonsNeeded - Score.totalAmountOfCoins).ToString() + " more.";
		}

		Cosmetic_Previewer.singleton.ChangePreview(currentImage.sprite, _description);
	}

	public void Reset()
	{
		amountOfDiamondsNeededText.text = amountOfDiamonsNeeded.ToString();

		if (Score.totalAmountOfCoins >= amountOfDiamonsNeeded || PlayerPrefs.GetInt("visibleCosmetics_"+name) == 1)
		{
			PlayerPrefs.SetInt("visibleCosmetics_"+name, 1);

			currentImage.sprite = unlockedSprite;
			isVisible = true;

			if (PlayerPrefs.GetInt("unlockedCosmetics_"+name) == 1)
			{
				isUnlocked = true;
				amountOfDiamondsNeededText.text = "";
				diamondImage.SetActive(false);
			}

		}
	}

}
