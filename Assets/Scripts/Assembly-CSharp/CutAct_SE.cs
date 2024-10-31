using System;
using UnityEngine;

public class CutAct_SE : CutAction
{
	private string file;

	public CutAct_SE(CutScene cutScene)
		: base(cutScene, CUTACT.SE)
	{
	}

	public CutAct_SE(CutScene cutScene, float time, string file)
		: base(cutScene, CUTACT.SE, time)
	{
		this.file = file;
	}

	public override object Clone()
	{
		return new CutAct_SE(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		file = TagTextUtility.Load_String(element, "file");
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("file", file);
	}

	public override void Action(bool skip)
	{
		if (!skip && file != null && file.Length > 0)
		{
			AudioClip audioClip = cutScene.LoadAsset<AudioClip>(file);
			if (audioClip != null)
			{
				cutScene.GC.audioCtrl.Play2DSE(audioClip);
			}
			else
			{
				Debug.LogError("効果音が読めなかった:" + file);
			}
		}
	}
}
