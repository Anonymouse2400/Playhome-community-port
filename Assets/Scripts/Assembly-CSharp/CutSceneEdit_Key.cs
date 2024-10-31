using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CutSceneEdit_Key : MonoBehaviour
{
	public CutSceneEdit_TimeBar timeBar;

	public Toggle toggle;

	private int keyID = -1;

	public float time = -1f;

	private List<CutAction> actions = new List<CutAction>();

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
	}

	public void Setup(CutSceneEdit_TimeBar timeBar, int id, float time)
	{
		this.timeBar = timeBar;
		keyID = id;
		this.time = time;
	}

	public void AddAction(CutAction act)
	{
		actions.Add(act);
	}

	public void DragBegin(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.button == PointerEventData.InputButton.Right)
		{
			timeBar.BeginKeyDrag(keyID);
		}
	}

	public void DragEnd(BaseEventData data)
	{
		timeBar.EndKeyDrag(keyID);
	}

	public void ChangeActionTime(float time)
	{
		foreach (CutAction action in actions)
		{
			action.time = time;
		}
	}
}
