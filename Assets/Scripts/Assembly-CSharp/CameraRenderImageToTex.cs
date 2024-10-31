using System;
using UnityEngine;

public class CameraRenderImageToTex : MonoBehaviour
{
	public RenderTexture Tex;

	public bool isCameraTarget;

	public Material material;

	public ScreenShotCamera ssCam;

	public KeyCode ssKey = KeyCode.F11;

	private void Awake()
	{
		CreateRenderTex();
	}

	private void LateUpdate()
	{
		if (ssCam != null && Input.GetKeyDown(ssKey))
		{
			ScreenShotCamera screenShotCamera = global::UnityEngine.Object.Instantiate(ssCam);
			screenShotCamera.Shot(Tex);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		CheckScreenSize();
		if (material != null)
		{
			if (!isCameraTarget)
			{
				Graphics.Blit(source, Tex, material);
			}
			Graphics.Blit(source, destination, material);
		}
		else
		{
			if (!isCameraTarget)
			{
				Graphics.Blit(source, Tex);
			}
			Graphics.Blit(source, destination);
		}
	}

	private void CheckScreenSize()
	{
		if (Tex.width != Screen.width || Tex.height != Screen.height)
		{
			CreateRenderTex();
		}
	}

	private void CreateRenderTex()
	{
		if (Tex != null)
		{
			Tex.Release();
            global::UnityEngine.Object.Destroy(Tex);
		}
		Tex = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
		Tex.filterMode = FilterMode.Point;
		Tex.wrapMode = TextureWrapMode.Clamp;
		if (isCameraTarget)
		{
			GetComponent<Camera>().targetTexture = Tex;
		}
	}

	private void Destroy()
	{
		if ((bool)Tex)
		{
			Tex.Release();
            global::UnityEngine.Object.Destroy(Tex);
			Tex = null;
		}
		if (isCameraTarget)
		{
			GetComponent<Camera>().targetTexture = null;
		}
	}

	public Texture2D GetTex2D()
	{
		RenderTexture.active = Tex;
		Texture2D texture2D = new Texture2D(Tex.width, Tex.height, TextureFormat.RGB24, false);
		texture2D.filterMode = FilterMode.Point;
		texture2D.wrapMode = TextureWrapMode.Clamp;
		texture2D.ReadPixels(new Rect(0f, 0f, Tex.width, Tex.height), 0, 0, false);
		texture2D.Apply();
		return texture2D;
	}
}
