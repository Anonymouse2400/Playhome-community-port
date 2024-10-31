using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSync : MonoBehaviour
{
	private Camera camera;

	public Camera owner;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	private void OnPreRender()
	{
		if (!(owner == null))
		{
			camera.depthTextureMode = owner.depthTextureMode;
			camera.fieldOfView = owner.fieldOfView;
			camera.nearClipPlane = owner.nearClipPlane;
			camera.farClipPlane = owner.farClipPlane;
		}
	}
}
