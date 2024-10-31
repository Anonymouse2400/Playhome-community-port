using System;
using System.Collections;
using UnityEngine;

public class EndFrameToScreenShot : MonoBehaviour
{
	public ScreenShotCamera ssCam;

	public KeyCode ssKey = KeyCode.F11;

	private void LateUpdate()
	{
		if (Input.GetKeyDown(ssKey))
		{
			ScreenShotCamera ss = UnityEngine.Object.Instantiate(ssCam);
			StartCoroutine(ReadPixEndFrame(ss));
		}
	}

	private IEnumerator ReadPixEndFrame(ScreenShotCamera ss)
	{
		yield return new WaitForEndOfFrame();
		Texture2D tex2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		tex2D.filterMode = FilterMode.Point;
		tex2D.wrapMode = TextureWrapMode.Clamp;
		tex2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, false);
		tex2D.Apply();
		ss.Shot(tex2D);
	}
}
