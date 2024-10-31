using System;
using UnityEngine;

[ExecuteInEditMode]
public class RenderTextureSetter : MonoBehaviour
{
	private RenderTexture rendTex;

	private Camera camera;

	private void OnEnable()
	{
		camera = GetComponent<Camera>();
		rendTex = new RenderTexture(Screen.width, Screen.height, 0);
		camera.targetTexture = rendTex;
	}

	private void OnDisable()
	{
		if (rendTex != null)
		{
			rendTex.Release();
			camera.targetTexture = null;
		}
	}

	public void LateUpdate()
	{
		if (rendTex.width != Screen.width || rendTex.height != Screen.height)
		{
			rendTex.Release();
			RenderTextureFormat format = ((!camera.hdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			rendTex = new RenderTexture(Screen.width, Screen.height, 0, format);
			camera.targetTexture = rendTex;
		}
	}

	public void RendTex(Material mat, string depthNormalTexName)
	{
		mat.SetTexture(depthNormalTexName, rendTex);
	}
}
