using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class SkinMaterialDebug : MonoBehaviour
{
	private Renderer rend;

	[SerializeField]
	private Text text;

	private void Start()
	{
		rend = GetComponent<Renderer>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			ChangeMaterial_Body();
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			ChangeMaterial_WearBody();
		}
	}

	private void ChangeMaterial_Body()
	{
		Human human = UnityEngine.Object.FindObjectOfType<Human>();
		rend.material = human.body.SkinMaterial;
		text.text = (rend.sharedMaterial).name;
	}

	private void ChangeMaterial_WearBody()
	{
		Human human = UnityEngine.Object.FindObjectOfType<Human>();
		GameObject obj = human.wears.GetWearObj(WEAR_TYPE.TOP).obj;
		SkinnedMeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		string value = "cf_m_body";
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			int num = -1;
			for (int j = 0; j < skinnedMeshRenderer.sharedMaterials.Length; j++)
			{
				if ((skinnedMeshRenderer.sharedMaterials[j]).name.ToLower().IndexOf(value) == 0)
				{
					num = j;
					break;
				}
			}
			if (num != -1)
			{
				rend.sharedMaterial = skinnedMeshRenderer.sharedMaterials[num];
				break;
			}
		}
		text.text = (rend.sharedMaterial).name;
	}
}
