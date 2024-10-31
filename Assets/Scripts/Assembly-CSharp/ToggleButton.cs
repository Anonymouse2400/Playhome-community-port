using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
	[Serializable]
	public class ToggleButtonEvent : UnityEvent<bool>
	{
	}

	[SerializeField]
	private Button button_on;

	[SerializeField]
	private Text text_on;

	[SerializeField]
	private Button button_off;

	[SerializeField]
	private Text text_off;

	public ToggleButtonEvent action;

	[SerializeField]
	private bool value;

	[SerializeField]
	private bool interactable = true;

	public bool Value
	{
		get
		{
			return value;
		}
		set
		{
			ChangeValue(value, true);
		}
	}

	public bool Interactable
	{
		get
		{
			return interactable;
		}
		set
		{
			ChangeInteractable(value);
		}
	}

	public void Setup(string textOFF, string textON, UnityAction<bool> action)
	{
		text_on.text = textON;
		text_off.text = textOFF;
		this.action.AddListener(action);
	}

	public void SetText(string textOFF, string textON)
	{
		text_on.text = textON;
		text_off.text = textOFF;
	}

	public void ActionAddListener(UnityAction<bool> action)
	{
		this.action.AddListener(action);
	}

	private void Awake()
	{
		SwitchButton();
		button_off.onClick.AddListener(delegate
		{
			OnClick(true);
		});
		button_on.onClick.AddListener(delegate
		{
			OnClick(false);
		});
		button_on.interactable = interactable;
		button_off.interactable = interactable;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ChangeValue(bool flag, bool invoke)
	{
		if (value != flag)
		{
			value = flag;
			SwitchButton();
			if (invoke && action != null)
			{
				action.Invoke(flag);
			}
		}
	}

	private void SwitchButton()
	{
		button_off.gameObject.SetActive(!value);
		button_on.gameObject.SetActive(value);
	}

	private void OnClick(bool flag)
	{
		value = flag;
		SwitchButton();
		if (action != null)
		{
			action.Invoke(flag);
		}
	}

	private void ChangeInteractable(bool flag)
	{
		interactable = flag;
		button_on.interactable = interactable;
		button_off.interactable = interactable;
	}

	public void SetColor(ColorBlock onColor, ColorBlock offColor)
	{
		button_on.colors = onColor;
		button_off.colors = offColor;
	}
}
