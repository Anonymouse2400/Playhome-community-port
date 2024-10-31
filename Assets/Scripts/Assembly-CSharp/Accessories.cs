using System;
using Character;
using UnityEngine;
using UnityEngine.Rendering;

public class Accessories
{
	private class AcceObj
	{
		public GameObject obj;

		public MaterialCustoms materialCustom;

		public bool enableColorCustom;

		public bool hasSecondColor;

		public bool hasSecondSpecular;

		private AccessoryParameter acceParam;

		private int slot;

		public AcceObj(AccessoryParameter acceParam, int slot, GameObject obj, AccessoryData data)
		{
			this.obj = obj;
			this.acceParam = acceParam;
			this.slot = slot;
			SetupMaterials(data);
		}

		private void SetupMaterials(AccessoryData data)
		{
			materialCustom = obj.GetComponentInChildren<MaterialCustoms>();
			enableColorCustom = materialCustom != null;
			hasSecondColor = false;
			hasSecondSpecular = false;
			if (enableColorCustom)
			{
				for (int i = 0; i < materialCustom.datas.Length; i++)
				{
					if (materialCustom.datas[i].param.name.IndexOf("Sub") >= 0)
					{
						hasSecondColor = true;
					}
					if (materialCustom.datas[i].param.name.IndexOf("SubMetallic") >= 0)
					{
						hasSecondColor = true;
						hasSecondSpecular = true;
						break;
					}
				}
			}
			UpdateColorCustom();
		}

		public void UpdateColorCustom()
		{
			if (enableColorCustom)
			{
				if (acceParam.slot[slot].color == null)
				{
					acceParam.slot[slot].color = new ColorParameter_PBR2(materialCustom);
				}
				else
				{
					acceParam.slot[slot].color.SetMaterialCustoms(materialCustom);
				}
			}
		}

		public void Destroy()
		{
            UnityEngine.Object.Destroy(obj);
		}

		public void SetShow(bool flag)
		{
			obj.SetActive(flag);
		}
	}

	private static readonly ACCESSORY_ATTACHTYPE[] AtttachToTypes = new ACCESSORY_ATTACHTYPE[30]
	{
		ACCESSORY_ATTACHTYPE.HEAD,
		ACCESSORY_ATTACHTYPE.GLASSES,
		ACCESSORY_ATTACHTYPE.EAR,
		ACCESSORY_ATTACHTYPE.EAR,
		ACCESSORY_ATTACHTYPE.MOUTH,
		ACCESSORY_ATTACHTYPE.NOSE,
		ACCESSORY_ATTACHTYPE.NECK,
		ACCESSORY_ATTACHTYPE.CHEST,
		ACCESSORY_ATTACHTYPE.WRIST,
		ACCESSORY_ATTACHTYPE.WRIST,
		ACCESSORY_ATTACHTYPE.ARM,
		ACCESSORY_ATTACHTYPE.ARM,
		ACCESSORY_ATTACHTYPE.FINGER,
		ACCESSORY_ATTACHTYPE.FINGER,
		ACCESSORY_ATTACHTYPE.FINGER,
		ACCESSORY_ATTACHTYPE.FINGER,
		ACCESSORY_ATTACHTYPE.FINGER,
		ACCESSORY_ATTACHTYPE.FINGER,
		ACCESSORY_ATTACHTYPE.LEG,
		ACCESSORY_ATTACHTYPE.LEG,
		ACCESSORY_ATTACHTYPE.ANKLE,
		ACCESSORY_ATTACHTYPE.ANKLE,
		ACCESSORY_ATTACHTYPE.NIP,
		ACCESSORY_ATTACHTYPE.NIP,
		ACCESSORY_ATTACHTYPE.WAIST,
		ACCESSORY_ATTACHTYPE.SHOULDER,
		ACCESSORY_ATTACHTYPE.SHOULDER,
		ACCESSORY_ATTACHTYPE.HAND,
		ACCESSORY_ATTACHTYPE.HAND,
		ACCESSORY_ATTACHTYPE.SKINNING
	};

	private static readonly ACCESSORY_ATTACH[] ReverseAtttach = new ACCESSORY_ATTACH[30]
	{
		ACCESSORY_ATTACH.AP_Head,
		ACCESSORY_ATTACH.AP_Megane,
		ACCESSORY_ATTACH.AP_Earring_R,
		ACCESSORY_ATTACH.AP_Earring_L,
		ACCESSORY_ATTACH.AP_Mouth,
		ACCESSORY_ATTACH.AP_Nose,
		ACCESSORY_ATTACH.AP_Neck,
		ACCESSORY_ATTACH.AP_Chest,
		ACCESSORY_ATTACH.AP_Wrist_R,
		ACCESSORY_ATTACH.AP_Wrist_L,
		ACCESSORY_ATTACH.AP_Arm_R,
		ACCESSORY_ATTACH.AP_Arm_L,
		ACCESSORY_ATTACH.AP_Index_R,
		ACCESSORY_ATTACH.AP_Index_L,
		ACCESSORY_ATTACH.AP_Middle_R,
		ACCESSORY_ATTACH.AP_Middle_L,
		ACCESSORY_ATTACH.AP_Ring_R,
		ACCESSORY_ATTACH.AP_Ring_L,
		ACCESSORY_ATTACH.AP_Leg_R,
		ACCESSORY_ATTACH.AP_Leg_L,
		ACCESSORY_ATTACH.AP_Ankle_R,
		ACCESSORY_ATTACH.AP_Ankle_L,
		ACCESSORY_ATTACH.AP_Tikubi_R,
		ACCESSORY_ATTACH.AP_Tikubi_L,
		ACCESSORY_ATTACH.AP_Waist,
		ACCESSORY_ATTACH.AP_Shoulder_R,
		ACCESSORY_ATTACH.AP_Shoulder_L,
		ACCESSORY_ATTACH.AP_Hand_R,
		ACCESSORY_ATTACH.AP_Hand_L,
		ACCESSORY_ATTACH.SKINNING
	};

	private SEX sex;

	private Transform baseBoneRoot;

	private Transform wearsRoot;

	private AcceObj[] acceObjs = new AcceObj[10];

	private bool[] showFlags = new bool[10];

	private static readonly string name_baseBoneRoot_F = "p_cf_anim/cf_J_Root";

	private static readonly string name_baseBoneRoot_M = "p_cm_anim/cm_J_Root";

	private static readonly string name_wears = "Wears";

	public Accessories(SEX sex, Transform charaRoot)
	{
		this.sex = sex;
		baseBoneRoot = charaRoot.Find((sex != 0) ? name_baseBoneRoot_M : name_baseBoneRoot_F);
		wearsRoot = charaRoot.Find(name_wears);
		for (int i = 0; i < showFlags.Length; i++)
		{
			showFlags[i] = true;
		}
	}

	private void Update()
	{
	}

	public bool EnableColorCustom(int slot)
	{
		if (acceObjs[slot] == null)
		{
			return false;
		}
		return acceObjs[slot].enableColorCustom;
	}

	public bool EnableSecondColorCustom(int slot)
	{
		if (acceObjs[slot] == null)
		{
			return false;
		}
		return acceObjs[slot].hasSecondColor;
	}

	public bool EnableSecondSpecularCustom(int slot)
	{
		if (acceObjs[slot] == null)
		{
			return false;
		}
		return acceObjs[slot].hasSecondSpecular;
	}

	public void UpdateColorCustom(int slot)
	{
		if (acceObjs[slot] != null)
		{
			acceObjs[slot].UpdateColorCustom();
		}
	}

	public void DeleteAccessory(int slot)
	{
		if (acceObjs[slot] != null)
		{
			acceObjs[slot].Destroy();
			acceObjs[slot] = null;
		}
	}

	public void AccessoryInstantiate(AccessoryParameter acceParam, int slot, bool fixAttachParent, AccessoryData prevData)
	{
		DeleteAccessory(slot);
		if (acceParam.slot[slot] == null)
		{
			return;
		}
		AccessoryCustom accessoryCustom = acceParam.slot[slot];
		if (accessoryCustom == null || accessoryCustom.type == ACCESSORY_TYPE.NONE)
		{
			return;
		}
		AccessoryData acceData = CustomDataManager.GetAcceData(accessoryCustom.type, accessoryCustom.id);
		if (acceData == null)
		{
			return;
		}
		string assetName = ((sex != 0) ? acceData.prefab_M : acceData.prefab_F);
		GameObject gameObject = AssetBundleLoader.LoadAndInstantiate<GameObject>(acceData.assetbundleDir, acceData.assetbundleName, assetName);
		if (!(gameObject == null))
		{
			GameObject gameObject2 = new GameObject("AcceParent");
			gameObject.transform.SetParent(gameObject2.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject = gameObject2;
			if (!GlobalData.vr_event_item && acceData.special == ItemDataBase.SPECIAL.VR_EVENT)
			{
				GlobalData.vr_event_item = true;
			}
			acceObjs[slot] = new AcceObj(acceParam, slot, gameObject, acceData);
			AttachAccessory(acceParam, slot, fixAttachParent, prevData);
			acceObjs[slot].SetShow(showFlags[slot]);
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				renderer.gameObject.layer = baseBoneRoot.gameObject.layer;
				renderer.lightProbeUsage = LightProbeUsage.Off;
			}
			SetupDynamicBone(gameObject);
		}
	}

	private void SetupDynamicBone(GameObject obj)
	{
		DynamicBone[] componentsInChildren = obj.GetComponentsInChildren<DynamicBone>(true);
		DynamicBoneCollider[] componentsInChildren2 = baseBoneRoot.GetComponentsInChildren<DynamicBoneCollider>(true);
		DynamicBone[] array = componentsInChildren;
		foreach (DynamicBone dynamicBone in array)
		{
			dynamicBone.m_Colliders.Clear();
			DynamicBoneCollider[] array2 = componentsInChildren2;
			foreach (DynamicBoneCollider item in array2)
			{
				dynamicBone.m_Colliders.Add(item);
			}
		}
	}

	public AccessoryData GetAccessoryData(AccessoryParameter acceParam, int slot)
	{
		if (acceParam.slot[slot] == null)
		{
			return null;
		}
		AccessoryCustom accessoryCustom = acceParam.slot[slot];
		if (accessoryCustom == null || accessoryCustom.type == ACCESSORY_TYPE.NONE)
		{
			return null;
		}
		return CustomDataManager.GetAcceData(accessoryCustom.type, accessoryCustom.id);
	}

	public void DetachHeadAccessory(AccessoryParameter acceParam)
	{
		for (int i = 0; i < 10; i++)
		{
			AccessoryCustom accessoryCustom = acceParam.slot[i];
			if (accessoryCustom != null && acceObjs[i] != null && accessoryCustom.CheckAttachInHead())
			{
				acceObjs[i].obj.transform.SetParent(null);
			}
		}
	}

	public void AttachHeadAccessory(AccessoryParameter acceParam)
	{
		for (int i = 0; i < 10; i++)
		{
			AttachAccessory(acceParam, i, false, null);
		}
	}

	public void AttachAccessory(AccessoryParameter acceParam, int slotNo, bool fixAttachParent, AccessoryData prevData)
	{
		AccessoryCustom accessoryCustom = acceParam.slot[slotNo];
		if (accessoryCustom == null || acceObjs[slotNo] == null)
		{
			return;
		}
		GameObject obj = acceObjs[slotNo].obj;
		AccessoryData acceData = CustomDataManager.GetAcceData(accessoryCustom.type, accessoryCustom.id);
		if (acceData == null)
		{
			return;
		}
		if (acceData.defAttach == ACCESSORY_ATTACH.SKINNING)
		{
			accessoryCustom.nowAttach = acceData.defAttach;
			obj.transform.SetParent(wearsRoot);
			SkinnedMeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			AttachBoneWeight.Attach(baseBoneRoot.gameObject, obj, true);
			return;
		}
		if (CheckAttachParentReset(accessoryCustom, prevData, acceData, fixAttachParent))
		{
			accessoryCustom.nowAttach = acceData.defAttach;
			accessoryCustom.ResetAttachPosition();
		}
		AttachParent(accessoryCustom, obj);
	}

	private void AttachParent(AccessoryCustom acce, GameObject obj)
	{
		Transform transform = null;
		transform = FindAccessoryParent(acce.nowAttach);
		obj.transform.SetParent(transform, false);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;
		if (transform == null)
		{
			Debug.LogWarning("アクセサリの親が見つかりませんでした");
		}
		Transform transform2 = Transform_Utility.FindTransform(obj.transform, "N_move");
		if (transform2 == null)
		{
			Debug.LogWarning("N_moveがない");
			transform2 = obj.transform;
		}
		transform2.localPosition = acce.addPos;
		transform2.localRotation = Quaternion.Euler(acce.addRot);
		transform2.localScale = acce.addScl;
	}

	public bool CheckAttachParentReset(AccessoryCustom acce, AccessoryData prevData, AccessoryData nextData, bool checkDifferentType)
	{
		if (acce.nowAttach == ACCESSORY_ATTACH.NONE || acce.nowAttach == ACCESSORY_ATTACH.SKINNING)
		{
			return true;
		}
		if (!checkDifferentType)
		{
			return false;
		}
		ACCESSORY_ATTACHTYPE aCCESSORY_ATTACHTYPE = AtttachToTypes[(int)nextData.defAttach];
		ACCESSORY_ATTACHTYPE aCCESSORY_ATTACHTYPE2 = AtttachToTypes[(int)acce.nowAttach];
		if (prevData != null && prevData.defAttach != nextData.defAttach)
		{
			return true;
		}
		if (aCCESSORY_ATTACHTYPE == aCCESSORY_ATTACHTYPE2)
		{
			return false;
		}
		return true;
	}

	public void UpdatePosition(AccessoryParameter acceParam, int slotNo)
	{
		AccessoryCustom accessoryCustom = acceParam.slot[slotNo];
		if (accessoryCustom != null && acceObjs[slotNo] != null)
		{
			LimitPosition(accessoryCustom);
			Transform moveTrans = GetMoveTrans(acceParam, slotNo);
			if (moveTrans != null)
			{
				moveTrans.localPosition = accessoryCustom.addPos;
				moveTrans.localRotation = Quaternion.Euler(accessoryCustom.addRot);
				moveTrans.localScale = accessoryCustom.addScl;
			}
		}
	}

	private void LimitPosition(AccessoryCustom acce)
	{
		acce.addPos.x = Mathf.Clamp(acce.addPos.x, -1f, 1f);
		acce.addPos.y = Mathf.Clamp(acce.addPos.y, -1f, 1f);
		acce.addPos.z = Mathf.Clamp(acce.addPos.z, -1f, 1f);
		acce.addRot.x = Mathf.DeltaAngle(0f, acce.addRot.x);
		acce.addRot.y = Mathf.DeltaAngle(0f, acce.addRot.y);
		acce.addRot.z = Mathf.DeltaAngle(0f, acce.addRot.z);
		acce.addScl.x = Mathf.Clamp(acce.addScl.x, 0.01f, 100f);
		acce.addScl.y = Mathf.Clamp(acce.addScl.y, 0.01f, 100f);
		acce.addScl.z = Mathf.Clamp(acce.addScl.z, 0.01f, 100f);
	}

	public Transform GetMoveTrans(AccessoryParameter acceParam, int slotNo)
	{
		AccessoryCustom accessoryCustom = acceParam.slot[slotNo];
		if (accessoryCustom != null && acceObjs[slotNo] != null)
		{
			GameObject obj = acceObjs[slotNo].obj;
			AccessoryData acceData = CustomDataManager.GetAcceData(accessoryCustom.type, accessoryCustom.id);
			if (acceData == null)
			{
				return null;
			}
			if (acceData.defAttach != ACCESSORY_ATTACH.SKINNING)
			{
				Transform transform = Transform_Utility.FindTransform(obj.transform, "N_move");
				if (transform == null)
				{
					Debug.LogWarning("N_moveがない");
					transform = obj.transform;
				}
				return transform;
			}
		}
		return null;
	}

	public void Release()
	{
		for (int i = 0; i < acceObjs.Length; i++)
		{
			if (acceObjs[i] != null)
			{
				acceObjs[i].Destroy();
			}
		}
	}

	private Transform FindAccessoryParent(ACCESSORY_ATTACH attach)
	{
		string attachBoneName = AccessoryData.GetAttachBoneName(attach);
		Transform transform = Transform_Utility.FindTransform(baseBoneRoot, attachBoneName);
		if (transform == null)
		{
			Debug.LogError("アクセサリの親がない : " + attachBoneName);
		}
		return transform;
	}

	public static ACCESSORY_ATTACH GetReverseAttach(ACCESSORY_ATTACH attach)
	{
		if (attach != ACCESSORY_ATTACH.NONE)
		{
			return ReverseAtttach[(int)attach];
		}
		return ACCESSORY_ATTACH.NONE;
	}

	public void SetShow(int slotNo, bool show)
	{
		showFlags[slotNo] = show;
		if (acceObjs[slotNo] != null)
		{
			acceObjs[slotNo].SetShow(show);
		}
	}

	public bool GetShow(int slotNo)
	{
		return showFlags[slotNo];
	}

	public void ChangeAllShow(bool show)
	{
		for (int i = 0; i < acceObjs.Length; i++)
		{
			if (acceObjs[i] != null)
			{
				acceObjs[i].SetShow(show);
			}
		}
	}

	public bool IsAttachedAccessory(int slot)
	{
		return acceObjs[slot] != null;
	}
}
