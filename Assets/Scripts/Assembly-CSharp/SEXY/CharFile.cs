using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	public abstract class CharFile
	{
		public readonly string CharaFileMark = string.Empty;

		public readonly string CharaFileDirectory = string.Empty;

		public const int CharaFileVersion = 2;

		public byte[] charaFilePNG;

		public string charaFileName = string.Empty;

		public int charaLoadFileVersion;

		public string blockFileName = string.Empty;

		public CharFileInfoPreview previewInfo { get; protected set; }

		public CharFileInfoCustom customInfo { get; protected set; }

		public CharFileInfoCoordinate coordinateInfo { get; protected set; }

		public CharFileInfoClothes clothesInfo { get; protected set; }

		public CharFile(string fileMarkName, string fileDirectory)
		{
			CharaFileMark = fileMarkName;
			CharaFileDirectory = fileDirectory;
			previewInfo = new CharFileInfoPreview();
			customInfo = null;
			coordinateInfo = null;
			clothesInfo = null;
		}

		public bool SetClothesInfo(CharFileInfoClothes info)
		{
			return clothesInfo.Copy(info);
		}

		public bool SetCoordinateInfo(CharDefine.CoordinateType type)
		{
			if (coordinateInfo == null)
			{
				return false;
			}
			return coordinateInfo.SetInfo(type, clothesInfo);
		}

		public static bool CheckSBPRFile(string path)
		{
			CharSave_SexyBeachPR charSave_SexyBeachPR = new CharSave_SexyBeachPR(CharSave_SexyBeachPR.ConvertType.HoneySelect);
			string fileTag = charSave_SexyBeachPR.GetFileTag(path);
			if ("【PremiumResortCharaFemale】" != fileTag && "【PremiumResortCharaMale】" != fileTag)
			{
				return false;
			}
			return true;
		}

		public static int CheckSBPRFileWithSex(string path)
		{
			CharSave_SexyBeachPR charSave_SexyBeachPR = new CharSave_SexyBeachPR(CharSave_SexyBeachPR.ConvertType.HoneySelect);
			string fileTag = charSave_SexyBeachPR.GetFileTag(path);
			if ("【PremiumResortCharaFemale】" == fileTag)
			{
				return 1;
			}
			if ("【PremiumResortCharaMale】" == fileTag)
			{
				return 0;
			}
			return -1;
		}

		public abstract bool LoadFromSBPR(BinaryReader br);

		public static int CheckHoneySelectCharaFile(string path)
		{
			if (!File.Exists(path))
			{
				return -1;
			}
			using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					if (binaryReader.BaseStream.Length == 0)
					{
						Debug.LogError("データが破損しています");
						return -1;
					}
					long size = 0L;
					PngAssist.CheckPngData(binaryReader, ref size, true);
					if (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position == 0)
					{
						Debug.LogWarning("ただのPNGファイルの可能性があります。");
						return -1;
					}
					string text = binaryReader.ReadString();
					int num = binaryReader.ReadInt32();
					if ("【HoneySelectCharaMale】" == text)
					{
						if (num == 1)
						{
							return 0;
						}
						return 2;
					}
					if ("【HoneySelectCharaFemale】" == text)
					{
						if (num == 1)
						{
							return 1;
						}
						return 3;
					}
					return -1;
				}
			}
		}

		public bool Load(string path = "", bool noSetPNG = false, bool noLoadStatus = true)
		{
			if (!File.Exists(path))
			{
				return false;
			}
			using (FileStream st = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				return Load(st, noSetPNG, noLoadStatus);
			}
		}

		public bool Load(TextAsset ta, bool noSetPNG = false, bool noLoadStatus = true)
		{
			if (null == ta)
			{
				return false;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(ta.bytes, 0, ta.bytes.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return Load(memoryStream, noSetPNG, noLoadStatus);
			}
		}

		public bool Load(Stream st, bool noSetPNG = false, bool noLoadStatus = true)
		{
			using (BinaryReader reader = new BinaryReader(st))
			{
				return Load(reader, noSetPNG, noLoadStatus);
			}
		}

		public bool Load(BinaryReader reader, bool noSetPNG = false, bool noLoadStatus = true)
		{
			if (reader.BaseStream.Length == 0)
			{
				Debug.LogError("データが破損しています");
				return false;
			}
			long size = 0L;
			if (noSetPNG)
			{
				PngAssist.CheckPngData(reader, ref size, true);
			}
			else
			{
				charaFilePNG = PngAssist.LoadPngData(reader);
			}
			if (reader.BaseStream.Length - reader.BaseStream.Position == 0)
			{
				Debug.LogWarning("ただのPNGファイルの可能性があります。");
				return false;
			}
			string text = reader.ReadString();
			if (text != CharaFileMark)
			{
				Debug.LogError("ファイルの種類が違います");
				return false;
			}
			charaLoadFileVersion = reader.ReadInt32();
			if (charaLoadFileVersion > 2)
			{
				Debug.LogError("実行ファイルよりも新しいキャラファイルです");
				return false;
			}
			int num = reader.ReadInt32();
			BlockHeader[] array = new BlockHeader[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new BlockHeader();
				array[i].LoadHeader(reader);
			}
			int num2 = 0;
			long position = reader.BaseStream.Position;
			previewInfo.previewLoadVersion = array[num2].version;
			num2++;
			customInfo.customLoadVersion = array[num2].version;
			if (customInfo.customLoadVersion > customInfo.version)
			{
				Debug.LogError("実行ファイルよりも新しいカスタム情報です。");
			}
			else
			{
				reader.BaseStream.Seek(position + array[num2].pos, SeekOrigin.Begin);
				byte[] data = reader.ReadBytes((int)array[num2].size);
				if (!customInfo.LoadBytes(data, array[num2].version))
				{
					Debug.LogError("カスタム情報の読み込みに失敗しました");
					return false;
				}
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(charaFileName);
			if ("ill_Sitri" != fileNameWithoutExtension)
			{
				customInfo.isConcierge = false;
			}
			num2++;
			coordinateInfo.coordinateLoadVersion = array[num2].version;
			if (coordinateInfo.coordinateLoadVersion > coordinateInfo.version)
			{
				Debug.LogError("実行ファイルよりも新しいコーディネート情報です。");
			}
			else
			{
				reader.BaseStream.Seek(position + array[num2].pos, SeekOrigin.Begin);
				byte[] data2 = reader.ReadBytes((int)array[num2].size);
				if (!coordinateInfo.LoadBytes(data2, array[num2].version))
				{
					Debug.LogError("コーディネート情報の読み込みに失敗しました");
					return false;
				}
			}
			num2++;
			num2++;
			long offset = position + array[num2].pos + array[num2].size;
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			if (2 <= charaLoadFileVersion)
			{
				reader.BaseStream.Seek(32L, SeekOrigin.Current);
			}
			reader.BaseStream.Seek(8L, SeekOrigin.Current);
			reader.BaseStream.Seek(8L, SeekOrigin.Current);
			return true;
		}

		public bool LoadBlockData<T>(T blockInfo, string path) where T : BlockControlBase
		{
			if (blockInfo == null)
			{
				return false;
			}
			if (!File.Exists(path))
			{
				return false;
			}
			blockFileName = Path.GetFileNameWithoutExtension(path);
			using (FileStream st = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				return LoadBlockData(blockInfo, st);
			}
		}

		public bool LoadBlockData<T>(T blockInfo, TextAsset ta) where T : BlockControlBase
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(ta.bytes, 0, ta.bytes.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return LoadBlockData(blockInfo, memoryStream);
			}
		}

		public bool LoadBlockData<T>(T blockInfo, Stream st) where T : BlockControlBase
		{
			using (BinaryReader reader = new BinaryReader(st))
			{
				return LoadBlockData(blockInfo, reader);
			}
		}

		public bool LoadBlockData<T>(T blockInfo, BinaryReader reader) where T : BlockControlBase
		{
			if (reader.BaseStream.Length == 0)
			{
				Debug.LogError("データが破損しています。");
				return false;
			}
			long size = 0L;
			PngAssist.CheckPngData(reader, ref size, true);
			if (reader.BaseStream.Length - reader.BaseStream.Position == 0)
			{
				Debug.LogWarning("ただのPNGファイルの可能性があります。");
				return false;
			}
			string text = reader.ReadString();
			if (text != CharaFileMark)
			{
				Debug.LogError("ファイルの種類が違います");
				return false;
			}
			charaLoadFileVersion = reader.ReadInt32();
			if (charaLoadFileVersion > 2)
			{
				Debug.LogError("実行ファイルよりも新しいキャラファイルです");
				return false;
			}
			int num = reader.ReadInt32();
			BlockHeader[] array = new BlockHeader[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new BlockHeader();
				array[i].LoadHeader(reader);
			}
			long position = reader.BaseStream.Position;
			int num2 = -1;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].tagName.StartsWith(blockInfo.tagName))
				{
					num2 = j;
					break;
				}
			}
			if (num2 == -1)
			{
				return false;
			}
			int version = array[num2].version;
			if (version > blockInfo.version)
			{
				Debug.LogError("実行ファイルよりも新しい情報です。");
				return false;
			}
			reader.BaseStream.Seek(position + array[num2].pos, SeekOrigin.Begin);
			byte[] data = reader.ReadBytes((int)array[num2].size);
			blockInfo.LoadBytes(data, version);
			if ("ill_Sitri" != blockFileName)
			{
				if (blockInfo is CharFileInfoPreview)
				{
					CharFileInfoPreview charFileInfoPreview = blockInfo as CharFileInfoPreview;
					charFileInfoPreview.isConcierge = 0;
				}
				else if (blockInfo is CharFileInfoCustomFemale)
				{
					CharFileInfoCustomFemale charFileInfoCustomFemale = blockInfo as CharFileInfoCustomFemale;
					charFileInfoCustomFemale.isConcierge = false;
				}
			}
			return true;
		}
	}
}
