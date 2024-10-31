using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.EventSystems;

public class HMaleShowChangeUIManager : MonoBehaviour
{
	[SerializeField]
	private ToggleButton mainToggle;

	[SerializeField]
	private GameObject hideableRoot;

	[SerializeField]
	private HMaleShowChangeUI[] uis = new HMaleShowChangeUI[4];

	private List<Male> males;

	private void Awake()
	{
		mainToggle.ActionAddListener(OnChangeMainToggle);
		hideableRoot.SetActive(false);
	}

	private void Update()
	{
		Update_ShortCutKeys();
	}

	private void Update_ShortCutKeys()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Male male = uis[0].GetMale();
			SystemSE.Play(SystemSE.SE.CHOICE);
			MALE_SHOW show = (MALE_SHOW)((int)(male.MaleShow + 1) % 5);
			male.ChangeMaleShow(show);
			RecordMaleShow(male);
			UpdateToggles();
		}
		if (!Input.GetKeyDown(KeyCode.Alpha5))
		{
			return;
		}
		Male male2 = uis[1].GetMale();
		if (!(male2 != null))
		{
			return;
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
		MALE_SHOW show2 = (MALE_SHOW)((int)(male2.MaleShow + 1) % 5);
		for (int i = 1; i < uis.Length; i++)
		{
			Male male3 = uis[i].GetMale();
			if (male3 != null)
			{
				male3.ChangeMaleShow(show2);
				RecordMaleShow(male3);
			}
		}
		UpdateToggles();
	}

	public void Setup(H_Scene h_scene)
	{
		for (int i = 0; i < uis.Length; i++)
		{
			uis[i].Setup(h_scene);
		}
	}

	public void SetMales(List<Male> males)
	{
		for (int i = 0; i < uis.Length; i++)
		{
			Male male = ((i >= males.Count) ? null : males[i]);
			uis[i].SetMale(male);
			uis[i].gameObject.SetActive(male != null);
		}
	}

	private void OnChangeMainToggle(bool change)
	{
		hideableRoot.SetActive(change);
		if (change)
		{
			UpdateToggles();
		}
	}

	private void UpdateToggles()
	{
		for (int i = 0; i < uis.Length; i++)
		{
			uis[i].UpdateToggles();
		}
	}

	private void RecordMaleShow(Male male)
	{
		GlobalData.maleShows[(int)male.MaleID] = male.MaleShow;
	}
}
