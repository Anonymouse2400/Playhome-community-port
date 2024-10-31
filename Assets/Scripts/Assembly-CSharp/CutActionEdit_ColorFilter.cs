using System;
using UnityEngine;
using UnityEngine.UI;

public class CutActionEdit_ColorFilter : CutActionEdit
{
	private CutAct_ColorFilter colorFilter;

	[SerializeField]
	private Image colorImage;

	[SerializeField]
	private Slider colorR;

	[SerializeField]
	private Slider colorG;

	[SerializeField]
	private Slider colorB;

	[SerializeField]
	private Slider colorA;

	[SerializeField]
	private InputField durationField;

	public override void Setup(CutAction act)
	{
		colorFilter = act as CutAct_ColorFilter;
		durationField.text = colorFilter.duration.ToString("000.00");
		colorR.value = colorFilter.color.r;
		colorG.value = colorFilter.color.g;
		colorB.value = colorFilter.color.b;
		colorA.value = colorFilter.color.a;
	}

	private void Update()
	{
		if (colorFilter != null)
		{
			colorImage.color = colorFilter.color;
		}
	}

	public void SetSliderR(float r)
	{
		colorFilter.color.r = r;
	}

	public void SetSliderG(float g)
	{
		colorFilter.color.g = g;
	}

	public void SetSliderB(float b)
	{
		colorFilter.color.b = b;
	}

	public void SetSliderA(float a)
	{
		colorFilter.color.a = a;
	}

	public void SetDuration(string str)
	{
		try
		{
			float duration = float.Parse(str);
			colorFilter.duration = duration;
		}
		catch
		{
		}
	}
}
