using System;
using UnityEngine;

public class ADVCameraClipboardCopy : MonoBehaviour
{
	public Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}
}
