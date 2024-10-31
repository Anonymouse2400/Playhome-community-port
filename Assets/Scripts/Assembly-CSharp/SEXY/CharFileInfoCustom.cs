using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	[Serializable]
	public abstract class CharFileInfoCustom : BlockControlBase
	{
		public byte sex;

		public int customLoadVersion;

		public readonly int hairKindNum;

		public float[] shapeValueFace;

		public float[] shapeValueBody;

		public int[] hairId;

		public int hairType;

		public HSColorSet[] hairColor;

		public HSColorSet[] hairAcsColor;

		public int headId;

		public HSColorSet skinColor = new HSColorSet();

		public int texFaceId;

		public int texTattoo_fId;

		public HSColorSet tattoo_fColor = new HSColorSet();

		public int matEyebrowId;

		public HSColorSet eyebrowColor = new HSColorSet();

		public int matEyeLId;

		public HSColorSet eyeLColor = new HSColorSet();

		public int matEyeRId;

		public HSColorSet eyeRColor = new HSColorSet();

		public HSColorSet eyeWColor = new HSColorSet();

		public int texFaceDetailId;

		public float faceDetailWeight;

		public int texBodyId;

		public int texTattoo_bId;

		public HSColorSet tattoo_bColor = new HSColorSet();

		public int texBodyDetailId;

		public float bodyDetailWeight;

		public string name = string.Empty;

		public int personality;

		public float voicePitch = 1f;

		public bool isConcierge;

		public CharFileInfoCustom(int hknum)
			: base("カスタム情報", 4)
		{
			hairKindNum = hknum;
		}

		protected void MemberInitialize()
		{
			if (shapeValueFace != null)
			{
				for (int i = 0; i < shapeValueFace.Length; i++)
				{
					shapeValueFace[i] = 0.5f;
				}
			}
			if (shapeValueBody != null)
			{
				for (int j = 0; j < shapeValueBody.Length; j++)
				{
					shapeValueBody[j] = 0.5f;
				}
			}
			headId = 0;
			skinColor.SetDiffuseRGB(Color.white);
			skinColor.SetSpecularRGB(Color.white);
			texFaceId = 0;
			texTattoo_fId = 0;
			tattoo_fColor.SetDiffuseRGBA(Color.white);
			matEyebrowId = 0;
			eyebrowColor.SetDiffuseRGBA(Color.white);
			eyebrowColor.hsvDiffuse.S = 1f;
			matEyeLId = 0;
			eyeLColor.SetDiffuseRGBA(Color.white);
			eyeLColor.hsvDiffuse.S = 1f;
			matEyeRId = 0;
			eyeRColor.SetDiffuseRGBA(Color.white);
			eyeRColor.hsvDiffuse.S = 1f;
			eyeWColor.SetDiffuseRGB(Color.white);
			texFaceDetailId = 0;
			faceDetailWeight = 0.5f;
			texBodyId = 0;
			texTattoo_bId = 0;
			tattoo_bColor.SetDiffuseRGBA(Color.white);
			texBodyDetailId = 0;
			bodyDetailWeight = 0f;
			voicePitch = 1f;
			isConcierge = false;
		}

		public override bool LoadBytes(byte[] data, int customVer)
		{
			using (MemoryStream input = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					int colorVer = binaryReader.ReadInt32();
					int num = binaryReader.ReadInt32();
					for (int i = 0; i < num; i++)
					{
						shapeValueFace[i] = Mathf.Clamp(binaryReader.ReadSingle(), 0f, 1f);
					}
					num = binaryReader.ReadInt32();
					for (int j = 0; j < num; j++)
					{
						shapeValueBody[j] = Mathf.Clamp(binaryReader.ReadSingle(), 0f, 1f);
					}
					num = binaryReader.ReadInt32();
					for (int k = 0; k < num; k++)
					{
						hairId[k] = binaryReader.ReadInt32();
						hairColor[k].Load(binaryReader, colorVer);
						hairAcsColor[k].Load(binaryReader, colorVer);
					}
					if (2 <= customVer)
					{
						hairType = binaryReader.ReadInt32();
					}
					headId = binaryReader.ReadInt32();
					skinColor.Load(binaryReader, colorVer);
					texFaceId = binaryReader.ReadInt32();
					texTattoo_fId = binaryReader.ReadInt32();
					tattoo_fColor.Load(binaryReader, colorVer);
					matEyebrowId = binaryReader.ReadInt32();
					eyebrowColor.Load(binaryReader, colorVer);
					matEyeLId = binaryReader.ReadInt32();
					eyeLColor.Load(binaryReader, colorVer);
					matEyeRId = binaryReader.ReadInt32();
					eyeRColor.Load(binaryReader, colorVer);
					eyeWColor.Load(binaryReader, colorVer);
					texFaceDetailId = binaryReader.ReadInt32();
					faceDetailWeight = binaryReader.ReadSingle();
					texBodyId = binaryReader.ReadInt32();
					texTattoo_bId = binaryReader.ReadInt32();
					tattoo_bColor.Load(binaryReader, colorVer);
					texBodyDetailId = binaryReader.ReadInt32();
					bodyDetailWeight = binaryReader.ReadSingle();
					name = binaryReader.ReadString();
					personality = binaryReader.ReadInt32();
					if (3 <= customVer)
					{
						voicePitch = binaryReader.ReadSingle();
					}
					if (4 <= customVer)
					{
						isConcierge = binaryReader.ReadBoolean();
					}
					return LoadSub(binaryReader, customVer, colorVer);
				}
			}
		}

		protected abstract bool LoadSub(BinaryReader br, int customVer, int colorVer);

		public bool ModCheck()
		{
			for (int i = 0; i < shapeValueFace.Length; i++)
			{
				if (!RangeEqualOn(0f, shapeValueFace[i], 1f))
				{
					return true;
				}
			}
			for (int j = 0; j < shapeValueBody.Length; j++)
			{
				if (!RangeEqualOn(0f, shapeValueBody[j], 1f))
				{
					return true;
				}
			}
			return ModCheckSub();
		}

		protected abstract bool ModCheckSub();

		protected static bool RangeEqualOn(float min, float val, float max)
		{
			return val >= min && val <= max;
		}
	}
}
