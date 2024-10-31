using System;
using UnityEngine;

public class RigidToDynamic : MonoBehaviour
{
	private Rigidbody rig;

	public float unlockTime = 1f;

	private float timer;

	private void Start()
	{
		rig = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (rig.isKinematic)
		{
			timer += Time.deltaTime;
			if (timer >= unlockTime)
			{
				rig.isKinematic = false;
			}
		}
	}
}
