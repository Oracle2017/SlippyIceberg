using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GUI_ControlsHelper : MonoBehaviour {
	[SerializeField] GUI_Helper phone;
	[SerializeField] Text description;

	// Use this for initialization
	public void StartSettings () {
		phone.StartSettings();
	}
	
	// Update is called once per frame
	public void UpdateSettings () {
		phone.UpdateSettings();
	}
}
