using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BodyCustomEdit : MonoBehaviour
{
	private enum TAB
	{
		GENERAL = 0,
		BUST = 1,
		UPPER = 2,
		LOWER = 3,
		ARM = 4,
		LEG = 5,
		NAIL = 6,
		UNDERHAIR = 7,
		SUNBURN = 8,
		TATTOO = 9,
		NUM = 10
	}

	private class SliderData
	{
		public int id;

		public string name;

		public TAB type;

		public SliderData(int id, string name, TAB type)
		{
			this.id = id;
			this.name = name;
			this.type = type;
		}
	}

	public class EasyData
	{
		public int no = -1;

		public float min;

		public float max = 1f;

		public EasyData(int no, float min, float max)
		{
			this.no = no;
			this.min = min;
			this.max = max;
		}
	}

	private static readonly SliderData[] datas_F = new SliderData[33]
	{
		new SliderData(0, "身長", TAB.GENERAL),
		new SliderData(1, "胸サイズ", TAB.BUST),
		new SliderData(2, "胸上下位置", TAB.BUST),
		new SliderData(3, "胸の左右開き", TAB.BUST),
		new SliderData(4, "胸の左右位置", TAB.BUST),
		new SliderData(5, "胸上下角度", TAB.BUST),
		new SliderData(6, "胸の尖り", TAB.BUST),
		new SliderData(7, "乳輪の膨らみ", TAB.BUST),
		new SliderData(8, "乳首太さ", TAB.BUST),
		new SliderData(9, "頭サイズ", TAB.GENERAL),
		new SliderData(10, "首周り幅", TAB.UPPER),
		new SliderData(11, "首周り奥", TAB.UPPER),
		new SliderData(12, "胴体肩周り幅", TAB.UPPER),
		new SliderData(13, "胴体肩周り奥", TAB.UPPER),
		new SliderData(14, "胴体上幅", TAB.UPPER),
		new SliderData(15, "胴体上奥", TAB.UPPER),
		new SliderData(16, "胴体下幅", TAB.UPPER),
		new SliderData(17, "胴体下奥", TAB.UPPER),
		new SliderData(18, "ウエスト位置", TAB.LOWER),
		new SliderData(19, "腰上幅", TAB.LOWER),
		new SliderData(20, "腰上奥", TAB.LOWER),
		new SliderData(21, "腰下幅", TAB.LOWER),
		new SliderData(22, "腰下奥", TAB.LOWER),
		new SliderData(23, "尻", TAB.LOWER),
		new SliderData(24, "尻角度", TAB.LOWER),
		new SliderData(25, "太もも上", TAB.LEG),
		new SliderData(26, "太もも下", TAB.LEG),
		new SliderData(27, "ふくらはぎ", TAB.LEG),
		new SliderData(28, "足首", TAB.LEG),
		new SliderData(29, "肩", TAB.ARM),
		new SliderData(30, "上腕", TAB.ARM),
		new SliderData(31, "前腕", TAB.ARM),
		new SliderData(32, "乳首立ち", TAB.BUST)
	};

	private static readonly SliderData[] datas_M = new SliderData[20]
	{
		new SliderData(1, "胸サイズ", TAB.BUST),
		new SliderData(2, "胸上下位置", TAB.BUST),
		new SliderData(3, "胸の左右開き", TAB.BUST),
		new SliderData(4, "胸上下角度", TAB.BUST),
		new SliderData(5, "頭サイズ", TAB.GENERAL),
		new SliderData(6, "首周り", TAB.UPPER),
		new SliderData(7, "胴体肩周り", TAB.UPPER),
		new SliderData(8, "胴体上", TAB.UPPER),
		new SliderData(9, "胴体下", TAB.UPPER),
		new SliderData(12, "腹", TAB.UPPER),
		new SliderData(10, "腰上", TAB.LOWER),
		new SliderData(11, "腰下", TAB.LOWER),
		new SliderData(13, "尻", TAB.LOWER),
		new SliderData(14, "太もも上", TAB.LEG),
		new SliderData(15, "太もも下", TAB.LEG),
		new SliderData(16, "ふくらはぎ", TAB.LEG),
		new SliderData(17, "足首", TAB.LEG),
		new SliderData(18, "肩", TAB.ARM),
		new SliderData(19, "上腕", TAB.ARM),
		new SliderData(20, "前腕", TAB.ARM)
	};

	private static readonly string CustomHighlightTexPath_F = "Custom Point F/Texture/cf_t_body_00_no";

	private static readonly string CustomHighlightTexPath_M = "Custom Point M/Texture/cm_t_body_00_no";

	private string CustomHighlightTexPath;

	private static readonly EasyData[] easy_F = new EasyData[22]
	{
		new EasyData(10, 0f, 1f),
		new EasyData(11, 0f, 1f),
		new EasyData(12, 0f, 1f),
		new EasyData(13, 0f, 1f),
		new EasyData(14, 0f, 1f),
		new EasyData(15, 0f, 1f),
		new EasyData(16, 0f, 1f),
		new EasyData(17, 0f, 1f),
		new EasyData(18, 1f, 0f),
		new EasyData(19, 0f, 1f),
		new EasyData(20, 0f, 1f),
		new EasyData(21, 0f, 1f),
		new EasyData(22, 0f, 1f),
		new EasyData(23, 0f, 1f),
		new EasyData(24, 1f, 0f),
		new EasyData(25, 0f, 1f),
		new EasyData(26, 0f, 1f),
		new EasyData(27, 0f, 1f),
		new EasyData(28, 0f, 1f),
		new EasyData(29, 0f, 1f),
		new EasyData(30, 0f, 1f),
		new EasyData(31, 0f, 1f)
	};

	private static readonly EasyData[] easy_M = new EasyData[15]
	{
		new EasyData(6, 0f, 1f),
		new EasyData(7, 0f, 1f),
		new EasyData(8, 0f, 1f),
		new EasyData(9, 0f, 1f),
		new EasyData(12, 0f, 1f),
		new EasyData(10, 0f, 1f),
		new EasyData(11, 0f, 1f),
		new EasyData(13, 0f, 1f),
		new EasyData(14, 0f, 1f),
		new EasyData(15, 0f, 1f),
		new EasyData(16, 0f, 1f),
		new EasyData(17, 0f, 1f),
		new EasyData(18, 0f, 1f),
		new EasyData(19, 0f, 1f),
		new EasyData(20, 0f, 1f)
	};

	[SerializeField]
	private EditMode editMode;

	private EditEquipShow equipShow;

	[SerializeField]
	private ToggleButton[] toggles = new ToggleButton[10];

	[SerializeField]
	private GameObject[] tabMains = new GameObject[10];

	[SerializeField]
	private ToggleButton colorCopyToggle;

	[SerializeField]
	private CharaColorCopyHelper colorCopyHelper;

	[SerializeField]
	private TextAsset presetText;

	private List<string> presetNames = new List<string>();

	private List<Vector3> presetColors = new List<Vector3>();

	private PresetSelectUISets presetSelectUI_Skin;

	private PresetSelectUISets presetSelectUI_Burn;

	private InputSliderUI[] sliders;

	private SwitchUI easySwitch;

	private InputSliderUI easySlider;

	private InputSliderUI skin_H;

	private InputSliderUI skin_S;

	private InputSliderUI skin_V;

	private InputSliderUI specular_skin;

	private InputSliderUI smooth_skin;

	private InputSliderUI areolaSize;

	private InputSliderUI nip_H;

	private InputSliderUI nip_S;

	private InputSliderUI nip_V;

	private InputSliderUI nip_A;

	private InputSliderUI specular_nip;

	private InputSliderUI smooth_nip;

	private ColorChangeButton color_underhair;

	private InputSliderUI nail_H;

	private InputSliderUI nail_S;

	private InputSliderUI nail_V;

	private InputSliderUI specular_nail;

	private InputSliderUI smooth_nail;

	private ColorChangeButton manicure_Color;

	private InputSliderUI specular_manicure;

	private InputSliderUI smooth_manicure;

	private InputSliderUI sunburn_H;

	private InputSliderUI sunburn_S;

	private InputSliderUI sunburn_V;

	private InputSliderUI sunburn_A;

	private ColorChangeButton color_tattoo;

	private ItemSelectUISets selSets_Skin;

	private InputSliderUI detailRate;

	private Text nipLabel;

	private ItemSelectUISets selSets_Nip;

	private ItemSelectUISets selSets_UnderHair;

	private ItemSelectUISets selSets_Sunburn;

	private ItemSelectUISets selSets_Tattoo;

	private InputSliderUI bustSoft;

	private InputSliderUI bustWeight;

	private Human human;

	private int nowTab = -1;

	private float customMatAlpha = 1f;

	private int onCursorNo = -1;

	private int highlightNo = -1;

	private bool showHighlight;

	private Texture highlightTex;

	private bool invoke = true;

	private bool easyCustomFlag;

	private float easyCustomVal = 0.5f;

	public void Setup(Human human, EditEquipShow equipShow)
	{
		this.human = human;
		this.equipShow = equipShow;
		CustomHighlightTexPath = ((human.sex != 0) ? CustomHighlightTexPath_M : CustomHighlightTexPath_F);
		SetupPreset();
		human.body.CreateCustomHighlightMaterial();
		customMatAlpha = human.body.CustomHighlightMat_Skin.color.a;
		Color color = human.body.CustomHighlightMat_Skin.color;
		color.a = 0f;
		human.body.CustomHighlightMat_Skin.color = color;
		SliderData[] array = ((human.sex != 0) ? datas_M : datas_F);
		sliders = new InputSliderUI[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			GameObject parent = tabMains[(int)array[i].type];
			InputSliderUI inputSliderUI = editMode.CreateInputSliderUI(parent, array[i].name, 0f, 100f, true, 50f, null);
			sliders[i] = inputSliderUI;
			EventTrigger eventTrigger = inputSliderUI.GetEventTrigger();
			int id = array[i].id;
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate
			{
				PointerEnter(id);
			});
			eventTrigger.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			entry2.callback.AddListener(delegate
			{
				PointerExit(id);
			});
			eventTrigger.triggers.Add(entry2);
		}
		for (int j = 0; j < tabMains.Length; j++)
		{
			tabMains[j].gameObject.SetActive(false);
		}
		onCursorNo = -1;
		highlightNo = -1;
		showHighlight = false;
		easySwitch = editMode.CreateSwitchUI(tabMains[0], "簡易体型設定", false, OnChangeSwitch_Easy);
		easySlider = editMode.CreateInputSliderUI(tabMains[0], "簡易体型", 0f, 100f, true, 50f, OnChangeSlider_Easy);
		bustSoft = editMode.CreateInputSliderUI(tabMains[1], "胸の柔らかさ", 0f, 100f, true, 50f, OnChangeSlider_BustSoft);
		bustWeight = editMode.CreateInputSliderUI(tabMains[1], "胸の重さ", 0f, 100f, true, 50f, OnChangeSlider_BustWeight);
		editMode.CreateLabel(tabMains[0], "肌");
		selSets_Skin = editMode.CreateItemSelectUISets(tabMains[0], "肌", editMode.thumnbs_bodySkin, OnChangeSkin);
		detailRate = editMode.CreateInputSliderUI(tabMains[0], "肉感の強さ", 0f, 100f, false, 0f, OnChangeDetailRate);
		skin_H = editMode.CreateInputSliderUI(tabMains[0], "色相", -50f, 50f, true, 0f, OnChangeColor_SkinH);
		skin_S = editMode.CreateInputSliderUI(tabMains[0], "彩度", 0f, 100f, true, 50f, OnChangeColor_SkinS);
		skin_V = editMode.CreateInputSliderUI(tabMains[0], "明度", 0f, 100f, true, 50f, OnChangeColor_SkinV);
		specular_skin = editMode.CreateInputSliderUI(tabMains[0], "ツヤの強さ", 0f, 100f, true, 0f, OnChangeSlider_SkinSpecular);
		smooth_skin = editMode.CreateInputSliderUI(tabMains[0], "ツヤの質感", 0f, 100f, true, 70.25f, OnChangeSlider_SkinSmooth);
		List<CustomSelectSet> list = new List<CustomSelectSet>();
		for (int k = 0; k < presetNames.Count; k++)
		{
			list.Add(new CustomSelectSet(k, presetNames[k], null, null, false));
		}
		presetSelectUI_Skin = editMode.CreatePresetSelectUISets(tabMains[0], "プリセット", list, OnPreset_Skin);
		nipLabel = editMode.CreateLabel(tabMains[1], "乳首");
		selSets_Nip = editMode.CreateItemSelectUISets(tabMains[1], "乳首", editMode.thumnbs_nip, OnChangeNip);
		areolaSize = editMode.CreateInputSliderUI(tabMains[1], "乳輪の大きさ", 0f, 100f, true, 70f, OnChangeAreolaSize);
		nip_H = editMode.CreateInputSliderUI(tabMains[1], "色相", -50f, 50f, true, 0f, OnChangeColor_NipH);
		nip_S = editMode.CreateInputSliderUI(tabMains[1], "彩度", 0f, 100f, true, 50f, OnChangeColor_NipS);
		nip_V = editMode.CreateInputSliderUI(tabMains[1], "明度", 0f, 100f, true, 50f, OnChangeColor_NipV);
		nip_A = editMode.CreateInputSliderUI(tabMains[1], "透明", 0f, 100f, false, 100f, OnChangeColor_NipA);
		specular_nip = editMode.CreateInputSliderUI(tabMains[1], "ツヤの強さ", 0f, 100f, true, 0f, OnChangeSlider_NipSpecular);
		smooth_nip = editMode.CreateInputSliderUI(tabMains[1], "ツヤの質感", 0f, 100f, true, 70.25f, OnChangeSlider_NipSmooth);
		selSets_UnderHair = editMode.CreateItemSelectUISets(tabMains[7], "陰毛", editMode.thumnbs_underhair, OnChangeUnderHair);
		color_underhair = editMode.CreateColorChangeButton(tabMains[7], "色", Color.white, true, OnChangeColor_UnderHair);
		selSets_Sunburn = editMode.CreateItemSelectUISets(tabMains[8], "日焼け", editMode.thumnbs_sunburn, OnChangeSunburn);
		sunburn_H = editMode.CreateInputSliderUI(tabMains[8], "色相", -50f, 50f, true, 0f, OnChangeSlider_SunburnH);
		sunburn_S = editMode.CreateInputSliderUI(tabMains[8], "彩度", 0f, 100f, true, 50f, OnChangeSlider_SunburnS);
		sunburn_V = editMode.CreateInputSliderUI(tabMains[8], "明度", 0f, 100f, true, 50f, OnChangeSlider_SunburnV);
		sunburn_A = editMode.CreateInputSliderUI(tabMains[8], "濃度", 0f, 100f, false, 0f, OnChangeSlider_SunburnA);
		List<CustomSelectSet> list2 = new List<CustomSelectSet>();
		for (int l = 0; l < presetNames.Count; l++)
		{
			list2.Add(new CustomSelectSet(l, presetNames[l], null, null, false));
		}
		presetSelectUI_Burn = editMode.CreatePresetSelectUISets(tabMains[8], "プリセット", list2, OnPreset_Burn);
		selSets_Tattoo = editMode.CreateItemSelectUISets(tabMains[9], "タトゥー", editMode.thumnbs_bodyTattoo, OnChangeTattoo);
		color_tattoo = editMode.CreateColorChangeButton(tabMains[9], "色", Color.white, true, OnChangeColor_Tattoo);
		editMode.CreateLabel(tabMains[6], "爪");
		nail_H = editMode.CreateInputSliderUI(tabMains[6], "色相", -50f, 50f, true, 0f, OnChangeColor_NailH);
		nail_S = editMode.CreateInputSliderUI(tabMains[6], "彩度", 0f, 100f, true, 50f, OnChangeColor_NailS);
		nail_V = editMode.CreateInputSliderUI(tabMains[6], "明度", 0f, 100f, true, 50f, OnChangeColor_NailV);
		specular_nail = editMode.CreateInputSliderUI(tabMains[6], "ツヤの強さ", 0f, 100f, true, 0f, OnChangeSlider_NailSpecular);
		smooth_nail = editMode.CreateInputSliderUI(tabMains[6], "ツヤの質感", 0f, 100f, true, 60f, OnChangeSlider_NailSmooth);
		editMode.CreateLabel(tabMains[6], "マニキュア");
		manicure_Color = editMode.CreateColorChangeButton(tabMains[6], "色", Color.white, true, OnChangeColor_ManicureColor);
		specular_manicure = editMode.CreateInputSliderUI(tabMains[6], "ツヤの強さ", 0f, 100f, false, 0f, OnChangeSlider_ManicureSpecular);
		smooth_manicure = editMode.CreateInputSliderUI(tabMains[6], "ツヤの質感", 0f, 100f, false, 0f, OnChangeSlider_ManicureSmooth);
		bool active = human.sex == SEX.FEMALE;
		((Component)(object)nipLabel).gameObject.SetActive(active);
		selSets_Nip.toggle.gameObject.SetActive(active);
		areolaSize.gameObject.SetActive(active);
		nip_H.gameObject.SetActive(active);
		nip_S.gameObject.SetActive(active);
		nip_V.gameObject.SetActive(active);
		nip_A.gameObject.SetActive(active);
		specular_nip.gameObject.SetActive(active);
		smooth_nip.gameObject.SetActive(active);
		selSets_UnderHair.toggle.gameObject.SetActive(active);
		selSets_Sunburn.toggle.gameObject.SetActive(active);
		sunburn_H.gameObject.SetActive(active);
		sunburn_S.gameObject.SetActive(active);
		sunburn_V.gameObject.SetActive(active);
		sunburn_A.gameObject.SetActive(active);
		nail_H.gameObject.SetActive(active);
		nail_S.gameObject.SetActive(active);
		nail_V.gameObject.SetActive(active);
		specular_nail.gameObject.SetActive(active);
		smooth_nail.gameObject.SetActive(active);
		manicure_Color.gameObject.SetActive(active);
		specular_manicure.gameObject.SetActive(active);
		smooth_manicure.gameObject.SetActive(active);
		bustSoft.gameObject.SetActive(active);
		bustWeight.gameObject.SetActive(active);
		toggles[6].gameObject.SetActive(active);
		toggles[8].gameObject.SetActive(active);
		LoadedHuman();
		colorCopyToggle.action.AddListener(OnToggleButton_ColorCopyHelper);
		colorCopyHelper.moveable.AddOnChange(OnChangeColorCopyHelperMoveableState);
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
				Vector3 item = default(Vector3);
				string empty = string.Empty;
				empty = TagTextUtility.Load_String(element, "name");
				item.x = TagTextUtility.Load_Float(element, "H");
				item.y = TagTextUtility.Load_Float(element, "S");
				item.z = TagTextUtility.Load_Float(element, "V");
				presetNames.Add(empty);
				presetColors.Add(item);
			}
		}
	}

	private void OnEnable()
	{
		if (equipShow != null)
		{
			equipShow.SetAuto(EditEquipShow.WEARSHOW.NUDE);
		}
		colorCopyToggle.ChangeValue(colorCopyHelper.gameObject.activeSelf, false);
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
		SliderData[] array = ((human.sex != 0) ? datas_M : datas_F);
		for (int j = 0; j < sliders.Length; j++)
		{
			if (array[j].type == (TAB)nowTab)
			{
				float value = sliders[j].Value * 0.01f;
				human.body.SetShape(array[j].id, value);
			}
		}
		UpdateHighlight();
		easySlider.gameObject.SetActive(easyCustomFlag);
		toggles[2].Interactable = !easyCustomFlag;
		toggles[3].Interactable = !easyCustomFlag;
		toggles[4].Interactable = !easyCustomFlag;
		toggles[5].Interactable = !easyCustomFlag;
	}

	private void OnChangeColorCopyHelperMoveableState(MoveableUI.STATE state)
	{
		if (state == MoveableUI.STATE.CLOSED)
		{
			colorCopyToggle.ChangeValue(false, false);
		}
	}

	public void ChangeSlider()
	{
	}

	public void LoadedHuman()
	{
		invoke = false;
		BodyParameter body = human.customParam.body;
		SliderData[] array = ((human.sex != 0) ? datas_M : datas_F);
		for (int i = 0; i < sliders.Length; i++)
		{
			if (array[i].type == (TAB)nowTab)
			{
				sliders[i].Value = human.body.GetShape(array[i].id) * 100f;
			}
		}
		selSets_Skin.SetSelectedFromDataID(body.bodyID);
		detailRate.SetValue(body.detailWeight * 100f);
		skin_H.SetValue(body.skinColor.offset_h * 100f);
		skin_S.SetValue(SVtoSliderVal(body.skinColor.offset_s));
		skin_V.SetValue(SVtoSliderVal(body.skinColor.offset_v));
		specular_skin.SetValue(body.skinColor.metallic * 100f * 2.5f);
		smooth_skin.SetValue(body.skinColor.smooth * 100f * 1.25f);
		bustSoft.SetValue(body.bustSoftness * 100f);
		bustWeight.SetValue(body.bustWeight * 100f);
		selSets_Nip.SetSelectedFromDataID(body.nipID);
		areolaSize.SetValue(body.areolaSize * 100f);
		nip_H.SetValue(body.nipColor.offset_h * 100f);
		nip_S.SetValue(SVtoSliderVal(body.nipColor.offset_s));
		nip_V.SetValue(SVtoSliderVal(body.nipColor.offset_v));
		nip_A.SetValue(body.nipColor.alpha * 100f);
		specular_nip.SetValue(body.nipColor.metallic * 100f * 2.5f);
		smooth_nip.SetValue(body.nipColor.smooth * 100f * 1.25f);
		selSets_UnderHair.SetSelectedFromDataID(body.underhairID);
		color_underhair.SetColor(body.underhairColor.mainColor);
		selSets_Sunburn.SetSelectedFromDataID(body.sunburnID);
		sunburn_H.SetValue(body.sunburnColor_H * 100f);
		sunburn_S.SetValue(SVtoSliderVal(body.sunburnColor_S));
		sunburn_V.SetValue(SVtoSliderVal(body.sunburnColor_V));
		sunburn_A.SetValue(body.sunburnColor_A * 100f);
		selSets_Tattoo.SetSelectedFromDataID(body.tattooID);
		color_tattoo.SetColor(body.tattooColor);
		nail_H.SetValue(body.nailColor.offset_h * 100f);
		nail_S.SetValue(SVtoSliderVal(body.nailColor.offset_s));
		nail_V.SetValue(SVtoSliderVal(body.nailColor.offset_v));
		specular_nail.SetValue(body.nailColor.metallic * 100f);
		smooth_nail.SetValue(body.nailColor.smooth * 100f);
		manicure_Color.SetColor(body.manicureColor.mainColor1);
		specular_manicure.SetValue(body.manicureColor.specular1 * 100f);
		smooth_manicure.SetValue(body.manicureColor.smooth1 * 100f);
		invoke = true;
	}

	public void ChangedColor()
	{
		invoke = false;
		BodyParameter body = human.customParam.body;
		skin_H.SetValue(body.skinColor.offset_h * 100f);
		skin_S.SetValue(SVtoSliderVal(body.skinColor.offset_s));
		skin_V.SetValue(SVtoSliderVal(body.skinColor.offset_v));
		nip_H.SetValue(body.nipColor.offset_h * 100f);
		nip_S.SetValue(SVtoSliderVal(body.nipColor.offset_s));
		nip_V.SetValue(SVtoSliderVal(body.nipColor.offset_v));
		nip_A.SetValue(body.nipColor.alpha * 100f);
		color_underhair.SetColor(body.underhairColor.mainColor);
		sunburn_H.SetValue(body.sunburnColor_H * 100f);
		sunburn_S.SetValue(SVtoSliderVal(body.sunburnColor_S));
		sunburn_V.SetValue(SVtoSliderVal(body.sunburnColor_V));
		sunburn_A.SetValue(body.sunburnColor_A * 100f);
		nail_H.SetValue(body.nailColor.offset_h * 100f);
		nail_S.SetValue(SVtoSliderVal(body.nailColor.offset_s));
		nail_V.SetValue(SVtoSliderVal(body.nailColor.offset_v));
		invoke = true;
	}

	private static float SVtoSliderVal(float v)
	{
		return v * 50f;
	}

	private static float SliderValtoSV(float v)
	{
		return v * 0.02f;
	}

	private void ChangeTab(int tab)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		SliderData[] array = ((human.sex != 0) ? datas_M : datas_F);
		nowTab = tab;
		if (nowTab == -1)
		{
			return;
		}
		for (int i = 0; i < sliders.Length; i++)
		{
			if (array[i].type == (TAB)nowTab)
			{
				sliders[i].Value = human.body.GetShape(array[i].id) * 100f;
			}
		}
	}

	private void PointerEnter(int no)
	{
		onCursorNo = no;
	}

	private void PointerExit(int no)
	{
		if (onCursorNo == no)
		{
			onCursorNo = -1;
		}
	}

	private void ChangeCustomPart(int newNo)
	{
		if (showHighlight && ConfigData.showCustomHighlight)
		{
			if (newNo == highlightNo)
			{
				return;
			}
			highlightNo = newNo;
			if (newNo != -1)
			{
				Texture texture = highlightTex;
				string path = CustomHighlightTexPath + highlightNo.ToString("00");
				highlightTex = Resources.Load<Texture>(path);
				human.body.CustomHighlightMat_Skin.mainTexture = highlightTex;
				float alpha = ((!highlightTex) ? 0f : customMatAlpha);
				ChangeCustomAlpha(human.body.CustomHighlightMat_Skin, alpha);
				if (texture != null && texture != highlightTex)
				{
					Resources.UnloadAsset(texture);
				}
			}
			else
			{
				ChangeCustomAlpha(human.body.CustomHighlightMat_Skin, 0f);
			}
		}
		else
		{
			ChangeCustomAlpha(human.body.CustomHighlightMat_Skin, 0f);
		}
	}

	private static void ChangeCustomAlpha(Material mat, float alpha)
	{
		Color color = mat.color;
		color.a = alpha;
		mat.color = color;
	}

	private void UpdateHighlight()
	{
		int newNo = onCursorNo;
		showHighlight = true;
		InputSliderUI[] array = sliders;
		foreach (InputSliderUI inputSliderUI in array)
		{
			if (inputSliderUI.IsHandling)
			{
				showHighlight = false;
			}
		}
		ChangeCustomPart(newNo);
	}

	private void OnChangeSkin(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.body.bodyID = set.id;
			human.body.ChangeSkinMaterial();
			human.body.RendSkinTexture();
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeDetailRate(float value)
	{
		if (invoke)
		{
			human.customParam.body.detailWeight = value * 0.01f;
			human.body.ChangeBumpRate();
		}
	}

	private void OnChangeNip(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.body.nipID = set.id;
			human.body.ChangeNip();
		}
	}

	private void OnChangeUnderHair(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.body.underhairID = set.id;
			human.body.ChangeUnderHair();
		}
	}

	private void OnChangeSunburn(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.body.sunburnID = set.id;
			human.body.ChangeSunburn();
			human.body.RendSkinTexture();
		}
	}

	private void OnChangeTattoo(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.body.tattooID = set.id;
			human.body.ChangeTattoo();
			human.body.RendSkinTexture();
		}
	}

	private void OnChangeColor_SkinH(float val)
	{
		if (invoke)
		{
			human.customParam.body.skinColor.offset_h = val * 0.01f;
			human.body.RendSkinTexture();
			human.UpdateSkinMaterial();
			human.wears.ChangeBodyMaterial(human.body.Rend_skin);
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeColor_SkinS(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.skinColor.offset_s = val;
			human.body.RendSkinTexture();
			human.UpdateSkinMaterial();
			human.wears.ChangeBodyMaterial(human.body.Rend_skin);
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeColor_SkinV(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.skinColor.offset_v = val;
			human.body.RendSkinTexture();
			human.UpdateSkinMaterial();
			human.wears.ChangeBodyMaterial(human.body.Rend_skin);
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeSlider_SkinSpecular(float val)
	{
		if (invoke)
		{
			human.customParam.body.skinColor.metallic = val * 0.004f;
			human.UpdateSkinMaterial();
			human.wears.ChangeBodyMaterial(human.body.Rend_skin);
		}
	}

	private void OnChangeSlider_SkinSmooth(float val)
	{
		if (invoke)
		{
			human.customParam.body.skinColor.smooth = val * 0.008f;
			human.UpdateSkinMaterial();
			human.wears.ChangeBodyMaterial(human.body.Rend_skin);
		}
	}

	private void OnChangeAreolaSize(float val)
	{
		if (invoke)
		{
			human.customParam.body.areolaSize = val * 0.01f;
			Female female = human as Female;
			if (female != null)
			{
				female.ChangeAreoraSize();
			}
		}
	}

	private void OnChangeColor_NipH(float val)
	{
		if (invoke)
		{
			human.customParam.body.nipColor.offset_h = val * 0.01f;
			human.body.ChangeNipColor();
		}
	}

	private void OnChangeColor_NipS(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.nipColor.offset_s = val;
			human.body.ChangeNipColor();
		}
	}

	private void OnChangeColor_NipV(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.nipColor.offset_v = val;
			human.body.ChangeNipColor();
		}
	}

	private void OnChangeColor_NipA(float val)
	{
		if (invoke)
		{
			human.customParam.body.nipColor.alpha = val * 0.01f;
			human.body.ChangeNipColor();
		}
	}

	private void OnChangeSlider_NipSpecular(float val)
	{
		if (invoke)
		{
			human.customParam.body.nipColor.metallic = val * 0.004f;
			human.body.ChangeNipColor();
		}
	}

	private void OnChangeSlider_NipSmooth(float val)
	{
		if (invoke)
		{
			human.customParam.body.nipColor.smooth = val * 0.008f;
			human.body.ChangeNipColor();
		}
	}

	private void OnNipParamFromSkin()
	{
		BodyParameter body = human.customParam.body;
		body.nipColor.offset_h = body.skinColor.offset_h;
		body.nipColor.offset_s = body.skinColor.offset_s;
		body.nipColor.offset_v = body.skinColor.offset_v;
		body.nipColor.metallic = body.skinColor.metallic;
		body.nipColor.smooth = body.skinColor.smooth;
		human.body.ChangeNipColor();
	}

	private void OnNipParamFromSunburn()
	{
		BodyParameter body = human.customParam.body;
		body.nipColor.offset_h = Mathf.Lerp(body.skinColor.offset_h, body.sunburnColor_H, body.sunburnColor_A);
		body.nipColor.offset_s = Mathf.Lerp(body.skinColor.offset_s, body.sunburnColor_S, body.sunburnColor_A);
		body.nipColor.offset_v = Mathf.Lerp(body.skinColor.offset_v, body.sunburnColor_V, body.sunburnColor_A);
		body.nipColor.metallic = body.skinColor.metallic;
		body.nipColor.smooth = body.skinColor.smooth;
		human.body.ChangeNipColor();
	}

	private void OnChangeColor_UnderHair(Color color)
	{
		if (invoke)
		{
			human.customParam.body.underhairColor.mainColor = color;
			human.body.ChangeUnderHairColor();
		}
	}

	private void OnChangeSlider_UnderhairSpecular(float val)
	{
		if (invoke)
		{
			human.customParam.body.underhairColor.metallic = val * 0.01f;
			human.body.ChangeUnderHairColor();
		}
	}

	private void OnChangeSlider_UnderhairSmooth(float val)
	{
		if (invoke)
		{
			human.customParam.body.underhairColor.smooth = val * 0.01f;
			human.body.ChangeUnderHairColor();
		}
	}

	private void OnChangeSlider_SunburnH(float val)
	{
		if (invoke)
		{
			human.customParam.body.sunburnColor_H = val * 0.01f;
			human.body.RendSkinTexture();
		}
	}

	private void OnChangeSlider_SunburnS(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.sunburnColor_S = val;
			human.body.RendSkinTexture();
		}
	}

	private void OnChangeSlider_SunburnV(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.sunburnColor_V = val;
			human.body.RendSkinTexture();
		}
	}

	private void OnChangeSlider_SunburnA(float val)
	{
		if (invoke)
		{
			human.customParam.body.sunburnColor_A = val * 0.01f;
			human.body.RendSkinTexture();
		}
	}

	private void OnChangeColor_Tattoo(Color color)
	{
		if (invoke)
		{
			human.customParam.body.tattooColor = color;
			human.body.RendSkinTexture();
		}
	}

	private void OnChangeColor_NailH(float val)
	{
		if (invoke)
		{
			human.customParam.body.nailColor.offset_h = val * 0.01f;
			human.body.ChangeNail();
		}
	}

	private void OnChangeColor_NailS(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.nailColor.offset_s = val;
			human.body.ChangeNail();
		}
	}

	private void OnChangeColor_NailV(float val)
	{
		if (invoke)
		{
			val = SliderValtoSV(val);
			human.customParam.body.nailColor.offset_v = val;
			human.body.ChangeNail();
		}
	}

	private void OnChangeSlider_NailSpecular(float val)
	{
		if (invoke)
		{
			human.customParam.body.nailColor.metallic = val * 0.01f;
			human.body.ChangeNail();
		}
	}

	private void OnChangeSlider_NailSmooth(float val)
	{
		if (invoke)
		{
			human.customParam.body.nailColor.smooth = val * 0.01f;
			human.body.ChangeNail();
		}
	}

	private void OnChangeColor_ManicureColor(Color color)
	{
		if (invoke)
		{
			human.customParam.body.manicureColor.mainColor1 = color;
			human.body.ChangeManicure();
		}
	}

	private void OnChangeSlider_ManicureSpecular(float val)
	{
		if (invoke)
		{
			human.customParam.body.manicureColor.specular1 = val * 0.01f;
			human.body.ChangeManicure();
		}
	}

	private void OnChangeSlider_ManicureSmooth(float val)
	{
		if (invoke)
		{
			human.customParam.body.manicureColor.smooth1 = val * 0.01f;
			human.body.ChangeManicure();
		}
	}

	private void OnChangeSlider_BustSoft(float val)
	{
		if (invoke)
		{
			human.customParam.body.bustSoftness = val * 0.01f;
			human.body.bustSoft.ReCalc();
		}
	}

	private void OnChangeSlider_BustWeight(float val)
	{
		if (invoke)
		{
			human.customParam.body.bustWeight = val * 0.01f;
			human.body.bustWeight.ReCalc();
		}
	}

	private void OnChangeSwitch_Easy(bool flag)
	{
		if (invoke)
		{
			easyCustomFlag = flag;
			if (easyCustomFlag)
			{
				easySlider.SetValue(easyCustomVal * 100f);
				OnChangeSlider_Easy(easyCustomVal * 100f);
			}
			SystemSE.SE se = (flag ? SystemSE.SE.YES : SystemSE.SE.NO);
			SystemSE.Play(se);
		}
	}

	private void OnChangeSlider_Easy(float val)
	{
		if (invoke)
		{
			easyCustomVal = val * 0.01f;
			EasyData[] array = ((human.sex != 0) ? easy_M : easy_F);
			for (int i = 0; i < array.Length; i++)
			{
				int no = array[i].no;
				float num = Mathf.Lerp(array[i].min, array[i].max, easyCustomVal);
				human.customParam.body.shapeVals[no] = num;
			}
			human.body.ShapeApply();
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

	private void OnPreset_Skin(CustomSelectSet data)
	{
		if (data.id >= 0)
		{
			Vector3 vector = presetColors[data.id];
			skin_H.SetValue(vector.x);
			skin_S.SetValue(vector.y);
			skin_V.SetValue(vector.z);
			human.customParam.body.skinColor.offset_h = vector.x * 0.01f;
			human.customParam.body.skinColor.offset_s = SliderValtoSV(vector.y);
			human.customParam.body.skinColor.offset_v = SliderValtoSV(vector.z);
			human.body.RendSkinTexture();
			human.UpdateSkinMaterial();
			human.wears.ChangeBodyMaterial(human.body.Rend_skin);
			human.head.RendSkinTexture();
		}
	}

	private void OnPreset_Burn(CustomSelectSet data)
	{
		if (data.id >= 0)
		{
			Vector3 vector = presetColors[data.id];
			sunburn_H.SetValue(vector.x);
			sunburn_S.SetValue(vector.y);
			sunburn_V.SetValue(vector.z);
			human.customParam.body.sunburnColor_H = vector.x * 0.01f;
			human.customParam.body.sunburnColor_S = SliderValtoSV(vector.y);
			human.customParam.body.sunburnColor_V = SliderValtoSV(vector.z);
			human.body.RendSkinTexture();
			human.UpdateSkinMaterial();
			human.wears.ChangeBodyMaterial(human.body.Rend_skin);
			human.head.RendSkinTexture();
		}
	}
}
