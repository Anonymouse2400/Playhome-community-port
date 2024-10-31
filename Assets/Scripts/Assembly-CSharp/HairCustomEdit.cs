using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class HairCustomEdit : MonoBehaviour
{
	private enum TAB
	{
		SET = 0,
		BACK = 1,
		SIDE = 2,
		FRONT = 3,
		NUM = 4
	}

	private enum PART
	{
		SET = 0,
		BACK = 1,
		SIDE = 2,
		FRONT = 3,
		NUM = 4
	}

	[SerializeField]
	private ToggleButton[] toggles = new ToggleButton[4];

	[SerializeField]
	private GameObject[] tabMains = new GameObject[4];

	[SerializeField]
	private Text[] dataNameTexts = new Text[4];

	[SerializeField]
	private ToggleButton colorCopyToggle;

	[SerializeField]
	private CharaColorCopyHelper colorCopyHelper;

	private ItemSelectUISets[] selects = new ItemSelectUISets[4];

	[SerializeField]
	private EditMode editMode;

	private EditEquipShow equipShow;

	private Human human;

	private int nowTab = -1;

	private bool setAllColor = true;

	private ColorChangeButton[] hairColorButton = new ColorChangeButton[4];

	private ColorChangeButton[] hairCuticleButton = new ColorChangeButton[4];

	private InputSliderUI[] cuticleExpSlider = new InputSliderUI[4];

	private ColorChangeButton[] fresnelColorButton = new ColorChangeButton[4];

	private InputSliderUI[] fresnelExpSlider = new InputSliderUI[4];

	private ColorChangeButton[] acceColorButton = new ColorChangeButton[4];

	private ColorChangeButton[] acceSpecularButton = new ColorChangeButton[4];

	private InputSliderUI[] acceMetaricSlider = new InputSliderUI[4];

	private InputSliderUI[] acceSmoothSlider = new InputSliderUI[4];

	private SwitchUI[] allColorSwitch = new SwitchUI[3];

	private Text[] labelHairColor = new Text[4];

	private Text[] labelAcceColor = new Text[4];

	[SerializeField]
	private TextAsset presetText;

	private List<string> presetNames = new List<string>();

	private List<ColorParameter_Hair> presetColors = new List<ColorParameter_Hair>();

	private PresetSelectUISets[] presetSelectUIS = new PresetSelectUISets[4];

	public void Setup(Human human, EditEquipShow equipShow)
	{
		this.human = human;
		this.equipShow = equipShow;
		SetupPreset();
		string text = ((human.sex != SEX.MALE) ? "ｾｯﾄ髪" : "髪型");
		toggles[0].SetText(text, text);
		for (PART pART = PART.SET; pART <= PART.FRONT; pART++)
		{
			bool flag = true;
			if (human.sex == SEX.MALE && pART != 0)
			{
				flag = false;
			}
			toggles[(int)pART].gameObject.SetActive(flag);
			if (flag)
			{
				Setup_Parts(pART);
			}
		}
		for (int i = 0; i < tabMains.Length; i++)
		{
			tabMains[i].gameObject.SetActive(false);
		}
		colorCopyToggle.action.AddListener(OnToggleButton_ColorCopyHelper);
		colorCopyHelper.moveable.AddOnChange(OnChangeColorCopyHelperMoveableState);
	}

	private void Setup_Parts(PART part)
	{
		int num = (int)part;
		editMode.CreateLabel(tabMains[num], "髪型");
		if (human.sex == SEX.FEMALE)
		{
			if (part == PART.SET)
			{
				selects[num] = editMode.CreateItemSelectUISets(tabMains[num], "セット", editMode.thumnbs_hair_set, ChangeHair_Set);
			}
			else if (part == PART.BACK)
			{
				selects[num] = editMode.CreateItemSelectUISets(tabMains[num], "後髪", editMode.thumnbs_hair_back, ChangeHair_Back);
			}
			else if (part == PART.SIDE)
			{
				selects[num] = editMode.CreateItemSelectUISets(tabMains[num], "横髪", editMode.thumnbs_hair_side, ChangeHair_Side);
			}
			else if (part == PART.FRONT)
			{
				selects[num] = editMode.CreateItemSelectUISets(tabMains[num], "前髪", editMode.thumnbs_hair_front, ChangeHair_Front);
			}
		}
		else if (part == PART.SET)
		{
			selects[num] = editMode.CreateItemSelectUISets(tabMains[num], "髪型", editMode.thumnbs_hair_set, ChangeHair_Set);
		}
		labelHairColor[num] = editMode.CreateLabel(tabMains[num], "色");
		HairParameter hair = human.customParam.hair;
		if (part != 0)
		{
			int num2 = num - 1;
			allColorSwitch[num2] = editMode.CreateSwitchUI(tabMains[num], "髪色を統一する", setAllColor, OnChangeSetAllColor);
		}
		HAIR_TYPE[] array = new HAIR_TYPE[4]
		{
			HAIR_TYPE.BACK,
			HAIR_TYPE.BACK,
			HAIR_TYPE.SIDE,
			HAIR_TYPE.FRONT
		};
		int num3 = (int)array[num];
		hairColorButton[num] = editMode.CreateColorChangeButton(tabMains[num], "髪色", hair.parts[num3].hairColor.mainColor, false, delegate(Color color)
		{
			OnChangeColor_Hair(part, color);
		});
		hairCuticleButton[num] = editMode.CreateColorChangeButton(tabMains[num], "ツヤ１色", hair.parts[num3].hairColor.cuticleColor, false, delegate(Color color)
		{
			OnChangeColor_Cuticle(part, color);
		});
		cuticleExpSlider[num] = editMode.CreateInputSliderUI(tabMains[num], "ツヤ１絞り", 0f, 100f, false, hair.parts[num3].hairColor.cuticleExp, delegate(float val)
		{
			OnHairCuticleExp(part, val);
		});
		fresnelColorButton[num] = editMode.CreateColorChangeButton(tabMains[num], "ツヤ２色", hair.parts[num3].hairColor.fresnelColor, false, delegate(Color color)
		{
			OnChangeColor_Fresnel(part, color);
		});
		fresnelExpSlider[num] = editMode.CreateInputSliderUI(tabMains[num], "ツヤ２絞り", 0f, 100f, false, hair.parts[num3].hairColor.fresnelExp, delegate(float val)
		{
			OnHairFresnelExp(part, val);
		});
		List<CustomSelectSet> list = new List<CustomSelectSet>();
		for (int i = 0; i < presetNames.Count; i++)
		{
			list.Add(new CustomSelectSet(i, presetNames[i], null, null, false));
		}
		presetSelectUIS[num] = editMode.CreatePresetSelectUISets(tabMains[num], "プリセット", list, delegate(CustomSelectSet set)
		{
			OnHairPreset(part, set);
		});
		labelAcceColor[num] = editMode.CreateLabel(tabMains[num], "アクセサリー");
		Color color2 = ((hair.parts[num3].acceColor == null) ? Color.white : hair.parts[num3].acceColor.mainColor1);
		Color color3 = ((hair.parts[num3].acceColor == null) ? Color.white : hair.parts[num3].acceColor.specColor1);
		float defVal = ((hair.parts[num3].acceColor == null) ? 0f : hair.parts[num3].acceColor.specular1);
		float defVal2 = ((hair.parts[num3].acceColor == null) ? 0f : hair.parts[num3].acceColor.smooth1);
		acceColorButton[num] = editMode.CreateColorChangeButton(tabMains[num], "アクセサリー色", color2, false, delegate(Color color)
		{
			OnChangeColor_Acce(part, color);
		});
		acceSpecularButton[num] = editMode.CreateColorChangeButton(tabMains[num], "ツヤ色", color3, false, delegate(Color color)
		{
			OnChangeColor_AcceSpec(part, color);
		});
		acceMetaricSlider[num] = editMode.CreateInputSliderUI(tabMains[num], "ツヤの強さ", 0f, 100f, false, defVal, delegate(float val)
		{
			OnAcceSpecular(part, val);
		});
		acceSmoothSlider[num] = editMode.CreateInputSliderUI(tabMains[num], "ツヤの質感", 0f, 100f, false, defVal2, delegate(float val)
		{
			OnAcceSmooth(part, val);
		});
	}

	private void SetupPreset()
	{
		presetNames.Clear();
		presetColors.Clear();
		TagText tagText = new TagText();
		tagText.Load_TextAsset(presetText);
		for (int i = 0; i < tagText.ElementNum; i++)
		{
			TagText.Element element = tagText.Elements[i];
			if (element.Tag == "Color")
			{
				ColorParameter_Hair colorParameter_Hair = new ColorParameter_Hair();
				string empty = string.Empty;
				empty = TagTextUtility.Load_String(element, "name");
				colorParameter_Hair.mainColor = TagTextUtility.Load_Color(element, "mainColor");
				colorParameter_Hair.cuticleColor = TagTextUtility.Load_Color(element, "cuticleColor");
				colorParameter_Hair.cuticleExp = TagTextUtility.Load_Float(element, "cuticleExp");
				colorParameter_Hair.fresnelColor = TagTextUtility.Load_Color(element, "fresnelColor");
				colorParameter_Hair.fresnelExp = TagTextUtility.Load_Float(element, "fresnelExp");
				presetNames.Add(empty);
				presetColors.Add(colorParameter_Hair);
			}
		}
	}

	private void OnEnable()
	{
		if (equipShow != null)
		{
			equipShow.SetAuto(EditEquipShow.WEARSHOW.ALL);
		}
		ChangeTab(nowTab);
		colorCopyToggle.ChangeValue(colorCopyHelper.gameObject.activeSelf, false);
	}

	private void OnDisable()
	{
		CloseMoveableUI();
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
			tabMains[i].gameObject.SetActive(toggles[i].Value);
		}
		if (num != nowTab)
		{
			ChangeTab(num);
		}
	}

	private void ChangeTab(int tab)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		nowTab = tab;
		if (nowTab >= 0)
		{
			if (nowTab > 0)
			{
				int num = nowTab - 1;
				if (num >= 0)
				{
					allColorSwitch[num].Value = setAllColor;
				}
			}
			HAIR_TYPE[] array = new HAIR_TYPE[4]
			{
				HAIR_TYPE.BACK,
				HAIR_TYPE.BACK,
				HAIR_TYPE.SIDE,
				HAIR_TYPE.FRONT
			};
			int num2 = (int)array[nowTab];
			HairPartParameter hairPartParameter = human.customParam.hair.parts[num2];
			selects[nowTab].SetSelectedFromDataID(hairPartParameter.ID);
			UpdateHairColorUI(nowTab);
		}
		CloseMoveableUI();
		UpdateSlotDataName();
	}

	private void UpdateHairColorUI(int no)
	{
		if (no >= 0)
		{
			HAIR_TYPE[] array = new HAIR_TYPE[4]
			{
				HAIR_TYPE.BACK,
				HAIR_TYPE.BACK,
				HAIR_TYPE.SIDE,
				HAIR_TYPE.FRONT
			};
			int num = (int)array[no];
			HairPartParameter hairPartParameter = human.customParam.hair.parts[num];
			bool flag = true;
			flag = ((no != 0) ? (!human.hairs.isSetHair) : human.hairs.isSetHair);
			bool flag2 = flag && human.hairs.HasAcce(array[no]);
			((Component)(object)labelHairColor[no]).gameObject.SetActive(flag);
			hairColorButton[no].gameObject.SetActive(flag);
			hairCuticleButton[no].gameObject.SetActive(flag);
			cuticleExpSlider[no].gameObject.SetActive(flag);
			fresnelColorButton[no].gameObject.SetActive(flag);
			fresnelExpSlider[no].gameObject.SetActive(flag);
			presetSelectUIS[no].toggle.gameObject.SetActive(flag);
			hairColorButton[no].SetColor(hairPartParameter.hairColor.mainColor);
			hairCuticleButton[no].SetColor(hairPartParameter.hairColor.cuticleColor);
			float value = Mathf.InverseLerp(0f, 20f, hairPartParameter.hairColor.cuticleExp) * 100f;
			cuticleExpSlider[no].SetValue(value);
			fresnelColorButton[no].SetColor(hairPartParameter.hairColor.fresnelColor);
			float value2 = Mathf.InverseLerp(0f, 8f, hairPartParameter.hairColor.fresnelExp) * 100f;
			fresnelExpSlider[no].SetValue(value2);
			((Component)(object)labelAcceColor[no]).gameObject.SetActive(flag2);
			acceColorButton[no].gameObject.SetActive(flag2);
			acceSpecularButton[no].gameObject.SetActive(flag2);
			acceMetaricSlider[no].gameObject.SetActive(flag2);
			acceSmoothSlider[no].gameObject.SetActive(flag2);
			if (flag2)
			{
				acceColorButton[no].SetColor(hairPartParameter.acceColor.mainColor1);
				acceSpecularButton[no].SetColor(hairPartParameter.acceColor.specColor1);
				acceMetaricSlider[no].SetValue(hairPartParameter.acceColor.specular1 * 100f);
				acceSmoothSlider[no].SetValue(hairPartParameter.acceColor.smooth1 * 100f);
			}
		}
	}

	public void ChangedColor()
	{
		UpdateHairColorUI(nowTab);
	}

	public void ChangeHair_Front(CustomSelectSet selData)
	{
		if (selData != null)
		{
			human.customParam.hair.parts[1].ID = selData.id;
			human.customParam.hair.parts[1].acceColor = null;
			CheckHair(false);
			MaterialCustomData.GetHairAcce(HAIR_TYPE.FRONT, human.customParam.hair.parts[1]);
			human.ApplyHair();
			UpdateHairColorUI(3);
		}
	}

	public void ChangeHair_Side(CustomSelectSet selData)
	{
		human.customParam.hair.parts[2].ID = selData.id;
		human.customParam.hair.parts[2].acceColor = null;
		CheckHair(false);
		MaterialCustomData.GetHairAcce(HAIR_TYPE.SIDE, human.customParam.hair.parts[2]);
		human.ApplyHair();
		UpdateHairColorUI(2);
	}

	public void ChangeHair_Back(CustomSelectSet selData)
	{
		human.customParam.hair.parts[0].ID = selData.id;
		human.customParam.hair.parts[0].acceColor = null;
		CheckHair(false);
		MaterialCustomData.GetHairAcce(HAIR_TYPE.BACK, human.customParam.hair.parts[0]);
		human.ApplyHair();
		UpdateHairColorUI(1);
	}

	public void ChangeHair_Set(CustomSelectSet selData)
	{
		human.customParam.hair.parts[0].ID = selData.id;
		human.customParam.hair.parts[0].acceColor = null;
		CheckHair(true);
		MaterialCustomData.GetHairAcce(HAIR_TYPE.BACK, human.customParam.hair.parts[0]);
		human.ApplyHair();
		UpdateHairColorUI(0);
	}

	private void CheckHair(bool isSet)
	{
		if (human.sex == SEX.MALE)
		{
			return;
		}
		if (isSet)
		{
			human.customParam.hair.parts[1].ID = 0;
			human.customParam.hair.parts[2].ID = 0;
			selects[1].SetSelectedNo(-1);
			selects[3].SetSelectedNo(0);
			selects[2].SetSelectedNo(0);
			return;
		}
		int num = human.customParam.hair.parts[1].ID;
		int num2 = human.customParam.hair.parts[2].ID;
		int num3 = human.customParam.hair.parts[0].ID;
		bool isSet2 = CustomDataManager.GetHair_Back(num3).isSet;
		if (num == 0)
		{
			CustomSelectSet selectedData = selects[3].GetSelectedData();
			if (selectedData != null)
			{
				num = selectedData.id;
			}
		}
		if (num2 == 0)
		{
			CustomSelectSet selectedData2 = selects[2].GetSelectedData();
			if (selectedData2 != null)
			{
				num2 = selectedData2.id;
			}
		}
		if (isSet2)
		{
			num3 = CustomDataManager.GetHair_Back(0).id;
		}
		selects[0].SetSelectedNo(-1);
		human.customParam.hair.parts[1].ID = num;
		human.customParam.hair.parts[2].ID = num2;
		human.customParam.hair.parts[0].ID = num3;
	}

	private void UpdateSlotDataName()
	{
		for (int i = 0; i < dataNameTexts.Length; i++)
		{
			((Component)(object)dataNameTexts[i]).gameObject.SetActive(nowTab == -1);
		}
		if (human.sex == SEX.MALE)
		{
			BackHairData hair_Male = CustomDataManager.GetHair_Male(human.customParam.hair.parts[0].ID);
			dataNameTexts[0].text = ((hair_Male == null) ? string.Empty : hair_Male.name);
			dataNameTexts[1].text = string.Empty;
			dataNameTexts[2].text = string.Empty;
			dataNameTexts[3].text = string.Empty;
			return;
		}
		BackHairData hair_Back = CustomDataManager.GetHair_Back(human.customParam.hair.parts[0].ID);
		HairData hair_Side = CustomDataManager.GetHair_Side(human.customParam.hair.parts[2].ID);
		HairData hair_Front = CustomDataManager.GetHair_Front(human.customParam.hair.parts[1].ID);
		dataNameTexts[0].text = ((hair_Back == null || !hair_Back.isSet) ? string.Empty : hair_Back.name);
		dataNameTexts[1].text = ((hair_Back == null || hair_Back.isSet) ? string.Empty : hair_Back.name);
		dataNameTexts[2].text = ((hair_Side == null) ? string.Empty : hair_Side.name);
		dataNameTexts[3].text = ((hair_Front == null) ? string.Empty : hair_Front.name);
		for (int j = 0; j < dataNameTexts.Length; j++)
		{
			if (dataNameTexts[j].text == "なし")
			{
				dataNameTexts[j].text = string.Empty;
			}
		}
	}

	private void OnChangeSetAllColor(bool set)
	{
		setAllColor = set;
	}

	private void OnChangeColor_Hair_All(Color color)
	{
		HairPartParameter[] parts = human.customParam.hair.parts;
		foreach (HairPartParameter hairPartParameter in parts)
		{
			hairPartParameter.hairColor.mainColor = color;
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_Hair(PART part, Color color)
	{
		if (setAllColor || part == PART.SET)
		{
			OnChangeColor_Hair_All(color);
			return;
		}
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		human.customParam.hair.parts[(int)hAIR_TYPE].hairColor.mainColor = color;
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_Cuticle_All(Color color)
	{
		HairPartParameter[] parts = human.customParam.hair.parts;
		foreach (HairPartParameter hairPartParameter in parts)
		{
			hairPartParameter.hairColor.cuticleColor = color;
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_Cuticle(PART part, Color color)
	{
		if (setAllColor || part == PART.SET)
		{
			OnChangeColor_Cuticle_All(color);
			return;
		}
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		human.customParam.hair.parts[(int)hAIR_TYPE].hairColor.cuticleColor = color;
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnHairCuticleExp_All(float val)
	{
		val = Mathf.Lerp(0f, 20f, val * 0.01f);
		HairPartParameter[] parts = human.customParam.hair.parts;
		foreach (HairPartParameter hairPartParameter in parts)
		{
			hairPartParameter.hairColor.cuticleExp = val;
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnHairCuticleExp(PART part, float val)
	{
		if (setAllColor || part == PART.SET)
		{
			OnHairCuticleExp_All(val);
			return;
		}
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		val = Mathf.Lerp(0f, 20f, val * 0.01f);
		human.customParam.hair.parts[(int)hAIR_TYPE].hairColor.cuticleExp = val;
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_Fresnel_All(Color color)
	{
		HairPartParameter[] parts = human.customParam.hair.parts;
		foreach (HairPartParameter hairPartParameter in parts)
		{
			hairPartParameter.hairColor.fresnelColor = color;
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_Fresnel(PART part, Color color)
	{
		if (setAllColor || part == PART.SET)
		{
			OnChangeColor_Fresnel_All(color);
			return;
		}
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		human.customParam.hair.parts[(int)hAIR_TYPE].hairColor.fresnelColor = color;
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnHairFresnelExp_All(float val)
	{
		val = Mathf.Lerp(0f, 8f, val * 0.01f);
		HairPartParameter[] parts = human.customParam.hair.parts;
		foreach (HairPartParameter hairPartParameter in parts)
		{
			hairPartParameter.hairColor.fresnelExp = val;
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnHairFresnelExp(PART part, float val)
	{
		if (setAllColor || part == PART.SET)
		{
			OnHairFresnelExp_All(val);
			return;
		}
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		val = Mathf.Lerp(0f, 8f, val * 0.01f);
		human.customParam.hair.parts[(int)hAIR_TYPE].hairColor.fresnelExp = val;
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_Acce_All(Color color)
	{
		for (int i = 0; i < human.customParam.hair.parts.Length; i++)
		{
			HairPartParameter hairPartParameter = human.customParam.hair.parts[i];
			HAIR_TYPE hairType = (HAIR_TYPE)i;
			if (hairPartParameter.acceColor != null)
			{
				hairPartParameter.acceColor.mainColor1 = color;
				MaterialCustomData.SetHairAcce(hairType, hairPartParameter);
			}
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_Acce(PART part, Color color)
	{
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		human.customParam.hair.parts[(int)hAIR_TYPE].acceColor.mainColor1 = color;
		human.hairs.ChangeColor(human.customParam.hair);
		MaterialCustomData.SetHairAcce(hAIR_TYPE, human.customParam.hair.parts[(int)hAIR_TYPE]);
	}

	private void OnChangeColor_AcceSpec_All(Color color)
	{
		for (int i = 0; i < human.customParam.hair.parts.Length; i++)
		{
			HairPartParameter hairPartParameter = human.customParam.hair.parts[i];
			HAIR_TYPE hairType = (HAIR_TYPE)i;
			if (hairPartParameter.acceColor != null)
			{
				hairPartParameter.acceColor.specColor1 = color;
				MaterialCustomData.SetHairAcce(hairType, hairPartParameter);
			}
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnChangeColor_AcceSpec(PART part, Color color)
	{
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		human.customParam.hair.parts[(int)hAIR_TYPE].acceColor.specColor1 = color;
		human.hairs.ChangeColor(human.customParam.hair);
		MaterialCustomData.SetHairAcce(hAIR_TYPE, human.customParam.hair.parts[(int)hAIR_TYPE]);
	}

	private void OnAcceSpecular_All(float val)
	{
		for (int i = 0; i < human.customParam.hair.parts.Length; i++)
		{
			HairPartParameter hairPartParameter = human.customParam.hair.parts[i];
			HAIR_TYPE hairType = (HAIR_TYPE)i;
			if (hairPartParameter.acceColor != null)
			{
				hairPartParameter.acceColor.specular1 = val * 0.01f;
				MaterialCustomData.SetHairAcce(hairType, hairPartParameter);
			}
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnAcceSpecular(PART part, float val)
	{
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		human.customParam.hair.parts[(int)hAIR_TYPE].acceColor.specular1 = val * 0.01f;
		human.hairs.ChangeColor(human.customParam.hair);
		MaterialCustomData.SetHairAcce(hAIR_TYPE, human.customParam.hair.parts[(int)hAIR_TYPE]);
	}

	private void OnAcceSmooth_All(float val)
	{
		for (int i = 0; i < human.customParam.hair.parts.Length; i++)
		{
			HairPartParameter hairPartParameter = human.customParam.hair.parts[i];
			HAIR_TYPE hairType = (HAIR_TYPE)i;
			if (hairPartParameter.acceColor != null)
			{
				hairPartParameter.acceColor.smooth1 = val * 0.01f;
				MaterialCustomData.SetHairAcce(hairType, hairPartParameter);
			}
		}
		human.hairs.ChangeColor(human.customParam.hair);
	}

	private void OnAcceSmooth(PART part, float val)
	{
		HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
		switch (part)
		{
		case PART.BACK:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		case PART.SIDE:
			hAIR_TYPE = HAIR_TYPE.SIDE;
			break;
		case PART.FRONT:
			hAIR_TYPE = HAIR_TYPE.FRONT;
			break;
		case PART.SET:
			hAIR_TYPE = HAIR_TYPE.BACK;
			break;
		}
		human.customParam.hair.parts[(int)hAIR_TYPE].acceColor.smooth1 = val * 0.01f;
		human.hairs.ChangeColor(human.customParam.hair);
		MaterialCustomData.SetHairAcce(hAIR_TYPE, human.customParam.hair.parts[(int)hAIR_TYPE]);
	}

	private void CloseMoveableUI()
	{
		for (int i = 0; i < selects.Length; i++)
		{
		}
	}

	private void OnToggleButton_ColorCopyHelper(bool flag)
	{
		SystemSE.SE se = ((!flag) ? SystemSE.SE.CLOSE : SystemSE.SE.OPEN);
		SystemSE.Play(se);
		if (flag)
		{
			colorCopyHelper.moveable.Open();
		}
		else
		{
			colorCopyHelper.moveable.Close();
		}
	}

	private void OnHairPreset(PART part, CustomSelectSet set)
	{
		if (setAllColor || part == PART.SET)
		{
			OnHairPreset_All(set);
		}
		else
		{
			OnHairPreset_One(part, set);
		}
	}

	private void OnHairPreset_One(PART part, CustomSelectSet set)
	{
		if (set.id >= 0)
		{
			HairParameter hair = human.customParam.hair;
			ColorParameter_Hair colorParameter_Hair = presetColors[set.id];
			HAIR_TYPE hAIR_TYPE = HAIR_TYPE.NUM;
			switch (part)
			{
			case PART.BACK:
				hAIR_TYPE = HAIR_TYPE.BACK;
				break;
			case PART.SIDE:
				hAIR_TYPE = HAIR_TYPE.SIDE;
				break;
			case PART.FRONT:
				hAIR_TYPE = HAIR_TYPE.FRONT;
				break;
			case PART.SET:
				hAIR_TYPE = HAIR_TYPE.BACK;
				break;
			}
			ColorParameter_Hair hairColor = hair.parts[(int)hAIR_TYPE].hairColor;
			hairColor.mainColor = colorParameter_Hair.mainColor;
			hairColor.cuticleColor = colorParameter_Hair.cuticleColor;
			hairColor.cuticleExp = colorParameter_Hair.cuticleExp;
			hairColor.fresnelColor = colorParameter_Hair.fresnelColor;
			hairColor.fresnelExp = colorParameter_Hair.fresnelExp;
			human.hairs.ChangeColor(hair);
			UpdateHairColorUI(nowTab);
		}
	}

	private void OnHairPreset_All(CustomSelectSet set)
	{
		if (set.id >= 0)
		{
			HairParameter hair = human.customParam.hair;
			ColorParameter_Hair colorParameter_Hair = presetColors[set.id];
			HairPartParameter[] parts = human.customParam.hair.parts;
			foreach (HairPartParameter hairPartParameter in parts)
			{
				hairPartParameter.hairColor.mainColor = colorParameter_Hair.mainColor;
				hairPartParameter.hairColor.cuticleColor = colorParameter_Hair.cuticleColor;
				hairPartParameter.hairColor.cuticleExp = colorParameter_Hair.cuticleExp;
				hairPartParameter.hairColor.fresnelColor = colorParameter_Hair.fresnelColor;
				hairPartParameter.hairColor.fresnelExp = colorParameter_Hair.fresnelExp;
			}
			human.hairs.ChangeColor(hair);
			UpdateHairColorUI(nowTab);
		}
	}

	private void OnChangeColorCopyHelperMoveableState(MoveableUI.STATE state)
	{
		if (state == MoveableUI.STATE.CLOSED)
		{
			colorCopyToggle.ChangeValue(false, false);
		}
	}
}
