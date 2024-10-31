using System;
using UnityEngine;

[RequireComponent(typeof(Human))]
public class DebugHeadPos : MonoBehaviour
{
	public Human human;

	private void Awake()
	{
		human = GetComponent<Human>();
	}
}
