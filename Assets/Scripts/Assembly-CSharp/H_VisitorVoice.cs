using System;
using System.Collections.Generic;
using Character;
using H;
using UnityEngine;

public class H_VisitorVoice : MonoBehaviour
{
	public enum TYPE
	{
		BREATH = 0,
		ACT = 1,
		AFTER = 2,
		LIQUID = 3,
		GENERAL = 4,
		NUM = 5
	}

	public enum STATE
	{
		FIRST = 0,
		RESIST = 1,
		FLOP = 2,
		FLOP_INDECENT = 3,
		NUM = 4
	}

	public enum DETAIL
	{
		NONE = 0,
		GAG = 1,
		INSERT_VAGINA = 2,
		INSERT_ANAL = 3,
		SERVICE = 4,
		IN_MOUTH = 5,
		LICK = 6,
		JOB = 7,
		TO_RITSUKO_RESIST = 8,
		TO_RITSUKO_FLOP = 9,
		TO_AKIKO_RESIST = 10,
		TO_AKIKO_FLOP = 11,
		TO_YUKIKO_RESIST = 12,
		TO_YUKIKO_FLOP = 13,
		TO_KOUICHI = 14,
		WEAKNESS = 15,
		AHE = 16,
		XTC_GENERAL = 17,
		XTC_VAGINA = 18,
		XTC_ANAL = 19,
		EJA_IN_V_A = 20,
		EJA_IN_VAGINA = 21,
		EJA_IN_ANAL = 22,
		OUT_EJA = 23,
		EJA_IN_MOUTH = 24,
		PRIOLITY = 25,
		BAD = 26,
		SOSO = 27,
		GOOD = 28,
		NUM = 29
	}

	public class Data
	{
		public TYPE type;

		public STATE state;

		public DETAIL detail;

		public string file;

		public Data(TYPE type, STATE state, DETAIL detail, string file)
		{
			this.type = type;
			this.state = state;
			this.detail = detail;
			this.file = file;
		}

		public bool Check(Female female, TYPE type, H_Members members)
		{
			if (!CheckType(type))
			{
				return false;
			}
			if (!CheckState(female, state))
			{
				return false;
			}
			if (!CheckDetail(female, members))
			{
				return false;
			}
			return true;
		}

		private bool CheckType(TYPE type)
		{
			if (this.type == type)
			{
				return true;
			}
			if ((type == TYPE.ACT || type == TYPE.AFTER) && this.type == TYPE.GENERAL)
			{
				return true;
			}
			return false;
		}

		private bool CheckState(Female female, STATE state)
		{
			switch (state)
			{
			case STATE.FIRST:
				return female.personality.state == Personality.STATE.FIRST;
			case STATE.RESIST:
				return !female.IsFloped();
			case STATE.FLOP:
				return female.IsFloped() && !female.personality.indecentLanguage;
			case STATE.FLOP_INDECENT:
				return female.IsFloped() && female.personality.indecentLanguage;
			default:
				return false;
			}
		}

		private bool CheckDetail(Female female, H_Members members)
		{
			H_StyleData.TYPE tYPE = ((members.StyleData == null) ? H_StyleData.TYPE.NUM : members.StyleData.type);
			int num = ((members.StyleData != null) ? members.StyleData.detailFlag : 0);
			XTC_TYPE xtcType = members.param.xtcType;
			if (detail == DETAIL.GAG)
			{
				return female.Gag;
			}
			if (female.Gag)
			{
				return false;
			}
			if (detail == DETAIL.INSERT_VAGINA)
			{
				return tYPE == H_StyleData.TYPE.INSERT && (num & 0x10) != 0;
			}
			if (detail == DETAIL.INSERT_ANAL)
			{
				return tYPE == H_StyleData.TYPE.INSERT && (num & 0x20) != 0;
			}
			if (detail == DETAIL.SERVICE)
			{
				return tYPE == H_StyleData.TYPE.SERVICE;
			}
			if (detail == DETAIL.IN_MOUTH)
			{
				return tYPE == H_StyleData.TYPE.SERVICE && (num & H_StyleData.DetailMasking_InMouth) != 0;
			}
			if (detail == DETAIL.LICK)
			{
				return tYPE == H_StyleData.TYPE.SERVICE && (num & 2) != 0;
			}
			if (detail == DETAIL.JOB)
			{
				return tYPE == H_StyleData.TYPE.SERVICE && (num & H_StyleData.DetailMasking_UseMouth) == 0;
			}
			if (detail == DETAIL.TO_RITSUKO_RESIST)
			{
				List<Female> females = members.GetFemales();
				for (int i = 0; i < females.Count; i++)
				{
					if (females[i].HeroineID == HEROINE.RITSUKO && !females[i].IsFloped())
					{
						return true;
					}
				}
				return false;
			}
			if (detail == DETAIL.TO_RITSUKO_FLOP)
			{
				List<Female> females2 = members.GetFemales();
				for (int j = 0; j < females2.Count; j++)
				{
					if (females2[j].HeroineID == HEROINE.RITSUKO && females2[j].IsFloped())
					{
						return true;
					}
				}
				return false;
			}
			if (detail == DETAIL.TO_AKIKO_RESIST)
			{
				List<Female> females3 = members.GetFemales();
				for (int k = 0; k < females3.Count; k++)
				{
					if (females3[k].HeroineID == HEROINE.AKIKO && !females3[k].IsFloped())
					{
						return true;
					}
				}
				return false;
			}
			if (detail == DETAIL.TO_AKIKO_FLOP)
			{
				List<Female> females4 = members.GetFemales();
				for (int l = 0; l < females4.Count; l++)
				{
					if (females4[l].HeroineID == HEROINE.AKIKO && females4[l].IsFloped())
					{
						return true;
					}
				}
				return false;
			}
			if (detail == DETAIL.TO_YUKIKO_RESIST)
			{
				List<Female> females5 = members.GetFemales();
				for (int m = 0; m < females5.Count; m++)
				{
					if (females5[m].HeroineID == HEROINE.YUKIKO && !females5[m].IsFloped())
					{
						return true;
					}
				}
				return false;
			}
			if (detail == DETAIL.TO_YUKIKO_FLOP)
			{
				List<Female> females6 = members.GetFemales();
				for (int n = 0; n < females6.Count; n++)
				{
					if (females6[n].HeroineID == HEROINE.YUKIKO && females6[n].IsFloped())
					{
						return true;
					}
				}
				return false;
			}
			if (detail == DETAIL.TO_KOUICHI)
			{
				List<Male> males = members.GetMales();
				for (int num2 = 0; num2 < males.Count; num2++)
				{
					if (males[num2].MaleID == MALE_ID.KOUICHI)
					{
						return true;
					}
				}
				return false;
			}
			if (detail == DETAIL.WEAKNESS)
			{
				return members.GetFemale(0).personality.weakness;
			}
			if (detail == DETAIL.AHE)
			{
				return members.GetFemale(0).personality.ahe;
			}
			if (detail == DETAIL.XTC_GENERAL)
			{
				return xtcType == XTC_TYPE.XTC_F || xtcType == XTC_TYPE.XTC_W;
			}
			if (detail == DETAIL.XTC_VAGINA)
			{
				return (xtcType == XTC_TYPE.XTC_F || xtcType == XTC_TYPE.XTC_W) && (num & 0x10) != 0;
			}
			if (detail == DETAIL.XTC_ANAL)
			{
				return (xtcType == XTC_TYPE.XTC_F || xtcType == XTC_TYPE.XTC_W) && (num & 0x20) != 0;
			}
			if (detail == DETAIL.EJA_IN_V_A)
			{
				return (xtcType == XTC_TYPE.EJA_IN || xtcType == XTC_TYPE.XTC_W) && tYPE == H_StyleData.TYPE.INSERT;
			}
			if (detail == DETAIL.EJA_IN_VAGINA)
			{
				return (xtcType == XTC_TYPE.EJA_IN || xtcType == XTC_TYPE.XTC_W) && (num & 0x10) != 0;
			}
			if (detail == DETAIL.EJA_IN_ANAL)
			{
				return (xtcType == XTC_TYPE.EJA_IN || xtcType == XTC_TYPE.XTC_W) && (num & 0x20) != 0;
			}
			if (detail == DETAIL.OUT_EJA)
			{
				return members.param.xtcType == XTC_TYPE.EJA_OUT;
			}
			if (detail == DETAIL.EJA_IN_MOUTH)
			{
				return xtcType == XTC_TYPE.EJA_IN && (num & H_StyleData.DetailMasking_InMouth) != 0;
			}
			if (detail == DETAIL.PRIOLITY)
			{
				return true;
			}
			if (detail == DETAIL.BAD)
			{
				return !female.IsFloped() && !female.personality.likeSperm;
			}
			if (detail == DETAIL.SOSO)
			{
				if (!female.IsFloped() && female.personality.likeSperm)
				{
					return true;
				}
				if (female.IsFloped() && !female.personality.likeSperm)
				{
					return true;
				}
				return false;
			}
			if (detail == DETAIL.GOOD)
			{
				return female.IsFloped() && female.personality.likeSperm;
			}
			return true;
		}
	}

	private static readonly string[] HeroinePrefix = new string[3] { "r", "a", "y" };

	private static readonly string[] FileStateNo = new string[4] { "90", "00", "10", "11" };

	private static readonly string[] TypeNames = new string[5] { "呼吸", "行為", "事後", "かけられ", "汎用" };

	private static readonly string[] DetailNames = new string[29]
	{
		"-", "口塞がれ", "性器挿入", "アナル挿入", "奉仕", "咥え", "舐め", "コキ", "対律子（抵抗）", "対律子（豹変）",
		"対明子（抵抗）", "対明子（豹変）", "対雪子（抵抗）", "対雪子（豹変）", "対広一", "脱力", "アヘ", "絶頂（汎用）", "絶頂（性器）", "絶頂（アナル）",
		"中出し（性器,アナル）", "中出し（性器）", "中出し（アナル）", "外出し", "口内射精", "優先", "悔しい", "それなり", "嬉しい"
	};

	[SerializeField]
	private TextAsset voiceList;

	private List<Data> datas = new List<Data>();

	public void Setup()
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(voiceList);
		int attributeNo = customDataListLoader.GetAttributeNo("タイプ");
		int attributeNo2 = customDataListLoader.GetAttributeNo("詳細");
		int attributeNo3 = customDataListLoader.GetAttributeNo("ファイル");
		int attributeNo4 = customDataListLoader.GetAttributeNo("初回");
		int attributeNo5 = customDataListLoader.GetAttributeNo("抵抗");
		int attributeNo6 = customDataListLoader.GetAttributeNo("豹変");
		int attributeNo7 = customDataListLoader.GetAttributeNo("豹変淫語");
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			string data3 = customDataListLoader.GetData(attributeNo3, i);
			string data4 = customDataListLoader.GetData(attributeNo4, i);
			string data5 = customDataListLoader.GetData(attributeNo5, i);
			string data6 = customDataListLoader.GetData(attributeNo6, i);
			string data7 = customDataListLoader.GetData(attributeNo7, i);
			TYPE type = (TYPE)StringsCheck(TypeNames, data);
			DETAIL detail = (DETAIL)StringsCheck(DetailNames, data2);
			int num = Number(data4);
			int num2 = Number(data5);
			int num3 = Number(data6);
			int num4 = Number(data7);
			AddData(type, detail, data3, STATE.FIRST, FileStateNo[0], num);
			AddData(type, detail, data3, STATE.RESIST, FileStateNo[1], num2);
			AddData(type, detail, data3, STATE.FLOP, FileStateNo[2], num3);
			if (num4 > 0)
			{
				AddData(type, detail, data3, STATE.FLOP_INDECENT, FileStateNo[3], num4);
			}
			else
			{
				AddData(type, detail, data3, STATE.FLOP_INDECENT, FileStateNo[2], num3);
			}
		}
	}

	private int Number(string check)
	{
		if (check == "-")
		{
			return -1;
		}
		int result = -1;
		try
		{
			result = int.Parse(check);
		}
		catch
		{
			Debug.LogError("数字変換失敗:" + check);
		}
		return result;
	}

	private void AddData(TYPE type, DETAIL detail, string fileBase, STATE state, string stateNoStr, int num)
	{
		for (int i = 0; i < num; i++)
		{
			string text = fileBase.Replace("yy", stateNoStr);
			int num2 = text.LastIndexOf("_");
			string s = text.Substring(num2 + 1);
			int num3 = int.Parse(s);
			text = text.Substring(0, num2 + 1);
			text += (num3 + i).ToString("00");
			datas.Add(new Data(type, state, detail, text));
		}
	}

	private static int StringsCheck(string[] strings, string check)
	{
		for (int i = 0; i < strings.Length; i++)
		{
			if (check == strings[i])
			{
				return i;
			}
		}
		return -1;
	}

	public Data Voice(Female female, H_VoiceLog log, AssetBundleController voiceABC, TYPE type, H_Members members)
	{
		Data data = GetData(female, log, type, members);
		if (data != null)
		{
			string fileName = GetFileName(female.HeroineID, data);
			AudioClip audioClip = voiceABC.LoadAsset<AudioClip>(fileName);
			if (audioClip == null)
			{
				Debug.LogError("音声クリップがない：" + fileName);
			}
			female.PhonationVoice(audioClip, false);
			bool flag = false;
			if (data.type == TYPE.ACT || data.type == TYPE.AFTER)
			{
				flag = true;
			}
			else if (data.detail == DETAIL.PRIOLITY)
			{
				flag = true;
			}
			if (flag)
			{
				log.AddPriorityTalk(data.file);
			}
			else
			{
				log.AddPant(data.file);
			}
			return data;
		}
		return null;
	}

	private Data GetData(Female female, H_VoiceLog log, TYPE type, H_Members members)
	{
		Data result = null;
		List<Data> list = new List<Data>();
		foreach (Data data in datas)
		{
			if (data.Check(female, type, members))
			{
				list.Add(data);
			}
		}
		if (list.Count > 0)
		{
			log.Check(list);
			if (list.Count == 0)
			{
				return null;
			}
			CheckPriority(list);
			int index = UnityEngine.Random.Range(0, list.Count);
			result = list[index];
		}
		return result;
	}

	private void CheckPriority(List<Data> datas)
	{
		bool flag = false;
		for (int i = 0; i < datas.Count; i++)
		{
			if (datas[i].detail == DETAIL.PRIOLITY)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		for (int j = 0; j < datas.Count; j++)
		{
			if (datas[j].detail != DETAIL.PRIOLITY)
			{
				datas.RemoveAt(j);
				j--;
			}
		}
	}

	private string GetFileName(HEROINE heroine, Data data)
	{
		if (data != null)
		{
			return HeroinePrefix[(int)heroine] + "_" + data.file;
		}
		return string.Empty;
	}
}
