using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PresetListToggle : MonoBehaviour
{
	[SerializeField]
	private MoveableThumbnailSelectUI selectUI;

	private bool invoke = true;

	public Action onDisable;

	private Toggle _toggle;

	public Toggle toggle
	{
		get
		{
			if (_toggle == null)
			{
				_toggle = GetComponent<Toggle>();
			}
			return _toggle;
		}
	}

	public bool interactable
	{
		get
		{
			return toggle.interactable;
		}
		set
		{
			toggle.interactable = value;
		}
	}

	public void Setup(MoveableThumbnailSelectUI selectUI)
	{
		this.selectUI = selectUI;
	}

	public void ToggleSelectUI(bool flag)
	{
		if (invoke && selectUI != null)
		{
			if (flag)
			{
				selectUI.Open(_toggle);
			}
			else if (selectUI.openUI == _toggle)
			{
				selectUI.Close();
			}
		}
	}

	public void OnChangeState(MoveableUI.STATE state)
	{
		invoke = false;
		toggle.isOn = state != MoveableUI.STATE.CLOSED;
		invoke = true;
	}

	private void OnDisable()
	{
		if (selectUI != null && selectUI.openUI == _toggle)
		{
			selectUI.Close();
		}
		toggle.isOn = false;
	}

	private void Update()
	{
		if (toggle.isOn && selectUI.openUI != _toggle)
		{
			invoke = false;
			toggle.isOn = false;
			invoke = true;
		}
	}
}
