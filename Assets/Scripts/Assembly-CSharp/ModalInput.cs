using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ModalInput : ModalUI
{
	public Canvas myCanvas;

	public Text text;

	public InputField input;

	public Button enterButton;

	public Button cancelButton;

	private Action<string> enterAct;

	private Action cancelAct;

	public AudioClip enterSE;

	public AudioClip cancelSE;

	private bool isFileName;

	private bool invoke = true;

	private bool enableBlank;

	protected override void Awake()
	{
		base.Awake();
		enterButton.onClick.AddListener(Enter);
		cancelButton.onClick.AddListener(Cancel);
	}

	private void Update()
	{
		enterButton.interactable = enableBlank || input.text.Length > 0;
	}

	private void Enter()
	{
		if (enableBlank || input.text.Length != 0)
		{
			if (enterSE != null)
			{
				GameCtrl.audioCtrl.Play2DSE(enterSE);
			}
			if (enterAct != null)
			{
				enterAct(input.text);
			}
			End();
		}
	}

	private void Cancel()
	{
		if (cancelSE != null)
		{
			GameCtrl.audioCtrl.Play2DSE(cancelSE);
		}
		if (cancelAct != null)
		{
			cancelAct();
		}
		End();
	}

	public void SetUp(bool isFileName, string text, Action<string> enterAct, Action cancelAct, bool enableBlank, string def)
	{
		this.isFileName = isFileName;
		this.text.text = text;
		this.enterAct = enterAct;
		this.cancelAct = cancelAct;
		this.enableBlank = enableBlank;
		input.text = def;
	}

	public void InputString(string str)
	{
		if (invoke && isFileName)
		{
			CheckFileName();
		}
	}

	private void CheckFileName()
	{
		if (isFileName)
		{
			string text = input.text;
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			char[] array = invalidFileNameChars;
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				text = text.Replace(c.ToString(), string.Empty);
			}
			invoke = false;
			input.text = text;
			invoke = true;
		}
	}
}
