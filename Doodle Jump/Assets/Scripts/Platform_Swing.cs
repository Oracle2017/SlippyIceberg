using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Swing : Platform {
	float timer;

	protected override float RotationTarget()
	{
		float sample = Mathf.Sin(Mathf.Deg2Rad * timer);
		timer+= Time.deltaTime * rotationSpeed;
		sample = Mathf.Clamp(sample * rotationMultiplier, -1, 1);
		print(sample);
		float rotationTarget = Utils.Map(sample, -1, 1, -60, 60);

		rotationTarget = RegulateAngle(rotationTarget);

		return rotationTarget;
	}

	public override void Reset()
	{
		base.Reset();
		timer = 0;
	}
}
