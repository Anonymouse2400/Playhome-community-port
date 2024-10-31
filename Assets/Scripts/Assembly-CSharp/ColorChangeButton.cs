using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChangeButton : MonoBehaviour
{
	private Color color;

	[SerializeField]
	private Text title;

	public MoveableColorCustomUI colorUI;

	[SerializeField]
	private Image colorPreview;

	[SerializeField]
	private Image alphaBack;

	[SerializeField]
	private Image alphaPreview;

	[SerializeField]
	private bool hasAlpha;

	private Action<Color> onChangeAct;

	private void Start()
	{
	}

	public void Setup(string title, Color color, bool hasAlpha, Action<Color> onChangeAct)
	{
		this.title.text = title;
		this.hasAlpha = hasAlpha;
		this.onChangeAct = onChangeAct;
		this.color = color;
		alphaBack.gameObject.SetActive(hasAlpha);
		SetImageColor();
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if (colorUI != null && colorUI.openUI == this)
		{
			colorUI.Close();
		}
	}

	public void SetTitle(string title)
	{
		this.title.text = title;
	}

	public void SetColor(Color color)
	{
		this.color = color;
		SetImageColor();
	}

	private void OnChangeColor(Color color)
	{
		this.color = color;
		SetImageColor();
		if (onChangeAct != null)
		{
			onChangeAct(color);
		}
	}

	private void SetImageColor()
	{
		Color color = this.color;
		color.a = 1f;
		colorPreview.color = color;
		float fillAmount = ((!hasAlpha) ? 0f : this.color.a);
		alphaPreview.fillAmount = fillAmount;
	}

	public void OpenColorUI()
	{
		colorUI.Open(this, title.text, color, hasAlpha, OnChangeColor);
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

	private void SetColorUIPos()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		RectTransform trans = colorUI.transform as RectTransform;
		Canvas componentInParent = rectTransform.GetComponentInParent<Canvas>();
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		float num = 1f / componentInParent.scaleFactor;
		float num2 = (float)Screen.width * num;
		float num3 = (float)Screen.height * num;
		float num4 = (num4 = array[0].x * num);
		float num5 = (num5 = array[0].y * num);
		float num6 = num4;
		float num7 = num4 + rectTransform.sizeDelta.x;
		float y = num5 + rectTransform.sizeDelta.y;
		float num8 = num5;
		float num9 = 10f;
		float num10 = num7;
		Vector2 offsetPivot = new Vector2(0f, 1f);
		if (num7 > num2 * 0.5f && num6 > num2 * 0.5f)
		{
			num10 = num6;
			num10 -= num9;
			offsetPivot.x = 1f;
		}
		else
		{
			num10 += num9;
		}
		Vector2 pos = new Vector2(num10, y);
		SetPosUI(trans, pos, offsetPivot);
	}

	private void SetPosUI(RectTransform trans, Vector2 pos, Vector2 offsetPivot)
	{
		Canvas componentInParent = trans.GetComponentInParent<Canvas>();
		float num = (float)Screen.width * (1f / componentInParent.scaleFactor);
		float num2 = (float)Screen.height * (1f / componentInParent.scaleFactor);
		float num3 = trans.anchorMin.x * num;
		float num4 = trans.anchorMin.y * num2;
		Vector2 vector = new Vector2(0f - num3, 0f - num4);
		Vector2 vector2 = new Vector2(trans.sizeDelta.x * trans.pivot.x, trans.sizeDelta.y * trans.pivot.y);
		Vector2 vector3 = new Vector2(trans.sizeDelta.x * (0f - offsetPivot.x), trans.sizeDelta.y * (0f - offsetPivot.y));
		trans.anchoredPosition = vector + vector2 + pos + vector3;
		float num5 = num3 + trans.offsetMin.x;
		float num6 = num3 + trans.offsetMax.x;
		float num7 = num4 + trans.offsetMax.y;
		float num8 = num4 + trans.offsetMin.y;
		if (num5 < 0f)
		{
			trans.anchoredPosition += new Vector2(0f - num5, 0f);
		}
		if (num6 > num)
		{
			trans.anchoredPosition += new Vector2(num - num6, 0f);
		}
		if (num7 > num2)
		{
			trans.anchoredPosition += new Vector2(0f, num2 - num7);
		}
		if (num8 < 0f)
		{
			trans.anchoredPosition += new Vector2(0f, 0f - num8);
		}
	}
}
