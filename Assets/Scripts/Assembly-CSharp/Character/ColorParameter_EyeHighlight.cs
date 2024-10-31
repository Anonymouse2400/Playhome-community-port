using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class ColorParameter_EyeHighlight : ColorParameter_Base
	{
		public Color mainColor1 = Color.white;

		public Color specColor1 = Color.white;

		public float specular1 = 1f;

		public float smooth1 = 1f;

		public ColorParameter_EyeHighlight()
		{
		}

		public ColorParameter_EyeHighlight(ColorParameter_EyeHighlight copy)
		{
			mainColor1 = copy.mainColor1;
			specColor1 = copy.specColor1;
			specular1 = copy.specular1;
			smooth1 = copy.smooth1;
		}

		public ColorParameter_EyeHighlight(Material material)
		{
			bool flag = CheckGloss(material);
			specColor1 = material.GetColor(CustomDataManager._SpecColor);
			if (flag)
			{
				specular1 = material.GetFloat(CustomDataManager._Specular);
				smooth1 = material.GetFloat(CustomDataManager._Gloss);
			}
			else
			{
				mainColor1 = material.color;
			}
		}

		public override COLOR_TYPE GetColorType()
		{
			return COLOR_TYPE.EYEHIGHLIGHT;
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
			switch (cOLOR_TYPE)
			{
			case COLOR_TYPE.NONE:
				return false;
			default:
				if (cOLOR_TYPE != GetColorType())
				{
					Debug.LogError("色タイプが違う");
				}
				break;
			case COLOR_TYPE.PBR1:
				break;
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
			if (CheckGloss(material))
			{
				material.SetFloat(CustomDataManager._Specular, specular1);
				material.SetFloat(CustomDataManager._Gloss, smooth1);
			}
			else
			{
				material.color = mainColor1;
			}
		}

		public static bool CheckGloss(Material material)
		{
			return (UnityEngine.Object)(object)material != null && material.shader.name.IndexOf("glass") != -1;
		}
	}
}
