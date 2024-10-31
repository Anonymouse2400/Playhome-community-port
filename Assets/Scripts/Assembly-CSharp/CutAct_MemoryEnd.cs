using System;

public class CutAct_MemoryEnd : CutAction
{
	public float fadeTime = -1f;

	public CutAct_MemoryEnd(CutScene cutScene)
		: base(cutScene, CUTACT.MEMORYEND)
	{
	}

	public CutAct_MemoryEnd(CutScene cutScene, float time)
		: base(cutScene, CUTACT.MEMORYEND, time)
	{
	}

	public override object Clone()
	{
		return new CutAct_MemoryEnd(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		fadeTime = -1f;
		base.Load(element, order);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
	}

	public override void Action(bool skip)
	{
		if (GlobalData.isMemory)
		{
			cutScene.NextScene("SelectScene", "MemoryEnd");
		}
	}
}
