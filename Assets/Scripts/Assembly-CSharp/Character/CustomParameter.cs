using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class CustomParameter : ParameterBase
	{
		public HairParameter hair;

		public HeadParameter head;

		public BodyParameter body;

		public WearParameter wear;

		public AccessoryParameter acce;

		public CustomParameter(SEX sex)
			: base(sex)
		{
			base.sex = sex;
			hair = new HairParameter(sex);
			head = new HeadParameter(sex);
			body = new BodyParameter(sex);
			wear = new WearParameter(sex);
			acce = new AccessoryParameter(sex);
		}

		public CustomParameter(CustomParameter copy)
			: base(copy.sex)
		{
			hair = new HairParameter(copy.hair);
			head = new HeadParameter(copy.head);
			body = new BodyParameter(copy.body);
			wear = new WearParameter(copy.wear);
			acce = new AccessoryParameter(copy.acce);
		}

		public void Copy(CustomParameter copy, int filter = -1)
		{
			if (sex != copy.sex)
			{
				Debug.LogWarning("違う性別のデータをコピーした");
			}
			sex = copy.sex;
			if (((uint)filter & (true ? 1u : 0u)) != 0)
			{
				hair.Copy(copy.hair);
			}
			if (((uint)filter & 2u) != 0)
			{
				head.Copy(copy.head);
			}
			if (((uint)filter & 4u) != 0)
			{
				body.Copy(copy.body);
			}
			if (((uint)filter & 8u) != 0)
			{
				wear.Copy(copy.wear);
				wear.CheckHasData();
			}
			if (((uint)filter & 0x10u) != 0)
			{
				acce.Copy(copy.acce);
				acce.CheckHasData();
			}
		}

		private void Init()
		{
			hair.Init();
			head.Init();
			body.Init();
			wear.Init();
			acce.Init();
		}

		private void InitCoordinate()
		{
			wear.Init();
			acce.Init();
		}

		public void FromSexyData(CharFemaleFile sexy)
		{
			Init();
			FromSexyData_Common(sexy);
			head.eyeLashID = sexy.femaleCustomInfo.matEyelashesId;
			head.eyeLashColor.FromSexyData(sexy.femaleCustomInfo.eyelashesColor);
			head.eyeshadowTexID = sexy.femaleCustomInfo.texEyeshadowId;
			head.eyeshadowColor = sexy.femaleCustomInfo.eyeshadowColor.rgbaDiffuse;
			head.cheekTexID = sexy.femaleCustomInfo.texCheekId;
			head.cheekColor = sexy.femaleCustomInfo.cheekColor.rgbaDiffuse;
			head.lipTexID = sexy.femaleCustomInfo.texLipId;
			head.lipColor = sexy.femaleCustomInfo.lipColor.rgbaDiffuse;
			head.moleTexID = sexy.femaleCustomInfo.texMoleId;
			head.moleColor = sexy.femaleCustomInfo.moleColor.rgbaDiffuse;
			head.eyeHighlightTexID = sexy.femaleCustomInfo.matEyeHiId;
			head.eyeHighlightColor.FromSexyData(sexy.femaleCustomInfo.eyeHiColor);
			head.eyeHighlightColor.specColor1 = Color.white;
			body.sunburnID = sexy.femaleCustomInfo.texSunburnId;
			body.nipID = sexy.femaleCustomInfo.matNipId;
			body.nipColor.FromSexyData(sexy.femaleCustomInfo.nipColor);
			body.underhairID = sexy.femaleCustomInfo.matUnderhairId;
			body.underhairColor.mainColor = sexy.femaleCustomInfo.underhairColor.rgbaDiffuse;
			body.underhairColor.metallic = 0f;
			body.underhairColor.smooth = 0.5f;
			body.nailColor.Init();
			body.manicureColor.FromSexyData(sexy.femaleCustomInfo.nailColor);
			body.manicureColor.mainColor1.a = 0f;
			body.areolaSize = sexy.femaleCustomInfo.areolaSize;
			body.bustSoftness = sexy.femaleCustomInfo.bustSoftness;
			body.bustWeight = sexy.femaleCustomInfo.bustWeight;
			CharFileInfoClothesFemale charFileInfoClothesFemale = sexy.femaleCoordinateInfo.GetInfo(CharDefine.CoordinateType.type00) as CharFileInfoClothesFemale;
			for (int i = 0; i < 11; i++)
			{
				wear.wears[i].id = charFileInfoClothesFemale.clothesId[i];
				if (wear.wears[i].color == null)
				{
					wear.wears[i].color = new ColorParameter_PBR2();
				}
				wear.wears[i].color.FromSexyData(charFileInfoClothesFemale.clothesColor[i], charFileInfoClothesFemale.clothesColor2[i]);
			}
			wear.isSwimwear = charFileInfoClothesFemale.swimType;
			wear.swimOptTop = !charFileInfoClothesFemale.hideSwimOptTop;
			wear.swimOptBtm = !charFileInfoClothesFemale.hideSwimOptBot;
			int num = body.shapeVals.Length - 1;
			body.shapeVals[num] = 0f;
		}

		public void FromSexyData(CharMaleFile sexy)
		{
			Init();
			FromSexyData_Common(sexy);
			head.beardID = sexy.maleCustomInfo.matBeardId;
			head.beardColor = sexy.maleCustomInfo.beardColor.rgbaDiffuse;
			body.underhairID = 0;
			body.underhairColor = new ColorParameter_Alloy();
			body.underhairColor.mainColor = Color.black;
			body.underhairColor.metallic = 0f;
			body.underhairColor.smooth = 0.5f;
			CharFileInfoClothes info = sexy.maleCoordinateInfo.GetInfo(CharDefine.CoordinateType.type00);
			int num = 0;
			int num2 = 1;
			if (wear.wears[0].color == null)
			{
				wear.wears[0].color = new ColorParameter_PBR2();
			}
			if (wear.wears[10].color == null)
			{
				wear.wears[10].color = new ColorParameter_PBR2();
			}
			wear.wears[0].id = info.clothesId[num];
			wear.wears[0].color.FromSexyData(info.clothesColor[num], info.clothesColor2[num]);
			wear.wears[10].id = info.clothesId[num2];
			wear.wears[10].color.FromSexyData(info.clothesColor[num2], info.clothesColor2[num2]);
		}

		private void FromSexyData_Common(CharFile sexy)
		{
			body.bodyID = sexy.customInfo.texBodyDetailId;
			body.detailID = sexy.customInfo.texBodyDetailId;
			body.detailWeight = sexy.customInfo.bodyDetailWeight;
			body.skinColor.FromSexyData(sexy.customInfo.skinColor);
			body.tattooID = sexy.customInfo.texTattoo_bId;
			body.tattooColor = sexy.customInfo.tattoo_bColor.rgbaDiffuse;
			sexy.customInfo.shapeValueBody.CopyTo(body.shapeVals, 0);
			head.headID = sexy.customInfo.headId;
			head.faceTexID = sexy.customInfo.texFaceId;
			head.tattooID = sexy.customInfo.texTattoo_fId;
			head.tattooColor = sexy.customInfo.tattoo_fColor.rgbaDiffuse;
			head.eyeBrowID = sexy.customInfo.matEyebrowId;
			head.eyeBrowColor.FromSexyData(sexy.customInfo.eyebrowColor);
			head.eyeID_L = sexy.customInfo.matEyeLId;
			head.eyeIrisColorL = sexy.customInfo.eyeLColor.rgbDiffuse;
			head.eyeID_R = sexy.customInfo.matEyeRId;
			head.eyeIrisColorR = sexy.customInfo.eyeRColor.rgbDiffuse;
			head.eyeScleraColorL = sexy.customInfo.eyeWColor.rgbDiffuse;
			head.eyeScleraColorR = sexy.customInfo.eyeWColor.rgbDiffuse;
			head.detailID = sexy.customInfo.texFaceDetailId;
			head.detailWeight = sexy.customInfo.faceDetailWeight;
			head.eyePupilDilationL = 0f;
			head.eyePupilDilationR = 0f;
			head.eyeEmissiveL = 0.5f;
			head.eyeEmissiveR = 0.5f;
			head.shapeVals = new float[sexy.customInfo.shapeValueFace.Length];
			sexy.customInfo.shapeValueFace.CopyTo(head.shapeVals, 0);
			int num = ((sex != 0) ? 1 : 3);
			for (int i = 0; i < num; i++)
			{
				hair.parts[i].ID = sexy.customInfo.hairId[i];
				if (hair.parts[i].hairColor == null)
				{
					hair.parts[i].hairColor = new ColorParameter_Hair();
				}
				hair.parts[i].hairColor.FromSexyData(sexy.customInfo.hairColor[i]);
				hair.parts[i].acceColor = null;
			}
			CharFileInfoClothes info = sexy.coordinateInfo.GetInfo(CharDefine.CoordinateType.type00);
			for (int j = 0; j < 10; j++)
			{
				CharFileInfoClothes.Accessory accessory = info.accessory[j];
				acce.slot[j].Set((ACCESSORY_TYPE)accessory.type, accessory.id, accessory.parentKey);
				acce.slot[j].addPos = accessory.addPos;
				acce.slot[j].addRot = accessory.addRot;
				acce.slot[j].addScl = accessory.addScl;
				if (acce.slot[j].color == null)
				{
					acce.slot[j].color = new ColorParameter_PBR2();
				}
				acce.slot[j].color.FromSexyData(accessory.color, accessory.color2);
			}
		}

		public void FromSexyCoordinateData(CharFileInfoClothesFemale sexyCoord)
		{
			InitCoordinate();
			FromSexyCoordinateData_Common(sexyCoord);
			for (int i = 0; i < 11; i++)
			{
				wear.wears[i].id = sexyCoord.clothesId[i];
				if (wear.wears[i].color == null)
				{
					wear.wears[i].color = new ColorParameter_PBR2();
				}
				wear.wears[i].color.FromSexyData(sexyCoord.clothesColor[i], sexyCoord.clothesColor2[i]);
			}
			wear.isSwimwear = sexyCoord.swimType;
			wear.swimOptTop = !sexyCoord.hideSwimOptTop;
			wear.swimOptBtm = !sexyCoord.hideSwimOptBot;
		}

		public void FromSexyCoordinateData(CharFileInfoClothesMale sexyCoord)
		{
			InitCoordinate();
			FromSexyCoordinateData_Common(sexyCoord);
			int num = 0;
			int num2 = 1;
			if (wear.wears[0].color == null)
			{
				wear.wears[0].color = new ColorParameter_PBR2();
			}
			if (wear.wears[10].color == null)
			{
				wear.wears[10].color = new ColorParameter_PBR2();
			}
			wear.wears[0].id = sexyCoord.clothesId[num];
			wear.wears[0].color.FromSexyData(sexyCoord.clothesColor[num], sexyCoord.clothesColor2[num]);
			wear.wears[10].id = sexyCoord.clothesId[num2];
			wear.wears[10].color.FromSexyData(sexyCoord.clothesColor[num2], sexyCoord.clothesColor2[num2]);
		}

		private void FromSexyCoordinateData_Common(CharFileInfoClothes sexyCoord)
		{
			for (int i = 0; i < 10; i++)
			{
				CharFileInfoClothes.Accessory accessory = sexyCoord.accessory[i];
				acce.slot[i].Set((ACCESSORY_TYPE)accessory.type, accessory.id, accessory.parentKey);
				acce.slot[i].addPos = accessory.addPos;
				acce.slot[i].addRot = accessory.addRot;
				acce.slot[i].addScl = accessory.addScl;
				if (acce.slot[i].color == null)
				{
					acce.slot[i].color = new ColorParameter_PBR2();
				}
				acce.slot[i].color.FromSexyData(accessory.color, accessory.color2);
			}
		}

		public void Save(BinaryWriter writer)
		{
			Write(writer, CUSTOM_DATA_VERSION.DEBUG_10);
			Write(writer, sex);
			hair.Save(writer, sex);
			head.Save(writer, sex);
			body.Save(writer, sex);
			wear.Save(writer, sex);
			acce.Save(writer, sex);
		}

		public void Load(TextAsset text, bool female, bool male)
		{
			if (text == null)
			{
				Debug.LogError("不明なバイナリカード");
				return;
			}
			using (MemoryStream input = new MemoryStream(text.bytes))
			{
				using (BinaryReader reader = new BinaryReader(input))
				{
					if (!Load(reader, female, male))
					{
						Debug.LogError("読み込みに失敗しました");
					}
				}
			}
		}

		public void Load(string file, bool female, bool male)
		{
			if (!File.Exists(file))
			{
				return;
			}
			using (FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader reader = new BinaryReader(input))
				{
					if (!Load(reader, female, male))
					{
						Debug.LogError("読み込みに失敗しました");
					}
				}
			}
		}

		protected bool Load(BinaryReader reader, bool female, bool male)
		{
			bool result = true;
			try
			{
				long offset = PNG_Loader.CheckSize(reader);
				reader.BaseStream.Seek(offset, SeekOrigin.Begin);
				string text = reader.ReadString();
				switch (text)
				{
				case "【HoneySelectCharaFemale】":
					Debug.Log("ハニーセレクト：女");
					if (female)
					{
						reader.BaseStream.Seek(offset, SeekOrigin.Begin);
						CharFemaleFile charFemaleFile2 = new CharFemaleFile();
						result = charFemaleFile2.Load(reader, true);
						FromSexyData(charFemaleFile2);
					}
					else
					{
						Debug.LogWarning("異性データ");
					}
					break;
				case "【PremiumResortCharaFemale】":
					Debug.Log("セクシービーチプレミアムリゾート：女");
					if (female)
					{
						reader.BaseStream.Seek(0L, SeekOrigin.Begin);
						CharFemaleFile charFemaleFile = new CharFemaleFile();
						result = charFemaleFile.LoadFromSBPR(reader);
						FromSexyData(charFemaleFile);
					}
					else
					{
						Debug.LogWarning("異性データ");
					}
					break;
				case "【PlayHome_Female】":
					if (female)
					{
						Load(reader);
					}
					else
					{
						Debug.LogWarning("異性データ");
					}
					break;
				case "【HoneySelectCharaMale】":
					Debug.Log("ハニーセレクト：男");
					if (male)
					{
						reader.BaseStream.Seek(offset, SeekOrigin.Begin);
						CharMaleFile charMaleFile = new CharMaleFile();
						result = charMaleFile.Load(reader, true);
						FromSexyData(charMaleFile);
					}
					else
					{
						Debug.LogWarning("異性データ");
					}
					break;
				case "【PremiumResortCharaMale】":
					Debug.Log("セクシービーチプレミアムリゾート：男");
					if (male)
					{
						reader.BaseStream.Seek(0L, SeekOrigin.Begin);
						CharMaleFile charMaleFile2 = new CharMaleFile();
						result = charMaleFile2.LoadFromSBPR(reader);
						FromSexyData(charMaleFile2);
					}
					else
					{
						Debug.LogWarning("異性データ");
					}
					break;
				case "【PlayHome_Male】":
					if (male)
					{
						Load(reader);
					}
					else
					{
						Debug.LogWarning("異性データ");
					}
					break;
				default:
					Debug.LogWarning("読めないセーブデータ:" + text);
					break;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				result = false;
			}
			return result;
		}

		public void Load(BinaryReader reader)
		{
			Init();
			CUSTOM_DATA_VERSION version = CUSTOM_DATA_VERSION.UNKNOWN;
			Read(reader, ref version);
			Read(reader, ref sex);
			hair.Load(reader, sex, version);
			head.Load(reader, sex, version);
			body.Load(reader, sex, version);
			wear.Load(reader, sex, version);
			acce.Load(reader, sex, version);
		}

		public void SaveCoordinate(BinaryWriter writer)
		{
			Write(writer, CUSTOM_DATA_VERSION.DEBUG_10);
			Write(writer, sex);
			wear.Save(writer, sex);
			acce.Save(writer, sex);
		}

		public void LoadCoordinate(BinaryReader reader)
		{
			Init();
			CUSTOM_DATA_VERSION version = CUSTOM_DATA_VERSION.UNKNOWN;
			Read(reader, ref version);
			Read(reader, ref sex);
			wear.Load(reader, sex, version);
			acce.Load(reader, sex, version);
		}

		protected void Write(BinaryWriter writer, CUSTOM_DATA_VERSION version)
		{
			writer.Write((int)version);
		}

		protected void Read(BinaryReader reader, ref CUSTOM_DATA_VERSION version)
		{
			int num = reader.ReadInt32();
			if (num > 10)
			{
				version = CUSTOM_DATA_VERSION.UNKNOWN;
			}
			else
			{
				version = (CUSTOM_DATA_VERSION)num;
			}
		}

		protected void Write(BinaryWriter writer, SEX sex)
		{
			writer.Write((int)sex);
		}

		protected void Read(BinaryReader reader, ref SEX sex)
		{
			sex = (SEX)reader.ReadInt32();
		}

		public bool CheckWrongParam()
		{
			if (sex != 0 && sex != SEX.MALE)
			{
				return true;
			}
			if (acce.slot.Length != 10)
			{
				return true;
			}
			for (int i = 0; i < head.shapeVals.Length; i++)
			{
				if (head.shapeVals[i] < 0f || head.shapeVals[i] > 1f)
				{
					return true;
				}
			}
			for (int j = 0; j < body.shapeVals.Length; j++)
			{
				if (body.shapeVals[j] < 0f || body.shapeVals[j] > 1f)
				{
					return true;
				}
			}
			return false;
		}
	}
}
