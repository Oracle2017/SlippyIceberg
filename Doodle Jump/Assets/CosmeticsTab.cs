using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticsTab : MonoBehaviour {
	[SerializeField] Transform cosmeticsParent;
	//[SerializeField] Cosmet cosmeticPreviewer;

	// Use this for initialization
	public void StartSettings () {
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

	public void CloseWindow()
	{
		GameManager.isPausing = false;
		gameObject.SetActive(false);
	}
}
