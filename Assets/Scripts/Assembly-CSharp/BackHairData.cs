using System;
using UnityEngine;

public class BackHairData : HairData
{
	public enum HAIR_TYPE
	{
		SHORT = 0,
		SEMI_LONG = 1,
		LONG = 2,
		PONY = 3,
		TWIN = 4,
		OTHER = 5
	}

	public HAIR_TYPE type;

	public bool isSet;

	public BackHairData(int id, string name, string assetbundle, string prefab, int order, bool isNew, string typeStr, bool isSet)
		: base(id, name, assetbundle, prefab, order, isNew)
	{
		type = CheckTypeName(typeStr);
		this.isSet = isSet;
	}

	public static HAIR_TYPE CheckTypeName(string check)
	{
		string[] array = new string[6] { "ショート", "セミロング", "ロング", "ポニー", "ツインテール", "その他" };
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == check)
			{
				return (HAIR_TYPE)i;
			}
		}
		Debug.LogWarning("不明なタイプ:" + check);
		return HAIR_TYPE.OTHER;
	}
}
