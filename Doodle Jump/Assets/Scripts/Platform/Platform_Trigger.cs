using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Trigger : MonoBehaviour {

	//the collider of the main visible platform
	[SerializeField] BoxCollider2D platformCollider;
	//this variable is true when the players is just below the platform so that its Box collider can be disabled that will allow the player to pass through the platform
	bool isTriggered;

	void Start () {
		platformCollider.enabled=false;   
	}

	void Update () {
		//Enabling or Disabling the platform's Box collider to allowing player to pass
		if (isTriggered)
			platformCollider.enabled=true;
		if (!isTriggered)
			platformCollider.enabled=false;   
	}
	//Checking the collison of the gameobject we created in step 2 for checking if the player is just below the platform and nedded to ignore the collison to the platform
	void OnTriggerStay2D(Collider2D other) {
		isTriggered = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		//Just to make sure that the platform's Box Collider does not get permantly disabled and it should be enabeled once the player get its through
		isTriggered = false;
	}

}
