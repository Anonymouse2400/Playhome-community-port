using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	[Serializable]
	public class CharFileInfoCustomFemale : CharFileInfoCustom
	{
		public int texEyeshadowId;

		public HSColorSet eyeshadowColor = new HSColorSet();

		public int texCheekId;

		public HSColorSet cheekColor = new HSColorSet();

		public int texLipId;

		public HSColorSet lipColor = new HSColorSet();

		public int texMoleId;

		public HSColorSet moleColor = new HSColorSet();

		public int matEyelashesId;

		public HSColorSet eyelashesColor = new HSColorSet();

		public int matEyeHiId;

		public HSColorSet eyeHiColor = new HSColorSet();

		public int texSunburnId;

		public HSColorSet sunburnColor = new HSColorSet();

		public int matNipId;

		public HSColorSet nipColor = new HSColorSet();

		public int matUnderhairId;

		public HSColorSet underhairColor = new HSColorSet();

		public HSColorSet nailColor = new HSColorSet();

		public float areolaSize = 0.5f;

		public float bustSoftness = 0.5f;

		public float bustWeight = 0.5f;

		public CharFileInfoCustomFemale()
			: base(Enum.GetNames(typeof(CharDefine.HairKindFemale)).Length)
		{
			sex = 1;
			shapeValueFace = new float[CharDefine.cf_headshapename.Length];
			shapeValueBody = new float[CharDefine.cf_bodyshapename.Length - 1];
			hairId = new int[hairKindNum];
			hairColor = new HSColorSet[hairKindNum];
			hairAcsColor = new HSColorSet[hairKindNum];
			MemberInitialize();
		}

		private new void MemberInitialize()
		{
			base.MemberInitialize();
			texEyeshadowId = 0;
			eyeshadowColor.SetDiffuseRGBA(Color.white);
			texCheekId = 0;
			cheekColor.SetDiffuseRGBA(Color.white);
			texLipId = 0;
			lipColor.SetDiffuseRGBA(Color.white);
			texMoleId = 0;
			moleColor.SetDiffuseRGBA(Color.white);
			matEyelashesId = 0;
			eyelashesColor.SetDiffuseRGBA(Color.white);
			eyelashesColor.hsvDiffuse.S = 1f;
			matEyeHiId = 0;
			eyeHiColor.SetDiffuseRGBA(Color.white);
			texSunburnId = 0;
			sunburnColor.SetDiffuseRGBA(Color.white);
			matNipId = 0;
			nipColor.SetDiffuseRGBA(Color.white);
			nipColor.hsvDiffuse.S = 1f;
			matUnderhairId = 0;
			underhairColor.SetDiffuseRGBA(Color.white);
			underhairColor.hsvDiffuse.S = 1f;
			nailColor.SetDiffuseRGBA(Color.white);
			nailColor.hsvDiffuse.S = 1f;
			areolaSize = 0.5f;
			bustSoftness = 0.5f;
			bustWeight = 0.5f;
			int[] array = new int[4] { 0, 1, 0, 0 };
			for (int i = 0; i < hairId.Length; i++)
			{
				hairId[i] = array[i];
				hairColor[i] = new HSColorSet();
				hairColor[i].hsvDiffuse = new HsvColor(30f, 0.5f, 0.7f);
				hairColor[i].hsvSpecular = new HsvColor(0f, 0f, 0.5f);
				hairColor[i].specularIntensity = 0.4f;
				hairColor[i].specularSharpness = 0.55f;
				hairAcsColor[i] = new HSColorSet();
				hairAcsColor[i].hsvDiffuse = new HsvColor(0f, 0.8f, 1f);
				hairAcsColor[i].hsvSpecular = new HsvColor(0f, 0f, 0.5f);
				hairAcsColor[i].specularIntensity = 0.4f;
				hairAcsColor[i].specularSharpness = 0.55f;
			}
			name = "カスタム娘";
			personality = 0;
		}

		protected override bool LoadSub(BinaryReader br, int customVer, int colorVer)
		{
			texEyeshadowId = br.ReadInt32();
			eyeshadowColor.Load(br, colorVer);
			texCheekId = br.ReadInt32();
			cheekColor.Load(br, colorVer);
			texLipId = br.ReadInt32();
			lipColor.Load(br, colorVer);
			texMoleId = br.ReadInt32();
			moleColor.Load(br, colorVer);
			matEyelashesId = br.ReadInt32();
			eyelashesColor.Load(br, colorVer);
			matEyeHiId = br.ReadInt32();
			eyeHiColor.Load(br, colorVer);
			texSunburnId = br.ReadInt32();
			sunburnColor.Load(br, colorVer);
			matNipId = br.ReadInt32();
			nipColor.Load(br, colorVer);
			matUnderhairId = br.ReadInt32();
			underhairColor.Load(br, colorVer);
			nailColor.Load(br, colorVer);
			areolaSize = br.ReadSingle();
			bustSoftness = br.ReadSingle();
			bustWeight = br.ReadSingle();
			return true;
		}

		protected override bool ModCheckSub()
		{
			if (!CharFileInfoCustom.RangeEqualOn(0f, areolaSize, 1f))
			{
				return true;
			}
			if (!CharFileInfoCustom.RangeEqualOn(0f, bustSoftness, 1f))
			{
				return true;
			}
			if (!CharFileInfoCustom.RangeEqualOn(0f, bustWeight, 1f))
			{
				return true;
			}
			return false;
		}
	}
}
