using System;
using UnityEngine;

public class CombineTextureData : ItemDataBase
{
	public string textureName;

	public Vector2 pos;

	public CombineTextureData(int id, string name, string assetbundle, string texture, int x, int y, int order, bool isNew)
		: base(id, name, assetbundle, order, isNew)
	{
		textureName = texture;
		pos.x = x;
		pos.y = y;
	}
}
