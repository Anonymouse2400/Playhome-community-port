using System;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RendCapCamera : MonoBehaviour
{
	private Camera capCamera;

	[SerializeField]
	private int superSize = 2;

	[SerializeField]
	private int saveWidth = 252;

	[SerializeField]
	private int saveHeight = 252;

	private void Awake()
	{
		capCamera = GetComponent<Camera>();
	}

	private void Start()
	{
		capCamera.enabled = false;
	}

	private void OnDestroy()
	{
	}

	public Texture2D Capture()
	{
		capCamera.enabled = true;
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
		capCamera.enabled = false;
		return texture2D;
	}

	public void Save(string file)
	{
		SavePNG(file, Capture());
	}

	public void Save()
	{
		DateTime now = DateTime.Now;
		string file = now.Year + "年" + now.Month + "月" + now.Day + "日" + now.Hour + "時" + now.Minute + "分" + now.Second + "秒" + now.Millisecond.ToString("0000") + ".png";
		SavePNG(file, Capture());
	}

	private void SavePNG(string file, Texture2D tex)
	{
		byte[] buffer = tex.EncodeToPNG();
		using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				binaryWriter.Write(buffer);
				binaryWriter.Close();
			}
			fileStream.Close();
		}
	}
}
