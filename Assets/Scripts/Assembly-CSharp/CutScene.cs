using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class CutScene : Scene
{
	public enum FOCUS
	{
		NONE = 0,
		POS = 1,
		CHARA = 2
	}

	[SerializeField]
	public Female femaleOriginal;

	[SerializeField]
	public Male maleOriginal;

	public string listAssetBundle;

	public string listAssetName;

	public Camera camera;

	private DepthOfField dof;

	private FOCUS focusType;

	private Vector3 focusPos;

	private Human focusChara;

	[SerializeField]
	private GameObject subtitleRoot;

	[SerializeField]
	private Text subTitle;

	[SerializeField]
	private Image colorFilter;

	[SerializeField]
	private Image image;

	private Dictionary<string, Human> humans = new Dictionary<string, Human>();

	private Dictionary<string, CharaCutActionSet> humanActSets = new Dictionary<string, CharaCutActionSet>();

	private List<Female> females = new List<Female>();

	private List<Male> males = new List<Male>();

	private Map map;

	private CustomDataListLoader list = new CustomDataListLoader();

	private int timeID;

	private int charaID = 1;

	private int textID = 2;

	private int voiceID = 3;

	private AssetBundleController assetBundleCtrl = new AssetBundleController();

	private CutActFactory factory;

	private List<CutAction> actions = new List<CutAction>();

	private List<CutAct_ColorFilter> act_ColorFilters = new List<CutAct_ColorFilter>();

	private List<CutAct_SubTitle> act_SubTitles = new List<CutAct_SubTitle>();

	private List<CutAct_Camera> act_Camera = new List<CutAct_Camera>();

	private int actionNo;

	private float timer;

	private static readonly string[] ActNames = new string[17]
	{
		"JumpScript", "ColorFilter", "Image", "BGM", "SE", "Voice", "SubTitle", "Anime", "Expression", "Camera",
		"Position", "CharaShow", "IK", "Light", "NextScene", "MemoryEnd", "GameVariable"
	};

	public List<CutAction> Actions
	{
		get
		{
			return actions;
		}
	}

	public Color FilterColor { get; set; }

	public bool IsPlay { get; set; }

	public float LastVoiceTime { get; set; }

	private void Awake()
	{
		InScene();
		base.GC.audioCtrl.BGM_Stop();
		if (sceneInMsg.Length > 0)
		{
			string[] array = sceneInMsg.Split(',');
			if (array.Length == 2)
			{
				listAssetBundle = array[0];
				listAssetName = array[1];
			}
			else
			{
				Debug.LogError("シーンインメッセージが不明:" + sceneInMsg);
			}
		}
		factory = new CutActFactory(this);
		factory.Register(ActNames[0], new CutAct_JumpScript(this));
		factory.Register(ActNames[1], new CutAct_ColorFilter(this));
		factory.Register(ActNames[2], new CutAct_Image(this));
		factory.Register(ActNames[3], new CutAct_BGM(this));
		factory.Register(ActNames[4], new CutAct_SE(this));
		factory.Register(ActNames[5], new CutAct_Voice(this));
		factory.Register(ActNames[6], new CutAct_SubTitle(this));
		factory.Register(ActNames[7], new CutAct_Anime(this));
		factory.Register(ActNames[8], new CutAct_Expression(this));
		factory.Register(ActNames[9], new CutAct_Camera(this));
		factory.Register(ActNames[10], new CutAct_Position(this));
		factory.Register(ActNames[11], new CutAct_CharaShow(this));
		factory.Register(ActNames[12], new CutAct_IK(this));
		factory.Register(ActNames[13], new CutAct_Light(this));
		factory.Register(ActNames[14], new CutAct_NextScene(this));
		factory.Register(ActNames[15], new CutAct_MemoryEnd(this));
		factory.Register(ActNames[16], new CutAct_GameVariable(this));
		assetBundleCtrl.OpenFromFile(Application.persistentDataPath + "/abdata", listAssetBundle);
		LoadScript_AssetBundle(listAssetName);
		dof = camera.GetComponent<DepthOfField>();
		IsPlay = true;
		LastVoiceTime = -1f;
	}

	private void Start()
	{
		InScene();
	}

	public override void OutScene(string next)
	{
		assetBundleCtrl.Close();
	}

	private void OnDestroy()
	{
		assetBundleCtrl.Close();
	}

	private void Update()
	{
		subtitleRoot.gameObject.SetActive(!gameCtrl.IsHideUI);
		if (!IsPlay)
		{
			return;
		}
		SetHeight();
		bool flag = false;
		while (actionNo < actions.Count && actions[actionNo].time <= timer)
		{
			CUTACT type = actions[actionNo].Type;
			actions[actionNo].Action(false);
			if (actions[actionNo].Type == CUTACT.VOICE)
			{
				LastVoiceTime = timer;
			}
			if (type == CUTACT.JUMPSCRIPT)
			{
				flag = true;
				break;
			}
			actionNo++;
		}
		if (!flag && Click())
		{
			SkipNext();
		}
		Check_ColorFilter(timer);
		if (!flag)
		{
			timer += Time.deltaTime;
		}
	}

	private void LateUpdate()
	{
		float focalLength = 0f;
		if (focusType == FOCUS.POS)
		{
			focalLength = Vector3.Distance(camera.transform.position, focusPos);
		}
		else if (focusType == FOCUS.CHARA)
		{
			focalLength = Vector3.Distance(camera.transform.position, focusChara.FacePos);
		}
		dof.enabled = ConfigData.dofEnable;
		dof.focalLength = focalLength;
	}

	public void SetHeight()
	{
	}

	private bool Click()
	{
		return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
	}

	private void SkipNext()
	{
		if (actionNo >= actions.Count)
		{
			return;
		}
		bool flag = false;
		timer = actions[actionNo].time;
		while (actionNo < actions.Count && actions[actionNo].time <= timer)
		{
			if (actions[actionNo].Type == CUTACT.SUBTITLE)
			{
				timer = actions[actionNo].time;
				flag = true;
			}
			else if (actions[actionNo].Type == CUTACT.VOICE)
			{
				if (LastVoiceTime != timer)
				{
					foreach (Human value in humans.Values)
					{
						if (!value.IsLoopVoicePlaying())
						{
							value.VoiceShutUp();
						}
					}
				}
				LastVoiceTime = timer;
			}
			CUTACT type = actions[actionNo].Type;
			actions[actionNo].Action(true);
			if (type == CUTACT.JUMPSCRIPT || type == CUTACT.NEXTSCENE)
			{
				break;
			}
			actionNo++;
			if (!flag && actionNo < actions.Count)
			{
				timer = actions[actionNo].time;
			}
		}
	}

	public void JumpScript(string nextAssetBundle, string nextScript)
	{
		assetBundleCtrl.Close();
		assetBundleCtrl.OpenFromFile(Application.persistentDataPath + "/abdata", nextAssetBundle);
		LoadScript_AssetBundle(nextScript);
	}

	public void LoadScript_AssetBundle(string assetName)
	{
		TextAsset text = assetBundleCtrl.LoadAsset<TextAsset>(assetName);
		TagText tt = new TagText(text);
		LoadScript(tt);
	}

	public void LoadScript_File(string textPath)
	{
		TagText tt = new TagText(textPath);
		LoadScript(tt);
	}

	public void LoadScript(TagText tt)
	{
		ClearScript();
		for (int i = 0; i < tt.Elements.Count; i++)
		{
			TagText.Element element = tt.Elements[i];
			if (element.Tag == "Setup")
			{
				LoadScript_Setup(element);
				continue;
			}
			CutAction cutAction = factory.Create(element.Tag) as CutAction;
			if (cutAction != null)
			{
				cutAction.Load(element, i);
				AddAction(cutAction);
				if (GlobalData.isMemory && cutAction.Type == CUTACT.MEMORYEND)
				{
					break;
				}
			}
		}
		SortActions();
		actionNo = 0;
		LastVoiceTime = -1f;
	}

	public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
    {
		return this.assetBundleCtrl.LoadAsset<T>(assetName);
	}

	public void SaveScript(string file)
	{
		TagText tagText = new TagText();
		foreach (CutAction action in actions)
		{
			string text = ActNames[(int)action.Type];
			TagText.Element element = new TagText.Element(text);
			action.Save(element);
			tagText.AddElement(element);
		}
		tagText.Save(file);
	}

	private void LoadScript_Setup(TagText.Element element)
	{
		string val = string.Empty;
		if (element.GetVal(ref val, "map", 0))
		{
			AssetBundleController assetBundleController = new AssetBundleController();
			assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", val);
			int num = val.LastIndexOf("/");
			string assetName = string.Empty;
			if (num != -1)
			{
				assetName = val.Substring(num + 1);
			}
			GameObject gameObject = assetBundleController.LoadAndInstantiate<GameObject>(assetName);
			if (gameObject != null)
			{
				map = gameObject.GetComponent<Map>();
			}
			else
			{
				map = null;
			}
			assetBundleController.Close();
			TagText.Attribute attribute = element.GetAttribute("light");
			if (attribute != null)
			{
				float val2 = 0f;
				float val3 = 0f;
				attribute.GetVal(ref val2, 0);
				attribute.GetVal(ref val3, 1);
				map.lightRoot.rotation = Quaternion.Euler(val2, val3, 0f);
			}
		}
		TagText.Attribute attribute2 = element.GetAttribute("male");
		if (attribute2 != null)
		{
			for (int i = 0; i < attribute2.vals.Count / 3; i++)
			{
				int num2 = i * 3;
				string text = attribute2.vals[num2];
				string text2 = attribute2.vals[num2 + 1];
				string text3 = attribute2.vals[num2 + 2];
				Male male = CreateMale(text);
				switch (text)
				{
				case "主人公":
				case "猛":
					male.SetMaleID(MALE_ID.HERO);
					break;
				case "広一":
					male.SetMaleID(MALE_ID.KOUICHI);
					break;
				}
				switch (text2)
				{
				case "MaleA":
					male.Load(GlobalData.PlayData.custom_hero);
					break;
				case "MaleB":
					male.Load(GlobalData.PlayData.custom_kouichi);
					break;
				case "MobA":
					male.Load(GlobalData.PlayData.custom_h_maleMobA);
					break;
				case "MobB":
					male.Load(GlobalData.PlayData.custom_h_maleMobB);
					break;
				case "MobC":
					male.Load(GlobalData.PlayData.custom_h_maleMobC);
					break;
				default:
					male.Apply();
					break;
				}
				male.ChangeMaleShow(MALE_SHOW.CLOTHING);
				if (text3 != "-")
				{
					male.body.Anime.runtimeAnimatorController = assetBundleCtrl.LoadAsset<RuntimeAnimatorController>(text3);
				}
				male.SetShowTinWithWear(false);
				male.SoundSpatialBlend(0f);
			}
		}
		TagText.Attribute attribute3 = element.GetAttribute("female");
		if (attribute3 != null)
		{
			for (int j = 0; j < attribute3.vals.Count / 3; j++)
			{
				int num3 = j * 3;
				string text4 = attribute3.vals[num3];
				string text5 = attribute3.vals[num3 + 1];
				string text6 = attribute3.vals[num3 + 2];
				Female female = CreateFemale(text4);
				if (text5 != "-")
				{
					switch (text5)
					{
					case "FemaleA":
						female.SetHeroineID(HEROINE.RITSUKO);
						female.Load(GlobalData.PlayData.custom_ritsuko);
						break;
					case "FemaleB":
						female.SetHeroineID(HEROINE.AKIKO);
						female.Load(GlobalData.PlayData.custom_akiko);
						break;
					case "FemaleC":
						female.SetHeroineID(HEROINE.YUKIKO);
						female.Load(GlobalData.PlayData.custom_yukiko);
						break;
					}
				}
				if (text6 != "-")
				{
					female.body.Anime.runtimeAnimatorController = assetBundleCtrl.LoadAsset<RuntimeAnimatorController>(text6);
				}
				female.SoundSpatialBlend(0f);
			}
		}
		foreach (KeyValuePair<string, Human> human in humans)
		{
			humanActSets.Add(human.Key, new CharaCutActionSet(human.Value));
			if (map != null)
			{
				human.Value.Foot(map.data.foot);
			}
		}
	}

	public void Check(float time)
	{
		IsPlay = false;
		Check_ColorFilter(time);
		Check_SubTitle(time);
		Check_Camera(time);
		Check_Chara(time);
	}

	public void NextScene(string scene, string message, float fadeTime = -1f)
	{
		assetBundleCtrl.Close();
		base.GC.ChangeScene(scene, message, fadeTime);
	}

	private void Check_ColorFilter(float time)
	{
		Color color = new Color(0f, 0f, 0f, 0f);
		if (act_ColorFilters.Count != 0)
		{
			CutAct_ColorFilter cutAct_ColorFilter = null;
			CutAct_ColorFilter cutAct_ColorFilter2 = null;
			time = Mathf.Clamp(time, act_ColorFilters[0].time, act_ColorFilters[act_ColorFilters.Count - 1].time);
			for (int i = 0; i < act_ColorFilters.Count; i++)
			{
				if (act_ColorFilters[i].time <= time)
				{
					cutAct_ColorFilter = act_ColorFilters[i];
				}
				if (act_ColorFilters[i].time >= time)
				{
					cutAct_ColorFilter2 = act_ColorFilters[i];
					break;
				}
			}
			if (cutAct_ColorFilter == cutAct_ColorFilter2)
			{
				color = cutAct_ColorFilter.color;
			}
			else
			{
				float t = Mathf.InverseLerp(cutAct_ColorFilter.time, cutAct_ColorFilter2.time, time);
				color = Color.Lerp(cutAct_ColorFilter.color, cutAct_ColorFilter2.color, t);
			}
		}
		colorFilter.color = color;
	}

	private void Check_SubTitle(float time)
	{
		string text = string.Empty;
		if (act_SubTitles.Count != 0)
		{
			CutAct_SubTitle cutAct_SubTitle = null;
			for (int i = 0; i < act_SubTitles.Count; i++)
			{
				if (act_SubTitles[i].time <= time)
				{
					cutAct_SubTitle = act_SubTitles[i];
				}
			}
			if (cutAct_SubTitle != null)
			{
				text = cutAct_SubTitle.text;
			}
		}
		SetSubTitle(text);
	}

	private void Check_Camera(float time)
	{
		if (act_Camera.Count == 0)
		{
			return;
		}
		CutAct_Camera cutAct_Camera = null;
		for (int i = 0; i < act_Camera.Count; i++)
		{
			if (act_Camera[i].time <= time)
			{
				cutAct_Camera = act_Camera[i];
			}
		}
		if (cutAct_Camera != null)
		{
			cutAct_Camera.SetCamera();
		}
	}

	private void Check_Chara(float time)
	{
		foreach (CharaCutActionSet value in humanActSets.Values)
		{
			value.Check(time);
		}
	}

	public Human GetHuman(string name)
	{
		if (!humans.ContainsKey(name))
		{
			return null;
		}
		return humans[name];
	}

	public void SetSubTitle(string text)
	{
		subTitle.text = text;
	}

	public void SetImage(string file)
	{
		Texture2D texture2D = assetBundleCtrl.LoadAsset<Texture2D>(file);
		if (texture2D == null)
		{
			Debug.LogError("テクスチャが読めない:" + file);
			return;
		}
		Vector2 vector = new Vector2(texture2D.width, texture2D.height);
		Rect rect = new Rect(Vector2.zero, vector);
		Vector2 pivot = vector * 0.5f;
		image.sprite = Sprite.Create(texture2D, rect, pivot, 100f, 0u, SpriteMeshType.FullRect);
	}

	public void ShowImage(bool show)
	{
		image.enabled = show;
	}

	public void SetCamera(Vector3 pos, Quaternion rot, float fov)
	{
		camera.transform.position = pos;
		camera.transform.rotation = rot;
		if (fov > 0f)
		{
			camera.fieldOfView = fov;
		}
	}

	public void SetFocus(Vector3 pos)
	{
		focusType = FOCUS.POS;
		focusPos = pos;
		focusChara = null;
	}

	public void SetFocus(string chara)
	{
		focusType = FOCUS.CHARA;
		focusChara = GetHuman(chara);
	}

	public void AddAction(CUTACT actType, float time)
	{
		IsPlay = false;
		CutAction cutAction = factory.Create(ActNames[(int)actType]) as CutAction;
		cutAction.time = time;
		if (cutAction != null)
		{
			AddAction(cutAction);
			SortActions();
		}
	}

	private void AddAction(CutAction act)
	{
		actions.Add(act);
		if (act.Type == CUTACT.COLORFILTER)
		{
			act_ColorFilters.Add(act as CutAct_ColorFilter);
		}
		else if (act.Type == CUTACT.SUBTITLE)
		{
			act_SubTitles.Add(act as CutAct_SubTitle);
		}
		else if (act.Type == CUTACT.CAMERA)
		{
			act_Camera.Add(act as CutAct_Camera);
		}
		else if (act.Type == CUTACT.ANIME)
		{
			CutAct_Anime cutAct_Anime = act as CutAct_Anime;
			if (humanActSets.ContainsKey(cutAct_Anime.chara))
			{
				humanActSets[cutAct_Anime.chara].AddAct(cutAct_Anime);
			}
			else
			{
				Debug.LogError("不明なキャラ：" + cutAct_Anime.chara);
			}
		}
		else if (act.Type == CUTACT.VOICE)
		{
			CutAct_Voice cutAct_Voice = act as CutAct_Voice;
			if (humanActSets.ContainsKey(cutAct_Voice.chara))
			{
				humanActSets[cutAct_Voice.chara].AddAct(cutAct_Voice);
			}
			else
			{
				Debug.LogError("不明なキャラ：" + cutAct_Voice.chara);
			}
		}
	}

	public void DeleteAction(CutAction act)
	{
		IsPlay = false;
		actions.Remove(act);
		if (act.Type == CUTACT.COLORFILTER)
		{
			act_ColorFilters.Remove(act as CutAct_ColorFilter);
		}
		else if (act.Type == CUTACT.SUBTITLE)
		{
			act_SubTitles.Remove(act as CutAct_SubTitle);
		}
		else if (act.Type == CUTACT.CAMERA)
		{
			act_Camera.Remove(act as CutAct_Camera);
		}
		else if (act.Type == CUTACT.ANIME)
		{
			CutAct_Anime cutAct_Anime = act as CutAct_Anime;
			humanActSets[cutAct_Anime.chara].RemoveAct(cutAct_Anime);
		}
		else if (act.Type == CUTACT.VOICE)
		{
			CutAct_Voice cutAct_Voice = act as CutAct_Voice;
			humanActSets[cutAct_Voice.chara].RemoveAct(cutAct_Voice);
		}
	}

	private void ClearScript()
	{
		actions.Clear();
		act_ColorFilters.Clear();
		act_SubTitles.Clear();
		act_Camera.Clear();
		humanActSets.Clear();
		foreach (KeyValuePair<string, Human> human in humans)
		{
            UnityEngine.Object.Destroy(human.Value.gameObject);
		}
		humans.Clear();
		females.Clear();
		males.Clear();
		focusChara = null;
		focusType = FOCUS.NONE;
		if (map != null)
		{
            UnityEngine.Object.Destroy(map.gameObject);
		}
		actionNo = 0;
		timer = 0f;
		LastVoiceTime = -1f;
	}

	public void SortActions()
	{
		actions.Sort(SortFunc);
		act_ColorFilters.Sort(SortFunc);
		act_SubTitles.Sort(SortFunc);
		act_Camera.Sort(SortFunc);
		foreach (CharaCutActionSet value in humanActSets.Values)
		{
			value.Sort();
		}
	}

	public static int SortFunc(CutAction a, CutAction b)
	{
		if (a == null || b == null)
		{
			Debug.LogError("アクションがNULL");
		}
		float num = a.time - b.time;
		int num2 = (int)num;
		if (num < 0f)
		{
			num2--;
		}
		if (num > 0f)
		{
			num2++;
		}
		if (num == 0f)
		{
			num2 = a.order - b.order;
		}
		return num2;
	}

	public Female CreateFemale(string name)
	{
		Female female = UnityEngine.Object.Instantiate<Female>(femaleOriginal);
		female.ChangeShowGag(false);
		humans.Add(name, female);
		females.Add(female);
		return female;
	}

	public Male CreateMale(string name)
	{
		Male male = UnityEngine.Object.Instantiate<Male>(maleOriginal);
		humans.Add(name, male);
		males.Add(male);
		return male;
	}

	public void SetLight(float yaw, float pitch)
	{
		if (map != null)
		{
			map.lightRoot.rotation = Quaternion.Euler(pitch, yaw, 0f);
		}
	}
}
