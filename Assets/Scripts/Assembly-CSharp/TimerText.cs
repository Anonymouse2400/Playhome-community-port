using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimerText : MonoBehaviour
{
	private float timer;

	[SerializeField]
	private string format = "0.0";

	private Text text;

	private void Start()
	{
		text = GetComponent<Text>();
		timer = 0f;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		text.text = timer.ToString(format);
	}
}
