using System;
using System.Collections.Generic;
using UnityEngine;

public class CharaCutActionSet
{
	public Human human;

	public List<CutAct_Anime> act_Animes = new List<CutAct_Anime>();

	public List<CutAct_Voice> act_Voices = new List<CutAct_Voice>();

	public CharaCutActionSet(Human human)
	{
		this.human = human;
	}

	public void AddAct(CutAct_Anime act)
	{
		act_Animes.Add(act);
	}

	public void AddAct(CutAct_Voice act)
	{
		act_Voices.Add(act);
	}

	public void RemoveAct(CutAct_Anime act)
	{
		act_Animes.Remove(act);
	}

	public void RemoveAct(CutAct_Voice act)
	{
		act_Voices.Remove(act);
	}

	public void Sort()
	{
		act_Animes.Sort(CutScene.SortFunc);
		act_Voices.Sort(CutScene.SortFunc);
	}

	public void Check(float time)
	{
		Check_Anime(time);
	}

	public void Check_Anime(float time)
	{
		Animator anime = human.body.Anime;
		anime.speed = 0f;
		if (act_Animes.Count == 0)
		{
			return;
		}
		CutAct_Anime cutAct_Anime = null;
		CutAct_Anime cutAct_Anime2 = null;
		time = Mathf.Clamp(time, act_Animes[0].time, act_Animes[act_Animes.Count - 1].time);
		for (int i = 0; i < act_Animes.Count; i++)
		{
			if (act_Animes[i].time <= time)
			{
				cutAct_Anime = act_Animes[i];
			}
			if (act_Animes[i].time >= time)
			{
				cutAct_Anime2 = act_Animes[i];
				break;
			}
		}
		if (cutAct_Anime == cutAct_Anime2)
		{
			anime.Play(cutAct_Anime.state, 0, 0f);
			return;
		}
		float num = Mathf.InverseLerp(cutAct_Anime.time, cutAct_Anime2.time, time);
		float num2 = (cutAct_Anime2.time - cutAct_Anime.time) * num;
		float normalizedTime = 0f;
		anime.Play(cutAct_Anime.state, 0, normalizedTime);
	}
}
