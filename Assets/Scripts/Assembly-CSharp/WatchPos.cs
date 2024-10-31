using System;
using UnityEngine;

public class WatchPos
{
	public enum TYPE
	{
		STAND = 0,
		FLOOR = 1,
		CHAIR = 2,
		NUM = 3
	}

	public PosYaw pos = new PosYaw();

	public TYPE type;

	public WatchPos()
	{
	}

	public WatchPos(string typeStr, Vector3 pos, float yaw)
	{
		type = StrToType(typeStr);
		this.pos.pos = pos;
		this.pos.yaw = yaw;
	}

	public static TYPE StrToType(string str)
	{
		if (str.Equals("floor", StringComparison.OrdinalIgnoreCase))
		{
			return TYPE.FLOOR;
		}
		if (str.Equals("chair", StringComparison.OrdinalIgnoreCase))
		{
			return TYPE.CHAIR;
		}
		return TYPE.STAND;
	}
}
