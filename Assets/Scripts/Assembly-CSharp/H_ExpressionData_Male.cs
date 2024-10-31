using System;
using System.Collections.Generic;
using Character;
using H;
using UnityEngine;

public class H_ExpressionData_Male
{
	public class Data
	{
		public string eyeType;

		public string mouthType;

		public float eyeOpen;

		public float mouthOpen;

		public H_Expression_Male.STATE state;

		public H_Expression_Male.TYPE type;

		public Data(string state, string type, string eyeType, string mouthType, float eyeOpen, float mouthOpen)
		{
			this.state = CheckState(state);
			this.type = CheckType(type);
			this.eyeType = eyeType;
			this.mouthType = mouthType;
			this.eyeOpen = eyeOpen;
			this.mouthOpen = mouthOpen;
		}

		private static H_Expression_Male.STATE CheckState(string str)
		{
			H_Expression_Male.STATE result = H_Expression_Male.STATE.NORMAL;
			int i;
			for (i = 0; i < StateStrs.Length; i++)
			{
				if (str == StateStrs[i])
				{
					result = States[i];
					break;
				}
			}
			if (i == StateStrs.Length)
			{
				Debug.LogError("不明な状態:" + str);
			}
			return result;
		}

		private static H_Expression_Male.TYPE CheckType(string str)
		{
			for (int i = 0; i < TypeStrs.Length; i++)
			{
				if (str == TypeStrs[i])
				{
					return (H_Expression_Male.TYPE)i;
				}
			}
			Debug.LogError("不明な種類:" + str);
			return H_Expression_Male.TYPE.NUM;
		}
	}

	private static readonly string[] StateStrs = new string[2] { "普通", "嫌がり" };

	private static readonly H_Expression_Male.STATE[] States = new H_Expression_Male.STATE[2]
	{
		H_Expression_Male.STATE.NORMAL,
		H_Expression_Male.STATE.HATE
	};

	private static readonly string[] TypeStrs = new string[2] { "通常", "射精" };

	private List<Data> datas = new List<Data>();

	public H_ExpressionData_Male(H_Expression_Male owner, TextAsset loadList)
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(loadList);
		int attributeNo = customDataListLoader.GetAttributeNo("状態");
		int attributeNo2 = customDataListLoader.GetAttributeNo("種類");
		int attributeNo3 = customDataListLoader.GetAttributeNo("目");
		int attributeNo4 = customDataListLoader.GetAttributeNo("口");
		int attributeNo5 = customDataListLoader.GetAttributeNo("目の開き");
		int attributeNo6 = customDataListLoader.GetAttributeNo("口の開き");
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			string data3 = customDataListLoader.GetData(attributeNo3, i);
			string data4 = customDataListLoader.GetData(attributeNo4, i);
			float eyeOpen = float.Parse(customDataListLoader.GetData(attributeNo5, i));
			float mouthOpen = float.Parse(customDataListLoader.GetData(attributeNo6, i));
			data3 = owner.EyeName(data3);
			data4 = owner.MouthName(data4);
			CreateTypeData(data, data2, data3, data4, eyeOpen, mouthOpen);
		}
	}

	private void CreateTypeData(string state, string type, string eyeType, string mouthType, float eyeOpen, float mouthOpen)
	{
		string[] array = state.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			datas.Add(new Data(array[i], type, eyeType, mouthType, eyeOpen, mouthOpen));
		}
	}

	public Data ChangeExpression(Male male, H_Expression_Male.TYPE type, H_Parameter param, float duration)
	{
		Data data = null;
		List<Data> list = new List<Data>();
		H_Expression_Male.STATE sTATE = H_Expression_Male.STATE.NORMAL;
		if (male.MaleID == MALE_ID.KOUICHI)
		{
			sTATE = H_Expression_Male.STATE.HATE;
		}
		foreach (Data data2 in datas)
		{
			if (data2.state == sTATE && data2.type == type)
			{
				list.Add(data2);
			}
		}
		if (list.Count > 0)
		{
			int index = global::UnityEngine.Random.Range(0, list.Count);
			data = list[index];
		}
		if (data != null)
		{
			male.ExpressionPlay(0, data.eyeType, duration);
			male.blink.LimitMax = data.eyeOpen * 0.01f;
			male.lipSync.RelaxOpen = data.mouthOpen * 0.01f;
			bool flag = param.UseMouth(SEX.MALE);
			string name = MouthNo(male, data, param);
			male.ExpressionPlay(1, name, duration);
			male.lipSync.Hold = flag || male.Gag;
			male.head.ResetMouth(flag || male.Gag);
			TONGUE_TYPE tongueType = (flag ? TONGUE_TYPE.BODY : TONGUE_TYPE.FACE);
			male.SetTongueType(tongueType);
			male.CheckShow();
		}
		else
		{
			Debug.LogError("ふさわしい表情がなかった:" + sTATE);
		}
		return data;
	}

	private static string MouthNo(Human human, Data data, H_Parameter param)
	{
		if (!param.UseMouth(human.sex))
		{
			return data.mouthType;
		}
		if (param.mouth == H_MOUTH.KISS)
		{
			return "Mouth_Kiss";
		}
		if (param.mouth == H_MOUTH.LICK)
		{
			return "Mouth_Name";
		}
		return data.mouthType;
	}
}
