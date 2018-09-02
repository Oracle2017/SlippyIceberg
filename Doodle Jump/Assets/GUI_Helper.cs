using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Helper : MonoBehaviour {
	[SerializeField] int tiltDegree = 45;
	float timer;
	[SerializeField] float rotationSpeed = 10;

	public void StartSettings()
	{
		timer = 0;
	}

	// Update is called once per frame
	public void UpdateSettings () {
		float sample = Mathf.Sin(Mathf.Deg2Rad * timer);
		timer+= Time.deltaTime * rotationSpeed;
		sample = Mathf.Clamp(sample, -1, 1);
		float rotationTarget = Utils.Map(sample, -1, 1, -tiltDegree, tiltDegree);

		transform.rotation = Quaternion.Euler(0, 0, rotationTarget);

		//transform.localScale = Vector3.SmoothDamp(transform.localScale, newScale, ref currentScaleVelocity, transitionSpeed);
	}



}
