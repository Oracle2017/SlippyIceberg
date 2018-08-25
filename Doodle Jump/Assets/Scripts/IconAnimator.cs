using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAnimator : MonoBehaviour {
	//Vector3 startPos;
	//[SerializeField] Vector3 targetDist;
	[SerializeField] float smoothTime;
	Vector3 currentVelocity;
	bool isFirstTime;
	[SerializeField] float yOffset = 1;

	// Use this for initialization
	public void StartSettings () {
		isFirstTime = true;
		currentVelocity = Vector3.zero;
	}
	
	// Update is called once per frame
	public void UpdateSettings (Vector3 _position, Vector3 _startDist, Vector3 _endDist) {

		if (isFirstTime || ((transform.position.x - _endDist.x) >= -0.01f && 
			(transform.position.x - _endDist.x) <= 0.01f))
		{
			isFirstTime = false;
			transform.position = _position.y * Vector3.up + new Vector3(0, yOffset, 0) + _startDist;
			return;
		}

		//_position + _startDist

		transform.position =  Vector3.SmoothDamp(transform.position, _position.y * Vector3.up + new Vector3(0, yOffset, 0) + _endDist, ref currentVelocity, smoothTime); 
	}
}
