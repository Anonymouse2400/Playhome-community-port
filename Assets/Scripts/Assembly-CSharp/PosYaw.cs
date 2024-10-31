using System;
using UnityEngine;

public class PosYaw
{
	public Vector3 pos;

	public float yaw;

	public PosYaw()
	{
	}

	public PosYaw(Vector3 pos, float yaw)
	{
		this.pos = pos;
		this.yaw = yaw;
	}

	public void Copy(PosYaw copy)
	{
		pos = copy.pos;
		yaw = copy.yaw;
	}

	public void Zero()
	{
		pos = Vector3.zero;
		yaw = 0f;
	}

	public void Get(out Vector3 getPos, out Quaternion getRot)
	{
		getPos = pos;
		getRot = Quaternion.Euler(0f, yaw, 0f);
	}
}
