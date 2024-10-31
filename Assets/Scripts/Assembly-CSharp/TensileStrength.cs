using System;
using UnityEngine;

public class TensileStrength : MonoBehaviour
{
	[SerializeField]
	protected float strength = 1f;

	[SerializeField]
	protected float relaxDistance = 0.1f;

	[SerializeField]
	protected float minDistance = 0.01f;

	[SerializeField]
	protected float maxDistance = 1f;

	[SerializeField]
	protected float kinematicWeight;

	[SerializeField]
	protected Rigidbody[] rigids;

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
		int num = 0;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		for (int i = 0; i < rigids.Length; i++)
		{
			if (rigids[i].isKinematic)
			{
				num++;
				zero += rigids[i].position;
			}
			else
			{
				zero2 += rigids[i].position;
			}
		}
		Vector3 zero3 = Vector3.zero;
		if (num == 0)
		{
			zero3 = zero2 / rigids.Length;
		}
		else if (num == rigids.Length)
		{
			zero3 = zero2 / (rigids.Length - num);
		}
		else
		{
			zero /= (float)num;
			zero2 /= (float)(rigids.Length - num);
			zero3 = Vector3.Lerp(zero2, zero, kinematicWeight);
		}
		for (int j = 0; j < rigids.Length; j++)
		{
			if (!rigids[j].isKinematic)
			{
				Vector3 vector = rigids[j].position - zero3;
				if (vector.magnitude < minDistance)
				{
					rigids[j].position = zero3 + vector.normalized * minDistance;
					vector = rigids[j].position - zero3;
				}
				else if (vector.magnitude > maxDistance)
				{
					rigids[j].position = zero3 + vector.normalized * maxDistance;
					vector = rigids[j].position - zero3;
				}
				Vector3 vector2 = zero3 + vector.normalized * relaxDistance;
				Vector3 force = vector2 - rigids[j].position;
				force *= strength;
				rigids[j].AddForce(force, ForceMode.Acceleration);
			}
		}
	}
}
