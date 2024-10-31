using System;
using UnityEngine;
using UnityEngine.UI;

public class CutActionEdit_Voice : CutActionEdit
{
	private CutAct_Voice actVoice;

	[SerializeField]
	private InputField charaInput;

	[SerializeField]
	private InputField voiceInput;

	public override void Setup(CutAction act)
	{
		actVoice = act as CutAct_Voice;
		charaInput.text = actVoice.chara;
		voiceInput.text = actVoice.voice;
	}

	public void SetChara(string str)
	{
		actVoice.chara = str;
	}

	public void SetVoice(string str)
	{
		actVoice.voice = str;
	}
}
