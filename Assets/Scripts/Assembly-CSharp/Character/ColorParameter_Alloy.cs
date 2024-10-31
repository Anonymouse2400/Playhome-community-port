using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class ColorParameter_Alloy : ColorParameter_Base
	{
		public Color mainColor = Color.white;

		public float metallic;

		public float smooth;

		public ColorParameter_Alloy()
		{
		}

		public ColorParameter_Alloy(ColorParameter_Alloy copy)
		{
			mainColor = copy.mainColor;
			metallic = copy.metallic;
			smooth = copy.smooth;
		}

		public override COLOR_TYPE GetColorType()
		{
			return COLOR_TYPE.ALLOY;
		}

		public override void Save(BinaryWriter writer)
		{
			Save_ColorType(writer);
			WriteColor(writer, mainColor);
			writer.Write(metallic);
			writer.Write(smooth);
		}

		public override bool Load(BinaryReader reader, CUSTOM_DATA_VERSION version)
		{
			if (version < CUSTOM_DATA_VERSION.DEBUG_04)
			{
				ReadColor(reader, ref mainColor);
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
			ReadColor(reader, ref mainColor);
			metallic = reader.ReadSingle();
			smooth = reader.ReadSingle();
			return true;
		}

		public void FromSexyData(HSColorSet colorSet)
		{
			mainColor = colorSet.rgbaDiffuse;
			metallic = colorSet.specularIntensity;
			smooth = colorSet.specularSharpness;
		}

		public override void SetToMaterial(Material material)
		{
			material.color = mainColor;
			material.SetFloat(CustomDataManager._Metal, metallic);
			material.SetFloat(CustomDataManager._Roughness, 1f - smooth);
		}
	}
}
