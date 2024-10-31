using System;
using System.Collections.Generic;
using UnityEngine;

public class H_Light : MonoBehaviour
{
	private class StyleLight
	{
		public string styleName;

		public List<Vector3> dir = new List<Vector3>();

		public StyleLight(ListDataLoader loader, int y)
		{
			styleName = loader.Get(0, y);
			for (int i = 0; i < 3; i++)
			{
				int num = i * 4 + 1;
				string text = loader.Get(num, y);
				string text2 = loader.Get(num + 1, y);
				if (text.Length != 0 && text2.Length != 0)
				{
					float x = StrToFloat(text);
					float y2 = StrToFloat(text2);
					dir.Add(new Vector3(x, y2, 0f));
					continue;
				}
				break;
			}
		}

		private float StrToFloat(string str)
		{
			float result = 0f;
			try
			{
				result = float.Parse(str);
			}
			catch
			{
				Debug.LogError("数字に変換できない:" + str);
			}
			return result;
		}
	}

	private Dictionary<string, StyleLight> styleLights = new Dictionary<string, StyleLight>();

	public void ChangeMap(string map_name)
	{
		styleLights.Clear();
		string assetName = "H_Light_" + map_name;
		string assetBundleName = "h/h_light";
		TextAsset textAsset = AssetBundleLoader.LoadAsset<TextAsset>(GlobalData.assetBundlePath, assetBundleName, assetName);
		if (!(textAsset != null))
		{
			return;
		}
		ListDataLoader listDataLoader = new ListDataLoader('\t', StringSplitOptions.None);
		listDataLoader.Load_Text(textAsset);
		for (int i = 0; i < listDataLoader.Y_Num; i++)
		{
			string text = listDataLoader.Get(0, i);
			if (text.Length > 0)
			{
				StyleLight styleLight = new StyleLight(listDataLoader, i);
				styleLights.Add(styleLight.styleName, styleLight);
			}
		}
	}

	public Vector3 Get(string id, int no)
	{
		Vector3 zero = Vector3.zero;
		if (styleLights.ContainsKey(id))
		{
			StyleLight styleLight = styleLights[id];
			if (no < styleLight.dir.Count)
			{
				return styleLight.dir[no];
			}
		}
		return zero;
	}
}
