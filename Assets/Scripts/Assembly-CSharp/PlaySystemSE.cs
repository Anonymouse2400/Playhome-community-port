using System;
using UnityEngine;

public class PlaySystemSE : MonoBehaviour
{
	public enum SE
	{
		CHOICE = 0,
		YES = 1,
		NO = 2,
		OPEN = 3,
		CLOSE = 4,
		NUM = 5
	}

	private GameControl gameCtrl;

	private AudioClip clip;

	[SerializeField]
	private SE type;

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
		switch (type)
		{
		case SE.CHOICE:
			clip = gameCtrl.audioCtrl.systemSE_choice;
			break;
		case SE.YES:
			clip = gameCtrl.audioCtrl.systemSE_yes;
			break;
		case SE.NO:
			clip = gameCtrl.audioCtrl.systemSE_no;
			break;
		case SE.OPEN:
			clip = gameCtrl.audioCtrl.systemSE_open;
			break;
		case SE.CLOSE:
			clip = gameCtrl.audioCtrl.systemSE_close;
			break;
		}
	}

	public void Play()
	{
		if (gameCtrl == null)
		{
			Setup();
		}
		gameCtrl.audioCtrl.Play2DSE(clip);
	}

	public void Play(bool flag)
	{
		if (flag)
		{
			if (gameCtrl == null)
			{
				Setup();
			}
			gameCtrl.audioCtrl.Play2DSE(clip);
		}
	}
}
