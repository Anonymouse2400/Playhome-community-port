using System;
using UnityEngine;

public static class AlloyUtils
{
	public const string CurrentVersion = "3.5.2";

	public const string RootFolder = "Alloy/";

	public const string ComponentsPath = "Alloy/";

	public const string MenubarPath = "Window/Alloy/";

	public const float MaxSectionColorIndex = 18f;

	public static string AssetsPath
	{
		get
		{
			return Application.persistentDataPath + "/Alloy/";
		}
	}
}
