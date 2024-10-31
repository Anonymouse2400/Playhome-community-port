using System;
using UnityEngine;

public class MeshBlend : MonoBehaviour
{
	[SerializeField]
	private Renderer baseRenderer;

	[SerializeField]
	private Renderer blendRenderer;

	[SerializeField]
	[Range(0f, 1f)]
	private float rate;

	[SerializeField]
	private bool changeUV = true;

	[SerializeField]
	private bool changeNormal = true;

	private Vector3[] baseNormals;

	private Vector3[] blendNormals;

	private Vector2[] baseUVs;

	private Vector2[] blendUVs;

	private bool isBlendEnable;

	private Mesh baseMesh;

	private void OnEnable()
	{
		Setup();
	}

	private void Update()
	{
	}

	private void OnValidate()
	{
		if (!isBlendEnable)
		{
			Setup();
		}
		Calc();
	}

	private void Setup()
	{
		if ((bool)baseRenderer && (bool)blendRenderer)
		{
			baseMesh = CloneMesh(baseRenderer.gameObject);
			Mesh mesh = GetMesh(blendRenderer.gameObject);
			if (baseMesh == null)
			{
				Debug.LogWarning("baseMesh null");
			}
			if (mesh == null)
			{
				Debug.LogWarning("blendMesh null");
			}
			baseNormals = baseMesh.normals;
			baseUVs = baseMesh.uv;
			blendNormals = mesh.normals;
			blendUVs = mesh.uv;
			if (baseNormals.Length == blendNormals.Length && baseUVs.Length == blendUVs.Length)
			{
				isBlendEnable = true;
			}
			else
			{
				isBlendEnable = false;
				Debug.LogWarning("メッシュブレンドのデータ数が合わない");
			}
			blendRenderer.gameObject.SetActive(false);
			Calc();
		}
	}

	private void Calc()
	{
		if (!isBlendEnable)
		{
			return;
		}
		if (changeNormal)
		{
			Vector3[] array = new Vector3[baseNormals.Length];
			for (int i = 0; i < baseNormals.Length; i++)
			{
				array[i] = Vector3.Lerp(baseNormals[i], blendNormals[i], rate);
			}
			baseMesh.normals = array;
		}
		if (changeUV)
		{
			Vector2[] array2 = new Vector2[baseUVs.Length];
			for (int j = 0; j < baseUVs.Length; j++)
			{
				array2[j] = Vector3.Lerp(baseUVs[j], blendUVs[j], rate);
			}
			baseMesh.uv = array2;
		}
	}

	public void Change(float rate)
	{
		this.rate = rate;
		Calc();
	}

	public static Mesh CloneMesh(GameObject OriginalData)
	{
		MeshFilter meshFilter = OriginalData.GetComponent(typeof(MeshFilter)) as MeshFilter;
		if (null != meshFilter)
		{
			Mesh mesh = UnityEngine.Object.Instantiate(meshFilter.sharedMesh);
			mesh.name = meshFilter.sharedMesh.name;
			meshFilter.sharedMesh = mesh;
			return meshFilter.sharedMesh;
		}
		SkinnedMeshRenderer component = OriginalData.GetComponent<SkinnedMeshRenderer>();
		if ((bool)component && (bool)component.sharedMesh)
		{
			Mesh mesh2 = UnityEngine.Object.Instantiate(component.sharedMesh);
			mesh2.name = component.sharedMesh.name;
			component.sharedMesh = mesh2;
			return component.sharedMesh;
		}
		return null;
	}

	public static Mesh GetMesh(GameObject OriginalData)
	{
		MeshFilter component = OriginalData.GetComponent<MeshFilter>();
		if (null != component)
		{
			return component.mesh;
		}
		SkinnedMeshRenderer component2 = OriginalData.GetComponent<SkinnedMeshRenderer>();
		if ((bool)component2 && (bool)component2.sharedMesh)
		{
			return component2.sharedMesh;
		}
		return null;
	}
}
