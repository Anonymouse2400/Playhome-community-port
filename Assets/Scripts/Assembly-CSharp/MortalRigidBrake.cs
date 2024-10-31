using System;
using UnityEngine;

public class MortalRigidBrake : MonoBehaviour
{
	public float spanOfLife = 1f;

	public bool rememberChilds = true;

	private float timer;

	private RigidBrake[] childs;

	private void Start()
	{
		if (rememberChilds)
		{
			childs = base.gameObject.GetComponentsInChildren<RigidBrake>(true);
		}
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (!(timer > spanOfLife))
		{
			return;
		}
		if (childs != null)
		{
			for (int i = 0; i < childs.Length; i++)
			{
				if (childs[i] != base.gameObject)
				{
					childs[i].transform.SetParent(base.transform);
				}
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
