using System;
using System.IO;
using UnityEngine;

public class AssetBundleController
{
	private AssetBundle assetBundle;

	public string directory { get; private set; }

	public string assetBundleName { get; private set; }

	public bool isSimulate { get; private set; }

	public bool isOpened
	{
		get
		{
			return assetBundle != null;
		}
	}

	public AssetBundleController()
	{
		assetBundleName = string.Empty;
		directory = string.Empty;
		isSimulate = false;
	}

	public AssetBundleController(bool isSimulate)
	{
		assetBundleName = string.Empty;
		directory = string.Empty;
		this.isSimulate = isSimulate;
	}

	~AssetBundleController()
	{
		if (isOpened)
		{
			Debug.LogError("Forgot to close asset bundle:" + assetBundleName);
		}
	}

	public bool OpenFromFile(string directory, string assetBundleName)
	{
		this.directory = directory;
		this.assetBundleName = assetBundleName;
		string empty = string.Empty;
		int num = assetBundleName.LastIndexOf("/abdata");
		if (num != -1)
		{
			empty = "/" + assetBundleName.Substring(0, num);
			assetBundleName = assetBundleName.Remove(0, num + 1);
		}
		string text2 = this.directory + "/" + this.assetBundleName;
		assetBundle = null;
		if (File.Exists(text2))
		{
			assetBundle = AssetBundle.LoadFromFile(text2);
		}
		else
		{
			Debug.Log("No asset bundle：" + text2);
		}
		return assetBundle != null;
	}

	public void Close(bool unloadAllLoadedObjects = false)
	{
		try
		{
			if (assetBundle != null)
			{
				assetBundle.Unload(unloadAllLoadedObjects);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(assetBundleName + ":" + ex);
		}
	}

	public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
	{
		if (assetBundle != null)
		{
			return assetBundle.LoadAsset<T>(assetName);
		}
		return (T)null;
	}

	public T LoadAndInstantiate<T>(string assetName) where T : UnityEngine.Object
	{
		T t = LoadAsset<T>(assetName);
		if (t != null)
		{
			return UnityEngine.Object.Instantiate<T>(t);
		}
		return (T)null;
	}

	public static AssetBundleController New_OpenFromFile(string directory, string assetBundleName)
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		if (assetBundleController.OpenFromFile(directory, assetBundleName))
		{
			return assetBundleController;
		}
		return null;
	}
}
