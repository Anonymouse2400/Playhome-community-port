using System;
using UnityEngine;

public class H_Item : MonoBehaviour
{
	private Transform target;

	public Animator animator { get; private set; }

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	private void LateUpdate()
	{
		if (target != null)
		{
			base.transform.position = target.position;
			base.transform.rotation = target.rotation;
		}
	}
}
