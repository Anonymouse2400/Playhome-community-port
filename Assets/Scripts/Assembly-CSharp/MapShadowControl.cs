using System;
using UnityEngine;
using UnityEngine.Rendering;

public class MapShadowControl : MonoBehaviour
{
	private void Start()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (renderer.shadowCastingMode == ShadowCastingMode.On || renderer.shadowCastingMode == ShadowCastingMode.TwoSided)
			{
				renderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			else if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
			{
				renderer.enabled = false;
			}
		}
		Terrain[] componentsInChildren2 = GetComponentsInChildren<Terrain>();
		Terrain[] array2 = componentsInChildren2;
		foreach (Terrain terrain in array2)
		{
		}
	}

	private void Update()
	{
	}
}
