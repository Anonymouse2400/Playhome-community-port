using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvSound : MonoBehaviour
{
	private AudioSource audio;

	private bool isADV;

	private void Awake()
	{
		isADV = SceneManager.GetActiveScene().name.IndexOf("ADV") == 0;
		audio = GetComponent<AudioSource>();
		Volume();
	}

	private void Update()
	{
		Volume();
	}

	private void Volume()
	{
		audio.volume = ((!isADV) ? ConfigData.VolumeEnv() : 0f);
	}
}
