using System;
using UnityEngine;

public class BGM_Control
{
	private enum COMMAND
	{
		NONE = 0,
		PLAY = 1,
		PAUSE = 2,
		STOP = 3,
		CHANGE = 4
	}

	private AudioSource source;

	private float fadeVol;

	private AudioClip nextClip;

	private COMMAND nowCommand = COMMAND.STOP;

	private COMMAND goalCommand = COMMAND.STOP;

	private float fadeSpeed = 1f;

	public bool IsPlaying
	{
		get
		{
			return nowCommand == COMMAND.PLAY;
		}
	}

	public BGM_Control(AudioSource source)
	{
		this.source = source;
		fadeVol = 0f;
		nextClip = null;
		nowCommand = COMMAND.STOP;
		goalCommand = COMMAND.STOP;
		fadeSpeed = 1f;
	}

	public void Update()
	{
		if ((nowCommand != goalCommand || nextClip != null) && fadeVol <= 0f)
		{
			if (nextClip != null)
			{
				ChangeClip(nextClip);
				nextClip = null;
			}
			if (goalCommand == COMMAND.PLAY)
			{
				Play();
				nowCommand = goalCommand;
			}
			else if (goalCommand == COMMAND.STOP)
			{
				Stop();
				nowCommand = goalCommand;
			}
			else if (goalCommand == COMMAND.PAUSE)
			{
				Pause();
				nowCommand = goalCommand;
			}
		}
		UpdateVolume();
	}

	private void UpdateVolume()
	{
		float goalVol = fadeVol;
		GoalVol(ref goalVol);
		if (fadeVol != goalVol)
		{
			if (fadeSpeed > 0f)
			{
				float value = goalVol - fadeVol;
				float num = fadeSpeed * Time.deltaTime;
				fadeVol += Mathf.Clamp(value, 0f - num, num);
			}
			else
			{
				fadeVol = goalVol;
			}
		}
		source.volume = Volume(fadeVol);
	}

	private void GoalVol(ref float goalVol)
	{
		if (nextClip != null)
		{
			goalVol = 0f;
		}
		else
		{
			goalVol = ((goalCommand != COMMAND.PLAY) ? 0f : 1f);
		}
	}

	public void Play(float speed)
	{
		fadeSpeed = speed;
		goalCommand = COMMAND.PLAY;
	}

	public void Pause(float speed)
	{
		fadeSpeed = speed;
		goalCommand = COMMAND.PAUSE;
	}

	public void Stop(float speed)
	{
		fadeSpeed = speed;
		goalCommand = COMMAND.STOP;
	}

	public void ChangeClip(AudioClip clip, float speed)
	{
		fadeSpeed = speed;
		nextClip = clip;
	}

	private void Play()
	{
		if (source.clip == null)
		{
			Debug.LogWarning("クリップがないのに再生しようとしました");
		}
		source.Play();
	}

	private void Pause()
	{
		source.Pause();
	}

	private void Stop()
	{
		source.Stop();
	}

	private void ChangeClip(AudioClip clip)
	{
		bool isPlaying = source.isPlaying;
		source.clip = clip;
		if (isPlaying)
		{
			Play();
		}
	}

	private float Volume(float vol)
	{
		return ConfigData.VolumeBGM() * vol;
	}
}
