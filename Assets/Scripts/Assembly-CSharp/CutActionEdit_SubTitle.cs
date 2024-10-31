using System;
using UnityEngine;
using UnityEngine.UI;

public class CutActionEdit_SubTitle : CutActionEdit
{
	private CutAct_SubTitle actSubtitle;

	[SerializeField]
	private InputField input;

	public override void Setup(CutAction act)
	{
		actSubtitle = act as CutAct_SubTitle;
		input.text = actSubtitle.text;
	}

	public void SetText(string str)
	{
		actSubtitle.text = str;
	}
}
