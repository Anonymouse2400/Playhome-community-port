using System;
using System.Collections.Generic;

public class IK_DataList
{
	public List<IK_Data> ikDatas = new List<IK_Data>();

	public void Add(IK_Data data)
	{
		ikDatas.Add(data);
	}
}
