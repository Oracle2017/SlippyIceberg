using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour {
	[SerializeField] float speed = 10;
	[HideInInspector] public bool canFall;

	void Start()
	{
		canFall = false;
	}

	// Update is called once per frame
	void Update () {
		if (canFall)
		{
			FallDown();
		}
	}

	void FallDown()
	{
		transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
	}
}
