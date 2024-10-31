using System;
using Character;
using UnityEngine;

public class CutAct_CharaShow : CutAction
{
	public string chara = string.Empty;

	public string wear = string.Empty;

	public string tin = string.Empty;

	public string gag = string.Empty;

	public string restrict = string.Empty;

	public string skirtFlick = string.Empty;

	public string show = string.Empty;

	public CutAct_CharaShow(CutScene cutScene)
		: base(cutScene, CUTACT.CHARASHOW)
	{
	}

	public override object Clone()
	{
		return new CutAct_CharaShow(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		Vector3 zero = Vector3.zero;
		element.GetVal(ref chara, "chara", 0);
		TagTextUtility.Load_String(ref wear, element, "wear");
		TagTextUtility.Load_String(ref tin, element, "tin");
		TagTextUtility.Load_String(ref gag, element, "gag");
		TagTextUtility.Load_String(ref restrict, element, "restrict");
		TagTextUtility.Load_String(ref skirtFlick, element, "skirtFlick");
		TagTextUtility.Load_String(ref show, element, "show");
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("chara", chara);
		if (wear.Length > 0)
		{
			element.AddAttribute("wear", wear);
		}
		if (tin.Length > 0)
		{
			element.AddAttribute("tin", tin);
		}
		if (gag.Length > 0)
		{
			element.AddAttribute("gag", gag);
		}
		if (restrict.Length > 0)
		{
			element.AddAttribute("restrict", restrict);
		}
		if (skirtFlick.Length > 0)
		{
			element.AddAttribute("skirtFlick", skirtFlick);
		}
		if (show.Length > 0)
		{
			element.AddAttribute("show", show);
		}
	}

	public override void Action(bool skip)
	{
		Human human = cutScene.GetHuman(chara);
		if (wear.Length > 0)
		{
			if (human.sex == SEX.FEMALE)
			{
				WEAR_SHOW wEAR_SHOW = WEAR_SHOW.ALL;
				if (string.Equals(wear, "UNDERWEAR", StringComparison.OrdinalIgnoreCase))
				{
					human.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.HIDE);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.HIDE);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.BOTTOM, WEAR_SHOW.HIDE);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.BRA, WEAR_SHOW.ALL);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.SHORTS, WEAR_SHOW.ALL);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMUPPER, WEAR_SHOW.HIDE);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMLOWER, WEAR_SHOW.HIDE);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPUPPER, WEAR_SHOW.ALL);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPLOWER, WEAR_SHOW.ALL);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_BOTTOM, WEAR_SHOW.ALL);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.GLOVE, WEAR_SHOW.ALL);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.PANST, WEAR_SHOW.ALL);
					human.wears.ChangeShow(WEAR_SHOW_TYPE.SOCKS, WEAR_SHOW.ALL);
				}
				else if (string.Equals(wear, "HALF", StringComparison.OrdinalIgnoreCase))
				{
					wEAR_SHOW = WEAR_SHOW.HALF;
					for (WEAR_SHOW_TYPE wEAR_SHOW_TYPE = WEAR_SHOW_TYPE.TOPUPPER; wEAR_SHOW_TYPE < WEAR_SHOW_TYPE.NUM; wEAR_SHOW_TYPE++)
					{
						human.wears.ChangeShow(wEAR_SHOW_TYPE, wEAR_SHOW);
					}
				}
				else if (string.Equals(wear, "HIDE", StringComparison.OrdinalIgnoreCase) || string.Equals(wear, "NUDE", StringComparison.OrdinalIgnoreCase))
				{
					wEAR_SHOW = WEAR_SHOW.HIDE;
					for (WEAR_SHOW_TYPE wEAR_SHOW_TYPE2 = WEAR_SHOW_TYPE.TOPUPPER; wEAR_SHOW_TYPE2 < WEAR_SHOW_TYPE.NUM; wEAR_SHOW_TYPE2++)
					{
						human.wears.ChangeShow(wEAR_SHOW_TYPE2, wEAR_SHOW);
					}
				}
				if (skirtFlick.Length > 0)
				{
					bool flag = bool.Parse(skirtFlick);
					WEAR_SHOW wEAR_SHOW2 = human.wears.GetShow(WEAR_SHOW_TYPE.BOTTOM, false);
					if (wEAR_SHOW2 == WEAR_SHOW.ALL && flag)
					{
						human.wears.ChangeShow(WEAR_SHOW_TYPE.BOTTOM, WEAR_SHOW.HALF);
					}
					else if (wEAR_SHOW2 == WEAR_SHOW.HALF && !flag)
					{
						human.wears.ChangeShow(WEAR_SHOW_TYPE.BOTTOM, WEAR_SHOW.ALL);
					}
				}
				human.CheckShow();
			}
			else
			{
				MALE_SHOW mALE_SHOW = MALE_SHOW.CLOTHING;
				if (string.Equals(wear, "HIDE", StringComparison.OrdinalIgnoreCase) || string.Equals(wear, "NUDE", StringComparison.OrdinalIgnoreCase))
				{
					mALE_SHOW = MALE_SHOW.NUDE;
				}
				Male male = human as Male;
				male.ChangeMaleShow(mALE_SHOW);
			}
		}
		if (tin.Length > 0 && human.sex == SEX.MALE)
		{
			Male male2 = human as Male;
			bool showTinWithWear = string.Equals(tin, "SHOW", StringComparison.OrdinalIgnoreCase);
			male2.SetShowTinWithWear(showTinWithWear);
		}
		if (gag.Length > 0 && human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			if (string.Equals(gag, "BallGag", StringComparison.OrdinalIgnoreCase) || string.Equals(gag, "GagBall", StringComparison.OrdinalIgnoreCase))
			{
				female.personality.gagItem = GAG_ITEM.BALLGAG;
				female.ChangeGagItem();
				female.ChangeShowGag(true);
			}
			else if (string.Equals(gag, "GumTape", StringComparison.OrdinalIgnoreCase))
			{
				female.personality.gagItem = GAG_ITEM.GUMTAPE;
				female.ChangeGagItem();
				female.ChangeShowGag(true);
			}
			else
			{
				female.personality.gagItem = GAG_ITEM.NONE;
				female.ChangeGagItem();
			}
		}
		if (restrict.Length > 0)
		{
			bool set = bool.Parse(restrict);
			human.ChangeRestrict(set);
		}
		if (show.Length > 0)
		{
			bool flag2 = true;
			if (show.Equals("hide", StringComparison.OrdinalIgnoreCase) || show.Equals("false", StringComparison.OrdinalIgnoreCase) || show.Equals("off", StringComparison.OrdinalIgnoreCase))
			{
				flag2 = false;
			}
			if (human.sex != 0)
			{
				Male male3 = human as Male;
				MALE_SHOW mALE_SHOW2 = ((!flag2) ? MALE_SHOW.HIDE : MALE_SHOW.CLOTHING);
				male3.ChangeMaleShow(mALE_SHOW2);
			}
		}
	}
}
