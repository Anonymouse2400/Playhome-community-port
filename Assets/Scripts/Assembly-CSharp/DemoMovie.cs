using System;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

public class DemoMovie : Scene
{
	[SerializeField]
	private MediaPlayer player;

	private void Start()
	{
		InScene();
		gameCtrl.audioCtrl.BGM_Stop();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
		{
			Exit();
		}
	}

	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
		case MediaPlayerEvent.EventType.ReadyToPlay:
			player.Control.SetVolume(ConfigData.volume_master);
			mp.Play();
			break;
		case MediaPlayerEvent.EventType.FinishedPlaying:
			Exit();
			break;
		}
	}

	private void Exit()
	{
		base.GC.ChangeScene("TitleScene", string.Empty, 1f);
	}
}
