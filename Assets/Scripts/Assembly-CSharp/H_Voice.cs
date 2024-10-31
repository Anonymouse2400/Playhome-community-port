using System;
using System.Collections.Generic;
using Character;
using H;
using UnityEngine;

public class H_Voice : MonoBehaviour
{
	public enum TYPE
	{
		BREATH = 0,
		START = 1,
		LEAVING = 2,
		INSERT = 3,
		ACT = 4,
		XTC_OMEN_F = 5,
		XTC_OMEN_M = 6,
		XTC_F = 7,
		XTC_M = 8,
		COUGH = 9,
		DRINK = 10,
		VOMIT = 11,
		SHOW_ORAL = 12,
		FALL_LIQUID = 13,
		INCONTINENCE = 14,
		XTC_AFTER_TALK = 15,
		XTC_AFTER_BREATH = 16,
		EXIT = 17,
		ACT_TALK = 18,
		CHANGE_STYLE = 19,
		NUM = 20
	}

	public enum STATE
	{
		ANY = 0,
		FIRST = 1,
		RESIST = 2,
		FLOP = 4,
		WEAKNESS = 8,
		AHE = 16,
		FIRST_WEAKNESS = 32,
		RESIST_WEAKNESS = 64,
		FLOP_WEAKNESS = 128,
		RESIST_INDECENT = 256,
		FLOP_INDECENT = 512,
		FLIP_FLOP = 1024,
		LAST_EVENT_SISTERS = 2048,
		LAST_EVENT_YUKIKO_1 = 4096,
		LAST_EVENT_YUKIKO_2 = 8192,
		NUM = 14
	}

	public enum KIND
	{
		TALK = 0,
		BREATH = 1,
		PANT = 2,
		AHE = 3,
		NUM = 4
	}

	public enum ACTION
	{
		ANY = 0,
		VAGINA = 1,
		ANAL = 2,
		INSERT = 4,
		PETTING = 8,
		SERVICE = 16,
		VAGINA_INSERT = 32,
		ANAL_INSERT = 64,
		HAND_JOB = 128,
		TITTY_FUCK = 256,
		PET_BUST = 512,
		ONANIE = 1024,
		VAGINA_ONANIE = 2048,
		ANAL_ONANIE = 4096,
		TOOL = 8192,
		VAGINA_TOOL = 16384,
		ANAL_TOOL = 32768,
		NUM = 16
	}

	public enum DETAIL : long
	{
		INITIATIVE_MALE = 0L,
		INITIATIVE_FEMALE = 1L,
		FELLATIO = 2L,
		IRRUMATIO = 3L,
		HIT = 4L,
		PAIN = 5L,
		MANY_EJACULATION = 6L,
		VAGINA_VIRGIN = 7L,
		ANAL_VIRGIN = 8L,
		XTC_FEMALE = 9L,
		EJACULATION_MALE = 10L,
		EJACULATION_IN = 11L,
		EJACULATION_OUT = 12L,
		COUGH = 13L,
		DRINK = 14L,
		VOMIT = 15L,
		SHOW_ORAL = 16L,
		BADGE_FEEL_VAGINA = 17L,
		BADGE_FEEL_ANUS = 18L,
		BADGE_LIKE_SPERM = 19L,
		BADGE_LIKE_FERATIO = 20L,
		TO_MULTI = 21L,
		TO_KOUICHI = 22L,
		FEMALE_HIT = 23L,
		FEMALE_NOHIT = 24L,
		MALE_HIT = 25L,
		MALE_NOHIT = 26L,
		ENDCHECK_MANY_XTC = 27L,
		ENDCHECK_MANY_XTC_VAGINA = 28L,
		ENDCHECK_MANY_XTC_ANUS = 29L,
		ENDCHECK_ONE_XTC = 30L,
		ENDCHECK_MANY_EJA = 31L,
		ENDCHECK_MANY_EJA_VAGINA = 32L,
		ENDCHECK_MANY_EJA_ANUS = 33L,
		ENDCHECK_ONE_EJA = 34L,
		ENDCHECK_TO_KOUICHI = 35L,
		ENDCHECK_NO_ACTION = 36L,
		FORMER_ONLOOKER = 37L,
		THERE_IS_ONLOOKER = 38L,
		MAP_MYROOM = 39L,
		MAP_GARDEN = 40L,
		MAP_TOILET = 41L,
		MAP_NARROW = 42L,
		WANT_INSERT_VAGINA = 43L,
		WANT_INSERT_ANUS = 44L,
		WANT_INSERT_STYLE = 45L,
		YOU_INSERT_ME = 46L,
		LIKE_ACTION = 47L,
		APPROACH_ONLOOKER = 48L,
		NUM = 49L
	}

	public class Data
	{
		public TYPE type { get; private set; }

		public KIND kind { get; private set; }

		public STATE state { get; private set; }

		public H_MOUTH mouth { get; private set; }

		public H_SPEED speed { get; private set; }

		public ACTION action { get; private set; }

		public bool[] details { get; private set; }

		public int priority { get; private set; }

		public string File { get; private set; }

		public Data(string typeStr, string stateStr, string mouthStr, string speedStr, string actionStr, string detailStr, string priorityStr, string kindStr, string fileStr)
		{
			type = (TYPE)StringsCheck(TypeNames, typeStr);
			state = SetupState(stateStr);
			mouth = SetupMouth(mouthStr);
			speed = SetupSpeed(speedStr);
			action = SetupAction(actionStr);
			details = new bool[49];
			SetupDetail(details, detailStr);
			if (priorityStr == "-")
			{
				priority = -1;
			}
			else
			{
				priority = int.Parse(priorityStr);
			}
			kind = (KIND)StringsCheck(KindNames, kindStr);
			File = fileStr;
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

		private static void SetupDetail(bool[] details, string str)
		{
			for (int i = 0; i < details.Length; i++)
			{
				details[i] = false;
			}
			string[] array = str.Split(new char[1] { '+' }, StringSplitOptions.RemoveEmptyEntries);
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == "-")
				{
					continue;
				}
				int num = 0;
				for (num = 0; num < DetailNames.Length; num++)
				{
					if (array[j] == DetailNames[num])
					{
						details[num] |= true;
						break;
					}
				}
				if (num == DetailNames.Length)
				{
					Debug.LogError("不明な詳細条件：" + array[j]);
				}
			}
		}

		private static H_MOUTH SetupMouth(string str)
		{
			H_MOUTH h_MOUTH = H_MOUTH.FREE;
			string[] array = str.Split(',');
			int[] array2 = new int[5] { 0, 1, 2, 4, 8 };
			for (int i = 0; i < array.Length; i++)
			{
				int num = StringsCheck(MouthNames, array[i]);
				h_MOUTH = (H_MOUTH)((int)h_MOUTH | array2[num]);
			}
			return h_MOUTH;
		}

		private static STATE SetupState(string str)
		{
			if (str == "-")
			{
				return STATE.ANY;
			}
			string[] array = str.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			STATE sTATE = STATE.ANY;
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				for (num = 0; num < StateNames.Length; num++)
				{
					if (array[i] == StateNames[num])
					{
						sTATE = (STATE)((int)sTATE | (1 << num));
						break;
					}
				}
				if (num == StateNames.Length)
				{
					Debug.LogError("不明な速度：" + array[i]);
				}
			}
			return sTATE;
		}

		private static H_SPEED SetupSpeed(string str)
		{
			if (str == "-")
			{
				return H_SPEED.ALL;
			}
			string[] array = str.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			H_SPEED h_SPEED = (H_SPEED)0;
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				for (num = 0; num < SpeedNames.Length; num++)
				{
					if (array[i] == SpeedNames[num])
					{
						h_SPEED = (H_SPEED)((int)h_SPEED | (1 << num));
						break;
					}
				}
				if (num == SpeedNames.Length)
				{
					Debug.LogError("不明な速度：" + array[i]);
				}
			}
			return h_SPEED;
		}

		private static ACTION SetupAction(string str)
		{
			if (str == "-")
			{
				return ACTION.ANY;
			}
			string[] array = str.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			ACTION aCTION = ACTION.ANY;
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				for (num = 0; num < ActionNames.Length; num++)
				{
					if (array[i] == ActionNames[num])
					{
						aCTION = (ACTION)((int)aCTION | (1 << num));
						break;
					}
				}
				if (num == ActionNames.Length)
				{
					Debug.LogError("不明な行為：" + array[i]);
				}
			}
			return aCTION;
		}

		public bool Check(Female female, TYPE type, H_Members members)
		{
			if (this.type != type)
			{
				return false;
			}
			if (!CheckMouth(female, members.param))
			{
				return false;
			}
			if (!CheckState(female, members.param))
			{
				return false;
			}
			if (!CheckSpeed(members.param))
			{
				return false;
			}
			if (!CheckAction(members.param))
			{
				return false;
			}
			if (!CheckDetail(female, members))
			{
				return false;
			}
			return true;
		}

		private bool CheckMouth(Female female, H_Parameter param)
		{
			H_MOUTH h_MOUTH = ((param.style != null && param.style.type != H_StyleData.TYPE.PETTING) ? param.mouth : H_MOUTH.FREE);
			if (female.Gag)
			{
				h_MOUTH = H_MOUTH.MASK;
			}
			if (mouth == H_MOUTH.FREE)
			{
				return h_MOUTH == H_MOUTH.FREE;
			}
			bool flag = false;
			if ((mouth & H_MOUTH.MASK) != 0)
			{
				flag |= female.Gag;
			}
			return flag | ((mouth & h_MOUTH) != 0);
		}

		private bool CheckSpeed(H_Parameter param)
		{
			return (speed & param.speed) != 0;
		}

		private bool CheckState(Female female, H_Parameter param)
		{
			if (state == STATE.ANY)
			{
				return true;
			}
			if ((state & STATE.FIRST) != 0 && female.personality.state == Personality.STATE.FIRST)
			{
				return true;
			}
			if ((state & STATE.RESIST) != 0 && female.personality.state != 0 && !female.IsFloped())
			{
				return true;
			}
			if ((state & STATE.FLOP) != 0 && female.IsFloped() && !female.personality.ahe)
			{
				return true;
			}
			if ((state & STATE.WEAKNESS) != 0 && female.personality.weakness)
			{
				return true;
			}
			if ((state & STATE.AHE) != 0 && female.personality.ahe)
			{
				return true;
			}
			if ((state & STATE.FIRST_WEAKNESS) != 0 && female.personality.state == Personality.STATE.FIRST && female.personality.weakness)
			{
				return true;
			}
			if ((state & STATE.RESIST_WEAKNESS) != 0 && female.personality.state != 0 && !female.IsFloped() && female.personality.weakness)
			{
				return true;
			}
			if ((state & STATE.FLOP_WEAKNESS) != 0 && female.IsFloped() && female.personality.weakness)
			{
				return true;
			}
			if ((state & STATE.RESIST_INDECENT) != 0 && !female.IsFloped() && female.personality.indecentLanguage)
			{
				return true;
			}
			if ((state & STATE.FLOP_INDECENT) != 0 && female.IsFloped() && female.personality.indecentLanguage && !female.personality.ahe)
			{
				return true;
			}
			if ((state & STATE.FLIP_FLOP) != 0 && female.personality.state == Personality.STATE.FLIP_FLOP)
			{
				return true;
			}
			if ((state & STATE.LAST_EVENT_SISTERS) != 0 && female.personality.state == Personality.STATE.LAST_EVENT_SISTERS)
			{
				return true;
			}
			if ((state & STATE.LAST_EVENT_YUKIKO_1) != 0 && female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_1)
			{
				return true;
			}
			if ((state & STATE.LAST_EVENT_YUKIKO_2) != 0 && female.personality.state == Personality.STATE.LAST_EVENT_YUKIKO_2)
			{
				return true;
			}
			return false;
		}

		private bool CheckAction(H_Parameter param)
		{
			if (action == ACTION.ANY)
			{
				return true;
			}
			if ((action & ACTION.VAGINA) != 0 && ((uint)param.style.detailFlag & 0x10u) != 0)
			{
				return true;
			}
			if ((action & ACTION.ANAL) != 0 && ((uint)param.style.detailFlag & 0x20u) != 0)
			{
				return true;
			}
			if ((action & ACTION.INSERT) != 0 && param.style.type == H_StyleData.TYPE.INSERT)
			{
				return true;
			}
			if ((action & ACTION.PETTING) != 0 && param.style.type == H_StyleData.TYPE.PETTING)
			{
				return true;
			}
			if ((action & ACTION.SERVICE) != 0 && param.style.type == H_StyleData.TYPE.SERVICE)
			{
				return true;
			}
			if ((action & ACTION.VAGINA_INSERT) != 0 && param.style.type == H_StyleData.TYPE.INSERT && ((uint)param.style.detailFlag & 0x10u) != 0)
			{
				return true;
			}
			if ((action & ACTION.ANAL_INSERT) != 0 && param.style.type == H_StyleData.TYPE.INSERT && ((uint)param.style.detailFlag & 0x20u) != 0)
			{
				return true;
			}
			if ((action & ACTION.HAND_JOB) != 0 && param.style.type == H_StyleData.TYPE.SERVICE && ((uint)param.style.detailFlag & 0x40u) != 0)
			{
				return true;
			}
			if ((action & ACTION.TITTY_FUCK) != 0 && param.style.type == H_StyleData.TYPE.SERVICE && ((uint)param.style.detailFlag & 0x80u) != 0)
			{
				return true;
			}
			if ((action & ACTION.PET_BUST) != 0 && param.style.type == H_StyleData.TYPE.PETTING && ((uint)param.style.detailFlag & 0x100u) != 0)
			{
				return true;
			}
			if ((action & ACTION.ONANIE) != 0 && ((uint)param.style.detailFlag & 0x200u) != 0)
			{
				return true;
			}
			if ((action & ACTION.VAGINA_ONANIE) != 0 && ((uint)param.style.detailFlag & 0x200u) != 0 && ((uint)param.style.detailFlag & 0x10u) != 0)
			{
				return true;
			}
			if ((action & ACTION.ANAL_ONANIE) != 0 && ((uint)param.style.detailFlag & 0x200u) != 0 && ((uint)param.style.detailFlag & 0x20u) != 0)
			{
				return true;
			}
			if ((action & ACTION.TOOL) != 0 && ((uint)param.style.detailFlag & 0x400u) != 0)
			{
				return true;
			}
			if ((action & ACTION.VAGINA_TOOL) != 0 && ((uint)param.style.detailFlag & 0x400u) != 0 && ((uint)param.style.detailFlag & 0x10u) != 0)
			{
				return true;
			}
			if ((action & ACTION.ANAL_TOOL) != 0 && ((uint)param.style.detailFlag & 0x400u) != 0 && ((uint)param.style.detailFlag & 0x20u) != 0)
			{
				return true;
			}
			return false;
		}

		private bool CheckDetail(Female female, H_Members members)
		{
			bool[] array = new bool[49];
			CheckDetail(female, members, array);
			for (int i = 0; i < details.Length; i++)
			{
				if (details[i] && !array[i])
				{
					return false;
				}
			}
			return true;
		}

		public static void CheckDetail(Female female, H_Members members, bool[] checkList)
		{
			H_Parameter param = members.param;
			H_StyleData style = param.style;
			List<Male> males = members.GetMales();
			checkList[0] = style != null && style.initiative == H_StyleData.INITIATIVE.MALE;
			checkList[1] = style != null && style.initiative == H_StyleData.INITIATIVE.FEMALE;
			checkList[2] = style != null && (style.detailFlag & 4) != 0;
			checkList[3] = style != null && (style.detailFlag & 8) != 0;
			checkList[4] = param.hitEnableStyle && param.hit;
			checkList[5] = false;
			if (style != null && style.type == H_StyleData.TYPE.INSERT)
			{
				if (((uint)style.detailFlag & 0x10u) != 0 && female.personality.vaginaVirgin)
				{
					checkList[5] = true;
				}
				else if (((uint)style.detailFlag & 0x20u) != 0 && female.personality.analVirgin)
				{
					checkList[5] = true;
				}
			}
			checkList[6] = param.manyEjaculation;
			checkList[7] = female.personality.vaginaVirgin;
			checkList[8] = female.personality.analVirgin;
			checkList[9] = param.xtcType == XTC_TYPE.XTC_W || param.xtcType == XTC_TYPE.XTC_F;
			checkList[10] = param.xtcType != XTC_TYPE.XTC_F;
			checkList[11] = param.xtcType == XTC_TYPE.EJA_IN;
			checkList[12] = param.xtcType == XTC_TYPE.EJA_OUT;
			checkList[13] = (param.detail & H_Parameter.DETAIL.COUGH) != 0;
			checkList[14] = (param.detail & H_Parameter.DETAIL.DRINK) != 0;
			checkList[15] = (param.detail & H_Parameter.DETAIL.VOMIT) != 0;
			checkList[16] = (param.detail & H_Parameter.DETAIL.SHOW_ORAL) != 0;
			checkList[17] = female.personality.feelVagina;
			checkList[18] = female.personality.feelAnus;
			checkList[19] = female.personality.likeSperm;
			checkList[20] = female.personality.likeFeratio;
			checkList[21] = style != null && (style.member == H_StyleData.MEMBER.M2F1 || style.member == H_StyleData.MEMBER.M3F1 || style.member == H_StyleData.MEMBER.M4F1);
			checkList[22] = false;
			for (int i = 0; i < members.GetMales().Count; i++)
			{
				Male male = members.GetMale(i);
				if (male.MaleID == MALE_ID.KOUICHI)
				{
					checkList[22] = true;
					break;
				}
			}
			checkList[23] = style != null && param.hitEnableStyle && style.type != H_StyleData.TYPE.SERVICE && param.hit;
			checkList[24] = style != null && param.hitEnableStyle && style.type != H_StyleData.TYPE.SERVICE && !param.hit;
			checkList[25] = style != null && style.type == H_StyleData.TYPE.SERVICE && param.hit;
			checkList[26] = style != null && style.type == H_StyleData.TYPE.SERVICE && !param.hit;
			checkList[27] = female.personality.xtc_count_vagina + female.personality.xtc_count_anal >= 5;
			checkList[28] = female.personality.xtc_count_vagina >= 5;
			checkList[29] = female.personality.xtc_count_anal >= 5;
			checkList[30] = female.personality.xtc_count_vagina + female.personality.xtc_count_anal == 1;
			checkList[31] = female.personality.eja_count >= 5;
			checkList[32] = female.personality.eja_count_vagina >= 5;
			checkList[33] = female.personality.eja_count_anal >= 5;
			checkList[34] = female.personality.eja_count_vagina + female.personality.eja_count_vagina == 1;
			checkList[35] = false;
			for (int j = 0; j < males.Count; j++)
			{
				if (males[j].MaleID == MALE_ID.KOUICHI)
				{
					checkList[35] = true;
					break;
				}
			}
			checkList[36] = !param.anyAction;
			checkList[37] = param.swapVisitor == 1;
			checkList[38] = members.h_scene.visitor != null;
			checkList[39] = false;
			if (female.HeroineID == HEROINE.RITSUKO && param.mapName.IndexOf("ritsuko") != -1)
			{
				checkList[39] = true;
			}
			else if (female.HeroineID == HEROINE.AKIKO && param.mapName.IndexOf("akiko") != -1)
			{
				checkList[39] = true;
			}
			else if (female.HeroineID == HEROINE.YUKIKO && param.mapName.IndexOf("bedroom") != -1)
			{
				checkList[39] = true;
			}
			checkList[40] = param.mapName.IndexOf("yard") != -1;
			checkList[41] = param.mapName.IndexOf("toilet") != -1;
			checkList[42] = param.mapName.IndexOf("japanese") != -1 && style != null && style.map.Length > 0;
			checkList[43] = female.personality.feelVagina && female.personality.continuousInsVagina <= -3;
			checkList[44] = female.personality.feelAnus && female.personality.continuousInsAnal <= -3;
			checkList[45] = (female.personality.feelVagina && female.personality.continuousInsVagina <= -3) || (female.personality.feelAnus && female.personality.continuousInsAnal <= -3);
			checkList[46] = (female.personality.feelVagina && female.personality.continuousInsVagina >= 3) || (female.personality.feelAnus && female.personality.continuousInsAnal >= 3);
			checkList[47] = false;
			if (style != null)
			{
				if (female.personality.feelVagina && ((uint)style.detailFlag & 0x10u) != 0)
				{
					checkList[47] = true;
				}
				if (female.personality.feelAnus && ((uint)style.detailFlag & 0x20u) != 0)
				{
					checkList[47] = true;
				}
				if (female.personality.likeFeratio && (style.detailFlag & H_StyleData.DetailMasking_InMouth) != 0)
				{
					checkList[47] = true;
				}
			}
			checkList[48] = false;
			float num = float.MaxValue;
			if (members.h_scene.visitor != null)
			{
				Vector3 position = members.h_scene.visitor.GetHuman().transform.position;
				num = Vector3.Distance(position, members.Transform.position);
			}
			checkList[48] = num <= 1.2f;
		}
	}

	private static readonly string[] TypeNames = new string[20]
	{
		"呼吸", "開始", "放置", "挿入", "行為", "女イきそう", "男イきそう", "女絶頂", "男射精", "咳き込み",
		"飲み込み", "吐き出し", "口汁見せ", "汁垂れ", "漏らし", "事後台詞", "事後呼吸", "終了時評価", "行為質問", "体位変更"
	};

	private static readonly string[] StateNames = new string[14]
	{
		"初回", "抵抗", "豹変", "脱力", "アヘ", "初回脱力", "抵抗脱力", "豹変脱力", "抵抗淫語", "豹変淫語",
		"豹変イベント", "姉妹最終イベント", "雪子最終イベント１", "雪子最終イベント２"
	};

	private static readonly string[] HeroinePrefix = new string[3] { "r", "a", "y" };

	private static readonly string[] KindNames = new string[4] { "台詞", "呼吸", "喘ぎ", "アヘ" };

	private static readonly string[] MouthNames = new string[5] { "-", "キス", "舐め", "咥え", "猿轡" };

	private static readonly string[] SpeedNames = new string[3] { "遅", "普", "速" };

	private static readonly string[] ActionNames = new string[16]
	{
		"性器", "アナル", "挿入", "愛撫", "奉仕", "性器挿入", "アナル挿入", "手コキ", "パイズリ", "胸揉み",
		"オナニー", "性器オナニー", "アナルオナニー", "道具", "性器道具", "アナル道具"
	};

	private static readonly string[] DetailNames = new string[49]
	{
		"男主導", "女主導", "フェラ", "イラマ", "アタリ", "苦痛", "多射精", "性器処女", "アナル処女", "女絶頂",
		"男射精", "中出し", "外出し", "咳き込み", "飲み込み", "吐き出し", "口内汁見せ", "膣感", "肛感", "精好",
		"口淫", "複数が相手", "広一が相手", "女アタリ", "女ハズレ", "男アタリ", "男ハズレ", "何度も絶頂した", "何度も性器絶頂した", "何度もアナルで絶頂した",
		"1度だけ絶頂した", "何度も射精した", "何度も性器に射精した", "何度もアナルに射精した", "1度だけ射精した", "広一とした", "何もせず終了", "元傍観者", "傍観者がいる", "自室マップ",
		"庭マップ", "トイレマップ", "狭所マップ", "膣挿入してほしい", "アナル挿入してほしい", "挿入してほしい体位", "挿入するんでしょ？", "好きな行為", "傍観者接近"
	};

	private static readonly string[] KindAnimes = new string[4] { "FemaleLipSync_Def", "FemaleLipSync_Breath", "FemaleLipSync_Pant", "FemaleLipSync_Ahe" };

	private const float NEAR_VISITOR = 1.2f;

	[SerializeField]
	private TextAsset voiceList;

	private List<Data> datas = new List<Data>();

	public void Setup()
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(voiceList);
		int attributeNo = customDataListLoader.GetAttributeNo("タイプ");
		int attributeNo2 = customDataListLoader.GetAttributeNo("状態");
		int attributeNo3 = customDataListLoader.GetAttributeNo("口の状態");
		int attributeNo4 = customDataListLoader.GetAttributeNo("速度");
		int attributeNo5 = customDataListLoader.GetAttributeNo("行為");
		int attributeNo6 = customDataListLoader.GetAttributeNo("詳細条件");
		int attributeNo7 = customDataListLoader.GetAttributeNo("優先度");
		int attributeNo8 = customDataListLoader.GetAttributeNo("種類");
		int attributeNo9 = customDataListLoader.GetAttributeNo("ファイル名");
		int attributeNo10 = customDataListLoader.GetAttributeNo("数");
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			string data3 = customDataListLoader.GetData(attributeNo3, i);
			string data4 = customDataListLoader.GetData(attributeNo4, i);
			string data5 = customDataListLoader.GetData(attributeNo5, i);
			string data6 = customDataListLoader.GetData(attributeNo6, i);
			string data7 = customDataListLoader.GetData(attributeNo7, i);
			string data8 = customDataListLoader.GetData(attributeNo8, i);
			string data9 = customDataListLoader.GetData(attributeNo9, i);
			string data10 = customDataListLoader.GetData(attributeNo10, i);
			int num = Number(data10);
			if (num > 1)
			{
				data9 = data9.Remove(data9.Length - 1);
				if (num > 10)
				{
					Debug.LogError("バリエーション数が予想以上に多い:" + num + "個");
				}
				for (int j = 0; j < num; j++)
				{
					string fileStr = data9 + j;
					datas.Add(new Data(data, data2, data3, data4, data5, data6, data7, data8, fileStr));
				}
			}
			else
			{
				datas.Add(new Data(data, data2, data3, data4, data5, data6, data7, data8, data9));
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
			if (female.personality.ahe)
			{
				female.ExpressionPlay(2, KindAnimes[3], 0.2f);
			}
			else
			{
				female.ExpressionPlay(2, KindAnimes[(int)data.kind], 0.2f);
			}
		}
		else
		{
			Debug.LogWarning("ふさわしい音声がない:" + type);
		}
		return data;
	}

	private Data GetData(Female female, H_VoiceLog log, TYPE type, H_Members members)
	{
		Data result = null;
		List<Data> list = new List<Data>();
		for (int i = 0; i < datas.Count; i++)
		{
			Data data = datas[i];
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
		int num = -1;
		for (int i = 0; i < datas.Count; i++)
		{
			if (datas[i].priority > num)
			{
				num = datas[i].priority;
			}
		}
		if (num == -1)
		{
			return;
		}
		for (int j = 0; j < datas.Count; j++)
		{
			if (datas[j].priority < num)
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
			return HeroinePrefix[(int)heroine] + "_" + data.File;
		}
		return string.Empty;
	}
}
