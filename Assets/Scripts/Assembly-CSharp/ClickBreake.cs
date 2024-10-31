using System;
using UnityEngine;

public class ClickBreake : MonoBehaviour
{
	private Rigidbody[] rigids;

	[SerializeField]
	private Transform explosionTrans;

	[SerializeField]
	private float explosionForce = 1f;

	[SerializeField]
	private float explosionRadius = 1f;

	private void Start()
	{
		rigids = GetComponentsInChildren<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (explosionTrans == null)
			{
				explosionTrans = base.transform;
			}
			for (int i = 0; i < rigids.Length; i++)
			{
				rigids[i].isKinematic = false;
				rigids[i].AddExplosionForce(explosionForce, explosionTrans.position, explosionRadius);
			}
		}
	}
}
