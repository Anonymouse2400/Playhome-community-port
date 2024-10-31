using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ThumbnailSelectCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	private ThumbnailSelectUI owner;

	[SerializeField]
	private Image image;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Image newImg;

	private Toggle toggle;

	private int id;

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
	}

	public void Setup(ThumbnailSelectUI owner, int id, string name, Sprite sprite, bool isNew)
	{
		this.owner = owner;
		image.sprite = sprite;
		this.id = id;
		if (nameText != null)
		{
			nameText.text = name;
		}
		newImg.gameObject.SetActive(isNew);
		image.enabled = sprite != null;
	}

	private void OnEnable()
	{
		OnCursorExit();
	}

	private void OnDisable()
	{
		OnCursorExit();
	}

	public void OnPointerEnter(PointerEventData data)
	{
		OnCursorEnter();
	}

	public void OnPointerExit(PointerEventData data)
	{
		OnCursorExit();
	}

	public void OnToggleValueChange(bool isOn)
	{
		if (isOn)
		{
			owner.OnSelect(id);
		}
	}

	private void OnCursorEnter()
	{
		if (owner != null)
		{
			owner.OnEnterCell(id);
		}
	}

	private void OnCursorExit()
	{
		if (owner != null)
		{
			owner.OnExitCell(id);
		}
	}

	public void SetToggle(bool isOn)
	{
		toggle.isOn = isOn;
	}

	public void SetInteractable(bool flag)
	{
		toggle.interactable = flag;
	}
}
