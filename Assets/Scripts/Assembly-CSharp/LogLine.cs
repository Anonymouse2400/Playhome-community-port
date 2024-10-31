using System;
using System.Collections.Generic;
using UnityEngine;

public class LogLine : MonoBehaviour
{
	[SerializeField]
	private LineRenderer line;

	[SerializeField]
	private float maxTime = 3f;

	[SerializeField]
	private float valueRate = 1f;

	[SerializeField]
	private Vector3 start = new Vector3(-0.5f, 0f, 0f);

	[SerializeField]
	private Vector3 end = new Vector3(0.5f, 0f, 0f);

	[SerializeField]
	private Vector3 up = new Vector3(0f, 0.5f, 0f);

	private List<LogLineData> datas = new List<LogLineData>();

	private void Start()
	{
		Add(0f);
		Add(0f);
	}

	private void Update()
	{
		int num = 0;
		while (num < datas.Count)
		{
			if (datas[num].Update(maxTime))
			{
				datas.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
		Vector3[] array = new Vector3[datas.Count];
		for (num = 0; num < datas.Count; num++)
		{
			float t = Mathf.InverseLerp(0f, maxTime, datas[num].timer);
			Vector3 vector = Vector3.Lerp(start, end, t);
			float num2 = datas[num].value * valueRate;
			vector += up * num2;
			array[num] = vector;
		}
		line.SetVertexCount(array.Length);
		line.SetPositions(array);
	}

	public void Add(float value)
	{
		datas.Add(new LogLineData(value));
	}
}
