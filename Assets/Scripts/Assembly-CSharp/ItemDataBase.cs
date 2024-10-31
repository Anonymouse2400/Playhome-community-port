using System;
using UnityEngine;

public class ItemDataBase
{
	public enum SPECIAL
	{
		NONE = 0,
		VR_EVENT = 1
	}

	public int id;

	public string name;

	public string name_LineFeed;

	public string assetbundleDir;

	public string assetbundleName;

	public int order;

	public SPECIAL special;

	public bool isNew;

	public ItemDataBase(int id, string name, string assetbundleName, int order, bool isNew)
	{
		name = name.Replace("\\n", "\n");
		this.id = id % 1000;
		name_LineFeed = name;
		this.name = name.Replace("\n", string.Empty);
		assetbundleDir = Application.persistentDataPath + "/abdata";
		this.assetbundleName = assetbundleName;
		this.order = order;
		this.isNew = isNew;
	}
}
