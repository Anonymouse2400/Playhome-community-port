using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

internal class HairObj
{
	public GameObject obj;

	private List<Material> hairMaterials = new List<Material>();

	private List<Material> acceMaterials = new List<Material>();

	public bool hasAcce { get; private set; }

	public HairObj(GameObject obj, Transform hirsParent)
	{
		this.obj = obj;
		SetParent(hirsParent);
		SetupMaterial();
		hasAcce = acceMaterials.Count > 0;
	}

	private void SetupMaterial()
	{
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			if (renderer.tag == "ObjHair")
			{
				Material[] materials = renderer.materials;
				foreach (Material item in materials)
				{
					hairMaterials.Add(item);
				}
			}
			else if (renderer.tag == "ObjHairAcs")
			{
				Material[] materials2 = renderer.materials;
				foreach (Material item2 in materials2)
				{
					acceMaterials.Add(item2);
				}
			}
		}
	}

	public void SetParent(Transform hirsParent)
	{
		obj.transform.SetParent(hirsParent, false);
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localPosition = Vector3.zero;
	}

	public void ChangeColor(HairPartParameter param)
	{
		for (int i = 0; i < hairMaterials.Count; i++)
		{
			param.hairColor.SetToMaterial(hairMaterials[i]);
		}
		if (hasAcce)
		{
			if (param.acceColor == null)
			{
				param.acceColor = new ColorParameter_PBR1(acceMaterials[0]);
				return;
			}
			for (int j = 0; j < acceMaterials.Count; j++)
			{
				param.acceColor.SetToMaterial(acceMaterials[j]);
			}
		}
		else
		{
			param.acceColor = null;
		}
	}
}
