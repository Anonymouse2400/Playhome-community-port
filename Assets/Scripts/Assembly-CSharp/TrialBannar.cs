using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class TrialBannar : MonoBehaviour
{
	private Canvas canvas;

	private GameControl gameCtrl;

	private bool _hide;

	private GameControl GameCtrl
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

	public bool Hide
	{
		get
		{
			return _hide;
		}
		set
		{
			_hide = value;
		}
	}

	private void Start()
	{
		canvas = GetComponent<Canvas>();
	}

	private void Update()
	{
		canvas.enabled = !GameCtrl.IsHideUI && !Hide;
	}
}
