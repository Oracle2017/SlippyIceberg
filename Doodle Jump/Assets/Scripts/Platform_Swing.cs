using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Swing : Platform {
	[Space(10)]
	[Header("Swing Parameters")]
	[SerializeField] float tiltDegree;

	protected override float RotationTarget()
	{
		float sample = Mathf.Sin(Mathf.Deg2Rad * swingTimer);
		swingTimer+= Time.deltaTime * rotationSpeed;
		sample = Mathf.Clamp(sample * rotationMultiplier, -1, 1);
		float rotationTarget = Utils.Map(sample, -1, 1, -tiltDegree, tiltDegree);

		rotationTarget = RegulateAngle(rotationTarget);

		return rotationTarget;
	}

	public override void Reset()
	{
		base.Reset();
		swingTimer = 0;
	}
}
