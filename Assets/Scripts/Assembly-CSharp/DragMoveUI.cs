using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMoveUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	[SerializeField]
	private Transform moveTarget;

	private RectTransform myRect;

	private RectTransform moveRect;

	public bool IsDragging { get; protected set; }

	private void Awake()
	{
		if (moveTarget == null)
		{
			moveTarget = base.transform;
		}
		myRect = GetComponent<RectTransform>();
		moveRect = moveTarget as RectTransform;
	}

	private void Update()
	{
	}

	public void OnBeginDrag(PointerEventData data)
	{
		IsDragging = true;
	}

	public void OnDrag(PointerEventData data)
	{
		Vector2 delta = data.delta;
		Vector2 vector = moveTarget.position;
		vector += delta;
		moveTarget.position = vector;
		CheckMoveArea();
		IsDragging = true;
	}

	public void OnEndDrag(PointerEventData data)
	{
		IsDragging = false;
	}

	private void CheckMoveArea()
	{
		Canvas componentInParent = base.transform.GetComponentInParent<Canvas>();
		float num = 1f / componentInParent.scaleFactor;
		Vector3[] array = new Vector3[4];
		myRect.GetWorldCorners(array);
		Vector2 zero = Vector2.zero;
		float x = array[0].x;
		float x2 = array[2].x;
		float y = array[1].y;
		float y2 = array[0].y;
		if (x < 0f)
		{
			zero += new Vector2(0f - x, 0f) * num;
		}
		if (x2 > (float)Screen.width)
		{
			zero += new Vector2((float)Screen.width - x2, 0f) * num;
		}
		if (y > (float)Screen.height)
		{
			zero += new Vector2(0f, (float)Screen.height - y) * num;
		}
		if (y2 < 0f)
		{
			zero += new Vector2(0f, 0f - y2) * num;
		}
		moveRect.anchoredPosition += zero;
	}
}
