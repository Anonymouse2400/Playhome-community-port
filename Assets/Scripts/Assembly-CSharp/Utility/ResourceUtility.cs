using System;
using UnityEngine;

namespace Utility
{
	public class ResourceUtility
	{
		public static UnityEngine.Object CreateInstance(string file)
		{
            UnityEngine.Object @object = Resources.Load(file);
			if (@object == null)
			{
				Debug.LogWarning("not found:" + file);
			}
			return UnityEngine.Object.Instantiate(@object);
		}

		public static UnityEngine.Object CreateInstance(string file, string name)
		{
            UnityEngine.Object @object = Resources.Load(file);
			if (@object == null)
			{
				Debug.LogWarning("not found:" + file);
			}
			@object.name = name;
			return UnityEngine.Object.Instantiate(@object);
		}

		public static UnityEngine.Object CreateInstance(string file, Vector3 pos, Quaternion rot)
		{
           UnityEngine.Object @object = Resources.Load(file);
			if (@object == null)
			{
				Debug.LogWarning("not found:" + file);
			}
			return UnityEngine.Object.Instantiate(@object, pos, rot);
		}

		public static Type CreateInstance<Type>(string file) where Type : UnityEngine.Object
        {
			Type val = Resources.Load<Type>(file);
			if (val == null)
			{
				Debug.LogWarning("not found:" + file);
				return (Type)null;
			}
			return UnityEngine.Object.Instantiate(val);
		}

		public static Type CreateInstance<Type>(string file, string name) where Type : UnityEngine.Object
        {
			Type val = Resources.Load<Type>(file);
			if (val == null)
			{
				Debug.LogWarning("not found:" + file);
			}
			val.name = name;
			return UnityEngine.Object.Instantiate(val);
		}

		public static Type CreateInstance<Type>(string file, Vector3 pos, Quaternion rot) where Type : UnityEngine.Object
        {
			Type val = Resources.Load<Type>(file);
			if (val == null)
			{
				Debug.LogWarning("not found:" + file);
			}
			return UnityEngine.Object.Instantiate(val, pos, rot);
		}
	}
}
