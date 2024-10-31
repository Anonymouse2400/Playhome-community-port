using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoveableUI : MonoBehaviour
{
	public enum STATE
	{
		CLOSED = 0,
		MINIMIZED = 1,
		OPEN = 2
	}

	[Serializable]
	public class ChangeStateEvent : UnityEvent<STATE>
	{
	}

	[SerializeField]
	private Text title;

	[SerializeField]
	private RectTransform hideableArea;

	[SerializeField]
	private Button minimizeButton;

	[SerializeField]
	private Button openButton;

	[SerializeField]
	private Button closeButton;

	public ChangeStateEvent onChangeState;

	private GameControl gameCtrl;

	public STATE State { get; protected set; }

	private void Awake()
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener(delegate
		{
			OnActiveSort();
		});
		GetEventTrigger().triggers.Add(entry);
	}

	private void Start()
	{
		gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
	}

	private void OnActiveSort()
	{
		base.transform.SetAsLastSibling();
	}

	public void SetTitle(string str)
	{
		title.text = str;
	}

	public void AddOnChange(UnityAction<STATE> act)
	{
		onChangeState.AddListener(act);
	}

	public void RemoveAllListenersOnChange()
	{
		onChangeState.RemoveAllListeners();
	}

	private void ChangeState(STATE next)
	{
		State = next;
		if (State == STATE.OPEN)
		{
			base.gameObject.SetActive(true);
			hideableArea.gameObject.SetActive(true);
			OnActiveSort();
		}
		else if (State == STATE.MINIMIZED)
		{
			base.gameObject.SetActive(true);
			hideableArea.gameObject.SetActive(false);
		}
		else if (State == STATE.CLOSED)
		{
			base.gameObject.SetActive(false);
		}
		minimizeButton.gameObject.SetActive(State != STATE.MINIMIZED);
		openButton.gameObject.SetActive(State != STATE.OPEN);
		onChangeState.Invoke(State);
	}

	public void Open()
	{
		ChangeState(STATE.OPEN);
		if (base.gameObject.activeInHierarchy)
		{
			SystemSE.Play(SystemSE.SE.OPEN);
		}
	}

	public void Minimize()
	{
		if (base.gameObject.activeInHierarchy)
		{
			SystemSE.Play(SystemSE.SE.CLOSE);
		}
		ChangeState(STATE.MINIMIZED);
	}

	public void Close()
	{
		if (base.gameObject.activeInHierarchy && State != 0)
		{
			SystemSE.Play(SystemSE.SE.CLOSE);
		}
		ChangeState(STATE.CLOSED);
	}

	public EventTrigger GetEventTrigger()
	{
		EventTrigger component = GetComponent<EventTrigger>();
		if ((bool)component)
		{
			return component;
		}
		return base.gameObject.AddComponent<EventTrigger>();
	}
}
