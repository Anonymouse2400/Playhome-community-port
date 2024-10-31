using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	[Serializable]
	public class HSColorSet
	{
		public const int SaveVersion = 1;

		public HsvColor hsvDiffuse = new HsvColor(19f, 0.07f, 0.63f);

		public float alpha = 1f;

		public HsvColor hsvSpecular = new HsvColor(0f, 0f, 0.8f);

		public float specularIntensity = 0.1f;

		public float specularSharpness = 0.33f;

		public Color rgbaDiffuse
		{
			get
			{
				return HsvColor.ToRgba(hsvDiffuse, alpha);
			}
		}

		public Color rgbDiffuse
		{
			get
			{
				return HsvColor.ToRgb(hsvDiffuse);
			}
		}

		public Color rgbSpecular
		{
			get
			{
				return HsvColor.ToRgb(hsvSpecular);
			}
		}

		public static bool CheckSameColor(HSColorSet a, HSColorSet b, bool hsv, bool alpha, bool specular)
		{
			if (alpha && a.alpha != b.alpha)
			{
				return false;
			}
			if (hsv && (a.hsvDiffuse.H != b.hsvDiffuse.H || a.hsvDiffuse.S != b.hsvDiffuse.S || a.hsvDiffuse.V != b.hsvDiffuse.V))
			{
				return false;
			}
			if (specular)
			{
				if (a.specularIntensity != b.specularIntensity)
				{
					return false;
				}
				if (a.specularSharpness != b.specularSharpness)
				{
					return false;
				}
				if (a.hsvSpecular.H != b.hsvSpecular.H || a.hsvSpecular.S != b.hsvSpecular.S || a.hsvSpecular.V != b.hsvSpecular.V)
				{
					return false;
				}
			}
			return true;
		}

		public void SetDiffuseRGBA(Color rgba)
		{
			hsvDiffuse = HsvColor.FromRgb(rgba);
			alpha = rgba.a;
		}

		public void SetDiffuseRGB(Color rgb)
		{
			hsvDiffuse = HsvColor.FromRgb(rgb);
			alpha = 1f;
		}

		public void SetSpecularRGB(Color rgb)
		{
			hsvSpecular = HsvColor.FromRgb(rgb);
		}

		public void Copy(HSColorSet src)
		{
			hsvDiffuse = new HsvColor(src.hsvDiffuse.H, src.hsvDiffuse.S, src.hsvDiffuse.V);
			alpha = src.alpha;
			hsvSpecular = new HsvColor(src.hsvSpecular.H, src.hsvSpecular.S, src.hsvSpecular.V);
			specularIntensity = src.specularIntensity;
			specularSharpness = src.specularSharpness;
		}

		public void Load(BinaryReader reader, int version)
		{
			hsvDiffuse.H = (float)reader.ReadDouble();
			hsvDiffuse.S = (float)reader.ReadDouble();
			hsvDiffuse.V = (float)reader.ReadDouble();
			alpha = (float)reader.ReadDouble();
			hsvSpecular.H = (float)reader.ReadDouble();
			hsvSpecular.S = (float)reader.ReadDouble();
			hsvSpecular.V = (float)reader.ReadDouble();
			specularIntensity = (float)reader.ReadDouble();
			specularSharpness = (float)reader.ReadDouble();
		}
	}
}
