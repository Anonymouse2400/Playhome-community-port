using System;
using UnityEngine;

public class IK_Data
{
	public enum PART
	{
		HAND_L = 0,
		HAND_R = 1,
		FOOT_L = 2,
		FOOT_R = 3,
		TIN = 4,
		MOUTH = 5
	}

	public string ikChara;

	public PART ikPart;

	public string targetChara;

	public string targetPart;

	public IK_Data(string ikChara, string ikPart, string targetChara, string targetPart)
	{
		this.ikChara = ikChara;
		this.ikPart = LoadIKPart(ikPart);
		this.targetChara = targetChara;
		this.targetPart = targetPart;
	}

	private PART LoadIKPart(string name)
	{
		switch (name)
		{
		case "handL":
			return PART.HAND_L;
		case "handR":
			return PART.HAND_R;
		case "footL":
			return PART.FOOT_L;
		case "footR":
			return PART.FOOT_R;
		case "tin":
			return PART.TIN;
		case "mouth":
			return PART.MOUTH;
		default:
			Debug.LogError("不明なIK箇所:" + name);
			return PART.TIN;
		}
	}
}
