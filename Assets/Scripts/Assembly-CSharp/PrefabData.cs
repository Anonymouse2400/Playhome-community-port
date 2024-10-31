using System;

public class PrefabData : ItemDataBase
{
	public string prefab;

	public PrefabData(int id, string name, string assetbundleName, string prefab, int order, bool isNew)
		: base(id, name, assetbundleName, order, isNew)
	{
		this.prefab = prefab;
	}
}
