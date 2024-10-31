using System;
using UnityEngine;

public class CameraRig_Orbit : MonoBehaviour
{
	[SerializeField]
	private float rotateSpeed = 5f;

	[SerializeField]
	private float moveSpeed = 0.1f;

	[SerializeField]
	private float zoomSpeed = 50f;

	[SerializeField]
	private float rollSpeed = 50f;

	[SerializeField]
	private bool invX;

	[SerializeField]
	private bool invY;

	[SerializeField]
	private bool invZ;

	[SerializeField]
	private bool invR;

	private Vector3 euler;

	private Camera cam;

	private float distance = 1f;

	private void Start()
	{
		cam = base.gameObject.GetComponentInChildren<Camera>();
		euler = base.transform.rotation.eulerAngles;
		distance = cam.transform.localPosition.magnitude;
	}

	private void Update()
	{
		if (Input.GetMouseButton(1) && Input.GetMouseButton(2))
		{
			euler.z = 0f;
		}
		if (Input.GetMouseButton(0) && Input.GetMouseButton(2))
		{
			cam.fieldOfView = 45f;
		}
		if (Input.GetMouseButton(2) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
		{
			Vector3 vector = default(Vector3);
			vector.x += Input.GetAxis("Mouse X") * moveSpeed;
			vector.z += Input.GetAxis("Mouse Y") * moveSpeed;
			vector = base.transform.rotation * vector;
			vector.y = 0f;
			base.transform.position += vector;
		}
		else if (Input.GetMouseButton(1))
		{
			float num = 0f;
			num = Input.GetAxis("Mouse X") * moveSpeed;
			distance = Mathf.Max(0f, distance + num);
			Vector3 vector2 = default(Vector3);
			vector2.y += Input.GetAxis("Mouse Y") * moveSpeed;
			base.transform.position += vector2;
			euler.z += Input.GetAxis("Mouse ScrollWheel") * rollSpeed * (float)((!invR) ? 1 : (-1));
		}
		else if (Input.GetMouseButton(0))
		{
			euler.y += Input.GetAxis("Mouse X") * rotateSpeed * (float)((!invX) ? 1 : (-1));
			euler.x -= Input.GetAxis("Mouse Y") * rotateSpeed * (float)((!invY) ? 1 : (-1));
		}
		else
		{
			cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * (float)((!invZ) ? 1 : (-1));
			cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 10f, 90f);
		}
		base.transform.rotation = Quaternion.Euler(euler);
		cam.transform.localPosition = Vector3.back * distance;
	}
}
