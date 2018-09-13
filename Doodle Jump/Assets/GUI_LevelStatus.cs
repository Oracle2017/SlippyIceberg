using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_LevelStatus : MonoBehaviour {
	Sprite emptyDiamond;
	[SerializeField] Sprite filledDiamond;
	List<Image> diamondImgs;
	[HideInInspector] public bool isFilled;
	[HideInInspector] public bool isInRoutine;

	// Use this for initialization
	public void StartSettings () {
		emptyDiamond = transform.GetChild(0).GetComponent<Image>().sprite;

		diamondImgs = new List<Image>();

		for (int i = 0; i < transform.childCount; i++)
		{
			Image _img = transform.GetChild(i).GetComponent<Image>();
			diamondImgs.Add(_img);
		}
	}

	void Update()
	{
		if (isFilled && !isInRoutine)
		{
			StartCoroutine(StayFilled(5));
		}
	}

	IEnumerator StayFilled(float _waitTime)
	{
		isInRoutine = true;
		yield return new WaitForSeconds(_waitTime);
		if (!isFilled)
		{
			//already reseted in filldiamond
			yield break;
		}
		Reset();
		isFilled = false;
		isInRoutine = false;
	}
	
	// Update is called once per frame
	public void FillDiamond (int _index) {
		if (_index < 0)
			return;

		if (_index == 0 && isFilled)
		{
			Reset();
		}

		else if (_index == diamondImgs.Count - 1)
		{
			isFilled = true;
		}

		diamondImgs[_index].sprite = filledDiamond;

		//print(_index);


	}

	public void Reset()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			diamondImgs[i].sprite = emptyDiamond;
		}	

		isFilled = false;
		isInRoutine = false;
	}
}
