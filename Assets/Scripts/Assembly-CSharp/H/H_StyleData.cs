using System;
using System.Collections.Generic;
using UnityEngine;

namespace H
{
	public class H_StyleData
	{
		public enum TYPE
		{
			INSERT = 0,
			SERVICE = 1,
			PETTING = 2,
			NUM = 3
		}

		public enum STATE
		{
			UNKNOWN = -1,
			RESIST = 0,
			FLOP = 1,
			WEAKNESS = 2
		}

		public enum DETAIL
		{
			NONE = 0,
			KISS = 1,
			LICK = 2,
			FELLATIO = 4,
			IRRUMATIO = 8,
			VAGINA = 16,
			ANAL = 32,
			HAND_JOB = 64,
			TITTY_FUCK = 128,
			PET_BUST = 256,
			ONANIE = 512,
			TOOL = 1024,
			MALE_RESTRICT = 2048,
			DO_NOT_EXTRACT = 4096,
			NUM = 13
		}

		public enum INITIATIVE
		{
			FAIR = -1,
			FEMALE = 0,
			MALE = 1
		}

		public enum POSITION
		{
			FLOOR = 0,
			WALL = 1,
			CHAIR = 2,
			SPECIAL = 3,
			FIVE_RESIST = 4,
			FIVE_FLOP = 5,
			FIVE_WEAKNESS = 6
		}

		public enum MEMBER
		{
			PAIR = 0,
			M2F1 = 1,
			M3F1 = 2,
			M4F1 = 3,
			M1F2 = 4,
			NUM = 5
		}

		private static readonly string[] DetailNames = new string[13]
		{
			"キス", "舐め", "フェラ", "イラマ", "性器", "アナル", "手コキ", "パイズリ", "胸揉み", "オナニー",
			"道具", "男拘束", "抜けない"
		};

		public static readonly int DetailMasking_InMouth = 12;

		public static readonly int DetailMasking_Bust = 384;

		public static readonly int DetailMasking_UseMouth = 15;

		public string id;

		public string name;

		public MEMBER member;

		public TYPE type;

		public STATE state;

		public POSITION position;

		public string map = string.Empty;

		public string assetBundle;

		public int detailFlag;

		public INITIATIVE initiative = INITIATIVE.FAIR;

		public float insertDelay;

		public bool hasLight;

		public Vector3 lightEuler;

		public int noInsMale;

		public IllusionCameraResetData camera;

		public IK_DataList baseIK = new IK_DataList();

		public Dictionary<string, IK_DataList> alternativeIK = new Dictionary<string, IK_DataList>();

		public H_StyleData(string assetBundle)
		{
			this.assetBundle = assetBundle;
			string text = assetBundle.Substring(assetBundle.LastIndexOf("/") + 1);
			id = text.ToUpper();
			CheckState();
			AssetBundleController assetBundleController = new AssetBundleController();
			assetBundleController.OpenFromFile(GlobalData.assetBundlePath, assetBundle);
			TextAsset textAsset = assetBundleController.LoadAsset<TextAsset>(id + "_Data");
			TagText tagText = new TagText(textAsset);
			foreach (TagText.Element element in tagText.Elements)
			{
				if (element.Tag == "StyleData")
				{
					LoadStyleData(element);
				}
				else if (element.Tag == "Camera")
				{
					LoadCamera(element);
				}
				else if (element.Tag == "IK")
				{
					LoadIK(element);
				}
				else if (element.Tag == "Light")
				{
					LoadLight(element);
				}
				else
				{
					Debug.LogError("不明なエレメント:" + element.Tag);
				}
			}
			assetBundleController.Close();
		}

		private void LoadStyleData(TagText.Element element)
		{
			element.GetVal(ref name, "name", 0);
			switch (element.Attributes["type"].valOriginal)
			{
			case "挿入":
				type = TYPE.INSERT;
				break;
			case "奉仕":
				type = TYPE.SERVICE;
				break;
			case "愛撫":
				type = TYPE.PETTING;
				break;
			default:
				Debug.LogError("タイプ不明");
				break;
			}
			string val = string.Empty;
			if (element.GetVal(ref val, "member", 0))
			{
				if (val.Equals("pair", StringComparison.OrdinalIgnoreCase))
				{
					member = MEMBER.PAIR;
				}
				else if (val.Equals("m1f1", StringComparison.OrdinalIgnoreCase) || val.Equals("f1m1", StringComparison.OrdinalIgnoreCase))
				{
					member = MEMBER.PAIR;
				}
				else if (val.Equals("m2f1", StringComparison.OrdinalIgnoreCase) || val.Equals("f1m2", StringComparison.OrdinalIgnoreCase))
				{
					member = MEMBER.M2F1;
				}
				else if (val.Equals("m3f1", StringComparison.OrdinalIgnoreCase) || val.Equals("f1m3", StringComparison.OrdinalIgnoreCase))
				{
					member = MEMBER.M3F1;
				}
				else if (val.Equals("m4f1", StringComparison.OrdinalIgnoreCase) || val.Equals("f1m4", StringComparison.OrdinalIgnoreCase))
				{
					member = MEMBER.M4F1;
				}
				else if (val.Equals("m1f2", StringComparison.OrdinalIgnoreCase) || val.Equals("f2m1", StringComparison.OrdinalIgnoreCase))
				{
					member = MEMBER.M1F2;
				}
			}
			detailFlag = DetailFromString(element.GetAttribute("detail"));
			initiative = InitiativeFromString(element.GetAttribute("initiative"));
			element.GetVal(ref insertDelay, "insertDelay", 0);
			string val2 = string.Empty;
			element.GetVal(ref val2, "position", 0);
			switch (val2)
			{
			case "床":
				position = POSITION.FLOOR;
				break;
			case "壁":
				position = POSITION.WALL;
				break;
			case "椅子":
				position = POSITION.CHAIR;
				break;
			case "特殊":
				position = POSITION.SPECIAL;
				break;
			default:
				position = POSITION.FLOOR;
				break;
			}
			if (member == MEMBER.M4F1)
			{
				if (state == STATE.RESIST)
				{
					position = POSITION.FIVE_RESIST;
				}
				else if (state == STATE.FLOP)
				{
					position = POSITION.FIVE_FLOP;
				}
				else if (state == STATE.WEAKNESS)
				{
					position = POSITION.FIVE_WEAKNESS;
				}
			}
			TagText.Attribute attribute = element.GetAttribute("noInsMale");
			if (attribute != null)
			{
				for (int i = 0; i < attribute.ValsNum; i++)
				{
					int num = int.Parse(attribute.vals[i]);
					noInsMale |= 1 << num;
				}
			}
			element.GetVal(ref map, "map", 0);
			if (detailFlag == 0)
			{
				if (type == TYPE.INSERT)
				{
					Debug.LogError("No details:" + id);
				}
				else
				{
					Debug.Log("No details:" + id);
				}
			}
		}

		private static int DetailFromString(TagText.Attribute detailAttr)
		{
			if (detailAttr == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < detailAttr.ValsNum; i++)
			{
				string text = detailAttr.vals[i];
				for (int j = 0; j < DetailNames.Length; j++)
				{
					if (text == DetailNames[j])
					{
						num |= 1 << j;
					}
				}
			}
			return num;
		}

		private static INITIATIVE InitiativeFromString(TagText.Attribute attr)
		{
			if (attr == null)
			{
				return INITIATIVE.FAIR;
			}
			if (attr.valOriginal == "男")
			{
				return INITIATIVE.MALE;
			}
			if (attr.valOriginal == "女")
			{
				return INITIATIVE.FEMALE;
			}
			if (attr.valOriginal == "-")
			{
				return INITIATIVE.FAIR;
			}
			Debug.LogError("Initiative不明:" + attr.valOriginal);
			return INITIATIVE.FAIR;
		}

		private void LoadCamera(TagText.Element element)
		{
			camera = new IllusionCameraResetData();
			element.GetVal(ref camera.pos.x, "pos", 0);
			element.GetVal(ref camera.pos.y, "pos", 1);
			element.GetVal(ref camera.pos.z, "pos", 2);
			element.GetVal(ref camera.eul.x, "eul", 0);
			element.GetVal(ref camera.eul.y, "eul", 1);
			element.GetVal(ref camera.eul.z, "eul", 2);
			element.GetVal(ref camera.dis, "dis", 0);
		}

		private void LoadIK(TagText.Element element)
		{
			IK_DataList iK_DataList = null;
			string val = string.Empty;
			element.GetVal(ref val, "anime", 0);
			if (val.Length > 0)
			{
				iK_DataList = new IK_DataList();
				alternativeIK.Add(val, iK_DataList);
			}
			else
			{
				iK_DataList = baseIK;
			}
			TagText.Attribute attribute = element.GetAttribute("ik");
			if (attribute != null && attribute.vals.Count % 4 == 0)
			{
				for (int i = 0; i < attribute.vals.Count; i += 4)
				{
					string ikChara = attribute.vals[i];
					string ikPart = attribute.vals[i + 1];
					string targetChara = attribute.vals[i + 2];
					string targetPart = attribute.vals[i + 3];
					iK_DataList.Add(new IK_Data(ikChara, ikPart, targetChara, targetPart));
				}
			}
		}

		private void LoadLight(TagText.Element element)
		{
			hasLight = true;
			element.GetVal(ref lightEuler.x, "eul", 0);
			element.GetVal(ref lightEuler.y, "eul", 1);
		}

		public H_MOUTH GetInsertedMouth()
		{
			if (((uint)detailFlag & (true ? 1u : 0u)) != 0)
			{
				return H_MOUTH.KISS;
			}
			if (((uint)detailFlag & 2u) != 0)
			{
				return H_MOUTH.LICK;
			}
			if ((detailFlag & DetailMasking_InMouth) != 0)
			{
				return H_MOUTH.INMOUTH;
			}
			return H_MOUTH.FREE;
		}

		private void CheckState()
		{
			string[] array = id.Split(new char[1] { '_' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 4)
			{
				string text = array[0];
				string text2 = array[1];
				string text3 = array[2];
				string text4 = array[3];
				if (text3 == "02")
				{
					state = STATE.WEAKNESS;
				}
				else if (text3 == "01")
				{
					state = STATE.FLOP;
				}
				else
				{
					state = STATE.RESIST;
				}
			}
			else
			{
				Debug.LogError("不明な命名規則の体位データ");
			}
		}

		public bool IsInMouth()
		{
			return (detailFlag & DetailMasking_InMouth) != 0;
		}
	}
}
