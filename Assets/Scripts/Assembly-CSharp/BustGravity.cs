using System;
using Character;
using UnityEngine;

public class BustGravity
{
	private static float[] range = new float[2] { 0f, -0.005f };

	private Human human;

	public BustGravity(Human human)
	{
		this.human = human;
	}

	public void ReCalc()
	{
		BodyParameter body = human.customParam.body;
		float num = body.shapeVals[1] * body.bustSoftness * 0.5f;
		float y = Mathf.Lerp(range[0], range[1], body.bustWeight) * num;
		human.body.bustDynamicBone_L.setGravity(0, new Vector3(0f, y, 0f));
		human.body.bustDynamicBone_R.setGravity(0, new Vector3(0f, y, 0f));
	}
}
