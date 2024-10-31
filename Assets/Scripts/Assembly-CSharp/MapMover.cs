using System;
using UnityEngine;

public class MapMover : MonoBehaviour
{
	[SerializeField]
	private Camera cam;

	[SerializeField]
	private float hipHeight = 0.1f;

	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private GameObject footPos;

	[SerializeField]
	private float walkSpeed = 1.4f;

	[SerializeField]
	private float sprintSpeed = 7f;

	[SerializeField]
	private float moveAccel = 1f;

	private float nowSpeed;

	private Vector3 moveVec;

	[SerializeField]
	private float gravity = 0.98f;

	private float fallAccel;

	[SerializeField]
	private float maxFallSpeed = 55.5f;

	private void Start()
	{
	}

	private void Update()
	{
		float y = cam.transform.rotation.eulerAngles.y;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		Vector3 zero = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
		{
			zero += Vector3.forward;
		}
		if (Input.GetKey(KeyCode.S))
		{
			zero += Vector3.back;
		}
		if (Input.GetKey(KeyCode.A))
		{
			zero += Vector3.left;
		}
		if (Input.GetKey(KeyCode.D))
		{
			zero += Vector3.right;
		}
		if (zero.magnitude > 0f)
		{
			moveVec = zero.normalized;
			nowSpeed = Accel(walkSpeed, nowSpeed, moveAccel);
		}
		else
		{
			nowSpeed = Accel(0f, nowSpeed, moveAccel);
		}
		zero = quaternion * moveVec * (nowSpeed * Time.deltaTime);
		base.transform.position = base.transform.position + zero;
		Ray ray = new Ray(base.transform.position, Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, hipHeight * 2f, layerMask))
		{
			fallAccel = 0f;
			base.transform.position = hitInfo.point + Vector3.up * hipHeight;
			return;
		}
		fallAccel += 0.98f * Time.deltaTime;
		if (fallAccel >= maxFallSpeed)
		{
			fallAccel = maxFallSpeed;
		}
		if (Physics.Raycast(ray, out hitInfo, fallAccel, layerMask))
		{
			base.transform.position = hitInfo.point + Vector3.up * hipHeight;
		}
		else
		{
			base.transform.position = base.transform.position + Vector3.down * fallAccel;
		}
	}

	private static float Accel(float goal, float now, float accel)
	{
		if (now < goal)
		{
			now += accel * Time.deltaTime;
			if (now > goal)
			{
				now = goal;
			}
		}
		else if (now > goal)
		{
			now -= accel * Time.deltaTime;
			if (now < goal)
			{
				now = goal;
			}
		}
		return now;
	}
}
