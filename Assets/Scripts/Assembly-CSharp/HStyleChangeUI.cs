using System;
using System.Collections.Generic;
using Character;
using H;
using UnityEngine;
using UnityEngine.UI;

public class HStyleChangeUI : MonoBehaviour
{
	[SerializeField]
	private ToggleButton mainToggle;

	[SerializeField]
	private ToggleButton[] genreToggles = new ToggleButton[3];

	[SerializeField]
	private ToggleButton[] stateToggles = new ToggleButton[3];

	[SerializeField]
	private GameObject hideableRoot;

	[SerializeField]
	private RectTransform selectsRoot;

	[SerializeField]
	private Toggle selectOriginal;

	private Toggle[] selects;

	[SerializeField]
	private RadioButtonGroup middleLeftGroup;

	private Dictionary<string, H_StyleData> stylesDictionary;

	private List<H_StyleData> styles_inset = new List<H_StyleData>();

	private List<H_StyleData> styles_service = new List<H_StyleData>();

	private List<H_StyleData> styles_petting = new List<H_StyleData>();

	private bool invoke = true;

	private H_StyleData.TYPE nowType;

	private H_StyleData.STATE nowState = H_StyleData.STATE.UNKNOWN;

	private List<H_StyleData> nowStyles = new List<H_StyleData>();

	private H_Scene h_scene;

	public bool Interactable
	{
		get
		{
			return mainToggle.Interactable;
		}
		set
		{
			mainToggle.Interactable = value;
			if (!mainToggle.Interactable && hideableRoot.gameObject.activeSelf)
			{
				hideableRoot.gameObject.SetActive(false);
			}
			else if (mainToggle.Interactable && mainToggle.Value && !hideableRoot.gameObject.activeSelf)
			{
				hideableRoot.gameObject.SetActive(true);
			}
		}
	}

	private void Awake()
	{
		mainToggle.ActionAddListener(OnChangeMainToggle);
		genreToggles[0].ActionAddListener(OnChangeGenre_Insert);
		genreToggles[1].ActionAddListener(OnChangeGenre_Service);
		genreToggles[2].ActionAddListener(OnChangeGenre_Petting);
		stateToggles[0].ActionAddListener(OnChangeGenre_Resist);
		stateToggles[1].ActionAddListener(OnChangeGenre_Flop);
		stateToggles[2].ActionAddListener(OnChangeGenre_Weakness);
		hideableRoot.SetActive(false);
	}

	public void Setup(H_Scene h_scene, Dictionary<string, H_StyleData> dictionary)
	{
		this.h_scene = h_scene;
		stylesDictionary = dictionary;
		foreach (H_StyleData value in dictionary.Values)
		{
			if (value.type == H_StyleData.TYPE.INSERT)
			{
				styles_inset.Add(value);
			}
			else if (value.type == H_StyleData.TYPE.SERVICE)
			{
				styles_service.Add(value);
			}
			else if (value.type == H_StyleData.TYPE.PETTING)
			{
				styles_petting.Add(value);
			}
		}
		if (h_scene.mainMembers != null)
		{
			Female female = h_scene.mainMembers.GetFemale(0);
			bool active = GlobalData.PlayData.Progress >= GamePlayData.PROGRESS.ALL_FREE;
			if (female.personality.state == Personality.STATE.FIRST || female.personality.state == Personality.STATE.FLIP_FLOP || female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_1 || female.personality.state == Personality.STATE.LAST_EVENT_SISTERS || female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_2)
			{
				active = false;
			}
			for (int i = 0; i < stateToggles.Length; i++)
			{
				stateToggles[i].gameObject.SetActive(active);
			}
			if (female.personality.weakness)
			{
				nowState = H_StyleData.STATE.WEAKNESS;
			}
			else if (!female.IsFloped())
			{
				nowState = H_StyleData.STATE.RESIST;
			}
			else
			{
				nowState = H_StyleData.STATE.FLOP;
			}
		}
		invoke = false;
		stateToggles[(int)nowState].ChangeValue(true, false);
		genreToggles[0].ChangeValue(true, false);
		ChangeGenre(H_StyleData.TYPE.INSERT);
		invoke = true;
	}

	private void UpdateState()
	{
		if (h_scene.mainMembers == null)
		{
			return;
		}
		Female female = h_scene.mainMembers.GetFemale(0);
		bool flag = GlobalData.PlayData.Progress >= GamePlayData.PROGRESS.ALL_FREE;
		if (female.personality.state == Personality.STATE.FIRST || female.personality.state == Personality.STATE.FLIP_FLOP || female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_1 || female.personality.state == Personality.STATE.LAST_EVENT_SISTERS || female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_2)
		{
			flag = false;
		}
		if (!flag)
		{
			if (female.personality.weakness)
			{
				nowState = H_StyleData.STATE.WEAKNESS;
			}
			else if (!female.IsFloped())
			{
				nowState = H_StyleData.STATE.RESIST;
			}
			else
			{
				nowState = H_StyleData.STATE.FLOP;
			}
		}
	}

	public void UpdateList()
	{
		UpdateState();
		ChangeGenre(nowType);
	}

	private void ChangeGenre(H_StyleData.TYPE type)
	{
		nowType = type;
		ChangeList();
	}

	private void ChangeState(H_StyleData.STATE state)
	{
		nowState = state;
		ChangeList();
	}

	private void ChangeList()
	{
		if (selects != null)
		{
			for (int i = 0; i < selects.Length; i++)
			{
                UnityEngine.Object.Destroy(selects[i].gameObject);
			}
			selects = null;
		}
		List<H_StyleData> list = null;
		if (nowType == H_StyleData.TYPE.INSERT)
		{
			list = styles_inset;
		}
		else if (nowType == H_StyleData.TYPE.SERVICE)
		{
			list = styles_service;
		}
		else if (nowType == H_StyleData.TYPE.PETTING)
		{
			list = styles_petting;
		}
		nowStyles.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			if (StyleCheck(list[j]))
			{
				nowStyles.Add(list[j]);
			}
		}
		selects = new Toggle[nowStyles.Count];
		for (int k = 0; k < nowStyles.Count; k++)
		{
			selects[k] = UnityEngine.Object.Instantiate(selectOriginal);
			selects[k].onValueChanged.AddListener(OnChangeSelect);
			selects[k].gameObject.SetActive(true);
			selects[k].transform.SetParent(selectsRoot, false);
			selects[k].GetComponentInChildren<Text>().text = nowStyles[k].name;
		}
		selectsRoot.anchoredPosition = Vector2.zero;
	}

	private bool StyleCheck(H_StyleData data)
	{
		Female female = h_scene.mainMembers.GetFemale(0);
		if (female.personality.state == Personality.STATE.FIRST || female.personality.state == Personality.STATE.FLIP_FLOP || female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_1 || female.personality.state == Personality.STATE.LAST_EVENT_SISTERS || female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_2)
		{
			return StyleCheck_Event(female, data);
		}
		if (data.member == H_StyleData.MEMBER.M1F2)
		{
			if (GlobalData.PlayData.Progress < GamePlayData.PROGRESS.ALL_FREE)
			{
				return false;
			}
			if (h_scene.visitor == null || h_scene.visitor.GetHuman().sex == SEX.MALE)
			{
				return false;
			}
		}
		if (((uint)data.detailFlag & 0x800u) != 0 && GlobalData.PlayData.Progress < GamePlayData.PROGRESS.ALL_FREE)
		{
			return false;
		}
		if (data.state != nowState)
		{
			return false;
		}
		if (data.map.Length > 0 && h_scene.map.name.IndexOf(data.map) != 0)
		{
			return false;
		}
		if (!h_scene.CheckEnableStyle(data.id))
		{
			return false;
		}
		return true;
	}

	private bool StyleCheck_Event(Female female, H_StyleData data)
	{
		if (female.personality.weakness && data.state != H_StyleData.STATE.WEAKNESS)
		{
			return false;
		}
		if (!female.personality.weakness && data.state == H_StyleData.STATE.WEAKNESS)
		{
			return false;
		}
		if (female.personality.state == Personality.STATE.FIRST)
		{
			string[,] array = new string[3, 8]
			{
				{ "HS_00_00_08", "HS_01_00_06", "HH_01_00_01", "HA_00_00_00", "HS_00_02_08", "HS_01_02_06", "HH_01_02_01", "HA_00_02_00" },
				{ "HS_00_00_00", "HS_01_00_00", "HH_01_00_00", "HA_00_00_01", "HS_00_02_00", "HS_01_02_00", "HH_01_02_00", "HA_00_02_01" },
				{ "HS_00_00_02", "HS_01_00_03", "HH_03_00_02", "HA_01_00_00", "HS_00_02_02", "HS_01_02_03", "HH_03_02_02", "HA_01_02_00" }
			};
			int heroineID = (int)female.HeroineID;
			for (int i = 0; i < 8; i++)
			{
				if (data.id == array[heroineID, i])
				{
					return true;
				}
			}
			return false;
		}
		if (female.personality.state == Personality.STATE.FLIP_FLOP)
		{
			string[,] array2 = new string[3, 8]
			{
				{ "HS_00_01_00", "HS_01_01_06", "HH_01_00_00", "HA_01_00_03", "HS_00_02_00", "HS_01_02_06", "HH_01_02_00", "HA_01_02_03" },
				{ "HS_00_00_01", "HS_01_00_06", "HH_01_00_01", "HA_00_00_00", "HS_00_02_01", "HS_01_02_06", "HH_01_02_01", "HA_00_02_00" },
				{ "HS_00_01_01", "HS_01_01_00", "HH_00_01_00", "HA_00_01_02", "HS_00_02_01", "HS_01_02_00", "HH_00_02_00", "HA_00_02_02" }
			};
			int heroineID2 = (int)female.HeroineID;
			for (int j = 0; j < 8; j++)
			{
				if (data.id == array2[heroineID2, j])
				{
					return true;
				}
			}
			return false;
		}
		if (female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_1)
		{
			string[] array3 = new string[8] { "HS_00_01_06", "HS_01_01_03", "HH_02_01_00", "HA_00_01_01", "HS_00_02_06", "HS_01_02_03", "HH_02_02_00", "HA_00_02_01" };
			for (int k = 0; k < array3.Length; k++)
			{
				if (data.id == array3[k])
				{
					return true;
				}
			}
			return false;
		}
		if (female.personality.state == Personality.STATE.LAST_EVENT_SISTERS)
		{
			string[] array4 = new string[2] { "HW_00_01_00", "HW_00_02_00" };
			for (int l = 0; l < array4.Length; l++)
			{
				if (data.id == array4[l])
				{
					return true;
				}
			}
			return false;
		}
		if (female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_2)
		{
			string[] array5 = new string[6] { "HS_10_01_00", "HH_10_01_00", "HA_10_01_00", "HS_10_02_00", "HH_10_02_00", "HA_10_02_00" };
			for (int m = 0; m < array5.Length; m++)
			{
				if (data.id == array5[m])
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public void Close()
	{
		if (middleLeftGroup.Value == 0)
		{
			mainToggle.ChangeValue(false, false);
			middleLeftGroup.Change(-1, false);
		}
	}

	private void OnChangeMainToggle(bool change)
	{
		hideableRoot.SetActive(change);
		if (change)
		{
			UpdateList();
		}
	}

	private void OnChangeGenre_Insert(bool change)
	{
		if (invoke && change)
		{
			ChangeGenre(H_StyleData.TYPE.INSERT);
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void OnChangeGenre_Service(bool change)
	{
		if (invoke && change)
		{
			ChangeGenre(H_StyleData.TYPE.SERVICE);
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void OnChangeGenre_Petting(bool change)
	{
		if (invoke && change)
		{
			ChangeGenre(H_StyleData.TYPE.PETTING);
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void OnChangeGenre_Resist(bool change)
	{
		if (invoke && change)
		{
			ChangeState(H_StyleData.STATE.RESIST);
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void OnChangeGenre_Flop(bool change)
	{
		if (invoke && change)
		{
			ChangeState(H_StyleData.STATE.FLOP);
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void OnChangeGenre_Weakness(bool change)
	{
		if (invoke && change)
		{
			ChangeState(H_StyleData.STATE.WEAKNESS);
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void OnChangeSelect(bool flag)
	{
		if (!invoke || !flag)
		{
			return;
		}
		for (int i = 0; i < selects.Length; i++)
		{
			if (selects[i].isOn)
			{
				ChangeSelect(i);
				break;
			}
		}
	}

	private void ChangeSelect(int no)
	{
		ScreenFade.StartFade(ScreenFade.TYPE.OUT_IN, Color.black, 0.5f, 0f, delegate
		{
			h_scene.ChangeStyle(nowStyles[no].id);
		});
		SystemSE.Play(SystemSE.SE.YES);
	}
}
