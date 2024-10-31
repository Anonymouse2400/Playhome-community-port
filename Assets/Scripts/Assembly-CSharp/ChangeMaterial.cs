using System;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
	public Material mat;

	public string ignoreAlphaShader = "Skin";

	public void Start()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			Material[] array2 = new Material[renderer.materials.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				Material material = new Material(mat);
				if (renderer.materials[j].shader.name.IndexOf(ignoreAlphaShader) == -1)
				{
					material.mainTexture = renderer.materials[j].mainTexture;
				}
				array2[j] = material;
			}
			renderer.materials = array2;
		}
	}
}
