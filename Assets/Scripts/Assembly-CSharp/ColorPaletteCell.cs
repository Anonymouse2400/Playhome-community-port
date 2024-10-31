using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPaletteCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	[SerializeField]
	private Image colorImage;

	[SerializeField]
	private Image opaqueImage;

	[SerializeField]
	private Image frame;

	private int id;

	private Action<int> onSave;

	private Action<int> onLoad;

	private void Start()
	{
		frame.enabled = false;
	}

	public void Setup(int id, Color color, Action<int> onSave, Action<int> onLoad)
	{
		this.id = id;
		SetColor(color);
		this.onSave = onSave;
		this.onLoad = onLoad;
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		frame.enabled = false;
	}

	public void OnPointerEnter(PointerEventData data)
	{
		frame.enabled = true;
	}

	public void OnPointerExit(PointerEventData data)
	{
		frame.enabled = false;
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (data.button == PointerEventData.InputButton.Left)
		{
			if (onSave != null)
			{
				onLoad(id);
			}
		}
		else if (data.button == PointerEventData.InputButton.Right && onSave != null)
		{
			onSave(id);
		}
	}

	public void SetColor(Color color)
	{
		Color color2 = color;
		color2.a = 1f;
		colorImage.color = color;
		opaqueImage.color = color2;
		frame.color = ColorPicker.InverseColor(color2);
	}
}
