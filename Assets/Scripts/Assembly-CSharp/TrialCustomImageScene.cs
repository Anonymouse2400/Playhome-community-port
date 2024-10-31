using System;
using UnityEngine;

public class TrialCustomImageScene : Scene
{
	private string nextMsg;

	private void Start()
	{
		InScene();
		nextMsg = sceneInMsg;
	}

	private void Update()
	{
		if (!gameCtrl.sceneCtrl.IsFading && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0)))
		{
			Next();
		}
	}

	public void Next()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		base.GC.ChangeScene("EditScene", nextMsg, 1f);
	}
}
