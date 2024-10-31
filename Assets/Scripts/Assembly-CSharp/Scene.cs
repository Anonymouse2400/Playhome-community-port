using System;
using UnityEngine;
using Utility;

public class Scene : MonoBehaviour
{
	protected string sceneInMsg = string.Empty;

	protected GameControl gameCtrl;

	protected ScreenFade inFade;

	public GameControl GC
	{
		get
		{
			return gameCtrl;
		}
	}

	public bool InFading
	{
		get
		{
			return inFade != null;
		}
	}

	protected void InScene(bool fadeIn = true)
	{
		gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
		if (gameCtrl == null)
		{
			gameCtrl = ResourceUtility.CreateInstance<GameControl>("CommonPrefabs/GameControl");
		}
		gameCtrl.sceneCtrl.SetScene(this);
		sceneInMsg = GC.sceneCtrl.RequestChangeMessage();
		if (fadeIn)
		{
			FadeIn();
		}
	}

	public virtual void OutScene(string next)
	{
	}

	protected void FadeIn()
	{
		Color fadeColor = GC.sceneCtrl.FadeColor;
		float fadeTime = GC.sceneCtrl.FadeTime;
		inFade = ScreenFade.StartFade(ScreenFade.TYPE.IN, fadeColor, fadeTime);
	}
}
