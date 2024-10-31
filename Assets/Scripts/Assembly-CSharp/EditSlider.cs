using System;
using UnityEngine;
using UnityEngine.UI;

public class EditSlider : MonoBehaviour
{
	public Text titleUI;

	public Slider sliderUI;

	public InputField inputUI;

	private float defVal = 0.5f;

	private Action<int, float> action;

	private int id;

	private bool echo;

	private bool invoke = true;

	public void Setup(string title, int id, float min, float max, float def, Action<int, float> action)
	{
		titleUI.text = title;
		this.id = id;
		sliderUI.minValue = min;
		sliderUI.maxValue = max;
		defVal = def;
		this.action = action;
		echo = true;
		sliderUI.value = def;
		inputUI.text = def.ToString("000");
		echo = false;
	}

	public void SetValue(float val, bool invoke)
	{
		this.invoke = invoke;
		ChangeVal(val);
		this.invoke = true;
	}

	public void ChangeText(string str)
	{
		float num = 0f;
		try
		{
			num = float.Parse(str);
		}
		catch
		{
			num = 0f;
		}
		ChangeVal(num);
	}

	public void Default()
	{
		ChangeVal(defVal);
	}

	public void ChangeVal(float val)
	{
		if (!echo)
		{
			echo = true;
			sliderUI.value = val;
			inputUI.text = val.ToString("000");
			if (action != null && invoke)
			{
				action(id, val);
			}
			echo = false;
		}
	}
}
