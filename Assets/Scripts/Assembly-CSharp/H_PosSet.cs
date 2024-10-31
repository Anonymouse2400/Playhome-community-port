using System;
using System.Collections.Generic;
using H;

public class H_PosSet
{
	public List<H_Pos> floor = new List<H_Pos>();

	public List<H_Pos> chair = new List<H_Pos>();

	public List<H_Pos> wall = new List<H_Pos>();

	public List<H_Pos> special = new List<H_Pos>();

	public List<H_Pos> five_Resist = new List<H_Pos>();

	public List<H_Pos> five_Flop = new List<H_Pos>();

	public List<H_Pos> five_Weakness = new List<H_Pos>();

	public bool CheckEnable(H_StyleData.POSITION position)
	{
		switch (position)
		{
		case H_StyleData.POSITION.FLOOR:
			return floor.Count > 0;
		case H_StyleData.POSITION.CHAIR:
			return chair.Count > 0;
		case H_StyleData.POSITION.WALL:
			return wall.Count > 0;
		case H_StyleData.POSITION.SPECIAL:
			return special.Count > 0;
		case H_StyleData.POSITION.FIVE_RESIST:
			return five_Resist.Count > 0;
		case H_StyleData.POSITION.FIVE_FLOP:
			return five_Flop.Count > 0;
		case H_StyleData.POSITION.FIVE_WEAKNESS:
			return five_Weakness.Count > 0;
		default:
			return false;
		}
	}
}
