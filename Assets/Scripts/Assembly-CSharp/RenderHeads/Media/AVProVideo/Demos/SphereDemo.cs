using UnityEngine;
using UnityEngine.VR;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class SphereDemo : MonoBehaviour
	{
		private float _spinX;

		private float _spinY;

		private void Start()
		{
			if (!VRDevice.isPresent && SystemInfo.supportsGyroscope)
			{
				Input.gyro.enabled = true;
				base.transform.parent.Rotate(new Vector3(90f, 0f, 0f));
			}
		}

		private void OnDestroy()
		{
			if (SystemInfo.supportsGyroscope)
			{
				Input.gyro.enabled = false;
			}
		}

		private void Update()
		{
			if (VRDevice.isPresent)
			{
				if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
				{
					InputTracking.Recenter();
				}
				if (Input.GetKeyDown(KeyCode.V))
				{
					VRSettings.enabled = !VRSettings.enabled;
				}
				return;
			}
			if (SystemInfo.supportsGyroscope)
			{
				base.transform.localRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, 0f - Input.gyro.attitude.z, 0f - Input.gyro.attitude.w);
			}
			else if (Input.GetMouseButton(0))
			{
				float value = 40f * (0f - Input.GetAxis("Mouse X")) * Time.deltaTime;
				float value2 = 40f * Input.GetAxis("Mouse Y") * Time.deltaTime;
				value = Mathf.Clamp(value, -0.5f, 0.5f);
				value2 = Mathf.Clamp(value2, -0.5f, 0.5f);
				_spinX += value;
				_spinY += value2;
			}
			if (!Mathf.Approximately(_spinX, 0f) || !Mathf.Approximately(_spinY, 0f))
			{
				base.transform.Rotate(Vector3.up, _spinX);
				base.transform.Rotate(Vector3.right, _spinY);
				_spinX = Mathf.MoveTowards(_spinX, 0f, 5f * Time.deltaTime);
				_spinY = Mathf.MoveTowards(_spinY, 0f, 5f * Time.deltaTime);
			}
		}
	}
}
