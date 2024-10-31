using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl
{
	private Scene nowScene;

	private string changeMsg = string.Empty;

	private ScreenFade fade;

	public Color FadeColor { get; private set; }

	public float FadeTime { get; private set; }

	public bool IsFading
	{
		get
		{
			return fade != null;
		}
	}

	public bool IsReserve { get; private set; }

	public string NowSceneName { get; private set; }

	public string PrevSceneName { get; private set; }

	public SceneControl(string loadedLevelName)
	{
		IsReserve = false;
		NowSceneName = loadedLevelName;
	}

	public void Change(string sceneName, string msg, Color color, float fadeTime, float fedeDeley = 0f, bool inSkip = false)
	{
		if (IsReserve)
		{
			Debug.LogWarning("シーン切替中" + sceneName + "への切り替え失敗");
			return;
		}
		changeMsg = msg;
		FadeTime = fadeTime;
		FadeColor = color;
		if (inSkip)
		{
			Change(sceneName);
			return;
		}
		IsReserve = true;
		fade = ScreenFade.StartFade(ScreenFade.TYPE.OUT, color, fadeTime, fedeDeley, delegate
		{
			Change(sceneName);
		}, false);
	}

	public void Change(string sceneName)
	{
		PrevSceneName = NowSceneName;
		NowSceneName = sceneName;
		IsReserve = false;
		if (nowScene != null)
		{
			nowScene.OutScene(sceneName);
		}
		SceneManager.LoadScene(sceneName);
	}

	public string RequestChangeMessage()
	{
		string result = changeMsg;
		changeMsg = string.Empty;
		return result;
	}

	public void SetScene(Scene newScene)
	{
		nowScene = newScene;
	}
}
