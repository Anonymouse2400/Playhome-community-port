using System;
using UnityEngine;

[Serializable]
public class MeshLightMapData
{
	public Renderer renderer;

	public int lightmapIndex = -1;

	public Vector4 lightmapScaleOffset;

	public void Record(Renderer renderer)
	{
		this.renderer = renderer;
		lightmapIndex = renderer.lightmapIndex;
		lightmapScaleOffset = renderer.lightmapScaleOffset;
	}

	public void Apply(LightMapControl.LightTexSet[] sets)
	{
		renderer.lightmapIndex = lightmapIndex;
		renderer.lightmapScaleOffset = lightmapScaleOffset;
		if (lightmapIndex >= 0)
		{
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetTexture("unity_Lightmap", sets[lightmapIndex].light);
			materialPropertyBlock.SetTexture("unity_LightmapInd", sets[lightmapIndex].dir);
			materialPropertyBlock.SetVector("unity_LightmapST", lightmapScaleOffset);
			renderer.SetPropertyBlock(materialPropertyBlock);
		}
	}
}
