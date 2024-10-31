using System;
using UnityEngine;

public class MortalObj : MonoBehaviour
{
	public float spanOfLife = 1f;

	public bool rememberChilds = true;

	private float timer;

	private void Start()
	{
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > spanOfLife)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
