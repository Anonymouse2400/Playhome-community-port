using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class H_VisitorExpressionData
{
	public abstract class Data
	{
		public string eyeType;

		public string mouthType;

		public float eyeOpen;

		public float mouthOpen;
	}

	public class Data_Voice : Data
	{
		public string voice;

		public Data_Voice(string voice, string eyeType, string mouthType, float eyeOpen, float mouthOpen)
		{
			this.voice = voice;
			base.eyeType = eyeType;
			base.mouthType = mouthType;
			base.eyeOpen = eyeOpen;
			base.mouthOpen = mouthOpen;
		}
	}

	private class Data_Voice_Set
	{
		private List<Data_Voice> list = new List<Data_Voice>();

		public Data_Voice this[int no]
		{
			get
			{
				return list[no];
			}
		}

		public int Count
		{
			get
			{
				return list.Count;
			}
		}

		public void Add(Data_Voice add)
		{
			list.Add(add);
		}
	}

	public class Data_Type : Data
	{
		public H_VisitorExpression.STATE state;

		public H_VisitorExpression.TYPE type;

		public Data_Type(string state, string type, string eyeType, string mouthType, float eyeOpen, float mouthOpen)
		{
			this.state = CheckState(state);
			this.type = CheckType(type);
			base.eyeType = eyeType;
			base.mouthType = mouthType;
			base.eyeOpen = eyeOpen;
			base.mouthOpen = mouthOpen;
		}

		private static H_VisitorExpression.STATE CheckState(string str)
		{
			H_VisitorExpression.STATE result = (H_VisitorExpression.STATE)0;
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

		private static H_VisitorExpression.TYPE CheckType(string str)
		{
			for (int i = 0; i < TypeStrs.Length; i++)
			{
				if (str == TypeStrs[i])
				{
					return (H_VisitorExpression.TYPE)i;
				}
			}
			Debug.LogError("不明な種類:" + str);
			return H_VisitorExpression.TYPE.NUM;
		}
	}

	private static readonly string[] StateStrs = new string[2] { "抵抗", "豹変" };

	private static readonly H_VisitorExpression.STATE[] States = new H_VisitorExpression.STATE[2]
	{
		H_VisitorExpression.STATE.RESIST,
		H_VisitorExpression.STATE.FLIP
	};

	private static readonly string[] TypeStrs = new string[2] { "呼吸", "台詞" };

	private List<Data_Type> type_datas = new List<Data_Type>();

	private Dictionary<string, Data_Voice_Set> voice_datas = new Dictionary<string, Data_Voice_Set>();

	public H_VisitorExpressionData(H_VisitorExpression owner, TextAsset loadList)
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
			if (data == "台詞")
			{
				Data_Voice data5 = new Data_Voice(data2, data3, data4, eyeOpen, mouthOpen);
				AddVoiceData(data5);
			}
			else
			{
				CreateTypeData(data, data2, data3, data4, eyeOpen, mouthOpen);
			}
		}
	}

	private void CreateTypeData(string state, string type, string eyeType, string mouthType, float eyeOpen, float mouthOpen)
	{
		string[] array = state.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			type_datas.Add(new Data_Type(array[i], type, eyeType, mouthType, eyeOpen, mouthOpen));
		}
	}

	private void AddVoiceData(Data_Voice data)
	{
		if (!voice_datas.ContainsKey(data.voice))
		{
			voice_datas.Add(data.voice, new Data_Voice_Set());
		}
		voice_datas[data.voice].Add(data);
	}

	public Data ChangeExpression(Human human, string voiceFile, float duration)
	{
		Data data = null;
		if (!voice_datas.ContainsKey(voiceFile))
		{
			return null;
		}
		Data_Voice_Set data_Voice_Set = voice_datas[voiceFile];
		int no = UnityEngine.Random.Range(0, data_Voice_Set.Count);
		data = data_Voice_Set[no];
		if (data != null)
		{
			human.ExpressionPlay(0, data.eyeType, duration);
			human.blink.LimitMax = data.eyeOpen * 0.01f;
			human.lipSync.RelaxOpen = data.mouthOpen * 0.01f;
			string mouthType = data.mouthType;
			human.ExpressionPlay(1, mouthType, duration);
			human.lipSync.Hold = human.Gag;
			human.head.ResetMouth(human.Gag);
			TONGUE_TYPE tongueType = TONGUE_TYPE.FACE;
			human.SetTongueType(tongueType);
		}
		return data;
	}

	public Data ChangeExpression(Human human, H_VisitorExpression.TYPE type, float duration)
	{
		Data data = null;
		List<Data> list = new List<Data>();
		H_VisitorExpression.STATE sTATE = (H_VisitorExpression.STATE)0;
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			sTATE = ((!female.IsFloped()) ? H_VisitorExpression.STATE.RESIST : H_VisitorExpression.STATE.FLIP);
		}
		foreach (Data_Type type_data in type_datas)
		{
			if ((type_data.state & sTATE) != 0 && type_data.type == type)
			{
				list.Add(type_data);
			}
		}
		if (list.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			data = list[index];
		}
		if (data != null)
		{
			human.lipSync.Hold = human.Gag;
			human.head.ResetMouth(human.Gag);
			human.SetTongueType(TONGUE_TYPE.FACE);
			human.ExpressionPlay(0, data.eyeType, duration);
			human.blink.LimitMax = data.eyeOpen * 0.01f;
			human.lipSync.RelaxOpen = data.mouthOpen * 0.01f;
			human.ExpressionPlay(1, data.mouthType, duration);
		}
		else
		{
			Debug.LogError("ふさわしい表情がなかった:" + sTATE);
		}
		return data;
	}
}
