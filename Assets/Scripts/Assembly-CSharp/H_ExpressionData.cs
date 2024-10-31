using System;
using System.Collections.Generic;
using Character;
using H;
using UnityEngine;

public class H_ExpressionData
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
		public H_Expression.STATE state;

		public H_Expression.TYPE type;

		public Data_Type(string state, string type, string eyeType, string mouthType, float eyeOpen, float mouthOpen)
		{
			this.state = CheckState(state);
			this.type = CheckType(type);
			base.eyeType = eyeType;
			base.mouthType = mouthType;
			base.eyeOpen = eyeOpen;
			base.mouthOpen = mouthOpen;
		}

		private static H_Expression.STATE CheckState(string str)
		{
			H_Expression.STATE result = (H_Expression.STATE)0;
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

		private static H_Expression.TYPE CheckType(string str)
		{
			for (int i = 0; i < TypeStrs.Length; i++)
			{
				if (str == TypeStrs[i])
				{
					return (H_Expression.TYPE)i;
				}
			}
			Debug.LogError("不明な種類:" + str);
			return H_Expression.TYPE.NUM;
		}
	}

	private static readonly string[] StateStrs = new string[6] { "抵抗", "豹変", "脱力", "アヘ", "抵抗痛がり", "豹変痛がり" };

	private static readonly H_Expression.STATE[] States = new H_Expression.STATE[6]
	{
		H_Expression.STATE.RESIST,
		H_Expression.STATE.FLIP,
		H_Expression.STATE.WEAKNESS,
		H_Expression.STATE.AHE,
		(H_Expression.STATE)5,
		(H_Expression.STATE)6
	};

	private static readonly string[] TypeStrs = new string[42]
	{
		"呼吸", "台詞", "喘ぎ台詞弱", "喘ぎ台詞強", "喘ぎ弱ハズレ", "喘ぎ弱アタリ", "喘ぎ強ハズレ", "喘ぎ強アタリ", "舐め弱性癖なし", "舐め弱性癖あり",
		"舐め強性癖なし", "舐め強性癖あり", "フェラ弱性癖なし", "フェラ弱性癖あり", "フェラ強性癖なし", "フェラ強性癖あり", "イラマ弱性癖なし", "イラマ弱性癖あり", "イラマ強性癖なし", "イラマ強性癖あり",
		"手コキ弱", "手コキ強", "挿入", "フェラ挿入", "イラマ挿入", "中出し性癖あり", "中出し性癖なし", "外出し性癖あり", "外出し性癖なし", "口内射精性癖あり",
		"口内射精性癖なし", "絶頂", "絶頂後呼吸", "中出し後呼吸", "外出し後呼吸", "抜き", "フェラ抜き", "イラマ抜き", "咳き込み", "飲み込み",
		"吐き出し", "口汁見せ"
	};

	private static readonly string[] TypeStrs_AddHit = new string[2] { "喘ぎ弱", "喘ぎ強" };

	private static readonly string[] TypeStrs_AddBadge = new string[9] { "舐め弱", "舐め強", "フェラ弱", "フェラ強", "イラマ弱", "イラマ強", "中出し", "外出し", "口内射精" };

	private List<Data_Type> type_datas = new List<Data_Type>();

	private Dictionary<string, Data_Voice_Set> voice_datas = new Dictionary<string, Data_Voice_Set>();

	public H_ExpressionData(H_Expression owner, TextAsset loadList)
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
			if (CheckCreateTypeData_Hit(type))
			{
				type_datas.Add(new Data_Type(array[i], type + "アタリ", eyeType, mouthType, eyeOpen, mouthOpen));
				type_datas.Add(new Data_Type(array[i], type + "ハズレ", eyeType, mouthType, eyeOpen, mouthOpen));
			}
			else if (CheckCreateTypeData_Badge(type))
			{
				type_datas.Add(new Data_Type(array[i], type + "性癖あり", eyeType, mouthType, eyeOpen, mouthOpen));
				type_datas.Add(new Data_Type(array[i], type + "性癖なし", eyeType, mouthType, eyeOpen, mouthOpen));
			}
			else
			{
				type_datas.Add(new Data_Type(array[i], type, eyeType, mouthType, eyeOpen, mouthOpen));
			}
		}
	}

	private bool CheckCreateTypeData_Hit(string state)
	{
		for (int i = 0; i < TypeStrs_AddHit.Length; i++)
		{
			if (state == TypeStrs_AddHit[i])
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckCreateTypeData_Badge(string state)
	{
		for (int i = 0; i < TypeStrs_AddBadge.Length; i++)
		{
			if (state == TypeStrs_AddBadge[i])
			{
				return true;
			}
		}
		return false;
	}

	private void AddVoiceData(Data_Voice data)
	{
		if (!voice_datas.ContainsKey(data.voice))
		{
			voice_datas.Add(data.voice, new Data_Voice_Set());
		}
		voice_datas[data.voice].Add(data);
	}

	public Data ChangeExpression(Human human, string voiceFile, H_Parameter param, float duration)
	{
		Data data = null;
		if (!voice_datas.ContainsKey(voiceFile))
		{
			return null;
		}
		Data_Voice_Set data_Voice_Set = voice_datas[voiceFile];
		int no = global::UnityEngine.Random.Range(0, data_Voice_Set.Count);
		data = data_Voice_Set[no];
		if (data != null)
		{
			human.ExpressionPlay(0, data.eyeType, duration);
			human.blink.LimitMax = data.eyeOpen * 0.01f;
			human.lipSync.RelaxOpen = data.mouthOpen * 0.01f;
			bool flag = param.UseMouth(human.sex);
			string name = MouthNo(human, data, param);
			human.ExpressionPlay(1, name, duration);
			human.lipSync.Hold = flag || human.Gag;
			human.head.ResetMouth(flag || human.Gag);
			TONGUE_TYPE tongueType = (flag ? TONGUE_TYPE.BODY : TONGUE_TYPE.FACE);
			human.SetTongueType(tongueType);
		}
		return data;
	}

	public Data ChangeExpression(Human human, H_Expression.TYPE type, H_Parameter param, float duration)
	{
		Data data = null;
		List<Data> list = new List<Data>();
		H_Expression.STATE sTATE = (H_Expression.STATE)0;
		H_Expression.STATE sTATE2 = (H_Expression.STATE)0;
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			sTATE = ((!female.IsFloped()) ? H_Expression.STATE.RESIST : H_Expression.STATE.FLIP);
			sTATE2 = sTATE;
			if (female.personality.ahe)
			{
				sTATE2 = H_Expression.STATE.AHE;
			}
			else if (female.personality.weakness)
			{
				sTATE2 = H_Expression.STATE.WEAKNESS;
			}
			else if (param.style != null && param.style.type == H_StyleData.TYPE.INSERT)
			{
				if (((uint)param.style.detailFlag & 0x10u) != 0 && female.personality.vaginaVirgin)
				{
					sTATE2 |= H_Expression.STATE.PAIN;
				}
				if (((uint)param.style.detailFlag & 0x20u) != 0 && female.personality.analVirgin)
				{
					sTATE2 |= H_Expression.STATE.PAIN;
				}
			}
		}
		foreach (Data_Type type_data in type_datas)
		{
			if (type_data.state == sTATE2 && type_data.type == type)
			{
				list.Add(type_data);
			}
		}
		if (sTATE2 != sTATE && list.Count == 0)
		{
			foreach (Data_Type type_data2 in type_datas)
			{
				if (type_data2.state == sTATE && type_data2.type == type)
				{
					list.Add(type_data2);
				}
			}
		}
		if (list.Count > 0)
		{
			int index = global::UnityEngine.Random.Range(0, list.Count);
			data = list[index];
		}
		if (data != null)
		{
			human.ExpressionPlay(0, data.eyeType, duration);
			human.blink.LimitMax = data.eyeOpen * 0.01f;
			human.lipSync.RelaxOpen = data.mouthOpen * 0.01f;
			bool flag = param.UseMouth(human.sex);
			string name = MouthNo(human, data, param);
			human.ExpressionPlay(1, name, duration);
			human.lipSync.Hold = flag || human.Gag;
			human.head.ResetMouth(flag || human.Gag);
			TONGUE_TYPE tongueType = (flag ? TONGUE_TYPE.BODY : TONGUE_TYPE.FACE);
			human.SetTongueType(tongueType);
		}
		else
		{
			Debug.LogError("ふさわしい表情がなかった:" + sTATE2);
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
			return "Mouth_Kiss_a";
		}
		if (param.mouth == H_MOUTH.INMOUTH)
		{
			return "Mouth_Fera_a";
		}
		if (param.mouth == H_MOUTH.LICK)
		{
			return "Mouth_Name_new";
		}
		return data.mouthType;
	}
}
