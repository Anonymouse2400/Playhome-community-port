using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SquarePickUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IEventSystemHandler
{
	[SerializeField]
	private RectTransform area;

	[SerializeField]
	private Image pointImg;

	private bool isDragging;

	private Vector2 value;

	private Action<Vector2> onChangeAction;

	public Vector2 Value
	{
		get
		{
			return value;
		}
		set
		{
			SetValue(value);
		}
	}

	public void SetValue(Vector2 val)
	{
		value = val;
		ValToPoint();
		OnChangeValue();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!isDragging)
		{
			CursorPosToVal(eventData.position);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		CursorPosToVal(eventData.position);
		isDragging = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		CursorPosToVal(eventData.position);
		isDragging = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
	}

	private void CursorPosToVal(Vector2 cursorPos)
	{
		Vector2 vector = area.InverseTransformPoint(cursorPos);
		value.x = Mathf.InverseLerp(area.rect.xMin, area.rect.xMax, vector.x);
		value.y = Mathf.InverseLerp(area.rect.yMin, area.rect.yMax, vector.y);
		ValToPoint();
		OnChangeValue();
	}

	private void ValToPoint()
	{
		Vector2 anchoredPosition = default(Vector2);
		anchoredPosition.x = area.rect.size.x * value.x;
		anchoredPosition.y = area.rect.size.y * value.y;
		pointImg.rectTransform.anchoredPosition = anchoredPosition;
	}

	private void OnChangeValue()
	{
		if (onChangeAction != null)
		{
			onChangeAction(value);
		}
	}

	public void SetOnChangeAction(Action<Vector2> action)
	{
		onChangeAction = action;
	}
}
