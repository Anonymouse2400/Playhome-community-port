using System;

using UnityEngine;

public class CutAct_Voice : CutAction
{
	public string chara;

	public string voice;

	public bool loop;

	public CutAct_Voice(CutScene cutScene)
		: base(cutScene, CUTACT.VOICE)
	{
	}

	public CutAct_Voice(CutScene cutScene, float time, string chara, string voice)
		: base(cutScene, CUTACT.VOICE, time)
	{
		this.chara = chara;
		this.voice = voice;
	}

	public override object Clone()
	{
		return new CutAct_Voice(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		element.GetVal(ref chara, "chara", 0);
		element.GetVal(ref voice, "voice", 0);
		element.GetVal(ref loop, "loop", 0);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("chara", chara);
		element.AddAttribute("voice", voice);
		if (loop)
		{
			element.AddAttribute("loop", loop.ToString());
		}
	}

	public override void Action(bool skip)
	{
		AudioClip clip = cutScene.LoadAsset<AudioClip>(voice);
		Human human = cutScene.GetHuman(chara);
		human.PhonationVoice(clip, loop);
	}
}
