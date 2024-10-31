using System;
using UnityEngine;
using UnityEngine.Rendering;

public class WearLiquidObj
{
	public GameObject root;

	private GameObject upper_all_f01;

	private GameObject upper_all_f02;

	private GameObject upper_all_b01;

	private GameObject upper_all_b02;

	private GameObject upper_half_f01;

	private GameObject upper_half_f02;

	private GameObject upper_half_b01;

	private GameObject upper_half_b02;

	private GameObject lower_all_f01;

	private GameObject lower_all_f02;

	private GameObject lower_all_b01;

	private GameObject lower_all_b02;

	private GameObject lower_half_f01;

	private GameObject lower_half_f02;

	private GameObject lower_half_b01;

	private GameObject lower_half_b02;

	public WearLiquidObj(GameObject root)
	{
		this.root = root;
		upper_all_f01 = Find("N_a_f01");
		upper_all_f02 = Find("N_a_f02");
		upper_all_b01 = Find("N_a_b01");
		upper_all_b02 = Find("N_a_b02");
		upper_half_f01 = Find("N_b_f01");
		upper_half_f02 = Find("N_b_f02");
		upper_half_b01 = Find("N_b_b01");
		upper_half_b02 = Find("N_b_b02");
		lower_all_f01 = Find("N_d_f01");
		lower_all_f02 = Find("N_d_f02");
		lower_all_b01 = Find("N_d_b01");
		lower_all_b02 = Find("N_d_b02");
		lower_half_f01 = Find("N_n_f01");
		lower_half_f02 = Find("N_n_f02");
		lower_half_b01 = Find("N_n_b01");
		lower_half_b02 = Find("N_n_b02");
		Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].lightProbeUsage = LightProbeUsage.Off;
			componentsInChildren[i].reflectionProbeUsage = ReflectionProbeUsage.Off;
		}
	}

	private GameObject Find(string name)
	{
		Transform transform = Transform_Utility.FindTransform(root.transform, name);
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
			return transform.gameObject;
		}
		return null;
	}

	public void SetShow(bool upperAll, bool upperHalf, bool lowerAll, bool lowerHalf, int[] sperms)
	{
		int num = sperms[1];
		int num2 = sperms[2];
		int num3 = sperms[3];
		int num4 = sperms[4];
		if ((bool)upper_all_f01)
		{
			upper_all_f01.SetActive(upperAll && num == 1);
		}
		if ((bool)upper_all_f02)
		{
			upper_all_f02.SetActive(upperAll && num == 2);
		}
		if ((bool)upper_all_b01)
		{
			upper_all_b01.SetActive(upperAll && num2 == 1);
		}
		if ((bool)upper_all_b02)
		{
			upper_all_b02.SetActive(upperAll && num2 == 2);
		}
		if ((bool)upper_half_f01)
		{
			upper_half_f01.SetActive(upperHalf && num == 1);
		}
		if ((bool)upper_half_f02)
		{
			upper_half_f02.SetActive(upperHalf && num == 2);
		}
		if ((bool)upper_half_b01)
		{
			upper_half_b01.SetActive(upperHalf && num2 == 1);
		}
		if ((bool)upper_half_b02)
		{
			upper_half_b02.SetActive(upperHalf && num2 == 2);
		}
		if ((bool)lower_all_f01)
		{
			lower_all_f01.SetActive(lowerAll && num3 == 1);
		}
		if ((bool)lower_all_f02)
		{
			lower_all_f02.SetActive(lowerAll && num3 == 2);
		}
		if ((bool)lower_all_b01)
		{
			lower_all_b01.SetActive(lowerAll && num4 == 1);
		}
		if ((bool)lower_all_b02)
		{
			lower_all_b02.SetActive(lowerAll && num4 == 2);
		}
		if ((bool)lower_half_f01)
		{
			lower_half_f01.SetActive(lowerHalf && num3 == 1);
		}
		if ((bool)lower_half_f02)
		{
			lower_half_f02.SetActive(lowerHalf && num3 == 2);
		}
		if ((bool)lower_half_b01)
		{
			lower_half_b01.SetActive(lowerHalf && num4 == 1);
		}
		if ((bool)lower_half_b02)
		{
			lower_half_b02.SetActive(lowerHalf && num4 == 2);
		}
	}
}
