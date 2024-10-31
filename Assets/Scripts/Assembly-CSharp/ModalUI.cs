using System;
using UnityEngine;

public abstract class ModalUI : MonoBehaviour
{
	protected GameControl GameCtrl;

	protected virtual void Awake()
	{
		GameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
	}

	protected void End()
	{
		GameCtrl.OnModelClose();
        UnityEngine.Object.Destroy(base.gameObject);
	}
}
