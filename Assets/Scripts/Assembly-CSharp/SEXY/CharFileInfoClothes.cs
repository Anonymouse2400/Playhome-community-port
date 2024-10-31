using System;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace SEXY
{
	[Serializable]
	public abstract class CharFileInfoClothes
	{
		[Serializable]
		public class Accessory : ISerializable
		{
			public int type = -1;

			public int id = -1;

			public string parentKey = string.Empty;

			public Vector3 addPos = Vector3.zero;

			public Vector3 addRot = Vector3.zero;

			public Vector3 addScl = Vector3.one;

			public HSColorSet color = new HSColorSet();

			public HSColorSet color2 = new HSColorSet();

			public Accessory()
			{
			}

			protected Accessory(SerializationInfo info, StreamingContext context)
			{
				type = info.GetInt32("type");
				id = info.GetInt32("id");
				parentKey = info.GetString("parentKey");
				addPos.x = info.GetSingle("addPosX");
				addPos.y = info.GetSingle("addPosY");
				addPos.z = info.GetSingle("addPosZ");
				addRot.x = info.GetSingle("addRotX");
				addRot.y = info.GetSingle("addRotY");
				addRot.z = info.GetSingle("addRotZ");
				addScl.x = info.GetSingle("addSclX");
				addScl.y = info.GetSingle("addSclY");
				addScl.z = info.GetSingle("addSclZ");
				color.hsvDiffuse.H = info.GetSingle("diffuseH");
				color.hsvDiffuse.S = info.GetSingle("diffuseS");
				color.hsvDiffuse.V = info.GetSingle("diffuseV");
				color.alpha = info.GetSingle("alpha");
				color.hsvSpecular.H = info.GetSingle("specularH");
				color.hsvSpecular.S = info.GetSingle("specularS");
				color.hsvSpecular.V = info.GetSingle("specularV");
				color.specularIntensity = info.GetSingle("specularIntensity");
				color.specularSharpness = info.GetSingle("specularSharpness");
				color2.hsvDiffuse.H = info.GetSingle("diffuseH2");
				color2.hsvDiffuse.S = info.GetSingle("diffuseS2");
				color2.hsvDiffuse.V = info.GetSingle("diffuseV2");
				color2.alpha = info.GetSingle("alpha2");
				color2.hsvSpecular.H = info.GetSingle("specularH2");
				color2.hsvSpecular.S = info.GetSingle("specularS2");
				color2.hsvSpecular.V = info.GetSingle("specularV2");
				color2.specularIntensity = info.GetSingle("specularIntensity2");
				color2.specularSharpness = info.GetSingle("specularSharpness2");
			}

			public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("type", type);
				info.AddValue("id", id);
				info.AddValue("parentKey", parentKey);
				info.AddValue("addPosX", addPos.x);
				info.AddValue("addPosY", addPos.y);
				info.AddValue("addPosZ", addPos.z);
				info.AddValue("addRotX", addRot.x);
				info.AddValue("addRotY", addRot.y);
				info.AddValue("addRotZ", addRot.z);
				info.AddValue("addSclX", addScl.x);
				info.AddValue("addSclY", addScl.y);
				info.AddValue("addSclZ", addScl.z);
				info.AddValue("diffuseH", color.hsvDiffuse.H);
				info.AddValue("diffuseS", color.hsvDiffuse.S);
				info.AddValue("diffuseV", color.hsvDiffuse.V);
				info.AddValue("alpha", color.alpha);
				info.AddValue("specularH", color.hsvSpecular.H);
				info.AddValue("specularS", color.hsvSpecular.S);
				info.AddValue("specularV", color.hsvSpecular.V);
				info.AddValue("specularIntensity", color.specularIntensity);
				info.AddValue("specularSharpness", color.specularSharpness);
				info.AddValue("diffuseH2", color2.hsvDiffuse.H);
				info.AddValue("diffuseS2", color2.hsvDiffuse.S);
				info.AddValue("diffuseV2", color2.hsvDiffuse.V);
				info.AddValue("alpha2", color2.alpha);
				info.AddValue("specularH2", color2.hsvSpecular.H);
				info.AddValue("specularS2", color2.hsvSpecular.S);
				info.AddValue("specularV2", color2.hsvSpecular.V);
				info.AddValue("specularIntensity2", color2.specularIntensity);
				info.AddValue("specularSharpness2", color2.specularSharpness);
			}

			public void MemberInitialize()
			{
				type = -1;
				id = -1;
				parentKey = string.Empty;
				addPos = Vector3.zero;
				addRot = Vector3.zero;
				addScl = Vector3.one;
				color = new HSColorSet();
				color2 = new HSColorSet();
			}

			public void Copy(Accessory src)
			{
				type = src.type;
				id = src.id;
				parentKey = src.parentKey;
				addPos = src.addPos;
				addRot = src.addRot;
				addScl = src.addScl;
				color.Copy(src.color);
				color2.Copy(src.color2);
			}

			public void Load(BinaryReader reader, int verClothes, int verColor)
			{
				type = reader.ReadInt32();
				id = reader.ReadInt32();
				parentKey = reader.ReadString();
				addPos.x = reader.ReadSingle();
				addPos.y = reader.ReadSingle();
				addPos.z = reader.ReadSingle();
				addRot.x = reader.ReadSingle();
				addRot.y = reader.ReadSingle();
				addRot.z = reader.ReadSingle();
				addScl.x = reader.ReadSingle();
				addScl.y = reader.ReadSingle();
				addScl.z = reader.ReadSingle();
				color.Load(reader, verColor);
				if (3 <= verClothes)
				{
					color2.Load(reader, verColor);
				}
			}
		}

		public const int ClothesFileVersion = 3;

		public readonly string ClothesFileMark = string.Empty;

		public readonly string ClothesFileDirectory = string.Empty;

		public byte[] clothesPNG;

		public string clothesFileName = string.Empty;

		public int clothesLoadFileVersion;

		public readonly int clothesKindNum;

		public int[] clothesId;

		public HSColorSet[] clothesColor;

		public HSColorSet[] clothesColor2;

		public string comment = string.Empty;

		public byte clothesTypeSex;

		private Accessory[] _accessory;

		public Accessory[] accessory
		{
			get
			{
				return _accessory;
			}
			private set
			{
				_accessory = value;
			}
		}

		public CharFileInfoClothes(string fileMarkName, string fileDirectory, int cknum)
		{
			ClothesFileMark = fileMarkName;
			ClothesFileDirectory = fileDirectory;
			clothesKindNum = cknum;
			clothesId = new int[clothesKindNum];
			clothesColor = new HSColorSet[clothesKindNum];
			clothesColor2 = new HSColorSet[clothesKindNum];
			accessory = new Accessory[10];
			for (int i = 0; i < accessory.Length; i++)
			{
				accessory[i] = new Accessory();
			}
			comment = "コーディネート名";
		}

		protected void MemberInitialize()
		{
		}

		public abstract bool Copy(CharFileInfoClothes srcData);

		public bool Load(string path, bool noSetPng = false)
		{
			bool result = false;
			if (!File.Exists(path))
			{
				return result;
			}
			clothesFileName = Path.GetFileName(path);
			using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(input))
				{
					return Load(br, noSetPng);
				}
			}
		}

		public bool Load(TextAsset ta, bool noSetPNG = false)
		{
			if (null == ta)
			{
				return false;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(ta.bytes, 0, ta.bytes.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return Load(memoryStream, noSetPNG);
			}
		}

		public bool Load(Stream st, bool noSetPNG = false)
		{
			using (BinaryReader br = new BinaryReader(st))
			{
				return Load(br, noSetPNG);
			}
		}

		public bool Load(BinaryReader br, bool noSetPng = false)
		{
			long size = 0L;
			if (noSetPng)
			{
				PngAssist.CheckPngData(br, ref size, true);
			}
			else
			{
				clothesPNG = PngAssist.LoadPngData(br);
			}
			if (!LoadWithoutPNG(br))
			{
				return false;
			}
			size = br.ReadInt64();
			return true;
		}

		public bool LoadWithoutPNG(BinaryReader br)
		{
			if (br.BaseStream.Length - br.BaseStream.Position == 0)
			{
				Debug.LogWarning("ただのPNGファイルの可能性があります。");
				return false;
			}
			string text = br.ReadString();
			if (text != ClothesFileMark)
			{
				Debug.LogError("ファイルの種類が違います。");
				return false;
			}
			clothesLoadFileVersion = br.ReadInt32();
			if (clothesLoadFileVersion > 3)
			{
				Debug.LogError("実行ファイルよりも新しいキャラファイルです。");
				return false;
			}
			int num = br.ReadInt32();
			int num2 = br.ReadInt32();
			for (int i = 0; i < num2; i++)
			{
				clothesId[i] = br.ReadInt32();
				clothesColor[i].Load(br, num);
				if (3 <= clothesLoadFileVersion)
				{
					clothesColor2[i].Load(br, num);
				}
			}
			num2 = br.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				accessory[j].Load(br, clothesLoadFileVersion, num);
			}
			if (2 <= clothesLoadFileVersion)
			{
				comment = br.ReadString();
				clothesTypeSex = br.ReadByte();
			}
			return LoadSub(br, clothesLoadFileVersion, num);
		}

		protected abstract bool LoadSub(BinaryReader br, int clothesVer, int colorVer);
	}
}
