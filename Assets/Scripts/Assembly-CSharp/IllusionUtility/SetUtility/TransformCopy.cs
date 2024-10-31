using System;
using UnityEngine;

namespace IllusionUtility.SetUtility
{
	public static class TransformCopy
	{
		public static void CopyPosRotScl(this Transform dst, Transform src)
		{
			dst.localPosition = src.localPosition;
			dst.localRotation = src.localRotation;
			dst.localScale = src.localScale;
			dst.position = src.position;
			dst.rotation = src.rotation;
		}

		public static void Identity(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}
	}
}
