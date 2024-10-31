using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropDownUI : MonoBehaviour
{
	[Serializable]
	private class OnChangeEvent : UnityEvent<int>
	{
	}

	[SerializeField]
	private Text title;

	public Dropdown dropdown;

	private bool invoke = true;

	[SerializeField]
	private OnChangeEvent act;

	public int Value
	{
		get
		{
			return dropdown.value;
		}
		set
		{
			SetValue(value);
		}
	}

	private void Awake()
	{
		dropdown.onValueChanged.AddListener(OnDropdownValueChange);
		EventTrigger eventTrigger = GetEventTrigger();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener(delegate
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
		});
		eventTrigger.triggers.Add(entry);
	}

	public void SetValue(int val)
	{
		if (invoke)
		{
			invoke = false;
			dropdown.value = val;
			invoke = true;
		}
	}

	public void SetTitle(string str)
	{
		title.text = str;
	}

	public void SetList(List<Dropdown.OptionData> options)
	{
		dropdown.options = options;
	}

	public void AddOnValueChange(UnityAction<int> onValueChange)
	{
		act.AddListener(onValueChange);
	}

	private void OnDropdownValueChange(int no)
	{
		if (invoke)
		{
			if (act != null)
			{
				act.Invoke(no);
			}
			SystemSE.Play(SystemSE.SE.CHOICE);
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
