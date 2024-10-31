using System;
using UnityEngine;

public class RigidEmitter : MonoBehaviour
{
	public GameObject[] original;

	[Range(-180f, 0f)]
	public float directionMin;

	[Range(0f, 180f)]
	public float directionMax;

	[Range(-180f, 0f)]
	public float elevationMin;

	[Range(0f, 180f)]
	public float elevationMax;

	public float speedMin = 1f;

	public float speedMax = 1f;

	public float emitTime = 1f;

	public int emitNum = 1;

	public Transform setParent;

	public KeyCode key = KeyCode.Space;

	private float timer;

	private int emitCount;

	private bool emitting;

	public bool Emitting
	{
		get
		{
			return emitting;
		}
		set
		{
			emitting = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(key))
		{
			StartEmit();
		}
		if (Emitting)
		{
			UpdateEmit();
		}
	}

	public void StartEmit()
	{
		Emitting = true;
		timer = 0f;
		emitCount = 0;
	}

	private void UpdateEmit()
	{
		float num = emitTime / ((float)emitNum - 1f);
		if (timer == 0f)
		{
			Emit();
		}
		else
		{
			int num2 = (int)(timer / num) + 1;
			while (emitCount < num2)
			{
				Emit();
			}
		}
		timer += Time.deltaTime;
		if (emitCount >= emitNum)
		{
			Emitting = false;
		}
	}

	private void Emit()
	{
		int num = UnityEngine.Random.Range(0, original.Length);
		GameObject gameObject = UnityEngine.Object.Instantiate(original[num]);
		gameObject.SetActive(true);
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = base.transform.rotation;
		gameObject.transform.SetParent(setParent);
		Rigidbody[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody>();
		Rigidbody[] array = componentsInChildren;
		foreach (Rigidbody rigidbody in array)
		{
			float y = UnityEngine.Random.Range(directionMin, directionMax);
			float x = UnityEngine.Random.Range(elevationMin, elevationMax);
			float num2 = UnityEngine.Random.Range(speedMin, speedMax);
			Quaternion quaternion = base.transform.rotation * Quaternion.Euler(x, y, 0f);
			Vector3 velocity = quaternion * Vector3.forward * num2;
			rigidbody.velocity = velocity;
		}
		emitCount++;
	}
}
