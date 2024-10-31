using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class WearCustomEdit : MonoBehaviour
{
	[SerializeField]
	private ToggleButton[] wearTtypeToggles = new ToggleButton[2];

	private ItemSelectUISets[] itemSelSets = new ItemSelectUISets[11];

	private ColorChangeButton[] mainColor = new ColorChangeButton[11];

	private ColorChangeButton[] mainSpecColor = new ColorChangeButton[11];

	private InputSliderUI[] mainSpecular = new InputSliderUI[11];

	private InputSliderUI[] mainSmooth = new InputSliderUI[11];

	private ColorChangeButton[] subColor = new ColorChangeButton[11];

	private ColorChangeButton[] subSpecColor = new ColorChangeButton[11];

	private InputSliderUI[] subSpecular = new InputSliderUI[11];

	private InputSliderUI[] subSmooth = new InputSliderUI[11];

	[SerializeField]
	private ToggleButton[] toggles = new ToggleButton[11];

	[SerializeField]
	private RectTransform[] tabMains = new RectTransform[11];

	[SerializeField]
	private Text[] dataNameTexts = new Text[11];

	private SwitchUI swim_opt_toggle_top;

	private SwitchUI swim_opt_toggle_btm;

	private Human human;

	private int nowTab = -1;

	[SerializeField]
	private EditMode editMode;

	private EditEquipShow equipShow;

	public void Setup(Human human, EditEquipShow equipShow)
	{
		this.human = human;
		this.equipShow = equipShow;
		wearTtypeToggles[0].gameObject.SetActive(human.sex == SEX.FEMALE);
		wearTtypeToggles[1].gameObject.SetActive(human.sex == SEX.FEMALE);
		for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
		{
			Setup_WearType(wEAR_TYPE);
		}
		for (int i = 0; i < tabMains.Length; i++)
		{
			tabMains[i].gameObject.SetActive(false);
		}
		CheckWearTabs();
	}

	private void OnEnable()
	{
		EditEquipShow.WEARSHOW auto = EditEquipShow.WEARSHOW.ALL;
		if (nowTab == 2 || nowTab == 3)
		{
			auto = EditEquipShow.WEARSHOW.UNDERWEAR;
		}
		if (equipShow != null)
		{
			equipShow.SetAuto(auto);
		}
		if (human.sex == SEX.FEMALE)
		{
			wearTtypeToggles[0].ChangeValue(!human.customParam.wear.isSwimwear, false);
			wearTtypeToggles[1].ChangeValue(human.customParam.wear.isSwimwear, false);
		}
		UpdateSlotDataName();
	}

	private void Setup_WearType(WEAR_TYPE wearType)
	{
		int num = (int)wearType;
		string[] array = new string[11]
		{
			"トップス", "ボトムス", "ブラ", "ショーツ", "水着", "水着トップス", "水着ボトムス", "手袋", "パンスト", "靴下",
			"靴"
		};
		if (human.sex != SEX.MALE || wearType == WEAR_TYPE.TOP || wearType == WEAR_TYPE.SHOES)
		{
			GameObject gameObject = tabMains[num].gameObject;
			List<CustomSelectSet> setDatas = null;
			if (human.sex == SEX.FEMALE)
			{
				List<CustomSelectSet>[] array2 = new List<CustomSelectSet>[11]
				{
					editMode.thumnbs_wear_tops, editMode.thumnbs_wear_bottoms, editMode.thumnbs_wear_bra, editMode.thumnbs_wear_shorts, editMode.thumnbs_wear_swimwear, editMode.thumnbs_wear_swimtops, editMode.thumnbs_wear_swimbottoms, editMode.thumnbs_wear_glove, editMode.thumnbs_wear_panst, editMode.thumnbs_wear_socks,
					editMode.thumnbs_wear_shoes
				};
				setDatas = array2[num];
			}
			else if (wearType == WEAR_TYPE.TOP)
			{
				setDatas = editMode.thumnbs_wear_tops;
			}
			else if (wearType == WEAR_TYPE.SHOES)
			{
				setDatas = editMode.thumnbs_wear_shoes;
			}
			itemSelSets[num] = editMode.CreateItemSelectUISets(gameObject.gameObject, array[num], setDatas, delegate(CustomSelectSet selData)
			{
				ChangeOnWear_SelData(wearType, selData);
			});
			editMode.CreateSpace(gameObject);
			mainColor[num] = editMode.CreateColorChangeButton(gameObject, "メインの色", Color.white, false, delegate(Color val)
			{
				OnChangeColor_Main(wearType, val);
			});
			mainSpecColor[num] = editMode.CreateColorChangeButton(gameObject, "ツヤの色", Color.white, false, delegate(Color val)
			{
				OnChangeColor_MainSpec(wearType, val);
			});
			mainSpecular[num] = editMode.CreateInputSliderUI(gameObject, "ツヤの強さ", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_MainSpecular(wearType, val);
			});
			mainSmooth[num] = editMode.CreateInputSliderUI(gameObject, "ツヤの質感", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_MainSmooth(wearType, val);
			});
			subColor[num] = editMode.CreateColorChangeButton(gameObject, "サブの色", Color.white, false, delegate(Color val)
			{
				OnChangeColor_Sub(wearType, val);
			});
			subSpecColor[num] = editMode.CreateColorChangeButton(gameObject, "ツヤの色", Color.white, false, delegate(Color val)
			{
				OnChangeColor_SubSpec(wearType, val);
			});
			subSpecular[num] = editMode.CreateInputSliderUI(gameObject, "ツヤの強さ", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_SubSpecular(wearType, val);
			});
			subSmooth[num] = editMode.CreateInputSliderUI(gameObject, "ツヤの質感", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_SubSmooth(wearType, val);
			});
			if (wearType == WEAR_TYPE.SWIM)
			{
				swim_opt_toggle_top = editMode.CreateSwitchUI(gameObject, "オプション:上", false, OnChangeSwimOpt_Top);
				swim_opt_toggle_btm = editMode.CreateSwitchUI(gameObject, "オプション:下", false, OnChangeSwimOpt_Bottom);
			}
			LoadedCoordinate();
		}
	}

	private void Update()
	{
		int num = -1;
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].Value)
			{
				num = i;
			}
			tabMains[i].gameObject.SetActive(toggles[i].gameObject.activeSelf && toggles[i].Value);
		}
		if (num != nowTab)
		{
			ChangeTab(num);
		}
	}

	private void ChangeTab(int tab)
	{
		nowTab = tab;
		Update_Tab();
		EditEquipShow.WEARSHOW auto = EditEquipShow.WEARSHOW.ALL;
		if (nowTab == 2 || nowTab == 3)
		{
			auto = EditEquipShow.WEARSHOW.UNDERWEAR;
		}
		if (equipShow != null)
		{
			equipShow.SetAuto(auto);
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void Update_Tab()
	{
		if (nowTab >= 0)
		{
			WEAR_TYPE wEAR_TYPE = (WEAR_TYPE)nowTab;
			LoadedCoordinate(wEAR_TYPE);
			List<CustomSelectSet> datas = itemSelSets[nowTab].GetDatas();
			for (int i = 0; i < datas.Count; i++)
			{
				WearData wearData = CustomDataManager.GetWearData(human.sex, wEAR_TYPE, datas[i].id);
				datas[i].hide = wearData != null && wearData.special == ItemDataBase.SPECIAL.VR_EVENT && !GlobalData.vr_event_item;
			}
			if (human.sex == SEX.FEMALE)
			{
				switch (wEAR_TYPE)
				{
				case WEAR_TYPE.TOP:
				{
					int wearID2 = human.customParam.wear.GetWearID(WEAR_TYPE.BOTTOM);
					WearData wearData4 = CustomDataManager.GetWearData(human.sex, WEAR_TYPE.BOTTOM, wearID2);
					List<CustomSelectSet> datas3 = itemSelSets[nowTab].GetDatas();
					for (int k = 0; k < datas3.Count; k++)
					{
						bool enable2 = true;
						if (wearData4 != null && wearData4.coordinates != 0)
						{
							WearData wearData5 = CustomDataManager.GetWearData(human.sex, WEAR_TYPE.TOP, datas3[k].id);
							if (wearData5.coordinates == 1)
							{
								enable2 = false;
							}
						}
						datas3[k].enable = enable2;
					}
					int wearID3 = human.customParam.wear.GetWearID(WEAR_TYPE.TOP);
					WearData wearData6 = CustomDataManager.GetWearData(human.sex, WEAR_TYPE.TOP, wearID3);
					if (wearData6 != null && wearData6.coordinates == 2)
					{
						toggles[1].Interactable = false;
					}
					else
					{
						toggles[1].Interactable = true;
					}
					CheckUnderWearTab();
					break;
				}
				case WEAR_TYPE.BOTTOM:
				{
					int wearID = human.customParam.wear.GetWearID(WEAR_TYPE.TOP);
					WearData wearData2 = CustomDataManager.GetWearData(human.sex, WEAR_TYPE.TOP, wearID);
					if (wearData2 != null && wearData2.coordinates == 2)
					{
						toggles[1].Interactable = false;
					}
					else
					{
						toggles[1].Interactable = true;
						List<CustomSelectSet> datas2 = itemSelSets[nowTab].GetDatas();
						for (int j = 0; j < datas2.Count; j++)
						{
							bool enable = true;
							if (wearData2 != null && wearData2.coordinates == 1)
							{
								WearData wearData3 = CustomDataManager.GetWearData(human.sex, WEAR_TYPE.BOTTOM, datas2[j].id);
								if (wearData3.coordinates != 0)
								{
									enable = false;
								}
							}
							datas2[j].enable = enable;
						}
					}
					CheckUnderWearTab();
					break;
				}
				case WEAR_TYPE.SWIM:
					CheckSwimTab();
					break;
				}
			}
		}
		UpdateSlotDataName();
	}

	private void UpdateSlotDataName()
	{
		for (int i = 0; i < dataNameTexts.Length; i++)
		{
			((Component)(object)dataNameTexts[i]).gameObject.SetActive(nowTab == -1);
			WearData wearData = human.wears.GetWearData((WEAR_TYPE)i);
			string text = ((wearData == null) ? string.Empty : wearData.name);
			if (text == "なし")
			{
				text = string.Empty;
			}
			dataNameTexts[i].text = text;
		}
	}

	public void ChangeWearType_Normal(bool flag)
	{
		if (flag && human.customParam.wear.isSwimwear)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			for (int i = 0; i < toggles.Length; i++)
			{
				toggles[i].Value = false;
			}
			human.customParam.wear.isSwimwear = false;
			human.Apply();
			CheckWearTabs();
			HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
			if (hWearAcceChangeUI != null)
			{
				hWearAcceChangeUI.CheckShowUI();
			}
		}
	}

	public void ChangeWearType_Swim(bool flag)
	{
		if (flag && !human.customParam.wear.isSwimwear)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			for (int i = 0; i < toggles.Length; i++)
			{
				toggles[i].Value = false;
			}
			human.customParam.wear.isSwimwear = true;
			human.Apply();
			CheckWearTabs();
			HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
			if (hWearAcceChangeUI != null)
			{
				hWearAcceChangeUI.CheckShowUI();
			}
		}
	}

	private void CheckWearTabs()
	{
		if (human.sex == SEX.FEMALE)
		{
			bool isSwimwear = human.customParam.wear.isSwimwear;
			toggles[0].gameObject.SetActive(!isSwimwear);
			toggles[1].gameObject.SetActive(!isSwimwear);
			toggles[2].gameObject.SetActive(!isSwimwear);
			toggles[3].gameObject.SetActive(!isSwimwear);
			toggles[4].gameObject.SetActive(isSwimwear);
			toggles[5].gameObject.SetActive(isSwimwear);
			toggles[6].gameObject.SetActive(isSwimwear);
			toggles[7].gameObject.SetActive(true);
			toggles[8].gameObject.SetActive(true);
			toggles[9].gameObject.SetActive(true);
			toggles[10].gameObject.SetActive(true);
			CheckBottomsTab();
			CheckUnderWearTab();
			CheckSwimTab();
		}
		else
		{
			toggles[0].gameObject.SetActive(true);
			toggles[1].gameObject.SetActive(false);
			toggles[2].gameObject.SetActive(false);
			toggles[3].gameObject.SetActive(false);
			toggles[4].gameObject.SetActive(false);
			toggles[5].gameObject.SetActive(false);
			toggles[6].gameObject.SetActive(false);
			toggles[7].gameObject.SetActive(false);
			toggles[8].gameObject.SetActive(false);
			toggles[9].gameObject.SetActive(false);
			toggles[10].gameObject.SetActive(true);
		}
		for (int i = 0; i < toggles.Length; i++)
		{
			if (tabMains[i].gameObject.activeSelf)
			{
				tabMains[i].gameObject.SetActive(toggles[i].gameObject.activeSelf && toggles[i].Value);
			}
		}
	}

	private void CheckBottomsTab()
	{
		if (human.sex == SEX.FEMALE)
		{
			bool interactable = true;
			WearData wearData_Female = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, human.customParam.wear.GetWearID(WEAR_TYPE.TOP));
			if (wearData_Female != null && wearData_Female.coordinates == 2)
			{
				interactable = false;
			}
			toggles[1].Interactable = interactable;
		}
	}

	public void CheckUnderWearTab()
	{
		if (human.sex != 0)
		{
			return;
		}
		bool interactable = true;
		bool interactable2 = true;
		WearData wearData_Female = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, human.customParam.wear.GetWearID(WEAR_TYPE.TOP));
		WearData wearData_Female2 = CustomDataManager.GetWearData_Female(WEAR_TYPE.BOTTOM, human.customParam.wear.GetWearID(WEAR_TYPE.BOTTOM));
		if (wearData_Female != null)
		{
			if (wearData_Female.braDisable)
			{
				interactable = false;
			}
			if (wearData_Female.shortsDisable)
			{
				interactable2 = false;
			}
		}
		if (wearData_Female2 != null)
		{
			if (wearData_Female2.braDisable)
			{
				interactable = false;
			}
			if (wearData_Female2.shortsDisable)
			{
				interactable2 = false;
			}
		}
		toggles[2].Interactable = interactable;
		toggles[3].Interactable = interactable2;
	}

	private void CheckSwimTab()
	{
		if (human.sex != 0)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (human.customParam.wear.isSwimwear)
		{
			WearObj wearObj = human.wears.GetWearObj(WEAR_TYPE.SWIM);
			if (wearObj != null)
			{
				Transform transform = Transform_Utility.FindTransform(wearObj.obj.transform, Wears.name_swimOptTop);
				Transform transform2 = Transform_Utility.FindTransform(wearObj.obj.transform, Wears.name_swimOptBot);
				flag = transform != null;
				flag2 = transform2 != null;
			}
		}
		if (swim_opt_toggle_top != null && swim_opt_toggle_top != null)
		{
			swim_opt_toggle_top.gameObject.SetActive(flag);
			swim_opt_toggle_btm.gameObject.SetActive(flag2);
			if (flag)
			{
				swim_opt_toggle_top.SetValue(human.customParam.wear.swimOptTop);
			}
			if (flag2)
			{
				swim_opt_toggle_btm.SetValue(human.customParam.wear.swimOptBtm);
			}
		}
	}

	public void LoadedHuman()
	{
		LoadedCoordinate();
	}

	public void LoadedCoordinate()
	{
		for (int i = 0; i < 11; i++)
		{
			LoadedCoordinate((WEAR_TYPE)i);
		}
		CheckWearTabs();
	}

	public void LoadedCoordinate(WEAR_TYPE type)
	{
		WearCustom wearCustom = human.customParam.wear.wears[(int)type];
		if (wearCustom != null && itemSelSets[(int)type] != null)
		{
			itemSelSets[(int)type].SetSelectedFromDataID(wearCustom.id);
			bool flag = human.wears.EnableColorCustom(type);
			bool flag2 = human.wears.EnableSecondColorCustom(type);
			bool flag3 = human.wears.EnableSecondSpecularCustom(type);
			if (flag)
			{
				mainColor[(int)type].SetColor(wearCustom.color.mainColor1);
				mainSpecColor[(int)type].SetColor(wearCustom.color.specColor1);
				mainSpecular[(int)type].SetValue(wearCustom.color.specular1 * 100f);
				mainSmooth[(int)type].SetValue(wearCustom.color.smooth1 * 100f);
			}
			if (flag2)
			{
				subColor[(int)type].SetColor(wearCustom.color.mainColor2);
				subSpecColor[(int)type].SetColor(wearCustom.color.specColor2);
				subSpecular[(int)type].SetValue(wearCustom.color.specular2 * 100f);
				subSmooth[(int)type].SetValue(wearCustom.color.smooth2 * 100f);
			}
			mainColor[(int)type].gameObject.SetActive(flag);
			mainSpecColor[(int)type].gameObject.SetActive(flag);
			mainSpecular[(int)type].gameObject.SetActive(flag);
			mainSmooth[(int)type].gameObject.SetActive(flag);
			subColor[(int)type].gameObject.SetActive(flag2);
			subSpecColor[(int)type].gameObject.SetActive(flag2);
			subSpecular[(int)type].gameObject.SetActive(flag2 && flag3);
			subSmooth[(int)type].gameObject.SetActive(flag2);
		}
	}

	private void ChangeOnWear(WEAR_TYPE wear, int id)
	{
		human.customParam.wear.wears[(int)wear].id = id;
		human.customParam.wear.wears[(int)wear].color = null;
		MaterialCustomData.GetWear(wear, human.customParam.wear.wears[(int)wear]);
		human.ApplyCoordinate();
		Update_Tab();
		HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
		if (hWearAcceChangeUI != null)
		{
			hWearAcceChangeUI.CheckShowUI();
		}
	}

	private void ChangeOnWear_SelData(WEAR_TYPE wearType, CustomSelectSet selData)
	{
		if (selData != null)
		{
			ChangeOnWear(wearType, selData.id);
		}
	}

	private void OnChangeColor_Main(WEAR_TYPE type, Color color)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.mainColor1 = color;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeColor_MainSpec(WEAR_TYPE type, Color color)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.specColor1 = color;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeSlider_MainSpecular(WEAR_TYPE type, float val)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.specular1 = val * 0.01f;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeSlider_MainSmooth(WEAR_TYPE type, float val)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.smooth1 = val * 0.01f;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeColor_Sub(WEAR_TYPE type, Color color)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.mainColor2 = color;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeColor_SubSpec(WEAR_TYPE type, Color color)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.specColor2 = color;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeSlider_SubSpecular(WEAR_TYPE type, float val)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.specular2 = val * 0.01f;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeSlider_SubSmooth(WEAR_TYPE type, float val)
	{
		if (human.customParam.wear.wears[(int)type] != null && human.customParam.wear.wears[(int)type].color != null)
		{
			human.customParam.wear.wears[(int)type].color.smooth2 = val * 0.01f;
			human.wears.UpdateColorCustom(type);
			MaterialCustomData.SetWear(type, human.customParam.wear.wears[(int)type]);
		}
	}

	private void OnChangeSwimOpt_Top(bool flag)
	{
		int num = 4;
		if (human.customParam.wear.wears[num] != null)
		{
			human.customParam.wear.swimOptTop = flag;
			human.wears.ChangeSwimOption_Top();
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void OnChangeSwimOpt_Bottom(bool flag)
	{
		int num = 4;
		if (human.customParam.wear.wears[num] != null)
		{
			human.customParam.wear.swimOptBtm = flag;
			human.wears.ChangeSwimOption_Bottom();
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}
}
