using System;
using UnityEngine;

public class MoveUITEST : MonoBehaviour
{
	public GuidesManager manager;

	private void Start()
	{
		manager.Add(base.transform);
	}

	private void Update()
	{
	}
}
