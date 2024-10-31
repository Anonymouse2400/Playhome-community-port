using System;

public class WearData : PrefabData
{
	public string liquid = string.Empty;

	public string normal = string.Empty;

	public int coordinates;

	public bool braDisable;

	public bool shortsDisable;

	public bool nip;

	public bool underhair;

	public WearData(int id, string name, string assetbundle, string prefab, int order, bool isNew)
		: base(id, name, assetbundle, prefab, order, isNew)
	{
	}
}
