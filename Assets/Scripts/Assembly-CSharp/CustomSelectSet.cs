using System;
using UnityEngine;

public class CustomSelectSet
{
	public int id;

	public string name;

	public Sprite thumbnail_S;

	public Sprite thumbnail_L;

	public bool enable;

	public bool hide;

	public bool isNew;

	public CustomSelectSet(int id, string name, Sprite thumbnail_S, Sprite thumbnail_L, bool isNew)
	{
		this.id = id;
		this.name = name;
		this.thumbnail_S = thumbnail_S;
		this.thumbnail_L = thumbnail_L;
		enable = true;
		hide = false;
		this.isNew = isNew;
	}
}
