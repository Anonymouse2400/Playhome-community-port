using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class RendAndDepthTexture : MonoBehaviour
{
	private Camera camera;

	private RenderTexture colorTex;

	private RenderTexture depthTex;

	[SerializeField]
	private Material postMat;

	private bool createdTex;

	public RenderTexture ColorTex
	{
		get
		{
			return colorTex;
		}
	}

	public RenderTexture DepthTex
	{
		get
		{
			return depthTex;
		}
	}

	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			CreateTexture();
		}
	}

	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			ReleaseTexture();
		}
	}

	private void OnPreRender()
	{
		if (Application.isPlaying && (colorTex.width != Screen.width || colorTex.height != Screen.height))
		{
			RemakeTexture();
		}
	}

	private void OnPostRender()
	{
		if (Application.isPlaying)
		{
			Graphics.SetRenderTarget(null);
			if (postMat == null)
			{
				Graphics.Blit(colorTex, postMat);
			}
		}
	}

	private void CreateTexture()
	{
		camera = GetComponent<Camera>();
		camera.depthTextureMode = DepthTextureMode.Depth;
		RenderTextureFormat format = ((!camera.hdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
		if (colorTex == null)
		{
			colorTex = new RenderTexture(Screen.width, Screen.height, 0, format);
		}
		else
		{
			colorTex.Release();
			colorTex.width = Screen.width;
			colorTex.height = Screen.height;
			colorTex.depth = 0;
			colorTex.format = format;
			colorTex.Create();
		}
		colorTex.Create();
		if (depthTex == null)
		{
			depthTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
		}
		else
		{
			depthTex.Release();
			depthTex.width = Screen.width;
			depthTex.height = Screen.height;
			depthTex.depth = 24;
			depthTex.format = RenderTextureFormat.Depth;
			depthTex.Create();
		}
		depthTex.Create();
		camera.SetTargetBuffers(colorTex.colorBuffer, depthTex.depthBuffer);
		createdTex = true;
	}

	private void ReleaseTexture()
	{
		if (createdTex)
		{
			Graphics.SetRenderTarget(null);
			camera.targetTexture = null;
			camera.SetTargetBuffers(Graphics.activeColorBuffer, Graphics.activeDepthBuffer);
			if (colorTex != null)
			{
				colorTex.Release();
				colorTex = null;
			}
			if (depthTex != null)
			{
				depthTex.Release();
				depthTex = null;
			}
		}
	}

	private void RemakeTexture()
	{
		camera.targetTexture = null;
		colorTex.Release();
		colorTex.width = Screen.width;
		colorTex.height = Screen.height;
		colorTex.Create();
		depthTex.Release();
		depthTex.width = Screen.width;
		depthTex.height = Screen.height;
		depthTex.Create();
		camera.SetTargetBuffers(colorTex.colorBuffer, depthTex.depthBuffer);
	}
}
