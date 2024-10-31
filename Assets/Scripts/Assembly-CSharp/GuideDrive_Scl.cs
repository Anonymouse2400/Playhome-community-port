using System;
using UnityEngine;

public class GuideDrive_Scl : GuideDrive
{
	public enum MoveType
	{
		X = 1,
		Y = 2,
		Z = 4,
		XYZ = 7
	}

	[SerializeField]
	private MoveType moveType;

	[SerializeField]
	private float speed = 0.001f;

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
		Vector3 vector = Input.mousePosition - movePrevPos;
		movePrevPos = Input.mousePosition;
		if (moveType == MoveType.XYZ)
		{
			float num = (vector.x + vector.y) * speed;
			vector = new Vector3(num, num, num);
			manager.Target.transform.localScale += vector;
		}
		else
		{
			vector = Camera.main.transform.rotation * vector * speed;
			if ((moveType & MoveType.X) == 0)
			{
				vector.x = 0f;
			}
			if ((moveType & MoveType.Y) == 0)
			{
				vector.y = 0f;
			}
			if ((moveType & MoveType.Z) == 0)
			{
				vector.z = 0f;
			}
			manager.DriveMoveScale(vector);
		}
		if (Input.GetMouseButtonUp(0))
		{
			OnMoveEnd();
			manager.OnMoveEnd(this);
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
}
