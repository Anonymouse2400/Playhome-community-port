using System;
using UnityEngine;

public class NormalLerpManager
{
	private Transform root;

	private NormalData targetData;

	private Mesh[] meshs;

	public NormalLerpManager(Transform root, NormalData targetData)
	{
		this.root = root;
		this.targetData = targetData;
		if (targetData == null)
		{
			return;
		}
		meshs = new Mesh[targetData.data.Count];
		for (int i = 0; i < targetData.data.Count; i++)
		{
			Renderer renderer = Transform_Utility.FindComponent<Renderer>(root.gameObject, targetData.data[i].ObjectName);
			if (renderer != null)
			{
				Mesh mesh = CloneMesh(renderer.gameObject);
				if (mesh.normals.Length == targetData.data[i].NormalMin.Count)
				{
					meshs[i] = mesh;
					continue;
				}
				Debug.LogError("メッシュの法線の数が合わない：" + targetData.data[i].ObjectName + " " + mesh.normals.Length + "," + targetData.data[i].NormalMin.Count);
			}
			else
			{
				Debug.LogError("メッシュがない：" + targetData.data[i].ObjectName);
			}
		}
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

	public void Lerp(float rate)
	{
		if (targetData == null)
		{
			return;
		}
		for (int i = 0; i < meshs.Length; i++)
		{
			if (!(meshs[i] == null))
			{
				NormalData.Param param = targetData.data[i];
				Vector3[] array = new Vector3[param.NormalMin.Count];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = Vector3.Lerp(param.NormalMin[j], param.NormalMax[j], rate);
				}
				meshs[i].normals = array;
			}
		}
	}
}
