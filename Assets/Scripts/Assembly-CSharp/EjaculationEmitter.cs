using System;
using UnityEngine;

public class EjaculationEmitter : MonoBehaviour
{
	public GameObject[] original;

	public Vector3 minOffset;

	public Vector3 maxOffset;

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

	private void Start()
	{
	}

	private void Update()
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
			int num2 = UnityEngine.Random.Range(0, original.Length);
			GameObject gameObject = UnityEngine.Object.Instantiate(original[num2]);
			gameObject.SetActive(true);
			Vector3 vector = new Vector3(UnityEngine.Random.Range(minOffset.x, maxOffset.x), UnityEngine.Random.Range(minOffset.y, maxOffset.y), UnityEngine.Random.Range(minOffset.z, maxOffset.z));
			vector = base.transform.rotation * vector;
			gameObject.transform.position = base.transform.position + vector;
			gameObject.transform.rotation = base.transform.rotation;
			gameObject.transform.SetParent(setParent);
			Rigidbody[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody>();
			Rigidbody[] array = componentsInChildren;
			foreach (Rigidbody rigidbody in array)
			{
				rigidbody.velocity = velocity;
			}
		}
	}
}
