using System;
using UnityEngine;

public class RigidBrake : MonoBehaviour
{
	private Rigidbody rig;

	public float brake = 0.1f;

	public bool stayBrake;

	public float lockTime = 1f;

	public float hitDrag = 10f;

	public float hitAngularDrag = 10f;

	private float timer;

	private bool hitted;

	private float normalDrag;

	private float normalAngularDrag;

	private bool hitNow;

	private void Start()
	{
		rig = GetComponent<Rigidbody>();
		normalDrag = rig.drag;
		normalAngularDrag = rig.angularDrag;
	}

	private void Update()
	{
		if (hitted && timer < lockTime)
		{
			timer += Time.deltaTime;
		}
		if (stayBrake)
		{
			if (hitNow)
			{
				hitNow = false;
				return;
			}
			rig.drag = normalDrag;
			rig.angularDrag = normalAngularDrag;
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		rig.velocity *= brake;
		rig.drag = hitDrag;
		rig.angularDrag = hitAngularDrag;
		hitted = true;
		hitNow = true;
	}

	private void OnCollisionStay(Collision col)
	{
		hitNow = true;
		if (stayBrake)
		{
			rig.velocity *= brake;
			rig.drag = hitDrag;
			rig.angularDrag = hitAngularDrag;
		}
		if (timer >= lockTime)
		{
			rig.isKinematic = true;
			base.transform.SetParent(col.collider.transform);
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				collider.enabled = false;
			}
		}
	}
}
