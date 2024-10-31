using System;
using UnityEngine;

public class CardCapture : MonoBehaviour
{
	[SerializeField]
	private Camera capCamera;

	[SerializeField]
	private int superSize = 2;

	[SerializeField]
	private int saveWidth = 252;

	[SerializeField]
	private int saveHeight = 352;

	private void OnDestroy()
	{
		if (!(capCamera == null))
		{
			RenderTexture targetTexture = capCamera.targetTexture;
			capCamera.targetTexture = null;
            UnityEngine.Object.Destroy(targetTexture);
		}
	}

	public Texture2D Capture()
	{
		int num = saveWidth * superSize;
		int num2 = saveHeight * superSize;
		RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 24);
		capCamera.targetTexture = temporary;
		capCamera.Render();
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
		texture2D.Apply();
		TextureScale.Bilinear(texture2D, saveWidth, saveHeight);
		capCamera.targetTexture = null;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}
}
