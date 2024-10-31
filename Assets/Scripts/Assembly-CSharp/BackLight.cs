using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class BackLight : MonoBehaviour
{
	private Light light;

	private void Awake()
	{
		light = GetComponent<Light>();
	}

	private void Update()
	{
		light.enabled = ConfigData.backLightIntensity > 0f;
		light.intensity = ConfigData.backLightIntensity;
	}
}
