using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CharaSaveUI : MonoBehaviour
{
	private enum MODE
	{
		STRING = 0,
		YESNO = 1
	}

	[SerializeField]
	private Image cardImage;

	[SerializeField]
	private GameObject inputRoot;

	[SerializeField]
	private GameObject yesNoRoot;

	[SerializeField]
	private InputField input;

	[SerializeField]
	private Button inputButtonSave;

	[SerializeField]
	private Button inputButtonCancel;

	[SerializeField]
	private Button yesNoButtonSave;

	[SerializeField]
	private Button yesNoButtonCancel;

	private Action<string> saveAct_new;

	private Action saveAct_overwrite;

	private Action cancelAct;

	private GameControl gameCtrl;

	private MODE mode;

	private bool invoke = true;

	private GameControl GameCtrl
	{
		get
		{
			if (gameCtrl == null)
			{
				gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
			}
			return gameCtrl;
		}
	}

	private void Awake()
	{
		input.onEndEdit.AddListener(InputString);
		inputButtonSave.onClick.AddListener(Button_SaveNew);
		inputButtonCancel.onClick.AddListener(Button_Cancel);
		yesNoButtonSave.onClick.AddListener(Button_SaveOverwrite);
		yesNoButtonCancel.onClick.AddListener(Button_Cancel);
	}

	public void Save_New(Texture2D captureTex, Action<string> saveAct, Action cancelAct)
	{
		saveAct_new = saveAct;
		this.cancelAct = cancelAct;
		mode = MODE.STRING;
		base.gameObject.SetActive(true);
		inputRoot.SetActive(true);
		yesNoRoot.SetActive(false);
		CardSprite(captureTex);
	}

	public void Save_Overwrite(Texture2D captureTex, Action saveAct, Action cancelAct)
	{
		saveAct_overwrite = saveAct;
		this.cancelAct = cancelAct;
		mode = MODE.YESNO;
		base.gameObject.SetActive(true);
		inputRoot.SetActive(false);
		yesNoRoot.SetActive(true);
		CardSprite(captureTex);
	}

	private void CardSprite(Texture2D tex)
	{
		Vector2 pNG_Size = CardFileList.PNG_Size;
		if (tex != null)
		{
			pNG_Size.x = tex.width;
			pNG_Size.y = tex.height;
		}
		cardImage.sprite = Sprite.Create(tex, new Rect(Vector2.zero, pNG_Size), pNG_Size * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
	}

	private void Button_SaveNew()
	{
		if (saveAct_new != null)
		{
			saveAct_new(input.text);
		}
		GameCtrl.audioCtrl.Play2DSE(GameCtrl.audioCtrl.systemSE_yes);
		Close();
	}

	private void Button_SaveOverwrite()
	{
		if (saveAct_overwrite != null)
		{
			saveAct_overwrite();
		}
		GameCtrl.audioCtrl.Play2DSE(GameCtrl.audioCtrl.systemSE_yes);
		Close();
	}

	private void Button_Cancel()
	{
		if (cancelAct != null)
		{
			cancelAct();
		}
		GameCtrl.audioCtrl.Play2DSE(GameCtrl.audioCtrl.systemSE_no);
		Close();
	}

	private void Close()
	{
		base.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (mode == MODE.STRING)
		{
			inputButtonSave.interactable = input.text.Length > 0;
		}
	}

	private void InputString(string str)
	{
		if (invoke)
		{
			CheckFileName();
		}
	}

	private void CheckFileName()
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
