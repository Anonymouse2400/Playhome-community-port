using System;
using UnityEngine;

public class GuideDrive_Rot : GuideDrive
{
	public enum MoveType
	{
		X = 1,
		Y = 2,
		Z = 4
	}

	[SerializeField]
	private MoveType moveType;

	[SerializeField]
	private float speed = 0.001f;

	[SerializeField]
	private float ringRadius = 0.065f;

	[SerializeField]
	private float sideDot = 0.1f;

	private Vector3 prevPlanePos;

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		if (!onMove)
		{
			return;
		}
		Vector3 vector = Camera.main.transform.rotation * Vector3.forward;
		Vector3 vector2 = base.transform.rotation * new Vector3(1f, 0f, 0f);
		float num = Vector3.Angle(vector, vector2);
		Vector3 zero = Vector3.zero;
		float f = Vector3.Dot(vector, vector2);
		if (Mathf.Abs(f) > sideDot)
		{
			Vector3 position = PlanePos(Input.mousePosition);
			Vector3 vector3 = Quaternion.Euler(0f, 90f, 0f) * base.transform.InverseTransformPoint(prevPlanePos);
			Vector3 vector4 = Quaternion.Euler(0f, 90f, 0f) * base.transform.InverseTransformPoint(position);
			prevPlanePos = position;
			float num2 = Vector2_VectorsToAngle(new Vector2(vector3.x, vector3.y), new Vector2(vector4.x, vector4.y));
			if (moveType == MoveType.X)
			{
				zero.x = num2;
			}
			if (moveType == MoveType.Y)
			{
				zero.y = num2;
			}
			if (moveType == MoveType.Z)
			{
				zero.z = num2;
			}
		}
		else
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = Vector3.Distance(prevPlanePos, Camera.main.transform.position);
			Vector3 position2 = movePrevPos;
			position2.z = Vector3.Distance(prevPlanePos, Camera.main.transform.position);
			Vector3 vector5 = Camera.main.ScreenToWorldPoint(mousePosition) - Camera.main.ScreenToWorldPoint(position2);
			Vector3 position3 = prevPlanePos + vector5;
			Vector3 vector6 = Quaternion.Euler(0f, 90f, 0f) * base.transform.InverseTransformPoint(prevPlanePos);
			Vector3 vector7 = Quaternion.Euler(0f, 90f, 0f) * base.transform.InverseTransformPoint(position3);
			prevPlanePos = position3;
			float num3 = Vector2_VectorsToAngle(new Vector2(vector6.x, vector6.y), new Vector2(vector7.x, vector7.y));
			if (moveType == MoveType.X)
			{
				zero.x = num3;
			}
			if (moveType == MoveType.Y)
			{
				zero.y = num3;
			}
			if (moveType == MoveType.Z)
			{
				zero.z = num3;
			}
		}
		manager.DriveMoveRotation(Quaternion.Euler(zero));
		if (Input.GetMouseButtonUp(0))
		{
			OnMoveEnd();
			manager.OnMoveEnd(this);
		}
		movePrevPos = Input.mousePosition;
	}

	public override void OnMoveStart(Vector3 clickPos)
	{
		onMove = true;
		movePrevPos = Input.mousePosition;
		manager.OnMoveStart(this);
		prevPlanePos = PlanePos(Input.mousePosition);
	}

	private Vector3 PlanePos(Vector3 screenPos)
	{
		Vector3 inNormal = base.transform.rotation * new Vector3(1f, 0f, 0f);
		Plane plane = new Plane(inNormal, base.transform.position);
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		float enter = 0f;
		if (plane.Raycast(ray, out enter))
		{
			return ray.GetPoint(enter);
		}
		return manager.transform.position;
	}

	public static float Vector2_VectorsToAngle(Vector2 v1, Vector2 v2)
	{
		float current = Mathf.Atan2(v1.x, v1.y) * 57.29578f;
		float target = Mathf.Atan2(v2.x, v2.y) * 57.29578f;
		return Mathf.DeltaAngle(current, target);
	}
}
