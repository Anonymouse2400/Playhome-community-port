using System;
using System.Collections.Generic;
using System.IO;
using Character;
using UnityEngine;

public static class MaterialCustomData
{
	//public static readonly string directory = Application.dataPath + "/../UserData/Save";

	//public static readonly string file = Application.dataPath + "/../UserData/Save/MaterialsSaveData";

    public static readonly string directory = Application.persistentDataPath + "/UserData/Save";

    public static readonly string file = Application.persistentDataPath + "/UserData/Save/MaterialsSaveData";

    private static Dictionary<int, ColorParameter_PBR1>[] hairAcces = new Dictionary<int, ColorParameter_PBR1>[3];

	private static Dictionary<int, ColorParameter_PBR2>[] wears = new Dictionary<int, ColorParameter_PBR2>[11];

	private static Dictionary<int, ColorParameter_PBR2>[] acces = new Dictionary<int, ColorParameter_PBR2>[12];

	private static void Setup()
	{
		for (int i = 0; i < hairAcces.Length; i++)
		{
			hairAcces[i] = new Dictionary<int, ColorParameter_PBR1>();
		}
		for (int j = 0; j < wears.Length; j++)
		{
			wears[j] = new Dictionary<int, ColorParameter_PBR2>();
		}
		for (int k = 0; k < acces.Length; k++)
		{
			acces[k] = new Dictionary<int, ColorParameter_PBR2>();
		}
	}

	private static void Clear()
	{
		for (int i = 0; i < hairAcces.Length; i++)
		{
			hairAcces[i].Clear();
		}
		for (int j = 0; j < wears.Length; j++)
		{
			wears[j].Clear();
		}
		for (int k = 0; k < acces.Length; k++)
		{
			acces[k].Clear();
		}
	}

	public static void Save()
	{
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		FileStream fileStream = new FileStream(file, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(10);
		binaryWriter.Write("hairAcces");
		for (int i = 0; i < hairAcces.Length; i++)
		{
			binaryWriter.Write(hairAcces[i].Count);
			foreach (KeyValuePair<int, ColorParameter_PBR1> item in hairAcces[i])
			{
				binaryWriter.Write(item.Key);
				item.Value.Save(binaryWriter);
			}
		}
		binaryWriter.Write("wears");
		for (int j = 0; j < wears.Length; j++)
		{
			binaryWriter.Write(wears[j].Count);
			foreach (KeyValuePair<int, ColorParameter_PBR2> item2 in wears[j])
			{
				binaryWriter.Write(item2.Key);
				item2.Value.Save(binaryWriter);
			}
		}
		binaryWriter.Write("acces");
		for (int k = 0; k < acces.Length; k++)
		{
			binaryWriter.Write(acces[k].Count);
			foreach (KeyValuePair<int, ColorParameter_PBR2> item3 in acces[k])
			{
				binaryWriter.Write(item3.Key);
				item3.Value.Save(binaryWriter);
			}
		}
		binaryWriter.Close();
		fileStream.Close();
	}

	public static void Load()
	{
		Setup();
		if (!File.Exists(file))
		{
			return;
		}
		FileStream fileStream = new FileStream(file, FileMode.Open);
		if (fileStream == null)
		{
			return;
		}
		BinaryReader binaryReader = new BinaryReader(fileStream);
		int num = binaryReader.ReadInt32();
		if (num < 0 || num > 10)
		{
			Debug.LogError("不明なバージョン：" + num);
			binaryReader.Close();
			fileStream.Close();
			return;
		}
		if (binaryReader.ReadString() != "hairAcces")
		{
			Debug.LogError("髪アクセ以降のマテリアルカスタムが読めなかった");
			return;
		}
		for (int i = 0; i < hairAcces.Length; i++)
		{
			int num2 = binaryReader.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				int key = binaryReader.ReadInt32();
				ColorParameter_PBR1 colorParameter_PBR = new ColorParameter_PBR1();
				colorParameter_PBR.Load(binaryReader, (CUSTOM_DATA_VERSION)num);
				SetHairAcce((HAIR_TYPE)i, key, colorParameter_PBR);
			}
		}
		if (binaryReader.ReadString() != "wears")
		{
			Debug.LogError("服以降のマテリアルカスタムが読めなかった");
			return;
		}
		for (int k = 0; k < wears.Length; k++)
		{
			int num3 = binaryReader.ReadInt32();
			for (int l = 0; l < num3; l++)
			{
				int key2 = binaryReader.ReadInt32();
				ColorParameter_PBR2 colorParameter_PBR2 = new ColorParameter_PBR2();
				colorParameter_PBR2.Load(binaryReader, (CUSTOM_DATA_VERSION)num);
				SetWear((WEAR_TYPE)k, key2, colorParameter_PBR2);
			}
		}
		if (binaryReader.ReadString() != "acces")
		{
			Debug.LogError("アクセ以降のマテリアルカスタムが読めなかった");
			return;
		}
		for (int m = 0; m < acces.Length; m++)
		{
			int num4 = binaryReader.ReadInt32();
			for (int n = 0; n < num4; n++)
			{
				int key3 = binaryReader.ReadInt32();
				ColorParameter_PBR2 colorParameter_PBR3 = new ColorParameter_PBR2();
				colorParameter_PBR3.Load(binaryReader, (CUSTOM_DATA_VERSION)num);
				SetAcce((ACCESSORY_TYPE)m, key3, colorParameter_PBR3);
			}
		}
		binaryReader.Close();
		fileStream.Close();
	}

	private static void SetHairAcce(HAIR_TYPE hairType, int key, ColorParameter_PBR1 color)
	{
		Dictionary<int, ColorParameter_PBR1> dictionary = hairAcces[(int)hairType];
		if (!dictionary.ContainsKey(key))
		{
			dictionary.Add(key, color);
		}
		else
		{
			dictionary[key].Copy(color);
		}
	}

	public static void SetHairAcce(HAIR_TYPE hairType, HairPartParameter param)
	{
		SetHairAcce(hairType, param.ID, param.acceColor);
	}

	private static void SetWear(WEAR_TYPE wearType, int key, ColorParameter_PBR2 color)
	{
		Dictionary<int, ColorParameter_PBR2> dictionary = wears[(int)wearType];
		if (!dictionary.ContainsKey(key))
		{
			dictionary.Add(key, color);
		}
		else
		{
			dictionary[key].Copy(color);
		}
	}

	public static void SetWear(WEAR_TYPE wearType, WearCustom param)
	{
		SetWear(wearType, param.id, param.color);
	}

	private static void SetAcce(ACCESSORY_TYPE acceType, int key, ColorParameter_PBR2 color)
	{
		Dictionary<int, ColorParameter_PBR2> dictionary = acces[(int)acceType];
		if (!dictionary.ContainsKey(key))
		{
			dictionary.Add(key, color);
		}
		else
		{
			dictionary[key].Copy(color);
		}
	}

	public static void SetAcce(AccessoryCustom custom)
	{
		SetAcce(custom.type, custom.id, custom.color);
	}

	private static ColorParameter_PBR1 GetHairAcce(HAIR_TYPE hairType, int key)
	{
		Dictionary<int, ColorParameter_PBR1> dictionary = hairAcces[(int)hairType];
		if (dictionary.ContainsKey(key))
		{
			return dictionary[key];
		}
		return null;
	}

	public static bool GetHairAcce(HAIR_TYPE hairType, HairPartParameter param)
	{
		ColorParameter_PBR1 hairAcce = GetHairAcce(hairType, param.ID);
		if (hairAcce != null)
		{
			if (param.acceColor == null)
			{
				param.acceColor = new ColorParameter_PBR1();
			}
			param.acceColor.Copy(hairAcce);
			return true;
		}
		return false;
	}

	private static ColorParameter_PBR2 GetWear(WEAR_TYPE wearType, int key)
	{
		Dictionary<int, ColorParameter_PBR2> dictionary = wears[(int)wearType];
		if (dictionary.ContainsKey(key))
		{
			return dictionary[key];
		}
		return null;
	}

	public static bool GetWear(WEAR_TYPE wearType, WearCustom param)
	{
		ColorParameter_PBR2 wear = GetWear(wearType, param.id);
		if (wear != null)
		{
			if (param.color == null)
			{
				param.color = new ColorParameter_PBR2();
			}
			param.color.Copy(wear);
			return true;
		}
		return false;
	}

	private static ColorParameter_PBR2 GetAcce(ACCESSORY_TYPE acceType, int key)
	{
		Dictionary<int, ColorParameter_PBR2> dictionary = acces[(int)acceType];
		if (dictionary.ContainsKey(key))
		{
			return dictionary[key];
		}
		return null;
	}

	public static bool GetAcce(AccessoryCustom custom)
	{
		ColorParameter_PBR2 acce = GetAcce(custom.type, custom.id);
		if (acce != null)
		{
			if (custom.color == null)
			{
				custom.color = new ColorParameter_PBR2();
			}
			custom.color.Copy(acce);
			return true;
		}
		return false;
	}
}
