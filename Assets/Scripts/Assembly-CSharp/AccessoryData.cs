using System;
using Character;
using UnityEngine;

public class AccessoryData : ItemDataBase
{
	private static string[] keys = new string[30]
	{
		"AP_Head", "AP_Megane", "AP_Earring_L", "AP_Earring_R", "AP_Mouth", "AP_Nose", "AP_Neck", "AP_Chest", "AP_Wrist_L", "AP_Wrist_R",
		"AP_Arm_L", "AP_Arm_R", "AP_Index_L", "AP_Index_R", "AP_Middle_L", "AP_Middle_R", "AP_Ring_L", "AP_Ring_R", "AP_Leg_L", "AP_Leg_R",
		"AP_Ankle_L", "AP_Ankle_R", "AP_Tikubi_L", "AP_Tikubi_R", "AP_Waist", "AP_Shoulder_L", "AP_Shoulder_R", "AP_Hand_L", "AP_Hand_R", "0"
	};

	private static string[] names = new string[30]
	{
		"N_Head",
		"N_Megane",
		"N_Earring_L",
		"N_Earring_R",
		"N_Mouth",
		"N_Nose",
		"N_Neck",
		"N_Chest",
		"N_Wrist_L",
		"N_Wrist_R",
		"N_Arm_L",
		"N_Arm_R",
		"N_Index_L",
		"N_Index_R",
		"N_Middle_L",
		"N_Middle_R",
		"N_Ring_L",
		"N_Ring_R",
		"N_Leg_L",
		"N_Leg_R",
		"N_Ankle_L",
		"N_Ankle_R",
		"N_Tikubi_L",
		"N_Tikubi_R",
		"N_Waist",
		"N_Shoulder_L",
		"N_Shoulder_R",
		"N_Hand_L",
		"N_Hand_R",
		string.Empty
	};

	public string prefab_M;

	public string prefab_F;

	public ACCESSORY_ATTACH defAttach;

	public AccessoryData(int id, string name, string assetbundle, string prefab_M, string prefab_F, string parent, SPECIAL special, int order, bool isNew)
		: base(id, name, assetbundle, order, isNew)
	{
		this.prefab_M = prefab_M;
		this.prefab_F = prefab_F;
		defAttach = CheckAttach(parent);
		base.special = special;
	}

	public static ACCESSORY_ATTACH CheckAttach(string parentKey)
	{
		ACCESSORY_ATTACH aCCESSORY_ATTACH = ACCESSORY_ATTACH.NONE;
		if (parentKey.Length > 0)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (parentKey == keys[i])
				{
					aCCESSORY_ATTACH = (ACCESSORY_ATTACH)i;
					break;
				}
			}
			if (aCCESSORY_ATTACH == ACCESSORY_ATTACH.NONE)
			{
				Debug.LogError("不明な親キー名:" + parentKey);
			}
		}
		return aCCESSORY_ATTACH;
	}

	public static bool CheckAttachInHead(ACCESSORY_ATTACH check)
	{
		return check >= ACCESSORY_ATTACH.AP_Head && check <= ACCESSORY_ATTACH.AP_Nose;
	}

	public static string GetAttachBoneName(ACCESSORY_ATTACH check)
	{
		if (check == ACCESSORY_ATTACH.NONE)
		{
			return string.Empty;
		}
		return names[(int)check];
	}
}
