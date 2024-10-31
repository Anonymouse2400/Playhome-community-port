using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MoveableToggle : MonoBehaviour
{
	[SerializeField]
	private MoveableUI moveableUI;

	private Toggle toggle;

	private bool invoke = true;

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
		moveableUI.onChangeState.AddListener(OnChangeState);
		toggle.onValueChanged.AddListener(OnToggleChange);
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
		toggle.isOn = moveableUI.State != MoveableUI.STATE.CLOSED;
		invoke = true;
	}
}
