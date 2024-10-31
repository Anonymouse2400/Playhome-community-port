using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraSetRenderTexture : MonoBehaviour
{
	private RenderTexture rendTex;

	private Camera _cam;

	public RenderTexture RendTex
	{
		get
		{
			return rendTex;
		}
	}

	private Camera Cam
	{
		get
		{
			if (_cam == null)
			{
				_cam = GetComponent<Camera>();
			}
			return _cam;
		}
	}

	private void OnPreRender()
	{
		if (rendTex == null || rendTex.width != Screen.width || rendTex.height != Screen.height)
		{
			Cam.targetTexture = null;
            UnityEngine.Object.DestroyImmediate(rendTex);
			rendTex = new RenderTexture(Screen.width, Screen.height, 0);
			rendTex.hideFlags = HideFlags.HideAndDontSave;
		}
		Cam.targetTexture = rendTex;
	}

	private void OnDisable()
	{
		Cam.targetTexture = null;
        UnityEngine.Object.DestroyImmediate(rendTex);
	}
}
