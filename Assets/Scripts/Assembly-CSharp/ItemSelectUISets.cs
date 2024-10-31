using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemSelectUISets
{
	public ItemChangeToggle toggle;

	public MoveableThumbnailSelectUI select;

	private UnityAction<CustomSelectSet> act;

	private List<CustomSelectSet> setDatas;

	private int selectID = -1;

	private string title;

	public ItemSelectUISets(EditMode editMode, GameObject toggleParent, MoveableThumbnailSelectUI selectUI, string title, List<CustomSelectSet> setDatas, UnityAction<CustomSelectSet> act)
	{
		this.title = title;
		this.act = act;
		this.setDatas = setDatas;
		toggle = editMode.CreateItemToggle(toggleParent);
		select = selectUI;
		select.gameObject.SetActive(false);
		toggle.Setup(title, select);
		toggle.toggle.onValueChanged.AddListener(SetupSelect);
	}

	private void SetupSelect(bool flag)
	{
		if (flag)
		{
			select.Setup(title, setDatas, toggle.OnChangeState, OnChange);
			select.SetSelectedFromDataID(selectID);
		}
	}

	public void ChangeDatas(List<CustomSelectSet> setDatas, bool resetSelectID)
	{
		if (resetSelectID)
		{
			selectID = -1;
		}
		this.setDatas = setDatas;
		select.Setup(title, setDatas, toggle.OnChangeState, OnChange);
		select.SetSelectedFromDataID(selectID);
		select.UpdateEnables();
		CustomSelectSet data = ((!resetSelectID) ? GetSelectedData(selectID) : null);
		toggle.ApplyFromData(data);
	}

	private void OnChange(CustomSelectSet set)
	{
		if (set != null)
		{
			selectID = set.id;
		}
		else
		{
			selectID = -1;
		}
		toggle.OnSelectChange();
		if (act != null)
		{
			act(set);
		}
	}

	public void SetSelectedFromDataID(int id)
	{
		selectID = id;
		toggle.ApplyFromData(GetSelectedData(selectID));
	}

	public void SetSelectedNo(int no)
	{
		if (no >= 0 && no < setDatas.Count)
		{
			selectID = setDatas[no].id;
		}
		else
		{
			selectID = -1;
		}
		toggle.ApplyFromData(GetSelectedData(selectID));
	}

	public CustomSelectSet GetSelectedData(int id)
	{
		if (setDatas == null)
		{
			return null;
		}
		for (int i = 0; i < setDatas.Count; i++)
		{
			if (setDatas[i].id == id)
			{
				return setDatas[i];
			}
		}
		return null;
	}

	public CustomSelectSet GetSelectedData()
	{
		return GetSelectedData(selectID);
	}

	public List<CustomSelectSet> GetDatas()
	{
		return setDatas;
	}

	public void ApplyFromID(int id)
	{
		selectID = id;
		toggle.ApplyFromData(GetSelectedData(selectID));
	}

	public void ApplyFromSelectedData()
	{
		toggle.ApplyFromData(GetSelectedData(selectID));
	}
}
