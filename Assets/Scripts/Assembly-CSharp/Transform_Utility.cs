using System;
using UnityEngine;

public static class Transform_Utility
{
	public static Transform FindTransform(Transform transform, string name)
	{
		Transform transform2 = null;
		if (transform == null)
		{
			return null;
		}
		if (transform.name == name)
		{
			return transform;
		}
		foreach (Transform item in transform)
		{
			transform2 = FindTransform(item, name);
			if (transform2 != null)
			{
				return transform2;
			}
		}
		return null;
	}

	public static Transform FindTransform_Partial(Transform transform, string name)
	{
		Transform transform2 = null;
		if (transform.name.IndexOf(name) != -1)
		{
			return transform;
		}
		foreach (Transform item in transform)
		{
			transform2 = FindTransform_Partial(item, name);
			if (transform2 != null)
			{
				return transform2;
			}
		}
		return null;
	}

	public static string GetRelativePath(Transform root, Transform search)
	{
		foreach (Transform item in root)
		{
			string relativePath_Sub = GetRelativePath_Sub(item, search, string.Empty);
			if (relativePath_Sub != null)
			{
				return relativePath_Sub;
			}
		}
		return string.Empty;
	}

	private static string GetRelativePath_Sub(Transform check, Transform search, string str)
	{
		if (str.Length > 0)
		{
			str += "/";
		}
		if (check == search)
		{
			str += search.name;
			return str;
		}
		str += check.name;
		foreach (Transform item in check)
		{
			string relativePath_Sub = GetRelativePath_Sub(item, search, str);
			if (relativePath_Sub != null)
			{
				return relativePath_Sub;
			}
		}
		return null;
	}

	public static Type FindComponent<Type>(string name) where Type : Component
	{
		GameObject gameObject = GameObject.Find(name);
		if (gameObject != null)
		{
			return gameObject.GetComponent<Type>();
		}
		return (Type)null;
	}

	public static Type FindComponent<Type>(GameObject obj, string name) where Type : Component
	{
		Transform transform = FindTransform(obj.transform, name);
		GameObject gameObject = ((!(transform != null)) ? null : transform.gameObject);
		if ((bool)gameObject)
		{
			return gameObject.GetComponent<Type>();
		}
		return (Type)null;
	}

	public static Type GetComponent<Type>(GameObject obj) where Type : Component
	{
		return obj.GetComponent<Type>();
	}
}
