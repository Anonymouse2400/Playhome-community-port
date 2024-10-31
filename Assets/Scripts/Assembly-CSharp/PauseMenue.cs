using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenue : MonoBehaviour
{
	private GameControl gameCtrl;

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private Button buttonConfig;

	[SerializeField]
	private Button buttonTitle;

	[SerializeField]
	private Button buttonExit;

	[SerializeField]
	private Button buttonClose;

	private Action openConfig;

	private Action titleAct;

	private Action exitAct;

	public bool EnableConfig
	{
		get
		{
			return buttonConfig.gameObject.activeSelf;
		}
		set
		{
			buttonConfig.gameObject.SetActive(value);
		}
	}

	public bool EnableTitle
	{
		get
		{
			return buttonTitle.gameObject.activeSelf;
		}
		set
		{
			buttonTitle.gameObject.SetActive(value);
		}
	}

	public bool IsOpen
	{
		get
		{
			return canvas.enabled;
		}
	}

	private void Awake()
	{
		gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
		buttonConfig.onClick.AddListener(OnClick_Config);
		buttonTitle.onClick.AddListener(OnClick_Title);
		buttonExit.onClick.AddListener(OnClick_Exit);
		buttonClose.onClick.AddListener(OnClick_Close);
		canvas.enabled = false;
	}

	public void Setup(bool enableConfig, bool enableTitle, Action openConfig, Action titleAct, Action exitAct)
	{
		this.openConfig = openConfig;
		this.titleAct = titleAct;
		this.exitAct = exitAct;
		buttonConfig.gameObject.SetActive(enableConfig);
		buttonTitle.gameObject.SetActive(enableTitle);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !gameCtrl.IsActiveModalUI)
		{
			if (IsOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}
		if (Input.GetKeyDown(KeyCode.F1) && !gameCtrl.IsActiveModalUI && EnableConfig)
		{
			openConfig();
		}
	}

	public void Open()
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_open);
		canvas.enabled = true;
	}

	public void Close()
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_close);
		canvas.enabled = false;
	}

	private void OnEnable()
	{
	}

	private void OnClick_Config()
	{
		if (openConfig != null)
		{
			canvas.enabled = false;
			openConfig();
		}
	}

	private void OnClick_Title()
	{
		if (titleAct != null)
		{
			titleAct();
			return;
		}
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		gameCtrl.CreateModalYesNoUI("Return to title。\nAre you sure?", ToTitle);
	}

	private void ToTitle()
	{
		gameCtrl.ChangeScene("TitleScene", string.Empty, 1f);
		canvas.enabled = false;
	}

	private void OnClick_Exit()
	{
		if (exitAct != null)
		{
			exitAct();
			return;
		}
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		gameCtrl.CreateModalYesNoUI("exit the game。\nAre you sure?", ToExit);
	}

	private void ToExit()
	{
		gameCtrl.ChangeScene("ExitScene", string.Empty, 1f);
		canvas.enabled = false;
	}

	private void OnClick_Close()
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_close);
		canvas.enabled = false;
	}
}
