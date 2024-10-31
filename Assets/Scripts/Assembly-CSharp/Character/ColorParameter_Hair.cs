using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class ColorParameter_Hair : ColorParameter_Base
	{
		public Color mainColor = Color.black;

		public Color cuticleColor = new Color(0.75f, 0.75f, 0.75f);

		public float cuticleExp = 6f;

		public Color fresnelColor = new Color(0.75f, 0.75f, 0.75f);

		public float fresnelExp = 0.3f;

		public ColorParameter_Hair()
		{
		}

		public ColorParameter_Hair(Color mainColor, Color cuticleColor, float cuticleExp, Color fresnelColor, float fresnelExp)
		{
			this.mainColor = mainColor;
			this.cuticleColor = cuticleColor;
			this.cuticleExp = cuticleExp;
			this.fresnelColor = fresnelColor;
			this.fresnelExp = fresnelExp;
		}

		public ColorParameter_Hair(ColorParameter_Hair copy)
		{
			mainColor = copy.mainColor;
			cuticleColor = copy.cuticleColor;
			cuticleExp = copy.cuticleExp;
			fresnelColor = copy.fresnelColor;
			fresnelExp = copy.fresnelExp;
		}

		public override COLOR_TYPE GetColorType()
		{
			return COLOR_TYPE.HAIR;
		}

		public override void Save(BinaryWriter writer)
		{
			Save_ColorType(writer);
			WriteColor(writer, mainColor);
			WriteColor(writer, cuticleColor);
			writer.Write(cuticleExp);
			WriteColor(writer, fresnelColor);
			writer.Write(fresnelExp);
		}

		public override bool Load(BinaryReader reader, CUSTOM_DATA_VERSION version)
		{
			if (version < CUSTOM_DATA_VERSION.DEBUG_04)
			{
				ReadColor(reader, ref mainColor);
				return true;
			}
			COLOR_TYPE cOLOR_TYPE = ColorParameter_Base.Load_ColorType(reader, version);
			if (cOLOR_TYPE != GetColorType())
			{
				Debug.LogError("色タイプが違う");
			}
			ReadColor(reader, ref mainColor);
			ReadColor(reader, ref cuticleColor);
			cuticleExp = reader.ReadSingle();
			ReadColor(reader, ref fresnelColor);
			fresnelExp = reader.ReadSingle();
			return true;
		}

		public void FromSexyData(HSColorSet colorSet)
		{
			mainColor = colorSet.rgbaDiffuse;
			cuticleColor = colorSet.rgbSpecular;
			fresnelColor = colorSet.rgbSpecular;
			cuticleExp = 6f;
			fresnelExp = 0.3f;
		}

		public override void SetToMaterial(Material material)
		{
			material.color = mainColor;
			material.SetColor(CustomDataManager._CuticleColor, cuticleColor);
			material.SetFloat(CustomDataManager._CuticleExp, cuticleExp);
			material.SetColor(CustomDataManager._FrenelColor, fresnelColor);
			material.SetFloat(CustomDataManager._FrenelExp, fresnelExp);
		}
	}
}
