using System;
using UnityEngine;

public class GuideDrive_PosEasy : GuideDrive_Pos
{
	private Vector3 dir;

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		if (onMove)
		{
			Vector3 move = PlanePos(Input.mousePosition) - PlanePos(movePrevPos);
			movePrevPos = Input.mousePosition;
			manager.DriveMovePosition(move);
			if (Input.GetMouseButtonUp(0))
			{
				OnMoveEnd();
				manager.OnMoveEnd(this);
			}
		}
	}

	public override void OnMoveStart(Vector3 clickPos)
	{
		base.OnMoveStart(clickPos);
		Vector3 rhs = Camera.main.transform.rotation * Vector3.forward;
		Quaternion quaternion = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
		float num = Mathf.Abs(Vector3.Dot(quaternion * Vector3.forward, rhs));
		float num2 = Mathf.Abs(Vector3.Dot(quaternion * Vector3.up, rhs));
		float num3 = Mathf.Abs(Vector3.Dot(quaternion * Vector3.right, rhs));
		if (num >= num2 && num >= num3)
		{
			moveType = MoveType.XY;
			dir = quaternion * Vector3.forward;
		}
		else if (num2 >= num && num2 >= num3)
		{
			moveType = MoveType.XZ;
			dir = quaternion * Vector3.up;
		}
		else
		{
			moveType = MoveType.YZ;
			dir = quaternion * Vector3.right;
		}
	}

	private Vector3 PlanePos(Vector3 screenPos)
	{
		Vector3 inNormal = base.transform.rotation * dir;
		Plane plane = new Plane(inNormal, base.transform.position);
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		float enter = 0f;
		if (plane.Raycast(ray, out enter))
		{
			return ray.GetPoint(enter);
		}
		return manager.transform.position;
	}
}
