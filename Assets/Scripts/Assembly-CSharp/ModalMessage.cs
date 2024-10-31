using System;
using UnityEngine;
using UnityEngine.UI;

public class ModalMessage : ModalUI
{
	public Canvas myCanvas;

	public Text text;

	private Action<bool> yesAct;

	public AudioClip yesSE;

	public AudioClip noSE;

	public Toggle doNotAgainCheck;

	protected override void Awake()
	{
		base.Awake();
		doNotAgainCheck.onValueChanged.AddListener(ToggleSE);
	}

	private void Update()
	{
	}

	public void Yes()
	{
		bool obj = false;
		if (doNotAgainCheck.gameObject.activeSelf)
		{
			obj = doNotAgainCheck.isOn;
		}
		if (yesSE != null)
		{
			GameCtrl.audioCtrl.Play2DSE(yesSE);
		}
		if (yesAct != null)
		{
			yesAct(obj);
		}
		End();
	}

	public void SetUp(string text, Action<bool> yesAct, bool againCheck = false)
	{
		this.text.text = text;
		this.yesAct = yesAct;
		doNotAgainCheck.gameObject.SetActive(againCheck);
	}

	private void ToggleSE(bool flag)
	{
		AudioClip audioClip = ((!flag) ? noSE : yesSE);
		if (audioClip != null)
		{
			GameCtrl.audioCtrl.Play2DSE(audioClip);
		}
	}
}
