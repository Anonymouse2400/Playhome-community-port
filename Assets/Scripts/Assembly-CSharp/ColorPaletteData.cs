using System;
using System.IO;
using UnityEngine;

public static class ColorPaletteData
{
	//public static readonly string directory = Application.dataPath + "/../UserData/Save";

	//public static readonly string file = Application.dataPath + "/../UserData/Save/ColorPalette";

    public static readonly string directory = Application.persistentDataPath + "/UserData/Save";

    public static readonly string file = Application.persistentDataPath + "/UserData/Save/ColorPalette";

    public static readonly int version = 0;

	public static Color[] colors = new Color[50];

	public static void Init()
	{
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = Color.white;
		}
		colors[0] = Color.HSVToRGB(0f, 0f, 0.75f);
		colors[10] = Color.HSVToRGB(0f, 0f, 0.5f);
		colors[20] = Color.HSVToRGB(0f, 0f, 0.25f);
		float[] array = new float[3] { 0.25f, 1f, 0.5f };
		float[] array2 = new float[3] { 0.75f, 1f, 0.5f };
		for (int j = 0; j < 3; j++)
		{
			float s = array[j];
			float v = array2[j];
			for (int k = 0; k < 9; k++)
			{
				int num = j * 10 + k + 1;
				float h = 40f * (float)k / 360f;
				colors[num] = Color.HSVToRGB(h, s, v);
			}
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
		binaryWriter.Write(version);
		for (int i = 0; i < colors.Length; i++)
		{
			binaryWriter.Write(colors[i].r);
			binaryWriter.Write(colors[i].g);
			binaryWriter.Write(colors[i].b);
			binaryWriter.Write(colors[i].a);
		}
		binaryWriter.Close();
		fileStream.Close();
	}

	public static void Load()
	{
		Init();
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
		if (num < 0 || num > version)
		{
			Debug.LogError("不明なバージョン：" + num);
			binaryReader.Close();
			fileStream.Close();
			return;
		}
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i].r = binaryReader.ReadSingle();
			colors[i].g = binaryReader.ReadSingle();
			colors[i].b = binaryReader.ReadSingle();
			colors[i].a = binaryReader.ReadSingle();
		}
		binaryReader.Close();
		fileStream.Close();
	}
}
