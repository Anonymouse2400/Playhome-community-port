using System;
using UnityEngine;
using UnityEngine.Events;

public class RadioButtonGroup : MonoBehaviour
{
	[Serializable]
	public class RadioButtonGroupEvent : UnityEvent<int>
	{
	}

	[SerializeField]
	private ToggleButton[] toggleButtons;

	[SerializeField]
	private bool allowOFF = true;

	public RadioButtonGroupEvent action = new RadioButtonGroupEvent();

	private bool change;

	private int onNo = -1;

	private bool allowOffCheck = true;

	public ToggleButton[] ToggleButtons
	{
		get
		{
			return toggleButtons;
		}
	}

	public bool AllowOFF
	{
		get
		{
			return allowOFF;
		}
		set
		{
			allowOFF = value;
		}
	}

	public int Value
	{
		get
		{
			return onNo;
		}
		set
		{
			Change(value);
		}
	}

	private void Awake()
	{
		SetupToggleButtons();
	}

	private void Update()
	{
		if (change)
		{
			action.Invoke(onNo);
			change = false;
		}
	}

	private void OnToggleButton(int no, bool flag)
	{
		if (change)
		{
			return;
		}
		if (flag)
		{
			if (onNo != no)
			{
				onNo = no;
				change = true;
			}
			ChangeToggleButtonsOff(no);
		}
		else if (!allowOFF)
		{
			if (allowOffCheck)
			{
				toggleButtons[no].ChangeValue(true, false);
			}
		}
		else
		{
			change = true;
			onNo = -1;
		}
	}

	private void ChangeToggleButtonsOff(int on)
	{
		allowOffCheck = false;
		for (int i = 0; i < toggleButtons.Length; i++)
		{
			if (i != on)
			{
				toggleButtons[i].ChangeValue(false, true);
			}
		}
		allowOffCheck = true;
	}

	private void ChangeToggleButtonsValue(int on, bool invoke)
	{
		allowOffCheck = false;
		for (int i = 0; i < toggleButtons.Length; i++)
		{
			toggleButtons[i].ChangeValue(i == on, invoke);
		}
		allowOffCheck = true;
	}

	public void Change(int no)
	{
		onNo = no;
		ChangeToggleButtonsValue(no, true);
	}

	public void Change(int no, bool invoke)
	{
		onNo = no;
		ChangeToggleButtonsValue(no, invoke);
	}

	private void SetupToggleButtons()
	{
		for (int i = 0; i < toggleButtons.Length; i++)
		{
			int no = i;
			toggleButtons[i].action.AddListener(delegate(bool val)
			{
				OnToggleButton(no, val);
			});
			if (toggleButtons[i].Value)
			{
				onNo = i;
			}
		}
	}

	public void SetToggleButtons(ToggleButton[] sets)
	{
		if (toggleButtons != null)
		{
			for (int i = 0; i < toggleButtons.Length; i++)
			{
				int no = i;
				toggleButtons[i].action.RemoveListener(delegate(bool val)
				{
					OnToggleButton(no, val);
				});
			}
		}
		toggleButtons = new ToggleButton[sets.Length];
		for (int j = 0; j < sets.Length; j++)
		{
			toggleButtons[j] = sets[j];
		}
		SetupToggleButtons();
	}
}
