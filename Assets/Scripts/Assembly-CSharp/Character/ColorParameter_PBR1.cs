using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class ColorParameter_PBR1 : ColorParameter_Base
	{
		public Color mainColor1 = Color.white;

		public Color specColor1 = Color.white;

		public float specular1;

		public float smooth1;

		public ColorParameter_PBR1()
		{
		}

		public ColorParameter_PBR1(ColorParameter_PBR1 copy)
		{
			Copy(copy);
		}

		public ColorParameter_PBR1(Material material)
		{
			mainColor1 = material.color;
			specColor1 = material.GetColor(CustomDataManager._SpecColor);
			specular1 = material.GetFloat(CustomDataManager._Metallic);
			smooth1 = material.GetFloat(CustomDataManager._Smoothness);
		}

		public override COLOR_TYPE GetColorType()
		{
			return COLOR_TYPE.PBR1;
		}

		public void Copy(ColorParameter_PBR1 copy)
		{
			mainColor1 = copy.mainColor1;
			specColor1 = copy.specColor1;
			specular1 = copy.specular1;
			smooth1 = copy.smooth1;
		}

		public override void Save(BinaryWriter writer)
		{
			Save_ColorType(writer);
			WriteColor(writer, mainColor1);
			WriteColor(writer, specColor1);
			writer.Write(specular1);
			writer.Write(smooth1);
		}

		public override bool Load(BinaryReader reader, CUSTOM_DATA_VERSION version)
		{
			if (version < CUSTOM_DATA_VERSION.DEBUG_04)
			{
				ReadColor(reader, ref mainColor1);
				return true;
			}
			COLOR_TYPE cOLOR_TYPE = ColorParameter_Base.Load_ColorType(reader, version);
			if (cOLOR_TYPE == COLOR_TYPE.NONE)
			{
				return false;
			}
			if (cOLOR_TYPE != GetColorType())
			{
				Debug.LogError("色タイプが違う");
			}
			ReadColor(reader, ref mainColor1);
			ReadColor(reader, ref specColor1);
			specular1 = reader.ReadSingle();
			smooth1 = reader.ReadSingle();
			return true;
		}

		public void FromSexyData(HSColorSet colorSet)
		{
			mainColor1 = colorSet.rgbaDiffuse;
			specColor1 = colorSet.rgbSpecular;
			specular1 = colorSet.specularIntensity;
			smooth1 = colorSet.specularSharpness;
		}

		public override void SetToMaterial(Material material)
		{
			material.color = mainColor1;
			material.SetColor(CustomDataManager._SpecColor, specColor1);
			material.SetFloat(CustomDataManager._Metallic, specular1);
			material.SetFloat(CustomDataManager._Smoothness, smooth1);
		}
	}
}
