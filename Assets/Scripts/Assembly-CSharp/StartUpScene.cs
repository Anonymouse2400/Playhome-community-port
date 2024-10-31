using System;

public class StartUpScene : Scene
{
	public string nextScene = "LogoScene";

	public string nextMessage = string.Empty;

	private void Start()
	{
		InScene();
	}

	private void Update()
	{
		if (!base.GC.sceneCtrl.IsFading)
		{
			base.GC.ChangeScene(nextScene, nextMessage, 0f);
		}
	}
}
