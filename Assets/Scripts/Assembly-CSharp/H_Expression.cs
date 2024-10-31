using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class H_Expression : MonoBehaviour
{
	public enum STATE
	{
		RESIST = 1,
		FLIP = 2,
		PAIN = 4,
		WEAKNESS = 8,
		AHE = 0x10
	}

	public enum TYPE
	{
		BREATH = 0,
		TALK = 1,
		PANT_TALK_LO = 2,
		PANT_TALK_HI = 3,
		PANT_LO_NORMAL = 4,
		PANT_LO_HIT = 5,
		PANT_HI_NORMAL = 6,
		PANT_HI_HIT = 7,
		LICK_LO_NORMAL = 8,
		LICK_LO_HIT = 9,
		LICK_HI_NORMAL = 10,
		LICK_HI_HIT = 11,
		FELLATIO_LO_NORMAL = 12,
		FELLATIO_LO_HIT = 13,
		FELLATIO_HI_NORMAL = 14,
		FELLATIO_HI_HIT = 15,
		IRRUMATIO_LO_NORMAL = 16,
		IRRUMATIO_LO_HIT = 17,
		IRRUMATIO_HI_NORMAL = 18,
		IRRUMATIO_HI_HIT = 19,
		JOB_LO = 20,
		JOB_HI = 21,
		INSERT = 22,
		INSERT_FELLATIO = 23,
		INSERT_IRRUMATIO = 24,
		EJACULATION_IN_LIKE = 25,
		EJACULATION_IN_NORMAL = 26,
		EJACULATION_OUT_LIKE = 27,
		EJACULATION_OUT_NORMAL = 28,
		EJACULATION_MOUTH_LIKE = 29,
		EJACULATION_MOUTH_NORMAL = 30,
		XTC = 31,
		XTC_AFTER_BREATH = 32,
		INEJA_AFTER_BREATH = 33,
		OUTEJA_AFTER_BREATH = 34,
		EXTRACT = 35,
		EXTRACT_FELLATIO = 36,
		EXTRACT_IRRUMATIO = 37,
		COUGH = 38,
		DRINK = 39,
		VOMIT = 40,
		SHOW_ORAL = 41,
		NUM = 42
	}

	[SerializeField]
	private TextAsset[] expressionList = new TextAsset[3];

	private H_ExpressionData[] datas = new H_ExpressionData[3];

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
			datas[i] = new H_ExpressionData(this, expressionList[i]);
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

	public H_ExpressionData.Data ChangeExpression(Human human, string voiceFile, H_Parameter param)
	{
		if (!isUpdate)
		{
			return null;
		}
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			return datas[(int)female.HeroineID].ChangeExpression(human, voiceFile, param, duration);
		}
		return null;
	}

	public H_ExpressionData.Data ChangeExpression(Human human, TYPE type, H_Parameter param)
	{
		if (!isUpdate)
		{
			return null;
		}
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			return datas[(int)female.HeroineID].ChangeExpression(human, type, param, duration);
		}
		return null;
	}
}
