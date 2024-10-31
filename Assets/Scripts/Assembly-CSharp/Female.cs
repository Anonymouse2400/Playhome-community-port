using System;
using System.Collections.Generic;
using Character;
using RootMotion.FinalIK;
using SEXY;
using UnityEngine;
using Utility;

public class Female : Human
{
    private const string HeadParentName = "cf_J_Head_s";

    private const string HairParentName = "cf_J_FaceUp_ty";

    private const string DynammicBoneBustL_Name = "muneL";

    private const string DynammicBoneBustR_Name = "muneR";

    private const string MosaicRootName = "N_mnpb";

    private const float SPERM_HIT_INTERVAL = 12f;

    [SerializeField]
    private NormalData bustNormal;

    [SerializeField]
    private UVNormalBlend nipNormal;

    [SerializeField]
    private UVData nipUV;

    private NormalLerpManager bustNormalMgr;

    private UVLerpManager nipUVMgr;

    private Renderer[] tears = new Renderer[3];

    private Material flushMat;

    private HEROINE heroineID = HEROINE.NUM;

    [SerializeField]
    private Material spermMat_face_lo;

    [SerializeField]
    private Material spermMat_face_hi;

    [SerializeField]
    private Material spermMat_bust_lo;

    [SerializeField]
    private Material spermMat_bust_hi;

    [SerializeField]
    private Material spermMat_back_lo;

    [SerializeField]
    private Material spermMat_back_hi;

    [SerializeField]
    private Material spermMat_crotch_lo;

    [SerializeField]
    private Material spermMat_crotch_hi;

    [SerializeField]
    private Material spermMat_hip_lo;

    [SerializeField]
    private Material spermMat_hip_hi;

    [SerializeField]
    private Material virginBlood;

    public ParticleSystem urineParticle;

    public ParticleSystem femaleEjaculationParticle;

    public ParticleSystem dripParticleVagina;

    public ParticleSystem dripParticleAnus;

    public ParticleSystem dripParticleMouth;

    private int[] sperms = new int[5];

    private float[] spermHitTimer = new float[5];

    private Transform mosaicRoot;

    private bool showMosaic = true;

    private Material instancedVirginBlood;

    public HEROINE HeroineID
    {
        get
        {
            return heroineID;
        }
        set
        {
            SetHeroineID(value);
        }
    }

    public Personality personality
    {
        get
        {
            return GlobalData.PlayData.GetHeroinePersonality(HeroineID);
        }
    }

    public float NipAdd { get; private set; }

    public float SkinSmoothAdd { get; private set; }

    public bool ShowVirginBlood { get; private set; }

    private void Awake()
    {
        base.sex = SEX.FEMALE;
        Transform parent = Transform_Utility.FindTransform(base.transform, "cf_J_Head_s");
        mosaicRoot = Transform_Utility.FindTransform(base.transform, "N_mnpb");
        DynamicBone_Ver02 bustDynamicBone_L = null;
        DynamicBone_Ver02 bustDynamicBone_R = null;
        DynamicBone_Ver02[] componentsInChildren = base.gameObject.GetComponentsInChildren<DynamicBone_Ver02>(true);
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            if (componentsInChildren[i].Comment == "muneL")
            {
                bustDynamicBone_L = componentsInChildren[i];
            }
            else if (componentsInChildren[i].Comment == "muneR")
            {
                bustDynamicBone_R = componentsInChildren[i];
            }
        }
        neckLook = base.transform.GetComponentInChildren<LookAtRotator>();
        base.customParam = new CustomParameter(SEX.FEMALE);
        SetDefParam();
        base.head = new Head(this, parent);
        base.body = new Body(this, bustDynamicBone_L, bustDynamicBone_R);
        base.hairs = new Hairs(this, null);
        base.wears = new Wears(base.sex, base.customParam.wear, base.transform, this);
        base.accessories = new Accessories(base.sex, base.transform);
        bustNormalMgr = new NormalLerpManager(base.transform, bustNormal);
        nipNormal.Setup();
        nipUVMgr = new UVLerpManager(nipNormal, nipUV);
        base.ik = new IK_Control(GetComponentInChildren<FullBodyBipedIK>(), null, null, null);
        SetTongueType(TONGUE_TYPE.FACE);
        NipAdd = 0f;
        heroineID = HEROINE.NUM;
    }

    public void SetHeroineID(HEROINE heroine)
    {
        heroineID = heroine;
    }

    private void Start()
    {
    }

    protected override void Update()
    {
        base.Update();
        Update_SpermHitTimer();
        Update_FlushAndTear(false);
        base.wears.UpdateLiquid(sperms);
    }

    private void Update_SpermHitTimer()
    {
        for (int i = 0; i < spermHitTimer.Length; i++)
        {
            if (spermHitTimer[i] > 0f)
            {
                spermHitTimer[i] -= Time.deltaTime;
            }
        }
    }

    private void Update_FlushAndTear(bool force)
    {
        float b = 0.5882353f;
        if (nowFlushRate != base.FlushRate || force)
        {
            if (force)
            {
                nowFlushRate = base.FlushRate;
            }
            else
            {
                nowFlushRate = Tween.LinearMove(nowFlushRate, base.FlushRate, Time.deltaTime);
            }
            Color color = flushMat.color;
            color.a = Mathf.Lerp(0f, b, nowFlushRate);
            flushMat.color = color;
        }
        if (nowTearsRate != base.TearsRate || force)
        {
            if (force)
            {
                nowTearsRate = base.TearsRate;
            }
            else
            {
                nowTearsRate = Tween.LinearMove(nowTearsRate, base.TearsRate, Time.deltaTime);
            }
            for (int i = 0; i < tears.Length; i++)
            {
                Color color2 = tears[i].material.color;
                color2.a = Mathf.Lerp(0f, b, nowTearsRate);
                tears[i].material.color = color2;
            }
        }
    }

    protected override float GetVolume()
    {
        if (heroineID == HEROINE.RITSUKO)
        {
            return ConfigData.VolumeVoice_Ritsuko();
        }
        if (heroineID == HEROINE.AKIKO)
        {
            return ConfigData.VolumeVoice_Akiko();
        }
        if (heroineID == HEROINE.YUKIKO)
        {
            return ConfigData.VolumeVoice_Yukiko();
        }
        return 0f;
    }

    protected override float GetVoicePitch()
    {
        if (heroineID == HEROINE.RITSUKO)
        {
            return ConfigData.PitchVoice_Ritsuko();
        }
        if (heroineID == HEROINE.AKIKO)
        {
            return ConfigData.PitchVoice_Akiko();
        }
        if (heroineID == HEROINE.YUKIKO)
        {
            return ConfigData.PitchVoice_Yukiko();
        }
        return 1f;
    }

    public override void Save(string file)
    {
        StartCoroutine(ToCharaPNG(file, "【PlayHome_Female】"));
    }

    public override void Save(string file, Texture2D tex)
    {
        ToCharaPNG(file, "【PlayHome_Female】", tex);
    }

    public override void SaveCoordinate(string file)
    {
        StartCoroutine(ToCoordinatePNG(file, "【PlayHome_FemaleCoordinate】"));
    }

    public override void SaveCoordinate(string file, Texture2D tex)
    {
        ToCoordinatePNG(file, "【PlayHome_FemaleCoordinate】", tex);
    }

    public override LOAD_MSG Load(string file, int filter = -1)
    {
        return Load(file, true, false, filter);
    }

    public override LOAD_MSG Load(TextAsset text, int filter = -1)
    {
        return Load(text, true, false, filter);
    }

    public override LOAD_MSG LoadCoordinate(string file, int filter = -1)
    {
        return LoadCoordinate(file, true, false, filter);
    }

    public override LOAD_MSG LoadCoordinate(TextAsset text, int filter = -1)
    {
        return LoadCoordinate(text, true, false, filter);
    }

    public override void Apply()
    {
        base.body.Load(base.gameObject, base.customParam.body);
        ChangeAreoraSize();
        base.head.Load(base.customParam.head);
        Transform hairsParent = Transform_Utility.FindTransform(base.head.Obj.transform, "cf_J_FaceUp_ty");
        base.hairs.SetHairsParent(hairsParent);
        base.hairs.Load(base.customParam.hair);
        for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
        {
            base.wears.WearInstantiate(wEAR_TYPE, base.body.SkinMaterial, base.body.CustomHighlightMat_Skin);
        }
        base.wears.CheckShow();
        BustUVNormal();
        for (int i = 0; i < 10; i++)
        {
            base.accessories.AccessoryInstantiate(base.customParam.acce, i, false, null);
        }
        Resources.UnloadUnusedAssets();
        SetupDynamicBones();
        base.lipSync = GetComponentInChildren<AnimeLipSync>();
        base.blink = GetComponentInChildren<AnimeParamBlink>();
        base.HeadPosTrans = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "aim");
        base.BrestPosTrans = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "cf_J_Spine03");
        base.CrotchTrans = Transform_Utility.FindTransform(base.body.AnimatedBoneRoot, "cf_J_Kokan");
        SetupExpression();
        ChangeGagItem();
    }

    public override void ApplyCoordinate()
    {
        for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
        {
            base.wears.WearInstantiate(wEAR_TYPE, base.body.SkinMaterial, base.body.CustomHighlightMat_Skin);
        }
        base.wears.CheckShow();
        BustUVNormal();
        for (int i = 0; i < 10; i++)
        {
            base.accessories.AccessoryInstantiate(base.customParam.acce, i, false, null);
        }
        Resources.UnloadUnusedAssets();
        SetupDynamicBones();
    }

    public override void ApplyHair()
    {
        Transform hairsParent = Transform_Utility.FindTransform(base.head.Obj.transform, "cf_J_FaceUp_ty");
        base.hairs.SetHairsParent(hairsParent);
        base.hairs.Load(base.customParam.hair);
    }

    public override void ChangeHead()
    {
        ChangeHead("cf_J_FaceUp_ty");
        SetupExpression();
    }

    public override void OnShapeApplied()
    {
        BustUVNormal();
    }

    private void BustUVNormal()
    {
        float num = base.customParam.body.shapeVals[CharDefine.cf_bodyshape_BustNo];
        float num2 = Mathf.Clamp01(num * 2f);
        bustNormalMgr.Lerp(num2);
        nipNormal.Rate = 1f - num2;
        base.wears.BustUVNormal(1f - num2);
    }

    public void ChangeAreoraSize()
    {
        nipUVMgr.Lerp(base.customParam.body.areolaSize);
    }

    public override void SetupDynamicBones()
    {
        DynamicBone[] componentsInChildren = base.hairs.HirsParent.GetComponentsInChildren<DynamicBone>(true);
        DynamicBone_Ver01[] componentsInChildren2 = base.hairs.HirsParent.GetComponentsInChildren<DynamicBone_Ver01>(true);
        DynamicBone_Ver02[] componentsInChildren3 = base.hairs.HirsParent.GetComponentsInChildren<DynamicBone_Ver02>(true);
        DynamicBoneCollider[] componentsInChildren4 = base.body.Obj.transform.GetComponentsInChildren<DynamicBoneCollider>(true);
        DynamicBone[] array = componentsInChildren;
        foreach (DynamicBone dynamicBone in array)
        {
            dynamicBone.m_Colliders.Clear();
            DynamicBoneCollider[] array2 = componentsInChildren4;
            foreach (DynamicBoneCollider item in array2)
            {
                dynamicBone.m_Colliders.Add(item);
            }
        }
        DynamicBone_Ver01[] array3 = componentsInChildren2;
        foreach (DynamicBone_Ver01 dynamicBone_Ver in array3)
        {
            dynamicBone_Ver.m_Colliders.Clear();
            DynamicBoneCollider[] array4 = componentsInChildren4;
            foreach (DynamicBoneCollider item2 in array4)
            {
                dynamicBone_Ver.m_Colliders.Add(item2);
            }
        }
        DynamicBone_Ver02[] array5 = componentsInChildren3;
        foreach (DynamicBone_Ver02 dynamicBone_Ver2 in array5)
        {
            dynamicBone_Ver2.Colliders.Clear();
            DynamicBoneCollider[] array6 = componentsInChildren4;
            foreach (DynamicBoneCollider item3 in array6)
            {
                dynamicBone_Ver2.Colliders.Add(item3);
            }
        }
    }

    public void SetupExpression()
    {
        eyeLook = base.head.Obj.GetComponentInChildren<LookAtRotator>();
        eyeLook.Change(eyeLookType, eyeLookTarget, eyeLookForce);
        Renderer[] componentsInChildren = base.head.Obj.GetComponentsInChildren<Renderer>(true);
        Renderer[] array = componentsInChildren;
        foreach (Renderer renderer in array)
        {
            if (renderer.name == "cf_O_namida01")
            {
                tears[0] = renderer;
            }
            else if (renderer.name == "cf_O_namida02")
            {
                tears[1] = renderer;
            }
            else if (renderer.name == "cf_O_namida03")
            {
                tears[2] = renderer;
            }
            for (int j = 0; j < renderer.sharedMaterials.Length; j++)
            {
                if (renderer.sharedMaterials[j].name.IndexOf("cf_M_f_hohoaka") == 0)
                {
                    flushMat = renderer.sharedMaterials[j];
                    break;
                }
            }
        }
        SetTear(base.TearsRate, true);
        SetFlush(base.FlushRate, true);
        ExpressionPlay(0, eyeState, 0f);
        ExpressionPlay(1, mouthState, 0f);
        ExpressionPlay(2, lipSyncState, 0f);
        OpenEye(eyeOpen);
        OpenMouth(mouthOpen);
    }

    public override void SetFlush(float rate, bool force = false)
    {
        base.FlushRate = rate;
        if (force)
        {
            Update_FlushAndTear(true);
        }
    }

    public override void SetTear(float rate, bool force = false)
    {
        base.TearsRate = rate;
        if (force)
        {
            Update_FlushAndTear(true);
        }
    }

    public override void CheckShow()
    {
        base.wears.CheckShow();
    }

    public void AddSperm(SPERM_POS pos)
    {
        if (sperms[(int)pos] < 2)
        {
            sperms[(int)pos]++;
            int lv = sperms[(int)pos];
            switch (pos)
            {
                case SPERM_POS.FACE:
                    ChangeSpermMaterial(base.head.Rend_skin, lv, spermMat_face_lo, spermMat_face_hi);
                    break;
                case SPERM_POS.BUST:
                    ChangeSpermMaterial(base.body.Rend_skin, lv, spermMat_bust_lo, spermMat_bust_hi);
                    base.wears.ChangeBodyMaterial(base.body.Rend_skin);
                    break;
                case SPERM_POS.BACK:
                    ChangeSpermMaterial(base.body.Rend_skin, lv, spermMat_back_lo, spermMat_back_hi);
                    base.wears.ChangeBodyMaterial(base.body.Rend_skin);
                    break;
                case SPERM_POS.CROTCH:
                    ChangeSpermMaterial(base.body.Rend_skin, lv, spermMat_crotch_lo, spermMat_crotch_hi);
                    base.wears.ChangeBodyMaterial(base.body.Rend_skin);
                    break;
                case SPERM_POS.HIP:
                    ChangeSpermMaterial(base.body.Rend_skin, lv, spermMat_hip_lo, spermMat_hip_hi);
                    base.wears.ChangeBodyMaterial(base.body.Rend_skin);
                    break;
            }
        }
    }

    private static void ChangeSpermMaterial(Renderer rend, int lv, Material lo, Material hi)
    {
        int num = -1;
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            if (rend.sharedMaterials[i].name == lo.name || rend.sharedMaterials[i].name == hi.name)
            {
                num = i;
                break;
            }
        }
        Material[] materials = rend.materials;
        if (num == -1)
        {
            Material addMat = ((lv != 1) ? hi : lo);
            MaterialUtility.AddSharedMaterials(rend, addMat);
        }
        else
        {
            Material mat = ((lv != 1) ? hi : lo);
            MaterialUtility.SwapMaterials(rend, num, mat);
        }
    }

    public void ClearSpermMaterials()
    {
        for (int i = 0; i < sperms.Length; i++)
        {
            sperms[i] = 0;
        }
        DeleteSpermMaterials(base.head.Rend_skin);
        DeleteSpermMaterials(base.body.Rend_skin);
        base.wears.ChangeBodyMaterial(base.body.Rend_skin);
        Resources.UnloadUnusedAssets();
    }

    private void DeleteSpermMaterials(Renderer rend)
    {
        string[] array = new string[10]
        {
            spermMat_face_lo.name,
            spermMat_face_hi.name,
            spermMat_bust_lo.name,
            spermMat_bust_hi.name,
            spermMat_back_lo.name,
            spermMat_back_hi.name,
            spermMat_crotch_lo.name,
            spermMat_crotch_hi.name,
            spermMat_hip_lo.name,
            spermMat_hip_hi.name
        };
        List<Material> list = new List<Material>();
        List<Material> list2 = new List<Material>();
        Material[] sharedMaterials = rend.sharedMaterials;
        int num = -1;
        for (int i = 0; i < sharedMaterials.Length; i++)
        {
            bool flag = false;
            for (int j = 0; j < array.Length; j++)
            {
                if ((sharedMaterials[i]).name.IndexOf(array[j]) == 0)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                list.Add(sharedMaterials[i]);
            }
            else
            {
                list2.Add(sharedMaterials[i]);
            }
        }
        if (sharedMaterials.Length != list.Count)
        {
            Material[] array2 = new Material[list.Count];
            for (int k = 0; k < list.Count; k++)
            {
                array2[k] = list[k];
            }
            rend.sharedMaterials = array2;
        }
    }

    public void SetVirginBlood(bool set)
    {
        ShowVirginBlood = set;
        Renderer rend_skin = base.body.Rend_skin;
        int num = -1;
        for (int i = 0; i < rend_skin.sharedMaterials.Length; i++)
        {
            if (rend_skin.sharedMaterials[i].name.IndexOf(this.virginBlood.name) == 0)
            {
                num = i;
                break;
            }
        }
        if (set)
        {
            if (num == -1)
            {
                MaterialUtility.AddSharedMaterials(rend_skin, virginBlood);
            }
        }
        else if (num != -1)
        {
            Material[] array = new Material[rend_skin.sharedMaterials.Length - 1];
            int num2 = 0;
            for (int j = 0; j < rend_skin.sharedMaterials.Length; j++)
            {
                if (j != num)
                {
                    array[num2] = rend_skin.sharedMaterials[j];
                    num2++;
                }
            }
            rend_skin.sharedMaterials = array;
            instancedVirginBlood = null;
        }
        base.wears.ChangeBodyMaterial(rend_skin);
    }

    public void ShowMosaic(bool show)
    {
        mosaicRoot.gameObject.SetActive(show);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody.gameObject.layer == 12)
        {
            Transform transform = col.contacts[0].thisCollider.transform;
            Vector3 v = Quaternion.Inverse(transform.rotation) * col.rigidbody.velocity;
            HitSperm(transform.name, v);
        }
    }

    public void HitSperm(string collisionName, Vector3 v)
    {
        SPERM_POS sPERM_POS;
        if (collisionName.IndexOf("head") != -1 || collisionName.IndexOf("neck") != -1)
        {
            sPERM_POS = SPERM_POS.FACE;
        }
        else if (collisionName.IndexOf("Mune") != -1 || collisionName.IndexOf("Spine03") != -1)
        {
            sPERM_POS = SPERM_POS.BUST;
        }
        else if (collisionName.IndexOf("Spine02") != -1)
        {
            sPERM_POS = SPERM_POS.BUST;
        }
        else
        {
            if (collisionName.IndexOf("Spine01") == -1 && collisionName.IndexOf("kosi") == -1)
            {
                return;
            }
            sPERM_POS = SPERM_POS.CROTCH;
        }
        if ((double)v.z > 0.0)
        {
            switch (sPERM_POS)
            {
                case SPERM_POS.BUST:
                    sPERM_POS = SPERM_POS.BACK;
                    break;
                case SPERM_POS.CROTCH:
                    sPERM_POS = SPERM_POS.HIP;
                    break;
            }
        }
        if (spermHitTimer[(int)sPERM_POS] <= 0f)
        {
            AddSperm(sPERM_POS);
        }
        spermHitTimer[(int)sPERM_POS] = 12f;
    }

    public override void Foot(MapData.FOOT foot)
    {
        WEAR_SHOW show = ((foot == MapData.FOOT.BARE) ? WEAR_SHOW.HIDE : WEAR_SHOW.ALL);
        WEAR_SHOW show2 = ((foot != MapData.FOOT.SHOES) ? WEAR_SHOW.HIDE : WEAR_SHOW.ALL);
        base.wears.ChangeShow(WEAR_SHOW_TYPE.SOCKS, show);
        base.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, show2);
        CheckShow();
    }

    public override void ChangeGagItem()
    {
        if (personality == null)
        {
            return;
        }
        GAG_ITEM gAG_ITEM = personality.gagItem;
        if (base.GagType != gAG_ITEM && gagItem != null)
        {
            DelAttachItem(gagItem.name);
            gagItem = null;
        }
        base.GagType = gAG_ITEM;
        switch (gAG_ITEM)
        {
            case GAG_ITEM.BALLGAG:
                {
                    if (gagItem == null)
                    {
                        GameObject obj2 = ResourceUtility.CreateInstance<GameObject>("mask_00");
                        gagItem = new HumanAttachItem("mask_00", obj2);
                        AddAttachItem(gagItem);
                    }
                    Vector3[] array2 = new Vector3[4]
                    {
                new Vector3(0f, -1.5849f, 0.024f),
                new Vector3(0f, -1.6008f, 0.024f),
                new Vector3(0f, -1.5849f, 0.024f),
                new Vector3(0f, -1.551f, 0.0241f)
                    };
                    Vector3[] array3 = new Vector3[4]
                    {
                new Vector3(0.98f, 0.99f, 0.98f),
                new Vector3(0.95f, 1f, 0.98f),
                new Vector3(0.98f, 0.99f, 0.98f),
                new Vector3(0.96f, 0.97f, 0.98f)
                    };
                    Transform parent2 = base.head.Anime.transform;
                    gagItem.obj.transform.SetParent(parent2, false);
                    gagItem.obj.transform.localPosition = array2[base.customParam.head.headID];
                    gagItem.obj.transform.localRotation = Quaternion.identity;
                    gagItem.obj.transform.localScale = array3[base.customParam.head.headID];
                    break;
                }
            case GAG_ITEM.GUMTAPE:
                {
                    if (gagItem == null)
                    {
                        GameObject obj = ResourceUtility.CreateInstance<GameObject>("mask_01");
                        gagItem = new HumanAttachItem("mask_01", obj);
                        AddAttachItem(gagItem);
                    }
                    float[] array = new float[4] { 0f, 0f, 0.002f, 0.003f };
                    float z = array[base.customParam.head.headID];
                    Transform parent = Transform_Utility.FindTransform(base.transform, "N_Mouth");
                    gagItem.obj.transform.SetParent(parent, false);
                    gagItem.obj.transform.localPosition = new Vector3(0f, 0f, z);
                    gagItem.obj.transform.localRotation = Quaternion.identity;
                    gagItem.obj.transform.localScale = Vector3.one;
                    break;
                }
        }
        ChangeShowGag(gagShow);
    }

    public override void ChangeShowGag(bool flag)
    {
        gagShow = flag;
        bool flag2 = false;
        if (gagItem != null && gagItem.obj != null)
        {
            gagItem.obj.gameObject.SetActive(gagShow);
            flag2 = gagShow;
        }
        if (base.lipSync != null)
        {
            base.lipSync.Hold = flag2;
        }
        if (base.head != null)
        {
            base.head.ResetMouth(flag2);
        }
        RePlayMouthExpression();
    }

    protected override string MouthExpressionCheck(string name)
    {
        if (base.GagType == GAG_ITEM.NONE || !gagShow)
        {
            if (name.Length == 0)
            {
                return "Mouth_Def";
            }
            return name;
        }
        if (base.GagType == GAG_ITEM.BALLGAG)
        {
            string[] array = new string[4] { "GagA1", "GagA2", "GagA3", "GagB" };
            return array[base.customParam.head.headID];
        }
        if (base.GagType == GAG_ITEM.GUMTAPE)
        {
            return "Mouth_Def";
        }
        return name;
    }

    protected override void ReAttachGag()
    {
        ChangeGagItem();
    }

    public static string HeroineName(HEROINE heroineID)
    {
        string[] array = new string[3] { "律子", "明子", "雪子" };
        string result = "- UNKNOWN -";
        if (heroineID >= HEROINE.RITSUKO && heroineID < HEROINE.NUM)
        {
            result = array[(int)heroineID];
        }
        return result;
    }

    public void SetNipAdd(float val)
    {
        NipAdd = val;
        int num = CharDefine.cf_bodyshapename.Length - 1;
        base.body.ShapeApply();
    }

    public override void BodyShapeInfoApply()
    {
        int num = base.customParam.body.shapeVals.Length - 1;
        for (int i = 0; i < num; i++)
        {
            base.body.Info.ChangeValue(i, base.customParam.body.shapeVals[i]);
        }
        base.body.Info.ChangeValue(num, base.customParam.body.shapeVals[num] + NipAdd);
        base.body.Info.Update();
    }

    public void SetSkinSmoothAdd(float value)
    {
        SkinSmoothAdd = value;
        UpdateSkinMaterial();
        base.wears.ChangeBodyMaterial(base.body.Rend_skin);
    }

    public override void UpdateSkinMaterial()
    {
        base.customParam.body.skinColor.SetToMaterial(base.body.SkinMaterial, SkinSmoothAdd);
        base.customParam.body.skinColor.SetToMaterial(base.head.SkinMaterial, SkinSmoothAdd);
    }

    public bool IsFloped()
    {
        return personality.IsFloped(heroineID);
    }

    public static VISITOR HeroineIDtoVisitorID(HEROINE heroineID)
    {
        return (VISITOR)heroineID;
    }

    public static HEROINE VisitorIDtoHeroineID(VISITOR visitorID)
    {
        if (IsVisitorHeroine(visitorID))
        {
            return (HEROINE)visitorID;
        }
        Debug.LogError("ヒロインへの変換不可");
        return HEROINE.NUM;
    }

    public static bool IsVisitorHeroine(VISITOR visitorID)
    {
        if (visitorID < VISITOR.RITSUKO || visitorID >= (VISITOR)3)
        {
            return false;
        }
        return true;
    }
}
