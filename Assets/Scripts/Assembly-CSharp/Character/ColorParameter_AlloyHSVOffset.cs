using System;
using System.IO;
using SEXY;
using UnityEngine;

namespace Character
{
	public class ColorParameter_AlloyHSVOffset : ColorParameter_Base
	{
		public float offset_h;

		public float offset_s = 1f;

		public float offset_v = 1f;

		public bool hasAlpha;

		public float alpha = 1f;

		public float metallic;

		public float smooth = 0.562f;

		public ColorParameter_AlloyHSVOffset()
		{
		}

		public ColorParameter_AlloyHSVOffset(bool hasAlpha, float alpha)
		{
			this.hasAlpha = hasAlpha;
			this.alpha = alpha;
		}

		public ColorParameter_AlloyHSVOffset(ColorParameter_AlloyHSVOffset copy)
		{
			offset_h = copy.offset_h;
			offset_s = copy.offset_s;
			offset_v = copy.offset_v;
			hasAlpha = copy.hasAlpha;
			alpha = copy.alpha;
			metallic = copy.metallic;
			smooth = copy.smooth;
		}

		public override COLOR_TYPE GetColorType()
		{
			return COLOR_TYPE.ALLOY_HSV;
		}

		public void Init(bool hasAlpha = false, float alpha = 1f)
		{
			offset_h = 0f;
			offset_s = 1f;
			offset_v = 1f;
			this.hasAlpha = hasAlpha;
			this.alpha = alpha;
			metallic = 0f;
			smooth = 0.562f;
		}

		public override void Save(BinaryWriter writer)
		{
			Save_ColorType(writer);
			writer.Write(offset_h);
			writer.Write(offset_s);
			writer.Write(offset_v);
			writer.Write(alpha);
			writer.Write(metallic);
			writer.Write(smooth);
		}

		public override bool Load(BinaryReader reader, CUSTOM_DATA_VERSION version)
		{
			Color color = Color.white;
			if (version < CUSTOM_DATA_VERSION.DEBUG_04)
			{
				ReadColor(reader, ref color);
				return true;
			}
			COLOR_TYPE cOLOR_TYPE = ColorParameter_Base.Load_ColorType(reader, version);
			if (cOLOR_TYPE == COLOR_TYPE.NONE)
			{
				return false;
			}
			if (cOLOR_TYPE != GetColorType())
			{
				bool flag = false;
				if (version < CUSTOM_DATA_VERSION.DEBUG_06 && cOLOR_TYPE == COLOR_TYPE.ALLOY)
				{
					flag = true;
				}
				else if (cOLOR_TYPE == COLOR_TYPE.ALLOY_HSV)
				{
					flag = true;
				}
				if (!flag)
				{
					Debug.LogError("色タイプが違う");
				}
			}
			if (version < CUSTOM_DATA_VERSION.DEBUG_06)
			{
				ReadColor(reader, ref color);
			}
			else
			{
				offset_h = reader.ReadSingle();
				offset_s = reader.ReadSingle();
				offset_v = reader.ReadSingle();
				if (version == CUSTOM_DATA_VERSION.DEBUG_07)
				{
					bool flag2 = reader.ReadBoolean();
					alpha = reader.ReadSingle();
				}
				else if (version >= CUSTOM_DATA_VERSION.TRIAL)
				{
					alpha = reader.ReadSingle();
				}
			}
			metallic = reader.ReadSingle();
			smooth = reader.ReadSingle();
			return true;
		}

		public void FromSexyData(HSColorSet colorSet)
		{
		}

		public override void SetToMaterial(Material material)
		{
			if (!((UnityEngine.Object)(object)material == null))
			{
				if (hasAlpha)
				{
					Color color = material.color;
					color.a = alpha;
					material.color = color;
				}
				material.SetFloat(CustomDataManager._Metal, metallic);
				material.SetFloat(CustomDataManager._Roughness, 1f - smooth);
			}
		}

		public void SetToMaterial(Material material, float addSmooth)
		{
			if (!((UnityEngine.Object)(object)material == null))
			{
				if (hasAlpha)
				{
					Color color = material.color;
					color.a = alpha;
					material.color = color;
				}
				float value = 1f - Mathf.Clamp01(smooth + addSmooth);
				material.SetFloat(CustomDataManager._Metal, metallic);
				material.SetFloat(CustomDataManager._Roughness, value);
			}
		}
	}
}
