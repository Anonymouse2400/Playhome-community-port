using System;
using UnityEngine;

public class DripTensileStrength : TensileStrength
{
	[SerializeField]
	private float changeRelaxDis = 1f;

	[SerializeField]
	private float changeRelaxDisTime = 3f;

	private float timer;

	private bool relaxChanged;

	protected override void Update()
	{
		base.Update();
		if (!relaxChanged)
		{
			timer += Time.deltaTime;
			if (timer >= changeRelaxDisTime)
			{
				relaxChanged = true;
				relaxDistance = changeRelaxDis;
			}
		}
	}
}
