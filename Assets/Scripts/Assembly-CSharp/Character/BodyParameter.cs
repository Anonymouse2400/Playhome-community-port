using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class BodyParameter : ParameterBase
	{
		public int bodyID;

		public ColorParameter_AlloyHSVOffset skinColor = new ColorParameter_AlloyHSVOffset();

		public int detailID;

		public float detailWeight;

		public int underhairID;

		public ColorParameter_Alloy underhairColor = new ColorParameter_Alloy();

		public int tattooID;

		public Color tattooColor = Color.white;

		public float[] shapeVals;

		public int nipID;

		public ColorParameter_AlloyHSVOffset nipColor = new ColorParameter_AlloyHSVOffset(true, 1f);

		public int sunburnID;

		public float sunburnColor_H;

		public float sunburnColor_S = 1f;

		public float sunburnColor_V = 1f;

		public float sunburnColor_A = 1f;

		public ColorParameter_AlloyHSVOffset nailColor = new ColorParameter_AlloyHSVOffset();

		public ColorParameter_PBR1 manicureColor = new ColorParameter_PBR1();

		public float areolaSize = 0.7f;

		public float bustSoftness = 0.5f;

		public float bustWeight = 0.5f;

		public BodyParameter(SEX sex)
			: base(sex)
		{
			shapeVals = new float[(sex != 0) ? CharDefine.cf_bodyshapename.Length : CharDefine.cf_bodyshapename.Length];
			for (int i = 0; i < shapeVals.Length; i++)
			{
				shapeVals[i] = 0.5f;
			}
			if (sex == SEX.FEMALE)
			{
				shapeVals[shapeVals.Length - 1] = 0f;
			}
		}

		public BodyParameter(BodyParameter copy)
			: base(copy.sex)
		{
			Copy(copy);
		}

		public void Init()
		{
			bodyID = 0;
			skinColor = new ColorParameter_AlloyHSVOffset();
			detailID = 0;
			detailWeight = 0f;
			underhairID = 0;
			underhairColor = new ColorParameter_Alloy();
			tattooID = 0;
			tattooColor = Color.white;
			nailColor = new ColorParameter_AlloyHSVOffset();
			manicureColor = new ColorParameter_PBR1();
			manicureColor.mainColor1.a = 0f;
			areolaSize = 0.5f;
			bustSoftness = 0.5f;
			bustWeight = 0.5f;
			if (shapeVals != null)
			{
				for (int i = 0; i < shapeVals.Length; i++)
				{
					shapeVals[i] = 0.5f;
				}
			}
			nipID = 0;
			nipColor = new ColorParameter_AlloyHSVOffset(true, 1f);
			sunburnID = 0;
			sunburnColor_H = 0f;
			sunburnColor_S = 1f;
			sunburnColor_V = 1f;
			sunburnColor_A = 1f;
		}

		public void Copy(BodyParameter copy)
		{
			if (shapeVals == null || shapeVals.Length != copy.shapeVals.Length)
			{
				shapeVals = new float[copy.shapeVals.Length];
			}
			Array.Copy(copy.shapeVals, shapeVals, shapeVals.Length);
			bodyID = copy.bodyID;
			skinColor = new ColorParameter_AlloyHSVOffset(copy.skinColor);
			detailID = copy.detailID;
			detailWeight = copy.detailWeight;
			underhairID = copy.underhairID;
			underhairColor = new ColorParameter_Alloy(copy.underhairColor);
			tattooID = copy.tattooID;
			tattooColor = copy.tattooColor;
			nipID = copy.nipID;
			nipColor = new ColorParameter_AlloyHSVOffset(copy.nipColor);
			sunburnID = copy.sunburnID;
			sunburnColor_H = copy.sunburnColor_H;
			sunburnColor_S = copy.sunburnColor_S;
			sunburnColor_V = copy.sunburnColor_V;
			sunburnColor_A = copy.sunburnColor_A;
			nailColor = new ColorParameter_AlloyHSVOffset(copy.nailColor);
			manicureColor = new ColorParameter_PBR1(copy.manicureColor);
			areolaSize = copy.areolaSize;
			bustSoftness = copy.bustSoftness;
			bustWeight = copy.bustWeight;
		}

		public float GetHeight()
		{
			return (sex != 0) ? 1f : shapeVals[0];
		}

		public float GetBustSize()
		{
			return (sex != 0) ? 0f : shapeVals[1];
		}

		public void Save(BinaryWriter writer, SEX sex)
		{
			Write(writer, bodyID);
			skinColor.Save(writer);
			Write(writer, detailID);
			Write(writer, detailWeight);
			Write(writer, underhairID);
			underhairColor.Save(writer);
			Write(writer, tattooID);
			Write(writer, tattooColor);
			Write(writer, shapeVals);
			if (sex == SEX.FEMALE)
			{
				Write(writer, nipID);
				nipColor.Save(writer);
				Write(writer, sunburnID);
				Write(writer, sunburnColor_H);
				Write(writer, sunburnColor_S);
				Write(writer, sunburnColor_V);
				Write(writer, sunburnColor_A);
				nailColor.Save(writer);
				manicureColor.Save(writer);
				Write(writer, areolaSize);
				Write(writer, bustSoftness);
				Write(writer, bustWeight);
			}
		}

		public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
		{
			Read(reader, ref bodyID);
			skinColor.Load(reader, version);
			Read(reader, ref detailID);
			Read(reader, ref detailWeight);
			Read(reader, ref underhairID);
			underhairColor.Load(reader, version);
			Read(reader, ref tattooID);
			Read(reader, ref tattooColor);
			Read(reader, ref shapeVals);
			if (sex != 0)
			{
				return;
			}
			Read(reader, ref nipID);
			nipColor.Load(reader, version);
			Read(reader, ref sunburnID);
			if (version < CUSTOM_DATA_VERSION.DEBUG_06)
			{
				Color color = Color.white;
				Read(reader, ref color);
				sunburnColor_H = 0f;
				sunburnColor_S = 1f;
				sunburnColor_V = 1f;
				sunburnColor_A = 1f;
			}
			else
			{
				Read(reader, ref sunburnColor_H);
				Read(reader, ref sunburnColor_S);
				Read(reader, ref sunburnColor_V);
				Read(reader, ref sunburnColor_A);
			}
			if (version >= CUSTOM_DATA_VERSION.DEBUG_03)
			{
				nailColor.Load(reader, version);
				if (version >= CUSTOM_DATA_VERSION.DEBUG_09)
				{
					manicureColor.Load(reader, version);
				}
				else
				{
					manicureColor.mainColor1 = Color.white;
					manicureColor.mainColor1.a = 0f;
					manicureColor.specColor1 = Color.white;
					manicureColor.specular1 = 0f;
					manicureColor.smooth1 = 0f;
				}
				Read(reader, ref areolaSize);
				Read(reader, ref bustSoftness);
				Read(reader, ref bustWeight);
			}
		}
	}
}
