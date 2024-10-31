using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Mucus")]
	public class MucusImageEffect : PostEffectsBase
	{
		public Shader downShader;

		private Material downMaterial;

		private Vector2[] downOffsets = new Vector2[16];

		[Range(1f, 16f)]
		public int downDivider = 2;

		public Shader gaussShader;

		private Material gaussMaterial;

		private float gauss8Angle;

		private float gauss8Dispersion = 100f;

		[Range(0f, 2f)]
		public float gauss8SampleLen = 1f;

		public Shader toInfoShader;

		private Material toInfoMaterial;

		public float toInfoNormalScale = 1f;

		public float toInfoPow = 1f;

		public float toInfoMul = 1f;

		public Shader finalShader;

		private Material finalMaterial;

		public CameraSetRenderTexture CameraSetRendTex;

		public float finalBumpRate = 1f;

		public float finalDistortionRate = 1f;

		public Color finalMuddyMaxColor = Color.white;

		public Color finalMuddyMinColor = Color.white;

		public float finalMuddyRate = 1f;

		public float finalAlphaPow = 1f;

		public Cubemap finalEnv;

		public Color finalEnvColor = Color.white;

		[Range(0f, 1f)]
		public float finalEnvRate = 0.2f;

		private Camera camera;

		private void OnEnable()
		{
			Camera component = GetComponent<Camera>();
		}

		public override bool CheckResources()
		{
			CheckSupport(false);
			SetupMaterial(downShader, ref downMaterial);
			SetupMaterial(gaussShader, ref gaussMaterial);
			SetupMaterial(toInfoShader, ref toInfoMaterial);
			SetupMaterial(finalShader, ref finalMaterial);
			if (!isSupported)
			{
				ReportAutoDisable();
			}
			return isSupported;
		}

		private void SetupMaterial(Shader shader, ref Material material)
		{
			material = CheckShaderAndCreateMaterial(shader, material);
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!CheckResources() || CameraSetRendTex == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (camera == null)
			{
				camera = GetComponent<Camera>();
			}
			int width = source.width / downDivider;
			int height = source.height / downDivider;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, source.format);
			RenderTexture temporary3 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			GetSampleOffsets_DownScale4x4(source.width, source.height, ref downOffsets);
			for (int i = 0; i < 16; i++)
			{
				downMaterial.SetVector("_Offsets" + i, downOffsets[i]);
			}
			Graphics.Blit(source, temporary, downMaterial);
			Gaussian8(temporary, temporary2, gauss8Angle, gauss8SampleLen, gauss8Dispersion);
			Gaussian8(temporary2, temporary, gauss8Angle + 90f, gauss8SampleLen, gauss8Dispersion);
			Gaussian8(temporary, temporary2, gauss8Angle, gauss8SampleLen, gauss8Dispersion);
			Gaussian8(temporary2, temporary, gauss8Angle + 90f, gauss8SampleLen, gauss8Dispersion);
			toInfoMaterial.SetFloat("_PixelX", Screen.width);
			toInfoMaterial.SetFloat("_PixelY", Screen.height);
			toInfoMaterial.SetFloat("_NormalScale", toInfoNormalScale);
			toInfoMaterial.SetFloat("_Pow", toInfoPow);
			toInfoMaterial.SetFloat("_Mul", toInfoMul);
			Graphics.Blit(temporary, temporary3, toInfoMaterial);
			finalMaterial.SetTexture("_MetaTex", CameraSetRendTex.RendTex);
			finalMaterial.SetFloat("_BumpRate", finalBumpRate);
			finalMaterial.SetFloat("_DistortionRate", finalDistortionRate);
			finalMaterial.SetFloat("_MuddyRate", finalMuddyRate);
			finalMaterial.SetFloat("_AlphaPow", finalAlphaPow);
			finalMaterial.SetFloat("_EnvRate", finalEnvRate);
			finalMaterial.SetColor("_MuddyMaxColor", finalMuddyMaxColor);
			finalMaterial.SetColor("_MuddyMinColor", finalMuddyMinColor);
			finalMaterial.SetTexture("_Env", finalEnv);
			finalMaterial.SetColor("_EnvColor", finalEnvColor);
			Quaternion rotation = base.transform.rotation;
			float num = camera.fieldOfView * 0.5f;
			float num2 = camera.fieldOfView * 0.5f * camera.aspect;
			Vector3 vector = rotation * (Quaternion.Euler(0f, 0f - num2, 0f) * Vector3.forward);
			Vector3 vector2 = rotation * (Quaternion.Euler(0f, num2, 0f) * Vector3.forward);
			Vector3 vector3 = rotation * (Quaternion.Euler(num, 0f, 0f) * Vector3.forward);
			Vector3 vector4 = rotation * (Quaternion.Euler(0f - num, 0f, 0f) * Vector3.forward);
			finalMaterial.SetVector("_ViewVecL", vector);
			finalMaterial.SetVector("_ViewVecR", vector2);
			finalMaterial.SetVector("_ViewVecU", vector3);
			finalMaterial.SetVector("_ViewVecD", vector4);
			Graphics.Blit(temporary3, destination, finalMaterial);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
		}

		private void GetSampleOffsets_DownScale4x4(int width, int height, ref Vector2[] offsets)
		{
			float num = 1f / (float)width;
			float num2 = 1f / (float)height;
			int num3 = 0;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					offsets[num3].x = ((float)j - 1.5f) * num;
					offsets[num3].y = ((float)i - 1.5f) * num2;
					num3++;
				}
			}
		}

		private void Gaussian8(Texture source, RenderTexture dest, float angle, float sampleLen, float dispersion)
		{
			float[] weight = new float[8];
			CalcWeight_Gauss8(ref weight, dispersion);
			Vector4[] array = new Vector4[8];
			float num = Mathf.Sin(angle * ((float)Math.PI / 180f));
			float num2 = Mathf.Cos(angle * ((float)Math.PI / 180f));
			Vector2 vector = default(Vector2);
			vector.x = num / (float)source.width * sampleLen;
			vector.y = num2 / (float)source.height * sampleLen;
			for (int i = 0; i < 8; i++)
			{
				array[i] = vector * i;
			}
			for (int j = 0; j < 8; j++)
			{
				gaussMaterial.SetVectorArray("_Offsets", array);
				gaussMaterial.SetFloatArray("_Weights", weight);
			}
			Graphics.Blit(source, dest, gaussMaterial, 1);
		}

		private void CalcWeight_Gauss8(ref float[] weight, float dispersion)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				float num2 = 1f + 2f * (float)i;
				float num3 = Mathf.Exp(-0.5f * (num2 * num2) / dispersion);
				num += 2f * num3;
				weight[i] = num3;
			}
			for (int i = 0; i < 8; i++)
			{
				weight[i] /= num;
			}
		}
	}
}
