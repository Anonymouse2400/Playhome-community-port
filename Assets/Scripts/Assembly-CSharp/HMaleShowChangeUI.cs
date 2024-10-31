using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class HMaleShowChangeUI : MonoBehaviour
{
	[SerializeField]
	private new Text name;

	[SerializeField]
	private ToggleButton clothing;

	[SerializeField]
	private ToggleButton nude;

	[SerializeField]
	private ToggleButton oneColor;

	[SerializeField]
	private ToggleButton tin;

	[SerializeField]
	private ToggleButton hide;

	[SerializeField]
	private Toggle bareFeet;

	private bool invoke = true;

	private H_Scene h_scene;

	private Male male;

	private void Awake()
	{
		clothing.ActionAddListener(delegate(bool flag)
		{
			if (flag)
			{
				OnChange_Clothing();
			}
		});
		bareFeet.onValueChanged.AddListener(OnChange_BareFeet);
		nude.ActionAddListener(delegate(bool flag)
		{
			if (flag)
			{
				OnChange_Nude();
			}
		});
		oneColor.ActionAddListener(delegate(bool flag)
		{
			if (flag)
			{
				OnChange_Color();
			}
		});
		tin.ActionAddListener(delegate(bool flag)
		{
			if (flag)
			{
				OnChange_Tin();
			}
		});
		hide.ActionAddListener(delegate(bool flag)
		{
			if (flag)
			{
				OnChange_Hide();
			}
		});
	}

	public void Setup(H_Scene h_scene)
	{
		this.h_scene = h_scene;
	}

	public void SetMale(Male male)
	{
		this.male = male;
		if (male != null)
		{
			name.text = Male.MaleName(male.MaleID);
		}
	}

	public void UpdateToggles()
	{
		invoke = false;
		if (male != null)
		{
			clothing.ChangeValue(male.MaleShow == MALE_SHOW.CLOTHING, false);
			nude.ChangeValue(male.MaleShow == MALE_SHOW.NUDE, false);
			oneColor.ChangeValue(male.MaleShow == MALE_SHOW.ONECOLOR, false);
			tin.ChangeValue(male.MaleShow == MALE_SHOW.TIN, false);
			hide.ChangeValue(male.MaleShow == MALE_SHOW.HIDE, false);
			bareFeet.gameObject.SetActive(male.MaleShow == MALE_SHOW.CLOTHING);
			bareFeet.isOn = male.WearShoes;
		}
		invoke = true;
	}

	public Male GetMale()
	{
		return male;
	}

	private void Record()
	{
		if (male != null)
		{
			GlobalData.maleShows[(int)male.MaleID] = male.MaleShow;
		}
	}

	private void OnChange_Clothing()
	{
		if (invoke)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (male != null)
			{
				male.ChangeMaleShow(MALE_SHOW.CLOTHING);
			}
			Record();
			bareFeet.gameObject.SetActive(true);
		}
	}

	private void OnChange_Nude()
	{
		if (invoke)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (male != null)
			{
				male.ChangeMaleShow(MALE_SHOW.NUDE);
			}
			Record();
			bareFeet.gameObject.SetActive(false);
		}
	}

	private void OnChange_Color()
	{
		if (invoke)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (male != null)
			{
				male.ChangeMaleShow(MALE_SHOW.ONECOLOR);
			}
			Record();
			bareFeet.gameObject.SetActive(false);
		}
	}

	private void OnChange_Tin()
	{
		if (invoke)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (male != null)
			{
				male.ChangeMaleShow(MALE_SHOW.TIN);
			}
			Record();
			bareFeet.gameObject.SetActive(false);
		}
	}

	private void OnChange_Hide()
	{
		if (invoke)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (male != null)
			{
				male.ChangeMaleShow(MALE_SHOW.HIDE);
			}
			Record();
			bareFeet.gameObject.SetActive(false);
		}
	}

	private void OnChange_BareFeet(bool flag)
	{
		if (invoke)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (male != null)
			{
				male.SetWearShoes(flag);
			}
		}
	}
}
