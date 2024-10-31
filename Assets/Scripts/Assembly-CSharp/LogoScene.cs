using System;
using UnityEngine;
using Utility;

public class LogoScene : Scene
{
	private float timer;

	public SpriteRenderer circle;

	public float rollSpeed = 30f;

	public SpriteRenderer title;

	public float titleSpeed = 5f;

	public float titleStartY = -1f;

	public float titleGoalY;

	public float endTime = 3f;

	public string nextScene = "Title";

	public string nextMessage = string.Empty;

	public float fadeInTime = 1f;

	public float fadeOutTime = 1f;

	public string bgm = string.Empty;

	private float roll;

	private bool isFirstFrame = true;

	private void Start()
	{
		InScene(false);
		ScreenFade.StartFade(ScreenFade.TYPE.IN, base.GC.FadeColor, fadeInTime);
		timer = 0f;
		roll = 0f;
		Vector3 position = title.transform.position;
		position.y = titleStartY;
		title.transform.position = position;
		isFirstFrame = true;
		if (bgm.Length > 0)
		{
			base.GC.audioCtrl.BGM_LoadAndPlay(bgm);
		}
	}

	private void Update()
	{
		if (isFirstFrame)
		{
			isFirstFrame = false;
			return;
		}
		roll += rollSpeed * Time.deltaTime;
		circle.transform.localRotation = Quaternion.Euler(0f, 0f, roll);
		Color color = title.color;
		color.a = Tween.Spring(color.a, 1f, titleSpeed, Time.deltaTime);
		title.color = color;
		Vector3 localPosition = title.transform.localPosition;
		localPosition.y = Tween.Spring(localPosition.y, titleGoalY, titleSpeed, Time.deltaTime);
		title.transform.localPosition = localPosition;
		if (!base.GC.sceneCtrl.IsFading)
		{
			timer += Time.deltaTime;
			if (timer >= endTime)
			{
				base.GC.ChangeScene(nextScene, nextMessage, fadeOutTime);
			}
			else if (GetInput())
			{
				base.GC.ChangeScene(nextScene, nextMessage, base.GC.FadeTime);
			}
		}
	}

	private bool GetInput()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape))
		{
			return true;
		}
		return false;
	}
}
