using System;
using UnityEngine;

public class CharaMoveController : MonoBehaviour
{
	protected enum TARGET
	{
		MAIN = 0,
		VISITOR = 1
	}

	[SerializeField]
	protected Camera camera;

	[SerializeField]
	protected InputSliderUI speedSlider;

	[SerializeField]
	protected float minSpeed = 0.1f;

	[SerializeField]
	protected float maxSpeed = 10f;

	protected float speedRate = 0.5f;

	[SerializeField]
	protected float speedMul = 0.025f;

	protected bool invoke = true;

	protected Transform moveTrans;

	protected Vector3 defPos;

	protected Quaternion defRot;

	protected virtual void Awake()
	{
		speedSlider.AddOnChangeAction(OnSpeedSlider);
	}

	public void Setup(Transform trans)
	{
		moveTrans = trans;
		invoke = false;
		speedSlider.SetValue(speedRate);
		invoke = true;
	}

	public void SetDef(Vector3 pos, Quaternion rot)
	{
		defPos = pos;
		defRot = rot;
	}

	protected void OnSpeedSlider(float val)
	{
		if (invoke)
		{
			speedRate = val;
		}
	}

	public void ResetMoveXZ()
	{
		Vector3 position = moveTrans.position;
		position.x = defPos.x;
		position.z = defPos.z;
		moveTrans.position = position;
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	public void ResetMoveY()
	{
		Vector3 position = moveTrans.position;
		position.y = defPos.y;
		moveTrans.position = position;
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	public void ResetYaw()
	{
		Vector3 eulerAngles = moveTrans.rotation.eulerAngles;
		eulerAngles.y = defRot.eulerAngles.y;
		moveTrans.rotation = Quaternion.Euler(eulerAngles);
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	public void ResetTrans()
	{
		moveTrans.position = defPos;
		moveTrans.rotation = defRot;
	}

	public void MoveXZ(Vector2 move)
	{
		Vector3 vector = new Vector3(move.x, 0f, move.y);
		vector *= Speed();
		vector = camera.transform.TransformVector(vector);
		vector.y = 0f;
		moveTrans.position += vector;
	}

	public void MoveY(Vector2 move)
	{
		Vector3 vector = new Vector3(0f, move.y, 0f);
		vector *= Speed();
		moveTrans.position += vector;
	}

	public void RotateYaw(Vector2 move)
	{
		float num = move.x * Speed() * 180f;
		Vector3 eulerAngles = moveTrans.rotation.eulerAngles;
		eulerAngles.y += num;
		moveTrans.rotation = Quaternion.Euler(eulerAngles);
	}

	protected float Speed()
	{
		return Mathf.Lerp(minSpeed, maxSpeed, speedRate) * speedMul;
	}
}
