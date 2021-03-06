﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticsTab : MonoBehaviour {
	[SerializeField] Transform cosmeticsParent;
	[SerializeField] GameObject exclamationPoint;
	[SerializeField] GUI_Messenger guiMessenger;

	//[SerializeField] Cosmet cosmeticPreviewer;

	// Use this for initialization
	public void StartSettings () {
		// Sets current player sprite
		int _currentCosmeticIndex = PlayerPrefs.GetInt("currentCosmeticChildNr");
		Cosmetic _cosmetic = cosmeticsParent.GetChild(_currentCosmeticIndex).GetComponent<Cosmetic>();
		Sprite _sprite = _cosmetic.unlockedSprite;
		GameManager.currentPlayer.spriteRenderer.sprite = _sprite;
		exclamationPoint.SetActive(false);
		//guiMessenger.gameObject.SetActive(false);
		gameObject.SetActive(false);


		//Cosmetic_Previewer.StartSettings();
	}

	public void DisplayWindow()
	{
		GameManager.isPausing = true;

		Cosmetic_Previewer.singleton.Reset();

		gameObject.SetActive(true);

		for (int i = 0; i < cosmeticsParent.childCount; i++)
		{
			Cosmetic _cosmetic = cosmeticsParent.GetChild(i).GetComponent<Cosmetic>();
			_cosmetic.Reset();
		}
	}

	public void CheckNewVisible()
	{
		// No need to check
		/*if (PlayerPrefs.GetInt("newCosmeticVisible") == 1)
		{
			return;
		}*/

		// Check starting

		//int _firstLockedIndex = -1;

		for (int i = 1; i < cosmeticsParent.childCount; i++)
		{
			Cosmetic _cosmetic = cosmeticsParent.GetChild(i).GetComponent<Cosmetic>();
			_cosmetic.Reset();

			//print ("cosmetic " + i + " is new ? " + _cosmetic.isNew);

			if (_cosmetic.isNew)
			{
				//print(_cosmetic.name + " is new! should open window!");
				int _firstLockedIndex = i;
				Cosmetic _lockedCosmetic = cosmeticsParent.GetChild(_firstLockedIndex).GetComponent<Cosmetic>();

				exclamationPoint.SetActive(true);
				guiMessenger.OpenWindow(i, _lockedCosmetic);

				PlayerPrefs.SetInt("newCosmeticVisible", 1);
				return;
			}

			else if (!_cosmetic.isVisible || i >= cosmeticsParent.childCount - 1)
			{
				return;
			}
		}
			



	}

	public void CloseWindow()
	{
		PlayerPrefs.SetInt("newCosmeticVisible", 0);
		exclamationPoint.SetActive(false);
		GameManager.isPausing = false;
		gameObject.SetActive(false);
	}
}
