using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour {
	public GameObject prefab;

	public int numberToCreate;

	// Use this for initialization
	public void StartSettings () {
		//Populate();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Populate()
	{
		GameObject newObj;

		for (int i = 0; i < numberToCreate; i++)
		{
			newObj = (GameObject) Instantiate(prefab, transform);
			newObj.GetComponent<Image>().color = Random.ColorHSV();
		}
	}

	void Reset()
	{
		gameObject.SetActive(false);
	}
}
