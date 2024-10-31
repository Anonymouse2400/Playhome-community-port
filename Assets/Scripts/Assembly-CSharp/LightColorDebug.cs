using System;
using System.Collections.Generic;
using UnityEngine;

public class LightColorDebug : MonoBehaviour
{
	public class MapLight
	{
		private class LightSet
		{
			public string name;

			public Color color;

			public float intensity = 1f;

			public LightSet(ListDataLoader loader, int x, int y)
			{
				name = loader.Get(x, y);
				float h = loader.GetFloat(x, y + 1) / 359f;
				float s = loader.GetFloat(x, y + 2) / 255f;
				float v = loader.GetFloat(x, y + 3) / 255f;
				color = Color.HSVToRGB(h, s, v);
				intensity = loader.GetFloat(x, y + 4);
			}
		}

		private class TimeZone
		{
			public LightSet[] lights = new LightSet[2];

			public float ambient = 1f;

			public TimeZone(ListDataLoader loader, int x, int y)
			{
				lights[0] = new LightSet(loader, x, y);
				lights[1] = new LightSet(loader, x + 1, y);
				ambient = loader.GetFloat(x, y + 5);
			}

			public void Set(Transform mapRoot)
			{
				int num = 0;
				Light[] componentsInChildren = mapRoot.GetComponentsInChildren<Light>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].name == lights[0].name)
					{
						SetLight(componentsInChildren[i], lights[0]);
						num |= 1;
					}
					else if (componentsInChildren[i].name == lights[1].name)
					{
						SetLight(componentsInChildren[i], lights[1]);
						num |= 2;
					}
				}
				if ((num & 1) == 0)
				{
					Debug.LogWarning("ライト:" + lights[0].name + "が見つかりませんでした");
				}
				if ((num & 2) == 0)
				{
					Debug.LogWarning("ライト:" + lights[1].name + "が見つかりませんでした");
				}
				if (RenderSettings.ambientIntensity != ambient)
				{
					RenderSettings.ambientIntensity = ambient;
					Debug.Log("アンビエント変更");
				}
			}

			private void SetLight(Light light, LightSet set)
			{
				if (light.color != set.color || light.intensity != set.intensity)
				{
					Debug.Log("ライトカラー変更");
				}
				light.color = set.color;
				light.intensity = set.intensity;
				AlloyAreaLight component = light.GetComponent<AlloyAreaLight>();
				if (component != null)
				{
					component.Color = set.color;
					component.Intensity = set.intensity;
				}
			}
		}

		private TimeZone[] timeZones = new TimeZone[4];

		public string name;

		public MapLight(ListDataLoader loader, int y)
		{
			name = loader.Get(0, y);
			for (int i = 0; i < timeZones.Length; i++)
			{
				int x = 2 + 3 * i;
				timeZones[i] = new TimeZone(loader, x, y);
			}
		}

		public void Set(Transform mapRoot, int timeNo)
		{
			timeZones[timeNo].Set(mapRoot);
		}
	}

	[SerializeField]
	private TextAsset mapLightColorText;

	private List<MapLight> mapLights = new List<MapLight>();

	public void Load()
	{
		ListDataLoader listDataLoader = new ListDataLoader('\t', StringSplitOptions.None);
		listDataLoader.Load_Text(mapLightColorText);
		for (int i = 0; i < listDataLoader.Y_Num; i++)
		{
			string text = listDataLoader.Get(0, i);
			if (text.Length > 0)
			{
				MapLight item = new MapLight(listDataLoader, i);
				mapLights.Add(item);
			}
		}
	}

	public void SetToMap(int mapNo, int timeNo, Transform mapRoot)
	{
		mapLights[mapNo].Set(mapRoot, timeNo);
	}
}
