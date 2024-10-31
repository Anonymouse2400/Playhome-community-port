using System;
using UnityEngine;

public class GuideDrive_Pos : GuideDrive
{
	public enum MoveType
	{
		X = 1,
		Y = 2,
		Z = 4,
		XY = 3,
		YZ = 6,
		XZ = 5,
		XYZ = 7
	}

	public MoveType moveType;

	public float speed = 0.001f;

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		if (onMove)
		{
			if (moveType == MoveType.XY || moveType == MoveType.YZ || moveType == MoveType.XZ)
			{
				Vector3 move = PlanePos(Input.mousePosition) - PlanePos(movePrevPos);
				movePrevPos = Input.mousePosition;
				manager.DriveMovePosition(move);
			}
			else
			{
				Vector3 point = AxisPos(Input.mousePosition) - AxisPos(movePrevPos);
				movePrevPos = Input.mousePosition;
				Vector3 zero = Vector3.zero;
				zero = base.transform.rotation * new Vector3(0f, 1f, 0f);
				point = ClosestPoint(point, Vector3.zero, zero);
				manager.DriveMovePosition(point);
			}
			if (Input.GetMouseButtonUp(0))
			{
				OnMoveEnd();
				manager.OnMoveEnd(this);
			}
		}
	}

	private Vector3 PlanePos(Vector3 screenPos)
	{
		Vector3 inNormal = base.transform.rotation * Vector3.up;
		Plane plane = new Plane(inNormal, base.transform.position);
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		float enter = 0f;
		if (plane.Raycast(ray, out enter))
		{
			return ray.GetPoint(enter);
		}
		return manager.transform.position;
	}

	private Vector3 AxisPos(Vector3 screenPos)
	{
		Vector3 rhs = Camera.main.transform.rotation * Vector3.forward;
		Vector3 zero = Vector3.zero;
		zero = base.transform.rotation * new Vector3(0f, 1f, 0f);
		Vector3 vector = Vector3.Cross(zero, rhs);
		Vector3 position = base.transform.position;
		Vector3 b = base.transform.position + zero;
		Vector3 c = base.transform.position + vector;
		Plane plane = new Plane(position, b, c);
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		float enter = 0f;
		if (plane.Raycast(ray, out enter))
		{
			return ray.GetPoint(enter);
		}
		return manager.transform.position;
	}

	private Vector3 ClosestPoint(Vector3 point, Vector3 center, Vector3 ab)
	{
		float num = Vector3.Dot(point - center, ab) / Vector3.Dot(ab, ab);
		return center + ab * num;
	}
}
