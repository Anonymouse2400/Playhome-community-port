using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (!instance)
			{
				instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
				if (!instance)
				{
					Debug.LogError(string.Concat(typeof(T), " is none."));
				}
			}
			return instance;
		}
	}

	protected void Awake()
	{
		CheckInstance();
	}

	protected bool CheckInstance()
	{
		if (this == Instance)
		{
			return true;
		}
        UnityEngine.Object.Destroy(this);
		return false;
	}
}
