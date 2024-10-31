using System;

public class CutAct_BGM : CutAction
{
	private string file;

	private bool play;

	public CutAct_BGM(CutScene cutScene)
		: base(cutScene, CUTACT.BGM)
	{
	}

	public override object Clone()
	{
		return new CutAct_BGM(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		file = TagTextUtility.Load_String(element, "file");
		TagTextUtility.Load_Bool(ref play, element, "play");
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("file", file);
		element.AddAttribute("play", play.ToString());
	}

	public override void Action(bool skip)
	{
		if (file != null && file.Length > 0)
		{
			cutScene.GC.audioCtrl.BGM_Load(file);
		}
		if (!play)
		{
			cutScene.GC.audioCtrl.BGM_Stop();
		}
		else
		{
			cutScene.GC.audioCtrl.BGM_Play();
		}
	}
}
