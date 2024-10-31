using System;
using Character;
using UnityEngine;
using UnityEngine.EventSystems;

public class FaceCustomEdit : MonoBehaviour
{
	private enum TAB
	{
		GENERAL = 0,
		EAR = 1,
		EYEBROW = 2,
		EYELASH = 3,
		ORBITA = 4,
		EYE = 5,
		NOSE = 6,
		CHEEK = 7,
		MOUTH = 8,
		CHIN = 9,
		MOLE = 10,
		MAKEUP = 11,
		TATTOO = 12,
		BEARD = 13,
		NUM = 14
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

	private static readonly SliderData[] datas = new SliderData[67]
	{
		new SliderData(0, "全体横幅", TAB.GENERAL),
		new SliderData(1, "上部前後", TAB.GENERAL),
		new SliderData(2, "上部上下", TAB.GENERAL),
		new SliderData(3, "下部前後", TAB.GENERAL),
		new SliderData(4, "下部横幅", TAB.GENERAL),
		new SliderData(5, "横幅", TAB.CHIN),
		new SliderData(6, "上下", TAB.CHIN),
		new SliderData(7, "前後", TAB.CHIN),
		new SliderData(8, "角度", TAB.CHIN),
		new SliderData(9, "下部上下", TAB.CHIN),
		new SliderData(10, "先幅", TAB.CHIN),
		new SliderData(11, "先上下", TAB.CHIN),
		new SliderData(12, "先前後", TAB.CHIN),
		new SliderData(13, "下部上下", TAB.CHEEK),
		new SliderData(14, "下部前後", TAB.CHEEK),
		new SliderData(15, "下部幅", TAB.CHEEK),
		new SliderData(16, "上部上下", TAB.CHEEK),
		new SliderData(17, "上部前後", TAB.CHEEK),
		new SliderData(18, "上部幅", TAB.CHEEK),
		new SliderData(19, "上下", TAB.EYEBROW),
		new SliderData(20, "横位置", TAB.EYEBROW),
		new SliderData(21, "角度Z軸", TAB.EYEBROW),
		new SliderData(22, "内側形状", TAB.EYEBROW),
		new SliderData(23, "外側形状", TAB.EYEBROW),
		new SliderData(24, "上下", TAB.ORBITA),
		new SliderData(25, "横位置", TAB.ORBITA),
		new SliderData(26, "前後", TAB.ORBITA),
		new SliderData(27, "横幅", TAB.ORBITA),
		new SliderData(28, "縦幅", TAB.ORBITA),
		new SliderData(29, "角度Z軸", TAB.ORBITA),
		new SliderData(30, "角度Y軸", TAB.ORBITA),
		new SliderData(31, "目頭左右位置", TAB.ORBITA),
		new SliderData(32, "目尻左右位置", TAB.ORBITA),
		new SliderData(33, "目頭上下位置", TAB.ORBITA),
		new SliderData(34, "目尻上下位置", TAB.ORBITA),
		new SliderData(35, "まぶた形状１", TAB.ORBITA),
		new SliderData(36, "まぶた形状２", TAB.ORBITA),
		new SliderData(37, "瞳の上下調整", TAB.EYE),
		new SliderData(38, "瞳の横幅", TAB.EYE),
		new SliderData(39, "瞳の縦幅", TAB.EYE),
		new SliderData(40, "全体上下", TAB.NOSE),
		new SliderData(41, "全体前後", TAB.NOSE),
		new SliderData(42, "全体角度X軸", TAB.NOSE),
		new SliderData(43, "全体横幅", TAB.NOSE),
		new SliderData(44, "鼻筋高さ", TAB.NOSE),
		new SliderData(45, "鼻筋横幅", TAB.NOSE),
		new SliderData(46, "鼻筋形状", TAB.NOSE),
		new SliderData(47, "小鼻横幅", TAB.NOSE),
		new SliderData(48, "小鼻上下", TAB.NOSE),
		new SliderData(49, "小鼻前後", TAB.NOSE),
		new SliderData(50, "小鼻角度X軸", TAB.NOSE),
		new SliderData(51, "小鼻角度Z軸", TAB.NOSE),
		new SliderData(52, "鼻先高さ", TAB.NOSE),
		new SliderData(53, "鼻先角度X軸", TAB.NOSE),
		new SliderData(54, "鼻先サイズ", TAB.NOSE),
		new SliderData(55, "上下", TAB.MOUTH),
		new SliderData(56, "横幅", TAB.MOUTH),
		new SliderData(57, "縦幅", TAB.MOUTH),
		new SliderData(58, "前後", TAB.MOUTH),
		new SliderData(59, "形状上", TAB.MOUTH),
		new SliderData(60, "形状下", TAB.MOUTH),
		new SliderData(61, "形状口角", TAB.MOUTH),
		new SliderData(62, "サイズ", TAB.EAR),
		new SliderData(63, "角度Y軸", TAB.EAR),
		new SliderData(64, "角度Z軸", TAB.EAR),
		new SliderData(65, "上部形状", TAB.EAR),
		new SliderData(66, "下部形状", TAB.EAR)
	};

	private static readonly string CustomHighlightTexPath_F = "Custom Point F/Texture/cf_t_face_00_no";

	private static readonly string CustomHighlightTexPath_M = "Custom Point M/Texture/cm_t_face_00_no";

	private string CustomHighlightTexPath;

	[SerializeField]
	private EditMode editMode;

	private EditEquipShow equipShow;

	[SerializeField]
	private ToggleButton[] toggles = new ToggleButton[14];

	[SerializeField]
	private GameObject[] tabMains = new GameObject[14];

	[SerializeField]
	private ToggleButton colorCopyToggle;

	[SerializeField]
	private CharaColorCopyHelper colorCopyHelper;

	private InputSliderUI[] sliders;

	private ItemSelectUISets selSets_FaceType;

	private ItemSelectUISets selSets_SkinType;

	private ItemSelectUISets selSets_BumpType;

	private InputSliderUI bumpRate;

	private SwitchUI syncEyeSwitch;

	private ItemSelectUISets selSets_EyeL;

	private ColorChangeButton colorChange_ScleraL;

	private ColorChangeButton colorChange_IrisL;

	private InputSliderUI pupilL;

	private InputSliderUI eyeEmissiveL;

	private ItemSelectUISets selSets_EyeR;

	private ColorChangeButton colorChange_ScleraR;

	private ColorChangeButton colorChange_IrisR;

	private InputSliderUI pupilR;

	private InputSliderUI eyeEmissiveR;

	private ItemSelectUISets selSets_EyeHighlight;

	private ColorChangeButton colorChange_EyeHighlight;

	private InputSliderUI metallic_EyeHighlight;

	private InputSliderUI smooth_EyeHighlight;

	private ItemSelectUISets selSets_Eyebrow;

	private ColorChangeButton colorChange_Eyebrow;

	private InputSliderUI metallic_Eyebrow;

	private InputSliderUI smooth_Eyebrow;

	private ItemSelectUISets selSets_Eyelash;

	private ColorChangeButton colorChange_Eyelash;

	private InputSliderUI metallic_Eyelash;

	private InputSliderUI smooth_Eyelash;

	private ItemSelectUISets selSets_Mole;

	private ColorChangeButton colorChange_Mole;

	private ItemSelectUISets selSets_EyeShadow;

	private ColorChangeButton colorChange_EyeShadow;

	private ItemSelectUISets selSets_Cheek;

	private ColorChangeButton colorChange_Cheek;

	private ItemSelectUISets selSets_Lip;

	private ColorChangeButton colorChange_Lip;

	private ItemSelectUISets selSets_Tattoo;

	private ColorChangeButton colorChange_Tattoo;

	private ItemSelectUISets selSets_Beard;

	private ColorChangeButton colorChange_Beard;

	private Human human;

	private int nowTab = -1;

	private float customMatAlpha = 1f;

	private int onCursorNo = -1;

	private int highlightNo = -1;

	private bool showHighlight;

	private Texture highlightTex;

	private bool invoke = true;

	public void Setup(Human human, EditEquipShow equipShow)
	{
		this.human = human;
		this.equipShow = equipShow;
		CustomHighlightTexPath = ((human.sex != 0) ? CustomHighlightTexPath_M : CustomHighlightTexPath_F);
		human.head.CreateCustomHighlightMaterial();
		customMatAlpha = human.head.CustomHighlightMat_Skin.color.a;
		Color color = human.head.CustomHighlightMat_Skin.color;
		color.a = 0f;
		human.head.CustomHighlightMat_Skin.color = color;
		human.head.CustomHighlightMat_Eye_L.color = color;
		human.head.CustomHighlightMat_Eye_R.color = color;
		human.head.CustomHighlightMat_Eyebrow.color = color;
		sliders = new InputSliderUI[datas.Length];
		for (int i = 0; i < datas.Length; i++)
		{
			InputSliderUI inputSliderUI = editMode.CreateInputSliderUI(tabMains[(int)datas[i].type], datas[i].name, 0f, 100f, true, 50f, null);
			sliders[i] = inputSliderUI;
			EventTrigger eventTrigger = inputSliderUI.GetEventTrigger();
			int id = datas[i].id;
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
		editMode.CreateSpace(tabMains[0]);
		selSets_FaceType = editMode.CreateItemSelectUISets(tabMains[0], "輪郭", editMode.thumnbs_faceType, OnChangeFaceType);
		editMode.CreateSpace(tabMains[0]);
		selSets_SkinType = editMode.CreateItemSelectUISets(tabMains[0], "肌", editMode.thumnbs_faceSkin, OnChangeFaceSkin);
		selSets_BumpType = editMode.CreateItemSelectUISets(tabMains[0], "シワ", editMode.thumnbs_faceDetail, OnChangeFaceBumpType);
		bumpRate = editMode.CreateInputSliderUI(tabMains[0], "シワの深さ", 0f, 100f, false, 0f, OnChangeFaceBumpRate);
		syncEyeSwitch = editMode.CreateSwitchUI(tabMains[5], "両目を統一する", true, OnSyncEyeSwitch);
		editMode.CreateLabel(tabMains[5], "白目");
		colorChange_ScleraL = editMode.CreateColorChangeButton(tabMains[5], "左の色", Color.white, false, OnChangeEyeSclera_L);
		colorChange_ScleraR = editMode.CreateColorChangeButton(tabMains[5], "右の色", Color.white, false, OnChangeEyeSclera_R);
		editMode.CreateLabel(tabMains[5], "瞳");
		selSets_EyeL = editMode.CreateItemSelectUISets(tabMains[5], "左の瞳", editMode.thumnbs_eye, OnChangeEyeID_L);
		colorChange_IrisL = editMode.CreateColorChangeButton(tabMains[5], "左の色", Color.white, false, OnChangeEyeIris_L);
		pupilL = editMode.CreateInputSliderUI(tabMains[5], "左の瞳孔の開き", 0f, 100f, false, 0f, OnChangeEyePupil_L);
		eyeEmissiveL = editMode.CreateInputSliderUI(tabMains[5], "左の明るさ補正", 0f, 100f, true, 50f, OnChangeEyeEmissive_L);
		editMode.CreateSpace(tabMains[5]);
		selSets_EyeR = editMode.CreateItemSelectUISets(tabMains[5], "右の瞳", editMode.thumnbs_eye, OnChangeEyeID_R);
		colorChange_IrisR = editMode.CreateColorChangeButton(tabMains[5], "右の色", Color.white, false, OnChangeEyeIris_R);
		pupilR = editMode.CreateInputSliderUI(tabMains[5], "右の瞳孔の開き", 0f, 100f, false, 0f, OnChangeEyePupil_R);
		eyeEmissiveR = editMode.CreateInputSliderUI(tabMains[5], "右の明るさ補正", 0f, 100f, true, 50f, OnChangeEyeEmissive_R);
		editMode.CreateLabel(tabMains[5], "ハイライト");
		selSets_EyeHighlight = editMode.CreateItemSelectUISets(tabMains[5], "ハイライト", editMode.thumnbs_eyehighlight, OnChangeEyeHighlight);
		colorChange_EyeHighlight = editMode.CreateColorChangeButton(tabMains[5], "色", Color.white, true, OnChangeEyeHighlightColor);
		metallic_EyeHighlight = editMode.CreateInputSliderUI(tabMains[5], "ツヤの強さ", 0f, 100f, false, 0f, OnChangeMetallic_EyeHighlightL);
		smooth_EyeHighlight = editMode.CreateInputSliderUI(tabMains[5], "ツヤの質感", 0f, 100f, false, 0f, OnChangeSmooth_EyeHighlight);
		editMode.CreateLabel(tabMains[2], "タイプ");
		selSets_Eyebrow = editMode.CreateItemSelectUISets(tabMains[2], "まゆげ", editMode.thumnbs_eyebrow, OnChangeEyebrow);
		editMode.CreateLabel(tabMains[2], "色");
		colorChange_Eyebrow = editMode.CreateColorChangeButton(tabMains[2], "色", Color.white, true, OnChangeEyebrowColor);
		metallic_Eyebrow = editMode.CreateInputSliderUI(tabMains[2], "ツヤの強さ", 0f, 100f, false, 0f, OnChangeMetallic_Eyebrow);
		smooth_Eyebrow = editMode.CreateInputSliderUI(tabMains[2], "ツヤの質感", 0f, 100f, false, 0f, OnChangeSmooth_Eyebrow);
		selSets_Eyelash = editMode.CreateItemSelectUISets(tabMains[3], "まつげ", editMode.thumnbs_eyelash, OnChangeEyelash);
		colorChange_Eyelash = editMode.CreateColorChangeButton(tabMains[3], "色", Color.white, true, OnChangeEyelashColor);
		metallic_Eyelash = editMode.CreateInputSliderUI(tabMains[3], "ツヤの強さ", 0f, 100f, false, 0f, OnChangeMetallic_Eyelash);
		smooth_Eyelash = editMode.CreateInputSliderUI(tabMains[3], "ツヤの質感", 0f, 100f, false, 0f, OnChangeSmooth_Eyelash);
		selSets_Mole = editMode.CreateItemSelectUISets(tabMains[10], "ホクロ", editMode.thumnbs_mole, OnChangeMole);
		colorChange_Mole = editMode.CreateColorChangeButton(tabMains[10], "色", Color.white, true, OnChangeMoleColor);
		editMode.CreateLabel(tabMains[11], "アイシャドウ");
		selSets_EyeShadow = editMode.CreateItemSelectUISets(tabMains[11], "アイシャドウ", editMode.thumnbs_eyeshadow, OnChangeEyeShadow);
		colorChange_EyeShadow = editMode.CreateColorChangeButton(tabMains[11], "色", Color.white, true, OnChangeEyeShadowColor);
		editMode.CreateLabel(tabMains[11], "チーク");
		selSets_Cheek = editMode.CreateItemSelectUISets(tabMains[11], "チーク", editMode.thumnbs_cheek, OnChangeCheek);
		colorChange_Cheek = editMode.CreateColorChangeButton(tabMains[11], "色", Color.white, true, OnChangeCheekColor);
		editMode.CreateLabel(tabMains[11], "リップ");
		selSets_Lip = editMode.CreateItemSelectUISets(tabMains[11], "リップ", editMode.thumnbs_lip, OnChangeLip);
		colorChange_Lip = editMode.CreateColorChangeButton(tabMains[11], "色", Color.white, true, OnChangeLipColor);
		selSets_Tattoo = editMode.CreateItemSelectUISets(tabMains[12], "タトゥー", editMode.thumnbs_faceTattoo, OnChangeTattoo);
		colorChange_Tattoo = editMode.CreateColorChangeButton(tabMains[12], "色", Color.white, true, OnChangeTattooColor);
		selSets_Beard = editMode.CreateItemSelectUISets(tabMains[13], "髭", editMode.thumnbs_beard, OnChangeBeard);
		colorChange_Beard = editMode.CreateColorChangeButton(tabMains[13], "色", Color.white, true, OnChangeBeardColor);
		onCursorNo = -1;
		highlightNo = -1;
		showHighlight = false;
		bool flag = human.sex == SEX.FEMALE;
		selSets_EyeHighlight.toggle.gameObject.SetActive(flag);
		toggles[3].gameObject.SetActive(flag);
		toggles[10].gameObject.SetActive(flag);
		toggles[11].gameObject.SetActive(flag);
		toggles[13].gameObject.SetActive(!flag);
		LoadedHuman();
		colorCopyToggle.action.AddListener(OnToggleButton_ColorCopyHelper);
		colorCopyHelper.moveable.AddOnChange(OnChangeColorCopyHelperMoveableState);
	}

	private void OnEnable()
	{
		if (equipShow != null)
		{
			equipShow.SetAuto(EditEquipShow.WEARSHOW.ALL);
		}
		colorCopyToggle.ChangeValue(colorCopyHelper.gameObject.activeSelf, false);
	}

	private void Update()
	{
		toggles[8].Interactable = !human.head.MouthReset;
		toggles[9].Interactable = !human.head.MouthReset;
		toggles[7].Interactable = !human.Gag;
		if (human.head.MouthReset)
		{
			toggles[8].Value = false;
			toggles[9].Value = false;
		}
		if (human.Gag)
		{
			toggles[7].Value = false;
		}
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
		for (int j = 0; j < sliders.Length; j++)
		{
			if (datas[j].type == (TAB)nowTab)
			{
				float value = sliders[j].Value * 0.01f;
				human.head.SetShape(datas[j].id, value);
			}
		}
		bool reset = human.head.MouthReset || human.Gag;
		human.head.ResetMouth(reset);
		UpdateHighlight();
	}

	public void LoadedHuman()
	{
		invoke = false;
		HeadParameter head = human.customParam.head;
		for (int i = 0; i < sliders.Length; i++)
		{
			if (datas[i].type == (TAB)nowTab)
			{
				sliders[i].Value = human.head.GetShape(datas[i].id) * 100f;
			}
		}
		selSets_FaceType.SetSelectedFromDataID(head.headID);
		selSets_SkinType.SetSelectedFromDataID(head.faceTexID);
		selSets_BumpType.SetSelectedFromDataID(head.detailID);
		bumpRate.SetValue(head.detailWeight * 100f);
		bool flag = head.CheckEyeEqual();
		syncEyeSwitch.Value = flag;
		colorChange_ScleraL.SetColor(head.eyeScleraColorL);
		colorChange_ScleraR.SetColor(head.eyeScleraColorR);
		selSets_EyeL.SetSelectedFromDataID(head.eyeID_L);
		colorChange_IrisL.SetColor(head.eyeIrisColorL);
		pupilL.SetValue(head.eyePupilDilationL * 100f);
		eyeEmissiveL.SetValue(head.eyeEmissiveL * 100f);
		selSets_EyeR.SetSelectedFromDataID(head.eyeID_R);
		colorChange_IrisR.SetColor(head.eyeIrisColorR);
		pupilR.SetValue(head.eyePupilDilationR * 100f);
		eyeEmissiveR.SetValue(head.eyeEmissiveR * 100f);
		ChangeEyeUI(flag);
		selSets_EyeHighlight.SetSelectedFromDataID(head.eyeHighlightTexID);
		metallic_EyeHighlight.SetValue(head.eyeHighlightColor.specular1 * 100f);
		smooth_EyeHighlight.SetValue(head.eyeHighlightColor.smooth1 * 100f);
		colorChange_EyeHighlight.SetColor(head.eyeHighlightColor.mainColor1);
		bool flag2 = human.head.IsGlossEyeHighlight();
		metallic_EyeHighlight.gameObject.SetActive(flag2);
		smooth_EyeHighlight.gameObject.SetActive(flag2);
		colorChange_EyeHighlight.gameObject.SetActive(!flag2);
		selSets_Eyebrow.SetSelectedFromDataID(head.eyeBrowID);
		colorChange_Eyebrow.SetColor(head.eyeBrowColor.mainColor1);
		metallic_Eyebrow.SetValue(head.eyeBrowColor.specular1 * 100f);
		smooth_Eyebrow.SetValue(head.eyeBrowColor.smooth1 * 100f);
		selSets_Eyelash.SetSelectedFromDataID(head.eyeLashID);
		colorChange_Eyelash.SetColor(head.eyeLashColor.mainColor1);
		metallic_Eyelash.SetValue(head.eyeLashColor.specular1 * 100f);
		smooth_Eyelash.SetValue(head.eyeLashColor.smooth1 * 100f);
		selSets_Mole.SetSelectedFromDataID(head.moleTexID);
		colorChange_Mole.SetColor(head.moleColor);
		selSets_EyeShadow.SetSelectedFromDataID(head.eyeshadowTexID);
		colorChange_EyeShadow.SetColor(head.eyeshadowColor);
		selSets_Cheek.SetSelectedFromDataID(head.cheekTexID);
		colorChange_Cheek.SetColor(head.cheekColor);
		selSets_Lip.SetSelectedFromDataID(head.lipTexID);
		colorChange_Lip.SetColor(head.lipColor);
		selSets_Tattoo.SetSelectedFromDataID(head.tattooID);
		colorChange_Tattoo.SetColor(head.tattooColor);
		selSets_Beard.SetSelectedFromDataID(head.beardID);
		colorChange_Beard.SetColor(head.beardColor);
		invoke = true;
	}

	public void ChangedColor()
	{
		invoke = false;
		HeadParameter head = human.customParam.head;
		colorChange_Eyebrow.SetColor(head.eyeBrowColor.mainColor1);
		colorChange_Eyelash.SetColor(head.eyeLashColor.mainColor1);
		colorChange_Beard.SetColor(head.beardColor);
		invoke = true;
	}

	private void ChangeTab(int tab)
	{
		nowTab = tab;
		SystemSE.Play(SystemSE.SE.CHOICE);
		if (nowTab == -1)
		{
			return;
		}
		for (int i = 0; i < sliders.Length; i++)
		{
			if (datas[i].type == (TAB)nowTab)
			{
				sliders[i].Value = human.head.GetShape(datas[i].id) * 100f;
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
				if (datas[newNo].type == TAB.EYE)
				{
					ChangeCustomAlpha(human.head.CustomHighlightMat_Skin, 0f);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_L, customMatAlpha);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_R, customMatAlpha);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eyebrow, 0f);
				}
				else if (datas[newNo].type == TAB.EYEBROW)
				{
					human.head.CustomHighlightMat_Eyebrow.mainTexture = human.head.Rend_eyebrow.material.mainTexture;
					ChangeCustomAlpha(human.head.CustomHighlightMat_Skin, 0f);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_L, 0f);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_R, 0f);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eyebrow, customMatAlpha);
				}
				else
				{
					string path = CustomHighlightTexPath + highlightNo.ToString("00");
					highlightTex = Resources.Load<Texture>(path);
					human.head.CustomHighlightMat_Skin.mainTexture = highlightTex;
					float alpha = ((!highlightTex) ? 0f : customMatAlpha);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Skin, alpha);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_L, 0f);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_R, 0f);
					ChangeCustomAlpha(human.head.CustomHighlightMat_Eyebrow, 0f);
				}
				if (texture != null && texture != highlightTex)
				{
					Resources.UnloadAsset(texture);
				}
			}
			else
			{
				ChangeCustomAlpha(human.head.CustomHighlightMat_Skin, 0f);
				ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_L, 0f);
				ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_R, 0f);
				ChangeCustomAlpha(human.head.CustomHighlightMat_Eyebrow, 0f);
			}
		}
		else
		{
			ChangeCustomAlpha(human.head.CustomHighlightMat_Skin, 0f);
			ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_L, 0f);
			ChangeCustomAlpha(human.head.CustomHighlightMat_Eye_R, 0f);
			ChangeCustomAlpha(human.head.CustomHighlightMat_Eyebrow, 0f);
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

	private void OnChangeFaceType(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.headID = set.id;
			human.ChangeHead();
		}
	}

	private void OnChangeFaceSkin(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.faceTexID = set.id;
			human.head.ChangeSkinMaterial();
		}
	}

	private void OnChangeFaceBumpType(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.detailID = set.id;
			human.head.RendNormalTexture();
		}
	}

	private void OnChangeFaceBumpRate(float rate)
	{
		if (invoke)
		{
			human.customParam.head.detailWeight = rate * 0.01f;
			human.head.RendNormalTexture();
		}
	}

	private void OnSyncEyeSwitch(bool sync)
	{
		if (invoke)
		{
			ChangeEyeUI(sync);
		}
	}

	private void ChangeEyeUI(bool sync)
	{
		colorChange_ScleraR.gameObject.SetActive(!sync);
		selSets_EyeR.toggle.gameObject.SetActive(!sync);
		colorChange_IrisR.gameObject.SetActive(!sync);
		pupilR.gameObject.SetActive(!sync);
		eyeEmissiveR.gameObject.SetActive(!sync);
		colorChange_ScleraL.SetTitle((!sync) ? "左の色" : "左右の色");
		selSets_EyeL.toggle.SetTitle((!sync) ? "左の瞳" : "左右の瞳");
		colorChange_IrisL.SetTitle((!sync) ? "左の色" : "左右の色");
		pupilL.SetTitle((!sync) ? "左の瞳孔の開き" : "左右の瞳孔の開き");
		eyeEmissiveL.SetTitle((!sync) ? "左の明るさ補正" : "左右の明るさ補正");
	}

	private void OnChangeEyeID_L(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.eyeID_L = set.id;
			human.head.ChangeEye_L();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				selSets_EyeR.SetSelectedFromDataID(set.id);
				human.customParam.head.eyeID_R = set.id;
				human.head.ChangeEye_R();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeSclera_L(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeScleraColorL = color;
			human.head.ChangeEyeColor_L();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				colorChange_ScleraR.SetColor(color);
				human.customParam.head.eyeScleraColorR = color;
				human.head.ChangeEyeColor_R();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeIris_L(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeIrisColorL = color;
			human.head.ChangeEyeColor_L();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				colorChange_IrisR.SetColor(color);
				human.customParam.head.eyeIrisColorR = color;
				human.head.ChangeEyeColor_R();
				invoke = true;
			}
		}
	}

	private void OnChangeEyePupil_L(float val)
	{
		if (invoke)
		{
			val *= 0.01f;
			human.customParam.head.eyePupilDilationL = val;
			human.head.ChangeEyeColor_L();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				pupilR.Value = val;
				human.customParam.head.eyePupilDilationR = val;
				human.head.ChangeEyeColor_R();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeEmissive_L(float val)
	{
		if (invoke)
		{
			val *= 0.01f;
			human.customParam.head.eyeEmissiveL = val;
			human.head.ChangeEyeColor_L();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				eyeEmissiveR.Value = val;
				human.customParam.head.eyeEmissiveR = val;
				human.head.ChangeEyeColor_R();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeID_R(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.eyeID_R = set.id;
			human.head.ChangeEye_R();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				selSets_EyeL.SetSelectedFromDataID(set.id);
				human.customParam.head.eyeID_L = set.id;
				human.head.ChangeEye_L();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeSclera_R(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeScleraColorR = color;
			human.head.ChangeEyeColor_R();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				colorChange_ScleraL.SetColor(color);
				human.customParam.head.eyeScleraColorL = color;
				human.head.ChangeEyeColor_L();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeIris_R(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeIrisColorR = color;
			human.head.ChangeEyeColor_R();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				colorChange_IrisL.SetColor(color);
				human.customParam.head.eyeIrisColorL = color;
				human.head.ChangeEyeColor_L();
				invoke = true;
			}
		}
	}

	private void OnChangeEyePupil_R(float val)
	{
		if (invoke)
		{
			val *= 0.01f;
			human.customParam.head.eyePupilDilationR = val;
			human.head.ChangeEyeColor_R();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				pupilL.Value = val;
				human.customParam.head.eyePupilDilationL = val;
				human.head.ChangeEyeColor_L();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeEmissive_R(float val)
	{
		if (invoke)
		{
			val *= 0.01f;
			human.customParam.head.eyeEmissiveR = val;
			human.head.ChangeEyeColor_R();
			if (syncEyeSwitch.Value)
			{
				invoke = false;
				pupilL.Value = val;
				human.customParam.head.eyeEmissiveL = val;
				human.head.ChangeEyeColor_L();
				invoke = true;
			}
		}
	}

	private void OnChangeEyeHighlight(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.eyeHighlightTexID = set.id;
			human.head.ChangeEyeHighlight();
			UI_UpdateHighlight();
		}
	}

	private void UI_UpdateHighlight()
	{
		bool flag = human.head.IsGlossEyeHighlight();
		metallic_EyeHighlight.gameObject.SetActive(flag);
		smooth_EyeHighlight.gameObject.SetActive(flag);
		colorChange_EyeHighlight.gameObject.SetActive(!flag);
	}

	private void OnChangeEyeHighlightColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeHighlightColor.mainColor1 = color;
			human.head.ChangeEyeHighlightColor();
		}
	}

	private void OnChangeMetallic_EyeHighlightL(float val)
	{
		if (invoke)
		{
			human.customParam.head.eyeHighlightColor.specular1 = val * 0.01f;
			human.head.ChangeEyeHighlightColor();
		}
	}

	private void OnChangeSmooth_EyeHighlight(float val)
	{
		if (invoke)
		{
			human.customParam.head.eyeHighlightColor.smooth1 = val * 0.01f;
			human.head.ChangeEyeHighlightColor();
		}
	}

	private void OnChangeEyebrow(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.eyeBrowID = set.id;
			human.head.ChangeEyebrow();
		}
	}

	private void OnChangeEyebrowColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeBrowColor.mainColor1 = color;
			human.head.ChangeEyebrowColor();
		}
	}

	private void OnChangeMetallic_Eyebrow(float val)
	{
		if (invoke)
		{
			human.customParam.head.eyeBrowColor.specular1 = val * 0.01f;
			human.head.ChangeEyebrowColor();
		}
	}

	private void OnChangeSmooth_Eyebrow(float val)
	{
		if (invoke)
		{
			human.customParam.head.eyeBrowColor.smooth1 = val * 0.01f;
			human.head.ChangeEyebrowColor();
		}
	}

	private void OnChangeEyelash(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.eyeLashID = set.id;
			human.head.ChangeEyelash();
		}
	}

	private void OnChangeEyelashColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeLashColor.mainColor1 = color;
			human.head.ChangeEyelashColor();
		}
	}

	private void OnChangeMetallic_Eyelash(float val)
	{
		if (invoke)
		{
			human.customParam.head.eyeLashColor.specular1 = val * 0.01f;
			human.head.ChangeEyelashColor();
		}
	}

	private void OnChangeSmooth_Eyelash(float val)
	{
		if (invoke)
		{
			human.customParam.head.eyeLashColor.smooth1 = val * 0.01f;
			human.head.ChangeEyelashColor();
		}
	}

	private void OnChangeMole(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.moleTexID = set.id;
			human.head.ChangeMole();
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeMoleColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.moleColor = color;
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeEyeShadow(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.eyeshadowTexID = set.id;
			human.head.ChangeEyeShadow();
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeEyeShadowColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.eyeshadowColor = color;
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeCheek(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.cheekTexID = set.id;
			human.head.ChangeCheek();
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeCheekColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.cheekColor = color;
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeLip(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.lipTexID = set.id;
			human.head.ChangeLip();
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeLipColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.lipColor = color;
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeTattoo(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.tattooID = set.id;
			human.head.ChangeTattoo();
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeTattooColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.tattooColor = color;
			human.head.RendSkinTexture();
		}
	}

	private void OnChangeBeard(CustomSelectSet set)
	{
		if (invoke)
		{
			human.customParam.head.beardID = set.id;
			human.head.ChangeBeard();
		}
	}

	private void OnChangeBeardColor(Color color)
	{
		if (invoke)
		{
			human.customParam.head.beardColor = color;
			human.head.ChangeBeardColor();
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

	private void OnChangeColorCopyHelperMoveableState(MoveableUI.STATE state)
	{
		if (state == MoveableUI.STATE.CLOSED)
		{
			colorCopyToggle.ChangeValue(false, false);
		}
	}
}
