using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class ScreenShotCamera : MonoBehaviour
{
	[SerializeField]
	private string directoryPath = "UserData/Cap";

	private RenderTexture rendTex;

	private int deadStep = 1;

	private Camera camera;

	public RawImage rawImage;

	private AudioSource audio;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		audio = GetComponent<AudioSource>();
	}

	public void Shot(Texture tex)
	{
		CreateRenderTex();
		rawImage.texture = tex;
		deadStep = 1;
		Volume();
		audio.Play();
	}

	private void LateUpdate()
	{
		Volume();
		if (deadStep == 0)
		{
			RenderTexture.active = rendTex;
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture2D.filterMode = FilterMode.Point;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);
			texture2D.Apply();
			byte[] buffer = texture2D.EncodeToPNG();
			string text = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + DateTime.Now.Millisecond.ToString("000");
			string path = directoryPath + "/" + text + ".png";
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			FileStream fileStream = new FileStream(path, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(buffer);
			fileStream.Close();
			camera.targetTexture = null;
		}
		if (deadStep <= 0 && AudioEnd())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		deadStep--;
	}

	private void Destroy()
	{
		if ((bool)rendTex)
		{
			camera.targetTexture = null;
			rendTex.Release();
			UnityEngine.Object.Destroy(rendTex);
			rendTex = null;
		}
	}

	private void CreateRenderTex()
	{
		if (rendTex != null)
		{
			camera.targetTexture = null;
			rendTex.Release();
			UnityEngine.Object.Destroy(rendTex);
		}
		rendTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
		rendTex.filterMode = FilterMode.Point;
		camera.orthographicSize = Screen.height / 2;
		camera.targetTexture = rendTex;
	}

	private void Volume()
	{
		if ((UnityEngine.Object)(object)audio != null)
		{
			audio.volume = ConfigData.VolumeSoundEffect();
		}
	}

	private bool AudioEnd()
	{
		if ((UnityEngine.Object)(object)audio == null)
		{
			return true;
		}
		return audio.clip.loadState == AudioDataLoadState.Loaded && !audio.isPlaying;
	}
}
