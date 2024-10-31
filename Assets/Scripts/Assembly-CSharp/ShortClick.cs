using System;
using UnityEngine;

internal class ShortClick
{
	private bool clicking;

	private float timer;

	private float timeLimit = 0.3f;

	private float moveLimit = 10f;

	private float move;

	public bool IsShortClickNow { get; private set; }

	public void Update()
	{
		IsShortClickNow = false;
		if (Input.GetMouseButtonDown(0) && !GameCursor.OnUI && GameCursor.IsDraw)
		{
			Down();
		}
		if (Input.GetMouseButtonUp(0))
		{
			Up();
		}
		if (clicking)
		{
			move += GameCursor.move.magnitude;
			timer += Time.deltaTime;
		}
	}

	private void Down()
	{
		timer = 0f;
		move = 0f;
		clicking = true;
	}

	private void Up()
	{
		if (clicking && timer <= timeLimit && move <= moveLimit)
		{
			IsShortClickNow = true;
		}
		clicking = false;
	}
}
