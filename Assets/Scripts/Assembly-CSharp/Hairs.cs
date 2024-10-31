using System;
using Character;
using UnityEngine;
using UnityEngine.Rendering;

public class Hairs
{
	private Human human;

	private HairObj[] parts = new HairObj[3];

	private Transform hirsParent;

	public Transform HirsParent
	{
		get
		{
			return hirsParent;
		}
	}

	public bool isSetHair { get; private set; }

	public Hairs(Human human, Transform parent)
	{
		this.human = human;
		hirsParent = parent;
		isSetHair = false;
	}

	public void SetHairsParent(Transform parent)
	{
		hirsParent = parent;
	}

	public void Load(HairParameter param)
	{
		if (hirsParent == null)
		{
			Debug.LogError("髪の親がない");
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			if (parts[i] != null)
			{
				parts[i].obj.transform.SetParent(null);
                UnityEngine.Object.Destroy(parts[i].obj);
				parts[i] = null;
			}
		}
		if (human.sex == SEX.FEMALE)
		{
			HairData hair_Front = CustomDataManager.GetHair_Front(param.parts[1].ID);
			HairData hair_Side = CustomDataManager.GetHair_Side(param.parts[2].ID);
			BackHairData hair_Back = CustomDataManager.GetHair_Back(param.parts[0].ID);
			HairData[] array = new HairData[3] { hair_Back, hair_Front, hair_Side };
			if (hair_Back != null && hair_Back.isSet)
			{
				param.parts[1].ID = 0;
				param.parts[2].ID = 0;
				isSetHair = true;
			}
			else
			{
				isSetHair = false;
			}
			for (int j = 0; j < 3; j++)
			{
				HairData hair_Female = CustomDataManager.GetHair_Female((HAIR_TYPE)j, param.parts[j].ID);
				if (hair_Female != null && !(hair_Female.prefab == "-"))
				{
					GameObject obj = AssetBundleLoader.LoadAndInstantiate<GameObject>(hair_Female.assetbundleDir, hair_Female.assetbundleName, hair_Female.prefab);
					hair_Female.isNew = false;
					parts[j] = new HairObj(obj, hirsParent);
					SetLayer(j);
				}
			}
		}
		else if (human.sex == SEX.MALE)
		{
			int num = 0;
			BackHairData hair_Male = CustomDataManager.GetHair_Male(param.parts[num].ID);
			GameObject obj2 = AssetBundleLoader.LoadAndInstantiate<GameObject>(hair_Male.assetbundleDir, hair_Male.assetbundleName, hair_Male.prefab);
			hair_Male.isNew = false;
			parts[num] = new HairObj(obj2, hirsParent);
			SetLayer(num);
			isSetHair = true;
		}
		ChangeColor(param);
		human.SetupDynamicBones();
		Resources.UnloadUnusedAssets();
	}

	public void ChangeColor(HairParameter param)
	{
		for (int i = 0; i < 3; i++)
		{
			if (parts[i] != null)
			{
				parts[i].ChangeColor(param.parts[i]);
			}
		}
	}

	public void DetachHairs()
	{
		for (int i = 0; i < parts.Length; i++)
		{
			if (parts[i] != null)
			{
				parts[i].obj.transform.SetParent(null);
			}
		}
	}

	public void AttachHairs(Transform parent)
	{
		hirsParent = parent;
		for (int i = 0; i < parts.Length; i++)
		{
			if (parts[i] != null)
			{
				parts[i].SetParent(hirsParent);
			}
		}
	}

	public void ChangeShow(bool show)
	{
		for (int i = 0; i < parts.Length; i++)
		{
			if (parts[i] != null)
			{
				parts[i].obj.SetActive(show);
			}
		}
	}

	private void SetLayer(int partNo)
	{
		Renderer[] componentsInChildren = parts[partNo].obj.GetComponentsInChildren<Renderer>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.gameObject.layer = hirsParent.gameObject.layer;
			renderer.lightProbeUsage = LightProbeUsage.Off;
		}
	}

	public bool HasAcce(HAIR_TYPE part)
	{
		if (parts[(int)part] != null)
		{
			return parts[(int)part].hasAcce;
		}
		return false;
	}
}
