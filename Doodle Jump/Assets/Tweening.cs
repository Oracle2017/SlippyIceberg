using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweening : MonoBehaviour {
	RectTransform rectTransform;
	float swingTimer;
	[SerializeField] float bouncingSpeed;
	[SerializeField] float scaleDifference;
	Vector3 startScale;
	public bool alwaysTween = false;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform>();
		startScale = rectTransform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.isPausing)
		{
			return;
		}

		if ((Score.currentLvl == 1 && PlayerPrefs.GetInt("newCosmeticVisible") == 1) || alwaysTween)
		{
			rectTransform.localScale = startScale + Vector3.one * (ScaleTarget());
		}

		else 
		{
			Reset();
		}
	}

	float ScaleTarget()
	{
		float sample = Mathf.Sin(Mathf.Deg2Rad * swingTimer);
		swingTimer+= Time.deltaTime * bouncingSpeed;
		sample = Mathf.Clamp(sample, -1, 1);
		float bounceTarget = Utils.Map(sample, -1, 1, -scaleDifference, scaleDifference);
	
		return bounceTarget;
	}

	void Reset()
	{
		swingTimer = 0;
		rectTransform.localScale = startScale;
	}

}
