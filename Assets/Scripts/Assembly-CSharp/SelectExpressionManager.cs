using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

internal class SelectExpressionManager
{
	private class Data
	{
		public string chara;

		public string state;

		public string eyeType;

		public float eyeOpen;

		public string mouthType;

		public float mouthOpen;

		public Data(string chara, string state, string eyeType, float eyeOpen, string mouthType, float mouthOpen)
		{
			this.chara = chara;
			this.state = state;
			this.eyeType = eyeType;
			this.eyeOpen = eyeOpen;
			this.mouthType = mouthType;
			this.mouthOpen = mouthOpen;
		}

		public bool Check(Female female)
		{
			string text = ((!female.IsFloped()) ? "抵抗" : "豹変");
			return text == state;
		}

		public bool Check(Male male)
		{
			return true;
		}
	}

	private class DataSet
	{
		public string chara;

		public List<Data> datas = new List<Data>();

		public DataSet(string chara)
		{
			this.chara = chara;
		}

		public void Add(Data data)
		{
			datas.Add(data);
		}

		public void Change(Female female)
		{
			List<Data> list = new List<Data>();
			for (int i = 0; i < datas.Count; i++)
			{
				if (datas[i].Check(female))
				{
					list.Add(datas[i]);
				}
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			ChangeExpression(female, list[index]);
		}

		public void Change(Male male)
		{
			List<Data> list = new List<Data>();
			for (int i = 0; i < datas.Count; i++)
			{
				if (datas[i].Check(male))
				{
					list.Add(datas[i]);
				}
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			ChangeExpression(male, list[index]);
		}

		private static void ChangeExpression(Human human, Data exp)
		{
			human.ExpressionPlay(0, exp.eyeType, 0f);
			human.ExpressionPlay(1, exp.mouthType, 0f);
			human.OpenEye(exp.eyeOpen);
			human.OpenMouth(exp.mouthOpen);
		}
	}

	private ExpressionNameSet setF;

	private ExpressionNameSet setM;

	private Dictionary<string, DataSet> dataSets = new Dictionary<string, DataSet>();

	public void Load(TextAsset eyeF, TextAsset mouthF, TextAsset eyeM, TextAsset mouthM, TextAsset loadList)
	{
		setF = new ExpressionNameSet(eyeF, mouthF);
		setM = new ExpressionNameSet(eyeM, mouthM);
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(loadList);
		int attributeNo = customDataListLoader.GetAttributeNo("キャラ");
		int attributeNo2 = customDataListLoader.GetAttributeNo("状態");
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
			float eyeOpen = float.Parse(customDataListLoader.GetData(attributeNo5, i)) * 0.01f;
			float mouthOpen = float.Parse(customDataListLoader.GetData(attributeNo6, i)) * 0.01f;
			if (data == "主人公" || data == "広一")
			{
				data3 = setM.EyeName(data3);
				data4 = setM.MouthName(data4);
			}
			else
			{
				data3 = setF.EyeName(data3);
				data4 = setF.MouthName(data4);
			}
			DataSet dataSet = null;
			if (dataSets.ContainsKey(data))
			{
				dataSet = dataSets[data];
			}
			else
			{
				dataSet = new DataSet(data);
				dataSets.Add(data, dataSet);
			}
			dataSet.Add(new Data(data, data2, data3, eyeOpen, data4, mouthOpen));
		}
	}

	public void Change(Human human)
	{
		string empty = string.Empty;
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			empty = Female.HeroineName(female.HeroineID);
			dataSets[empty].Change(female);
		}
		else
		{
			Male male = human as Male;
			empty = ((male.MaleID != MALE_ID.KOUICHI) ? "主人公" : "広一");
			dataSets[empty].Change(male);
		}
	}
}
