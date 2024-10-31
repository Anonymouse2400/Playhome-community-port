using System;
using System.Collections.Generic;
using System.IO;
using Character;
using H;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class H_Scene : Scene
{
    private enum EDIT_CHARA
    {
        NONE,
        FEMALE,
        MALE,
        VISITOR,
        MOB_A,
        MOB_B,
        MOB_C
    }

    public MixController MixCtrl;

    public static readonly int AnmParamID_Pose = Animator.StringToHash("Pose");

    public static readonly int AnmParamID_Stroke = Animator.StringToHash("Stroke");

    public static readonly int AnmParamID_Height = Animator.StringToHash("Height");

    public static readonly int AnmParamID_Bust = Animator.StringToHash("Bust");

    private static readonly bool[] ui_StateCheckList = new bool[27]
    {
        true, true, true, false, true, true, false, false, false, false,
        false, false, false, false, false, true, true, false, true, false,
        true, false, false, true, false, true, false
    };

    public float femaleGageSpeed = 1f;

    public float maleGageSpeed = 1f;

    public float highSpeed = 2f;

    public float hitSpeedRate = 2f;

    public Slider femaleGage;

    public Toggle femaleGageLock;

    public Slider maleGage;

    public Toggle maleGageLock;

    public Button buttonInEja;

    public Button buttonOutEja;

    public Button buttonXTC_F;

    public Button buttonXTC_W;

    public Button buttonExtract;

    public Button buttonVomit;

    public Button buttonDrink;

    public GameObject padRoot;

    public Button talkButton;

    public H_SE se;

    private float femaleXTCEnableVal = 2f / 3f;

    public List<H_Members> members = new List<H_Members>();

    public H_Visitor visitor;

    [SerializeField]
    private Canvas uiCanvas;

    [SerializeField]
    private Canvas middleLeftCanvas;

    [SerializeField]
    private HStyleChangeUI styleChangeUI;

    [SerializeField]
    private HWearAcceChangeUI wearAcceChangeUI;

    [SerializeField]
    private HMaleShowChangeUIManager maleShowChangeUI;

    [SerializeField]
    private Config configOriginal;

    private Config config;

    [SerializeField]
    private PauseMenue pauseMenueOriginal;

    private PauseMenue pauseMenue;

    [SerializeField]
    private LightController light;

    [SerializeField]
    private H_Light h_light;

    [SerializeField]
    private H_CharaMoveController charaMove;

    [SerializeField]
    private GameObject badgeFeelVagina;

    [SerializeField]
    private GameObject badgeFeelAnus;

    [SerializeField]
    private GameObject badgeIndecentLanguage;

    [SerializeField]
    private GameObject badgeLikeFeratio;

    [SerializeField]
    private GameObject badgeLikeSperm;

    [SerializeField]
    private Button buttonExit;

    private H_Voice voice;

    private H_Expression expression;

    private H_Expression_Male expression_M;

    private H_VisitorVoice visitorVoice;

    private H_VisitorExpression visitorExpression;

    private Dictionary<string, H_StyleData> styleDatas = new Dictionary<string, H_StyleData>();

    private Dictionary<string, H_PoseData> poseDatas = new Dictionary<string, H_PoseData>();

    private Dictionary<string, H_PoseData> visitorDatas = new Dictionary<string, H_PoseData>();

    private int mainMembersNo = -1;

    private IllusionCamera camera;

    private IllusionCameraResetData cameraReset;

    private AssetBundleController[] voiceABC = new AssetBundleController[3];

    [SerializeField]
    private EditMode editModeOriginal;

    private EditMode editMode;

    private EDIT_CHARA editChara;

    [SerializeField]
    private Transform moveableRoot;

    [SerializeField]
    private MoveableColorCustomUI colorUI;

    [SerializeField]
    private Canvas hEditModeEndCanvas;

    [SerializeField]
    private ToggleButton editToggleButton;

    [SerializeField]
    private Button editFemale;

    [SerializeField]
    private Button editMale;

    [SerializeField]
    private Button editVisitor;

    [SerializeField]
    private Button editMaleMobA;

    [SerializeField]
    private Button editMaleMobB;

    [SerializeField]
    private Button editMaleMobC;

    [SerializeField]
    private ToggleButton mapToggleButton;

    [SerializeField]
    private ToggleButton moveToggleButton;

    [SerializeField]
    private Animator mapIK_Original;

    private Dictionary<string, IllusionCameraResetData> cameraDatas = new Dictionary<string, IllusionCameraResetData>();

    [SerializeField]
    private GameObject swapUIRoot;

    [SerializeField]
    private Button swapButton;

    [SerializeField]
    private Text mainFemaleName;

    [SerializeField]
    private H_GagUI gagUI;

    [SerializeField]
    private GameObject weaknessShowUI;

    [SerializeField]
    private Button weaknessRecoveryButton;

    public HStyleChangeUI StyleChangeUI
    {
        get
        {
            return styleChangeUI;
        }
    }

    public H_CharaMoveController CharaMove
    {
        get
        {
            return charaMove;
        }
    }

    public bool ExitEnable { get; set; }

    public H_Members mainMembers
    {
        get
        {
            return (mainMembersNo == -1) ? null : members[mainMembersNo];
        }
    }

    public IllusionCamera IllCam
    {
        get
        {
            return camera;
        }
    }

    public Map map { get; private set; }

    public MapMob mob { get; private set; }

    public bool IsEvent { get; private set; }

    public bool ExitStandBy { get; private set; }

    private void Awake()
    {
        InScene();
        config = UnityEngine.Object.Instantiate(configOriginal);
        config.gameObject.SetActive(false);
        pauseMenue = UnityEngine.Object.Instantiate(pauseMenueOriginal);
        pauseMenue.Setup(true, true, OpenConfig, null, null);
        AnalyzeMsg();
        ExitEnable = sceneInMsg.Length == 0;
        buttonExit.interactable = ExitEnable;
        HEROINE lastSelectFemale = GlobalData.PlayData.lastSelectFemale;
        Personality heroinePersonality = GlobalData.PlayData.GetHeroinePersonality(lastSelectFemale);
        if (sceneInMsg == null || sceneInMsg.Length == 0)
        {
            if (!Personality.IsFloped(heroinePersonality.state, lastSelectFemale))
            {
                string[] array = new string[3] { "12", "13", "14" };
                int num = UnityEngine.Random.Range(0, array.Length);
                gameCtrl.audioCtrl.BGM_LoadAndPlay(array[num]);
            }
            else
            {
                string[] array2 = new string[3] { "11", "15", "16" };
                int num2 = UnityEngine.Random.Range(0, array2.Length);
                gameCtrl.audioCtrl.BGM_LoadAndPlay(array2[num2]);
            }
        }
        camera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
        LoadStyles();
        maleShowChangeUI.Setup(this);
        expression = GetComponent<H_Expression>();
        expression.Setup();
        expression_M = GetComponent<H_Expression_Male>();
        expression_M.Setup();
        voice = GetComponent<H_Voice>();
        voice.Setup();
        visitorVoice = GetComponent<H_VisitorVoice>();
        visitorVoice.Setup();
        visitorExpression = GetComponent<H_VisitorExpression>();
        visitorExpression.Setup();
        voiceABC[0] = new AssetBundleController();
        voiceABC[0].OpenFromFile(Application.persistentDataPath + "/abdata", "h_voice_ritsuko");
        voiceABC[1] = new AssetBundleController();
        voiceABC[1].OpenFromFile(Application.persistentDataPath + "/abdata", "h_voice_akiko");
        voiceABC[2] = new AssetBundleController();
        voiceABC[2].OpenFromFile(Application.persistentDataPath + "/abdata", "h_voice_yukiko");
        LoadMap(false);
        H_Members h_Members = new H_Members(this);
        members.Add(h_Members);
        Female female = h_Members.AddFemale(lastSelectFemale);
        Male male = h_Members.AddMale(GlobalData.PlayData.lastSelectMale);
        mainMembersNo = 0;
        SetInput(h_Members, MixCtrl);
        badgeFeelVagina.SetActive(female.personality.feelVagina);
        badgeFeelAnus.SetActive(female.personality.feelAnus);
        badgeIndecentLanguage.SetActive(female.personality.indecentLanguage);
        badgeLikeFeratio.SetActive(female.personality.likeFeratio);
        badgeLikeSperm.SetActive(female.personality.likeSperm);
        AssetBundleController assetBundleController = new AssetBundleController(false);
        assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", "h/h_in");
        LoadPoses(assetBundleController);
        assetBundleController.Close();
        int lastSelectVisitor = (int)GlobalData.PlayData.lastSelectVisitor;
        bool flag = false;
        if (lastSelectVisitor >= 0 && lastSelectVisitor < 3)
        {
            assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", "h/h_visitor");
            LoadVisitorPoses(assetBundleController);
            HEROINE lastSelectVisitor2 = (HEROINE)GlobalData.PlayData.lastSelectVisitor;
            CustomParameter heroineCustomParam = GlobalData.PlayData.GetHeroineCustomParam(lastSelectVisitor2);
            RuntimeAnimatorController runtimeAC = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_Visitor");
            visitor = new H_Visitor(this, lastSelectVisitor2, heroineCustomParam, runtimeAC, visitorVoice, visitorExpression);
            assetBundleController.Close();
            flag = true;
        }
        else if (GlobalData.PlayData.lastSelectVisitor == VISITOR.KOUICHI)
        {
            assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", "h/h_visitor");
            LoadVisitorPoses(assetBundleController);
            CustomParameter maleCustomParam = GlobalData.PlayData.GetMaleCustomParam(MALE_ID.KOUICHI);
            RuntimeAnimatorController runtimeAC2 = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_Visitor_M");
            visitor = new H_Visitor(this, MALE_ID.KOUICHI, maleCustomParam, runtimeAC2);
            assetBundleController.Close();
            flag = true;
        }
        SetupEventCharacters(sceneInMsg, h_Members, visitor);
        swapUIRoot.gameObject.SetActive(flag);
        styleChangeUI.Setup(this, styleDatas);
        gagUI.Setup(this);
        SetMainFemaleName();
        h_Members.ChangeMap(map.data, false);
        charaMove.Setup(this);
        wearAcceChangeUI.Setup(this);
        UpdateMaleShowUI();
        editFemale.onClick.AddListener(CharaCustom_Female);
        editMale.onClick.AddListener(delegate
        {
            CharaCustom_Male(0);
        });
        editVisitor.onClick.AddListener(CharaCustom_Visitor);
        editMaleMobA.onClick.AddListener(delegate
        {
            CharaCustom_Male(1);
        });
        editMaleMobB.onClick.AddListener(delegate
        {
            CharaCustom_Male(2);
        });
        editMaleMobC.onClick.AddListener(delegate
        {
            CharaCustom_Male(3);
        });
        editVisitor.gameObject.SetActive(flag);
        editFemale.GetComponentInChildren<Text>().text = Female.HeroineName(h_Members.GetFemale(0).HeroineID);
        editMale.GetComponentInChildren<Text>().text = Male.MaleName(h_Members.GetMale(0).MaleID);
        if (flag)
        {
            Female female2 = visitor.GetHuman() as Female;
            if (female2 != null)
            {
                editVisitor.GetComponentInChildren<Text>().text = Female.HeroineName(female2.HeroineID);
            }
        }
        H_Pos h_pos = StartPose(0);
        int posNo = ((sceneInMsg.Length <= 0) ? (-1) : 0);
        VisitorPos(h_pos, posNo);
        if (IsEvent)
        {
            mapToggleButton.Interactable = false;
            moveToggleButton.Interactable = false;
        }
    }

    private void UpdateMaleShowUI()
    {
        maleShowChangeUI.SetMales(mainMembers.GetMales());
    }

    private void AnalyzeMsg()
    {
        if (sceneInMsg == null || sceneInMsg.Length == 0)
        {
            return;
        }
        if (sceneInMsg == "RitsukoFirstH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FIRST_RITSUKO;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.RITSUKO).state = Personality.STATE.FIRST;
            GlobalData.PlayData.lastSelectFemale = HEROINE.RITSUKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.HERO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.NONE;
            GlobalData.PlayData.lastSelectMap = 1;
            GlobalData.PlayData.lastSelectTimeZone = 1;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("12");
            IsEvent = true;
        }
        else if (sceneInMsg == "AkikoFirstH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FIRST_AKIKO;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.AKIKO).state = Personality.STATE.FIRST;
            GlobalData.PlayData.lastSelectFemale = HEROINE.AKIKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.HERO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.RITSUKO;
            GlobalData.PlayData.lastSelectMap = 2;
            GlobalData.PlayData.lastSelectTimeZone = 1;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("13");
            IsEvent = true;
        }
        else if (sceneInMsg == "YukikoFirstH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FIRST_YUKIKO;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.YUKIKO).state = Personality.STATE.FIRST;
            GlobalData.PlayData.lastSelectFemale = HEROINE.YUKIKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.HERO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.NONE;
            GlobalData.PlayData.lastSelectMap = 3;
            GlobalData.PlayData.lastSelectTimeZone = 2;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("14");
            IsEvent = true;
        }
        else if (sceneInMsg == "YukikoFlipFlopH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FLIPFLOP_YUKIKO;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.YUKIKO).state = Personality.STATE.FLIP_FLOP;
            GlobalData.PlayData.lastSelectFemale = HEROINE.YUKIKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.HERO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.NONE;
            GlobalData.PlayData.lastSelectMap = 0;
            GlobalData.PlayData.lastSelectTimeZone = 2;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("11");
            IsEvent = true;
        }
        else if (sceneInMsg == "AkikoFlipFlopH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FLIPFLOP_AKIKO;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.AKIKO).state = Personality.STATE.FLIP_FLOP;
            GlobalData.PlayData.lastSelectFemale = HEROINE.AKIKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.HERO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.YUKIKO;
            GlobalData.PlayData.lastSelectMap = 0;
            GlobalData.PlayData.lastSelectTimeZone = 2;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("11");
            IsEvent = true;
        }
        else if (sceneInMsg == "RitsukoFlipFlopH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FLIPFLOP_RITSUKO;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.RITSUKO).state = Personality.STATE.FLIP_FLOP;
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.AKIKO).state = Personality.STATE.FLOP;
            GlobalData.PlayData.lastSelectFemale = HEROINE.RITSUKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.HERO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.AKIKO;
            GlobalData.PlayData.lastSelectMap = 1;
            GlobalData.PlayData.lastSelectTimeZone = 2;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("11");
            IsEvent = true;
        }
        else if (sceneInMsg == "FinalYukikoH1")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.END_YUKIKO_1;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.YUKIKO).state = Personality.STATE.LAST_EVENT_YUKIKO_1;
            GlobalData.PlayData.lastSelectFemale = HEROINE.YUKIKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.HERO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.KOUICHI;
            GlobalData.PlayData.lastSelectMap = 3;
            GlobalData.PlayData.lastSelectTimeZone = 2;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("11");
            IsEvent = true;
        }
        else if (sceneInMsg == "FinalSistersH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.END_SISTERS;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.RITSUKO).state = Personality.STATE.LAST_EVENT_SISTERS;
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.AKIKO).state = Personality.STATE.LAST_EVENT_SISTERS;
            GlobalData.PlayData.lastSelectFemale = HEROINE.RITSUKO;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.AKIKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.KOUICHI;
            GlobalData.PlayData.lastSelectMap = 3;
            GlobalData.PlayData.lastSelectTimeZone = 2;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("15");
            IsEvent = true;
        }
        else if (sceneInMsg == "FinalYukikoH2")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.END_YUKIKO_2;
            }
            GlobalData.PlayData.GetHeroinePersonality(HEROINE.YUKIKO).state = Personality.STATE.LAST_EVENT_YUKIKO_2;
            GlobalData.PlayData.lastSelectFemale = HEROINE.YUKIKO;
            GlobalData.PlayData.lastSelectMale = MALE_ID.KOUICHI;
            GlobalData.PlayData.lastSelectVisitor = VISITOR.NONE;
            GlobalData.PlayData.lastSelectMap = 3;
            GlobalData.PlayData.lastSelectTimeZone = 2;
            gameCtrl.audioCtrl.BGM_LoadAndPlay("15");
            IsEvent = true;
        }
    }

    private static void SetupEventCharacters(string msg, H_Members members, H_Visitor visitor)
    {
        switch (msg)
        {
            case "AkikoFirstH":
                {
                    Female female3 = visitor.GetFemale();
                    female3.ChangeRestrict(true);
                    female3.personality.gagItem = GAG_ITEM.GUMTAPE;
                    female3.ChangeGagItem();
                    female3.transform.position = new Vector3(0.8f, 0.4f, 2.1f);
                    female3.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                    female3.body.Anime.Play("RitsukoFirst");
                    visitor.LockPos = true;
                    SetupEventCharacters_UnderWear(female3);
                    break;
                }
            case "YukikoFlipFlopH":
                SetupEventCharacters_UnderWear(members.GetFemale(0));
                break;
            case "AkikoFlipFlopH":
                {
                    Female female2 = visitor.GetFemale();
                    SetupEventCharacters_UnderWear(female2);
                    break;
                }
            case "RitsukoFlipFlopH":
                {
                    Female female = visitor.GetFemale();
                    SetupEventCharacters_UnderWear(female);
                    break;
                }
            case "FinalYukikoH1":
                {
                    SetupEventCharacters_Nude(members.GetFemale(0));
                    Human human = visitor.GetHuman();
                    human.ChangeRestrict(true);
                    human.transform.position = new Vector3(-0.18f, 0f, 4.3f);
                    human.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                    visitor.LockPos = true;
                    break;
                }
            case "FinalSistersH":
                SetupEventCharacters_Nude(members.GetFemale(0));
                SetupEventCharacters_Nude(visitor.GetFemale());
                members.GetMale(0).ChangeRestrict(true);
                break;
            case "FinalYukikoH2":
                SetupEventCharacters_Nude(members.GetFemale(0));
                members.GetMale(0).ChangeRestrict(true);
                break;
        }
    }

    private static void SetupEventCharacters_UnderWear(Female female)
    {
        female.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.HIDE);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.HIDE);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.BOTTOM, WEAR_SHOW.HIDE);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.BRA, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SHORTS, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMUPPER, WEAR_SHOW.HIDE);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMLOWER, WEAR_SHOW.HIDE);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPUPPER, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPLOWER, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_BOTTOM, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.GLOVE, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.PANST, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SOCKS, WEAR_SHOW.ALL);
        female.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, WEAR_SHOW.HIDE);
        female.CheckShow();
    }

    private static void SetupEventCharacters_Nude(Female female)
    {
        for (int i = 0; i < 14; i++)
        {
            female.wears.ChangeShow((WEAR_SHOW_TYPE)i, WEAR_SHOW.HIDE);
        }
        female.CheckShow();
    }

    private void LoadCameraDataList(string map_name)
    {
        cameraDatas.Clear();
        string assetName = "H_Camera_" + map_name;
        string assetBundleName = "h/h_camera";
        TextAsset text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", assetBundleName, assetName);
        ListDataLoader listDataLoader = new ListDataLoader('\t', StringSplitOptions.None);
        listDataLoader.Load_Text(text);
        for (int i = 0; i < listDataLoader.Y_Num; i++)
        {
            string text2 = listDataLoader.Get(0, i);
            if (text2.Length > 0)
            {
                IllusionCameraResetData value = LoadCameraData(listDataLoader, i);
                cameraDatas.Add(text2, value);
            }
        }
    }

    private IllusionCameraResetData LoadCameraData(ListDataLoader loader, int y)
    {
        IllusionCameraResetData illusionCameraResetData = new IllusionCameraResetData();
        illusionCameraResetData.pos.x = loader.GetFloat(1, y);
        illusionCameraResetData.pos.y = loader.GetFloat(2, y);
        illusionCameraResetData.pos.z = loader.GetFloat(3, y);
        illusionCameraResetData.eul.x = loader.GetFloat(4, y);
        illusionCameraResetData.eul.y = loader.GetFloat(5, y);
        illusionCameraResetData.eul.z = loader.GetFloat(6, y);
        illusionCameraResetData.dis = loader.GetFloat(7, y);
        return illusionCameraResetData;
    }

    public IllusionCameraResetData GetCameraReset(string id)
    {
        if (cameraDatas.ContainsKey(id))
        {
            return cameraDatas[id];
        }
        return null;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < voiceABC.Length; i++)
        {
            if (voiceABC[i] != null)
            {
                voiceABC[i].Close();
            }
        }
    }

    public void ChangeMap(int no)
    {
        if (GlobalData.PlayData.lastSelectMap != no)
        {
            GlobalData.PlayData.lastSelectMap = no;
            LoadMap(false);
        }
    }

    public void LoadMap(bool changeTimeOnly)
    {
        IllusionCameraResetData illusionCameraResetData = CalcRelativeCamera();
        if (map != null)
        {
            UnityEngine.Object.Destroy(map.gameObject);
            map = null;
        }
        if (mob != null)
        {
            UnityEngine.Object.Destroy(mob.gameObject);
            mob = null;
        }
        string text = "map/";
        string[] array = new string[10] { "bedroom", "ritsuko_room", "akiko_room", "living", "bathroom", "japanese", "poweder", "entrance", "toilet", "yard" };
        string[] array2 = new string[4] { "day", "evening", "night_light", "night_dark" };
        text = text + array[GlobalData.PlayData.lastSelectMap] + "_" + array2[GlobalData.PlayData.lastSelectTimeZone];
        AssetBundleController assetBundleController = new AssetBundleController();
        assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", text);
        string assetName = text.Substring(text.LastIndexOf("/") + 1);
        GameObject gameObject = assetBundleController.LoadAndInstantiate<GameObject>(assetName);
        map = gameObject.GetComponent<Map>();
        LightMapControl componentInChildren = map.GetComponentInChildren<LightMapControl>();
        componentInChildren.Apply();
        if (map.data.mob.Length > 0)
        {
            GameObject gameObject2 = assetBundleController.LoadAndInstantiate<GameObject>(map.data.mob);
            if (gameObject2 != null)
            {
                mob = gameObject2.GetComponent<MapMob>();
                mob.gameObject.SetActive(ConfigData.showMob);
            }
        }
        assetBundleController.Close();
        Resources.UnloadUnusedAssets();
        light.SetupLight(map.lightRoot, false);
        h_light.ChangeMap(array[GlobalData.PlayData.lastSelectMap]);
        bool flag = false;
        for (int i = 0; i < members.Count; i++)
        {
            bool flag2 = members[i].ChangeMap(map.data, changeTimeOnly);
            if (i == mainMembersNo)
            {
                flag = flag2;
            }
        }
        if (visitor != null)
        {
            visitor.Foot(map.data.foot);
            if (!changeTimeOnly)
            {
                int no;
                H_Pos h_pos;
                mainMembers.GetNowDataPosNo(out no, out h_pos);
                VisitorPos(h_pos);
            }
        }
        LoadCameraDataList(array[GlobalData.PlayData.lastSelectMap]);
        if (mainMembers == null)
        {
            return;
        }
        string id = string.Empty;
        if (mainMembers.StyleData != null)
        {
            id = mainMembers.StyleData.id;
        }
        else if (mainMembers.PoseData != null)
        {
            id = mainMembers.PoseData.id;
        }
        cameraReset = GetCameraReset(id);
        IllusionCameraResetData illusionCameraResetData2 = illusionCameraResetData;
        if (ConfigData.h_camReset_style && flag)
        {
            if (cameraReset != null)
            {
                illusionCameraResetData2 = cameraReset;
            }
        }
        else if (ConfigData.h_camReset_position && !changeTimeOnly && cameraReset != null)
        {
            illusionCameraResetData2 = cameraReset;
        }
        if (illusionCameraResetData2 != null)
        {
            illusionCameraResetData2.ResetCamera(camera, mainMembers.Transform);
        }
    }

    private H_Pos StartPose(int memberNo)
    {
        H_Members memberSet = members[memberNo];
        Female female = memberSet.GetFemale(0);
        string[] array = new string[3] { "Ritsuko", "Akiko", "Yukiko" };
        string text = array[(int)GlobalData.PlayData.lastSelectFemale];
        string startPose = string.Empty;
        H_Pos startPos = null;
        int posNo = -1;
        if (female.personality.state == Personality.STATE.FIRST)
        {
            startPose = text + "FirstH";
            H_PoseData pose = poseDatas[startPose];
            posNo = ((female.HeroineID == HEROINE.AKIKO) ? (memberSet.floorPosNo = 1) : 0);
            startPos = GetPosePosList(pose)[posNo];
        }
        else if (female.personality.state == Personality.STATE.FLIP_FLOP)
        {
            startPose = text + "FlipFlop";
            H_PoseData pose2 = poseDatas[startPose];
            startPos = GetPosePosList(pose2)[0];
            posNo = 0;
        }
        else if (female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_1)
        {
            startPose = "FinalYukiko1";
            H_PoseData pose3 = poseDatas[startPose];
            startPos = GetPosePosList(pose3)[0];
            posNo = 0;
        }
        else if (female.personality.state == Personality.STATE.LAST_EVENT_SISTERS)
        {
            startPose = "FinalSisters";
            posNo = (memberSet.chairPosNo = 2);
            H_PoseData pose4 = poseDatas[startPose];
            startPos = GetPosePosList(pose4)[posNo];
        }
        else if (female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_2)
        {
            startPose = "FinalYukiko2";
            posNo = (memberSet.floorPosNo = 1);
            H_PoseData pose5 = poseDatas[startPose];
            startPos = GetPosePosList(pose5)[posNo];
        }
        else
        {
            if (!StartPose_CheckExcite(memberSet, female, ref startPos, ref startPose))
            {
                int num = UnityEngine.Random.Range(0, 3);
                string[] array2 = new string[3] { "Lie", "Stand", "Chair" };
                HEROINE lastSelectFemale = GlobalData.PlayData.lastSelectFemale;
                string text2 = ((!GlobalData.PlayData.IsHeroineFloped(lastSelectFemale)) ? "Resist" : "Flip");
                startPose = text + array2[num] + text2;
            }
            if (poseDatas.ContainsKey(startPose))
            {
                H_PoseData data = poseDatas[startPose];
                StartPosePos(data, ref memberSet, ref startPos, ref posNo);
            }
            else
            {
                Debug.LogError("不明な開始ポーズ:" + startPose);
            }
        }
        memberSet.Transform.position = startPos.pos.pos;
        memberSet.Transform.rotation = Quaternion.Euler(0f, startPos.pos.yaw, 0f);
        charaMove.SetDef(memberSet.Transform.position, memberSet.Transform.rotation);
        StartPose(memberNo, startPose);
        SetH_Light(posNo);
        return startPos;
    }

    private bool StartPose_CheckExcite(H_Members memberSet, Female female, ref H_Pos startPos, ref string startPose)
    {
        if (female.personality.state != Personality.STATE.FLOP)
        {
            return false;
        }
        if (UnityEngine.Random.Range(0, 3) != 0)
        {
            return false;
        }
        int num = UnityEngine.Random.Range(0, 3);
        string[] array = new string[3] { "A", "B", "C" };
        if (num == 0 && female.personality.feelVagina)
        {
            int num2 = UnityEngine.Random.Range(0, 3);
            startPose = "Excite_Vagina" + array[num2];
            if (female.customParam.wear.isSwimwear)
            {
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMLOWER, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPLOWER, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_BOTTOM, WEAR_SHOW.HALF);
            }
            else
            {
                female.wears.ChangeShow(WEAR_SHOW_TYPE.BOTTOM, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.PANST, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SHORTS, WEAR_SHOW.HALF);
            }
            return true;
        }
        if (num == 1 && female.personality.feelAnus)
        {
            int num3 = UnityEngine.Random.Range(0, 3);
            startPose = "Excite_Anal" + array[num3];
            if (female.customParam.wear.isSwimwear)
            {
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMLOWER, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPLOWER, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_BOTTOM, WEAR_SHOW.HALF);
            }
            else
            {
                female.wears.ChangeShow(WEAR_SHOW_TYPE.BOTTOM, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.PANST, WEAR_SHOW.HALF);
                female.wears.ChangeShow(WEAR_SHOW_TYPE.SHORTS, WEAR_SHOW.HIDE);
            }
            return true;
        }
        if (num == 2 && female.personality.likeFeratio)
        {
            int num4 = UnityEngine.Random.Range(0, 3);
            startPose = "Excite_Service" + array[num4];
            return true;
        }
        return false;
    }

    private void StartPosePos(H_PoseData data, ref H_Members memberSet, ref H_Pos startPos, ref int posNo)
    {
        if (data.position == H_PoseData.POSITION.FLOOR)
        {
            posNo = UnityEngine.Random.Range(0, map.data.h_pos.floor.Count);
            startPos = map.data.h_pos.floor[posNo];
            memberSet.floorPosNo = posNo;
        }
        else if (data.position == H_PoseData.POSITION.CHAIR)
        {
            posNo = UnityEngine.Random.Range(0, map.data.h_pos.chair.Count);
            startPos = map.data.h_pos.chair[posNo];
            memberSet.chairPosNo = posNo;
        }
        else if (data.position == H_PoseData.POSITION.WALL)
        {
            posNo = UnityEngine.Random.Range(0, map.data.h_pos.wall.Count);
            startPos = map.data.h_pos.wall[posNo];
            memberSet.wallPosNo = posNo;
        }
    }

    private List<H_Pos> GetPosePosList(H_PoseData pose)
    {
        if (pose.position == H_PoseData.POSITION.WALL)
        {
            return map.data.h_pos.wall;
        }
        if (pose.position == H_PoseData.POSITION.CHAIR)
        {
            return map.data.h_pos.chair;
        }
        return map.data.h_pos.floor;
    }

    public void StartPose(int memberNo, string startPose)
    {
        H_Members h_Members = members[memberNo];
        H_PoseData h_PoseData = poseDatas[startPose];
        h_Members.StartPose(h_PoseData);
        if (memberNo == 0)
        {
            cameraReset = GetCameraReset(startPose);
            if (cameraReset == null)
            {
                cameraReset = h_PoseData.camera;
            }
            cameraReset.ResetCamera(camera, h_Members.Transform);
            if (h_PoseData.hasLight)
            {
                light.SetDirection(h_PoseData.lightDir);
            }
            int h_Light = 0;
            if (h_PoseData.position == H_PoseData.POSITION.FLOOR)
            {
                h_Light = h_Members.floorPosNo;
            }
            else if (h_PoseData.position == H_PoseData.POSITION.WALL)
            {
                h_Light = h_Members.wallPosNo;
            }
            else if (h_PoseData.position == H_PoseData.POSITION.CHAIR)
            {
                h_Light = h_Members.chairPosNo;
            }
            SetH_Light(h_Light);
        }
    }

    private void LoadStyles()
    {
        LoadStyles_Search("h/ha_*");
        LoadStyles_Search("h/hh_*");
        LoadStyles_Search("h/hs_*");
        LoadStyles_Search("h/hm_*");
        LoadStyles_Search("h/hw_*");
    }

    public void LoadStyles_Search(string search)
    {
        string text = string.Empty;
        int num = search.LastIndexOf("/");
        if (num != -1)
        {
            text = search.Substring(0, num);
            search = search.Remove(0, num + 1);
        }
        string path = Application.persistentDataPath + "/abdata" + "/" + text;
        string[] files = Directory.GetFiles(path, search, SearchOption.TopDirectoryOnly);
        string[] array = files;
        foreach (string path2 in array)
        {
            string extension = Path.GetExtension(path2);
            if (extension.Length == 0)
            {
                string text2 = Path.GetFileNameWithoutExtension(path2);
                if (text.Length > 0)
                {
                    text2 = text + "/" + text2;
                }
                LoadStyle(text2);
            }
        }
    }

    private void LoadStyle(string assetBundle)
    {
        H_StyleData h_StyleData = new H_StyleData(assetBundle);
        styleDatas.Add(h_StyleData.id, h_StyleData);
    }

    private void LoadPoses(AssetBundleController acb)
    {
        TextAsset text = acb.LoadAsset<TextAsset>("H_IN_Data");
        TagText tagText = new TagText(text);
        foreach (TagText.Element element in tagText.Elements)
        {
            H_PoseData h_PoseData = new H_PoseData(element);
            poseDatas.Add(h_PoseData.id, h_PoseData);
        }
    }

    private void LoadVisitorPoses(AssetBundleController acb)
    {
        TextAsset text = acb.LoadAsset<TextAsset>("H_Visitor_Data");
        TagText tagText = new TagText(text);
        foreach (TagText.Element element in tagText.Elements)
        {
            H_PoseData h_PoseData = new H_PoseData(element);
            visitorDatas.Add(h_PoseData.id, h_PoseData);
        }
    }

    private void Update()
    {
        if (mob != null)
        {
            mob.gameObject.SetActive(ConfigData.showMob);
        }
        bool enableProcess = true;
        if (IsOverlapMode())
        {
            enableProcess = false;
        }
        if (visitor != null)
        {
            visitor.Update();
        }
        pauseMenue.EnableConfig = !config.isActiveAndEnabled;
        foreach (H_Members member in members)
        {
            member.Update(enableProcess);
        }
        UpdateUI(enableProcess);
        UpdateShortCutKeys();
        femaleGage.value = mainMembers.FemaleGageVal;
        maleGage.value = mainMembers.MaleGageVal;
    }

    private void LateUpdate()
    {
        foreach (H_Members member in members)
        {
            member.LateUpdate();
        }
    }

    private void UpdateShortCutKeys()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (cameraReset != null)
            {
                cameraReset.ResetCamera(camera, mainMembers.Transform);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 facePos = mainMembers.GetFemale(0).FacePos;
            camera.SetFocus(facePos, true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Transform transform = Transform_Utility.FindTransform(mainMembers.GetFemale(0).body.AnimatedBoneRoot, "cf_J_Mune02_L");
            Transform transform2 = Transform_Utility.FindTransform(mainMembers.GetFemale(0).body.AnimatedBoneRoot, "cf_J_Mune02_R");
            Vector3 focus = (transform.position + transform2.position) * 0.5f;
            camera.SetFocus(focus, true);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Transform transform3 = Transform_Utility.FindTransform(mainMembers.GetFemale(0).body.AnimatedBoneRoot, "cf_J_Kokan");
            camera.SetFocus(transform3.position, true);
        }
    }

    private void UpdateUI(bool enableProcess)
    {
        uiCanvas.enabled = enableProcess && !base.GC.IsHideUI && !ExitStandBy;
        middleLeftCanvas.enabled = !config.isActiveAndEnabled && !base.GC.IsHideUI && !ExitStandBy;
        bool active = false;
        bool active2 = false;
        bool active3 = false;
        bool active4 = false;
        bool active5 = false;
        bool flag = false;
        bool active6 = mainMembers.StateMgr.NowStateID != H_STATE.START;
        padRoot.SetActive(active6);
        talkButton.gameObject.SetActive(active6);
        talkButton.interactable = mainMembers.StateMgr.NowStateID == H_STATE.LOOP;
        femaleGage.value = mainMembers.FemaleGageVal;
        maleGage.value = mainMembers.MaleGageVal;
        if (mainMembers.StateMgr.NowStateID == H_STATE.LOOP)
        {
            bool flag2 = mainMembers.StyleData.type == H_StyleData.TYPE.INSERT || mainMembers.StyleData.type == H_StyleData.TYPE.PETTING;
            bool flag3 = mainMembers.StyleData.type == H_StyleData.TYPE.INSERT || mainMembers.StyleData.type == H_StyleData.TYPE.SERVICE;
            active = flag3;
            active2 = flag3;
            if (mainMembers.StyleData.type == H_StyleData.TYPE.SERVICE && !mainMembers.StyleData.IsInMouth())
            {
                active = false;
            }
            if (mainMembers.FemaleGageVal >= femaleXTCEnableVal)
            {
                active3 = flag2;
                active4 = flag2 && flag3;
            }
        }
        if (mainMembers.StateMgr.NowStateID == H_STATE.IN_XTC_AFTER_WAIT)
        {
            if (mainMembers.StyleData.type == H_StyleData.TYPE.INSERT)
            {
                active5 = true;
            }
            else if (mainMembers.StyleData.type == H_StyleData.TYPE.SERVICE && mainMembers.StyleData.IsInMouth())
            {
                active5 = true;
            }
        }
        flag = mainMembers.StateMgr.NowStateID == H_STATE.SHOW_MOUTH_LIQUID;
        if (mainMembers.StyleData != null && ((uint)mainMembers.StyleData.detailFlag & 0x1000u) != 0)
        {
            active2 = false;
            active5 = false;
        }
        buttonInEja.gameObject.SetActive(active);
        buttonOutEja.gameObject.SetActive(active2);
        buttonXTC_F.gameObject.SetActive(active3);
        buttonXTC_W.gameObject.SetActive(active4);
        buttonExtract.gameObject.SetActive(active5);
        buttonVomit.gameObject.SetActive(flag);
        buttonDrink.gameObject.SetActive(flag);
        if (editMode != null)
        {
            editMode.ShowUI(!config.isActiveAndEnabled);
            hEditModeEndCanvas.enabled = !config.isActiveAndEnabled && !base.GC.IsHideUI;
        }
        editFemale.interactable = editChara != EDIT_CHARA.FEMALE;
        editMale.interactable = editChara != EDIT_CHARA.MALE && mainMembers.GetMale(0).gameObject.activeSelf;
        if (visitor != null)
        {
            editVisitor.interactable = editChara != EDIT_CHARA.VISITOR;
        }
        int count = mainMembers.GetMales().Count;
        editMaleMobA.gameObject.SetActive(count >= 2);
        editMaleMobA.interactable = editChara != EDIT_CHARA.MOB_A;
        editMaleMobB.gameObject.SetActive(count >= 3);
        editMaleMobB.interactable = editChara != EDIT_CHARA.MOB_B;
        editMaleMobC.gameObject.SetActive(count >= 4);
        editMaleMobC.interactable = editChara != EDIT_CHARA.MOB_C;
        buttonExit.interactable = ExitEnable;
        weaknessShowUI.SetActive(mainMembers.GetFemale(0).personality.weakness);
        weaknessRecoveryButton.gameObject.SetActive(GlobalData.PlayData.unlockWeaknessRecovery);
        bool flag4 = ui_StateCheckList[(int)mainMembers.StateMgr.NowStateID];
        bool flag5 = editChara >= EDIT_CHARA.MOB_A && editChara <= EDIT_CHARA.MOB_C;
        styleChangeUI.Interactable = flag4 && !flag5;
        if (!styleChangeUI.Interactable)
        {
            styleChangeUI.Close();
        }
        mapToggleButton.Interactable = !IsEvent && flag4 && !flag5;
        if (!mapToggleButton.Interactable)
        {
            mapToggleButton.ChangeValue(false, true);
        }
        swapButton.interactable = !IsEvent && flag4 && mainMembers.StateMgr.NowStateID != H_STATE.START;
        moveToggleButton.Interactable = !IsEvent;
    }

    public static void SetInput(H_Members members, H_InputBase input)
    {
        input.SetMembers(members);
        members.input = input;
    }

    public void OnInput(H_INPUT input)
    {
        foreach (H_Members member in members)
        {
            member.OnInput(input);
        }
    }

    public void Button_EjaIn()
    {
        OnInput(H_INPUT.EJA_IN);
    }

    public void Button_EjaOut()
    {
        OnInput(H_INPUT.EJA_OUT);
    }

    public void Button_XTC_F()
    {
        OnInput(H_INPUT.XTC_F);
    }

    public void Button_XTC_W()
    {
        OnInput(H_INPUT.XTC_W);
    }

    public void Button_Extract()
    {
        OnInput(H_INPUT.EXTRACT);
    }

    public void Button_Vomit()
    {
        OnInput(H_INPUT.VOMIT);
    }

    public void Button_Drink()
    {
        OnInput(H_INPUT.DRINK);
    }

    public void OpenConfig()
    {
        config.gameObject.SetActive(true);
        gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_open);
    }

    public void Button_Exit()
    {
        gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_choice);
        gameCtrl.CreateModalYesNoUI("Hシーンを終了しますか？", ExitState);
    }

    private void ExitState()
    {
        pauseMenue.enabled = false;
        ExitStandBy = true;
        mainMembers.StateMgr.ChangeState(H_STATE.EXIT, null);
    }

    public void Exit()
    {
        NextScene();
    }

    private void NextScene()
    {
        SceneEndParamaCalc();
        if (sceneInMsg == "RitsukoFirstH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FIRST_AKIKO;
            }
            gameCtrl.ChangeScene("ADVScene", "adv/adv_00_02,ADV_Script_00_02", 1f);
        }
        else if (sceneInMsg == "AkikoFirstH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FIRST_YUKIKO;
                gameCtrl.ChangeScene("ADVScene", "adv/adv_00_04,ADV_Script_00_04", 1f);
            }
            else
            {
                gameCtrl.ChangeScene("SelectScene", "MemoryEnd", 1f);
            }
        }
        else if (sceneInMsg == "YukikoFirstH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.RESIST;
            }
            gameCtrl.ChangeScene("ADVScene", "adv/adv_00_05,ADV_Script_00_05", 1f);
        }
        else if (sceneInMsg == "YukikoFlipFlopH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FLIPFLOP_AKIKO;
                gameCtrl.ChangeScene("ADVScene", "adv/adv_01_01,ADV_Script_01_01", 1f);
            }
            else
            {
                gameCtrl.ChangeScene("SelectScene", "MemoryEnd", 1f);
            }
        }
        else if (sceneInMsg == "AkikoFlipFlopH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FLIPFLOP_RITSUKO;
            }
            gameCtrl.ChangeScene("ADVScene", "adv/adv_01_02,ADV_Script_01_02", 1f);
        }
        else if (sceneInMsg == "RitsukoFlipFlopH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.FLOP;
            }
            gameCtrl.ChangeScene("ADVScene", "adv/adv_01_04,ADV_Script_01_04", 1f);
        }
        else if (sceneInMsg == "FinalYukikoH1")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.END_SISTERS;
                gameCtrl.ChangeScene("ADVScene", "adv/adv_02_01,ADV_Script_02_01", 1f);
            }
            else
            {
                gameCtrl.ChangeScene("SelectScene", "MemoryEnd", 1f);
            }
        }
        else if (sceneInMsg == "FinalSistersH")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.END_YUKIKO_2;
                gameCtrl.ChangeScene("ADVScene", "adv/adv_02_02,ADV_Script_02_02", 1f);
            }
            else
            {
                gameCtrl.ChangeScene("SelectScene", "MemoryEnd", 1f);
            }
        }
        else if (sceneInMsg == "FinalYukikoH2")
        {
            if (!GlobalData.isMemory)
            {
                GlobalData.PlayData.Progress = GamePlayData.PROGRESS.ALL_FREE;
                gameCtrl.ChangeScene("ADVScene", "adv/adv_02_03,ADV_Script_02_03", 1f);
            }
            else
            {
                gameCtrl.ChangeScene("SelectScene", "MemoryEnd", 1f);
            }
        }
        else
        {
            gameCtrl.ChangeScene("SelectScene", string.Empty, 1f);
        }
    }

    private void SceneEndParamaCalc()
    {
        for (HEROINE hEROINE = HEROINE.RITSUKO; hEROINE < HEROINE.NUM; hEROINE++)
        {
            Personality heroinePersonality = GlobalData.PlayData.GetHeroinePersonality(hEROINE);
            if (heroinePersonality.insertVagina)
            {
                heroinePersonality.continuousInsVagina++;
            }
            else
            {
                if (heroinePersonality.continuousInsVagina > 0)
                {
                    heroinePersonality.continuousInsVagina = 0;
                }
                heroinePersonality.continuousInsVagina--;
            }
            if (heroinePersonality.insertAnal)
            {
                heroinePersonality.continuousInsAnal++;
            }
            else
            {
                if (heroinePersonality.continuousInsAnal > 0)
                {
                    heroinePersonality.continuousInsAnal = 0;
                }
                heroinePersonality.continuousInsAnal--;
            }
            heroinePersonality.insertVagina = false;
            heroinePersonality.insertAnal = false;
        }
    }

    public void ChangeStyle(string id)
    {
        if (!styleDatas.ContainsKey(id))
        {
            return;
        }
        H_StyleData styleData = mainMembers.StyleData;
        IllusionCameraResetData illusionCameraResetData = CalcRelativeCamera();
        H_StyleData h_StyleData = styleDatas[id];
        List<Male> males = mainMembers.GetMales();
        foreach (Male item in males)
        {
            item.gameObject.SetActive(true);
        }
        mainMembers.ChangeStyle(h_StyleData);
        UpdateMaleShowUI();
        MixCtrl.ChangeStyle();
        ChangeStyle_ChangeState(styleData, h_StyleData);
        H_StyleData h_StyleData2 = styleDatas[id];
        cameraReset = GetCameraReset(id);
        if (cameraReset == null)
        {
            cameraReset = h_StyleData2.camera;
        }
        bool flag = styleData != null && styleData.id == h_StyleData2.id;
        IllusionCameraResetData illusionCameraResetData2 = illusionCameraResetData;
        if (((ConfigData.h_camReset_style && !flag) || illusionCameraResetData2 == null) && cameraReset != null)
        {
            illusionCameraResetData2 = cameraReset;
        }
        if (illusionCameraResetData2 != null)
        {
            illusionCameraResetData2.ResetCamera(camera, mainMembers.Transform);
        }
    }

    private void ChangeStyle_ChangeState(H_StyleData prev, H_StyleData next)
    {
        bool flag = prev != null && prev.type == next.type;
        bool flag2 = mainMembers.StateMgr.NowStateID == H_STATE.LOOP;
        bool flag3 = ConfigData.h_action_continue && flag && flag2;
        if (next.type == H_StyleData.TYPE.PETTING)
        {
            if (flag3)
            {
                mainMembers.StateMgr.ChangeState(H_STATE.LOOP, new H_State.BlendMessage(0f));
            }
            else
            {
                mainMembers.StateMgr.ChangeState(H_STATE.PRE_TOUCH_WAIT, new H_State.BlendMessage(0f));
            }
        }
        else if (next.type == H_StyleData.TYPE.INSERT)
        {
            int num = ((prev != null) ? (prev.detailFlag & next.detailFlag) : 0);
            bool flag4 = ((uint)num & 0x20u) != 0 || (num & 0x10) != 0;
            if (flag3 && flag4)
            {
                mainMembers.StateMgr.ChangeState(H_STATE.LOOP, new H_State.BlendMessage(0f));
            }
            else
            {
                mainMembers.StateMgr.ChangeState(H_STATE.PRE_INSERT_WAIT, new H_State.BlendMessage(0f));
            }
        }
        else if (next.type == H_StyleData.TYPE.SERVICE)
        {
            bool flag5 = prev != null && prev.IsInMouth() == next.IsInMouth();
            if (flag3 && flag5)
            {
                mainMembers.StateMgr.ChangeState(H_STATE.LOOP, new H_State.BlendMessage(0f));
            }
            else if (next.IsInMouth())
            {
                mainMembers.StateMgr.ChangeState(H_STATE.PRE_INSERT_WAIT, new H_State.BlendMessage(0f));
            }
            else
            {
                mainMembers.StateMgr.ChangeState(H_STATE.PRE_TOUCH_WAIT, new H_State.BlendMessage(0f));
            }
        }
    }

    public void ChangeStyleLight()
    {
        int no;
        H_Pos h_pos;
        mainMembers.GetNowDataPosNo(out no, out h_pos);
        SetH_Light(no);
    }

    private void Select_ChangeTimeZone(int no)
    {
        if (no >= 0)
        {
            SystemSE.Play(SystemSE.SE.CHOICE);
            ChangeTimeZone(no);
        }
    }

    private void ChangeTimeZone(int no)
    {
        if (GlobalData.PlayData.lastSelectTimeZone != no)
        {
            GlobalData.PlayData.lastSelectTimeZone = no;
            LoadMap(true);
        }
    }

    public H_Voice.Data Voice(Female female, H_VoiceLog log, H_Voice.TYPE type, H_Members members)
    {
        return voice.Voice(female, log, voiceABC[(int)female.HeroineID], type, members);
    }

    public H_ExpressionData.Data Expression(Female female, string voice, H_Parameter param)
    {
        return expression.ChangeExpression(female, voice, param);
    }

    public H_ExpressionData.Data Expression(Female female, H_Expression.TYPE type, H_Parameter param)
    {
        return expression.ChangeExpression(female, type, param);
    }

    public H_ExpressionData_Male.Data Expression(Male male, H_Expression_Male.TYPE type, H_Parameter param)
    {
        return expression_M.ChangeExpression(male, type, param);
    }

    public bool VisitorVoiceExpression(H_VisitorVoice.TYPE type)
    {
        if (visitor != null)
        {
            Female female = visitor.GetFemale();
            if (female != null)
            {
                return visitor.VoiceExpression(type, voiceABC[(int)female.HeroineID], mainMembers);
            }
        }
        return false;
    }

    public void SetGageLock_F(bool flag)
    {
        if (flag)
        {
            gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_yes);
        }
        else
        {
            gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_no);
        }
    }

    public void SetGageLock_M(bool flag)
    {
        if (flag)
        {
            gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_yes);
        }
        else
        {
            gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_no);
        }
    }

    public void OnClickTalk()
    {
        mainMembers.VoiceExpression(H_Voice.TYPE.ACT_TALK);
        foreach (Female female in mainMembers.GetFemales())
        {
            female.personality.AddIndecentLanguage();
        }
    }

    public void CharaCustom_Female()
    {
        if (editMode != null)
        {
            string text = "カスタムを切り替える前に変更を適用しますか？";
            string[] choices = new string[3] { "変更を適用して切り替え", "変更を戻して切り替え", "カスタムに戻る" };
            Action[] acts = new Action[3]
            {
                delegate
                {
                    ToCharaCustom_Female(true);
                },
                delegate
                {
                    ToCharaCustom_Female(false);
                },
                null
            };
            gameCtrl.CreateModalChoices(text, choices, acts);
        }
        else
        {
            ToCharaCustom_Female(false);
        }
    }

    private void ToCharaCustom_Female(bool destroyRecord)
    {
        ToCharaCustom(destroyRecord);
        editMode.Setup(mainMembers.GetFemale(0), null);
        editChara = EDIT_CHARA.FEMALE;
    }

    public void CharaCustom_Male(int no)
    {
        if (editMode != null)
        {
            string text = "カスタムを切り替える前に変更を適用しますか？";
            string[] choices = new string[3] { "変更を適用して切り替え", "変更を戻して切り替え", "カスタムに戻る" };
            Action[] acts = new Action[3]
            {
                delegate
                {
                    ToCharaCustom_Male(no, true);
                },
                delegate
                {
                    ToCharaCustom_Male(no, false);
                },
                null
            };
            gameCtrl.CreateModalChoices(text, choices, acts);
        }
        else
        {
            ToCharaCustom_Male(no, false);
        }
    }

    private void ToCharaCustom_Male(int no, bool destroyRecord)
    {
        ToCharaCustom(destroyRecord);
        editMode.Setup(mainMembers.GetMale(no), null);
        switch (no)
        {
            case 0:
                editChara = EDIT_CHARA.MALE;
                break;
            case 1:
                editChara = EDIT_CHARA.MOB_A;
                break;
            case 2:
                editChara = EDIT_CHARA.MOB_B;
                break;
            case 3:
                editChara = EDIT_CHARA.MOB_C;
                break;
        }
    }

    public void CharaCustom_Visitor()
    {
        if (editMode != null)
        {
            string text = "カスタムを切り替える前に変更を適用しますか？";
            string[] choices = new string[3] { "変更を適用して切り替え", "変更を戻して切り替え", "カスタムに戻る" };
            Action[] acts = new Action[3]
            {
                delegate
                {
                    ToCharaCustom_Visitor(true);
                },
                delegate
                {
                    ToCharaCustom_Visitor(false);
                },
                null
            };
            gameCtrl.CreateModalChoices(text, choices, acts);
        }
        else
        {
            ToCharaCustom_Visitor(false);
        }
    }

    private void ToCharaCustom_Visitor(bool destroyRecord)
    {
        ToCharaCustom(destroyRecord);
        editMode.Setup(visitor.GetHuman(), null);
        editChara = EDIT_CHARA.VISITOR;
    }

    private void ToCharaCustom(bool destroyRecord)
    {
        if (editMode != null)
        {
            if (destroyRecord)
            {
                editMode.RecordCustomData();
            }
            else
            {
                editMode.RevertCustomData();
            }
            editMode.gameObject.SetActive(false);
            hEditModeEndCanvas.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(editMode.gameObject);
            editMode = null;
            Resources.UnloadUnusedAssets();
        }
        hEditModeEndCanvas.gameObject.SetActive(true);
        editMode = UnityEngine.Object.Instantiate(editModeOriginal);
        editMode.moveableRoot = moveableRoot;
        editMode.colorUI = colorUI;
        editMode.gameObject.SetActive(true);
    }

    public void Button_CustomEnd()
    {
        gameCtrl.CreateModalChoices("カスタムを終了しますか？", new string[3] { "変更を適用して終了", "変更を戻して終了", "キャンセル" }, new Action[3] { CustomEnd_Record, CustomEnd_Revert, null });
    }

    private void CustomEnd_Record()
    {
        editMode.RecordCustomData();
        editMode.gameObject.SetActive(false);
        hEditModeEndCanvas.gameObject.SetActive(false);
        UnityEngine.Object.Destroy(editMode.gameObject);
        editMode = null;
        Resources.UnloadUnusedAssets();
        editChara = EDIT_CHARA.NONE;
    }

    private void CustomEnd_Revert()
    {
        editMode.RevertCustomData();
        editMode.gameObject.SetActive(false);
        hEditModeEndCanvas.gameObject.SetActive(false);
        UnityEngine.Object.Destroy(editMode.gameObject);
        editMode = null;
        Resources.UnloadUnusedAssets();
        editChara = EDIT_CHARA.NONE;
    }

    public void SetH_Light(int no)
    {
        string id = ((mainMembers.StyleData == null) ? mainMembers.PoseData.id : mainMembers.StyleData.id);
        Vector3 lightDir = h_light.Get(id, no);
        SetLightDir(lightDir);
    }

    public void SetLightDir(Vector3 euler)
    {
        light.SetDirection(euler);
    }

    public Animator CreateMapIK()
    {
        return UnityEngine.Object.Instantiate(mapIK_Original);
    }

    public void VisitorPos(H_Pos h_pos, int posNo = -1)
    {
        if (visitor != null && h_pos != null)
        {
            if (posNo >= 0)
            {
                visitor.SetPose(visitorDatas, h_pos, posNo);
            }
            else
            {
                visitor.RandomPose(visitorDatas, h_pos);
            }
        }
    }

    public void SwapVisitor()
    {
        ScreenFade.StartFade(ScreenFade.TYPE.OUT_IN, Color.black, 0.5f, 0f, SwapVisitor_EXE);
        SystemSE.Play(SystemSE.SE.YES);
    }

    public void SwapVisitor_EXE()
    {
        if (mainMembers.StyleData == null)
        {
            return;
        }
        Female female = visitor.GetHuman() as Female;
        Female female2 = mainMembers.GetFemale(0);
        mainMembers.param.swapVisitor++;
        string id = mainMembers.StyleData.id;
        if (mainMembers.GetFemales().Count == 1)
        {
            female2.transform.SetParent(null);
            visitor.SwapHuman(female2, voiceABC[(int)female2.HeroineID], mainMembers, false);
            female2.transform.position = female.transform.position;
            female2.transform.rotation = female.transform.rotation;
            mainMembers.SwapFemale(0, female);
            mainMembers.ClearIK();
            visitor.GetHuman().ik.ClearIK();
            GlobalData.PlayData.lastSelectVisitor = Female.HeroineIDtoVisitorID(female2.HeroineID);
            GlobalData.PlayData.lastSelectFemale = female.HeroineID;
            female.personality.ahe = false;
            female.personality.weakness = false;
            female.personality.spermInCntV = 0;
            female.personality.spermInCntA = 0;
            female2.personality.ahe = false;
            female2.personality.weakness = false;
            female2.personality.spermInCntV = 0;
            female2.personality.spermInCntA = 0;
            StyleChangeUI.UpdateList();
            if (GlobalData.PlayData.Progress != GamePlayData.PROGRESS.ALL_FREE)
            {
                H_StyleData styleData = mainMembers.StyleData;
                if (styleData.state == H_StyleData.STATE.WEAKNESS)
                {
                    string[] array = styleData.id.Split(new char[1] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    string text = ((!female.IsFloped()) ? "00" : "01");
                    id = array[0] + "_" + array[1] + "_" + text + "_" + array[3];
                }
            }
            ChangeStyle(id);
            visitor.ChangePose();
        }
        else
        {
            visitor.SwapHuman(female2, voiceABC[(int)female2.HeroineID], mainMembers, true);
            mainMembers.SwapFemale01();
            mainMembers.ClearIK();
            GlobalData.PlayData.lastSelectVisitor = Female.HeroineIDtoVisitorID(female2.HeroineID);
            GlobalData.PlayData.lastSelectFemale = female.HeroineID;
            StyleChangeUI.UpdateList();
            ChangeStyle(id);
        }
        SetMainFemaleName();
        editFemale.GetComponentInChildren<Text>().text = Female.HeroineName(female.HeroineID);
        editVisitor.GetComponentInChildren<Text>().text = Female.HeroineName(female2.HeroineID);
        gagUI.SetNameUI();
        badgeFeelVagina.SetActive(female.personality.feelVagina);
        badgeFeelAnus.SetActive(female.personality.feelAnus);
        badgeIndecentLanguage.SetActive(female.personality.indecentLanguage);
        badgeLikeFeratio.SetActive(female.personality.likeFeratio);
        badgeLikeSperm.SetActive(female.personality.likeSperm);
        wearAcceChangeUI.SwitchedFemaleVisitor();
        charaMove.SetNameUI();
        charaMove.ChangeTrans();
    }

    private void SetMainFemaleName()
    {
        mainFemaleName.text = Female.HeroineName(mainMembers.GetFemale(0).HeroineID);
    }

    public void NextMapPos_MainMember()
    {
        IllusionCameraResetData illusionCameraResetData = CalcRelativeCamera();
        if (ConfigData.h_camReset_position && cameraReset != null)
        {
            illusionCameraResetData = cameraReset;
        }
        H_Pos h_pos = mainMembers.NextPos();
        illusionCameraResetData.ResetCamera(camera, mainMembers.Transform);
        VisitorPos(h_pos);
    }

    public void PrevMapPos_MainMember()
    {
        IllusionCameraResetData illusionCameraResetData = CalcRelativeCamera();
        if (ConfigData.h_camReset_position && cameraReset != null)
        {
            illusionCameraResetData = cameraReset;
        }
        H_Pos h_pos = mainMembers.PrevPos();
        illusionCameraResetData.ResetCamera(camera, mainMembers.Transform);
        VisitorPos(h_pos);
    }

    public void NextMapPos_Visitor()
    {
        if (visitor != null)
        {
            visitor.NextPos();
        }
    }

    public void PrevMapPos_Visitor()
    {
        if (visitor != null)
        {
            visitor.PrevPos();
        }
    }

    public bool CheckEnableStyle(string id)
    {
        if (styleDatas.ContainsKey(id))
        {
            H_StyleData h_StyleData = styleDatas[id];
            if (h_StyleData.map.Length > 0 && map.name.IndexOf(h_StyleData.map) != 0)
            {
                return false;
            }
            if (!map.data.h_pos.CheckEnable(h_StyleData.position))
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void OnWeaknessRecovery()
    {
        List<Female> females = mainMembers.GetFemales();
        mainMembers.param.continuanceXTC_F = 0;
        for (int i = 0; i < females.Count; i++)
        {
            females[i].personality.ahe = false;
            females[i].personality.weakness = false;
        }
        StyleChangeUI.UpdateList();
        if (GlobalData.PlayData.Progress != GamePlayData.PROGRESS.ALL_FREE)
        {
            H_StyleData styleData = mainMembers.StyleData;
            if (styleData.state == H_StyleData.STATE.WEAKNESS)
            {
                H_StyleData h_StyleData = null;
                string[] array = styleData.id.Split(new char[1] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                string text = ((!females[0].IsFloped()) ? "00" : "01");
                string text2 = array[0] + "_" + array[1] + "_" + text + "_" + array[3];
                ChangeStyle(text2);
            }
        }
    }

    public bool IsOverlapMode()
    {
        return (editMode != null && editMode.isActiveAndEnabled) || config.isActiveAndEnabled;
    }

    public IllusionCameraResetData CalcRelativeCamera()
    {
        if (mainMembers == null)
        {
            return null;
        }
        IllusionCameraResetData illusionCameraResetData = new IllusionCameraResetData();
        Vector3 focus = camera.Focus;
        Vector3 rotation = camera.Rotation;
        float distance = camera.Distance;
        Transform transform = mainMembers.Transform;
        illusionCameraResetData.pos = transform.InverseTransformPoint(focus);
        illusionCameraResetData.eul.x = rotation.x;
        illusionCameraResetData.eul.y = rotation.y - transform.rotation.eulerAngles.y;
        illusionCameraResetData.eul.z = rotation.z;
        float num = 25f / ConfigData.defParse;
        illusionCameraResetData.dis = distance / num;
        return illusionCameraResetData;
    }
}
