using System;
using System.IO;
using UnityEngine;

public class SaveHeader
{
	public enum VERSION
	{
		VERSION_0 = 0,
		VERSION_1 = 1,
		VERSION_2 = 2,
		VERSION_3 = 3,
		NEXTVERSION = 4,
		NEW_VERSION = 3
	}

	public int version;

	public string comment;

	public string time;

	public SaveHeader()
	{
		Clear();
	}

	public SaveHeader(SaveHeader copy)
	{
		version = copy.version;
		comment = copy.comment;
		time = copy.time;
	}

	public SaveHeader(int version)
	{
		this.version = version;
		comment = string.Empty;
		time = string.Empty;
	}

	public void Clear()
	{
		version = -1;
		comment = string.Empty;
		time = string.Empty;
	}

	public bool Load(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		if (num > 3)
		{
			Debug.LogError("不明なバージョン");
			return false;
		}
		version = reader.ReadInt32();
		comment = reader.ReadString();
		time = reader.ReadString();
		return true;
	}

	public bool Load(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = File.OpenRead(path);
		}
		catch
		{
			return false;
		}
		if (fileStream == null)
		{
			return false;
		}
		BinaryReader reader = new BinaryReader(fileStream);
		bool result = Load(reader);
		fileStream.Close();
		return result;
	}

	public bool Save(BinaryWriter writer)
	{
		time = NowTimeToStr();
		writer.Write(3);
		writer.Write(version);
		writer.Write(comment);
		writer.Write(time);
		return true;
	}

	public void SetNowTime()
	{
		time = NowTimeToStr();
	}

	public static string NowTimeToStr()
	{
		return DateTime.Now.Year.ToString("0000") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Day.ToString("00") + " " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
	}

	public static SaveHeader LoadSaveHeader(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = File.OpenRead(path);
		}
		catch
		{
			return null;
		}
		if (fileStream == null)
		{
			return null;
		}
		BinaryReader reader = new BinaryReader(fileStream);
		SaveHeader saveHeader = new SaveHeader();
		bool flag = saveHeader.Load(reader);
		fileStream.Close();
		if (flag)
		{
			return saveHeader;
		}
		return null;
	}
}
