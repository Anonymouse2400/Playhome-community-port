using System;
using System.IO;
using UnityEngine;

public static class DebugLog
{
	public static readonly string path = Application.persistentDataPath + "/DebugLog.txt";

	public static void AddLog(string str)
	{
		Debug.Log(str);
		StreamWriter streamWriter = new StreamWriter(path, true);
		streamWriter.WriteLine(str);
		streamWriter.Close();
	}
}
