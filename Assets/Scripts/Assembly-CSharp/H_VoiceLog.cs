using System;
using System.Collections.Generic;
using UnityEngine;

public class H_VoiceLog
{
	private class LogData
	{
		public float timer;

		public string file;

		public LogData(string file, float timer)
		{
			this.file = file;
			this.timer = timer;
		}

		public bool Update(float time)
		{
			timer -= time;
			return timer > 0f;
		}
	}

	private const float talkForgetTime = 300f;

	private const int pantLogNum = 5;

	private List<LogData> priorityLogs = new List<LogData>();

	private Dictionary<string, LogData> priorityLogsDic = new Dictionary<string, LogData>();

	private string[] pantLogs = new string[5];

	public void Clear()
	{
		priorityLogs.Clear();
		priorityLogsDic.Clear();
		for (int i = 0; i < pantLogs.Length; i++)
		{
			pantLogs[i] = string.Empty;
		}
	}

	public void AddPriorityTalk(string file)
	{
		if (!priorityLogsDic.ContainsKey(file))
		{
			LogData logData = new LogData(file, 300f);
			priorityLogs.Add(logData);
			priorityLogsDic.Add(file, logData);
		}
	}

	public void AddPant(string file)
	{
		for (int num = pantLogs.Length - 1; num > 0; num--)
		{
			pantLogs[num] = pantLogs[num - 1];
		}
		pantLogs[0] = file;
	}

	public void Update()
	{
		for (int i = 0; i < priorityLogs.Count; i++)
		{
			if (!priorityLogs[i].Update(Time.deltaTime))
			{
				priorityLogs.RemoveAt(i);
				i--;
			}
		}
	}

	public void Check(List<H_Voice.Data> hitDatas)
	{
		for (int i = 0; i < hitDatas.Count; i++)
		{
			if (hitDatas.Count <= 1)
			{
				break;
			}
			if (priorityLogsDic.ContainsKey(hitDatas[i].File))
			{
				hitDatas.RemoveAt(i);
				i--;
			}
		}
		for (int j = 0; j < pantLogs.Length; j++)
		{
			for (int k = 0; k < hitDatas.Count; k++)
			{
				if (hitDatas.Count <= 1)
				{
					break;
				}
				if (hitDatas[k].File == pantLogs[j])
				{
					hitDatas.RemoveAt(k);
					k--;
				}
			}
		}
	}

	public void Check(List<H_VisitorVoice.Data> hitDatas)
	{
		for (int i = 0; i < hitDatas.Count; i++)
		{
			if (hitDatas.Count <= 1)
			{
				break;
			}
			if (priorityLogsDic.ContainsKey(hitDatas[i].file))
			{
				hitDatas.RemoveAt(i);
				i--;
			}
		}
		for (int j = 0; j < pantLogs.Length; j++)
		{
			for (int k = 0; k < hitDatas.Count; k++)
			{
				if (hitDatas.Count <= 1)
				{
					break;
				}
				if (hitDatas[k].file == pantLogs[j])
				{
					hitDatas.RemoveAt(k);
					k--;
				}
			}
		}
	}
}
