using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ToggleSwitchText : MonoBehaviour
{
	[SerializeField]
	private Toggle toggle;

	private Text text;

	[SerializeField]
	private string offText;

	[SerializeField]
	private string onText;

	private void Awake()
	{
		text = GetComponent<Text>();
		SetText();
		toggle.onValueChanged.AddListener(OnToggleChange);
	}

	private void SetText()
	{
		if (toggle != null)
		{
			text.text = ((!toggle.isOn) ? offText : onText);
		}
	}

	private void OnToggleChange(bool flag)
	{
		SetText();
	}
}
