using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchUI : MonoBehaviour
{
	[Serializable]
	private class SwitchEvent : UnityEvent<bool>
	{
	}

	[SerializeField]
	private Text title;

	[SerializeField]
	private Toggle on;

	[SerializeField]
	private Toggle off;

	[SerializeField]
	private bool value;

	[SerializeField]
	private Image noIntaractImage;

	[SerializeField]
	private SwitchEvent onChange;

	private bool invoke = true;

	private bool intaractable = true;

	public bool Value
	{
		get
		{
			return value;
		}
		set
		{
			SetValue(value);
		}
	}

	public bool Intaractable
	{
		get
		{
			return intaractable;
		}
		set
		{
			intaractable = value;
			on.interactable = value;
			off.interactable = value;
			if (noIntaractImage != null)
			{
				noIntaractImage.enabled = !value;
			}
		}
	}

	private void Start()
	{
		invoke = true;
		on.onValueChanged.AddListener(OnChange_ON);
		off.onValueChanged.AddListener(OnChange_OFF);
	}

	private void Update()
	{
		bool isOn = on.isOn;
		if (value != isOn)
		{
			value = isOn;
		}
	}

	public void SetValue(bool flag)
	{
		invoke = false;
		value = flag;
		if (flag)
		{
			on.isOn = true;
			off.isOn = false;
		}
		else
		{
			off.isOn = true;
			on.isOn = false;
		}
		invoke = true;
	}

	public void SetTitle(string str)
	{
		title.text = str;
	}

	public void Setup(string title, bool flag, UnityAction<bool> act)
	{
		this.title.text = title;
		SetValue(flag);
		onChange.AddListener(act);
	}

	private void OnChange_ON(bool flag)
	{
		if (flag && onChange != null)
		{
			onChange.Invoke(true);
		}
	}

	private void OnChange_OFF(bool flag)
	{
		if (flag && onChange != null)
		{
			onChange.Invoke(false);
		}
	}

	public EventTrigger GetEventTrigger()
	{
		EventTrigger component = GetComponent<EventTrigger>();
		if ((bool)component)
		{
			return component;
		}
		return base.gameObject.AddComponent<EventTrigger>();
	}
}
