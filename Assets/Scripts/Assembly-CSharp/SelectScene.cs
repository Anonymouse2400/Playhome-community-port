using System;
using Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

public class SelectScene : Scene
{
	[Serializable]
	public class StateSet
	{
		public Text stateName;

		public Button stateButton;

		public Toggle[] badgeToggle = new Toggle[5];

		public Image[] badgeGage = new Image[5];

		public ToggleButton vaginaVirginToggle;

		public ToggleButton analVirginToggle;
	}

	private enum TAB
	{
		MALE = 0,
		FEMALE = 1,
		VISITOR = 2,
		CUSTOM = 3,
		MAP = 4,
		STATE = 5,
		GAG = 6
	}

	[SerializeField]
	private Canvas uiCanvas;

	[SerializeField]
	private Config configOriginal;

	private Config config;

	[SerializeField]
	private SaveLoadMode save_loadOriginal;

	private SaveLoadMode save_load;

	[SerializeField]
	private PauseMenue pauseMenueOriginal;

	private PauseMenue pauseMenue;

	[SerializeField]
	private ToggleButton originalToggle;

	[SerializeField]
	private Button originalButton;

	[SerializeField]
	private Text originalText;

	[SerializeField]
	private GameObject originalSpace;

	[SerializeField]
	private GameObject[] mains;

	[SerializeField]
	private ToggleButton maleToggleButton;

	private Button customButton_Female;

	private Button customButton_Male;

	private Button customButton_Visitor;

	private Male male;

	private Female female;

	private Female visitor;

	private Map map;

	[SerializeField]
	private Transform charaPosRoot;

	private Transform charaPos_MainFemale;

	private Transform charaPos_MainMale;

	private Transform charaPos_Visitor;

	private Transform charaPos_SubFemale;

	private Transform charaPos_SubMale;

	[SerializeField]
	private IllusionCamera cam;

	[SerializeField]
	private Button buttonMemory;

	[SerializeField]
	private GameObject memoryRoot;

	private ToggleButton[] maleToggles;

	private ToggleButton[] femaleToggles;

	private ToggleButton[] visitorToggles;

	private Transform mainStateTrans;

	private Transform subStateTrans;

	[SerializeField]
	private StateSet[] stateSets = new StateSet[2];

	private Text[] gagName = new Text[2];

	private RadioButtonGroup[] gagRadioGroup = new RadioButtonGroup[2];

	private ToggleButton[,] gagToggles = new ToggleButton[2, 3];

	private string bgm = "Select";

	private int prevMapNo = -1;

	private bool invoke = true;

	[SerializeField]
	private TextAsset eyeList_F;

	[SerializeField]
	private TextAsset mouthList_F;

	[SerializeField]
	private TextAsset eyeList_M;

	[SerializeField]
	private TextAsset mouthList_M;

	[SerializeField]
	private TextAsset selectExpressionList;

	private SelectExpressionManager expressionMgr = new SelectExpressionManager();

	private HEROINE sceneInHeroine;

	private void Awake()
	{
		InScene(false);
		expressionMgr.Load(eyeList_F, mouthList_F, eyeList_M, mouthList_M, selectExpressionList);
	}

	private void Start()
	{
		GlobalData.isMemory = false;
		for (int i = 0; i < GlobalData.PlayData.personality.Length; i++)
		{
			Personality personality = GlobalData.PlayData.personality[i];
			if (personality.state == Personality.STATE.FIRST)
			{
				personality.state = Personality.STATE.RESIST;
			}
			if (personality.state >= Personality.STATE.FLIP_FLOP)
			{
				personality.state = Personality.STATE.FLOP;
			}
		}
		if (GlobalData.PlayData.lastSelectVisitor == VISITOR.KOUICHI)
		{
			GlobalData.PlayData.lastSelectVisitor = VISITOR.NONE;
		}
		buttonMemory.gameObject.SetActive(GlobalData.PlayData.Progress == GamePlayData.PROGRESS.ALL_FREE);
		config = UnityEngine.Object.Instantiate(configOriginal);
		config.gameObject.SetActive(false);
		save_load = UnityEngine.Object.Instantiate(save_loadOriginal);
		save_load.gameObject.SetActive(false);
		base.GC.audioCtrl.BGM_LoadAndPlay(bgm, false, true);
		pauseMenue = UnityEngine.Object.Instantiate(pauseMenueOriginal);
		pauseMenue.Setup(true, true, Button_Config, Button_Exit, null);
		ScreenFade.StartFade(ScreenFade.TYPE.IN, base.GC.FadeColor, base.GC.FadeTime);
		UISetup();
		charaPos_MainFemale = Transform_Utility.FindTransform(charaPosRoot, "MainFemale");
		charaPos_MainMale = Transform_Utility.FindTransform(charaPosRoot, "MainMale");
		charaPos_Visitor = Transform_Utility.FindTransform(charaPosRoot, "Visitor");
		charaPos_SubFemale = Transform_Utility.FindTransform(charaPosRoot, "SubFemale");
		charaPos_SubMale = Transform_Utility.FindTransform(charaPosRoot, "SubMale");
		male = CreateMale((int)GlobalData.PlayData.lastSelectMale, charaPos_MainMale);
		female = CreateFemale((int)GlobalData.PlayData.lastSelectFemale, charaPos_MainFemale);
		visitor = CreateFemale((int)GlobalData.PlayData.lastSelectVisitor, charaPos_Visitor);
		for (int j = 0; j < visitorToggles.Length; j++)
		{
			visitorToggles[j].Interactable = j != (int)GlobalData.PlayData.lastSelectFemale;
		}
		for (int k = 0; k < femaleToggles.Length; k++)
		{
			femaleToggles[k].Interactable = k != (int)GlobalData.PlayData.lastSelectVisitor;
		}
		prevMapNo = GlobalData.PlayData.lastSelectMap;
		sceneInHeroine = GlobalData.PlayData.lastSelectFemale;
		LoadMap();
		CharaPos();
		LightPos();
		ResetCamera();
		UpdateState();
		if (sceneInMsg == "MemoryEnd")
		{
			memoryRoot.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		bool flag = !config.isActiveAndEnabled && !save_load.isActiveAndEnabled;
		uiCanvas.enabled = flag && !base.GC.IsHideUI;
		pauseMenue.EnableConfig = flag;
		UpdateInput();
	}

	public override void OutScene(string next)
	{
		base.OutScene(next);
	}

	private void UpdateInput()
	{
		if ((!(EventSystem.current != null) || !(EventSystem.current.currentSelectedGameObject != null)) && Input.GetKeyDown(KeyCode.R))
		{
			CameraPos();
		}
	}

	private Female CreateFemale(int no, Transform parent)
	{
		CustomParameter heroineCustomParam = GlobalData.PlayData.GetHeroineCustomParam((HEROINE)no);
		if (heroineCustomParam == null)
		{
			return null;
		}
		Female female = ResourceUtility.CreateInstance<Female>("FemaleBody");
		female.SetHeroineID((HEROINE)no);
		female.Load(heroineCustomParam);
		female.Apply();
		LoadAnimation(female, "AC_Edit_F");
		Resources.UnloadUnusedAssets();
		female.ShowMosaic(false);
		IllusionCamera illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		female.ChangeEyeLook(LookAtRotator.TYPE.TARGET, illusionCamera.transform, false);
		if (parent != null)
		{
			female.transform.SetParent(parent, false);
		}
		return female;
	}

	private Male CreateMale(int no, Transform parent)
	{
		CustomParameter maleCustomParam = GlobalData.PlayData.GetMaleCustomParam((MALE_ID)no);
		if (maleCustomParam == null)
		{
			return null;
		}
		Male male = ResourceUtility.CreateInstance<Male>("MaleBody");
		male.MaleID = (MALE_ID)no;
		male.Load(maleCustomParam);
		male.Apply();
		LoadAnimation(male, "AC_Edit_M");
		if (no == 1)
		{
			male.body.Anime.Play("tachi_pose_01");
			ChangeExpression(male);
		}
		else
		{
			male.body.Anime.Play("tachi_pose_02");
			ChangeExpression(male);
		}
		Resources.UnloadUnusedAssets();
		male.ChangeMaleShow(MALE_SHOW.CLOTHING);
		male.SetShowTinWithWear(false);
		IllusionCamera illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		male.ChangeEyeLook(LookAtRotator.TYPE.TARGET, illusionCamera.transform, false);
		if (parent != null)
		{
			male.transform.SetParent(parent, false);
		}
		return male;
	}

	private void LoadAnimation(Human human, string asset)
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(GlobalData.assetBundlePath, "pose");
		human.body.Anime.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>(asset);
		assetBundleController.Close();
	}

	private void ChangeExpression(Human human)
	{
		expressionMgr.Change(human);
	}

	private void UISetup()
	{
		bool flag = GlobalData.PlayData.Progress == GamePlayData.PROGRESS.ALL_FREE;
		GameObject gameObject = mains[0];
		for (MALE_ID mALE_ID = MALE_ID.HERO; mALE_ID <= MALE_ID.KOUICHI; mALE_ID++)
		{
			string title = Male.MaleName(mALE_ID);
			CreateToggleButton(title, gameObject.transform);
		}
		RadioButtonGroup radioButtonGroup = SetupRadioButtonGroup(gameObject, false);
		maleToggles = radioButtonGroup.ToggleButtons;
		radioButtonGroup.Value = (int)GlobalData.PlayData.lastSelectMale;
		radioButtonGroup.action.AddListener(ChangeMale);
		maleToggleButton.gameObject.SetActive(flag);
		GameObject gameObject2 = mains[1];
		for (HEROINE hEROINE = HEROINE.RITSUKO; hEROINE < HEROINE.NUM; hEROINE++)
		{
			string title2 = Female.HeroineName(hEROINE);
			CreateToggleButton(title2, gameObject2.transform);
		}
		RadioButtonGroup radioButtonGroup2 = SetupRadioButtonGroup(gameObject2, false);
		femaleToggles = radioButtonGroup2.ToggleButtons;
		radioButtonGroup2.Value = (int)GlobalData.PlayData.lastSelectFemale;
		radioButtonGroup2.action.AddListener(ChangeFemale);
		GameObject gameObject3 = mains[2];
		for (HEROINE hEROINE2 = HEROINE.RITSUKO; hEROINE2 < HEROINE.NUM; hEROINE2++)
		{
			string title3 = Female.HeroineName(hEROINE2);
			CreateToggleButton(title3, gameObject3.transform);
		}
		CreateToggleButton("なし", gameObject3.transform);
		RadioButtonGroup radioButtonGroup3 = SetupRadioButtonGroup(gameObject3, false);
		visitorToggles = radioButtonGroup3.ToggleButtons;
		radioButtonGroup3.Value = (int)GlobalData.PlayData.lastSelectVisitor;
		radioButtonGroup3.action.AddListener(ChangeVisitor);
		GameObject gameObject4 = mains[3];
		customButton_Female = CreateButton("女", gameObject4.transform);
		customButton_Male = CreateButton("男", gameObject4.transform);
		customButton_Visitor = CreateButton("参観者", gameObject4.transform);
		customButton_Female.onClick.AddListener(Button_CustomFemale);
		customButton_Male.onClick.AddListener(Button_CustomMale);
		customButton_Visitor.onClick.AddListener(Button_CustomVisitor);
		ChangeCustomButtonName();
		GameObject gameObject5 = mains[4];
		GameObject gameObject6 = Transform_Utility.FindTransform(gameObject5.transform, "Place").gameObject;
		CreateToggleButton("夫婦の寝室", gameObject6.transform);
		CreateToggleButton("律子の部屋", gameObject6.transform);
		CreateToggleButton("明子の部屋", gameObject6.transform);
		CreateToggleButton("リビング", gameObject6.transform);
		CreateToggleButton("風呂", gameObject6.transform);
		CreateToggleButton("和室", gameObject6.transform);
		CreateToggleButton("洗面所", gameObject6.transform);
		CreateToggleButton("玄関(内)", gameObject6.transform);
		CreateToggleButton("トイレ", gameObject6.transform);
		CreateToggleButton("玄関(外)", gameObject6.transform);
		RadioButtonGroup radioButtonGroup4 = SetupRadioButtonGroup(gameObject6, false);
		radioButtonGroup4.Value = GlobalData.PlayData.lastSelectMap;
		radioButtonGroup4.action.AddListener(ChangeMap);
		GameObject gameObject7 = Transform_Utility.FindTransform(gameObject5.transform, "TimeZone").gameObject;
		CreateToggleButton("昼", gameObject7.transform);
		CreateToggleButton("夕", gameObject7.transform);
		CreateToggleButton("夜(点灯)", gameObject7.transform);
		CreateToggleButton("夜(消灯)", gameObject7.transform);
		RadioButtonGroup radioButtonGroup5 = SetupRadioButtonGroup(gameObject7, false);
		radioButtonGroup5.Value = GlobalData.PlayData.lastSelectTimeZone;
		radioButtonGroup5.action.AddListener(ChangeTimeZone);
		GameObject gameObject8 = mains[5];
		mainStateTrans = Transform_Utility.FindTransform(gameObject8.transform, "Main");
		subStateTrans = Transform_Utility.FindTransform(gameObject8.transform, "Sub");
		Transform[] array = new Transform[2] { mainStateTrans, subStateTrans };
		for (int i = 0; i < array.Length; i++)
		{
			int no2 = i;
			stateSets[i].stateButton.onClick.AddListener(delegate
			{
				Button_State(no2);
			});
			stateSets[i].badgeToggle[0].onValueChanged.AddListener(delegate(bool value)
			{
				Toggle_Badge(no2, 0, value);
			});
			stateSets[i].badgeToggle[1].onValueChanged.AddListener(delegate(bool value)
			{
				Toggle_Badge(no2, 1, value);
			});
			stateSets[i].badgeToggle[2].onValueChanged.AddListener(delegate(bool value)
			{
				Toggle_Badge(no2, 2, value);
			});
			stateSets[i].badgeToggle[3].onValueChanged.AddListener(delegate(bool value)
			{
				Toggle_Badge(no2, 3, value);
			});
			stateSets[i].badgeToggle[4].onValueChanged.AddListener(delegate(bool value)
			{
				Toggle_Badge(no2, 4, value);
			});
			stateSets[i].vaginaVirginToggle.ActionAddListener(delegate(bool value)
			{
				Togge_VaginaVergin(no2, value);
			});
			stateSets[i].analVirginToggle.ActionAddListener(delegate(bool value)
			{
				Togge_AnalVergin(no2, value);
			});
			stateSets[i].stateButton.interactable = flag;
			stateSets[i].badgeToggle[0].interactable = flag;
			stateSets[i].badgeToggle[1].interactable = flag;
			stateSets[i].badgeToggle[2].interactable = flag;
			stateSets[i].badgeToggle[3].interactable = flag;
			stateSets[i].badgeToggle[4].interactable = flag;
			stateSets[i].vaginaVirginToggle.Interactable = flag;
			stateSets[i].analVirginToggle.Interactable = flag;
		}
		GameObject gameObject9 = mains[6];
		Transform transform = Transform_Utility.FindTransform(gameObject9.transform, "Main");
		Transform transform2 = Transform_Utility.FindTransform(gameObject9.transform, "Sub");
		Transform[] array2 = new Transform[2] { transform, transform2 };
		for (int j = 0; j < array2.Length; j++)
		{
			gagName[j] = CreateText("名前", array2[j]);
			gagToggles[j, 0] = CreateToggleButton("なし", array2[j]);
			gagToggles[j, 1] = CreateToggleButton("ﾎﾞｰﾙｷﾞｬｸﾞ", array2[j]);
			gagToggles[j, 2] = CreateToggleButton("ｶﾞﾑﾃｰﾌﾟ", array2[j]);
			int no = j;
			gagRadioGroup[j] = array2[j].GetComponent<RadioButtonGroup>();
			ToggleButton[] toggleButtons = new ToggleButton[3]
			{
				gagToggles[j, 0],
				gagToggles[j, 1],
				gagToggles[j, 2]
			};
			gagRadioGroup[j].SetToggleButtons(toggleButtons);
			gagRadioGroup[j].action.AddListener(delegate(int value)
			{
				OnChangeGagItem(no, value);
			});
		}
	}

	private void ChangeCustomButtonName()
	{
		bool flag = GlobalData.PlayData.lastSelectVisitor >= VISITOR.RITSUKO && GlobalData.PlayData.lastSelectVisitor < (VISITOR)3;
		customButton_Female.GetComponentInChildren<Text>().text = Female.HeroineName(GlobalData.PlayData.lastSelectFemale);
		customButton_Male.GetComponentInChildren<Text>().text = Male.MaleName(GlobalData.PlayData.lastSelectMale);
		customButton_Visitor.GetComponentInChildren<Text>().text = ((!flag) ? "なし" : Female.HeroineName((HEROINE)GlobalData.PlayData.lastSelectVisitor));
		customButton_Visitor.gameObject.SetActive(flag);
	}

	private ToggleButton CreateToggleButton(string title, Transform parent)
	{
		ToggleButton toggleButton = UnityEngine.Object.Instantiate(originalToggle);
		toggleButton.SetText(title, title);
		toggleButton.transform.SetParent(parent, false);
		return toggleButton;
	}

	private RadioButtonGroup SetupRadioButtonGroup(GameObject go, bool allowOff)
	{
		RadioButtonGroup radioButtonGroup = go.GetComponent<RadioButtonGroup>();
		if (radioButtonGroup == null)
		{
			radioButtonGroup = go.AddComponent<RadioButtonGroup>();
		}
		radioButtonGroup.AllowOFF = allowOff;
		ToggleButton[] componentsInChildren = go.GetComponentsInChildren<ToggleButton>();
		radioButtonGroup.SetToggleButtons(componentsInChildren);
		return radioButtonGroup;
	}

	private Button CreateButton(string title, Transform parent)
	{
		Button button = UnityEngine.Object.Instantiate(originalButton);
		button.GetComponentInChildren<Text>().text = title;
		button.transform.SetParent(parent, false);
		return button;
	}

	private Text CreateText(string text, Transform parent)
	{
		Text text2 = UnityEngine.Object.Instantiate<Text>(originalText);
		((Component)(object)text2).GetComponentInChildren<Text>().text = text;
		((Component)(object)text2).transform.SetParent(parent, false);
		return text2;
	}

	private GameObject CreateSpace(Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(originalSpace);
		gameObject.transform.SetParent(parent, false);
		return gameObject;
	}

	private void ChangeFemale(int no)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		GlobalData.PlayData.lastSelectFemale = (HEROINE)no;
		UnityEngine.Object.Destroy(female.gameObject);
		female = CreateFemale(no, charaPos_MainFemale);
		female.Foot(map.data.foot);
		ChangeCustomButtonName();
		for (int i = 0; i < visitorToggles.Length; i++)
		{
			visitorToggles[i].Interactable = i != no;
		}
		UpdateState();
		Voice(female);
	}

	private void ChangeMale(int no)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		GlobalData.PlayData.lastSelectMale = (MALE_ID)no;
		UnityEngine.Object.Destroy(male.gameObject);
		male = CreateMale(no, charaPos_MainMale);
		male.Foot(map.data.foot);
		ChangeCustomButtonName();
	}

	private void ChangeVisitor(int no)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		GlobalData.PlayData.lastSelectVisitor = (VISITOR)no;
		if (visitor != null)
		{
			UnityEngine.Object.Destroy(visitor.gameObject);
		}
		visitor = CreateFemale(no, charaPos_Visitor);
		if (visitor != null)
		{
			visitor.Foot(map.data.foot);
		}
		ChangeCustomButtonName();
		for (int i = 0; i < femaleToggles.Length; i++)
		{
			femaleToggles[i].Interactable = i != no;
		}
		UpdateState();
		if (visitor != null)
		{
			Voice(visitor);
		}
	}

	private void ChangeMap(int no)
	{
		if (GlobalData.PlayData.lastSelectMap != no)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			prevMapNo = GlobalData.PlayData.lastSelectMap;
			GlobalData.PlayData.lastSelectMap = no;
			LoadMap();
			CharaPos();
			LightPos();
			CameraPos();
		}
	}

	private void ChangeTimeZone(int no)
	{
		if (GlobalData.PlayData.lastSelectTimeZone != no)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			GlobalData.PlayData.lastSelectTimeZone = no;
			LoadMap();
			LightPos();
		}
	}

	private void LoadMap()
	{
		string text = "map/";
		string[] array = new string[10] { "bedroom", "ritsuko_room", "akiko_room", "living", "bathroom", "japanese", "poweder", "entrance", "toilet", "yard" };
		string[] array2 = new string[4] { "day", "evening", "night_light", "night_dark" };
		text = text + array[GlobalData.PlayData.lastSelectMap] + "_" + array2[GlobalData.PlayData.lastSelectTimeZone];
		LoadMap(text);
		Foot();
	}

	private void Foot()
	{
		female.Foot(map.data.foot);
		male.Foot(map.data.foot);
		if (visitor != null)
		{
			visitor.Foot(map.data.foot);
		}
	}

	private void LoadMap(string map_path)
	{
		if (map != null)
		{
			UnityEngine.Object.Destroy(map.gameObject);
			map = null;
		}
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", map_path);
		string assetName = map_path.Substring(map_path.LastIndexOf("/") + 1);
		GameObject gameObject = assetBundleController.LoadAndInstantiate<GameObject>(assetName);
		map = gameObject.GetComponent<Map>();
		LightMapControl componentInChildren = map.GetComponentInChildren<LightMapControl>();
		componentInChildren.Apply();
		assetBundleController.Close();
		Resources.UnloadUnusedAssets();
	}

	private void CharaPos()
	{
		charaPosRoot.position = map.data.selectPos;
		charaPosRoot.rotation = Quaternion.Euler(0f, map.data.selectYaw, 0f);
	}

	private void ResetCamera()
	{
		Vector3 position = charaPosRoot.position;
		position.y += 1.3f;
		Vector3 zero = Vector3.zero;
		zero.y = map.data.selectYaw + 180f;
		float num = 25f / ConfigData.defParse;
		cam.Set(position, zero, 2.5f * num, ConfigData.defParse);
	}

	private void CameraPos()
	{
		Vector3 position = charaPosRoot.position;
		position.y += 1.3f;
		Vector3 zero = Vector3.zero;
		zero.y = map.data.selectYaw + 180f;
		float num = 25f / cam.FOV;
		cam.Set(position, zero, 2.5f * num);
	}

	private void LightPos()
	{
		float selectYaw = map.data.selectYaw;
		map.lightRoot.rotation = Quaternion.Euler(20f, selectYaw + 180f, 0f);
	}

	public void Button_Config()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_open);
		config.gameObject.SetActive(true);
	}

	public void Button_Exit()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_choice);
		base.GC.CreateModalYesNoUI("タイトルに戻ります。\nセーブしていない場合は\nゲームの進行状況、キャラカスタム等は失われます。\nよろしいですか？", ToTitle);
	}

	private void ExitGame()
	{
		base.GC.ChangeScene("ExitScene", string.Empty, 1f);
	}

	private void ToTitle()
	{
		base.GC.ChangeScene("TitleScene", string.Empty, 1f);
	}

	public void ChangeTab(int no)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		for (int i = 0; i < mains.Length; i++)
		{
			mains[i].SetActive(i == no);
		}
	}

	public void Button_H()
	{
		SystemSE.Play(SystemSE.SE.YES);
		base.GC.ChangeScene("H", string.Empty);
	}

	public void UpdateState()
	{
		invoke = false;
		Female[] array = new Female[2] { female, visitor };
		Transform[] array2 = new Transform[2] { mainStateTrans, subStateTrans };
		string[] array3 = new string[7] { "初回", "抵抗", "豹変", "豹変イベント", "姉妹最終イベント", "雪子最終イベント１", "雪子最終イベント２" };
		string[,] array4 = new string[3, 2]
		{
			{ "hp00_f_00_00", "hp00_f_01_00" },
			{ "hp01_f_00_00", "hp01_f_01_00" },
			{ "hp02_f_00_00", "hp02_f_01_00" }
		};
		bool flag = GlobalData.PlayData.Progress == GamePlayData.PROGRESS.ALL_FREE;
		for (int i = 0; i < array.Length; i++)
		{
			array2[i].gameObject.SetActive(array[i] != null);
			gagRadioGroup[i].gameObject.SetActive(array[i] != null);
			if (!(array[i] == null))
			{
				string text = Female.HeroineName(array[i].HeroineID);
				stateSets[i].stateName.text = text;
				stateSets[i].stateButton.GetComponentInChildren<Text>().text = array3[(int)array[i].personality.state];
				stateSets[i].badgeToggle[0].isOn = array[i].personality.feelVagina;
				stateSets[i].badgeToggle[1].isOn = array[i].personality.feelAnus;
				stateSets[i].badgeToggle[2].isOn = array[i].personality.indecentLanguage;
				stateSets[i].badgeToggle[3].isOn = array[i].personality.likeFeratio;
				stateSets[i].badgeToggle[4].isOn = array[i].personality.likeSperm;
				stateSets[i].badgeGage[0].fillAmount = array[i].personality.ExpFeelVaginaRate;
				stateSets[i].badgeGage[1].fillAmount = array[i].personality.ExpFeelAnusRate;
				stateSets[i].badgeGage[2].fillAmount = array[i].personality.ExpIndecentLanguageRate;
				stateSets[i].badgeGage[3].fillAmount = array[i].personality.ExpLikeFeratioRate;
				stateSets[i].badgeGage[4].fillAmount = array[i].personality.ExpLikeSpermRate;
				stateSets[i].vaginaVirginToggle.ChangeValue(array[i].personality.vaginaVirgin, false);
				stateSets[i].analVirginToggle.ChangeValue(array[i].personality.analVirgin, false);
				bool flag2 = true;
				if (array[i].HeroineID == HEROINE.YUKIKO)
				{
					flag2 = false;
				}
				stateSets[i].vaginaVirginToggle.Interactable = flag && flag2;
				gagName[i].text = text;
				gagRadioGroup[i].Change((int)array[i].personality.gagItem, false);
				int heroineID = (int)array[i].HeroineID;
				int num = (array[i].IsFloped() ? 1 : 0);
				string stateName = array4[heroineID, num];
				array[i].body.Anime.Play(stateName, 0);
				ChangeExpression(array[i]);
			}
		}
		invoke = true;
	}

	private void Button_State(int no)
	{
		string[] array = new string[7] { "初回", "抵抗", "豹変", "豹変イベント", "姉妹最終イベント", "雪子最終イベント１", "雪子最終イベント２" };
		Female[] array2 = new Female[2] { this.female, visitor };
		Female female = array2[no];
		if (female.personality.state == Personality.STATE.RESIST)
		{
			female.personality.state = Personality.STATE.FLOP;
		}
		else
		{
			female.personality.state = Personality.STATE.RESIST;
		}
		stateSets[no].stateButton.GetComponentInChildren<Text>().text = array[(int)female.personality.state];
		SystemSE.Play(SystemSE.SE.CHOICE);
		string[,] array3 = new string[3, 2]
		{
			{ "hp00_f_00_00", "hp00_f_01_00" },
			{ "hp01_f_00_00", "hp01_f_01_00" },
			{ "hp02_f_00_00", "hp02_f_01_00" }
		};
		int heroineID = (int)female.HeroineID;
		int num = (female.IsFloped() ? 1 : 0);
		string stateName = array3[heroineID, num];
		female.body.Anime.CrossFadeInFixedTime(stateName, 0.3f, 0);
		ChangeExpression(female);
	}

	private void Toggle_Badge(int no, int badge, bool value)
	{
		if (invoke)
		{
			Female[] array = new Female[2] { this.female, visitor };
			Female female = array[no];
			if (badge == 0)
			{
				female.personality.feelVagina = value;
			}
			if (badge == 1)
			{
				female.personality.feelAnus = value;
			}
			if (badge == 2)
			{
				female.personality.indecentLanguage = value;
			}
			if (badge == 3)
			{
				female.personality.likeFeratio = value;
			}
			if (badge == 4)
			{
				female.personality.likeSperm = value;
			}
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void Togge_VaginaVergin(int no, bool value)
	{
		if (invoke)
		{
			Female[] array = new Female[2] { this.female, visitor };
			Female female = array[no];
			female.personality.vaginaVirgin = value;
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void Togge_AnalVergin(int no, bool value)
	{
		if (invoke)
		{
			Female[] array = new Female[2] { this.female, visitor };
			Female female = array[no];
			female.personality.analVirgin = value;
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void Button_CustomFemale()
	{
		SystemSE.Play(SystemSE.SE.YES);
		string charaName = Female.HeroineName(GlobalData.PlayData.lastSelectFemale);
		string msg = EditScene.CreateMessage(charaName, "SelectScene");
		gameCtrl.ChangeScene("EditScene", msg);
	}

	private void Button_CustomMale()
	{
		SystemSE.Play(SystemSE.SE.YES);
		string charaName = Male.MaleName(GlobalData.PlayData.lastSelectMale);
		string msg = EditScene.CreateMessage(charaName, "SelectScene");
		gameCtrl.ChangeScene("EditScene", msg);
	}

	private void Button_CustomVisitor()
	{
		SystemSE.Play(SystemSE.SE.YES);
		string charaName = Female.HeroineName((HEROINE)GlobalData.PlayData.lastSelectVisitor);
		string msg = EditScene.CreateMessage(charaName, "SelectScene");
		gameCtrl.ChangeScene("EditScene", msg);
	}

	public void Button_Save()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_open);
		save_load.Setup(SaveLoadMode.MODE.SAVE);
		save_load.gameObject.SetActive(true);
	}

	public void Button_Load()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_open);
		save_load.Setup(SaveLoadMode.MODE.LOAD);
		save_load.gameObject.SetActive(true);
	}

	public void Button_Memory()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_open);
		memoryRoot.gameObject.SetActive(true);
	}

	public void Button_MemoryClose()
	{
		base.GC.audioCtrl.Play2DSE(base.GC.audioCtrl.systemSE_close);
		memoryRoot.gameObject.SetActive(false);
	}

	public void OnChangeGagItem(int main_sub, int value)
	{
		Female[] array = new Female[2] { this.female, visitor };
		Female female = array[main_sub];
		female.personality.gagItem = (GAG_ITEM)value;
		female.ChangeGagItem();
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	public void LoadMemory(int no)
	{
		SystemSE.Play(SystemSE.SE.YES);
		string[] array = new string[11]
		{
			"00_00", "00_01", "00_03", "00_04", "01_00", "01_01", "01_03", "02_00", "02_01", "02_02",
			"02_03"
		};
		if (no >= 0 && no < array.Length)
		{
			string msg = "adv/adv_" + array[no] + ",ADV_Script_" + array[no];
			GlobalData.isMemory = true;
			base.GC.ChangeScene("ADVScene", msg, 1f);
		}
	}

	private void Voice(Female female)
	{
		if (!(female == null))
		{
			AudioClip audioClip = null;
			string text = "SystemVoice/CharaSelect/CharaSelect_";
			string[] array = new string[2] { "A", "B" };
			int num = (female.IsFloped() ? 1 : 0);
			text = text + "F" + ((int)female.HeroineID).ToString("00") + "_";
			if (female.Gag)
			{
				text = text + "Gag" + array[num];
			}
			else if (sceneInHeroine == female.HeroineID)
			{
				text = text + "Continuous" + array[num];
			}
			else
			{
				int num2 = UnityEngine.Random.Range(0, 2);
				text = text + array[num] + num2.ToString("00");
			}
			audioClip = Resources.Load<AudioClip>(text);
			if (audioClip == null)
			{
				Debug.LogError("オーディオクリップがない");
			}
			else
			{
				female.PhonationVoice(audioClip, false);
			}
		}
	}
}
