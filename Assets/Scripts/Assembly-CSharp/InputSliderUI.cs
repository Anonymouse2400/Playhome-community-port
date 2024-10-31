using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputSliderUI : MonoBehaviour
{
	[Serializable]
	public class OnChangeAction : UnityEvent<float>
	{
	}

	[SerializeField]
	private Text title;

	[SerializeField]
	private string textFormat = "0.00";

	[SerializeField]
	private CheckableSlider slider;

	[SerializeField]
	private InputField inputField;

	[SerializeField]
	private bool hasDef;

	[SerializeField]
	private float defVal;

	[SerializeField]
	private Button defButton;

	[SerializeField]
	private Image noIntaractImage;

	private bool invoke = true;

	private float value;

	[SerializeField]
	private OnChangeAction onChangeAction;

	private bool intaractable = true;

	public float Value
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

	public bool IsHandling
	{
		get
		{
			return slider.IsHandling;
		}
	}

	public bool Intaractable
	{
		get
		{
			return intaractable;
		}
		set
		{
			intaractable = value;
			slider.interactable = value;
			((Selectable)(object)inputField).interactable = value;
			defButton.interactable = value;
			if (noIntaractImage != null)
			{
				noIntaractImage.enabled = !value;
			}
		}
	}

	private void Start()
	{
		SetDefPos();
	}

	private void Update()
	{
	}

	public void SetTitle(string str)
	{
		title.text = str;
	}

	public void SetValue(float val)
	{
		OnChanged(val, false);
	}

	public void ChangeSlider(float val)
	{
		if (invoke)
		{
			OnChanged(val);
		}
	}

	public void ChangeInputField(string val)
	{
		if (invoke)
		{
			float val2 = 0f;
			try
			{
				val2 = float.Parse(val);
			}
			catch
			{
			}
			OnChanged(val2);
		}
	}

	private void OnChanged(float val, bool invokeAction = true)
	{
		invoke = false;
		value = Mathf.Clamp(val, slider.minValue, slider.maxValue);
		slider.value = value;
		inputField.text = value.ToString(textFormat);
		if (invokeAction && onChangeAction != null)
		{
			onChangeAction.Invoke(value);
		}
		invoke = true;
	}

	public void ResetDef()
	{
		value = defVal;
		if (invoke)
		{
			OnChanged(defVal);
		}
	}

	public void Setup(float min, float max, bool hasDef, float defVal)
	{
		slider.minValue = min;
		slider.maxValue = max;
		this.hasDef = hasDef;
		this.defVal = defVal;
		SetDefPos();
	}

	public void Setup(string title, float min, float max, bool hasDef, float defVal, UnityAction<float> onChange)
	{
		this.title.text = title;
		slider.minValue = min;
		slider.maxValue = max;
		this.hasDef = hasDef;
		this.defVal = defVal;
		if (onChange != null)
		{
			onChangeAction.AddListener(onChange);
		}
		SetDefPos();
	}

	private void SetDefPos()
	{
		defButton.gameObject.SetActive(hasDef);
		if (hasDef)
		{
			RectTransform rectTransform = defButton.transform as RectTransform;
			RectTransform rectTransform2 = rectTransform.parent as RectTransform;
			float num = Mathf.InverseLerp(slider.minValue, slider.maxValue, defVal);
			float num2 = rectTransform2.rect.xMax - rectTransform2.rect.xMin;
			Vector3 vector = rectTransform.anchoredPosition;
			vector.x = num2 * num;
			rectTransform.anchoredPosition = vector;
		}
	}

	private void OnValidate()
	{
		SetDefPos();
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

	public void AddEventEntry(EventTrigger.Entry entry)
	{
		EventTrigger eventTrigger = GetEventTrigger();
		eventTrigger.triggers.Add(entry);
	}

	public void AddOnChangeAction(UnityAction<float> action)
	{
		onChangeAction.AddListener(action);
	}
}
