using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class HeadParameter : ParameterBase
	{
		public int headID;

		public int faceTexID;

		public int detailID;

		public float detailWeight;

		public int eyeBrowID;

		public ColorParameter_PBR1 eyeBrowColor = new ColorParameter_PBR1();

		public int eyeID_L;

		public Color eyeScleraColorL = Color.white;

		public Color eyeIrisColorL = Color.black;

		public float eyePupilDilationL;

		public float eyeEmissiveL = 0.5f;

		public int eyeID_R;

		public Color eyeScleraColorR = Color.white;

		public Color eyeIrisColorR = Color.black;

		public float eyePupilDilationR;

		public float eyeEmissiveR = 0.5f;

		public int tattooID;

		public Color tattooColor = Color.white;

		public float[] shapeVals;

		public int eyeLashID;

		public ColorParameter_PBR1 eyeLashColor = new ColorParameter_PBR1();

		public int eyeshadowTexID;

		public Color eyeshadowColor = Color.black;

		public int cheekTexID;

		public Color cheekColor = Color.white;

		public int lipTexID;

		public Color lipColor = Color.white;

		public int moleTexID;

		public Color moleColor = Color.white;

		public int eyeHighlightTexID;

		public ColorParameter_EyeHighlight eyeHighlightColor = new ColorParameter_EyeHighlight();

		public int beardID;

		public Color beardColor = Color.black;

		public HeadParameter(SEX sex)
			: base(sex)
		{
			shapeVals = new float[(sex != 0) ? CharDefine.cm_headshapename.Length : CharDefine.cf_headshapename.Length];
			for (int i = 0; i < shapeVals.Length; i++)
			{
				shapeVals[i] = 0.5f;
			}
		}

		public HeadParameter(HeadParameter copy)
			: base(copy.sex)
		{
			Copy(copy);
		}

		public void Init()
		{
			headID = 0;
			faceTexID = 0;
			detailID = 0;
			detailWeight = 0f;
			eyeBrowID = 0;
			eyeBrowColor = new ColorParameter_PBR1();
			eyeScleraColorL = Color.white;
			eyeID_L = 0;
			eyeIrisColorL = Color.black;
			eyeScleraColorR = Color.white;
			eyeID_R = 0;
			eyeIrisColorR = Color.black;
			tattooID = 0;
			tattooColor = Color.white;
			if (shapeVals != null)
			{
				for (int i = 0; i < shapeVals.Length; i++)
				{
					shapeVals[i] = 0.5f;
				}
			}
			eyeLashID = 0;
			eyeLashColor = new ColorParameter_PBR1();
			eyeshadowTexID = 0;
			eyeshadowColor = Color.black;
			cheekTexID = 0;
			cheekColor = Color.white;
			lipTexID = 0;
			lipColor = Color.white;
			moleTexID = 0;
			moleColor = Color.white;
			eyeHighlightTexID = 0;
			eyeHighlightColor = new ColorParameter_EyeHighlight();
			beardID = 0;
			beardColor = Color.black;
		}

		public void Copy(HeadParameter copy)
		{
			if (shapeVals == null || shapeVals.Length != copy.shapeVals.Length)
			{
				shapeVals = new float[copy.shapeVals.Length];
			}
			Array.Copy(copy.shapeVals, shapeVals, shapeVals.Length);
			headID = copy.headID;
			faceTexID = copy.faceTexID;
			detailID = copy.detailID;
			detailWeight = copy.detailWeight;
			eyeBrowID = copy.eyeBrowID;
			eyeBrowColor = copy.eyeBrowColor;
			eyeID_L = copy.eyeID_L;
			eyeScleraColorL = copy.eyeScleraColorL;
			eyeIrisColorL = copy.eyeIrisColorL;
			eyePupilDilationL = copy.eyePupilDilationL;
			eyeEmissiveL = copy.eyeEmissiveL;
			eyeID_R = copy.eyeID_R;
			eyeScleraColorR = copy.eyeScleraColorR;
			eyeIrisColorR = copy.eyeIrisColorR;
			eyePupilDilationR = copy.eyePupilDilationR;
			eyeEmissiveR = copy.eyeEmissiveR;
			tattooID = copy.tattooID;
			tattooColor = copy.tattooColor;
			eyeLashID = copy.eyeLashID;
			eyeLashColor = copy.eyeLashColor;
			eyeshadowTexID = copy.eyeshadowTexID;
			eyeshadowColor = copy.eyeshadowColor;
			cheekTexID = copy.cheekTexID;
			cheekColor = copy.cheekColor;
			lipTexID = copy.lipTexID;
			lipColor = copy.lipColor;
			moleTexID = copy.moleTexID;
			moleColor = copy.moleColor;
			eyeHighlightTexID = copy.eyeHighlightTexID;
			eyeHighlightColor = copy.eyeHighlightColor;
			beardID = copy.beardID;
			beardColor = copy.beardColor;
		}

		public void Save(BinaryWriter writer, SEX sex)
		{
			Write(writer, headID);
			Write(writer, faceTexID);
			Write(writer, detailID);
			Write(writer, detailWeight);
			Write(writer, eyeBrowID);
			eyeBrowColor.Save(writer);
			Write(writer, eyeID_L);
			Write(writer, eyeScleraColorL);
			Write(writer, eyeIrisColorL);
			Write(writer, eyePupilDilationL);
			Write(writer, eyeEmissiveL);
			Write(writer, eyeID_R);
			Write(writer, eyeScleraColorR);
			Write(writer, eyeIrisColorR);
			Write(writer, eyePupilDilationR);
			Write(writer, eyeEmissiveR);
			Write(writer, tattooID);
			Write(writer, tattooColor);
			Write(writer, shapeVals);
			switch (sex)
			{
			case SEX.FEMALE:
				Write(writer, eyeLashID);
				eyeLashColor.Save(writer);
				Write(writer, eyeshadowTexID);
				Write(writer, eyeshadowColor);
				Write(writer, cheekTexID);
				Write(writer, cheekColor);
				Write(writer, lipTexID);
				Write(writer, lipColor);
				Write(writer, moleTexID);
				Write(writer, moleColor);
				Write(writer, eyeHighlightTexID);
				eyeHighlightColor.Save(writer);
				break;
			case SEX.MALE:
				Write(writer, beardID);
				Write(writer, beardColor);
				eyeHighlightColor.Save(writer);
				break;
			}
		}

		public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
		{
			Color white = Color.white;
			Read(reader, ref headID);
			Read(reader, ref faceTexID);
			Read(reader, ref detailID);
			Read(reader, ref detailWeight);
			Read(reader, ref eyeBrowID);
			eyeBrowColor.Load(reader, version);
			eyePupilDilationL = 0f;
			eyePupilDilationR = 0f;
			eyeEmissiveL = 0.5f;
			eyeEmissiveR = 0.5f;
			if (version < CUSTOM_DATA_VERSION.DEBUG_04)
			{
				Read(reader, ref eyeScleraColorL);
				eyeScleraColorR = eyeScleraColorL;
				Read(reader, ref eyeID_L);
				Read(reader, ref eyeIrisColorL);
				Read(reader, ref eyeID_R);
				Read(reader, ref eyeIrisColorR);
			}
			else
			{
				Read(reader, ref eyeID_L);
				Read(reader, ref eyeScleraColorL);
				Read(reader, ref eyeIrisColorL);
				Read(reader, ref eyePupilDilationL);
				if (version >= CUSTOM_DATA_VERSION.DEBUG_10)
				{
					Read(reader, ref eyeEmissiveL);
				}
				Read(reader, ref eyeID_R);
				Read(reader, ref eyeScleraColorR);
				Read(reader, ref eyeIrisColorR);
				Read(reader, ref eyePupilDilationR);
				if (version >= CUSTOM_DATA_VERSION.DEBUG_10)
				{
					Read(reader, ref eyeEmissiveR);
				}
			}
			Read(reader, ref tattooID);
			Read(reader, ref tattooColor);
			Read(reader, ref shapeVals);
			switch (sex)
			{
			case SEX.FEMALE:
				Read(reader, ref eyeLashID);
				eyeLashColor.Load(reader, version);
				Read(reader, ref eyeshadowTexID);
				Read(reader, ref eyeshadowColor);
				Read(reader, ref cheekTexID);
				Read(reader, ref cheekColor);
				Read(reader, ref lipTexID);
				Read(reader, ref lipColor);
				Read(reader, ref moleTexID);
				Read(reader, ref moleColor);
				Read(reader, ref eyeHighlightTexID);
				eyeHighlightColor.Load(reader, version);
				break;
			case SEX.MALE:
				Read(reader, ref beardID);
				Read(reader, ref beardColor);
				if (version >= CUSTOM_DATA_VERSION.DEBUG_02)
				{
					eyeHighlightColor.Load(reader, version);
				}
				break;
			}
		}

		public bool CheckEyeEqual()
		{
			return eyeID_L == eyeID_R && eyeScleraColorL == eyeScleraColorR && eyeIrisColorL == eyeIrisColorR && eyePupilDilationL == eyePupilDilationR;
		}
	}
}
