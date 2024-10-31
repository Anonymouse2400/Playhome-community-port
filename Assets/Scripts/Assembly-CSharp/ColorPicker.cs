using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
	public enum SLIDER_TYPE
	{
		HSV = 0,
		RGB = 1
	}

	public enum BAR_TYPE
	{
		H = 0,
		S = 1,
		V = 2,
		R = 3,
		G = 4,
		B = 5
	}

	[SerializeField]
	private InputSliderUI[] sliders = new InputSliderUI[4];

	[SerializeField]
	private Image[] sliderBGs = new Image[4];

	[SerializeField]
	private Toggle[] barTypeToggles = new Toggle[6];

	[SerializeField]
	private Toggle[] sliderTypeToggles = new Toggle[2];

	[SerializeField]
	private Slider bar;

	[SerializeField]
	private Image barBG;

	[SerializeField]
	private Image chartBG;

	[SerializeField]
	private Image point;

	[SerializeField]
	private SquarePickUI squarePick;

	[SerializeField]
	private Material matSlider_H;

	[SerializeField]
	private Material matSlider_S;

	[SerializeField]
	private Material matSlider_V;

	[SerializeField]
	private Material matSlider_R;

	[SerializeField]
	private Material matSlider_G;

	[SerializeField]
	private Material matSlider_B;

	[SerializeField]
	private Material matBar_H;

	[SerializeField]
	private Material matBar_S;

	[SerializeField]
	private Material matBar_V;

	[SerializeField]
	private Material matBar_R;

	[SerializeField]
	private Material matBar_G;

	[SerializeField]
	private Material matBar_B;

	[SerializeField]
	private Material matChart_H;

	[SerializeField]
	private Material matChart_S;

	[SerializeField]
	private Material matChart_V;

	[SerializeField]
	private Material matChart_R;

	[SerializeField]
	private Material matChart_G;

	[SerializeField]
	private Material matChart_B;

	private Color editColor;

	private float color_hue;

	private float color_saturation;

	private float color_value;

	private bool hasAlpha;

	private BAR_TYPE barType;

	private SLIDER_TYPE sliderType;

	private static readonly string[] SliderNames_HSV = new string[4] { "色相", "彩度", "明度", "透明" };

	private static readonly string[] SliderNames_RGB = new string[4] { "赤色", "緑色", "青色", "透明" };

	private static readonly float[] SliderMax_HSV = new float[4] { 359f, 255f, 255f, 255f };

	private static readonly float[] SliderMax_RGB = new float[4] { 255f, 255f, 255f, 255f };

	private int ShaderProperty_Red;

	private int ShaderProperty_Green;

	private int ShaderProperty_Blue;

	private int ShaderProperty_Hue;

	private int ShaderProperty_Saturation;

	private int ShaderProperty_Value;

	private bool invoke = true;

	private Action<Color> onChange;

	public Color EditColor
	{
		get
		{
			return editColor;
		}
		set
		{
			SetColor(value);
		}
	}

	public bool HasAlpha
	{
		get
		{
			return hasAlpha;
		}
		set
		{
			SetHasAlpha(value);
		}
	}

	private void Awake()
	{
		ShaderProperty_Red = Shader.PropertyToID("_R");
		ShaderProperty_Green = Shader.PropertyToID("_G");
		ShaderProperty_Blue = Shader.PropertyToID("_B");
		ShaderProperty_Hue = Shader.PropertyToID("_H");
		ShaderProperty_Saturation = Shader.PropertyToID("_S");
		ShaderProperty_Value = Shader.PropertyToID("_V");
		sliderTypeToggles[0].onValueChanged.AddListener(Toggle_HSV);
		sliderTypeToggles[1].onValueChanged.AddListener(Toggle_RGB);
		barTypeToggles[0].onValueChanged.AddListener(Toggle_BarH);
		barTypeToggles[1].onValueChanged.AddListener(Toggle_BarS);
		barTypeToggles[2].onValueChanged.AddListener(Toggle_BarV);
		barTypeToggles[3].onValueChanged.AddListener(Toggle_BarR);
		barTypeToggles[4].onValueChanged.AddListener(Toggle_BarG);
		barTypeToggles[5].onValueChanged.AddListener(Toggle_BarB);
		sliders[0].AddOnChangeAction(ChangeSlider_0);
		sliders[1].AddOnChangeAction(ChangeSlider_1);
		sliders[2].AddOnChangeAction(ChangeSlider_2);
		sliders[3].AddOnChangeAction(ChangeSlider_3);
		bar.onValueChanged.AddListener(ChangeBar);
		squarePick.SetOnChangeAction(ChangeChart);
		ChangeColorRGB(editColor);
		ChangeBarType(BAR_TYPE.H);
		ChangeSliderType(SLIDER_TYPE.HSV);
	}

	private void Update()
	{
		Update_Materials();
	}

	private void Update_Materials()
	{
		if (barType == BAR_TYPE.H)
		{
			barBG.material.SetFloat(ShaderProperty_Saturation, 1f);
			barBG.material.SetFloat(ShaderProperty_Value, 1f);
			chartBG.material.SetFloat(ShaderProperty_Hue, color_hue);
			chartBG.GraphicUpdateComplete();
		}
		else if (barType == BAR_TYPE.S)
		{
			barBG.material.SetFloat(ShaderProperty_Hue, color_hue);
			barBG.material.SetFloat(ShaderProperty_Value, 1f);
			chartBG.material.SetFloat(ShaderProperty_Saturation, color_saturation);
		}
		else if (barType == BAR_TYPE.V)
		{
			barBG.material.SetFloat(ShaderProperty_Hue, color_hue);
			barBG.material.SetFloat(ShaderProperty_Saturation, 1f);
			chartBG.material.SetFloat(ShaderProperty_Value, color_value);
		}
		else if (barType == BAR_TYPE.R)
		{
			barBG.material.SetFloat(ShaderProperty_Green, editColor.g);
			barBG.material.SetFloat(ShaderProperty_Blue, editColor.b);
			chartBG.material.SetFloat(ShaderProperty_Red, editColor.r);
		}
		else if (barType == BAR_TYPE.G)
		{
			barBG.material.SetFloat(ShaderProperty_Red, editColor.r);
			barBG.material.SetFloat(ShaderProperty_Blue, editColor.b);
			chartBG.material.SetFloat(ShaderProperty_Green, editColor.g);
		}
		else if (barType == BAR_TYPE.B)
		{
			barBG.material.SetFloat(ShaderProperty_Red, editColor.r);
			barBG.material.SetFloat(ShaderProperty_Green, editColor.g);
			chartBG.material.SetFloat(ShaderProperty_Blue, editColor.b);
		}
		if (sliderType == SLIDER_TYPE.HSV)
		{
			sliderBGs[0].material.SetFloat(ShaderProperty_Saturation, 1f);
			sliderBGs[0].material.SetFloat(ShaderProperty_Value, 1f);
			sliderBGs[1].material.SetFloat(ShaderProperty_Hue, color_hue);
			sliderBGs[1].material.SetFloat(ShaderProperty_Value, 1f);
			sliderBGs[2].material.SetFloat(ShaderProperty_Hue, color_hue);
			sliderBGs[2].material.SetFloat(ShaderProperty_Saturation, 1f);
		}
		else
		{
			sliderBGs[0].material.SetFloat(ShaderProperty_Green, editColor.g);
			sliderBGs[0].material.SetFloat(ShaderProperty_Blue, editColor.b);
			sliderBGs[1].material.SetFloat(ShaderProperty_Red, editColor.r);
			sliderBGs[1].material.SetFloat(ShaderProperty_Blue, editColor.b);
			sliderBGs[2].material.SetFloat(ShaderProperty_Red, editColor.r);
			sliderBGs[2].material.SetFloat(ShaderProperty_Green, editColor.g);
		}
		Color color = InverseColor(editColor);
		color.a = 1f;
		point.color = color;
	}

	private void SetHasAlpha(bool flag)
	{
		hasAlpha = flag;
		sliders[3].gameObject.SetActive(hasAlpha);
	}

	public void ChangeBarType(BAR_TYPE type)
	{
		invoke = false;
		barType = type;
		bar.maxValue = ((type != 0) ? 255f : 359f);
		switch (type)
		{
		case BAR_TYPE.H:
			bar.value = color_hue * 359f;
			barBG.material = matBar_H;
			chartBG.material = matChart_H;
			break;
		case BAR_TYPE.S:
			bar.value = color_saturation * 255f;
			barBG.material = matBar_S;
			chartBG.material = matChart_S;
			break;
		case BAR_TYPE.V:
			bar.value = color_value * 255f;
			barBG.material = matBar_V;
			chartBG.material = matChart_V;
			break;
		case BAR_TYPE.R:
			bar.value = editColor.r * 255f;
			barBG.material = matBar_R;
			chartBG.material = matChart_R;
			break;
		case BAR_TYPE.G:
			bar.value = editColor.g * 255f;
			barBG.material = matBar_G;
			chartBG.material = matChart_G;
			break;
		case BAR_TYPE.B:
			bar.value = editColor.b * 255f;
			barBG.material = matBar_B;
			chartBG.material = matChart_B;
			break;
		}
		ColorToBarChart();
		invoke = true;
	}

	public void ChangeSliderType(SLIDER_TYPE type)
	{
		invoke = false;
		sliderType = type;
		if (sliderType == SLIDER_TYPE.HSV)
		{
			for (int i = 0; i < SliderNames_HSV.Length; i++)
			{
				sliders[i].SetTitle(SliderNames_HSV[i]);
				sliders[i].Setup(0f, SliderMax_HSV[i], false, 0f);
			}
			sliderBGs[0].material = matSlider_H;
			sliderBGs[1].material = matSlider_S;
			sliderBGs[2].material = matSlider_V;
		}
		else if (sliderType == SLIDER_TYPE.RGB)
		{
			for (int j = 0; j < SliderNames_RGB.Length; j++)
			{
				sliders[j].SetTitle(SliderNames_RGB[j]);
				sliders[j].Setup(0f, SliderMax_RGB[j], false, 0f);
			}
			sliderBGs[0].material = matSlider_R;
			sliderBGs[1].material = matSlider_G;
			sliderBGs[2].material = matSlider_B;
		}
		ColorToSliders();
		invoke = true;
	}

	public void ChangeChart(Vector2 val)
	{
		if (invoke)
		{
			if (barType == BAR_TYPE.H)
			{
				ChangeColorHSV(color_hue, val.x, val.y, editColor.a);
			}
			else if (barType == BAR_TYPE.S)
			{
				ChangeColorHSV(val.x, color_saturation, val.y, editColor.a);
			}
			else if (barType == BAR_TYPE.V)
			{
				ChangeColorHSV(val.x, val.y, color_value, editColor.a);
			}
			else if (barType == BAR_TYPE.R)
			{
				ChangeColorRGB(editColor.r, val.x, val.y, editColor.a);
			}
			else if (barType == BAR_TYPE.G)
			{
				ChangeColorRGB(val.x, editColor.g, val.y, editColor.a);
			}
			else if (barType == BAR_TYPE.B)
			{
				ChangeColorRGB(val.x, val.y, editColor.b, editColor.a);
			}
			ColorToSliders();
		}
	}

	public void ChangeSlider_0(float val)
	{
		if (invoke)
		{
			SlidersToColor();
			ColorToBarChart();
		}
	}

	public void ChangeSlider_1(float val)
	{
		if (invoke)
		{
			SlidersToColor();
			ColorToBarChart();
		}
	}

	public void ChangeSlider_2(float val)
	{
		if (invoke)
		{
			SlidersToColor();
			ColorToBarChart();
		}
	}

	public void ChangeSlider_3(float val)
	{
		if (invoke)
		{
			SlidersToColor();
			ColorToBarChart();
		}
	}

	public void ChangeBar(float val)
	{
		if (invoke)
		{
			if (barType == BAR_TYPE.H)
			{
				ChangeColorHSV(val / 359f, color_saturation, color_value, editColor.a);
			}
			else if (barType == BAR_TYPE.S)
			{
				ChangeColorHSV(color_hue, val / 255f, color_value, editColor.a);
			}
			else if (barType == BAR_TYPE.V)
			{
				ChangeColorHSV(color_hue, color_saturation, val / 255f, editColor.a);
			}
			else if (barType == BAR_TYPE.R)
			{
				ChangeColorRGB(val / 255f, editColor.g, editColor.b, editColor.a);
			}
			else if (barType == BAR_TYPE.G)
			{
				ChangeColorRGB(editColor.r, val / 255f, editColor.b, editColor.a);
			}
			else if (barType == BAR_TYPE.B)
			{
				ChangeColorRGB(editColor.r, editColor.g, val / 255f, editColor.a);
			}
			ColorToSliders();
		}
	}

	private void SlidersToColor()
	{
		float a = (editColor.a = sliders[3].Value / 255f);
		if (sliderType == SLIDER_TYPE.RGB)
		{
			float r = sliders[0].Value / 255f;
			float g = sliders[1].Value / 255f;
			float b = sliders[2].Value / 255f;
			ChangeColorRGB(r, g, b, a);
		}
		else if (sliderType == SLIDER_TYPE.HSV)
		{
			float h = sliders[0].Value / 359f;
			float s = sliders[1].Value / 255f;
			float v = sliders[2].Value / 255f;
			ChangeColorHSV(h, s, v, a);
		}
	}

	private void ColorToSliders()
	{
		bool flag = invoke;
		invoke = false;
		if (sliderType == SLIDER_TYPE.RGB)
		{
			sliders[0].Value = editColor.r * SliderMax_RGB[0];
			sliders[1].Value = editColor.g * SliderMax_RGB[1];
			sliders[2].Value = editColor.b * SliderMax_RGB[2];
		}
		else if (sliderType == SLIDER_TYPE.HSV)
		{
			sliders[0].Value = color_hue * SliderMax_HSV[0];
			sliders[1].Value = color_saturation * SliderMax_HSV[1];
			sliders[2].Value = color_value * SliderMax_HSV[2];
		}
		sliders[3].Value = editColor.a * 255f;
		invoke = flag;
	}

	private void ColorToBarChart()
	{
		bool flag = invoke;
		invoke = false;
		if (barType == BAR_TYPE.H)
		{
			bar.value = color_hue * SliderMax_HSV[0];
			squarePick.Value = new Vector2(color_saturation, color_value);
		}
		else if (barType == BAR_TYPE.S)
		{
			bar.value = color_saturation * SliderMax_HSV[1];
			squarePick.Value = new Vector2(color_hue, color_value);
		}
		else if (barType == BAR_TYPE.V)
		{
			bar.value = color_value * SliderMax_HSV[2];
			squarePick.Value = new Vector2(color_hue, color_saturation);
		}
		else if (barType == BAR_TYPE.R)
		{
			bar.value = editColor.r * SliderMax_RGB[0];
			squarePick.Value = new Vector2(editColor.g, editColor.b);
		}
		else if (barType == BAR_TYPE.G)
		{
			bar.value = editColor.g * SliderMax_RGB[1];
			squarePick.Value = new Vector2(editColor.r, editColor.b);
		}
		else if (barType == BAR_TYPE.B)
		{
			bar.value = editColor.b * SliderMax_RGB[2];
			squarePick.Value = new Vector2(editColor.r, editColor.g);
		}
		invoke = flag;
	}

	private void Toggle_HSV(bool flag)
	{
		if (invoke && flag)
		{
			ChangeSliderType(SLIDER_TYPE.HSV);
		}
	}

	private void Toggle_RGB(bool flag)
	{
		if (invoke && flag)
		{
			ChangeSliderType(SLIDER_TYPE.RGB);
		}
	}

	private void Toggle_BarH(bool flag)
	{
		if (invoke && flag)
		{
			ChangeBarType(BAR_TYPE.H);
		}
	}

	private void Toggle_BarS(bool flag)
	{
		if (invoke && flag)
		{
			ChangeBarType(BAR_TYPE.S);
		}
	}

	private void Toggle_BarV(bool flag)
	{
		if (invoke && flag)
		{
			ChangeBarType(BAR_TYPE.V);
		}
	}

	private void Toggle_BarR(bool flag)
	{
		if (invoke && flag)
		{
			ChangeBarType(BAR_TYPE.R);
		}
	}

	private void Toggle_BarG(bool flag)
	{
		if (invoke && flag)
		{
			ChangeBarType(BAR_TYPE.G);
		}
	}

	private void Toggle_BarB(bool flag)
	{
		if (invoke && flag)
		{
			ChangeBarType(BAR_TYPE.B);
		}
	}

	private void ChangeColorHSV(float h, float s, float v, float a)
	{
		color_hue = h;
		color_saturation = s;
		color_value = v;
		editColor = Color.HSVToRGB(color_hue, color_saturation, color_value);
		editColor.a = a;
		OnChangeColor();
	}

	private void ChangeColorRGB(Color color)
	{
		editColor = color;
		Color.RGBToHSV(editColor, out color_hue, out color_saturation, out color_value);
		OnChangeColor();
	}

	private void ChangeColorRGB(float r, float g, float b, float a)
	{
		editColor.r = r;
		editColor.g = g;
		editColor.b = b;
		editColor.a = a;
		Color.RGBToHSV(editColor, out color_hue, out color_saturation, out color_value);
		OnChangeColor();
	}

	public void SetColor(Color value)
	{
		if (!hasAlpha)
		{
			value.a = 1f;
		}
		ChangeColorRGB(value);
		ColorToBarChart();
		ColorToSliders();
	}

	public void Setup(Color value, bool hasAlpha, Action<Color> onChange)
	{
		invoke = false;
		this.onChange = onChange;
		SetHasAlpha(hasAlpha);
		SetColor(value);
		invoke = true;
	}

	public static Color InverseColor(Color baseColor)
	{
		float S;
		float H;
		float V;
		Color.RGBToHSV(baseColor, out H, out S, out V);
		H = (H + 0.5f) % 1f;
		V = 1f - V;
		return Color.HSVToRGB(H, S, V);
	}

	private void OnChangeColor()
	{
		if (invoke && onChange != null)
		{
			onChange(editColor);
		}
	}
}
