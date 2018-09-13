using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour {
	float timer;
	float startPosY;
	[SerializeField] GameObject GUI_feedback;

	// Use this for initialization
	void Start () {
		startPosY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		float sample = Mathf.Sin(Mathf.Deg2Rad * timer);
		timer+= Time.deltaTime * CoinManager.moveSpeed;
		sample = Mathf.Clamp(sample, -1, 1);
		float newPosY = Utils.Map(sample, -1, 1, startPosY - CoinManager.moveDist, startPosY + CoinManager.moveDist);
		transform.position = new Vector3(transform.position.x, newPosY);
	}

	void OnDestroy()
	{
		GameObject _coinRewardGameObject = Instantiate(GUI_feedback, transform.position, Quaternion.identity, GameManager.GUICanvas.transform);
	}
}
