using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Scaling : MonoBehaviour {
	[SerializeField] IconAnimator leftIcon;
	[SerializeField] IconAnimator rightIcon;

	// Use this for initialization
	public void StartSettings () {
		leftIcon.StartSettings();
		rightIcon.StartSettings();
	}
	
	// Update is called once per frame
	public void UpdateSettings (Vector3 _position, Vector3 _startDist, Vector3 _endDist) {
		
		leftIcon.UpdateSettings(_position, -1 * _startDist, - 1 * _endDist);
		//print("left side transform.position.x - _endDist.x = " + (leftIcon.transform.position.x - (- 1 *_endDist.x)));

		rightIcon.UpdateSettings(_position, _startDist, _endDist);
		//print("right side transform.position.x - _endDist.x = " + (rightIcon.transform.position.x - _endDist.x));
	}
}
