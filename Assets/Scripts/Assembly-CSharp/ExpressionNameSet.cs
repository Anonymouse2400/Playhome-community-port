using System;
using System.Collections.Generic;
using UnityEngine;

internal class ExpressionNameSet
{
	private Dictionary<string, string> eyeDic = new Dictionary<string, string>();

	private Dictionary<string, string> mouthDic = new Dictionary<string, string>();

	public ExpressionNameSet(TextAsset eyeList, TextAsset mouthList)
	{
		Setup_Dictionary(eyeDic, eyeList);
		Setup_Dictionary(mouthDic, mouthList);
	}

	public string EyeName(string name)
	{
		if (eyeDic.ContainsKey(name))
		{
			return eyeDic[name];
		}
		Debug.LogError("目の表情：" + name + " が見つかりません");
		return "Eye_Def";
	}

	public string MouthName(string name)
	{
		if (mouthDic.ContainsKey(name))
		{
			return mouthDic[name];
		}
		Debug.LogError("口の表情：" + name + " が見つかりません");
		return "Mouth_Def";
	}

	private static void Setup_Dictionary(Dictionary<string, string> dic, TextAsset text)
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(text);
		int attributeNo = customDataListLoader.GetAttributeNo("anime");
		int attributeNo2 = customDataListLoader.GetAttributeNo("name");
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			dic.Add(data2, data);
		}
	}
}
