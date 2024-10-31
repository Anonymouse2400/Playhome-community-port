using System;
using UnityEngine;

[RequireComponent(typeof(ToggleButton))]
public class MoveableToggleButton : MonoBehaviour
{
	[SerializeField]
	private MoveableUI moveableUI;

	private ToggleButton toggle;

	private bool invoke = true;

	private void Awake()
	{
		toggle = GetComponent<ToggleButton>();
		moveableUI.onChangeState.AddListener(OnChangeState);
		toggle.ActionAddListener(OnToggleChange);
	}

	private void Update()
	{
	}

	private void OnToggleChange(bool flag)
	{
		if (invoke)
		{
			if (flag)
			{
				moveableUI.Open();
			}
			else
			{
				moveableUI.Close();
			}
		}
	}

	private void OnChangeState(MoveableUI.STATE state)
	{
		invoke = false;
		toggle.Value = moveableUI.State != MoveableUI.STATE.CLOSED;
		invoke = true;
	}
}
