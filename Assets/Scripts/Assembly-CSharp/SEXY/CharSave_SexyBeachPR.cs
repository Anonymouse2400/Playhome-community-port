using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	public class CharSave_SexyBeachPR
	{
		public class ColorSetSBPR
		{
			public HsvColor diffuseColor = new HsvColor(20f, 0.8f, 0.8f);

			public float alpha = 1f;

			public HsvColor specularColor = new HsvColor(0f, 0f, 0.8f);

			public float specularIntensity = 0.5f;

			public float specularSharpness = 3f;

			public void Load(BinaryReader reader, int convertType)
			{
				diffuseColor.H = (float)reader.ReadDouble();
				diffuseColor.S = (float)reader.ReadDouble();
				diffuseColor.V = (float)reader.ReadDouble();
				alpha = (float)reader.ReadDouble();
				specularColor.H = (float)reader.ReadDouble();
				specularColor.S = (float)reader.ReadDouble();
				specularColor.V = (float)reader.ReadDouble();
				specularIntensity = (float)reader.ReadDouble();
				specularSharpness = (float)reader.ReadDouble();
				if (convertType == 0)
				{
					specularIntensity = Mathf.InverseLerp(0f, 5f, specularIntensity) * 0.8f;
					specularSharpness = Mathf.InverseLerp(0f, 9f, specularSharpness) * 0.9f;
				}
			}

			public void CopyToHoneySelect(HSColorSet dst)
			{
				dst.hsvDiffuse.Copy(diffuseColor);
				dst.alpha = alpha;
				dst.hsvSpecular.Copy(specularColor);
				dst.specularIntensity = specularIntensity;
				dst.specularSharpness = specularSharpness;
			}
		}

		public class Coordinate
		{
			public int clothesTopId;

			public ColorSetSBPR clothesTopColor;

			public int clothesBotId;

			public ColorSetSBPR clothesBotColor;

			public int braId;

			public ColorSetSBPR braColor;

			public int shortsId;

			public ColorSetSBPR shortsColor;

			public int glovesId;

			public ColorSetSBPR glovesColor;

			public int panstId;

			public ColorSetSBPR panstColor;

			public int socksId;

			public ColorSetSBPR socksColor;

			public int shoesId;

			public ColorSetSBPR shoesColor;

			public int swimsuitId;

			public ColorSetSBPR swimsuitColor;

			public int swimTopId;

			public ColorSetSBPR swimTopColor;

			public int swimBotId;

			public ColorSetSBPR swimBotColor;
		}

		public class Accessory
		{
			public int accessoryType = -1;

			public int accessoryId = -1;

			public string parentKey = string.Empty;

			public Vector3 plusPos = Vector3.zero;

			public Vector3 plusRot = Vector3.zero;

			public Vector3 plusScl = Vector3.one;

			public void Load(BinaryReader reader)
			{
				accessoryType = reader.ReadInt32();
				accessoryId = reader.ReadInt32();
				parentKey = reader.ReadString();
				plusPos.x = (float)reader.ReadDouble();
				plusPos.y = (float)reader.ReadDouble();
				plusPos.z = (float)reader.ReadDouble();
				plusRot.x = (float)reader.ReadDouble();
				plusRot.y = (float)reader.ReadDouble();
				plusRot.z = (float)reader.ReadDouble();
				plusScl.x = (float)reader.ReadDouble();
				plusScl.y = (float)reader.ReadDouble();
				plusScl.z = (float)reader.ReadDouble();
			}
		}

		public class SaveData
		{
			public string fileName = string.Empty;

			public byte[] pngData;

			public int fileVersion;

			public byte sex;

			public int customVersion;

			public int personality;

			public string name = string.Empty;

			public int headId;

			public float[] sbpr_shapeFace;

			public float[] sbpr_shapeBody;

			public float[] shapeFace;

			public float[] shapeBody;

			public int[] hairId;

			public int hairType;

			public ColorSetSBPR[] hairColor;

			public ColorSetSBPR[] hairAcsColor;

			public int texFaceId;

			public ColorSetSBPR skinColor;

			public int texEyeshadowId;

			public ColorSetSBPR eyeshadowColor;

			public int texCheekId;

			public ColorSetSBPR cheekColor;

			public int texLipId;

			public ColorSetSBPR lipColor;

			public int texTattoo_fId;

			public ColorSetSBPR tattoo_fColor;

			public int texMoleId;

			public ColorSetSBPR moleColor;

			public int matEyebrowId;

			public ColorSetSBPR eyebrowColor;

			public int matEyelashesId;

			public ColorSetSBPR eyelashesColor;

			public int matEyeLId;

			public ColorSetSBPR eyeLColor;

			public int matEyeRId;

			public ColorSetSBPR eyeRColor;

			public int matEyeHiId;

			public ColorSetSBPR eyeHiColor;

			public ColorSetSBPR eyeWColor;

			public float faceDetailWeight;

			public int texBodyId;

			public int texSunburnId;

			public ColorSetSBPR sunburnColor;

			public int texTattoo_bId;

			public ColorSetSBPR tattoo_bColor;

			public int matNipId;

			public ColorSetSBPR nipColor;

			public int matUnderhairId;

			public ColorSetSBPR underhairColor;

			public ColorSetSBPR nailColor;

			public float nipSize = 0.5f;

			public float bodyDetailWeight;

			public int beardId;

			public ColorSetSBPR beardColor;

			public float bustSoftness = 0.5f;

			public float bustWeight = 0.5f;

			public int clothesVersion;

			public Coordinate[] coord;

			public byte stateSwimOptTop;

			public byte stateSwimOptBot;

			public Accessory[,] accessory;

			public ColorSetSBPR[,] accessoryColor;
		}

		public enum ConvertType
		{
			HoneySelect = 0
		}

		public const string CharaFileFemaleDir = "chara/female/";

		public const string CharaFileMaleDir = "chara/male/";

		public const string CharaFileFemaleMark = "【PremiumResortCharaFemale】";

		public const string CharaFileMaleMark = "【PremiumResortCharaMale】";

		public const int CharaFileVersion = 1;

		public const int PreviewVersion = 1;

		public const int CustomVersion = 5;

		public const int ClothesVersion = 2;

		public const int MaleShapeFaceNum = 67;

		public const int MaleShapeBodyNum = 21;

		public const int FemaleShapeFaceNum = 67;

		public const int FemaleShapeBodyNum = 32;

		public const int CoordinateNum = 2;

		public const int AccessorySlotNum = 5;

		public SaveData savedata { get; private set; }

		public ConvertType convertType { get; private set; }

		public CharSave_SexyBeachPR(ConvertType type)
		{
			convertType = type;
			savedata = new SaveData();
		}

		public string GetFileTag(string filepath = "")
		{
			string empty = string.Empty;
			if (!File.Exists(filepath))
			{
				return empty;
			}
			using (FileStream input = new FileStream(filepath, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					if (binaryReader.BaseStream.Length == 0)
					{
						Debug.LogError("データが破損しています。");
						return empty;
					}
					long size = 0L;
					PngAssist.CheckPngData(binaryReader, ref size, true);
					return binaryReader.ReadString();
				}
			}
		}

		public bool LoadCharaFile(string filepath = "")
		{
			if (!File.Exists(filepath))
			{
				return false;
			}
			savedata.fileName = Path.GetFileName(filepath);
			bool flag = false;
			using (FileStream input = new FileStream(filepath, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(input))
				{
					return LoadCharaFile(br);
				}
			}
		}

		public bool LoadCharaFile(BinaryReader br)
		{
			if (br.BaseStream.Length == 0)
			{
				Debug.LogError("データが破損しています。");
				return false;
			}
			savedata.pngData = PngAssist.LoadPngData(br);
			string text = br.ReadString();
			byte b = 0;
			if (text == "【PremiumResortCharaFemale】")
			{
				b = 1;
			}
			else
			{
				if (!(text == "【PremiumResortCharaMale】"))
				{
					Debug.LogError("ファイルの種類が違います。");
					return false;
				}
				b = 0;
			}
			savedata.sex = b;
			savedata.fileVersion = br.ReadInt32();
			if (savedata.fileVersion > 1)
			{
				Debug.LogError("実行ファイルよりも新しいキャラファイルです。");
				return false;
			}
			if (savedata.pngData != null && convertType == ConvertType.HoneySelect)
			{
				Texture2D texture2D = new Texture2D(0, 0, TextureFormat.ARGB32, false);
				texture2D.LoadImage(savedata.pngData);
				TextureScale.Bilinear(texture2D, 252, 352);
				savedata.pngData = texture2D.EncodeToPNG();
			}
			int num = br.ReadInt32();
			BlockHeader[] array = new BlockHeader[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new BlockHeader();
				array[i].LoadHeader(br);
			}
			int num2 = 0;
			num2++;
			savedata.customVersion = array[num2].version;
			if (savedata.customVersion > 5)
			{
				Debug.LogError("実行ファイルよりも新しいカスタム設定です。");
			}
			else
			{
				br.BaseStream.Seek(array[num2].pos, SeekOrigin.Begin);
				byte[] data = br.ReadBytes((int)array[num2].size);
				if (!LoadCustomData(data, array[num2].version, b))
				{
					Debug.LogError("カスタム設定の読み込みに失敗しました");
					return false;
				}
				data = null;
			}
			num2++;
			savedata.clothesVersion = array[num2].version;
			if (array[num2].version > 2)
			{
				Debug.LogError("実行ファイルよりも新しい服装設定です。");
			}
			else
			{
				br.BaseStream.Seek(array[num2].pos, SeekOrigin.Begin);
				byte[] data2 = br.ReadBytes((int)array[num2].size);
				if (!LoadClothesData(data2, array[num2].version, b))
				{
					Debug.LogError("服装設定の読み込みに失敗しました");
					return false;
				}
				data2 = null;
			}
			return true;
		}

		public bool LoadCustomData(byte[] data, int version, byte sex)
		{
			using (MemoryStream input = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					binaryReader.BaseStream.Seek(1L, SeekOrigin.Current);
					savedata.personality = binaryReader.ReadInt32();
					savedata.name = binaryReader.ReadString();
					savedata.headId = binaryReader.ReadInt32();
					savedata.sbpr_shapeFace = new float[(sex != 0) ? 67 : 67];
					if (version < 3)
					{
						for (int i = 0; i < 66; i++)
						{
							savedata.sbpr_shapeFace[i] = (float)binaryReader.ReadDouble();
						}
					}
					else
					{
						for (int j = 0; j < savedata.sbpr_shapeFace.Length; j++)
						{
							savedata.sbpr_shapeFace[j] = (float)binaryReader.ReadDouble();
						}
					}
					savedata.sbpr_shapeBody = new float[(sex != 0) ? 32 : 21];
					for (int k = 0; k < savedata.sbpr_shapeBody.Length; k++)
					{
						savedata.sbpr_shapeBody[k] = (float)binaryReader.ReadDouble();
					}
					savedata.hairId = new int[(sex == 0) ? 1 : 4];
					for (int l = 0; l < savedata.hairId.Length; l++)
					{
						savedata.hairId[l] = binaryReader.ReadInt32();
					}
					savedata.hairColor = new ColorSetSBPR[(sex == 0) ? 1 : 4];
					for (int m = 0; m < savedata.hairColor.Length; m++)
					{
						savedata.hairColor[m] = new ColorSetSBPR();
						savedata.hairColor[m].Load(binaryReader, (int)convertType);
					}
					savedata.hairAcsColor = new ColorSetSBPR[(sex == 0) ? 1 : 4];
					for (int n = 0; n < savedata.hairAcsColor.Length; n++)
					{
						savedata.hairAcsColor[n] = new ColorSetSBPR();
						savedata.hairAcsColor[n].Load(binaryReader, (int)convertType);
					}
					savedata.texFaceId = binaryReader.ReadInt32();
					savedata.skinColor = new ColorSetSBPR();
					savedata.skinColor.Load(binaryReader, (int)convertType);
					if (sex == 1)
					{
						savedata.texEyeshadowId = binaryReader.ReadInt32();
						savedata.eyeshadowColor = new ColorSetSBPR();
						savedata.eyeshadowColor.Load(binaryReader, (int)convertType);
						savedata.texCheekId = binaryReader.ReadInt32();
						savedata.cheekColor = new ColorSetSBPR();
						savedata.cheekColor.Load(binaryReader, (int)convertType);
						savedata.texLipId = binaryReader.ReadInt32();
						savedata.lipColor = new ColorSetSBPR();
						savedata.lipColor.Load(binaryReader, (int)convertType);
					}
					savedata.texTattoo_fId = binaryReader.ReadInt32();
					savedata.tattoo_fColor = new ColorSetSBPR();
					savedata.tattoo_fColor.Load(binaryReader, (int)convertType);
					if (sex == 1)
					{
						savedata.texMoleId = binaryReader.ReadInt32();
						savedata.moleColor = new ColorSetSBPR();
						savedata.moleColor.Load(binaryReader, (int)convertType);
					}
					savedata.matEyebrowId = binaryReader.ReadInt32();
					savedata.eyebrowColor = new ColorSetSBPR();
					savedata.eyebrowColor.Load(binaryReader, (int)convertType);
					if (sex == 1)
					{
						savedata.matEyelashesId = binaryReader.ReadInt32();
						savedata.eyelashesColor = new ColorSetSBPR();
						savedata.eyelashesColor.Load(binaryReader, (int)convertType);
					}
					savedata.matEyeLId = binaryReader.ReadInt32();
					savedata.eyeLColor = new ColorSetSBPR();
					savedata.eyeLColor.Load(binaryReader, (int)convertType);
					savedata.matEyeRId = binaryReader.ReadInt32();
					savedata.eyeRColor = new ColorSetSBPR();
					savedata.eyeRColor.Load(binaryReader, (int)convertType);
					if (sex == 1)
					{
						savedata.matEyeHiId = binaryReader.ReadInt32();
						savedata.eyeHiColor = new ColorSetSBPR();
						savedata.eyeHiColor.Load(binaryReader, (int)convertType);
					}
					savedata.eyeWColor = new ColorSetSBPR();
					savedata.eyeWColor.Load(binaryReader, (int)convertType);
					if (version >= 2)
					{
						savedata.faceDetailWeight = (float)binaryReader.ReadDouble();
					}
					savedata.texBodyId = binaryReader.ReadInt32();
					if (sex == 1)
					{
						savedata.texSunburnId = binaryReader.ReadInt32();
						savedata.sunburnColor = new ColorSetSBPR();
						savedata.sunburnColor.Load(binaryReader, (int)convertType);
					}
					savedata.texTattoo_bId = binaryReader.ReadInt32();
					savedata.tattoo_bColor = new ColorSetSBPR();
					savedata.tattoo_bColor.Load(binaryReader, (int)convertType);
					if (sex == 1)
					{
						savedata.matNipId = binaryReader.ReadInt32();
						savedata.nipColor = new ColorSetSBPR();
						savedata.nipColor.Load(binaryReader, (int)convertType);
						savedata.matUnderhairId = binaryReader.ReadInt32();
						savedata.underhairColor = new ColorSetSBPR();
						savedata.underhairColor.Load(binaryReader, (int)convertType);
						savedata.nailColor = new ColorSetSBPR();
						savedata.nailColor.Load(binaryReader, (int)convertType);
						if (version >= 1)
						{
							savedata.nipSize = (float)binaryReader.ReadDouble();
						}
					}
					if (version >= 2)
					{
						savedata.bodyDetailWeight = (float)binaryReader.ReadDouble();
						if (convertType == ConvertType.HoneySelect)
						{
							savedata.bodyDetailWeight *= 0.6f;
						}
					}
					if (sex == 0)
					{
						savedata.beardId = binaryReader.ReadInt32();
						savedata.beardColor = new ColorSetSBPR();
						savedata.beardColor.Load(binaryReader, (int)convertType);
					}
					else
					{
						if (version >= 4)
						{
							binaryReader.BaseStream.Seek(4L, SeekOrigin.Current);
						}
						if (version >= 5)
						{
							savedata.bustSoftness = (float)binaryReader.ReadDouble();
							savedata.bustWeight = (float)binaryReader.ReadDouble();
						}
					}
					if (convertType == ConvertType.HoneySelect)
					{
						if (sex == 1)
						{
							if (14 > savedata.personality || savedata.personality > 18)
							{
								savedata.personality = 0;
							}
							else
							{
								savedata.personality -= 14;
							}
							int[] array = new int[41]
							{
								0, 2, 3, 3, 0, 1, 2, 2, 1, 4,
								2, 5, 0, 4, 1, 5, 4, 4, 3, 3,
								3, 2, 3, 4, 5, 0, 5, 0, 1, 5,
								0, 0, 2, 5, 3, 3, 5, 5, 5, 2,
								0
							};
							savedata.hairType = array[savedata.hairId[0]];
						}
						else
						{
							savedata.personality = 0;
						}
						ConvertShapeValue();
					}
					return true;
				}
			}
		}

		public void ConvertShapeValue()
		{
			if (convertType != 0)
			{
				return;
			}
			if (savedata.sex == 0)
			{
				savedata.shapeBody = new float[21];
				for (int i = 0; i < savedata.shapeBody.Length; i++)
				{
					savedata.shapeBody[i] = savedata.sbpr_shapeBody[i];
				}
				savedata.shapeFace = new float[67];
				int[] array = new int[67]
				{
					0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
					10, 11, 12, 27, 28, 29, 30, 31, 32, 49,
					65, 50, 51, 52, 33, 34, 35, 36, 37, 38,
					39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
					13, 14, 15, 16, 17, 18, 19, 20, 21, 66,
					22, 23, 24, 25, 26, 58, 59, 60, 61, 62,
					63, 64, 53, 54, 55, 56, 57
				};
				for (int j = 0; j < 67; j++)
				{
					savedata.shapeFace[j] = savedata.sbpr_shapeFace[array[j]];
				}
			}
			else
			{
				savedata.shapeBody = new float[32];
				for (int k = 0; k < savedata.shapeBody.Length; k++)
				{
					savedata.shapeBody[k] = savedata.sbpr_shapeBody[k];
				}
				savedata.shapeFace = new float[67];
				int[] array2 = new int[67]
				{
					0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
					10, 11, 12, 27, 28, 29, 30, 31, 32, 48,
					49, 50, 51, 52, 33, 34, 35, 36, 37, 38,
					65, 39, 40, 41, 42, 43, 44, 45, 46, 47,
					13, 14, 15, 16, 17, 18, 19, 20, 21, 66,
					22, 23, 24, 25, 26, 58, 59, 60, 61, 62,
					63, 64, 53, 54, 55, 56, 57
				};
				for (int l = 0; l < 67; l++)
				{
					savedata.shapeFace[l] = savedata.sbpr_shapeFace[array2[l]];
				}
			}
		}

		public bool LoadClothesData(byte[] data, int version, byte sex)
		{
			using (MemoryStream input = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					savedata.coord = new Coordinate[2];
					binaryReader.BaseStream.Seek(1L, SeekOrigin.Current);
					for (int i = 0; i < savedata.coord.Length; i++)
					{
						savedata.coord[i] = new Coordinate();
						if (sex == 0)
						{
							savedata.coord[i].clothesTopId = binaryReader.ReadInt32();
							savedata.coord[i].shoesId = binaryReader.ReadInt32();
							if (version >= 2)
							{
								savedata.coord[i].clothesTopColor = new ColorSetSBPR();
								savedata.coord[i].clothesTopColor.Load(binaryReader, (int)convertType);
								savedata.coord[i].shoesColor = new ColorSetSBPR();
								savedata.coord[i].shoesColor.Load(binaryReader, (int)convertType);
							}
							continue;
						}
						savedata.coord[i].clothesTopId = binaryReader.ReadInt32();
						savedata.coord[i].clothesBotId = binaryReader.ReadInt32();
						savedata.coord[i].braId = binaryReader.ReadInt32();
						savedata.coord[i].shortsId = binaryReader.ReadInt32();
						savedata.coord[i].glovesId = binaryReader.ReadInt32();
						savedata.coord[i].panstId = binaryReader.ReadInt32();
						savedata.coord[i].socksId = binaryReader.ReadInt32();
						savedata.coord[i].shoesId = binaryReader.ReadInt32();
						savedata.coord[i].swimsuitId = binaryReader.ReadInt32();
						savedata.coord[i].swimTopId = binaryReader.ReadInt32();
						savedata.coord[i].swimBotId = binaryReader.ReadInt32();
						if (convertType == ConvertType.HoneySelect)
						{
							if (savedata.coord[i].clothesTopId == 102 || savedata.coord[i].clothesTopId == 111)
							{
								savedata.coord[i].clothesTopId = 101;
							}
							if (savedata.coord[i].clothesBotId == 82)
							{
								savedata.coord[i].clothesBotId = 0;
							}
							if (savedata.coord[i].swimsuitId == 46)
							{
								savedata.coord[i].swimsuitId = 50;
							}
						}
						if (version >= 2)
						{
							savedata.coord[i].clothesTopColor = new ColorSetSBPR();
							savedata.coord[i].clothesTopColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].clothesBotColor = new ColorSetSBPR();
							savedata.coord[i].clothesBotColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].braColor = new ColorSetSBPR();
							savedata.coord[i].braColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].shortsColor = new ColorSetSBPR();
							savedata.coord[i].shortsColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].glovesColor = new ColorSetSBPR();
							savedata.coord[i].glovesColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].panstColor = new ColorSetSBPR();
							savedata.coord[i].panstColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].socksColor = new ColorSetSBPR();
							savedata.coord[i].socksColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].shoesColor = new ColorSetSBPR();
							savedata.coord[i].shoesColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].swimsuitColor = new ColorSetSBPR();
							savedata.coord[i].swimsuitColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].swimTopColor = new ColorSetSBPR();
							savedata.coord[i].swimTopColor.Load(binaryReader, (int)convertType);
							savedata.coord[i].swimBotColor = new ColorSetSBPR();
							savedata.coord[i].swimBotColor.Load(binaryReader, (int)convertType);
						}
					}
					if (sex == 0)
					{
						if (version >= 2)
						{
							binaryReader.BaseStream.Seek(1L, SeekOrigin.Current);
						}
					}
					else if (version >= 1)
					{
						savedata.stateSwimOptTop = binaryReader.ReadByte();
						savedata.stateSwimOptBot = binaryReader.ReadByte();
						binaryReader.BaseStream.Seek(1L, SeekOrigin.Current);
					}
					savedata.accessory = new Accessory[2, 5];
					savedata.accessoryColor = new ColorSetSBPR[2, 5];
					for (int j = 0; j < 2; j++)
					{
						for (int k = 0; k < 5; k++)
						{
							savedata.accessory[j, k] = new Accessory();
							savedata.accessory[j, k].Load(binaryReader);
							if (version >= 2)
							{
								savedata.accessoryColor[j, k] = new ColorSetSBPR();
								savedata.accessoryColor[j, k].Load(binaryReader, (int)convertType);
							}
						}
					}
					return true;
				}
			}
		}
	}
}
