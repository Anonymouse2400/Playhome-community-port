using System;
using UnityEngine;

public class LookAtRotatorSortManager : MonoBehaviour
{
	public LookAtRotator[] rotators;

	private void Awake()
	{
		LookAtRotator[] array = rotators;
		foreach (LookAtRotator lookAtRotator in array)
		{
			lookAtRotator.enabled = false;
			lookAtRotator.Init();
		}
	}

	private void LateUpdate()
	{
		LookAtRotator[] array = rotators;
		foreach (LookAtRotator lookAtRotator in array)
		{
			lookAtRotator.Calc();
		}
	}
}
