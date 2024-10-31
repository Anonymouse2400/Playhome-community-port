using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneEdit_Command : MonoBehaviour
{
	public CutSceneEdit edit;

	[SerializeField]
	private Toggle commandOriginal;

	[SerializeField]
	private RectTransform commandsParent;

	[SerializeField]
	private Dropdown addCommandDropdown;

	[SerializeField]
	private Text timeText;

	private CutActionEdit[] cutActEdits = new CutActionEdit[17];

	private List<CutAction> selActs = new List<CutAction>();

	private List<Toggle> cmndTgls = new List<Toggle>();

	private static readonly string[] CutActNames = new string[17]
	{
		"スクリプトジャンプ", "カラーフィルター", "イメージ", "BGM", "SE", "音声", "字幕", "アニメ", "表情", "カメラ",
		"位置", "キャラ表示", "IK", "ライト", "次のシーン", "回想終了", "ゲーム変数"
	};

	public int ActiveCommandNo { get; private set; }

	public float SelectTime { get; private set; }

	private void Awake()
	{
		cutActEdits[1] = GetComponentInChildren<CutActionEdit_ColorFilter>(true);
		cutActEdits[5] = GetComponentInChildren<CutActionEdit_Voice>(true);
		cutActEdits[6] = GetComponentInChildren<CutActionEdit_SubTitle>(true);
		cutActEdits[7] = GetComponentInChildren<CutActionEdit_Anime>(true);
		cutActEdits[9] = GetComponentInChildren<CutActionEdit_Camera>(true);
		addCommandDropdown.ClearOptions();
		for (int i = 0; i < 17; i++)
		{
			addCommandDropdown.options.Add(new Dropdown.OptionData(CutActNames[i]));
		}
		addCommandDropdown.value = -1;
		ActiveCommandNo = -1;
		SelectTime = -1f;
	}

	public void SetList(float time)
	{
		SelectTime = time;
		selActs.Clear();
		foreach (CutAction action in edit.cutScene.Actions)
		{
			if (action.time == time)
			{
				selActs.Add(action);
			}
		}
		for (int i = 0; i < cmndTgls.Count; i++)
		{
            UnityEngine.Object.Destroy(cmndTgls[i].gameObject);
		}
		cmndTgls.Clear();
		foreach (CutAction selAct in selActs)
		{
			Toggle toggle = UnityEngine.Object.Instantiate(commandOriginal);
			toggle.gameObject.SetActive(true);
			Text componentInChildren = toggle.GetComponentInChildren<Text>();
			componentInChildren.text = CutActNames[(int)selAct.Type];
			toggle.transform.SetParent(commandsParent);
			cmndTgls.Add(toggle);
		}
		ActiveCommandNo = -1;
		Select(true);
		timeText.text = time.ToString("000.00");
	}

	public void ClearList()
	{
		selActs.Clear();
		for (int i = 0; i < cmndTgls.Count; i++)
		{
            UnityEngine.Object.Destroy(cmndTgls[i].gameObject);
		}
		cmndTgls.Clear();
		AllEditClose();
	}

	private void Update()
	{
		if (selActs.Count != 0)
		{
			Select(false);
		}
	}

	private void Select(bool forceUpdate)
	{
		int num = -1;
		for (int i = 0; i < cmndTgls.Count; i++)
		{
			if (cmndTgls[i].isOn)
			{
				num = i;
				break;
			}
		}
		if (ActiveCommandNo == num && !forceUpdate)
		{
			return;
		}
		ActiveCommandNo = num;
		CUTACT cUTACT = CUTACT.UNKNOWN;
		if (ActiveCommandNo != -1)
		{
			cUTACT = selActs[ActiveCommandNo].Type;
		}
		for (int j = 0; j < 17; j++)
		{
			if (!(cutActEdits[j] == null))
			{
				if (j == (int)cUTACT)
				{
					cutActEdits[j].gameObject.SetActive(true);
					cutActEdits[j].Setup(selActs[ActiveCommandNo]);
				}
				else
				{
					cutActEdits[j].gameObject.SetActive(false);
				}
			}
		}
	}

	private void AllEditClose()
	{
		for (int i = 0; i < 17; i++)
		{
			if ((bool)cutActEdits[i])
			{
				cutActEdits[i].gameObject.SetActive(false);
			}
		}
	}

	public void AddCommand()
	{
		float nowTime = edit.timeBar.nowTime;
		CUTACT value = (CUTACT)addCommandDropdown.value;
		edit.cutScene.AddAction(value, nowTime);
		edit.timeBar.ReputKeys(true);
		edit.timeBar.SetNowTime(nowTime);
	}

	public void DeleteCommand()
	{
		edit.cutScene.DeleteAction(selActs[ActiveCommandNo]);
		ActiveCommandNo = -1;
		ClearList();
		edit.timeBar.ReputKeys(true);
		AllEditClose();
		SetList(SelectTime);
	}
}
