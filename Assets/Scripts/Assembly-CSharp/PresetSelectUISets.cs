using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PresetSelectUISets
{
	public PresetListToggle toggle;

	public MoveableThumbnailSelectUI select;

	private UnityAction<CustomSelectSet> act;

	private List<CustomSelectSet> setDatas;

	private string title;

	public PresetSelectUISets(EditMode editMode, GameObject toggleParent, MoveableThumbnailSelectUI selectUI, string title, List<CustomSelectSet> setDatas, UnityAction<CustomSelectSet> act)
	{
		this.title = title;
		this.act = act;
		this.setDatas = setDatas;
		toggle = editMode.CreatePresetListToggle(toggleParent);
		select = selectUI;
		select.gameObject.SetActive(false);
		toggle.Setup(select);
		toggle.GetComponentInChildren<Text>().text = title;
		toggle.toggle.onValueChanged.AddListener(SetupSelect);
	}

	private void SetupSelect(bool flag)
	{
		if (flag)
		{
			select.Setup(title, setDatas, toggle.OnChangeState, OnChange);
		}
	}

	private void OnChange(CustomSelectSet set)
	{
		if (act != null)
		{
			act(set);
		}
	}
}
