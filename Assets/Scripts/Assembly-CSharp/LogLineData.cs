using UnityEngine;

internal class LogLineData
{
	public float timer { get; private set; }

	public float value { get; private set; }

	public LogLineData(float value)
	{
		this.value = value;
		timer = 0f;
	}

	public bool Update(float checkTime)
	{
		timer += Time.deltaTime;
		return timer >= checkTime;
	}
}
