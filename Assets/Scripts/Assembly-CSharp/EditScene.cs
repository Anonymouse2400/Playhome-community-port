using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

public class EditScene : Scene
{
	public string path_FemaleBody = "FemaleBody";

	public string path_MaleBody = "MaleBody";

	[SerializeField]
	private GameObject[] createButtons;

	[SerializeField]
	private EditMode editMode;

	[SerializeField]
	private Canvas sceneCanvas;

	public EditEquipShow equipShow;

	private Human human;

	[SerializeField]
	private TextAsset poseList_F;

	[SerializeField]
	private TextAsset poseList_M;

	[SerializeField]
	private TextAsset mouthExpressionList_F;

	[SerializeField]
	private TextAsset mouthExpressionList_M;

	[SerializeField]
	private TextAsset eyeExpressionList_F;

	[SerializeField]
	private TextAsset eyeExpressionList_M;

	[SerializeField]
	private Dropdown poseTypeDropdown;

	[SerializeField]
	private Dropdown poseNoDropdown;

	[SerializeField]
	private Toggle poseCameraFocus;

	private List<AnimeAndNameSet> poseSets_stand = new List<AnimeAndNameSet>();

	private List<AnimeAndNameSet> poseSets_floor = new List<AnimeAndNameSet>();

	private List<AnimeAndNameSet> poseSets_chair = new List<AnimeAndNameSet>();

	[SerializeField]
	private Dropdown eyeExpressionDropdown;

	[SerializeField]
	private Dropdown mouthExpressionDropdown;

	[SerializeField]
	private Slider tearSlider;

	[SerializeField]
	private Slider flushSlider;

	private List<AnimeAndNameSet> eyeExpressionSets = new List<AnimeAndNameSet>();

	private List<AnimeAndNameSet> mouthExpressionSets = new List<AnimeAndNameSet>();

	[SerializeField]
	private Config configOriginal;

	private Config config;

	[SerializeField]
	private PauseMenue pauseMenueOriginal;

	private PauseMenue pauseMenue;

	[SerializeField]
	private LightController light;

	[SerializeField]
	private Button[] toCustomMs;

	[SerializeField]
	private Button[] toCustomFs;

	[SerializeField]
	private Button toReturn;

	[SerializeField]
	private CharaMoveController charaMoveUI;

	private IllusionCamera camera;

	private string bgm = "Select";

	private float headForcus = -1f;

	private string nextScene = string.Empty;

	private void Start()
	{
		InScene();
		base.GC.audioCtrl.BGM_LoadAndPlay(bgm);
		camera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		poseTypeDropdown.onValueChanged.AddListener(OnChangePoseType);
		poseNoDropdown.onValueChanged.AddListener(OnChangePoseNo);
		eyeExpressionDropdown.onValueChanged.AddListener(OnChangeEyeExpression);
		mouthExpressionDropdown.onValueChanged.AddListener(OnChangeMouthExpression);
		string charaName = string.Empty;
		AnalyzeMessage(sceneInMsg, out charaName, out nextScene);
		HEROINE hEROINE = HEROINE.NUM;
		MALE_ID mALE_ID = MALE_ID.NUM;
		if (charaName.Length > 0)
		{
			for (HEROINE hEROINE2 = HEROINE.RITSUKO; hEROINE2 < HEROINE.NUM; hEROINE2++)
			{
				if (charaName == Female.HeroineName(hEROINE2))
				{
					hEROINE = hEROINE2;
					break;
				}
			}
			for (MALE_ID mALE_ID2 = MALE_ID.HERO; mALE_ID2 < MALE_ID.NUM; mALE_ID2++)
			{
				if (charaName == Male.MaleName(mALE_ID2))
				{
					mALE_ID = mALE_ID2;
					break;
				}
			}
		}
		if (hEROINE != HEROINE.NUM)
		{
			for (int i = 0; i < toCustomMs.Length; i++)
			{
				toCustomMs[i].gameObject.SetActive(true);
			}
			for (int j = 0; j < toCustomFs.Length; j++)
			{
				toCustomFs[j].gameObject.SetActive(j != (int)hEROINE);
			}
			CreateFemale(hEROINE);
		}
		else if (mALE_ID != MALE_ID.NUM)
		{
			for (int k = 0; k < toCustomMs.Length; k++)
			{
				toCustomMs[k].gameObject.SetActive(k != (int)mALE_ID);
			}
			for (int l = 0; l < toCustomFs.Length; l++)
			{
				toCustomFs[l].gameObject.SetActive(true);
			}
			CreateMale(mALE_ID);
		}
		for (int m = 0; m < toCustomMs.Length; m++)
		{
			int no = m;
			toCustomMs[m].onClick.AddListener(delegate
			{
				Button_Custom_M(no);
			});
		}
		for (int n = 0; n < toCustomFs.Length; n++)
		{
			int no2 = n;
			toCustomFs[n].onClick.AddListener(delegate
			{
				Button_Custom_F(no2);
			});
		}
		if (nextScene == "GameStart")
		{
			toReturn.GetComponentInChildren<Text>().text = "game start";
		}
		else
		{
			toReturn.GetComponentInChildren<Text>().text = "Selection screen";
		}
		config = UnityEngine.Object.Instantiate(configOriginal);
		config.gameObject.SetActive(false);
		pauseMenue = UnityEngine.Object.Instantiate(pauseMenueOriginal);
		pauseMenue.Setup(true, true, OpenConfig, Button_Title, OnExit);
		light.SetupLight();
		poseCameraFocus.isOn = GlobalData.poseChangeCameraFocus;
	}

	public static string CreateMessage(string charaName, string next)
	{
		string text = string.Empty;
		if (charaName != null)
		{
			text = text + "chara:" + charaName;
		}
		if (next != null)
		{
			if (text.Length > 0)
			{
				text += ",";
			}
			text = text + "next:" + next;
		}
		return text;
	}

	private static void AnalyzeMessage(string msg, out string charaName, out string next)
	{
		string[] array = msg.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		charaName = string.Empty;
		next = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 2)
			{
				if (array2[0] == "chara")
				{
					charaName = array2[1];
				}
				else if (array2[0] == "next")
				{
					next = array2[1];
				}
			}
		}
	}

	private void Update()
	{
		sceneCanvas.enabled = !config.isActiveAndEnabled && !gameCtrl.IsHideUI;
		pauseMenue.EnableConfig = !config.isActiveAndEnabled;
		editMode.ShowUI(!config.gameObject.activeSelf);
		UpdateShortCutKeys();
		if (headForcus > 0f)
		{
			headForcus -= Time.deltaTime;
			if (headForcus <= 0f)
			{
				camera.SetFocus(human.FacePos, true);
			}
		}
		GlobalData.poseChangeCameraFocus = poseCameraFocus.isOn;
	}

	private void UpdateShortCutKeys()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			CameraReset();
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			camera.SetFocus(human.FacePos, true);
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			string text = ((human.sex != 0) ? "cm_J_Spine03" : "cf_J_Spine03");
			Transform transform = Transform_Utility.FindTransform_Partial(human.body.AnimatedBoneRoot, text);
			if (transform != null)
			{
				camera.SetFocus(transform.position, true);
			}
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			string text2 = ((human.sex != 0) ? "cm_J_Kokan" : "cf_J_Kokan");
			Transform transform2 = Transform_Utility.FindTransform_Partial(human.body.AnimatedBoneRoot, text2);
			if (transform2 != null)
			{
				camera.SetFocus(transform2.position, true);
			}
		}
	}

	private void CameraReset()
	{
		string text = ((human.sex != 0) ? "cm_J_Spine03" : "cf_J_Spine03");
		Transform transform = Transform_Utility.FindTransform_Partial(human.body.AnimatedBoneRoot, text);
		if (transform != null)
		{
			Vector3 facePos = human.FacePos;
			Vector3 vector = transform.TransformPoint(new Vector3(0f, 0f, 0.08f));
			Vector3 focus = (facePos + vector) * 0.5f;
			Vector3 rotate = new Vector3(10f, 0f, 0f);
			float num = 25f / ConfigData.defParse;
			camera.Set(focus, rotate, 1.5f * num, ConfigData.defParse);
		}
	}

	private void SetupCreatedHuman()
	{
		equipShow.Setup(human);
		if (human.sex == SEX.FEMALE)
		{
			PoseSetupList(poseList_F);
			SetupList(eyeExpressionSets, eyeExpressionList_F, eyeExpressionDropdown);
			SetupList(mouthExpressionSets, mouthExpressionList_F, mouthExpressionDropdown);
		}
		else
		{
			PoseSetupList(poseList_M);
			SetupList(eyeExpressionSets, eyeExpressionList_M, eyeExpressionDropdown);
			SetupList(mouthExpressionSets, mouthExpressionList_M, mouthExpressionDropdown);
		}
		editMode.Setup(human, this);
		editMode.gameObject.SetActive(true);
	}

	private void SetupList(List<AnimeAndNameSet> sets, TextAsset textAsset, Dropdown dropdown)
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(textAsset);
		int attributeNo = customDataListLoader.GetAttributeNo("anime");
		int attributeNo2 = customDataListLoader.GetAttributeNo("name");
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			sets.Add(new AnimeAndNameSet(data, data2));
			list.Add(new Dropdown.OptionData(i.ToString("00:") + data2));
		}
		dropdown.ClearOptions();
		dropdown.options = list;
	}

	private void PoseSetupList(TextAsset textAsset)
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(textAsset);
		int attributeNo = customDataListLoader.GetAttributeNo("anime");
		int attributeNo2 = customDataListLoader.GetAttributeNo("name");
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			if (data2.IndexOf("床") == 0)
			{
				poseSets_floor.Add(new AnimeAndNameSet(data, data2));
			}
			else if (data2.IndexOf("椅子") == 0)
			{
				poseSets_chair.Add(new AnimeAndNameSet(data, data2));
			}
			else
			{
				poseSets_stand.Add(new AnimeAndNameSet(data, data2));
			}
		}
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		list.Add(new Dropdown.OptionData("立ち"));
		if (human.sex == SEX.FEMALE)
		{
			list.Add(new Dropdown.OptionData("床"));
			list.Add(new Dropdown.OptionData("椅子"));
		}
		poseTypeDropdown.ClearOptions();
		poseTypeDropdown.options = list;
		ChangePoseNoDropdown();
	}

	private void CreateFemale(HEROINE heroineID)
	{
		Female female = ResourceUtility.CreateInstance<Female>(path_FemaleBody, "Female");
		female.SetHeroineID(heroineID);
		human = female;
		human.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		female.GagShow = false;
		CustomParameter heroineCustomParam = GlobalData.PlayData.GetHeroineCustomParam(heroineID);
		human.Load(heroineCustomParam);
		SetupCreatedHuman();
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", "pose");
		human.body.Anime.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_Edit_F");
		assetBundleController.Close();
		GameObject[] array = createButtons;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		Resources.UnloadUnusedAssets();
		female.ChangeShowGag(false);
		female.ShowMosaic(false);
		IllusionCamera illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		human.ChangeEyeLook(LookAtRotator.TYPE.TARGET, illusionCamera.transform, false);
		CameraReset();
		tearSlider.interactable = human.sex == SEX.FEMALE;
		flushSlider.interactable = human.sex == SEX.FEMALE;
		charaMoveUI.Setup(human.transform);
		charaMoveUI.SetDef(Vector3.zero, Quaternion.Euler(0f, 180f, 0f));
		female.ExpressionPlay(1, "Mouth_Def", 0f);
	}

	private void CreateMale(MALE_ID maleID)
	{
		Male male = ResourceUtility.CreateInstance<Male>(path_MaleBody, "Male");
		male.SetMaleID(maleID);
		human = male;
		human.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		CustomParameter maleCustomParam = GlobalData.PlayData.GetMaleCustomParam(maleID);
		human.Load(maleCustomParam);
		SetupCreatedHuman();
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", "pose");
		human.body.Anime.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_Edit_M");
		assetBundleController.Close();
		GameObject[] array = createButtons;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		Resources.UnloadUnusedAssets();
		male.SetShowTinWithWear(false);
		IllusionCamera illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		human.ChangeEyeLook(LookAtRotator.TYPE.TARGET, illusionCamera.transform, false);
		CameraReset();
		tearSlider.interactable = human.sex == SEX.FEMALE;
		flushSlider.interactable = human.sex == SEX.FEMALE;
		charaMoveUI.Setup(human.transform);
		charaMoveUI.SetDef(Vector3.zero, Quaternion.Euler(0f, 180f, 0f));
	}

	public void Button_CreateFemale(int no)
	{
		CreateFemale((HEROINE)no);
	}

	public void Button_CreateMale(int no)
	{
		CreateMale((MALE_ID)no);
	}

	private void OnChangePoseType(int id)
	{
		ChangePoseNoDropdown();
		poseNoDropdown.value = 0;
		List<AnimeAndNameSet> poseSet = GetPoseSet(poseTypeDropdown.value);
		human.body.Anime.CrossFadeInFixedTime(poseSet[0].anime, 0.1f);
	}

	private void OnChangePoseNo(int id)
	{
		List<AnimeAndNameSet> poseSet = GetPoseSet(poseTypeDropdown.value);
		human.body.Anime.CrossFadeInFixedTime(poseSet[id].anime, 0.1f);
		if (poseCameraFocus.isOn)
		{
			headForcus = 0.1f;
		}
	}

	private List<AnimeAndNameSet> GetPoseSet(int no)
	{
		switch (no)
		{
		case 0:
			return poseSets_stand;
		case 1:
			return poseSets_floor;
		case 2:
			return poseSets_chair;
		default:
			return poseSets_stand;
		}
	}

	private void ChangePoseNoDropdown()
	{
		List<AnimeAndNameSet> poseSet = GetPoseSet(poseTypeDropdown.value);
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		for (int i = 0; i < poseSet.Count; i++)
		{
			list.Add(new Dropdown.OptionData(i.ToString("00")));
		}
		poseNoDropdown.ClearOptions();
		poseNoDropdown.AddOptions(list);
	}

	private void OnChangeEyeExpression(int id)
	{
		human.ExpressionPlay(0, eyeExpressionSets[id].anime, 0.1f);
	}

	private void OnChangeMouthExpression(int id)
	{
		human.ExpressionPlay(1, mouthExpressionSets[id].anime, 0.1f);
	}

	public void MovePose(int move)
	{
		DropdownMove(poseNoDropdown, move);
	}

	public void MoveEyeExpression(int move)
	{
		DropdownMove(eyeExpressionDropdown, move);
	}

	public void MoveMouthExpression(int move)
	{
		DropdownMove(mouthExpressionDropdown, move);
	}

	private void DropdownMove(Dropdown dropdown, int move)
	{
		dropdown.value = (dropdown.options.Count + dropdown.value + move) % dropdown.options.Count;
	}

	public void ExpressionPreset(int no)
	{
		string[] array = new string[8] { "無表情", "喜び", "不機嫌", "怒り", "悲しい", "痛がり", "恥ずかし", "感じ" };
		string[] array2 = new string[8] { "無表情", "喜び", "不機嫌", "怒り", "悲しい", "痛がり", "困り", "無表情" };
		string[] array3 = new string[8] { "無表情", "笑顔", "怒り", "怒り", "悲しみ", "痛がり", "笑顔", "スケベ顔" };
		string[] array4 = new string[8] { "無表情", "笑顔１", "無表情", "怒り", "悲しみ", "痛がり", "ニヤリ左", "スケベ顔" };
		int value = 0;
		int value2 = 0;
		for (int i = 0; i < eyeExpressionSets.Count; i++)
		{
			string text = ((human.sex != 0) ? array3[no] : array[no]);
			if (eyeExpressionSets[i].name == text)
			{
				value = i;
				break;
			}
		}
		for (int j = 0; j < mouthExpressionSets.Count; j++)
		{
			string text2 = ((human.sex != 0) ? array4[no] : array2[no]);
			if (mouthExpressionSets[j].name == text2)
			{
				value2 = j;
				break;
			}
		}
		eyeExpressionDropdown.value = value;
		mouthExpressionDropdown.value = value2;
	}

	public void OnChangeEyeOpen(float val)
	{
		human.OpenEye(val);
	}

	public void OnChangeMouthOpen(float val)
	{
		human.OpenMouth(val);
	}

	public void OnChangeFlush(float val)
	{
		human.SetFlush(val, true);
	}

	public void OnChangeTear(float val)
	{
		human.SetTear(val, true);
	}

	public void OnChangeEye(int no)
	{
		IllusionCamera illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		switch (no)
		{
		case 0:
			human.ChangeEyeLook(LookAtRotator.TYPE.FORWARD, null, false);
			break;
		case 1:
			human.ChangeEyeLook(LookAtRotator.TYPE.TARGET, illusionCamera.transform, false);
			break;
		case 2:
			human.ChangeEyeLook(LookAtRotator.TYPE.HOLD, null, false);
			break;
		}
	}

	public void OnChangeNeck(int no)
	{
		IllusionCamera illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		switch (no)
		{
		case 0:
			human.ChangeNeckLook(LookAtRotator.TYPE.NO, null, false);
			break;
		case 1:
			human.ChangeNeckLook(LookAtRotator.TYPE.TARGET, illusionCamera.transform, false);
			break;
		case 2:
			human.ChangeNeckLook(LookAtRotator.TYPE.HOLD, null, false);
			break;
		}
	}

	public void OpenConfig()
	{
		config.gameObject.SetActive(true);
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_open);
	}

	public void Button_Back()
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		if (nextScene == "GameStart")
		{
			SaveCaution("ゲームを開始する", ToGameStart);
		}
		else
		{
			SaveCaution("選択画面へ戻る", ToSelect);
		}
	}

	private void Button_Title()
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		string text = "タイトルに戻ります。\nセーブしていない場合は\nゲームの進行状況、キャラカスタム等は失われます。\nよろしいですか？";
		gameCtrl.CreateModalYesNoUI(text, ToTitle);
	}

	private void ToTitle()
	{
		gameCtrl.ChangeScene("TitleScene", string.Empty, 0.5f);
	}

	private void ToSelect(bool record)
	{
		if (record)
		{
			RecordCustomData();
		}
		gameCtrl.ChangeScene("SelectScene", string.Empty, 0.5f);
	}

	public void OnExit()
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		string text = "ゲームを終了します。\nセーブしていない場合は\nゲームの進行状況、キャラカスタム等は失われます。\nよろしいですか？";
		gameCtrl.CreateModalYesNoUI(text, ToExit);
	}

	private void ToExit()
	{
		gameCtrl.ChangeScene("ExitScene", string.Empty, 1f);
	}

	public void Button_GameStart()
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		SaveCaution("ゲームを開始する", ToGameStart);
	}

	private void ToGameStart(bool record)
	{
		if (record)
		{
			RecordCustomData();
		}
		base.GC.ChangeScene("ADVScene", "adv/adv_00_00,ADV_Script_00_00", 1f);
	}

	public void Button_DemoMovie()
	{
	}

	private void RecordCustomData()
	{
		CustomParameter customParameter = null;
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			customParameter = GlobalData.PlayData.GetHeroineCustomParam(female.HeroineID);
		}
		else
		{
			Male male = human as Male;
			customParameter = GlobalData.PlayData.GetMaleCustomParam(male.MaleID);
		}
		if (customParameter != null)
		{
			customParameter.Copy(human.customParam);
		}
	}

	private void Button_Custom_M(int no)
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		string nextName = Male.MaleName((MALE_ID)no) + "のカスタムへ切り替える";
		SaveCaution(nextName, delegate(bool b)
		{
			ToCustom_M(b, no);
		});
	}

	private void ToCustom_M(bool record, int no)
	{
		if (record)
		{
			RecordCustomData();
		}
		string charaName = Male.MaleName((MALE_ID)no);
		string msg = CreateMessage(charaName, nextScene);
		base.GC.ChangeScene("EditScene", msg, 1f);
	}

	private void Button_Custom_F(int no)
	{
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
		string nextName = Female.HeroineName((HEROINE)no) + "のカスタムへ切り替える";
		SaveCaution(nextName, delegate(bool b)
		{
			ToCustom_F(b, no);
		});
	}

	private void ToCustom_F(bool record, int no)
	{
		if (record)
		{
			RecordCustomData();
		}
		string charaName = Female.HeroineName((HEROINE)no);
		string msg = CreateMessage(charaName, nextScene);
		base.GC.ChangeScene("EditScene", msg, 1f);
	}

	private void SaveCaution(string nextName, Action<bool> next)
	{
		string[] choices = new string[3] { "適用する", "適用しない", "カスタムに戻る" };
		Action[] acts = new Action[3]
		{
			delegate
			{
				next(true);
			},
			delegate
			{
				next(false);
			},
			null
		};
		string empty = string.Empty;
		empty = empty + nextName + "前に、";
		empty = empty + NowCharaName() + "の変更を適用しますか？\n";
		empty += "(ゲームで使用する場合は変更を適用する必要があります。\n";
		empty += "適用しない場合、カスタム画面が開かれる前の状態に戻り\n";
		empty += "現在のキャラはカードとして保存していない場合は失われます)";
		gameCtrl.CreateModalChoices(empty, choices, acts);
	}

	private string NowCharaName()
	{
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			return Female.HeroineName(female.HeroineID);
		}
		if (human.sex == SEX.MALE)
		{
			Male male = human as Male;
			return Male.MaleName(male.MaleID);
		}
		return "- UNKNOWN -";
	}
}
