using System;
using UnityEngine;
using Utility;

public class FPS_Inertia : MonoBehaviour
{
	[SerializeField]
	private float mouseSensitivity = 1f;

	[SerializeField]
	private float accel = 10f;

	[SerializeField]
	private float brake = 10f;

	[SerializeField]
	private float min = 0.01f;

	private float yaw;

	private float pitch;

	private float yawInertia;

	private float pitchInertia;

	private void Start()
	{
		yaw = base.transform.localRotation.eulerAngles.y;
		pitch = base.transform.localRotation.eulerAngles.x;
	}

	private void LateUpdate()
	{
		float num = Input.GetAxis("Mouse X") * mouseSensitivity;
		float num2 = Input.GetAxis("Mouse Y") * mouseSensitivity;
		float k = ((!(Mathf.Abs(num) > Mathf.Abs(yawInertia))) ? brake : accel);
		float k2 = ((!(Mathf.Abs(num2) > Mathf.Abs(pitchInertia))) ? brake : accel);
		yawInertia = Tween.Spring(yawInertia, num, k, Time.deltaTime, min);
		pitchInertia = Tween.Spring(pitchInertia, num2, k2, Time.deltaTime, min);
		yaw += yawInertia;
		pitch -= pitchInertia;
		base.transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
	}
}
