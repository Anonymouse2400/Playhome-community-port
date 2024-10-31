using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryCustomEdit : MonoBehaviour
{
	[SerializeField]
	private EditMode editMode;

	private EditEquipShow equipShow;

	private Human human;

	[SerializeField]
	private ToggleButton[] toggles = new ToggleButton[10];

	[SerializeField]
	private RectTransform[] tabMains = new RectTransform[10];

	[SerializeField]
	private Text[] acceNameTexts = new Text[10];

	private DropDownUI[] typeDropdowns = new DropDownUI[10];

	private DropDownUI[] parentDropdowns = new DropDownUI[10];

	[SerializeField]
	private Toggle posEditSwitchOriginal;

	private Toggle[] posEditSwitchs = new Toggle[10];

	private ColorChangeButton[] mainColor = new ColorChangeButton[10];

	private ColorChangeButton[] mainSpecColor = new ColorChangeButton[10];

	private InputSliderUI[] mainSpecular = new InputSliderUI[10];

	private InputSliderUI[] mainSmooth = new InputSliderUI[10];

	private ColorChangeButton[] subColor = new ColorChangeButton[10];

	private ColorChangeButton[] subSpecColor = new ColorChangeButton[10];

	private InputSliderUI[] subSpecular = new InputSliderUI[10];

	private InputSliderUI[] subSmooth = new InputSliderUI[10];

	[SerializeField]
	private ToggleButton helperToggle;

	[SerializeField]
	private AcceCopyHelperUI helperUI;

	private int nowTab = -1;

	private ItemSelectUISets[] itemSelSets = new ItemSelectUISets[10];

	private bool invoke = true;

	public void Setup(Human human, EditEquipShow equipShow)
	{
		this.human = human;
		this.equipShow = equipShow;
		helperUI.SetHuman(human);
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		string[] array = new string[13]
		{
			"なし", "頭", "耳", "眼鏡", "顔", "首", "肩", "胸", "腰", "背中",
			"腕", "手", "脚"
		};
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(new Dropdown.OptionData(array[i]));
		}
		List<Dropdown.OptionData> list2 = new List<Dropdown.OptionData>();
		string[] array2 = new string[29]
		{
			"頭", "眼鏡", "左耳", "右耳", "口", "鼻", "首", "胸", "左手首", "右手首",
			"左腕", "右腕", "左人差指", "右人差指", "左中指", "右中指", "左薬指", "右薬指", "左脚", "右脚",
			"左足首", "右足首", "左乳首", "右乳首", "腰", "左肩", "右肩", "左手", "右手"
		};
		for (int j = 0; j < array2.Length; j++)
		{
			list2.Add(new Dropdown.OptionData(array2[j]));
		}
		for (int k = 0; k < tabMains.Length; k++)
		{
			GameObject parent = tabMains[k].gameObject;
			typeDropdowns[k] = editMode.CreateDropDownUI(parent, "タイプ", list, delegate(int type)
			{
				OnChangeAcceType(nowTab, (ACCESSORY_TYPE)(type - 1));
			});
			itemSelSets[k] = editMode.CreateItemSelectUISets(parent, "アクセサリ", null, delegate(CustomSelectSet set)
			{
				OnChangeAcceItem(nowTab, set);
			});
			parentDropdowns[k] = editMode.CreateDropDownUI(parent, "親", list2, delegate(int no)
			{
				OnChangeAcceParent(nowTab, no);
			});
			Toggle toggle = UnityEngine.Object.Instantiate(posEditSwitchOriginal);
			toggle.gameObject.SetActive(true);
			toggle.transform.SetParent(tabMains[k].transform, false);
			posEditSwitchs[k] = toggle;
			toggle.onValueChanged.AddListener(OnTogglePosEdit);
			editMode.CreateSpace(parent);
			mainColor[k] = editMode.CreateColorChangeButton(parent, "メインの色", Color.white, false, delegate(Color color)
			{
				OnChangeColor_Main(nowTab, color);
			});
			mainSpecColor[k] = editMode.CreateColorChangeButton(parent, "ツヤの色", Color.white, false, delegate(Color color)
			{
				OnChangeColor_MainSpec(nowTab, color);
			});
			mainSpecular[k] = editMode.CreateInputSliderUI(parent, "ツヤの強さ", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_MainSpecular(nowTab, val);
			});
			mainSmooth[k] = editMode.CreateInputSliderUI(parent, "ツヤの質感", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_MainSmooth(nowTab, val);
			});
			subColor[k] = editMode.CreateColorChangeButton(parent, "サブの色", Color.white, false, delegate(Color color)
			{
				OnChangeColor_Sub(nowTab, color);
			});
			subSpecColor[k] = editMode.CreateColorChangeButton(parent, "ツヤの色", Color.white, false, delegate(Color color)
			{
				OnChangeColor_SubSpec(nowTab, color);
			});
			subSpecular[k] = editMode.CreateInputSliderUI(parent, "ツヤの強さ", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_SubSpecular(nowTab, val);
			});
			subSmooth[k] = editMode.CreateInputSliderUI(parent, "ツヤの質感", 0f, 100f, false, 0f, delegate(float val)
			{
				OnChangeSlider_SubSmooth(nowTab, val);
			});
		}
		for (int l = 0; l < tabMains.Length; l++)
		{
			tabMains[l].gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		if (equipShow != null)
		{
			equipShow.SetAuto(EditEquipShow.WEARSHOW.ALL);
		}
		UpdateSlotDataName();
	}

	private void OnDisable()
	{
		for (int i = 0; i < toggles.Length; i++)
		{
			toggles[i].Value = false;
			tabMains[i].gameObject.SetActive(false);
		}
		helperToggle.Value = false;
		CloseMoveGuideDriveUI(false);
	}

	private void Update()
	{
		Update_CheckTab();
		Update_Tab();
	}

	private void Update_CheckTab()
	{
		int num = -1;
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].Value)
			{
				num = i;
			}
			tabMains[i].gameObject.SetActive(toggles[i].Value);
		}
		if (num != nowTab)
		{
			ChangeTab(num);
		}
	}

	private void ChangeTab(int newTab)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		CloseMoveGuideDriveUI(false);
		nowTab = newTab;
		if (nowTab != -1)
		{
			UpdateSlotUI(nowTab);
		}
		UpdateSlotDataName();
	}

	public void LoadedHuman()
	{
		LoadedCoordinate();
	}

	public void LoadedCoordinate()
	{
		for (int i = 0; i < 10; i++)
		{
			UpdateSlotUI(i);
		}
	}

	public void UpdateUI_NowTab()
	{
		UpdateSlotUI(nowTab);
		UpdateSlotDataName();
	}

	private void UpdateSlotUI(int slot)
	{
		if (slot == -1)
		{
			return;
		}
		invoke = false;
		AccessoryCustom accessoryCustom = human.customParam.acce.slot[slot];
		typeDropdowns[slot].SetValue((int)(accessoryCustom.type + 1));
		if (accessoryCustom.type == ACCESSORY_TYPE.NONE)
		{
			itemSelSets[slot].ChangeDatas(null, true);
		}
		else
		{
			itemSelSets[slot].ChangeDatas(editMode.thumnbs_acce[(int)accessoryCustom.type], true);
			List<CustomSelectSet> datas = itemSelSets[slot].GetDatas();
			for (int i = 0; i < datas.Count; i++)
			{
				AccessoryData acceData = CustomDataManager.GetAcceData(accessoryCustom.type, datas[i].id);
				datas[i].hide = acceData != null && acceData.special == ItemDataBase.SPECIAL.VR_EVENT && !GlobalData.vr_event_item;
			}
		}
		itemSelSets[slot].SetSelectedFromDataID(accessoryCustom.id);
		itemSelSets[slot].ApplyFromID(accessoryCustom.id);
		parentDropdowns[slot].SetValue((int)accessoryCustom.nowAttach);
		parentDropdowns[nowTab].dropdown.interactable = accessoryCustom.nowAttach != ACCESSORY_ATTACH.SKINNING;
		UpdateColorUI(slot);
		invoke = true;
	}

	private void Update_Tab()
	{
		if (nowTab != -1)
		{
			bool flag = typeDropdowns[nowTab].Value != 0;
			bool flag2 = itemSelSets[nowTab].GetSelectedData() != null;
			itemSelSets[nowTab].toggle.gameObject.SetActive(flag);
			parentDropdowns[nowTab].gameObject.SetActive(flag && flag2);
			posEditSwitchs[nowTab].gameObject.SetActive(flag && flag2);
			UpdateColorUI(nowTab);
		}
	}

	private void UpdateColorUI(int no)
	{
		AccessoryCustom accessoryCustom = human.customParam.acce.slot[no];
		bool flag = human.accessories.EnableColorCustom(no);
		bool flag2 = human.accessories.EnableSecondColorCustom(no);
		bool flag3 = human.accessories.EnableSecondSpecularCustom(no);
		mainColor[no].gameObject.SetActive(flag);
		mainSpecColor[no].gameObject.SetActive(flag);
		mainSpecular[no].gameObject.SetActive(flag);
		mainSmooth[no].gameObject.SetActive(flag);
		subColor[no].gameObject.SetActive(flag2);
		subSpecColor[no].gameObject.SetActive(flag2);
		subSpecular[no].gameObject.SetActive(flag2 && flag3);
		subSmooth[no].gameObject.SetActive(flag2);
		if (flag)
		{
			mainColor[no].SetColor(accessoryCustom.color.mainColor1);
			mainSpecColor[no].SetColor(accessoryCustom.color.specColor1);
			mainSpecular[no].Value = accessoryCustom.color.specular1 * 100f;
			mainSmooth[no].Value = accessoryCustom.color.smooth1 * 100f;
		}
		if (flag2)
		{
			subColor[no].SetColor(accessoryCustom.color.mainColor2);
			subSpecColor[no].SetColor(accessoryCustom.color.specColor2);
			subSpecular[no].Value = accessoryCustom.color.specular2 * 100f;
			subSmooth[no].Value = accessoryCustom.color.smooth2 * 100f;
		}
	}

	private void UpdateSlotDataName()
	{
		for (int i = 0; i < acceNameTexts.Length; i++)
		{
			((Component)(object)acceNameTexts[i]).gameObject.SetActive(nowTab == -1);
			AccessoryData accessoryData = human.accessories.GetAccessoryData(human.customParam.acce, i);
			acceNameTexts[i].text = ((accessoryData == null) ? string.Empty : accessoryData.name);
		}
	}

	private void OnChangeAcceType(int slot, ACCESSORY_TYPE type)
	{
		if (!invoke)
		{
			return;
		}
		AccessoryCustom accessoryCustom = human.customParam.acce.slot[slot];
		accessoryCustom.type = type;
		accessoryCustom.id = -1;
		accessoryCustom.nowAttach = ACCESSORY_ATTACH.NONE;
		accessoryCustom.ResetAttachPosition();
		human.accessories.DeleteAccessory(slot);
		if (type == ACCESSORY_TYPE.NONE)
		{
			itemSelSets[slot].ChangeDatas(null, true);
		}
		else
		{
			itemSelSets[slot].ChangeDatas(editMode.thumnbs_acce[(int)type], true);
			List<CustomSelectSet> datas = itemSelSets[slot].GetDatas();
			for (int i = 0; i < datas.Count; i++)
			{
				AccessoryData acceData = CustomDataManager.GetAcceData(type, datas[i].id);
				datas[i].hide = acceData != null && acceData.special == ItemDataBase.SPECIAL.VR_EVENT && !GlobalData.vr_event_item;
			}
		}
		helperUI.SetAccessoryNames();
		HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
		if (hWearAcceChangeUI != null)
		{
			hWearAcceChangeUI.CheckShowUI();
		}
	}

	private void OnChangeAcceItem(int slot, CustomSelectSet set)
	{
		if (invoke)
		{
			AccessoryCustom accessoryCustom = human.customParam.acce.slot[slot];
			int type = (int)human.customParam.acce.slot[slot].type;
			AccessoryData acceData = CustomDataManager.GetAcceData(accessoryCustom.type, accessoryCustom.id);
			AccessoryData acceData2 = CustomDataManager.GetAcceData((ACCESSORY_TYPE)type, set.id);
			bool fixAttachParent = true;
			if (acceData != null && acceData2 != null && acceData.defAttach == acceData2.defAttach)
			{
				fixAttachParent = false;
			}
			accessoryCustom.id = set.id;
			accessoryCustom.color = null;
			human.accessories.AccessoryInstantiate(human.customParam.acce, slot, fixAttachParent, acceData);
			if (MaterialCustomData.GetAcce(human.customParam.acce.slot[slot]))
			{
				human.accessories.UpdateColorCustom(slot);
			}
			invoke = false;
			int num = (int)((accessoryCustom.nowAttach != ACCESSORY_ATTACH.SKINNING) ? accessoryCustom.nowAttach : ACCESSORY_ATTACH.NONE);
			parentDropdowns[nowTab].dropdown.value = num;
			parentDropdowns[nowTab].dropdown.interactable = num != -1;
			invoke = true;
			Resources.UnloadUnusedAssets();
			helperUI.SetAccessoryNames();
			HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
			if (hWearAcceChangeUI != null)
			{
				hWearAcceChangeUI.CheckShowUI();
			}
		}
	}

	private void OnChangeAcceParent(int slot, int no)
	{
		if (invoke)
		{
			AccessoryCustom accessoryCustom = human.customParam.acce.slot[slot];
			accessoryCustom.nowAttach = (ACCESSORY_ATTACH)no;
			human.accessories.AttachAccessory(human.customParam.acce, slot, false, null);
		}
	}

	private void OnTogglePosEdit(bool flag)
	{
		if (!invoke)
		{
			return;
		}
		if (flag)
		{
			AccessoryCustom accessoryCustom = human.customParam.acce.slot[nowTab];
			if (accessoryCustom != null && accessoryCustom.type != ACCESSORY_TYPE.NONE)
			{
				AccessoryData acceData = CustomDataManager.GetAcceData(accessoryCustom.type, accessoryCustom.id);
				if (acceData != null)
				{
					string title = acceData.name + "位置調整";
					AccessoryCustom accessoryCustom2 = human.customParam.acce.slot[nowTab];
					MoveableGuideDriveUI moveableGuideDriveUI = editMode.ShowMoveableGuideDriveUI(title, accessoryCustom2.addPos, accessoryCustom2.addRot, accessoryCustom2.addScl, MoveableUIOnChangeState, OnMoveGuide);
					Vector3 limitPosMin = Vector3.one * -1f;
					Vector3 limitPosMax = Vector3.one * 1f;
					Vector3 limitSclMin = Vector3.one * 0.01f;
					Vector3 limitSclMax = Vector3.one * 100f;
					moveableGuideDriveUI.SetLimit(true, limitPosMin, limitPosMax, true, limitSclMin, limitSclMax);
				}
			}
		}
		else
		{
			CloseMoveGuideDriveUI(true);
		}
	}

	private void CloseMoveGuideDriveUI(bool SE)
	{
		if (nowTab != -1)
		{
			invoke = false;
			Transform moveTrans = human.accessories.GetMoveTrans(human.customParam.acce, nowTab);
			if (SE)
			{
				editMode.HideMoveableGuideDriveUI(moveTrans);
			}
			else
			{
				editMode.HideMoveableGuideDriveUI_WithoutSE(moveTrans);
			}
			posEditSwitchs[nowTab].isOn = false;
			invoke = true;
		}
	}

	private void OnMoveAcce()
	{
		human.accessories.UpdatePosition(human.customParam.acce, nowTab);
	}

	private void MoveableUIOnChangeState(MoveableUI.STATE state)
	{
		if (state == MoveableUI.STATE.CLOSED)
		{
			invoke = false;
			posEditSwitchs[nowTab].isOn = false;
			invoke = true;
		}
	}

	private void OnChangeColor_Main(int slot, Color color)
	{
		human.customParam.acce.slot[slot].color.mainColor1 = color;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnChangeColor_MainSpec(int slot, Color color)
	{
		human.customParam.acce.slot[slot].color.specColor1 = color;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnChangeSlider_MainSpecular(int slot, float val)
	{
		human.customParam.acce.slot[slot].color.specular1 = val * 0.01f;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnChangeSlider_MainSmooth(int slot, float val)
	{
		human.customParam.acce.slot[slot].color.smooth1 = val * 0.01f;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnChangeColor_Sub(int slot, Color color)
	{
		human.customParam.acce.slot[slot].color.mainColor2 = color;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnChangeColor_SubSpec(int slot, Color color)
	{
		human.customParam.acce.slot[slot].color.specColor2 = color;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnChangeSlider_SubSpecular(int slot, float val)
	{
		human.customParam.acce.slot[slot].color.specular2 = val * 0.01f;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnChangeSlider_SubSmooth(int slot, float val)
	{
		human.customParam.acce.slot[slot].color.smooth2 = val * 0.01f;
		human.accessories.UpdateColorCustom(slot);
		MaterialCustomData.SetAcce(human.customParam.acce.slot[slot]);
	}

	private void OnMoveGuide(Vector3 pos, Vector3 eul, Vector3 scl)
	{
		human.customParam.acce.slot[nowTab].addPos = pos;
		human.customParam.acce.slot[nowTab].addRot = eul;
		human.customParam.acce.slot[nowTab].addScl = scl;
		human.accessories.UpdatePosition(human.customParam.acce, nowTab);
	}
}
