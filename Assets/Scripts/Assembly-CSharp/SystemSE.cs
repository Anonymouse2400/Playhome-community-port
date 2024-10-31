using System;
using UnityEngine;

public static class SystemSE
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

	private static GameControl gameCtrl;

	public static GameControl GameCtrl
	{
		get
		{
			if (gameCtrl == null)
			{
				gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
			}
			return gameCtrl;
		}
	}

	private static AudioClip ChoiceClip(SE se)
	{
		if (GameCtrl == null || GameCtrl.audioCtrl == null)
		{
			return null;
		}
		AudioClip result = null;
		switch (se)
		{
		case SE.CHOICE:
			result = GameCtrl.audioCtrl.systemSE_choice;
			break;
		case SE.YES:
			result = GameCtrl.audioCtrl.systemSE_yes;
			break;
		case SE.NO:
			result = GameCtrl.audioCtrl.systemSE_no;
			break;
		case SE.OPEN:
			result = GameCtrl.audioCtrl.systemSE_open;
			break;
		case SE.CLOSE:
			result = GameCtrl.audioCtrl.systemSE_close;
			break;
		}
		return result;
	}

	public static void Play(SE se)
	{
		AudioClip clip = ChoiceClip(se);
		if (GameCtrl != null && GameCtrl.audioCtrl != null)
		{
			GameCtrl.audioCtrl.Play2DSE(clip);
		}
	}
}
