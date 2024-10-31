using System;
using Character;
using RootMotion.FinalIK;
using UnityEngine;

public class Male : Human
{
    private const string HeadParentName = "cm_J_Head_s";

    private const string HairParentName = "cm_J_FaceUp_ty";

    private const string SilhouetteName = "cm_body_silhouette";

    private const string TinRootName = "N_dan";

    private const string IK_Tin_Root = "cm_J_dan101_00";

    private const string IK_Tin_Tip = "cm_J_dan109_00";

    private const string IK_Mouth = "N_Mouth";

    private SkinnedMeshRenderer silhouetteRend;

    private MALE_SHOW maleShow;

    private MALE_ID maleID = MALE_ID.NUM;

    private GameObject tinRoot;

    [SerializeField]
    private int ejaculationNormalNum = 3;

    [SerializeField]
    private int ejaculationManylNum = 15;

    [SerializeField]
    private float ejaculationNormalTime = 0.5f;

    [SerializeField]
    private float ejaculationManylTime = 2.5f;

    [SerializeField]
    private RigidEmitter ejaculator;

    private bool tinWithWear = true;

    private bool wearShoes = true;

    public MALE_SHOW MaleShow
    {
        get
        {
            return maleShow;
        }
    }

    public MALE_ID MaleID
    {
        get
        {
            return maleID;
        }
        set
        {
            SetMaleID(value);
        }
    }

    public bool WearShoes
    {
        get
        {
            return wearShoes;
        }
    }

    private void Awake()
    {
        base.sex = SEX.MALE;
        Transform transform = Transform_Utility.FindTransform(base.transform, "cm_J_Head_s");
        base.customParam = new CustomParameter(SEX.MALE);
        base.SetDefParam();
        base.head = new Head(this, transform);
        base.body = new Body(this, null, null);
        base.hairs = new Hairs(this, null);
        base.wears = new Wears(base.sex, base.customParam.wear, base.transform, this);
        base.accessories = new Accessories(base.sex, base.transform);
        silhouetteRend = Transform_Utility.FindComponent<SkinnedMeshRenderer>(base.gameObject, "cm_body_silhouette");
        AttachBoneWeight.Attach(base.body.AnimatedBoneRoot.gameObject, silhouetteRend.gameObject, true);
        silhouetteRend.enabled = false;
        tinRoot = Transform_Utility.FindTransform(base.transform, "N_dan").gameObject;
        Transform transform2 = Transform_Utility.FindTransform(base.transform, "cm_J_dan101_00");
        Transform transform3 = Transform_Utility.FindTransform(base.transform, "cm_J_dan109_00");
        Transform transform4 = Transform_Utility.FindTransform(base.transform, "N_Mouth");
        base.ik = new IK_Control(GetComponentInChildren<FullBodyBipedIK>(), transform2, transform3, transform4);
        SetTongueType(TONGUE_TYPE.FACE);
        neckLook = base.transform.GetComponentInChildren<LookAtRotator>();
        maleID = MALE_ID.NUM;
    }

    public void SetMaleID(MALE_ID maleID)
    {
        this.maleID = maleID;
        maleShow = GlobalData.maleShows[(int)maleID];
    }

    private void Start()
    {
    }

    protected override void Update()
    {
        base.Update();
        if (silhouetteRend.enabled)
        {
            silhouetteRend.material.color = ConfigData.maleColor;
        }
    }

    protected override float GetVolume()
    {
        if (maleID == MALE_ID.HERO)
        {
            return ConfigData.VolumeVoice_Hero();
        }
        if (maleID == MALE_ID.KOUICHI)
        {
            return ConfigData.VolumeVoice_Kouichi();
        }
        if (maleID >= MALE_ID.MOB_A && maleID <= MALE_ID.MOB_C)
        {
            return ConfigData.VolumeVoice_Mob();
        }
        return 0f;
    }

    protected override float GetVoicePitch()
    {
        if (maleID == MALE_ID.HERO)
        {
            return ConfigData.PitchVoice_Hero();
        }
        if (maleID == MALE_ID.KOUICHI)
        {
            return ConfigData.PitchVoice_Kouichi();
        }
        return 1f;
    }

    public override void Save(string file)
    {
        StartCoroutine(ToCharaPNG(file, "【PlayHome_Male】"));
    }

    public override void Save(string file, Texture2D tex)
    {
        ToCharaPNG(file, "【PlayHome_Male】", tex);
    }

    public override void SaveCoordinate(string file)
    {
        StartCoroutine(file, "【PlayHome_MaleCoordinate】");
    }

    public override void SaveCoordinate(string file, Texture2D tex)
    {
        ToCoordinatePNG(file, "【PlayHome_MaleCoordinate】", tex);
    }

    public override LOAD_MSG Load(string file, int filter = -1)
    {
        return Load(file, false, true, filter);
    }

    public override LOAD_MSG Load(TextAsset text, int filter = -1)
    {
        return Load(text, false, true, filter);
    }

    public override LOAD_MSG LoadCoordinate(string file, int filter = -1)
    {
        return LoadCoordinate(file, false, true, filter);
    }

    public override LOAD_MSG LoadCoordinate(TextAsset text, int filter = -1)
    {
        return LoadCoordinate(text, false, true, filter);
    }

    public override void Apply()
    {
        base.body.Load(base.gameObject, base.customParam.body);
        base.head.Load(base.customParam.head);
        Transform transform = Transform_Utility.FindTransform(base.head.Obj.transform, "cm_J_FaceUp_ty");
        base.hairs.SetHairsParent(transform);
        base.hairs.Load(base.customParam.hair);
        for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
        {
            base.wears.WearInstantiate(wEAR_TYPE, base.body.SkinMaterial, base.body.CustomHighlightMat_Skin);
        }
        base.wears.CheckShow();
        for (int i = 0; i < 10; i++)
        {
            base.accessories.AccessoryInstantiate(base.customParam.acce, i, false, null);
        }
        base.lipSync = GetComponentInChildren<AnimeLipSync>();
        base.blink = GetComponentInChildren<AnimeParamBlink>();
        eyeLook = base.head.Obj.GetComponentInChildren<LookAtRotator>();
        base.HeadPosTrans = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "aim");
        base.BrestPosTrans = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "cm_J_Spine03");
        base.CrotchTrans = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "cm_J_Kokan");
        Resources.UnloadUnusedAssets();
        ChangeMaleShow(maleShow);
        SetupExpression();
        if (base.ik != null)
        {
            Transform transform2 = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "N_Mouth");
            base.ik.SetMouthTrans(transform2);
        }
    }

    public override void ApplyCoordinate()
    {
        for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
        {
            base.wears.WearInstantiate(wEAR_TYPE, base.body.SkinMaterial, base.body.CustomHighlightMat_Skin);
        }
        base.wears.CheckShow();
        for (int i = 0; i < 10; i++)
        {
            base.accessories.AccessoryInstantiate(base.customParam.acce, i, false, null);
        }
        Resources.UnloadUnusedAssets();
        ChangeMaleShow(maleShow);
    }

    public override void ApplyHair()
    {
        Transform hairsParent = Transform_Utility.FindTransform(base.head.Obj.transform, "cm_J_FaceUp_ty");
        base.hairs.SetHairsParent(hairsParent);
        base.hairs.Load(base.customParam.hair);
        ChangeMaleShow(maleShow);
    }

    public override void ChangeHead()
    {
        ChangeHead("cm_J_FaceUp_ty");
        ChangeMaleShow(maleShow);
        SetupExpression();
        if (base.ik != null)
        {
            Transform mouthTrans = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "N_Mouth");
            base.ik.SetMouthTrans(mouthTrans);
        }
    }

    public void ChangeMaleShow(MALE_SHOW show)
    {
        maleShow = show;
        switch (show)
        {
            case MALE_SHOW.CLOTHING:
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.ALL);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.ALL);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, WEAR_SHOW.ALL);
                CheckShow();
                break;
            case MALE_SHOW.NUDE:
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, WEAR_SHOW.HIDE);
                CheckShow();
                break;
            case MALE_SHOW.ONECOLOR:
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, WEAR_SHOW.HIDE);
                CheckShow();
                break;
            case MALE_SHOW.TIN:
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, WEAR_SHOW.HIDE);
                CheckShow();
                break;
            case MALE_SHOW.HIDE:
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.HIDE);
                base.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, WEAR_SHOW.HIDE);
                CheckShow();
                break;
        }
    }

    public override void CheckShow()
    {
        bool flag = maleShow == MALE_SHOW.CLOTHING || maleShow == MALE_SHOW.NUDE;
        base.head.ChangeShow(flag);
        base.hairs.ChangeShow(flag);
        base.wears.CheckShow(flag);
        base.accessories.ChangeAllShow(flag);
        silhouetteRend.enabled = maleShow == MALE_SHOW.ONECOLOR;
        if (flag)
        {
            SetTongueType(base.TongueType);
        }
        else
        {
            base.body.SetShowTongue(false);
            base.head.SetShowTongue(false);
        }
        bool active = true;
        if (maleShow == MALE_SHOW.HIDE)
        {
            active = false;
        }
        else if (maleShow == MALE_SHOW.CLOTHING && base.wears.GetShow(WEAR_SHOW_TYPE.TOPUPPER, true) == WEAR_SHOW.ALL && !tinWithWear)
        {
            active = false;
        }
        tinRoot.SetActive(active);
        base.body.ShowUnderHair(maleShow == MALE_SHOW.NUDE);
        WEAR_SHOW show = ((maleShow != 0 || !wearShoes) ? WEAR_SHOW.HIDE : WEAR_SHOW.ALL);
        base.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, show);
        bool flag2 = maleShow >= MALE_SHOW.CLOTHING && maleShow <= MALE_SHOW.ONECOLOR;
        for (int i = 0; i < restrictItems.Count; i++)
        {
            Renderer[] componentsInChildren = restrictItems[i].obj.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < componentsInChildren.Length; j++)
            {
                componentsInChildren[j].enabled = flag2;
            }
        }
    }

    public void SetupExpression()
    {
        eyeLook = base.head.Obj.GetComponentInChildren<LookAtRotator>();
        eyeLook.Change(eyeLookType, eyeLookTarget, eyeLookForce);
        ExpressionPlay(0, eyeState, 0f);
        ExpressionPlay(1, mouthState, 0f);
        OpenEye(eyeOpen);
        OpenMouth(mouthOpen);
    }

    public void Ejaculation(bool many)
    {
        ejaculator.emitTime = ((!many) ? ejaculationNormalTime : ejaculationManylTime);
        ejaculator.emitNum = ((!many) ? ejaculationNormalNum : ejaculationManylNum);
        ejaculator.StartEmit();
    }

    public void SetShowTinWithWear(bool flag)
    {
        tinWithWear = flag;
        CheckShow();
    }

    public void SetWearShoes(bool flag)
    {
        wearShoes = flag;
        CheckShow();
    }

    public override void Foot(MapData.FOOT foot)
    {
        SetWearShoes(foot == MapData.FOOT.SHOES);
    }

    public static string MaleName(MALE_ID maleID)
    {
        string[] array = new string[] { "主人公", "広一", "モブ男A", "モブ男B", "モブ男C" };
        string result = "- UNKNOWN -";
        if (maleID >= MALE_ID.HERO && maleID < MALE_ID.NUM)
        {
            result = array[(int)maleID];
        }
        return result;
    }
}
