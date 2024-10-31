using System;
using UnityEngine;

public class VectorUtility
{
	public static float Vector3_ToYaw(Vector3 v)
	{
		float magnitude = v.magnitude;
		if (magnitude == 0f)
		{
			return 0f;
		}
		float y = v.x / magnitude;
		float x = v.z / magnitude;
		return Mathf.Atan2(y, x) * 57.29578f;
	}

	public static float Vector3_ToPitch(Vector3 v)
	{
		float magnitude = v.magnitude;
		if (magnitude == 0f)
		{
			return 0f;
		}
		float value = v.y / magnitude;
		value = Mathf.Clamp(value, -1f, 1f);
		return Mathf.Asin(0f - value) * 57.29578f;
	}

	public static void Vector3_ToYawPitch(Vector3 v, out float yaw, out float pitch)
	{
		yaw = Vector3_ToYaw(v);
		pitch = Vector3_ToPitch(v);
	}

	public static Vector3 Vector3_FromYawPitch(float yaw, float pitch)
	{
		float f = pitch * ((float)Math.PI / 180f);
		float f2 = yaw * ((float)Math.PI / 180f);
		Vector3 result = default(Vector3);
		result.x = Mathf.Cos(f) * Mathf.Sin(f2);
		result.y = 0f - Mathf.Sin(f);
		result.z = Mathf.Cos(f) * Mathf.Cos(f2);
		result.Normalize();
		return result;
	}
}
