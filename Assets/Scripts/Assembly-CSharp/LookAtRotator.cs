using System;
using UnityEngine;
using Utility;

public class LookAtRotator : MonoBehaviour
{
	public enum TYPE
	{
		NO = 0,
		TARGET = 1,
		AWAY = 2,
		FORWARD = 3,
		DIRECTION = 4,
		DIRECTION_UNLIMITED = 5,
		HOLD = 6
	}

	[Serializable]
	private class RotateBone
	{
		public Transform bone;

		public float maxLeft = 45f;

		public float maxRight = 45f;

		private Quaternion baseRot;

		private Vector3 euler;

		private Vector3 prevEuler;

		public Vector3 Euler
		{
			get
			{
				return euler;
			}
			set
			{
				euler = value;
			}
		}

		public Vector3 PrevEuler
		{
			get
			{
				return prevEuler;
			}
			set
			{
				prevEuler = value;
			}
		}

		public void Init()
		{
			baseRot = bone.localRotation;
			euler = Vector3.zero;
		}

		public void Change()
		{
			prevEuler = euler;
		}

		public void Rotate(Quaternion rot)
		{
			bone.localRotation = baseRot * rot;
		}

		public void ResetRotate()
		{
			bone.localRotation = baseRot;
		}

		public void LerpAngle(Vector3 rel, float noRate)
		{
			euler.x = Mathf.LerpAngle(prevEuler.x, rel.x, noRate);
			euler.y = Mathf.LerpAngle(prevEuler.y, rel.y, noRate);
			euler.z = Mathf.LerpAngle(prevEuler.z, rel.z, noRate);
		}

		public void Interpolation(Vector3 rel, float speed, float min, float max)
		{
			euler.y = Tween.Spring(euler.y, rel.y, speed, Time.deltaTime, min, max);
			euler.x = Tween.Spring(euler.x, rel.x, speed, Time.deltaTime, min, max);
			euler.z = Tween.Spring(euler.z, rel.z, speed, Time.deltaTime, min, max);
		}
	}

	[SerializeField]
	private Transform baseBone;

	[SerializeField]
	private Transform target;

	[SerializeField]
	private Vector3 targetPos;

	[SerializeField]
	private RotateBone[] rotateBones;

	private Vector3? prevTarget;

	[SerializeField]
	[Range(0f, 90f)]
	private float maxUp = 45f;

	[SerializeField]
	[Range(0f, 90f)]
	private float maxDown = 45f;

	[SerializeField]
	[Range(0f, 2f)]
	private float turnRate = 1f;

	[SerializeField]
	[Range(-45f, 45f)]
	private float offsetPitch;

	[SerializeField]
	private float turnPower = 3f;

	[SerializeField]
	private float turnMinSpeed = 1f;

	[SerializeField]
	private float turnMaxSpeed;

	[SerializeField]
	private float ignoreYaw = 120f;

	[SerializeField]
	private float awayIgnoreYaw = 45f;

	private Quaternion baseLocalRot;

	[SerializeField]
	private TYPE calcType;

	private float noRate;

	public Vector3 TargetPos
	{
		get
		{
			return targetPos;
		}
		set
		{
			targetPos = value;
		}
	}

	public TYPE CalcType
	{
		get
		{
			return calcType;
		}
	}

	public void Init()
	{
		baseLocalRot = baseBone.transform.localRotation;
		if (rotateBones != null && rotateBones.Length > 0)
		{
			for (int i = 0; i < rotateBones.Length; i++)
			{
				rotateBones[i].Init();
			}
		}
	}

	public void Calc()
	{
		if (calcType == TYPE.NO)
		{
			return;
		}
		RotateBone[] array = rotateBones;
		foreach (RotateBone rotateBone in array)
		{
			rotateBone.ResetRotate();
		}
		baseBone.transform.localRotation = baseLocalRot;
		if ((bool)target && (calcType == TYPE.TARGET || calcType == TYPE.AWAY))
		{
			targetPos = target.transform.position;
		}
		Vector3 vector = baseBone.InverseTransformPoint(targetPos);
		Vector3 rel = Vector3.zero;
		RotateBone[] array2 = rotateBones;
		foreach (RotateBone rotateBone2 in array2)
		{
			if (calcType == TYPE.NO)
			{
				rel = rotateBone2.bone.localEulerAngles;
			}
			else if (calcType == TYPE.TARGET)
			{
				VectorUtility.Vector3_ToYawPitch(vector, out rel.y, out rel.x);
				rel.y = Mathf.DeltaAngle(0f, rel.y);
				rel.x = Mathf.DeltaAngle(0f, rel.x) + offsetPitch;
				if (prevTarget.HasValue && Mathf.Abs(rel.y) >= ignoreYaw)
				{
					VectorUtility.Vector3_ToYawPitch(prevTarget.Value, out rel.y, out rel.x);
					rel.y = Mathf.DeltaAngle(0f, rel.y);
					rel.x = Mathf.DeltaAngle(0f, rel.x) + offsetPitch;
				}
				else
				{
					prevTarget = vector;
				}
				rel *= turnRate;
				rel.y = Mathf.Clamp(rel.y, 0f - rotateBone2.maxLeft, rotateBone2.maxRight);
				rel.x = Mathf.Clamp(rel.x, 0f - maxUp, maxDown);
				rel.z = 0f;
			}
			else if (calcType == TYPE.FORWARD)
			{
				rel.y = 0f;
				rel.x = offsetPitch;
				rel.y = Mathf.Clamp(rel.y, 0f - rotateBone2.maxLeft, rotateBone2.maxRight);
				rel.x = Mathf.Clamp(rel.x, 0f - maxUp, maxDown);
				rel.z = 0f;
			}
			else if (calcType == TYPE.DIRECTION)
			{
				rel.y = targetPos.y;
				rel.x = targetPos.x + offsetPitch;
				rel.y = Mathf.Clamp(rel.y, 0f - rotateBone2.maxLeft, rotateBone2.maxRight);
				rel.x = Mathf.Clamp(rel.x, 0f - maxUp, maxDown);
				rel.z = 0f;
			}
			else if (calcType == TYPE.DIRECTION_UNLIMITED)
			{
				rel.y = targetPos.y;
				rel.x = targetPos.x + offsetPitch;
				rel.z = 0f;
			}
			else if (calcType == TYPE.AWAY)
			{
				Vector3 zero = Vector3.zero;
				VectorUtility.Vector3_ToYawPitch(vector, out zero.y, out zero.x);
				zero.y = Mathf.DeltaAngle(0f, zero.y);
				zero.x = Mathf.DeltaAngle(0f, zero.x) + offsetPitch;
				if (prevTarget.HasValue && Mathf.Abs(zero.y) <= awayIgnoreYaw)
				{
					VectorUtility.Vector3_ToYawPitch(prevTarget.Value, out zero.y, out zero.x);
					zero.y = Mathf.DeltaAngle(0f, zero.y);
					zero.x = Mathf.DeltaAngle(0f, zero.x) + offsetPitch;
				}
				else
				{
					prevTarget = vector;
				}
				rel.y = ((!(zero.y > 0f)) ? rotateBone2.maxRight : (0f - rotateBone2.maxLeft));
				rel.x = ((!(zero.x > 0f)) ? maxDown : (0f - maxUp));
				rel.z = 0f;
			}
			if (calcType == TYPE.HOLD)
			{
				rotateBone2.PrevEuler = rotateBone2.Euler;
				Quaternion quaternion = Quaternion.Euler(rotateBone2.Euler);
				Quaternion rot = baseLocalRot * quaternion;
				rotateBone2.Rotate(rot);
			}
			else
			{
				rotateBone2.Interpolation(rel, turnPower, turnMinSpeed, turnMaxSpeed);
				rotateBone2.PrevEuler = rotateBone2.Euler;
				Quaternion quaternion2 = Quaternion.Euler(rotateBone2.Euler);
				Quaternion quaternion3 = baseLocalRot * quaternion2;
				rotateBone2.Rotate(quaternion2);
			}
		}
	}

	public void Change(TYPE type, Transform tgt, bool force)
	{
		calcType = type;
		target = tgt;
		if ((bool)tgt)
		{
			targetPos = tgt.position;
		}
		noRate = ((!force) ? 0f : 1f);
		RotateBone[] array = rotateBones;
		foreach (RotateBone rotateBone in array)
		{
			rotateBone.Change();
		}
	}

	public void Change(TYPE type, Vector3 tgt, bool force)
	{
		calcType = type;
		targetPos = tgt;
		target = null;
		noRate = ((!force) ? 0f : 1f);
		RotateBone[] array = rotateBones;
		foreach (RotateBone rotateBone in array)
		{
			rotateBone.Change();
		}
	}

	private void Start()
	{
		Init();
	}

	private void LateUpdate()
	{
		Calc();
	}
}
