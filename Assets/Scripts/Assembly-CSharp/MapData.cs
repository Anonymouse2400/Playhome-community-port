using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
	public enum FOOT
	{
		BARE = 0,
		SOCKS = 1,
		SHOES = 2
	}

	public string name = "マップ名";

	public int order = -1;

	public FOOT foot = FOOT.SHOES;

	public H_PosSet h_pos = new H_PosSet();

	public List<string> noRecieveShadows = new List<string>();

	public Vector3 selectPos;

	public float selectYaw;

	public string mob = string.Empty;
}
