using System;
using UnityEngine;

public class LightController : MonoBehaviour
{
	private class Set
	{
		public Light light;

		public float defPower;

		public Color defColor;

		public Set(Light light)
		{
			this.light = light;
			defPower = light.intensity;
			defColor = light.color;
		}
	}

	[SerializeField]
	private InputSliderUI sliderPitch;

	[SerializeField]
	private InputSliderUI sliderYaw;

	[SerializeField]
	private InputSliderUI sliderPow;

	[SerializeField]
	private ColorChangeButton colorChange;

	private float power = 1f;

	private Color color = Color.white;

	private Set[] lightSets;

	private bool invoke = true;

	private float yaw;

	private float pitch;

	private float defYaw;

	private float defPitch;

	private void Awake()
	{
		sliderPitch.AddOnChangeAction(OnSliderPitch);
		sliderYaw.AddOnChangeAction(OnSliderYaw);
		sliderPow.AddOnChangeAction(OnSliderPower);
	}

	public void SetupLight()
	{
		Light light = null;
		Light[] array = UnityEngine.Object.FindObjectsOfType<Light>();
		Light[] array2 = array;
		foreach (Light light2 in array2)
		{
			if (light2.name.IndexOf("MainLight") == 0)
			{
				light = light2;
				break;
			}
		}
		SetupLight(light.transform, true);
	}

	public void SetupLight(Transform mainLight, bool setDef)
	{
		invoke = false;
		if (mainLight != null)
		{
			Light[] componentsInChildren = mainLight.GetComponentsInChildren<Light>();
			lightSets = new Set[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				lightSets[i] = new Set(componentsInChildren[i]);
			}
			if (setDef)
			{
				Vector3 eulerAngles = componentsInChildren[0].transform.rotation.eulerAngles;
				defYaw = Mathf.DeltaAngle(0f, eulerAngles.y);
				if (defYaw > 180f)
				{
					defYaw -= 360f;
				}
				defPitch = Mathf.DeltaAngle(0f, eulerAngles.x);
				if (defPitch > 180f)
				{
					defPitch -= 360f;
				}
				yaw = defYaw;
				pitch = defPitch;
				sliderPitch.SetValue(pitch);
				sliderYaw.SetValue(yaw);
				sliderPow.SetValue(power);
			}
		}
		colorChange.Setup("色", color, false, OnChangeColor);
		if (!setDef)
		{
			CalcRotate();
			CalcColor();
		}
		invoke = true;
	}

	private void OnSliderYaw(float val)
	{
		if (invoke)
		{
			yaw = val;
			CalcRotate();
		}
	}

	private void OnSliderPitch(float val)
	{
		if (invoke)
		{
			pitch = val;
			CalcRotate();
		}
	}

	private void OnSliderPower(float val)
	{
		if (invoke)
		{
			power = val;
			CalcColor();
		}
	}

	private void OnChangeColor(Color val)
	{
		if (invoke)
		{
			color = val;
			CalcColor();
		}
	}

	private void CalcRotate()
	{
		if (lightSets.Length > 0)
		{
			lightSets[0].light.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
		}
	}

	private void CalcColor()
	{
		for (int i = 0; i < lightSets.Length; i++)
		{
			AlloyAreaLight component = lightSets[i].light.GetComponent<AlloyAreaLight>();
			float intensity = lightSets[i].defPower * power;
			Color color = lightSets[i].defColor * this.color;
			component.Intensity = intensity;
			component.Color = color;
			lightSets[i].light.intensity = intensity;
			lightSets[i].light.color = color;
		}
	}

	public void Reset()
	{
		yaw = defYaw;
		pitch = defPitch;
		power = 1f;
		color = Color.white;
		invoke = false;
		sliderPitch.SetValue(pitch);
		sliderYaw.SetValue(yaw);
		sliderPow.SetValue(power);
		colorChange.SetColor(color);
		invoke = true;
		CalcRotate();
		CalcColor();
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	public void SetDirection(Vector3 euler)
	{
		defPitch = Mathf.DeltaAngle(0f, euler.x);
		if (defPitch > 180f)
		{
			defPitch -= 360f;
		}
		if (defPitch > 90f)
		{
			defPitch = 180f - defPitch;
			euler.y += 180f;
		}
		else if (defPitch < -90f)
		{
			defPitch = -180f - defPitch;
			euler.y += 180f;
		}
		defYaw = Mathf.DeltaAngle(0f, euler.y);
		if (defYaw > 180f)
		{
			defYaw -= 360f;
		}
		yaw = defYaw;
		pitch = defPitch;
		invoke = false;
		sliderPitch.SetValue(pitch);
		sliderYaw.SetValue(yaw);
		sliderPow.SetValue(power);
		invoke = true;
		CalcRotate();
	}
}
