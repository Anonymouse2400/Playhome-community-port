using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

public class ShortCut_Help : MonoBehaviour
{
	private enum STATE
	{
		NONE = 0,
		SHORTCUT = 1,
		HELP = 2
	}

	[SerializeField]
	private CanvasGroup shortCut;

	[SerializeField]
	private CanvasGroup help;

	[SerializeField]
	private float K = 10f;

	private STATE state;

	private float rate = 1f;

	private void Start()
	{
		if (shortCut != null)
		{
			shortCut.alpha = 0f;
		}
		if (help != null)
		{
			help.alpha = 0f;
		}
	}

	private void Update()
	{
		UpdateInput();
		float num = 0f;
		if (shortCut != null)
		{
			num = ((state != STATE.SHORTCUT) ? 0f : 1f);
			shortCut.alpha = Tween.Spring(shortCut.alpha, num, K, Time.deltaTime, 0.01f);
		}
		if (help != null)
		{
			num = ((state != STATE.HELP) ? 0f : 1f);
			help.alpha = Tween.Spring(help.alpha, num, K, Time.deltaTime, 0.01f);
		}
	}

	private void UpdateInput()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.F2) && shortCut != null)
		{
			if (state != STATE.SHORTCUT)
			{
				SystemSE.Play(SystemSE.SE.OPEN);
				state = STATE.SHORTCUT;
			}
			else
			{
				SystemSE.Play(SystemSE.SE.CLOSE);
				state = STATE.NONE;
			}
		}
		if (Input.GetKeyDown(KeyCode.F3) && help != null)
		{
			if (state != STATE.HELP)
			{
				SystemSE.Play(SystemSE.SE.OPEN);
				state = STATE.HELP;
			}
			else
			{
				SystemSE.Play(SystemSE.SE.CLOSE);
				state = STATE.NONE;
			}
		}
	}
}
