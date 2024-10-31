using System;
using UnityEngine;

public class MoveableColorCustomUI : MonoBehaviour
{
	[SerializeField]
	private MoveableUI moveable;

	[SerializeField]
	private ColorPicker colorPicker;

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

	public Color color
	{
		get
		{
			return colorPicker.EditColor;
		}
		set
		{
			colorPicker.EditColor = value;
		}
	}

	public ColorChangeButton openUI { get; private set; }

	public void SetTitle(string str)
	{
		moveable.SetTitle(str);
	}

	public void Open(ColorChangeButton ui)
	{
		openUI = ui;
		moveable.Open();
	}

	public void Open(ColorChangeButton ui, string title, Color color, bool hasAlpha, Action<Color> act)
	{
		openUI = ui;
		moveable.SetTitle(title);
		colorPicker.Setup(color, hasAlpha, act);
		moveable.Open();
		MoveableThumbnailSelectUI[] array = UnityEngine.Object.FindObjectsOfType<MoveableThumbnailSelectUI>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Close();
		}
	}

	public void Close()
	{
		moveable.Close();
	}
}
