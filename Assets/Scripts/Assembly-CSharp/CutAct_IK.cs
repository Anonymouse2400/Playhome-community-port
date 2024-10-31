using System;
using UnityEngine;

public class CutAct_IK : CutAction
{
	public string chara = string.Empty;

	public string handR_Tgt = string.Empty;

	public string handR_Nul = string.Empty;

	public string handL_Tgt = string.Empty;

	public string handL_Nul = string.Empty;

	public string tin_Tgt = string.Empty;

	public string tin_Nul = string.Empty;

	public CutAct_IK(CutScene cutScene)
		: base(cutScene, CUTACT.IK)
	{
	}

	public override object Clone()
	{
		return new CutAct_IK(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		element.GetVal(ref chara, "chara", 0);
		element.GetVal(ref handR_Tgt, "handR", 0);
		element.GetVal(ref handR_Nul, "handR", 1);
		element.GetVal(ref handL_Tgt, "handL", 0);
		element.GetVal(ref handL_Nul, "handL", 1);
		element.GetVal(ref tin_Tgt, "tin", 0);
		element.GetVal(ref tin_Nul, "tin", 1);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("chara", chara);
		if (handR_Tgt.Length > 0)
		{
			element.AddAttribute("handR", handR_Tgt);
			if (handR_Nul.Length > 0)
			{
				element.AddAttribute("handR", handR_Nul);
			}
		}
		if (handL_Tgt.Length > 0)
		{
			element.AddAttribute("handL", handL_Tgt);
			if (handL_Nul.Length > 0)
			{
				element.AddAttribute("handL", handL_Nul);
			}
		}
		if (tin_Tgt.Length > 0)
		{
			element.AddAttribute("tin", tin_Tgt);
			if (tin_Nul.Length > 0)
			{
				element.AddAttribute("tin", tin_Nul);
			}
		}
	}

	public override void Action(bool skip)
	{
		Human human = cutScene.GetHuman(chara);
		if (handR_Tgt.Length > 0)
		{
			Human human2 = cutScene.GetHuman(handR_Tgt);
			if (human2 != null)
			{
				Transform target = Transform_Utility.FindTransform(human2.body.Anime.transform, handR_Nul);
				human.ik.SetIK(IK_Data.PART.HAND_R, target);
			}
			else
			{
				human.ik.ClearIK(IK_Data.PART.HAND_R);
			}
		}
		if (handL_Tgt.Length > 0)
		{
			Human human3 = cutScene.GetHuman(handL_Tgt);
			if (human3 != null)
			{
				Transform target2 = Transform_Utility.FindTransform(human3.body.Anime.transform, handL_Nul);
				human.ik.SetIK(IK_Data.PART.HAND_L, target2);
			}
			else
			{
				human.ik.ClearIK(IK_Data.PART.HAND_L);
			}
		}
		if (tin_Tgt.Length > 0)
		{
			Human human4 = cutScene.GetHuman(tin_Tgt);
			if (human4 != null)
			{
				Transform target3 = Transform_Utility.FindTransform(human4.body.Anime.transform, tin_Nul);
				human.ik.SetIK(IK_Data.PART.TIN, target3);
			}
			else
			{
				human.ik.ClearIK(IK_Data.PART.TIN);
			}
		}
	}
}
