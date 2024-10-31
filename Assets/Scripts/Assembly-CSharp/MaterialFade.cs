using System;
using UnityEngine;

public class MaterialFade : MonoBehaviour
{
	public float fadeStart = 1f;

	public float fadeEnd = 5f;

	private float timer;

	private Renderer[] rends;

	private void Start()
	{
		rends = GetComponentsInChildren<Renderer>();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= fadeStart)
		{
			float a = 1f - Mathf.InverseLerp(fadeStart, fadeEnd, timer);
			Renderer[] array = rends;
			foreach (Renderer renderer in array)
			{
				renderer.material.color = new Color(1f, 1f, 1f, a);
			}
		}
	}
}
