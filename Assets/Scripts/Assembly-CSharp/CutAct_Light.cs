using System;
using UnityEngine;

public class CutAct_Light : CutAction
{
	public Vector3 light = Vector3.zero;

	public CutAct_Light(CutScene cutScene)
		: base(cutScene, CUTACT.LIGHT)
	{
	}

	public override object Clone()
	{
		return new CutAct_Light(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		element.GetVal(ref light.x, "light", 0);
		element.GetVal(ref light.y, "light", 1);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("light", light.x.ToString());
		element.AddAttribute("light", light.y.ToString());
	}

	public override void Action(bool skip)
	{
		cutScene.SetLight(light.y, light.x);
	}
}
