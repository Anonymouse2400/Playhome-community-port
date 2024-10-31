using System;
using UnityEngine;

[Serializable]
public class AnimeParam_Float
{
	public string name;

	public float min;

	public float max = 1f;

	public float val;

	public void Set(Animator anm)
	{
		anm.SetFloat(name, val);
	}
}
