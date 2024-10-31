using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckableSlider : Slider, IEndDragHandler, IEventSystemHandler
{
	private bool isDrag;

	public bool IsHandling
	{
		get
		{
			return isDrag;
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		isDrag = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDrag = false;
	}
}
