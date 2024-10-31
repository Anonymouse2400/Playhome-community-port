using System;
using UnityEngine;
using System.IO;

public static class AssetBundleLoader
{
	public static T LoadAsset<T>(string directory, string assetBundleName, string assetName) where T : UnityEngine.Object
    {
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(directory, assetBundleName);
		T t = assetBundleController.LoadAsset<T>(assetName);
		assetBundleController.Close();
		return t;
	}

	public static T LoadAndInstantiate<T>(string directory, string assetBundleName, string assetName) where T : UnityEngine.Object
    {
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(directory, assetBundleName);
		T t = assetBundleController.LoadAndInstantiate<T>(assetName);
		assetBundleController.Close();
		return t;
	}
}
