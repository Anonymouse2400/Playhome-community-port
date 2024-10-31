using System;
using UnityEngine;

internal class AudioVolumeAnalyzer
{
	private AudioSource audio;

	private int frequency;

	private int channels;

	private float[] buf;

	public bool HasAudio
	{
		get
		{
			return audio != null;
		}
	}

	public float AnalyzeSound(float checkTime = 0.1f)
	{
		if (audio == null || !audio.isPlaying)
		{
			return 0f;
		}
		int timeSamples = audio.timeSamples;
		if (timeSamples == 0)
		{
			return 0f;
		}
		CheckBuffer(checkTime);
		int num = audio.clip.samples * audio.clip.channels;
		int num2 = num - timeSamples * audio.clip.channels;
		if (audio.clip.GetData(buf, timeSamples))
		{
			float num3 = 0f;
			for (int i = 0; i < buf.Length && (audio.loop || i < num2); i++)
			{
				num3 += Mathf.Abs(buf[i]);
			}
			num3 /= (float)buf.Length;
			return Mathf.Abs(num3);
		}
		return 0f;
	}

	public void SetAudio(AudioSource audio)
	{
		this.audio = audio;
	}

	private void CheckBuffer(float checkTime)
	{
		int num = (int)((float)audio.clip.frequency * checkTime);
		if (num <= 0)
		{
			num = 1;
		}
		int num2 = audio.clip.channels * num;
		if (buf == null || buf.Length != num2)
		{
			CreateBuffer(num2);
		}
	}

	private void CreateBuffer(int bufSize)
	{
		buf = new float[bufSize];
	}
}
