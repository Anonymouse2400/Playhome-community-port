using System;

public class HeadData : ItemDataBase
{
	public string path;

	public HeadData(int id, string name, string path, int order, bool isNew)
		: base(id, name, string.Empty, order, isNew)
	{
		this.path = path;
	}
}
