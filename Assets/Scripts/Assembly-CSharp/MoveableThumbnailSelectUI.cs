using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoveableThumbnailSelectUI : MonoBehaviour
{
	[SerializeField]
	private MoveableUI moveable;

	[SerializeField]
	private ThumbnailSelectUI select;

	public bool isShow
	{
		get
		{
			return moveable.isActiveAndEnabled;
		}
	}

	public bool isOpen
	{
		get
		{
			return moveable.State == MoveableUI.STATE.OPEN;
		}
	}

	public Selectable openUI { get; private set; }

	public void Setup(string str, List<CustomSelectSet> setDatas, UnityAction<MoveableUI.STATE> onChangeState, UnityAction<CustomSelectSet> onSelectAct)
	{
		moveable.SetTitle(str);
		moveable.RemoveAllListenersOnChange();
		moveable.AddOnChange(onChangeState);
		select.SetDatas(setDatas);
		select.RemoveAllListenersOnSelectAction();
		select.AddOnSelectAction(onSelectAct);
		UpdateEnables();
	}

	public void Open(Selectable ui)
	{
		openUI = ui;
		moveable.Open();
		MoveableColorCustomUI[] array = UnityEngine.Object.FindObjectsOfType<MoveableColorCustomUI>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Close();
		}
	}

	public void Close()
	{
		moveable.Close();
	}

	public void SetSelectedNo(int no)
	{
		select.SetSelectedNo(no);
	}

	public void SetSelectedFromDataID(int id)
	{
		select.SetSelectedFromDataID(id);
	}

	public CustomSelectSet GetSelectedData()
	{
		return select.GetSelectedData();
	}

	public void UpdateEnables()
	{
		select.UpdateEnables();
	}
}
