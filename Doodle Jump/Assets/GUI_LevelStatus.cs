using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_LevelStatus : MonoBehaviour {
	Sprite emptyDiamond;
	[SerializeField] Sprite filledDiamond;
	List<Image> diamondImgs;

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
	
	// Update is called once per frame
	public void FillDiamond (int _index) {
		if (_index < 0)
			return;

		diamondImgs[_index].sprite = filledDiamond;
	}

	public void Reset()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			diamondImgs[i].sprite = emptyDiamond;
		}	
	}
}
