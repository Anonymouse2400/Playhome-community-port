using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class CustomDataSetupLoader<T_Data> where T_Data : class
{
	private Action<Dictionary<int, T_Data>, AssetBundleController, CustomDataListLoader> action;

	public CustomDataSetupLoader(Action<Dictionary<int, T_Data>, AssetBundleController, CustomDataListLoader> action)
	{
		this.action = action;
	}

	public void Setup_Search(Dictionary<int, T_Data> datas, string search)
	{
		string text = string.Empty;
		int num = search.LastIndexOf("/");
		if (num != -1)
		{
			text = search.Substring(0, num);
			search = search.Remove(0, num + 1);
		}
        //string path = GlobalData.assetBundlePath + "/" + text;
        string path = Application.persistentDataPath + "/abdata" + "/" + text;
        string[] files = Directory.GetFiles(path, search, SearchOption.TopDirectoryOnly);
		Array.Sort(files);
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
				AssetBundleController assetBundleController = new AssetBundleController();
				assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", text2);
				Setup(datas, assetBundleController);
				assetBundleController.Close();
			}
		}
	}

	public void Setup_Virtual(Dictionary<int, T_Data> datas, string assetBundleName)
	{
		AssetBundleController assetBundleController = new AssetBundleController(true);
		assetBundleController.OpenFromFile(string.Empty, assetBundleName);
		Setup(datas, assetBundleController);
		assetBundleController.Close();
	}

	private void Setup(Dictionary<int, T_Data> datas, AssetBundleController abc)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(abc.assetBundleName);
		TextAsset textAsset = abc.LoadAsset<TextAsset>(fileNameWithoutExtension + "_list");
		if (textAsset != null)
		{
			CustomDataListLoader customDataListLoader = new CustomDataListLoader();
			customDataListLoader.Load(textAsset);
			action(datas, abc, customDataListLoader);
		}
	}

	public static void SetupAction_Prefab(Dictionary<int, PrefabData> datas, AssetBundleController abc, CustomDataListLoader loader)
	{
		bool isNew = IsNewCheckFromABC(abc);
		int attributeNo = loader.GetAttributeNo("ID");
		int attributeNo2 = loader.GetAttributeNo("Name");
		int attributeNo3 = loader.GetAttributeNo("Prefab");
		int dataNum = loader.GetDataNum();
		for (int i = 0; i < dataNum; i++)
		{
			int num = int.Parse(loader.GetData(attributeNo, i));
			string data = loader.GetData(attributeNo2, i);
			string data2 = loader.GetData(attributeNo3, i);
			int count = datas.Count;
			PrefabData prefabData = new PrefabData(num, data, abc.assetBundleName, data2, count, isNew);
			if (datas.ContainsKey(prefabData.id))
			{
				Debug.LogWarning("ID重複:" + num + " " + data);
				datas[prefabData.id] = prefabData;
			}
			else
			{
				datas.Add(prefabData.id, prefabData);
			}
		}
	}

	public static void SetupAction_Hair(Dictionary<int, HairData> datas, AssetBundleController abc, CustomDataListLoader loader)
	{
		bool isNew = IsNewCheckFromABC(abc);
		int attributeNo = loader.GetAttributeNo("ID");
		int attributeNo2 = loader.GetAttributeNo("Name");
		int attributeNo3 = loader.GetAttributeNo("Prefab");
		int dataNum = loader.GetDataNum();
		for (int i = 0; i < dataNum; i++)
		{
			int id = int.Parse(loader.GetData(attributeNo, i));
			string data = loader.GetData(attributeNo2, i);
			string data2 = loader.GetData(attributeNo3, i);
			int count = datas.Count;
			HairData hairData = new HairData(id, data, abc.assetBundleName, data2, count, isNew);
			datas.Add(hairData.id, hairData);
		}
	}

	public static void SetupAction_BackHair(Dictionary<int, BackHairData> datas, AssetBundleController abc, CustomDataListLoader loader)
	{
		bool isNew = IsNewCheckFromABC(abc);
		int attributeNo = loader.GetAttributeNo("ID");
		int attributeNo2 = loader.GetAttributeNo("Name");
		int attributeNo3 = loader.GetAttributeNo("Prefab");
		int attributeNo4 = loader.GetAttributeNo("Set");
		int attributeNo5 = loader.GetAttributeNo("Type");
		int dataNum = loader.GetDataNum();
		for (int i = 0; i < dataNum; i++)
		{
			int id = int.Parse(loader.GetData(attributeNo, i));
			string data = loader.GetData(attributeNo2, i);
			string data2 = loader.GetData(attributeNo3, i);
			string data3 = loader.GetData(attributeNo5, i);
			bool isSet = bool.Parse(loader.GetData(attributeNo4, i));
			int count = datas.Count;
			BackHairData backHairData = new BackHairData(id, data, abc.assetBundleName, data2, count, isNew, data3, isSet);
			datas.Add(backHairData.id, backHairData);
		}
	}

	public static void SetupAction_UnderHair(Dictionary<int, UnderhairData> datas, AssetBundleController abc, CustomDataListLoader loader)
	{
		bool isNew = IsNewCheckFromABC(abc);
		int attributeNo = loader.GetAttributeNo("ID");
		int attributeNo2 = loader.GetAttributeNo("Name");
		int attributeNo3 = loader.GetAttributeNo("Prefab");
		int attributeNo4 = loader.GetAttributeNo("Sub");
		int dataNum = loader.GetDataNum();
		for (int i = 0; i < dataNum; i++)
		{
			int id = int.Parse(loader.GetData(attributeNo, i));
			string data = loader.GetData(attributeNo2, i);
			string data2 = loader.GetData(attributeNo3, i);
			int sub = -1;
			string data3 = loader.GetData(attributeNo4, i);
			if (data3 != "-")
			{
				sub = int.Parse(data3);
			}
			int count = datas.Count;
			UnderhairData underhairData = new UnderhairData(id, data, abc.assetBundleName, data2, sub, count, isNew);
			datas.Add(underhairData.id, underhairData);
		}
	}

	public static void SetupAction_Wear(Dictionary<int, WearData> datas, AssetBundleController abc, CustomDataListLoader loader)
	{
		bool isNew = IsNewCheckFromABC(abc);
		int attributeNo = loader.GetAttributeNo("ID");
		int attributeNo2 = loader.GetAttributeNo("Name");
		int attributeNo3 = loader.GetAttributeNo("Prefab");
		int attributeNo4 = loader.GetAttributeNo("Liquid");
		int attributeNo5 = loader.GetAttributeNo("Normal");
		int attributeNo6 = loader.GetAttributeNo("Coordinates");
		int attributeNo7 = loader.GetAttributeNo("BraDisable");
		int attributeNo8 = loader.GetAttributeNo("ShortsDisable");
		int attributeNo9 = loader.GetAttributeNo("Nip");
		int attributeNo10 = loader.GetAttributeNo("UnderHair");
		int attributeNo11 = loader.GetAttributeNo("Special");
		int dataNum = loader.GetDataNum();
		for (int i = 0; i < dataNum; i++)
		{
			int id = int.Parse(loader.GetData(attributeNo, i));
			string data = loader.GetData(attributeNo2, i);
			string data2 = loader.GetData(attributeNo3, i);
			int order = datas.Count;
			if (data == "なし")
			{
				order = -1;
			}
			WearData wearData = new WearData(id, data, abc.assetBundleName, data2, order, isNew);
			string text = "-";
			if (attributeNo4 != -1)
			{
				wearData.liquid = loader.GetData(attributeNo4, i);
			}
			if (attributeNo5 != -1)
			{
				wearData.normal = loader.GetData(attributeNo5, i);
			}
			if (attributeNo6 != -1)
			{
				wearData.coordinates = int.Parse(loader.GetData(attributeNo6, i));
			}
			if (attributeNo7 != -1)
			{
				wearData.braDisable = int.Parse(loader.GetData(attributeNo7, i)) != 0;
			}
			if (attributeNo8 != -1)
			{
				wearData.shortsDisable = int.Parse(loader.GetData(attributeNo8, i)) != 0;
			}
			if (attributeNo9 != -1)
			{
				wearData.nip = int.Parse(loader.GetData(attributeNo9, i)) != 0;
			}
			if (attributeNo10 != -1)
			{
				wearData.underhair = int.Parse(loader.GetData(attributeNo10, i)) != 0;
			}
			if (attributeNo11 != -1)
			{
				text = loader.GetData(attributeNo11, i);
			}
			if (text == "VR_EVENT")
			{
				wearData.special = ItemDataBase.SPECIAL.VR_EVENT;
			}
			if (!datas.ContainsKey(wearData.id))
			{
				datas.Add(wearData.id, wearData);
				continue;
			}
			Debug.LogError("ID重複：" + wearData.id + " " + wearData.name);
		}
	}

	public static void SetupAction_Accessory(Dictionary<int, AccessoryData> datas, AssetBundleController abc, CustomDataListLoader loader)
	{
		bool isNew = IsNewCheckFromABC(abc);
		int attributeNo = loader.GetAttributeNo("ID");
		int attributeNo2 = loader.GetAttributeNo("Name");
		int attributeNo3 = loader.GetAttributeNo("Prefab_M");
		int attributeNo4 = loader.GetAttributeNo("Prefab_F");
		int attributeNo5 = loader.GetAttributeNo("Parent");
		int attributeNo6 = loader.GetAttributeNo("Special");
		int dataNum = loader.GetDataNum();
		for (int i = 0; i < dataNum; i++)
		{
			int id = int.Parse(loader.GetData(attributeNo, i));
			string data = loader.GetData(attributeNo2, i);
			string data2 = loader.GetData(attributeNo3, i);
			string data3 = loader.GetData(attributeNo4, i);
			string data4 = loader.GetData(attributeNo5, i);
			int count = datas.Count;
			ItemDataBase.SPECIAL special = ItemDataBase.SPECIAL.NONE;
			string text = "-";
			if (attributeNo6 != -1)
			{
				text = loader.GetData(attributeNo6, i);
			}
			if (text == "VR_EVENT")
			{
				special = ItemDataBase.SPECIAL.VR_EVENT;
			}
			AccessoryData accessoryData = new AccessoryData(id, data, abc.assetBundleName, data2, data3, data4, special, count, isNew);
			if (!datas.ContainsKey(accessoryData.id))
			{
				datas.Add(accessoryData.id, accessoryData);
				continue;
			}
			Debug.LogWarning("ID重複：" + accessoryData.id + " " + accessoryData.name);
			datas[accessoryData.id] = accessoryData;
		}
	}

	public static void SetupAction_CombineTexture(Dictionary<int, CombineTextureData> datas, AssetBundleController abc, CustomDataListLoader loader)
	{
		int attributeNo = loader.GetAttributeNo("ID");
		int attributeNo2 = loader.GetAttributeNo("Name");
		int attributeNo3 = loader.GetAttributeNo("Prefab");
		int attributeNo4 = loader.GetAttributeNo("X");
		int attributeNo5 = loader.GetAttributeNo("Y");
		bool isNew = IsNewCheckFromABC(abc);
		int dataNum = loader.GetDataNum();
		for (int i = 0; i < dataNum; i++)
		{
			int count = datas.Count;
			int id = int.Parse(loader.GetData(attributeNo, i));
			string data = loader.GetData(attributeNo2, i);
			string data2 = loader.GetData(attributeNo3, i);
			int x = ((attributeNo4 != -1) ? int.Parse(loader.GetData(attributeNo4, i)) : 0);
			int y = ((attributeNo5 != -1) ? int.Parse(loader.GetData(attributeNo5, i)) : 0);
			CombineTextureData combineTextureData = new CombineTextureData(id, data, abc.assetBundleName, data2, x, y, count, isNew);
			datas.Add(combineTextureData.id, combineTextureData);
		}
	}

	private static bool IsNewCheckFromABC(AssetBundleController abc)
	{
		string assetBundleName = abc.assetBundleName;
		if (assetBundleName.IndexOf("hs00") != -1 || assetBundleName.IndexOf("hsad") != -1 || assetBundleName.IndexOf("hs15") != -1 || assetBundleName.IndexOf("hs01-20") != -1 || assetBundleName.IndexOf("00ph") != -1 || assetBundleName.LastIndexOf("_00") != -1 || assetBundleName.IndexOf("ph00") != -1)
		{
			return false;
		}
		return true;
	}
}
