using System;
using UnityEngine;

public class AssetBundleLoadTest : MonoBehaviour
{
	public string assetBundleName;

	public string assetName;

	public void Load()
	{
		string assetBundlePath = GlobalData.assetBundlePath;
		AssetBundleLoader.LoadAndInstantiate<GameObject>(assetBundlePath, assetBundleName, assetName);
	}
}
