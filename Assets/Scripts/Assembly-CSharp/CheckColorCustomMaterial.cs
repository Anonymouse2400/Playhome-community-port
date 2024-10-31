using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class CheckColorCustomMaterial : MonoBehaviour
{
	private class Data
	{
		public string name;

		public string prefab;

		private List<string> materialNames = new List<string>();

		public Data(WearData wearData, Dictionary<string, int> dic)
		{
			name = wearData.name;
			prefab = wearData.prefab;
			foreach (string key in dic.Keys)
			{
				materialNames.Add(key);
			}
		}

		public Data(AccessoryData acceData, Dictionary<string, int> dic)
		{
			name = acceData.name;
			prefab = acceData.prefab_F;
			foreach (string key in dic.Keys)
			{
				materialNames.Add(key);
			}
		}

		public void Save(StreamWriter writer)
		{
			writer.Write(name);
			writer.Write("\t");
			writer.Write(materialNames.Count);
			foreach (string materialName in materialNames)
			{
				writer.Write("\t");
				writer.Write(materialName);
			}
			writer.WriteLine();
		}
	}

	private List<Data> datas = new List<Data>();

	private int now;

	private int dataNum;

	[SerializeField]
	private Slider slider;

	private void Start()
	{
	}

	private void Update()
	{
		float value = (float)now / (float)dataNum;
		slider.value = value;
	}

	public void Exe()
	{
		now = 0;
		StartCoroutine(Calc_Accessory());
	}

	private IEnumerator Calc_Wear()
	{
		dataNum = 0;
		for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
		{
			dataNum += CustomDataManager.GetWearDictionary_Female(wEAR_TYPE).Count;
		}
		for (WEAR_TYPE i = WEAR_TYPE.TOP; i < WEAR_TYPE.NUM; i++)
		{
			Dictionary<int, WearData> wear_datas = CustomDataManager.GetWearDictionary_Female(i);
			foreach (KeyValuePair<int, WearData> item in wear_datas)
			{
				now++;
				Calc_WearData(item.Value);
				yield return null;
			}
		}
		Debug.Log("完了");
	}

	private void Calc_WearData(WearData data)
	{
		if (data == null)
		{
			return;
		}
		GameObject gameObject = AssetBundleLoader.LoadAndInstantiate<GameObject>(data.assetbundleDir, data.assetbundleName, data.prefab);
		if (gameObject == null)
		{
			Debug.Log("読めなかった:" + data.name);
			return;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		CheckObj(dictionary, gameObject);
        UnityEngine.Object.Destroy(gameObject);
		if (dictionary.Count > 0)
		{
			datas.Add(new Data(data, dictionary));
		}
	}

	private IEnumerator Calc_Accessory()
	{
		dataNum = 0;
		for (ACCESSORY_TYPE aCCESSORY_TYPE = ACCESSORY_TYPE.HEAD; aCCESSORY_TYPE < ACCESSORY_TYPE.NUM; aCCESSORY_TYPE++)
		{
			dataNum += CustomDataManager.GetAccessoryDictionary(aCCESSORY_TYPE).Count;
		}
		for (ACCESSORY_TYPE i = ACCESSORY_TYPE.HEAD; i < ACCESSORY_TYPE.NUM; i++)
		{
			Dictionary<int, AccessoryData> acce_datas = CustomDataManager.GetAccessoryDictionary(i);
			foreach (KeyValuePair<int, AccessoryData> item in acce_datas)
			{
				now++;
				Calc_AccessoryData(item.Value);
				yield return null;
			}
		}
		Debug.Log("完了");
	}

	private void Calc_AccessoryData(AccessoryData data)
	{
		if (data == null)
		{
			return;
		}
		GameObject gameObject = AssetBundleLoader.LoadAndInstantiate<GameObject>(data.assetbundleDir, data.assetbundleName, data.prefab_F);
		GameObject gameObject2 = AssetBundleLoader.LoadAndInstantiate<GameObject>(data.assetbundleDir, data.assetbundleName, data.prefab_M);
		if (gameObject == null && gameObject2 == null)
		{
			Debug.Log("読めなかった:" + data.name);
			return;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		CheckObj(dictionary, gameObject);
		CheckObj(dictionary, gameObject2);
		if (gameObject != null)
		{
            UnityEngine.Object.Destroy(gameObject);
		}
		if (gameObject2 != null)
		{
            UnityEngine.Object.Destroy(gameObject2);
		}
		if (dictionary.Count > 0)
		{
			datas.Add(new Data(data, dictionary));
		}
	}

	private void CheckObj(Dictionary<string, int> dic, GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (!renderer.CompareTag("ObjColor"))
			{
				continue;
			}
			Material[] sharedMaterials = renderer.sharedMaterials;
			foreach (Material material in sharedMaterials)
			{
				if (dic.ContainsKey((material).name))
				{
					dic[(material).name]++;
				}
				else
				{
					dic.Add((material).name, 1);
				}
			}
		}
	}

	public void Save(StreamWriter writer)
	{
		foreach (Data data in datas)
		{
			data.Save(writer);
		}
	}
}
