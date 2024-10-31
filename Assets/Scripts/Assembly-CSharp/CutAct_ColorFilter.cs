using System;
using UnityEngine;

public class CutAct_ColorFilter : CutAction
{
	public Color color;

	public float duration;

	protected float changeTimer;

	public CutAct_ColorFilter(CutScene cutScene)
		: base(cutScene, CUTACT.COLORFILTER)
	{
	}

	public CutAct_ColorFilter(CutScene cutScene, float time, Color color, float duration)
		: base(cutScene, CUTACT.COLORFILTER, time)
	{
		this.color = color;
		this.duration = duration;
		changeTimer = 0f;
	}

	public override object Clone()
	{
		return new CutAct_ColorFilter(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		color = TagTextUtility.Load_Color(element, "color");
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		TagTextUtility.Save_Color(element, "color", color);
	}
}
