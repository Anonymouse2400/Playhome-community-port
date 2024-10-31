using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleGroupEvent : MonoBehaviour
{
	[Serializable]
	public class ToggleSelectEvent : UnityEvent<int>
	{
	}

	public ToggleSelectEvent selEvent;

	[SerializeField]
	private Toggle[] toggles;

	private bool isChange;

	private void Start()
	{
		for (int i = 0; i < toggles.Length; i++)
		{
			toggles[i].onValueChanged.AddListener(OnChange);
		}
	}

	private void Update()
	{
		if (!isChange)
		{
			return;
		}
		int arg = -1;
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].isOn)
			{
				arg = i;
				break;
			}
		}
		isChange = false;
		selEvent.Invoke(arg);
	}

	public void OnChange(bool flag)
	{
		isChange = true;
	}
}
