using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemChangeButton : MonoBehaviour
{
	private Color color;

	[SerializeField]
	private Text title;

	[SerializeField]
	private Image thumnbnailImage;

	[SerializeField]
	private Text dataName;

	[SerializeField]
	private MoveableThumbnailSelectUI selectUI;

	private Button _button;

	public Button button
	{
		get
		{
			if (_button == null)
			{
				_button = GetComponent<Button>();
			}
			return _button;
		}
	}

	public bool interactable
	{
		get
		{
			return button.interactable;
		}
		set
		{
			button.interactable = value;
		}
	}

	public void Setup(string title, MoveableThumbnailSelectUI selectUI)
	{
		this.title.text = title;
		this.selectUI = selectUI;
		ApplyFromSelectedData();
	}

	public void ApplyFromSelectedData()
	{
		CustomSelectSet selectedData = selectUI.GetSelectedData();
		dataName.text = ((selectedData == null) ? string.Empty : selectedData.name);
		thumnbnailImage.sprite = ((selectedData == null) ? null : selectedData.thumbnail_S);
	}

	public void OnSelectChange()
	{
		CustomSelectSet selectedData = selectUI.GetSelectedData();
		dataName.text = ((selectedData == null) ? string.Empty : selectedData.name);
		thumnbnailImage.sprite = ((selectedData == null) ? null : selectedData.thumbnail_S);
	}

	public void OpenSelectUI()
	{
		if (selectUI != null)
		{
			selectUI.Open(_button);
		}
	}
}
