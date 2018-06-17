using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {
	 
	public static float Map(float unscaledNum, float minInput, float maxInput, float minOutput, float maxOutput) {
		return (maxOutput - minOutput) * (unscaledNum - minInput) / (maxInput - minInput) + minOutput;
	}

	public static Vector3 AbsY(Vector3 _v)
	{
		if (_v.y < 0)
			return new Vector3(_v.x, -_v.y, _v.z);
		else
			return _v;
	}
}
