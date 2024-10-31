using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadMode : MonoBehaviour
{
	public enum MODE
	{
		NONE = 0,
		SAVE = 1,
		LOAD = 2
	}

	public enum UICOLOR_TYPE
	{
		BASE = 0,
		HIGH = 1,
		PRESSED = 2,
		DISABLE = 3,
		NUM = 4
	}

	[SerializeField]
	private MODE mode;

	[SerializeField]
	private Color uiColorSave;

	[SerializeField]
	private Color uiColorLoad;

	[SerializeField]
	private ColorBlock colorsSaveOn;

	[SerializeField]
	private ColorBlock colorsSaveOff;

	[SerializeField]
	private ColorBlock colorsLoadOn;

	[SerializeField]
	private ColorBlock colorsLoadOff;

	[SerializeField]
	private Image titleBG;

	[SerializeField]
	private Text titleText;

	[SerializeField]
	private Text exeText;

	[SerializeField]
	private RadioButtonGroup mainRoot;

	[SerializeField]
	private RadioButtonGroup pageRoot;

	[SerializeField]
	private Button exeButton;

	[SerializeField]
	private Button delButton;

	private SaveHeader[] datas = new SaveHeader[50];

	private GameControl GC;

	private void Start()
	{
		GC = UnityEngine.Object.FindObjectOfType<GameControl>();
		if (mode != 0)
		{
			Setup(mode);
		}
	}

	public void Setup(MODE mode)
	{
		this.mode = mode;
		titleBG.color = ((mode != MODE.SAVE) ? uiColorLoad : uiColorSave);
		titleText.text = ((mode != MODE.SAVE) ? "ロード" : "セーブ");
		exeText.text = titleText.text;
		ToggleButton[] toggleButtons = pageRoot.ToggleButtons;
		ColorBlock onColor = ((mode != MODE.SAVE) ? colorsLoadOn : colorsSaveOn);
		ColorBlock offColor = ((mode != MODE.SAVE) ? colorsLoadOff : colorsSaveOff);
		for (int i = 0; i < toggleButtons.Length; i++)
		{
			toggleButtons[i].SetColor(onColor, offColor);
		}
		DataListup();
		ChangePage();
	}

	private void ChangePage()
	{
		int value = pageRoot.Value;
		ColorBlock onColor = ((mode != MODE.SAVE) ? colorsLoadOn : colorsSaveOn);
		ColorBlock offColor = ((mode != MODE.SAVE) ? colorsLoadOff : colorsSaveOff);
		ToggleButton[] componentsInChildren = mainRoot.GetComponentsInChildren<ToggleButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetColor(onColor, offColor);
			int num = value * 10 + i;
			SaveHeader saveHeader = datas[num];
			Text component = componentsInChildren[i].transform.FindChild("Text Name").GetComponent<Text>();
			Text component2 = componentsInChildren[i].transform.FindChild("Text No").GetComponent<Text>();
			Text component3 = componentsInChildren[i].transform.FindChild("Text Time").GetComponent<Text>();
			component2.text = "No:" + (num + 1).ToString("00");
			component.text = ((saveHeader == null) ? "データがありません" : saveHeader.comment);
			component3.text = ((saveHeader == null) ? string.Empty : saveHeader.time);
			bool interactable = true;
			if (mode == MODE.LOAD)
			{
				interactable = datas[num] != null;
			}
			componentsInChildren[i].Interactable = interactable;
		}
		mainRoot.Value = -1;
	}

	private void Update()
	{
		int num = NowListNo();
		exeButton.interactable = num != -1;
		delButton.interactable = num != -1 && datas[num] != null;
	}

	private void DataListup()
	{
		for (int i = 0; i < 50; i++)
		{
			datas[i] = SaveHeader.LoadSaveHeader(GameSavePath(i));
		}
	}

	public static string GameSavePath(int no)
	{
		//return Directory.GetCurrentDirectory() + "/UserData/save/Game/" + (no + 1).ToString("00") + ".gsd";

        return Application.persistentDataPath + "/UserData/save/Game/" + (no + 1).ToString("00") + ".gsd";
    }

	private void Save()
	{
		int num = NowListNo();
		string def = ((datas[num] == null) ? string.Empty : datas[num].comment);
		GC.CreateModalInputStringUI(false, "コメントを入力してください", SaveExe, null, true, def);
	}

	public void SaveOverWrite()
	{
		string text = "セーブデータ" + (NowListNo() + 1).ToString("00") + "を上書き保存しますか？";
		GC.CreateModalYesNoUI(text, Save);
	}

	private void SaveExe(string comment)
	{
		int num = NowListNo();
		string path = GameSavePath(num);
		GlobalData.PlayData.Save(path, comment);
		datas[num] = new SaveHeader(GlobalData.PlayData.Header);
		GlobalData.continueSaveNo = num;
		ChangePage();
	}

	private void Load()
	{
		string path = GameSavePath(NowListNo());
		GlobalData.PlayData.Load(path);
		GC.ChangeScene("SelectScene", "Load", 1f);
	}

	private void DeleteExe()
	{
		int num = NowListNo();
		string path = GameSavePath(num);
		File.Delete(path);
		datas[num] = null;
		if (GlobalData.continueSaveNo == num)
		{
			GlobalData.continueSaveNo = -1;
		}
		ChangePage();
	}

	private int NowListNo()
	{
		int value = pageRoot.Value;
		int value2 = mainRoot.Value;
		if (value2 != -1)
		{
			return value * 10 + value2;
		}
		return -1;
	}

	public void Radio_ChangePage(int no)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		ChangePage();
	}

	public void Button_Exe()
	{
		int num = NowListNo();
		if (num == -1)
		{
			return;
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
		if (mode == MODE.SAVE)
		{
			if (datas[num] == null)
			{
				Save();
			}
			else
			{
				SaveOverWrite();
			}
		}
		else if (mode == MODE.LOAD)
		{
			string text = "セーブデータ" + (num + 1).ToString("00") + "を読込みますか？";
			GC.CreateModalYesNoUI(text, Load);
		}
	}

	public void Button_Del()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		string text = "セーブデータ" + (NowListNo() + 1).ToString("00") + "を削除しますか？";
		GC.CreateModalYesNoUI(text, DeleteExe);
	}

	public void Button_Return()
	{
		SystemSE.Play(SystemSE.SE.CLOSE);
		base.gameObject.SetActive(false);
	}

	public void Radio_Main(int no)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
	}
}
