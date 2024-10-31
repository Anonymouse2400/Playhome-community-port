using System;
using Character;
using UnityEngine;

public class Mannequin : MonoBehaviour
{
	public SEX sex;

	[SerializeField]
	private Material skinMaterial;

	private WearParameter wearParam;

	private AccessoryParameter acceParan;

	private const string HeadParentName_M = "cm_J_Head_s";

	private const string HeadParentName_F = "cf_J_Head_s";

	public Wears wears { get; protected set; }

	public Accessories accessories { get; protected set; }

	private void Awake()
	{
		string text = ((sex != 0) ? "cm_J_Head_s" : "cf_J_Head_s");
		Transform transform = Transform_Utility.FindTransform(base.transform, text);
		wearParam = new WearParameter(sex);
		acceParan = new AccessoryParameter(sex);
		wears = new Wears(sex, wearParam, base.transform, null);
		accessories = new Accessories(sex, base.transform);
	}

	public void FromHuman(Human human)
	{
		if (human.sex != sex)
		{
			Debug.LogError("人間とマネキンの性別が違う");
			return;
		}
		wearParam.Copy(human.customParam.wear);
		for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
		{
			wears.WearInstantiate(wEAR_TYPE, skinMaterial, null);
		}
		wears.CheckShow();
		acceParan.Copy(human.customParam.acce);
		for (int i = 0; i < 10; i++)
		{
			accessories.AccessoryInstantiate(acceParan, i, false, null);
		}
		Resources.UnloadUnusedAssets();
	}

	public void Strip()
	{
		wearParam.Init();
		for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
		{
			wears.DeleteWear(wEAR_TYPE);
		}
		wears.CheckShow();
		acceParan.Init();
		for (int i = 0; i < 10; i++)
		{
			accessories.DeleteAccessory(i);
		}
		Resources.UnloadUnusedAssets();
	}
}
