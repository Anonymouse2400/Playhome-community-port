using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class ColorParameter_PBR2 : ColorParameter_Base
	{
		public Color mainColor1 = Color.white;

		public Color specColor1 = Color.white;

		public float specular1;

		public float smooth1;

		public Color mainColor2 = Color.white;

		public Color specColor2 = Color.white;

		public float specular2;

		public float smooth2;

		public ColorParameter_PBR2()
		{
		}

		public ColorParameter_PBR2(ColorParameter_PBR2 src)
		{
			mainColor1 = src.mainColor1;
			specColor1 = src.specColor1;
			specular1 = src.specular1;
			smooth1 = src.smooth1;
			mainColor2 = src.mainColor2;
			specColor2 = src.specColor2;
			specular2 = src.specular2;
			smooth2 = src.smooth2;
		}

		public ColorParameter_PBR2(MaterialCustoms custom)
		{
			mainColor1 = custom.GetColor("MainColor");
			specColor1 = custom.GetColor("MainSpecColor");
			specular1 = custom.GetFloat("MainMetallic");
			smooth1 = custom.GetFloat("MainSmoothness");
			mainColor2 = custom.GetColor("SubColor");
			specColor2 = custom.GetColor("SubSpecColor");
			specular2 = custom.GetFloat("SubMetallic");
			smooth2 = custom.GetFloat("SubSmoothness");
		}

		public override COLOR_TYPE GetColorType()
		{
			return COLOR_TYPE.PBR2;
		}

		public override void Save(BinaryWriter writer)
		{
			Save_ColorType(writer);
			WriteColor(writer, mainColor1);
			WriteColor(writer, specColor1);
			writer.Write(specular1);
			writer.Write(smooth1);
			WriteColor(writer, mainColor2);
			WriteColor(writer, specColor2);
			writer.Write(specular2);
			writer.Write(smooth2);
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
			ReadColor(reader, ref mainColor2);
			ReadColor(reader, ref specColor2);
			if (version < CUSTOM_DATA_VERSION.DEBUG_05)
			{
				specular2 = 0f;
			}
			else
			{
				specular2 = reader.ReadSingle();
			}
			smooth2 = reader.ReadSingle();
			return true;
		}

		public void FromSexyData(HSColorSet colorSet1, HSColorSet colorSet2)
		{
			mainColor1 = colorSet1.rgbaDiffuse;
			specColor1 = colorSet1.rgbSpecular;
			specular1 = colorSet1.specularIntensity;
			smooth1 = colorSet1.specularSharpness;
			mainColor2 = colorSet2.rgbaDiffuse;
			specColor2 = colorSet2.rgbSpecular;
			specular2 = colorSet2.alpha;
			smooth2 = colorSet2.specularSharpness;
		}

		public override void SetToMaterial(Material material)
		{
			Debug.LogError("つかわないで");
			material.color = mainColor1;
			material.SetColor(CustomDataManager._SpecColor, specColor1);
			material.SetFloat(CustomDataManager._Metallic, specular1);
			material.SetFloat(CustomDataManager._Smoothness, smooth1);
			Color color = specColor2;
			color.a = smooth2;
			material.SetColor(CustomDataManager._Color_3, mainColor2);
			material.SetColor(CustomDataManager._SpecColor_3, color);
		}

		public void SetMaterialCustoms(MaterialCustoms custom)
		{
			custom.SetColor("MainColor", mainColor1);
			custom.SetColor("MainSpecColor", specColor1);
			custom.SetFloat("MainMetallic", specular1);
			custom.SetFloat("MainSmoothness", smooth1);
			custom.SetColor("SubColor", mainColor2);
			custom.SetColor("SubSpecColor", specColor2);
			custom.SetFloat("SubMetallic", specular2);
			custom.SetFloat("SubSmoothness", smooth2);
		}

		public void Copy(ColorParameter_PBR2 source)
		{
			mainColor1 = source.mainColor1;
			specColor1 = source.specColor1;
			specular1 = source.specular1;
			smooth1 = source.smooth1;
			mainColor2 = source.mainColor2;
			specColor2 = source.specColor2;
			specular2 = source.specular2;
			smooth2 = source.smooth2;
		}
	}
}
