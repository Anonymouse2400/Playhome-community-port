using System;
using Character;
using UnityEngine;

public class BustSoft
{
	private static float[] bustDamping = new float[3] { 0.2f, 0.1f, 0.1f };

	private static float[] bustElasticity = new float[3] { 0.2f, 0.15f, 0.05f };

	private static float[] bustStiffness = new float[3] { 1f, 0.1f, 0.01f };

	private Human human;

	public BustSoft(Human human)
	{
		this.human = human;
	}

	public void ReCalc()
	{
		BodyParameter body = human.customParam.body;
		float value = body.bustSoftness * body.shapeVals[1] + 0.01f;
		value = Mathf.Clamp(value, 0f, 1f);
		float stiffness = TreeLerp(bustStiffness, value);
		float elasticity = TreeLerp(bustElasticity, value);
		float damping = TreeLerp(bustDamping, value);
		human.body.bustDynamicBone_L.setSoftParams(0, -1, damping, elasticity, stiffness);
		human.body.bustDynamicBone_R.setSoftParams(0, -1, damping, elasticity, stiffness);
	}

	private float TreeLerp(float[] vals, float rate)
	{
		if (rate < 0.5f)
		{
			return Mathf.Lerp(vals[0], vals[1], rate * 2f);
		}
		return Mathf.Lerp(vals[1], vals[2], (rate - 0.5f) * 2f);
	}
}
