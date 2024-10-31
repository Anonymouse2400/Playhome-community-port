using System;
using UnityEngine;

public class CutAct_Position : CutAction
{
	public string chara;

	public Vector3 pos;

	public Quaternion rot;

	public CutAct_Position(CutScene cutScene)
		: base(cutScene, CUTACT.POSITION)
	{
	}

	public override object Clone()
	{
		return new CutAct_Position(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		Vector3 vec = Vector3.zero;
		element.GetVal(ref chara, "chara", 0);
		TagTextUtility.Load_Vector3(ref pos, element, "pos");
		if (TagTextUtility.Load_Vector3(ref vec, element, "euler"))
		{
			rot = Quaternion.Euler(vec);
		}
		else
		{
			TagTextUtility.Load_Quaternion(ref rot, element, "rot");
		}
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("chara", chara);
		TagTextUtility.Save_Vector3(element, "pos", pos);
		TagTextUtility.Save_Quaternion(element, "rot", rot);
	}

	public override void Action(bool skip)
	{
		Human human = cutScene.GetHuman(chara);
		human.transform.position = pos;
		human.transform.rotation = rot;
	}
}
