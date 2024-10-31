using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class MaleTest : MonoBehaviour
{
	public Male original;

	public int num = 1;

	public float distance = 10f;

	private List<Male> males = new List<Male>();

	private void Update()
	{
	}

	public void Fade()
	{
		ScreenFade.StartFade(ScreenFade.TYPE.OUT_IN, Color.red, 1f, 0f, Calc);
	}

	public void Calc()
	{
		for (int i = 0; i < num; i++)
		{
			Male male = UnityEngine.Object.Instantiate(original);
			male.SetMaleID(MALE_ID.MOB_B);
			CustomParameter maleCustomParam = GlobalData.PlayData.GetMaleCustomParam(MALE_ID.MOB_B);
			male.Load(maleCustomParam);
			male.Apply();
			male.transform.SetParent(base.transform, false);
			float x = UnityEngine.Random.Range(0f - distance, distance);
			float y = 0f;
			float z = UnityEngine.Random.Range(0f - distance, distance);
			male.transform.position = new Vector3(x, y, z);
			male.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			males.Add(male);
		}
	}
}
