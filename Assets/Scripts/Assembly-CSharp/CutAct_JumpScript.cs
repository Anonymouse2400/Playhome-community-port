using System;

public class CutAct_JumpScript : CutAction
{
	public string loadAssetBundle;

	public string loadScript;

	public CutAct_JumpScript(CutScene cutScene)
		: base(cutScene, CUTACT.JUMPSCRIPT)
	{
	}

	public CutAct_JumpScript(CutScene cutScene, float time, string loadAssetBundle, string loadScript)
		: base(cutScene, CUTACT.JUMPSCRIPT, time)
	{
		this.loadAssetBundle = loadAssetBundle;
		this.loadScript = loadScript;
	}

	public override object Clone()
	{
		return new CutAct_JumpScript(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		loadAssetBundle = TagTextUtility.Load_String(element, "script");
		loadScript = TagTextUtility.Load_String(element, "script", 1);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("script", loadAssetBundle);
		element.AddAttribute("script", loadScript);
	}

	public override void Action(bool skip)
	{
		cutScene.JumpScript(loadAssetBundle, loadScript);
	}
}
