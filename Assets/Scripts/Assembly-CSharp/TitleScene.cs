using System;
using System.Collections;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : Scene
{
	private enum BUTTONS
	{
		CONTINUE = 0,
		START = 1,
		LOAD = 2,
		CONFIG = 3,
		EXIT = 4,
		NUM = 5
	}

	public float fadeInTime = 1f;

	public float titleCallTime = 2f;

	private float timer;

	private bool titleCallEnable = true;

	[SerializeField]
	private AudioSource audio;

	[SerializeField]
	private Canvas uiCanvas;

	[SerializeField]
	private Button[] buttons = new Button[5];

	[SerializeField]
	private Config configOriginal;

	private Config config;

	[SerializeField]
	private SaveLoadMode save_loadOriginal;

	private SaveLoadMode save_load;

	[SerializeField]
	private PauseMenue pauseMenueOriginal;

	private PauseMenue pauseMenue;

	private string bgm = "Title";

	private void Start()
	{
		GlobalData.isMemory = false;
		config = UnityEngine.Object.Instantiate(configOriginal);
		config.gameObject.SetActive(false);
		save_load = UnityEngine.Object.Instantiate(save_loadOriginal);
		save_load.gameObject.SetActive(false);
		InScene(false);
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].gameObject.SetActive(false);
		}
		base.GC.audioCtrl.BGM_LoadAndPlay(bgm, false, true);
		pauseMenue = UnityEngine.Object.Instantiate(pauseMenueOriginal);
		pauseMenue.Setup(true, false, Button_Config, null, null);
		ScreenFade.StartFade(ScreenFade.TYPE.IN, base.GC.FadeColor, fadeInTime);
		StartCoroutine(ShowButtons());
	}

	private void Update()
	{
		if (titleCallEnable && timer >= titleCallTime)
		{
			TitleCall();
		}
		timer += Time.deltaTime;
		bool enableConfig = !config.isActiveAndEnabled && !save_load.isActiveAndEnabled;
		uiCanvas.enabled = enableConfig;
		pauseMenue.EnableConfig = enableConfig;
	}

	public void Button_Start()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_yes);
		GlobalData.PlayData.Start();
		string msg = EditScene.CreateMessage("Ritsuko", "GameStart");
		base.GC.CreateModalYesNoUI("Customize your character before starting the gameか？", delegate
		{
			base.GC.ChangeScene("EditScene", msg, 1f);
		}, StartADVScene);
	}

	private void StartADVScene()
	{
		base.GC.ChangeScene("ADVScene", "adv/adv_00_00,ADV_Script_00_00", 1f);
	}

	public void Button_Load()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_open);
		save_load.Setup(SaveLoadMode.MODE.LOAD);
		save_load.gameObject.SetActive(true);
	}

	public void Button_Continue()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_yes);
		string continueSaveFile = GlobalData.GetContinueSaveFile();
		GlobalData.PlayData.Load(continueSaveFile);
		base.GC.ChangeScene("SelectScene", "Load", 1f);
	}

	public void Button_Config()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_open);
		config.gameObject.SetActive(true);
	}

	public void Button_DemoMovie()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_yes);
		base.GC.ChangeScene("DemoMovie", string.Empty, 1f);
	}

	public void Button_Uploader()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_yes);
		base.GC.ChangeScene("UpDownLoader", string.Empty, 1f);
	}

	public void Button_Exit()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_yes);
		base.GC.CreateModalYesNoUI("Exit the game. \nAre you sure?", ExitGame);
	}

	private void ExitGame()
	{
		base.GC.ChangeScene("ExitScene", string.Empty, 1f);
	}

	private void TitleCall()
	{
		titleCallEnable = false;
		string[] array = new string[2] { "A", "B" };
		int num = UnityEngine.Random.Range(0, 3);
		HEROINE heroineID = (HEROINE)num;
		int num2 = (GlobalData.flipflop ? UnityEngine.Random.Range(0, 2) : 0);
		string path = "SystemVoice/TitleCall/TitleCall_F" + num.ToString("00") + "_" + array[num2];
		audio.clip = Resources.Load<AudioClip>(path);
		audio.volume = ConfigData.VolumeVoice_Heroine(heroineID);
		audio.pitch = ConfigData.PitchVoice_Heroine(heroineID);
		audio.Play();
	}

	private void Android()
	{
		if (!Application.isMobilePlatform)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();

		}

	}

	private IEnumerator ShowButtons()
	{
		yield return new WaitForSeconds(1.5f);
		if (GlobalData.CheckContinueSave())
		{
			buttons[0].gameObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
		}
		for (int i = 1; i < buttons.Length; i++)
		{
			buttons[i].gameObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
