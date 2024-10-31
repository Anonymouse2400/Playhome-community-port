using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
	public enum STATE
	{
		NONE = 0
	}

	public Color FadeColor = Color.black;

	public float FadeTime = 0.5f;

	public int TargetFPS = -1;

	public SceneControl sceneCtrl;

	public AudioControl audioCtrl;

	public Canvas fpsCanvas;

	public ModalYesNo modalYesNo_original;

	public ModalMessage modalMessage_original;

	public ModalInput modalInput_original;

	public ModalChoices modalChoices_original;

	private ModalUI modalUI;

	[SerializeField]
	private float timeScale = 1f;

	public Material mirrorDummy;

	//public string installRegistry = "Software\\illusion\\PlayHome";

	public bool IsHideUI { get; set; }

	public bool IsWhiteMoz { get; private set; }

	public int State { get; private set; }

	public bool ShowFPS
	{
		get
		{
			return fpsCanvas != null && fpsCanvas.gameObject.activeInHierarchy;
		}
		set
		{
			if (fpsCanvas != null)
			{
				fpsCanvas.gameObject.SetActive(value);
			}
		}
	}

	public bool IsActiveModalUI
	{
		get
		{
			return modalUI != null;
		}
	}

	private void Awake()
	{
		//InstallCheck();
		Time.timeScale = timeScale;
		//if (TargetFPS > 0)
		//{
		//	Application.targetFrameRate = TargetFPS;
		//}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        //if (QualitySettings.GetQualityLevel() == 0)
        //{
        //	Shader.globalMaximumLOD = 500;
        //}
        //else
        //{
        //	Shader.globalMaximumLOD = int.MaxValue;
        //}
        Application.targetFrameRate = TargetFPS;
        Shader.globalMaximumLOD = 500;
        Shader.WarmupAllShaders();
		sceneCtrl = new SceneControl(Application.loadedLevelName);
        IsHideUI = false;
		IsWhiteMoz = false;
		State = 0;
	}

	private void Start()
	{
	}

	private void Update()
	{
		ShowFPS = ConfigData.showFPS;
		Update_Key();
	}

	private void Update_Key()
	{
		if ((!(EventSystem.current != null) || !(EventSystem.current.currentSelectedGameObject != null)) && Input.GetKeyDown(KeyCode.Space))
		{
			IsHideUI = !IsHideUI;
		}
	}

	public void CreateModalMessageUI(string text, Action<bool> yesAct = null, bool againCheck = false)
	{
		ModalMessage modalMessage = UnityEngine.Object.Instantiate(modalMessage_original);
		modalMessage.gameObject.SetActive(true);
		modalMessage.SetUp(text, yesAct, againCheck);
		modalUI = modalMessage;
	}

	public void CreateModalYesNoUI(string text, Action yesAct, Action noAct = null)
	{
		ModalYesNo modalYesNo = UnityEngine.Object.Instantiate(modalYesNo_original);
		modalYesNo.gameObject.SetActive(true);
		modalYesNo.SetUp(text, yesAct, noAct);
		modalUI = modalYesNo;
	}

	public void CreateModalInputStringUI(bool isFileName, string text, Action<string> enterAct, Action cancelAct = null, bool enableBlank = false, string def = "")
	{
		ModalInput modalInput = UnityEngine.Object.Instantiate(modalInput_original);
		modalInput.gameObject.SetActive(true);
		modalInput.SetUp(isFileName, text, enterAct, cancelAct, enableBlank, def);
		modalUI = modalInput;
	}

	public void CreateModalChoices(string text, string[] choices, Action[] acts)
	{
		ModalChoices modalChoices = UnityEngine.Object.Instantiate(modalChoices_original);
		modalChoices.gameObject.SetActive(true);
		modalChoices.SetUp(text, choices, acts);
		modalUI = modalChoices;
	}

	public void OnModelClose()
	{
		modalUI = null;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
	}

	private void OnApplicationQuit()
	{
	}

	public void AddState(STATE state)
	{
		State |= (int)state;
	}

	public void SubState(STATE state)
	{
		if (((uint)State & (uint)state) != 0)
		{
			State ^= (int)state;
		}
	}

	public bool CheckState(STATE state)
	{
		return ((uint)State & (uint)state) != 0;
	}

	private void OnValidate()
	{
		if (timeScale > 0f)
		{
			Time.timeScale = timeScale;
		}
	}

	public void ChangeScene(string scene, string msg = "", float fadeTime = -1f)
	{
		if (fadeTime < 0f)
		{
			fadeTime = FadeTime;
		}
		sceneCtrl.Change(scene, msg, FadeColor, fadeTime);
	}

	public bool CheckInput()
	{
		foreach (Selectable allSelectable in Selectable.allSelectables)
		{
			InputField inputField = allSelectable as InputField;
			if ((UnityEngine.Object)(object)inputField != null && inputField.isFocused)
			{
				return false;
			}
		}
		return true;
	}

	private void InstallCheck()
	{
		Directory.SetCurrentDirectory(Application.persistentDataPath + "/");
	}
}
