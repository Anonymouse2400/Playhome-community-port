using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class ScreenFade : MonoBehaviour
{
	public enum TYPE
	{
		IN = 0,
		OUT = 1,
		OUT_IN = 2
	}

	private enum NEXT
	{
		NONE = 0,
		ACTION = 1,
		DIE = 2
	}

	private Color setColor = Color.black;

	private TYPE type;

	private float span = 1f;

	private float delay;

	private float timer;

	private int depth = -1000;

	private Action action;

	private bool actionEnd;

	private bool suicide = true;

	private bool end;

	private GameObject clickBlocker;

	private Image fadeImage;

	private float rate;

	private NEXT next;

	private void Awake()
	{
		clickBlocker = ResourceUtility.CreateInstance<GameObject>("CommonPrefabs/ClickBlocker");
		fadeImage = clickBlocker.GetComponentInChildren<Image>();
		rate = 0f;
		ChangeColor(0f);
		actionEnd = false;
	}

	private void LateUpdate()
	{
		timer += Time.deltaTime;
		if (!end)
		{
			if (next == NEXT.ACTION)
			{
				next = NEXT.NONE;
				action();
				return;
			}
			if (next == NEXT.DIE)
			{
				next = NEXT.NONE;
				end = true;
				Die();
				return;
			}
			rate = CalcRate();
			if (timer > delay)
			{
				if (action != null && !actionEnd)
				{
					if (type == TYPE.OUT_IN && rate >= 0.5f)
					{
						next = NEXT.ACTION;
						actionEnd = true;
					}
					else if (rate >= 1f)
					{
						next = NEXT.ACTION;
						actionEnd = true;
					}
				}
				else if (rate >= 1f && suicide)
				{
					next = NEXT.DIE;
				}
			}
		}
		ChangeColor(rate);
	}

	private void OnDestroy()
	{
		if (clickBlocker != null)
		{
			UnityEngine.Object.Destroy(clickBlocker);
		}
	}

	public static ScreenFade Create()
	{
		return new GameObject("ScreenFade").AddComponent<ScreenFade>();
	}

	private void Setup(TYPE type, Color color, float span, float delay, Action action, bool suicide)
	{
		this.type = type;
		setColor = color;
		this.span = span;
		this.delay = delay;
		this.action = action;
		this.suicide = suicide;
		end = false;
		ChangeColor(0f);
	}

	private float CalcRate()
	{
		if (span == 0f)
		{
			return 1f;
		}
		if (timer < delay)
		{
			return 0f;
		}
		float num = timer - delay;
		return Mathf.Clamp01(num / span);
	}

	private void ChangeColor(float rate)
	{
		Color color = setColor;
		if (type == TYPE.OUT_IN)
		{
			if (next == NEXT.ACTION)
			{
				color.a = 1f;
			}
			else if (rate < 0.5f)
			{
				color.a = rate * 2f;
			}
			else
			{
				color.a = 1f - (rate - 0.5f) * 2f;
			}
		}
		else
		{
			float a = ((type != 0) ? 0f : 1f);
			float b = ((type != 0) ? 1f : 0f);
			color.a = Mathf.Lerp(a, b, rate);
		}
		fadeImage.color = color;
	}

	public void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static ScreenFade StartFade(TYPE type, Color color, float span, float delay = 0f, Action action = null, bool suicide = true)
	{
		ScreenFade screenFade = Create();
		screenFade.Setup(type, color, span, delay, action, suicide);
		return screenFade;
	}
}
