using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Manager : MonoBehaviour {
	[SerializeField] GUI_Scaling guiScalingPrefab;
	public static GUI_Scaling guiScaling;

	// Use this for initialization
	void Start () {
		guiScaling = guiScalingPrefab;
	}

}
