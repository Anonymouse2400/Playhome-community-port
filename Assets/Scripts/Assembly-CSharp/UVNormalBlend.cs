using System;
using System.Collections.Generic;
using UnityEngine;

public class UVNormalBlend : MonoBehaviour
{
	[Serializable]
	public class Data
	{
		public string rendererName;

		public Renderer renderer;

		public Vector3[] baseNormals;

		public Vector3[] blendNormals;

		public Vector2[] baseUVs;

		public Vector2[] blendUVs;

		public Mesh cloneMesh { get; set; }
	}

	private float rate;

	[SerializeField]
	private bool changeUV = true;

	[SerializeField]
	private bool changeNormal = true;

	[SerializeField]
	private Data[] datas;

	public float Rate
	{
		get
		{
			return rate;
		}
		set
		{
			rate = value;
			Calc();
		}
	}

	public bool IsSetuped { get; private set; }

	private void Awake()
	{
		if (!IsSetuped)
		{
			Setup();
		}
		Calc();
	}

	private void Update()
	{
	}

	public void Setup()
	{
		for (int i = 0; i < datas.Length; i++)
		{
			SetupMesh(datas[i]);
		}
		IsSetuped = true;
	}

	private void SetupMesh(Data data)
	{
		data.cloneMesh = CloneMesh(data.renderer.gameObject);
	}

	private void Calc()
	{
		for (int i = 0; i < datas.Length; i++)
		{
			CalcData(datas[i]);
		}
	}

	private void CalcData(Data data)
	{
		if (changeNormal)
		{
			if (data.cloneMesh.normals.Length == data.baseNormals.Length)
			{
				Vector3[] array = new Vector3[data.baseNormals.Length];
				for (int i = 0; i < data.baseNormals.Length; i++)
				{
					array[i] = Vector3.Lerp(data.baseNormals[i], data.blendNormals[i], rate);
				}
				data.cloneMesh.normals = array;
			}
			else
			{
				Debug.LogWarning("法線の数が違う");
			}
		}
		if (!changeUV)
		{
			return;
		}
		if (data.cloneMesh.uv.Length == data.baseUVs.Length)
		{
			Vector2[] array2 = new Vector2[data.baseUVs.Length];
			for (int j = 0; j < data.baseUVs.Length; j++)
			{
				array2[j] = Vector3.Lerp(data.baseUVs[j], data.blendUVs[j], rate);
			}
			data.cloneMesh.uv = array2;
		}
		else
		{
			Debug.LogWarning("UVの数が違う");
		}
	}

	public void Change(float rate)
	{
		this.rate = rate;
		Calc();
	}

	public void CreateData(Transform trans)
	{
		Renderer[] componentsInChildren = base.transform.GetComponentsInChildren<Renderer>(true);
		Renderer[] componentsInChildren2 = trans.GetComponentsInChildren<Renderer>(true);
		List<Data> list = new List<Data>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (componentsInChildren[i].name == componentsInChildren2[j].name)
				{
					Data data = CreateData(componentsInChildren[i], componentsInChildren2[j]);
					if (data != null)
					{
						list.Add(data);
					}
				}
			}
		}
		datas = list.ToArray();
		MonoBehaviour.print("データ作成完了:" + datas.Length + "個");
	}

	public void CreateData(NormalData blendData)
	{
		Renderer[] componentsInChildren = base.transform.GetComponentsInChildren<Renderer>(true);
		List<Data> list = new List<Data>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < blendData.data.Count; j++)
			{
				if (componentsInChildren[i].name == blendData.data[j].ObjectName)
				{
					Data data = CreateData(componentsInChildren[i], blendData.data[j]);
					if (data != null)
					{
						list.Add(data);
					}
				}
			}
		}
		datas = list.ToArray();
		changeUV = false;
		MonoBehaviour.print("データ作成完了:" + datas.Length + "個");
	}

	private Data CreateData(Renderer baseRend, Renderer blendRend)
	{
		Data data = new Data();
		Mesh mesh = GetMesh(baseRend.gameObject);
		Mesh mesh2 = GetMesh(blendRend.gameObject);
		data.rendererName = baseRend.name;
		data.renderer = baseRend;
		data.baseNormals = mesh.normals;
		data.baseUVs = mesh.uv;
		data.blendNormals = mesh2.normals;
		data.blendUVs = mesh2.uv;
		if (data.baseNormals.Length != data.blendNormals.Length || data.baseUVs.Length != data.blendUVs.Length)
		{
			Debug.LogWarning("メッシュブレンドのデータ数が合わない");
			return null;
		}
		return data;
	}

	private Data CreateData(Renderer rend, NormalData.Param param)
	{
		Data data = new Data();
		data.rendererName = param.ObjectName;
		data.renderer = rend;
		data.baseNormals = param.NormalMax.ToArray();
		data.baseUVs = null;
		data.blendNormals = param.NormalMin.ToArray();
		data.blendUVs = null;
		if (data.baseNormals.Length != data.blendNormals.Length)
		{
			Debug.LogWarning("メッシュブレンドのデータ数が合わない");
			return null;
		}
		return data;
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
			return component.sharedMesh;
		}
		SkinnedMeshRenderer component2 = OriginalData.GetComponent<SkinnedMeshRenderer>();
		if ((bool)component2 && (bool)component2.sharedMesh)
		{
			return component2.sharedMesh;
		}
		return null;
	}

	public Mesh GetClonedMesh(string name)
	{
		for (int i = 0; i < datas.Length; i++)
		{
			if (datas[i].rendererName == name)
			{
				return datas[i].cloneMesh;
			}
		}
		return null;
	}
}
