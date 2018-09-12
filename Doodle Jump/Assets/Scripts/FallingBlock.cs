using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour {
	[SerializeField] float speed;
	public bool canFall;

	void Start()
	{
		canFall = false;
	}

	// Update is called once per frame
	void Update () {
		if (canFall && !GameManager.isPausing)
		{
			FallDown();
		}
	}

	void FallDown()
	{
		
		if ((transform.position.y + Level3.blockSize.y / 2) < (-GameManager.screenHeight / 2))
		{
			if (transform.parent.childCount <= 1)
			{
				Destroy(transform.parent.gameObject);
			}
			Destroy(gameObject);
			return;
		}

		transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
	}
}
