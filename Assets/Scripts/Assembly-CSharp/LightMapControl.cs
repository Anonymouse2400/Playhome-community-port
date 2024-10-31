using System;
using UnityEngine;

public class LightMapControl : MonoBehaviour
{
	[Serializable]
	public class LightTexSet
	{
		public Texture2D light;

		public Texture2D dir;
	}

	[SerializeField]
	private LightTexSet[] textures;

	[SerializeField]
	private MeshLightMapData[] meshLightMapDatas;

	[SerializeField]
	private LightProbes lightProbes;

	public void Apply()
	{
		LightmapData[] array = new LightmapData[textures.Length];
		for (int i = 0; i < array.Length; i++)
		{
			LightmapData lightmapData = new LightmapData();
			lightmapData.lightmapDir = textures[i].dir;
			lightmapData.lightmapLight = textures[i].light;
			array[i] = lightmapData;
		}
		LightmapSettings.lightmaps = array;
		if (meshLightMapDatas != null)
		{
			for (int j = 0; j < meshLightMapDatas.Length; j++)
			{
				meshLightMapDatas[j].Apply(textures);
			}
		}
		LightmapSettings.lightProbes = lightProbes;
	}

	public void RecordLightMaps()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		meshLightMapDatas = new MeshLightMapData[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			meshLightMapDatas[i] = new MeshLightMapData();
			meshLightMapDatas[i].Record(componentsInChildren[i]);
		}
		textures = new LightTexSet[LightmapSettings.lightmaps.Length];
		for (int j = 0; j < textures.Length; j++)
		{
			textures[j] = new LightTexSet();
			textures[j].light = LightmapSettings.lightmaps[j].lightmapLight;
			textures[j].dir = LightmapSettings.lightmaps[j].lightmapDir;
		}
	}
}
