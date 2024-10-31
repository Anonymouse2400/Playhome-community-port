using System;
using System.Collections.Generic;
using UnityEngine;

public class H_Pos
{
	public PosYaw pos = new PosYaw();

	public List<WatchPos> watchPos = new List<WatchPos>();

	public H_Pos()
	{
	}

	public H_Pos(Vector3 pos, float yaw)
	{
		this.pos.pos = pos;
		this.pos.yaw = yaw;
	}

	public void Get(out Vector3 getPos, out Quaternion getRot)
	{
		pos.Get(out getPos, out getRot);
	}

	public void AddWathPos(string typeStr, Vector3 pos, float yaw)
	{
		watchPos.Add(new WatchPos(typeStr, pos, yaw));
	}
}
