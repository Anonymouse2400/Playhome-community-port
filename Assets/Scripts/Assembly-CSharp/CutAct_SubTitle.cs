using System;

public class CutAct_SubTitle : CutAction
{
	public string text;

	public CutAct_SubTitle(CutScene cutScene)
		: base(cutScene, CUTACT.SUBTITLE)
	{
	}

	public CutAct_SubTitle(CutScene cutScene, float time, string text)
		: base(cutScene, CUTACT.SUBTITLE, time)
	{
		this.text = text;
	}

	public override object Clone()
	{
		return new CutAct_SubTitle(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		text = TagTextUtility.Load_String(element, "text", -1);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("text", text);
	}

	public override void Action(bool skip)
	{
		cutScene.SetSubTitle(text);
	}
}
