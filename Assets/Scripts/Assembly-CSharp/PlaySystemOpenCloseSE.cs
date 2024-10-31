using System;
using UnityEngine;

public class PlaySystemOpenCloseSE : MonoBehaviour
{
	private GameControl gameCtrl;

	private void Start()
	{
		if (gameCtrl == null)
		{
			Setup();
		}
	}

	private void Setup()
	{
		gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
	}

	public void Play(bool isOpen)
	{
		AudioClip clip = ((!isOpen) ? gameCtrl.audioCtrl.systemSE_close : gameCtrl.audioCtrl.systemSE_open);
		gameCtrl.audioCtrl.Play2DSE(clip);
	}
}
