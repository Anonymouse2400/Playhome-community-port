using System;
using System.IO;
using Character;
using UnityEngine;



public static class GlobalData
{

    public static void Start()
    {
        //Debug.Log($"The persistent data path on {Application.platform} is located at: {Application.persistentDataPath}");
    }
    //windows save data
    //public static readonly string directory = Application.dataPath + "/../UserData/Save";

	//windows system data
	//public static readonly string file = Application.dataPath + "/../UserData/Save/SystemData";

	//windows storage
	//public static readonly string assetBundlePath = Application.dataPath + "/StreamingAssets/abdata";

    //android save data
    public static readonly string directory = Application.persistentDataPath + "/UserData/Save";

    //android storage
    public static readonly string assetBundlePath = Application.persistentDataPath + "/abdata";

    //windows system data
    public static readonly string file = Application.persistentDataPath + "/UserData/Save/SystemData";

    public static readonly int version = 12;

	public static int sortChara = 0;

	public static int sortCoord = 0;

	public static bool poseChangeCameraFocus = false;

	public static int continueSaveNo = -1;

	public static bool flipflop = false;

	public static bool vr_event_item = false;

	public static bool showUploaderRule = false;

	public static string uploaderHandleName = string.Empty;

	public static MALE_SHOW[] maleShows = new MALE_SHOW[5];

	public static bool showMap = true;

	public static bool isMemory = false;

	public static GamePlayData PlayData = null;

	public static void Save()
	{
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		FileStream fileStream = new FileStream(file, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(version);
		binaryWriter.Write(sortChara);
		binaryWriter.Write(sortCoord);
		binaryWriter.Write(poseChangeCameraFocus);
		binaryWriter.Write(continueSaveNo);
		binaryWriter.Write(flipflop);
		binaryWriter.Write(vr_event_item);
		binaryWriter.Write(showUploaderRule);
		binaryWriter.Write(uploaderHandleName);
		for (int i = 0; i < maleShows.Length; i++)
		{
			binaryWriter.Write((int)maleShows[i]);
		}
		binaryWriter.Close();
		fileStream.Close();
	}

	public static void Load()
	{
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
			Debug.LogError("unknown version：" + num);
			binaryReader.Close();
			fileStream.Close();
			return;
		}
		if (num < 7)
		{
			binaryReader.Close();
			fileStream.Close();
			return;
		}
		sortChara = binaryReader.ReadInt32();
		sortCoord = binaryReader.ReadInt32();
		poseChangeCameraFocus = binaryReader.ReadBoolean();
		if (num >= 8)
		{
			continueSaveNo = binaryReader.ReadInt32();
		}
		if (num >= 9)
		{
			flipflop = binaryReader.ReadBoolean();
			vr_event_item = binaryReader.ReadBoolean();
		}
		if (num >= 10)
		{
			showUploaderRule = binaryReader.ReadBoolean();
		}
		if (num >= 11)
		{
			uploaderHandleName = binaryReader.ReadString();
		}
		if (num >= 12)
		{
			for (int i = 0; i < maleShows.Length; i++)
			{
				maleShows[i] = (MALE_SHOW)binaryReader.ReadInt32();
			}
		}
		else
		{
			for (int j = 0; j < maleShows.Length; j++)
			{
				maleShows[j] = MALE_SHOW.CLOTHING;
			}
		}
		binaryReader.Close();
		fileStream.Close();
	}

	public static bool CheckContinueSave()
	{
		if (continueSaveNo != -1)
		{
			string continueSaveFile = GetContinueSaveFile();
			return File.Exists(continueSaveFile);
		}
		return false;
	}

	public static string GetContinueSaveFile()
	{
        //return Directory.GetCurrentDirectory() + "/UserData/save/Game/" + (continueSaveNo + 1).ToString("00") + ".gsd";
        return Application.persistentDataPath + "/UserData/save/Game/" + (continueSaveNo + 1).ToString("00") + ".gsd";
    }

}
