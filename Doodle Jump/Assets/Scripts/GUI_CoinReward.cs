using UnityEngine;
using UnityEngine.UI;

public class GUI_CoinReward : MonoBehaviour {
	[SerializeField] float scaleSpeed = 10;
	[SerializeField] float moveSpeed = 10;
	Text GUItext;

	// Use this for initialization
	void Start () {
		GUItext = gameObject.GetComponent<Text>();
		GUItext.text = "+1";
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.magnitude < 0.1f)
		{
			Destroy(gameObject);
		}

		transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
		transform.position += Vector3.up * moveSpeed * Time.deltaTime;
	}
}
