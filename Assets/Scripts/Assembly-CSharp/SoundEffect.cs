using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
internal class SoundEffect : MonoBehaviour
{
	public enum TYPE
	{
		TYPE_SE = 0,
		TYPE_SYSTEM = 1
	}

	private AudioSource audio;

	public bool finishSuside;

	public TYPE type;

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		audio.volume = ((type != 0) ? ConfigData.VolumeSyetem() : ConfigData.VolumeSoundEffect());
		if (finishSuside && !audio.isPlaying)
		{
            UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Play()
	{
		audio.volume = ((type != 0) ? ConfigData.VolumeSyetem() : ConfigData.VolumeSoundEffect());
		audio.Play();
	}

	public void Play(AudioClip clip)
	{
		audio.clip = clip;
		audio.volume = ((type != 0) ? ConfigData.VolumeSyetem() : ConfigData.VolumeSoundEffect());
		audio.Play();
	}
}
