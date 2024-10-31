using System;
using UnityEngine;
using UnityEngine.UI;

public class CutActionEdit_Anime : CutActionEdit
{
	private CutAct_Anime actAnime;

	[SerializeField]
	private InputField charaInput;

	[SerializeField]
	private InputField stateInput;

	[SerializeField]
	private InputField durationInput;

	public override void Setup(CutAction act)
	{
		actAnime = act as CutAct_Anime;
		charaInput.text = actAnime.chara;
		stateInput.text = actAnime.state;
		durationInput.text = actAnime.duration.ToString();
	}

	public void SetChara(string str)
	{
		actAnime.chara = str;
	}

	public void SetState(string str)
	{
		actAnime.state = str;
	}

	public void SetDuration(string str)
	{
		try
		{
			actAnime.duration = float.Parse(str);
		}
		catch
		{
		}
	}
}
