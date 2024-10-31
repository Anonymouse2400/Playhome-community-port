using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragInputUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	[Serializable]
	public class DragEvent : UnityEvent<Vector2>
	{
	}

	private RectTransform myRect;

	public DragEvent onDragEvent;

	public bool IsDragging { get; protected set; }

	private void Awake()
	{
		myRect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (IsDragging)
		{
			UpdateDrag();
		}
	}

	public void AddListener(UnityAction<Vector2> call)
	{
		onDragEvent.AddListener(call);
	}

	public void OnBeginDrag(PointerEventData data)
	{
		IsDragging = true;
		GameCursor.Lock();
	}

	public void OnDrag(PointerEventData data)
	{
		IsDragging = true;
	}

	private void UpdateDrag()
	{
		GameCursor.Lock();
		Vector2 move = Vector3.zero;
		move.x = Input.GetAxis("Mouse X");
		move.y = Input.GetAxis("Mouse Y");
		CheckMoveArea(ref move);
		if (onDragEvent != null)
		{
			onDragEvent.Invoke(move);
		}
	}

	public void OnEndDrag(PointerEventData data)
	{
		IsDragging = false;
	}

	private void CheckMoveArea(ref Vector2 move)
	{
		Vector2 vector = myRect.position;
		Vector2 vector2 = vector + myRect.rect.min + move;
		Vector2 vector3 = vector + myRect.rect.max + move;
		Vector2 zero = Vector2.zero;
		if (vector2.x < 0f)
		{
			zero.x = 0f - vector2.x;
		}
		if (vector2.y < 0f)
		{
			zero.y = 0f - vector2.y;
		}
		if (vector3.x > (float)Screen.width)
		{
			zero.x = (float)Screen.width - vector3.x;
		}
		if (vector3.y > (float)Screen.height)
		{
			zero.y = (float)Screen.height - vector3.y;
		}
		move += zero;
	}
}
