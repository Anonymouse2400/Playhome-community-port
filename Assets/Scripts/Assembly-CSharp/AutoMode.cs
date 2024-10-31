using System;
using Spline;
using UnityEngine;

public class AutoMode
{
	private float poseMin = -1f;

	private float poseMax = 1f;

	private float strokeMin = -0.5f;

	private float strokeMax = 0.5f;

	private RNS wanderSpline = new TNS();

	private float nextRate;

	private int wanderNum = 10;

	public Vector2 nowPos { get; private set; }

	public void StartWander(Vector2 pos, float gage, float distanceThreshold)
	{
		nowPos = pos;
		wanderSpline.Init();
		wanderSpline.AddNode(nowPos);
		Vector2 checkPos = nowPos;
		for (int i = 0; i < wanderNum - 1; i++)
		{
			Vector2 vector = NextPos2(checkPos, gage, distanceThreshold);
			wanderSpline.AddNode(vector);
			checkPos = vector;
		}
		wanderSpline.BuildSpline();
		nextRate = 0f;
	}

	public void Update(float animeSpeed, float changeSpeed, float gage, float distanceThreshold)
	{
		float num = changeSpeed * Time.deltaTime / (float)wanderNum;
		nextRate += num;
		float num2 = 0.5f;
		float num3 = 1f / (float)wanderNum;
		if (nextRate >= num2)
		{
			Vector3 position = wanderSpline.node[wanderSpline.nodeCount - 1].position;
			Vector3 pos = NextPos2(position, gage, distanceThreshold);
			nextRate = wanderSpline.RenewMove(pos, nextRate);
		}
		nowPos = wanderSpline.GetPosition(nextRate);
	}

	private Vector2 NextPos2(Vector2 checkPos, float gage, float threshold)
	{
		Vector2 vector = checkPos;
		for (float num = 0f; num < threshold; num = Vector2.Distance(checkPos, vector))
		{
			vector.x = UnityEngine.Random.Range(poseMin, poseMax);
			vector.y = gage + UnityEngine.Random.Range(strokeMin, strokeMax);
			vector.x = Mathf.Clamp(vector.x, -1f, 1f);
			vector.y = Mathf.Clamp(vector.y, -1f, 1f);
		}
		return vector;
	}
}
