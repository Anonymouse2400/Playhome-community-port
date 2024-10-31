using System;
using UnityEngine;

public class RigidEmitter_OneShot : MonoBehaviour
{
	public GameObject original;

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

	public int emitNum = 1;

	public Transform setParent;

	private float timer;

	public float interval = 0.05f;

	private float intervalTimer;

	public KeyCode key;

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
		Emitting = Input.GetKey(key);
		if (Emitting)
		{
			intervalTimer -= Time.deltaTime;
			if (intervalTimer <= 0f)
			{
				Emit();
			}
		}
	}

	public void EmitStart()
	{
	}

	public void Emit()
	{
		for (int i = 0; i < emitNum; i++)
		{
			float y = UnityEngine.Random.Range(directionMin, directionMax);
			float x = UnityEngine.Random.Range(elevationMin, elevationMax);
			float num = UnityEngine.Random.Range(speedMin, speedMax);
			Quaternion quaternion = base.transform.rotation * Quaternion.Euler(x, y, 0f);
			Vector3 velocity = quaternion * Vector3.forward * num;
			GameObject gameObject = UnityEngine.Object.Instantiate(original);
			gameObject.SetActive(true);
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			gameObject.transform.SetParent(setParent);
			Rigidbody[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody>();
			Rigidbody[] array = componentsInChildren;
			foreach (Rigidbody rigidbody in array)
			{
				rigidbody.velocity = velocity;
			}
		}
		intervalTimer = interval;
	}
}
