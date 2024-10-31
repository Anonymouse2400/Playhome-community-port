using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverlapSwitch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	[SerializeField]
	private GameObject target;

	private void OnEnable()
	{
		target.SetActive(false);
	}

	private void OnDisable()
	{
		target.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData data)
	{
		target.SetActive(true);
	}

	public void OnPointerExit(PointerEventData data)
	{
		target.SetActive(false);
	}
}
