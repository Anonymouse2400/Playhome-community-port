using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPS_Counter : MonoBehaviour
{
	public float UpdateTime = 1f;

	public string fpsText = "FPS";

	private float timer;

	private float tmp;

	private float nowFPS;

	private float counter;

	private Text text;

	private void Start()
	{
		timer = 0f;
		tmp = 0f;
		nowFPS = 0f;
		text = GetComponent<Text>();
	}

	private void Update()
	{
		if (timer >= UpdateTime)
		{
			nowFPS = counter / timer;
			timer = 0f;
			counter = 0f;
		}
		timer += Time.deltaTime;
		counter += 1f;
		text.text = nowFPS.ToString("000") + fpsText;
	}
}
