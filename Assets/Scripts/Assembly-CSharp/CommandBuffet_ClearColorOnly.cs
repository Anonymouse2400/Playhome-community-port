using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CommandBuffet_ClearColorOnly : MonoBehaviour
{
	public Color clearColor;

	public CameraEvent camEvent = CameraEvent.BeforeGBuffer;

	private Camera cam;

	private CommandBuffer buf;

	private void Cleanup()
	{
		if (cam != null && buf != null)
		{
			cam.RemoveCommandBuffer(camEvent, buf);
			buf.Release();
			buf = null;
		}
	}

	public void OnEnable()
	{
		Cleanup();
	}

	public void OnDisable()
	{
		Cleanup();
	}

	public void OnPreRender()
	{
		if (!base.gameObject.activeInHierarchy || !base.enabled)
		{
			Cleanup();
		}
		else if (buf == null)
		{
			if (cam == null)
			{
				cam = GetComponent<Camera>();
			}
			buf = new CommandBuffer();
			buf.name = "clear color only";
			buf.ClearRenderTarget(false, true, clearColor);
			cam.AddCommandBuffer(camEvent, buf);
		}
	}
}
