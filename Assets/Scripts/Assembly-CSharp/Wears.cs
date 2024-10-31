using System;
using Character;
using UnityEngine;
using UnityEngine.Rendering;

public class Wears
{
	private SEX sex;

	private WearParameter wearParam;

	private Transform baseBoneRoot;

	private Transform wearsRoot;

	private GameObject skinObj;

	private SkinnedMeshRenderer bodySkinMesh;

	private Human human;

	private string wear_meshRootName = string.Empty;

	private WearObj[] wearObjs = new WearObj[11];

	private WEAR_SHOW[] showModes = new WEAR_SHOW[14];

	private static readonly string name_baseBoneRoot_F = "p_cf_anim/cf_J_Root";

	private static readonly string name_baseBoneRoot_M = "p_cm_anim/cm_J_Root";

	private static readonly string name_baseBoneRoot_MannequimF = "cf_J_Root";

	private static readonly string name_baseBoneRoot_MannequimM = "cm_J_Root";

	private static readonly string name_skinObj_F = "cf_O_body_00";

	private static readonly string name_skinObj_M = "O_body";

	private static readonly string name_wear_meshRootName_F = "cf_N_O_root";

	private static readonly string name_wear_meshRootName_M = "cm_N_O_root";

	private static readonly string name_skinMaterial_F = "cf_m_body";

	private static readonly string name_skinMaterial_M = "cm_m_body";

	private static readonly string name_wearsRoot = "Wears";

	private static readonly string name_defMaterial = "Default-Material";

	public static readonly WEAR_SHOW_TYPE[] WeatToShowType = new WEAR_SHOW_TYPE[11]
	{
		WEAR_SHOW_TYPE.NUM,
		WEAR_SHOW_TYPE.BOTTOM,
		WEAR_SHOW_TYPE.BRA,
		WEAR_SHOW_TYPE.SHORTS,
		WEAR_SHOW_TYPE.NUM,
		WEAR_SHOW_TYPE.NUM,
		WEAR_SHOW_TYPE.SWIM_BOTTOM,
		WEAR_SHOW_TYPE.GLOVE,
		WEAR_SHOW_TYPE.PANST,
		WEAR_SHOW_TYPE.SOCKS,
		WEAR_SHOW_TYPE.SHOES
	};

	public static readonly WEAR_TYPE[] ShowToWearType = new WEAR_TYPE[14]
	{
		WEAR_TYPE.TOP,
		WEAR_TYPE.TOP,
		WEAR_TYPE.BOTTOM,
		WEAR_TYPE.BRA,
		WEAR_TYPE.SHORTS,
		WEAR_TYPE.SWIM,
		WEAR_TYPE.SWIM,
		WEAR_TYPE.SWIM_TOP,
		WEAR_TYPE.SWIM_TOP,
		WEAR_TYPE.SWIM_BOTTOM,
		WEAR_TYPE.GLOVE,
		WEAR_TYPE.PANST,
		WEAR_TYPE.SOCKS,
		WEAR_TYPE.SHOES
	};

	public static readonly string name_swimOptTop = "N_top_op1";

	public static readonly string name_swimOptBot = "N_bot_op1";

	public Wears(SEX sex, WearParameter wearParam, Transform charaRoot, Human human)
	{
		this.sex = sex;
		this.wearParam = wearParam;
		wearsRoot = charaRoot.Find(name_wearsRoot);
		this.human = human;
		switch (sex)
		{
		case SEX.MALE:
			baseBoneRoot = charaRoot.Find(name_baseBoneRoot_M).transform;
			skinObj = Transform_Utility.FindTransform(charaRoot, name_skinObj_M).gameObject;
			wear_meshRootName = name_wear_meshRootName_M;
			break;
		case SEX.FEMALE:
			baseBoneRoot = charaRoot.Find(name_baseBoneRoot_F).transform;
			skinObj = Transform_Utility.FindTransform(charaRoot, name_skinObj_F).gameObject;
			wear_meshRootName = name_wear_meshRootName_F;
			break;
		}
		bodySkinMesh = skinObj.GetComponent<SkinnedMeshRenderer>();
		for (int i = 0; i < showModes.Length; i++)
		{
			showModes[i] = WEAR_SHOW.ALL;
		}
	}

	public WearData GetWearData(WEAR_TYPE type)
	{
		if (wearParam.wears[(int)type] == null)
		{
			return null;
		}
		int id = wearParam.wears[(int)type].id;
		WearData wearData = null;
		return CustomDataManager.GetWearData(sex, type, id);
	}

	public WearObj GetWearObj(WEAR_TYPE type)
	{
		return wearObjs[(int)type];
	}

	public void WearInstantiate(WEAR_TYPE type, Material skinMaterial, Material customHighlightMat_Skin)
	{
		int num = (int)type;
		if (wearParam.wears[num] == null)
		{
			return;
		}
		DeleteWear(type);
		if (!IsEnableWear(wearParam.isSwimwear, type))
		{
			CheckBodyShow();
			return;
		}
		WearData wearData = GetWearData(type);
		if (wearData == null)
		{
			CheckBodyShow();
			return;
		}
		GameObject gameObject = AssetBundleLoader.LoadAndInstantiate<GameObject>(wearData.assetbundleDir, wearData.assetbundleName, wearData.prefab);
		if (gameObject == null)
		{
			CheckBodyShow();
			return;
		}
		wearData.isNew = false;
		if (!GlobalData.vr_event_item && wearData.special == ItemDataBase.SPECIAL.VR_EVENT)
		{
			GlobalData.vr_event_item = true;
		}
		gameObject.transform.SetParent(wearsRoot);
		wearObjs[num] = new WearObj(wearParam, type, gameObject, wearData);
		wearObjs[num].UpdateColorCustom();
		Transform transform = Transform_Utility.FindTransform(gameObject.transform, wear_meshRootName);
		if (transform != null)
		{
			AttachBoneWeight.Attach(baseBoneRoot.gameObject, transform.gameObject, true);
		}
		else
		{
			Debug.LogWarning(type.ToString() + " " + wear_meshRootName + "なし");
		}
		ReAttachDynamicBone(gameObject);
		if (CustomDataManager.WearLiquids.ContainsKey(wearData.liquid))
		{
			string assetBundleName = CustomDataManager.WearLiquids[wearData.liquid];
            //string text = GlobalData.assetBundlePath + "/wearliquid";
            string text = Application.persistentDataPath + "/abdata" + "/wearliquid";
            GameObject gameObject2 = AssetBundleLoader.LoadAndInstantiate<GameObject>(Application.persistentDataPath + "/abdata", assetBundleName, wearData.liquid);
			gameObject2.transform.SetParent(wearsRoot);
			wearObjs[num].liquid = new WearLiquidObj(gameObject2);
			transform = Transform_Utility.FindTransform(gameObject2.transform, wear_meshRootName);
			if (transform != null)
			{
				AttachBoneWeight.Attach(baseBoneRoot.gameObject, transform.gameObject, true);
			}
			else
			{
				Debug.LogWarning(type.ToString() + " " + wear_meshRootName + "なし");
			}
		}
		if (type == WEAR_TYPE.TOP)
		{
			bool flag = false;
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				if (renderer.sharedMaterial == null)
				{
					renderer.sharedMaterial = bodySkinMesh.sharedMaterial;
					renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
					flag = true;
				}
				else if (renderer.sharedMaterial.name == (bodySkinMesh.sharedMaterial).name)
				{
					renderer.sharedMaterial = bodySkinMesh.sharedMaterial;
					renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
					flag = true;
				}
				else if (renderer.sharedMaterial.name.IndexOf(name_defMaterial) == 0)
				{
					renderer.sharedMaterial = bodySkinMesh.sharedMaterial;
					renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
					flag = true;
				}
			}
			skinObj.SetActive(false);
			if (human != null)
			{
				human.body.ShowNip(wearData.nip);
			}
			ChangeBodyMaterial(bodySkinMesh);
		}
		UpdateShow(type);
		CheckBodyShow();
		Renderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<Renderer>(true);
		Renderer[] array2 = componentsInChildren2;
		foreach (Renderer renderer2 in array2)
		{
			renderer2.gameObject.layer = baseBoneRoot.gameObject.layer;
			renderer2.lightProbeUsage = LightProbeUsage.Off;
		}
	}

	public bool EnableColorCustom(WEAR_TYPE type)
	{
		if (wearObjs[(int)type] == null)
		{
			return false;
		}
		return wearObjs[(int)type].enableColorCustom;
	}

	public bool EnableSecondColorCustom(WEAR_TYPE type)
	{
		if (wearObjs[(int)type] == null)
		{
			return false;
		}
		return wearObjs[(int)type].hasSecondColor;
	}

	public bool EnableSecondSpecularCustom(WEAR_TYPE type)
	{
		if (wearObjs[(int)type] == null)
		{
			return false;
		}
		return wearObjs[(int)type].hasSecondSpecular;
	}

	public void UpdateColorCustom(WEAR_TYPE type)
	{
		if (wearObjs[(int)type] != null)
		{
			wearObjs[(int)type].UpdateColorCustom();
		}
	}

	public void DeleteWear(WEAR_TYPE type)
	{
		if (wearObjs[(int)type] != null)
		{
			if (wearObjs[(int)type].liquid != null)
			{
                UnityEngine.Object.Destroy(wearObjs[(int)type].liquid.root);
			}
            UnityEngine.Object.Destroy(wearObjs[(int)type].obj);
			wearObjs[(int)type] = null;
		}
	}

	public void ChangeSwimOption_Top(bool show = true)
	{
		WearObj wearObj = wearObjs[4];
		if (wearObj != null)
		{
			Transform transform = Transform_Utility.FindTransform(wearObj.obj.transform, name_swimOptTop);
			if (transform != null)
			{
				bool flag = show && GetShow(WEAR_SHOW_TYPE.SWIMUPPER, true) != WEAR_SHOW.HIDE;
				transform.gameObject.SetActive(wearParam.swimOptTop && flag);
			}
		}
	}

	public void ChangeSwimOption_Bottom(bool show = true)
	{
		WearObj wearObj = wearObjs[4];
		if (wearObj != null)
		{
			Transform transform = Transform_Utility.FindTransform(wearObj.obj.transform, name_swimOptBot);
			if (transform != null)
			{
				bool flag = show && GetShow(WEAR_SHOW_TYPE.SWIMLOWER, true) != WEAR_SHOW.HIDE;
				transform.gameObject.SetActive(wearParam.swimOptBtm && flag);
			}
		}
	}

	public void Release()
	{
		for (int i = 0; i < wearObjs.Length; i++)
		{
			if (wearObjs[i] != null)
			{
                UnityEngine.Object.Destroy(wearObjs[i].obj);
				wearObjs[i] = null;
			}
		}
	}

	public void CheckShow(bool show = true)
	{
		UpdateShow(WEAR_TYPE.TOP);
		UpdateShow(WEAR_TYPE.BOTTOM);
		UpdateShow(WEAR_TYPE.BRA);
		UpdateShow(WEAR_TYPE.SHORTS);
		UpdateShow(WEAR_TYPE.SWIM);
		UpdateShow(WEAR_TYPE.SWIM_TOP);
		UpdateShow(WEAR_TYPE.SWIM_BOTTOM);
		CheckBodyShow(show);
		ChangeSwimOption_Top(show);
		ChangeSwimOption_Bottom(show);
	}

	private void CheckBodyShow(bool show = true)
	{
		bool flag = wearParam.isSwimwear || wearObjs[0] == null || GetShow(WEAR_SHOW_TYPE.TOPUPPER, true) == WEAR_SHOW.HIDE;
		if (!show)
		{
			flag = false;
		}
		skinObj.SetActive(flag);
		if (sex == SEX.FEMALE)
		{
			bool show2 = true;
			if (!wearParam.isSwimwear)
			{
				int wearID = wearParam.GetWearID(WEAR_TYPE.TOP);
				WearData wearData_Female = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, wearID);
				bool flag2 = wearData_Female != null && !wearData_Female.nip && GetShow(WEAR_SHOW_TYPE.TOPUPPER, true) == WEAR_SHOW.ALL;
				bool flag3 = false;
				if (flag2 || flag3)
				{
					show2 = false;
				}
			}
			if (!show)
			{
				show2 = false;
			}
			if (human != null)
			{
				human.body.ShowNip(show2);
			}
			bool show3 = true;
			if (wearParam.isSwimwear)
			{
				int wearID2 = wearParam.GetWearID(WEAR_TYPE.SWIM);
				WearData wearData_Female2 = CustomDataManager.GetWearData_Female(WEAR_TYPE.SWIM, wearID2);
				if (wearID2 != 50 && wearData_Female2 != null && !wearData_Female2.underhair && GetShow(WEAR_SHOW_TYPE.SWIMLOWER, true) == WEAR_SHOW.ALL)
				{
					show3 = false;
				}
			}
			else
			{
				int wearID3 = wearParam.GetWearID(WEAR_TYPE.SHORTS);
				WearData wearData_Female3 = CustomDataManager.GetWearData_Female(WEAR_TYPE.SHORTS, wearID3);
				if (wearID3 != 0 && wearData_Female3 != null && GetShow(WEAR_SHOW_TYPE.SHORTS, true) == WEAR_SHOW.ALL)
				{
					show3 = false;
				}
			}
			int wearID4 = wearParam.GetWearID(WEAR_TYPE.PANST);
			WearData wearData_Female4 = CustomDataManager.GetWearData_Female(WEAR_TYPE.PANST, wearID4);
			if (wearData_Female4 != null && GetShow(WEAR_SHOW_TYPE.PANST, true) == WEAR_SHOW.ALL && !wearData_Female4.underhair)
			{
				show3 = false;
			}
			if (!show)
			{
				show3 = false;
			}
			if (human != null)
			{
				human.body.ShowUnderHair3D(show3);
			}
		}
		else if (human != null)
		{
			human.body.ShowUnderHair3D(flag);
		}
	}

	public void ChangeBodyMaterial(Renderer bodySkin)
	{
		SkinnedMeshRenderer[] componentsInChildren = wearsRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		string text = ((sex != 0) ? name_skinMaterial_M : name_skinMaterial_F);
		text += "_CustomMaterial";
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			bool flag = false;
			for (int j = 0; j < skinnedMeshRenderer.sharedMaterials.Length; j++)
			{
				if (skinnedMeshRenderer.sharedMaterials[j].name.IndexOf(text) == 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				skinnedMeshRenderer.sharedMaterials = bodySkin.sharedMaterials;
			}
		}
	}

	private void ReAttachDynamicBone(GameObject obj)
	{
		DynamicBone[] componentsInChildren = obj.GetComponentsInChildren<DynamicBone>(true);
		DynamicBoneCollider[] componentsInChildren2 = baseBoneRoot.GetComponentsInChildren<DynamicBoneCollider>(true);
		DynamicBone[] array = componentsInChildren;
		foreach (DynamicBone dynamicBone in array)
		{
			Transform transform = Transform_Utility.FindTransform(baseBoneRoot, dynamicBone.m_Root.name);
			if (transform != null)
			{
				dynamicBone.m_Root = transform;
			}
			else
			{
				Debug.LogError("ダイナミックボーン付け替えに失敗:" + dynamicBone.m_Root.name);
			}
			dynamicBone.m_Colliders.Clear();
			DynamicBoneCollider[] array2 = componentsInChildren2;
			foreach (DynamicBoneCollider item in array2)
			{
				dynamicBone.m_Colliders.Add(item);
			}
		}
	}

	public void ChangeShow(WEAR_SHOW_TYPE type, WEAR_SHOW show)
	{
		showModes[(int)type] = show;
		UpdateShow(type);
	}

	public void ChangeShow_StripOnly(WEAR_SHOW_TYPE type, WEAR_SHOW show)
	{
		if (showModes[(int)type] < show)
		{
			showModes[(int)type] = show;
			UpdateShow(type);
		}
	}

	public WEAR_SHOW GetShow(WEAR_SHOW_TYPE showType, bool checkPair)
	{
		WEAR_SHOW result = showModes[(int)showType];
		WEAR_TYPE wEAR_TYPE = ShowToWearType[(int)showType];
		if (!checkPair || wearObjs[(int)wEAR_TYPE] == null || !wearObjs[(int)wEAR_TYPE].HasLowerShow())
		{
			return result;
		}
		if (wEAR_TYPE == WEAR_TYPE.SWIM)
		{
			WearData wearData = GetWearData(WEAR_TYPE.SWIM);
			if (wearData != null && wearData.coordinates == 0)
			{
				return result;
			}
		}
		if (showType == WEAR_SHOW_TYPE.TOPUPPER && showModes[1] == WEAR_SHOW.HIDE)
		{
			result = WEAR_SHOW.HIDE;
		}
		if (showType == WEAR_SHOW_TYPE.TOPLOWER && showModes[0] == WEAR_SHOW.HIDE)
		{
			result = WEAR_SHOW.HIDE;
		}
		if (showType == WEAR_SHOW_TYPE.SWIMUPPER && showModes[6] == WEAR_SHOW.HIDE)
		{
			result = WEAR_SHOW.HIDE;
		}
		if (showType == WEAR_SHOW_TYPE.SWIMLOWER && showModes[5] == WEAR_SHOW.HIDE)
		{
			result = WEAR_SHOW.HIDE;
		}
		if (showType == WEAR_SHOW_TYPE.SWIM_TOPUPPER && showModes[8] == WEAR_SHOW.HIDE)
		{
			result = WEAR_SHOW.HIDE;
		}
		if (showType == WEAR_SHOW_TYPE.SWIM_TOPLOWER && showModes[7] == WEAR_SHOW.HIDE)
		{
			result = WEAR_SHOW.HIDE;
		}
		return result;
	}

	public static WEAR_SHOW_TYPE GetWearShowTypePair(WEAR_SHOW_TYPE showType)
	{
		switch (showType)
		{
		case WEAR_SHOW_TYPE.TOPUPPER:
			return WEAR_SHOW_TYPE.TOPLOWER;
		case WEAR_SHOW_TYPE.TOPLOWER:
			return WEAR_SHOW_TYPE.TOPUPPER;
		case WEAR_SHOW_TYPE.SWIMUPPER:
			return WEAR_SHOW_TYPE.SWIMLOWER;
		case WEAR_SHOW_TYPE.SWIMLOWER:
			return WEAR_SHOW_TYPE.SWIMUPPER;
		case WEAR_SHOW_TYPE.SWIM_TOPUPPER:
			return WEAR_SHOW_TYPE.SWIM_TOPLOWER;
		case WEAR_SHOW_TYPE.SWIM_TOPLOWER:
			return WEAR_SHOW_TYPE.SWIM_TOPUPPER;
		default:
			return WEAR_SHOW_TYPE.NUM;
		}
	}

	private void UpdateShow(WEAR_TYPE type)
	{
		switch (type)
		{
		case WEAR_TYPE.TOP:
			UpdateShow(WEAR_SHOW_TYPE.TOPUPPER);
			UpdateShow(WEAR_SHOW_TYPE.TOPLOWER);
			break;
		case WEAR_TYPE.SWIM_TOP:
			UpdateShow(WEAR_SHOW_TYPE.SWIM_TOPUPPER);
			UpdateShow(WEAR_SHOW_TYPE.SWIM_TOPLOWER);
			break;
		case WEAR_TYPE.SWIM:
			UpdateShow(WEAR_SHOW_TYPE.SWIMUPPER);
			UpdateShow(WEAR_SHOW_TYPE.SWIMLOWER);
			break;
		default:
			UpdateShow(WeatToShowType[(int)type]);
			break;
		}
	}

	private void UpdateShow(WEAR_SHOW_TYPE showType)
	{
		WEAR_TYPE wEAR_TYPE = ShowToWearType[(int)showType];
		int num = (int)wEAR_TYPE;
		if (wearObjs[num] == null)
		{
			return;
		}
		GameObject obj = wearObjs[num].obj;
		WEAR_SHOW show = GetShow(showType, true);
		bool flag = true;
		bool flag2 = false;
		if (wEAR_TYPE == WEAR_TYPE.SWIM)
		{
			WearData wearData = GetWearData(WEAR_TYPE.SWIM);
			if (wearData != null && wearData.coordinates == 0)
			{
				flag2 = true;
			}
		}
		if (!flag2)
		{
			flag = show != WEAR_SHOW.HIDE;
		}
		if (flag)
		{
			flag = IsEnableWear(wearParam.isSwimwear, wEAR_TYPE);
		}
		if (wEAR_TYPE == WEAR_TYPE.BRA && flag)
		{
			if (wearObjs[0] != null && GetShow(WEAR_SHOW_TYPE.TOPUPPER, true) == WEAR_SHOW.ALL)
			{
				flag = false;
			}
			WearData wearData2 = GetWearData(WEAR_TYPE.TOP);
			WearData wearData3 = GetWearData(WEAR_TYPE.BOTTOM);
			if (wearData2 != null && wearData2.braDisable)
			{
				flag = false;
			}
			if (wearData3 != null && wearData3.braDisable)
			{
				flag = false;
			}
		}
		if (wEAR_TYPE == WEAR_TYPE.SHORTS && flag)
		{
			WearData wearData4 = GetWearData(WEAR_TYPE.TOP);
			WearData wearData5 = GetWearData(WEAR_TYPE.BOTTOM);
			if (wearData4 != null && wearData4.shortsDisable)
			{
				flag = false;
			}
			if (wearData5 != null && wearData5.shortsDisable)
			{
				flag = false;
			}
		}
		if (wEAR_TYPE == WEAR_TYPE.BOTTOM && flag)
		{
			WearData wearData6 = GetWearData(WEAR_TYPE.TOP);
			if (wearData6 != null && wearData6.coordinates == 2)
			{
				flag = false;
			}
		}
		if (IsLower(showType))
		{
			wearObjs[num].ChangeShow_Lower(show);
			return;
		}
		wearObjs[num].obj.SetActive(flag);
		wearObjs[num].ChangeShow_Upper(show);
	}

	public static bool IsEnableWear(bool isSwim, WEAR_TYPE type)
	{
		if (type == WEAR_TYPE.GLOVE || type == WEAR_TYPE.PANST || type == WEAR_TYPE.SOCKS || type == WEAR_TYPE.SHOES)
		{
			return true;
		}
		if (isSwim)
		{
			return type == WEAR_TYPE.SWIM || type == WEAR_TYPE.SWIM_TOP || type == WEAR_TYPE.SWIM_BOTTOM;
		}
		return type == WEAR_TYPE.TOP || type == WEAR_TYPE.BOTTOM || type == WEAR_TYPE.BRA || type == WEAR_TYPE.SHORTS;
	}

	public void BustUVNormal(float rate)
	{
		for (int i = 0; i < wearObjs.Length; i++)
		{
			if (wearObjs[i] != null && wearObjs[i].uv_normal != null)
			{
				wearObjs[i].uv_normal.Rate = rate;
			}
		}
	}

	public bool IsEquiped(CustomParameter param, WEAR_TYPE type)
	{
		if (!IsEnableWear(wearParam.isSwimwear, type))
		{
			return false;
		}
		if (type == WEAR_TYPE.BRA && !CheckBraEnable(sex, param))
		{
			return false;
		}
		if (type == WEAR_TYPE.SHORTS && !CheckShortsEnable(sex, param))
		{
			return false;
		}
		if (type == WEAR_TYPE.BOTTOM && !CheckBottomsEnable(sex, param))
		{
			return false;
		}
		return wearObjs[(int)type] != null;
	}

	public bool IsEquiped(CustomParameter param, WEAR_SHOW_TYPE type)
	{
		WEAR_TYPE wEAR_TYPE = ShowToWearType[(int)type];
		if (!IsEquiped(param, wEAR_TYPE))
		{
			return false;
		}
		return wearObjs[(int)wEAR_TYPE].HasShow(type);
	}

	public static bool IsLower(WEAR_SHOW_TYPE showType)
	{
		return showType == WEAR_SHOW_TYPE.TOPLOWER || showType == WEAR_SHOW_TYPE.SWIMLOWER || showType == WEAR_SHOW_TYPE.SWIM_TOPLOWER;
	}

	public static void CheckBraShortsEnable(SEX sex, CustomParameter customParam, out bool braEnable, out bool shortsEnable)
	{
		braEnable = true;
		shortsEnable = true;
		if (sex != 0)
		{
			return;
		}
		WearData wearData_Female = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, customParam.wear.GetWearID(WEAR_TYPE.TOP));
		WearData wearData_Female2 = CustomDataManager.GetWearData_Female(WEAR_TYPE.BOTTOM, customParam.wear.GetWearID(WEAR_TYPE.BOTTOM));
		if (wearData_Female != null)
		{
			if (wearData_Female.braDisable)
			{
				braEnable = false;
			}
			if (wearData_Female.shortsDisable)
			{
				shortsEnable = false;
			}
		}
		if (wearData_Female2 != null)
		{
			if (wearData_Female2.braDisable)
			{
				braEnable = false;
			}
			if (wearData_Female2.shortsDisable)
			{
				shortsEnable = false;
			}
		}
	}

	public static bool CheckBraEnable(SEX sex, CustomParameter customParam)
	{
		bool result = true;
		if (sex == SEX.FEMALE)
		{
			WearData wearData_Female = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, customParam.wear.GetWearID(WEAR_TYPE.TOP));
			WearData wearData_Female2 = CustomDataManager.GetWearData_Female(WEAR_TYPE.BOTTOM, customParam.wear.GetWearID(WEAR_TYPE.BOTTOM));
			if (wearData_Female != null && wearData_Female.braDisable)
			{
				result = false;
			}
			if (wearData_Female2 != null && wearData_Female2.braDisable)
			{
				result = false;
			}
		}
		return result;
	}

	public static bool CheckShortsEnable(SEX sex, CustomParameter customParam)
	{
		bool result = true;
		if (sex == SEX.FEMALE)
		{
			WearData wearData_Female = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, customParam.wear.GetWearID(WEAR_TYPE.TOP));
			WearData wearData_Female2 = CustomDataManager.GetWearData_Female(WEAR_TYPE.BOTTOM, customParam.wear.GetWearID(WEAR_TYPE.BOTTOM));
			if (wearData_Female != null && wearData_Female.shortsDisable)
			{
				result = false;
			}
			if (wearData_Female2 != null && wearData_Female2.shortsDisable)
			{
				result = false;
			}
		}
		return result;
	}

	public static bool CheckBottomsEnable(SEX sex, CustomParameter customParam)
	{
		bool result = true;
		if (sex == SEX.FEMALE)
		{
			WearData wearData_Female = CustomDataManager.GetWearData_Female(WEAR_TYPE.TOP, customParam.wear.GetWearID(WEAR_TYPE.TOP));
			WearData wearData_Female2 = CustomDataManager.GetWearData_Female(WEAR_TYPE.BOTTOM, customParam.wear.GetWearID(WEAR_TYPE.BOTTOM));
			if (wearData_Female != null)
			{
				if (wearData_Female.coordinates == 2)
				{
					result = false;
				}
				if (wearData_Female2 != null && wearData_Female.coordinates == 1 && wearData_Female2.coordinates != 0)
				{
					result = false;
				}
			}
		}
		return result;
	}

	public int GetWearShowNum(WEAR_SHOW_TYPE showType)
	{
		WEAR_TYPE wEAR_TYPE = ShowToWearType[(int)showType];
		WearObj wearObj = wearObjs[(int)wEAR_TYPE];
		if (wearObj != null)
		{
			if (IsLower(showType))
			{
				return wearObj.ShowLowerNum;
			}
			return wearObj.ShowUpperNum;
		}
		return 0;
	}

	public void UpdateLiquid(int[] sperms)
	{
		for (int i = 0; i < wearObjs.Length; i++)
		{
			if (wearObjs[i] != null && wearObjs[i].liquid != null)
			{
				wearObjs[i].SetLiquidShow(sperms);
			}
		}
	}
}
