using System;

public class CutAct_NextScene : CutAction
{
	public string scene;

	public string message;

	public float fadeTime = -1f;

	public CutAct_NextScene(CutScene cutScene)
		: base(cutScene, CUTACT.NEXTSCENE)
	{
	}

	public CutAct_NextScene(CutScene cutScene, float time, string scene, string message)
		: base(cutScene, CUTACT.NEXTSCENE, time)
	{
		this.scene = scene;
		this.message = message;
	}

	public override object Clone()
	{
		return new CutAct_NextScene(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		fadeTime = -1f;
		base.Load(element, order);
		scene = TagTextUtility.Load_String(element, "scene");
		message = TagTextUtility.Load_String(element, "scene", 1);
		TagTextUtility.Load_Float(ref fadeTime, element, "scene", 2);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("scene", scene);
		element.AddAttribute("scene", message);
	}

	public override void Action(bool skip)
	{
		cutScene.NextScene(scene, message);
	}
}
