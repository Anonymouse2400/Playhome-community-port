using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class H_VisitorExpression : MonoBehaviour
{
	public enum STATE
	{
		RESIST = 1,
		FLIP = 2
	}

	public enum TYPE
	{
		BREATH = 0,
		TALK = 1,
		NUM = 2
	}

	[SerializeField]
	private TextAsset[] expressionList = new TextAsset[3];

	private H_VisitorExpressionData[] datas = new H_VisitorExpressionData[3];

	[SerializeField]
	private TextAsset eyeList;

	[SerializeField]
	private TextAsset mouthList;

	[SerializeField]
	private float duration = 0.2f;

	private Dictionary<string, string> eyeDic = new Dictionary<string, string>();

	private Dictionary<string, string> mouthDic = new Dictionary<string, string>();

	[SerializeField]
	private bool isUpdate = true;

	public void Setup()
	{
		Setup_Dictionary(eyeDic, eyeList);
		Setup_Dictionary(mouthDic, mouthList);
		for (int i = 0; i < 3; i++)
		{
			datas[i] = new H_VisitorExpressionData(this, expressionList[i]);
		}
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

	public H_VisitorExpressionData.Data ChangeExpression(Human human, string voiceFile)
	{
		if (!isUpdate)
		{
			return null;
		}
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			return datas[(int)female.HeroineID].ChangeExpression(human, voiceFile, duration);
		}
		return null;
	}

	public H_VisitorExpressionData.Data ChangeExpression(Human human, TYPE type)
	{
		if (!isUpdate)
		{
			return null;
		}
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			return datas[(int)female.HeroineID].ChangeExpression(human, type, duration);
		}
		return null;
	}
}
