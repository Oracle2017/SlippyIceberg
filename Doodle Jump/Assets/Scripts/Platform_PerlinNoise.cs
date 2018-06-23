using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_PerlinNoise : Platform {
	float offset;

	public override void StartSettings()
	{
		base.StartSettings();
		offset = Random.Range(0, 100000);
	}

	public override void UpdatePlatform()
	{
		base.UpdatePlatform();
		offset += Time.deltaTime * rotationSpeed;
	}

	protected override float RotationTarget()
	{
		float zRotation = offset;
		float sample = Mathf.PerlinNoise(zRotation, 0);
		sample = Mathf.Clamp(sample * rotationMultiplier, 0, 1);
		float rotationTarget = Utils.Map(sample, 0, 1, -90, 90);

		rotationTarget = RegulateAngle(rotationTarget);

		return rotationTarget;
	}

	public override void Reset ()
	{
		base.Reset();
		offset = Random.Range(0, 100000);
	}
}
