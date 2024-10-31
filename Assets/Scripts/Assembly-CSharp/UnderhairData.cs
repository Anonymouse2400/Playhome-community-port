using System;

public class UnderhairData : PrefabData
{
	public int sub;

	public UnderhairData(int id, string name, string assetbundle, string prefab, int sub, int order, bool isNew)
		: base(id, name, assetbundle, prefab, order, isNew)
	{
		this.sub = sub;
	}
}
