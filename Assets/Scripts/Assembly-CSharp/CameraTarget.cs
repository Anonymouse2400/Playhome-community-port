using System;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
	private enum STATE
	{
		NONE = 0,
		MOVE = 1,
		STOP = 2
	}

	public Camera cam;

	public float AlphaInSpeed = 5f;

	public float AlphaOutSpeed = 5f;

	public Vector3 rotOffsetEuler;

	private Quaternion rotOffset;

	private Vector3 pos;

	private Renderer renderer;

	private STATE state;

	private void Awake()
	{
		renderer = GetComponent<Renderer>();
		if (cam == null)
		{
			cam = Camera.main;
		}
		rotOffset = Quaternion.Euler(rotOffsetEuler);
		pos = base.transform.position;
		state = STATE.NONE;
		Color color = renderer.material.color;
		color.a = 0f;
		renderer.material.color = color;
	}

	private void Start()
	{
	}

	public void Move(Vector3 setPos, bool show)
	{
		pos = setPos;
		if (show)
		{
			state = STATE.MOVE;
		}
	}

	private void LateUpdate()
	{
		base.transform.position = pos;
		base.transform.rotation = cam.transform.rotation * rotOffset;
		if (state == STATE.MOVE)
		{
			ChangeAlpha(Mathf.Min(renderer.material.color.a + Time.deltaTime * AlphaInSpeed, 1f));
			state = STATE.STOP;
		}
		else if (state == STATE.STOP)
		{
			ChangeAlpha(Mathf.Max(renderer.material.color.a - Time.deltaTime * AlphaInSpeed, 0f));
			if (renderer.material.color.a <= 0f)
			{
				state = STATE.NONE;
			}
		}
	}

	private void ChangeAlpha(float alpha)
	{
		Color color = renderer.material.color;
		color.a = alpha;
		renderer.material.color = color;
	}
}
