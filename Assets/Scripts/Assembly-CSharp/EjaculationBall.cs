using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EjaculationBall : MonoBehaviour
{
	public float spanOfLife = 1f;

	private float lifeTimer;

	[Range(0f, 1f)]
	public float fadeStart = 0.75f;

	private Rigidbody rig;

	public float enterBrake = 20f;

	public float addBrake = 20f;

	public float lockTime = 1f;

	private bool hitted;

	[SerializeField]
	private LineRenderer mucusLine;

	private List<GameObject> mucusObjs = new List<GameObject>();

	public float minDis = 0.01f;

	public float maxLen = 1f;

	private Collider lastHitCollider;

	private void Start()
	{
		rig = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Update_Life();
		Update_LockTime();
		Update_Hitted();
		Update_Line();
	}

	private void Update_Life()
	{
		float a = 1f;
		float num = lifeTimer / spanOfLife;
		if (num >= fadeStart)
		{
			a = 1f - Mathf.InverseLerp(fadeStart, 1f, num);
		}
		Color color = mucusLine.material.color;
		color.a = a;
		mucusLine.material.color = color;
		lifeTimer += Time.deltaTime;
		if (lifeTimer > spanOfLife)
		{
			for (int i = 0; i < mucusObjs.Count; i++)
			{
				mucusObjs[i].transform.SetParent(base.transform);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update_LockTime()
	{
		if (lockTime > 0f)
		{
			lockTime -= Time.deltaTime;
			if (lockTime <= 0f)
			{
				rig.isKinematic = true;
			}
		}
	}

	private void Update_Hitted()
	{
		if (hitted)
		{
			rig.drag += addBrake * Time.deltaTime;
			rig.angularDrag += addBrake * Time.deltaTime;
		}
	}

	private void Update_Line()
	{
		if (lastHitCollider != null && !AddMucusPos(lastHitCollider))
		{
			MucusLength(base.transform.position, mucusObjs, maxLen);
			if (mucusLine.numPositions != mucusObjs.Count + 1)
			{
				mucusLine.numPositions = mucusObjs.Count + 1;
			}
			mucusLine.SetPosition(0, base.transform.position);
			for (int i = 0; i < mucusObjs.Count; i++)
			{
				mucusLine.SetPosition(i + 1, mucusObjs[i].transform.position);
			}
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		hitted = true;
		lastHitCollider = col.collider;
		AddMucusPos(col.collider);
		rig.drag += enterBrake;
		rig.angularDrag += enterBrake;
	}

	private void OnCollisionStay(Collision col)
	{
		lastHitCollider = col.collider;
	}

	private bool AddMucusPos(Collider collider)
	{
		Vector3 position = base.transform.position;
		if (mucusObjs.Count >= 2)
		{
			Vector3 position2 = mucusObjs[1].transform.position;
			if (Vector3.Distance(position, position2) < minDis)
			{
				return false;
			}
		}
		GameObject gameObject = new GameObject("MucusLinePos");
		mucusObjs.Insert(0, gameObject);
		gameObject.transform.SetParent(collider.transform);
		gameObject.transform.position = base.transform.position;
		Vector3[] array = new Vector3[mucusObjs.Count + 1];
		array[0] = base.transform.position;
		for (int i = 0; i < mucusObjs.Count; i++)
		{
			array[i + 1] = mucusObjs[i].transform.position;
		}
		array = MucusLength(array, maxLen);
		mucusLine.numPositions = array.Length;
		mucusLine.SetPositions(array);
		return true;
	}

	private static Vector3[] MucusLength(Vector3[] positions, float maxLen)
	{
		if (positions.Length < 2)
		{
			return positions;
		}
		Vector3 a = positions[0];
		float num = 0f;
		int num2 = -1;
		for (int i = 1; i < positions.Length; i++)
		{
			float num3 = Vector3.Distance(a, positions[i]);
			num += num3;
			a = positions[i];
			if (num > maxLen)
			{
				num2 = i + 1;
				float num4 = num - maxLen;
				Vector3 vector = positions[i - 1] - positions[i];
				vector.Normalize();
				vector *= num4;
				positions[i] += vector;
				break;
			}
		}
		if (num2 == -1)
		{
			return positions;
		}
		Vector3[] array = new Vector3[num2];
		Array.Copy(positions, array, num2);
		return array;
	}

	private static void MucusLength(Vector3 top, List<GameObject> objs, float maxLen)
	{
		if (objs.Count < 1)
		{
			return;
		}
		Vector3 vector = top;
		float num = 0f;
		int num2 = -1;
		for (int i = 0; i < objs.Count; i++)
		{
			Vector3 position = objs[i].transform.position;
			float num3 = Vector3.Distance(vector, position);
			num += num3;
			if (num > maxLen)
			{
				num2 = i + 1;
				float num4 = num - maxLen;
				Vector3 vector2 = vector - position;
				vector2.Normalize();
				vector2 *= num4;
				objs[i].transform.position = position + vector2;
				break;
			}
			vector = position;
		}
		if (num2 != -1)
		{
			while (objs.Count > num2)
			{
				UnityEngine.Object.Destroy(objs[num2]);
				objs.RemoveAt(num2);
			}
		}
	}
}
