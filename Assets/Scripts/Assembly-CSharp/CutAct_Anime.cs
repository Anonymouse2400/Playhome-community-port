using System;
using UnityEngine;

public class CutAct_Anime : CutAction
{
	public string chara;

	public string state;

	public float duration = 0.5f;

	public CutAct_Anime(CutScene cutScene)
		: base(cutScene, CUTACT.ANIME)
	{
	}

	public CutAct_Anime(CutScene cutScene, float time, string chara, string state, float duration)
		: base(cutScene, CUTACT.ANIME, time)
	{
		this.chara = chara;
		this.state = state;
		this.duration = duration;
	}

	public override object Clone()
	{
		return new CutAct_Anime(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		element.GetVal(ref chara, "chara", 0);
		element.GetVal(ref state, "state", 0);
		element.GetVal(ref duration, "duration", 0);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("chara", chara);
		element.AddAttribute("state", state);
		element.AddAttribute("duration", duration.ToString());
	}

	public override void Action(bool skip)
	{
		Human human = cutScene.GetHuman(chara);
		Animator anime = human.body.Anime;
		anime.CrossFadeInFixedTime(state, duration, 0);
	}
}
