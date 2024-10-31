using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	[Serializable]
	public class CharFileInfoCustomMale : CharFileInfoCustom
	{
		public int matBeardId;

		public HSColorSet beardColor = new HSColorSet();

		public CharFileInfoCustomMale()
			: base(Enum.GetNames(typeof(CharDefine.HairKindMale)).Length)
		{
			sex = 0;
			shapeValueFace = new float[CharDefine.cm_headshapename.Length];
			shapeValueBody = new float[CharDefine.cm_bodyshapename.Length];
			hairId = new int[hairKindNum];
			hairColor = new HSColorSet[hairKindNum];
			hairAcsColor = new HSColorSet[hairKindNum];
			MemberInitialize();
		}

		private new void MemberInitialize()
		{
			base.MemberInitialize();
			matBeardId = 0;
			beardColor.SetDiffuseRGBA(Color.white);
			beardColor.hsvDiffuse.S = 1f;
			int[] array = new int[1];
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
			name = "俺";
			personality = 99;
		}

		protected override bool LoadSub(BinaryReader br, int customVer, int colorVer)
		{
			matBeardId = br.ReadInt32();
			beardColor.Load(br, colorVer);
			return true;
		}

		protected override bool ModCheckSub()
		{
			return false;
		}
	}
}
