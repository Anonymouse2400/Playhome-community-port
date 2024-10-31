using System;
using UnityEngine;

public class AnimePlayTest : MonoBehaviour
{
	public Animator[] anms;

	public string[] clipNames;

	public float changeSpeed;

	public AnimeParam_Float[] floats;

	private void Start()
	{
	}

	private void Update()
	{
		if (anms == null || floats == null)
		{
			return;
		}
		Animator[] array = anms;
		foreach (Animator anm in array)
		{
			AnimeParam_Float[] array2 = floats;
			foreach (AnimeParam_Float animeParam_Float in array2)
			{
				animeParam_Float.Set(anm);
			}
		}
	}

	public void Play(string anime)
	{
		Animator[] array = anms;
		foreach (Animator animator in array)
		{
			if (changeSpeed <= 0f)
			{
				animator.Play(anime);
			}
			else
			{
				animator.CrossFadeInFixedTime(anime, changeSpeed);
			}
		}
	}
}
