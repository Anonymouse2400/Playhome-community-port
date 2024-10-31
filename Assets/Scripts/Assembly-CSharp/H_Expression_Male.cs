using System;
using System.Collections.Generic;
using UnityEngine;

public class H_Expression_Male : MonoBehaviour
{
	public enum STATE
	{
		NORMAL = 0,
		HATE = 1
	}

	public enum TYPE
	{
		NORMAL = 0,
		EJACULATION = 1,
		NUM = 2
	}

	[SerializeField]
	private TextAsset expressionList;

	[SerializeField]
	private TextAsset eyeList;

	[SerializeField]
	private TextAsset mouthList;

	[SerializeField]
	private float duration = 0.2f;

	private H_ExpressionData_Male data;

	private Dictionary<string, string> eyeDic = new Dictionary<string, string>();

	private Dictionary<string, string> mouthDic = new Dictionary<string, string>();

	[SerializeField]
	private bool isUpdate = true;

	public void Setup()
	{
		Setup_Dictionary(eyeDic, eyeList);
		Setup_Dictionary(mouthDic, mouthList);
		data = new H_ExpressionData_Male(this, expressionList);
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
			string value = customDataListLoader.GetData(attributeNo, i);
			string key = customDataListLoader.GetData(attributeNo2, i);
			dic.Add(key, value);
		}
	}

	public H_ExpressionData_Male.Data ChangeExpression(Male male, TYPE type, H_Parameter param)
	{
		if (!isUpdate)
		{
			return null;
		}
		return data.ChangeExpression(male, type, param, duration);
	}
}
