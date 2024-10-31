using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CutSceneEdit_TimeBar : MonoBehaviour
{
	[SerializeField]
	private CutSceneEdit edit;

	public float endTime = 60f;

	public float nowTime;

	public bool isPlay;

	private bool invoke = true;

	[SerializeField]
	private InputField endTimeInput;

	[SerializeField]
	private Text zoomText;

	[SerializeField]
	private Slider timeBar;

	[SerializeField]
	private InputField nowTimeInput;

	[SerializeField]
	private RectTransform keysArea;

	[SerializeField]
	private ScrollRect scrollRect;

	[SerializeField]
	private CutSceneEdit_Key keyOriginal;

	[SerializeField]
	private float keysMaxW = 1000f;

	[SerializeField]
	private RectTransform keysParent;

	[SerializeField]
	private RectTransform tickOriginal;

	[SerializeField]
	private float tickNum = 10f;

	[SerializeField]
	private RectTransform ticksParent;

	[SerializeField]
	private RectTransform nowTick;

	[SerializeField]
	private Text nowTickText;

	private float zoom = 1f;

	private const float zoomMax = 100f;

	private List<CutSceneEdit_Key> keys = new List<CutSceneEdit_Key>();

	private List<GameObject> ticks = new List<GameObject>();

	private CutSceneEdit_Key draggingKey;

	public bool IsReputKeys { get; private set; }

	public int ActiveKey { get; private set; }

	private void Start()
	{
		ActiveKey = -1;
		endTimeInput.text = endTime.ToString();
		PutKeys();
		UpdateZoomText();
	}

	private void Update()
	{
		if (IsReputKeys)
		{
			PutKeys();
		}
		if (isPlay && nowTime < endTime)
		{
			nowTime += Time.deltaTime;
			if (nowTime >= endTime)
			{
				nowTime = endTime;
				isPlay = false;
			}
		}
		invoke = false;
		if (endTime > 0f)
		{
			timeBar.value = Mathf.InverseLerp(0f, endTime, nowTime);
		}
		else
		{
			timeBar.value = 0f;
		}
		if (!nowTimeInput.isFocused)
		{
			nowTimeInput.text = nowTime.ToString("000.00");
		}
		UpdateNowTick();
		int num = -1;
		for (int i = 0; i < keys.Count; i++)
		{
			if (keys[i].toggle.isOn)
			{
				num = i;
				break;
			}
		}
		if (num != ActiveKey)
		{
			ActiveKey = num;
			if (ActiveKey == -1)
			{
				edit.command.ClearList();
			}
			else
			{
				float time = keys[ActiveKey].time;
				nowTime = time;
				CheckNowTime();
			}
		}
		invoke = true;
		if (draggingKey != null)
		{
			KeyDrag();
		}
	}

	public void SetPlay(bool flag)
	{
		isPlay = flag;
		if (isPlay)
		{
			edit.cutScene.IsPlay = true;
		}
	}

	public void ZoomIn()
	{
		zoom += 1f;
		zoom = Mathf.Clamp(zoom, 1f, 100f);
		UpdateZoomText();
		IsReputKeys = true;
	}

	public void ZoomOut()
	{
		zoom -= 1f;
		zoom = Mathf.Clamp(zoom, 1f, 100f);
		UpdateZoomText();
		IsReputKeys = true;
	}

	public void ZoomScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		zoom += pointerEventData.scrollDelta.y;
		zoom = Mathf.Clamp(zoom, 1f, 100f);
		UpdateZoomText();
		IsReputKeys = true;
	}

	private void UpdateZoomText()
	{
		zoomText.text = "x" + zoom.ToString("0.0");
	}

	public void SetEndTime(string str)
	{
		endTime = float.Parse(str);
		IsReputKeys = true;
	}

	public void SetNowTime(string str)
	{
		nowTime = float.Parse(str);
		CheckNowTime();
	}

	public void SetNowTime(float time)
	{
		nowTime = time;
		CheckNowTime();
	}

	public void SetNowTimeRate(float rate)
	{
		nowTime = endTime * rate;
		CheckNowTime();
	}

	private void CheckNowTime()
	{
		edit.command.SetList(nowTime);
		edit.cutScene.Check(nowTime);
		int num = -1;
		for (int i = 0; i < keys.Count; i++)
		{
			if (keys[i].time == nowTime)
			{
				num = i;
			}
		}
		if (num != ActiveKey)
		{
			invoke = false;
			if (num == -1)
			{
				keys[ActiveKey].toggle.isOn = false;
			}
			else
			{
				keys[num].toggle.isOn = true;
			}
			ActiveKey = num;
			invoke = true;
		}
	}

	public void ReputKeys(bool forced)
	{
		ActiveKey = -1;
		IsReputKeys = true;
		if (forced)
		{
			PutKeys();
		}
	}

	private void TimeEpsilon(ref float time)
	{
		int num = (int)time;
		int num2 = (int)(time % 1f * 100f);
		time = (float)num + (float)num2 * 0.01f;
	}

	private void PutKeys()
	{
		for (int i = 0; i < keys.Count; i++)
		{
            UnityEngine.Object.Destroy(keys[i].toggle.gameObject);
		}
		keys.Clear();
		for (int j = 0; j < ticks.Count; j++)
		{
            UnityEngine.Object.Destroy(ticks[j]);
		}
		ticks.Clear();
		float num = 10f;
		float num2 = keysMaxW * zoom;
		Vector2 sizeDelta = keysArea.sizeDelta;
		sizeDelta.x = num2 + num * 2f;
		keysArea.sizeDelta = sizeDelta;
		float num3 = -1f;
		float b = endTime;
		for (int k = 0; k < edit.cutScene.Actions.Count; k++)
		{
			float time = edit.cutScene.Actions[k].time;
			int count = keys.Count;
			if (num3 != time)
			{
				num3 = time;
				float num4 = Mathf.InverseLerp(0f, b, time);
				Vector3 zero = Vector3.zero;
				zero.x = num + num4 * num2;
				zero.y = 0f;
				CutSceneEdit_Key cutSceneEdit_Key = UnityEngine.Object.Instantiate(keyOriginal);
				cutSceneEdit_Key.Setup(this, count, time);
				cutSceneEdit_Key.AddAction(edit.cutScene.Actions[k]);
				cutSceneEdit_Key.gameObject.name = "Key_" + count;
				cutSceneEdit_Key.gameObject.SetActive(true);
				RectTransform rectTransform = cutSceneEdit_Key.transform as RectTransform;
				rectTransform.SetParent(keysParent, false);
				rectTransform.anchoredPosition = zero;
				keys.Add(cutSceneEdit_Key);
			}
			else
			{
				keys[count - 1].AddAction(edit.cutScene.Actions[k]);
			}
		}
		if (tickNum > 0f)
		{
			float num5 = CalcTickTime();
			for (num3 = 0f; num3 <= endTime; num3 += num5)
			{
				float num6 = Mathf.InverseLerp(0f, b, num3);
				Vector3 zero2 = Vector3.zero;
				zero2.x = num + num6 * num2;
				zero2.y = 0f;
				RectTransform rectTransform2 = UnityEngine.Object.Instantiate(tickOriginal);
				rectTransform2.gameObject.SetActive(true);
				rectTransform2.SetParent(ticksParent, false);
				rectTransform2.anchoredPosition = zero2;
				rectTransform2.GetComponentInChildren<Text>().text = num3.ToString("0.0");
				ticks.Add(rectTransform2.gameObject);
			}
		}
		if (ActiveKey != -1)
		{
			keys[ActiveKey].toggle.isOn = true;
		}
		IsReputKeys = false;
	}

	private void UpdateNowTick()
	{
		float num = 10f;
		float num2 = keysMaxW * zoom;
		float b = endTime;
		float num3 = Mathf.InverseLerp(0f, b, nowTime);
		Vector3 zero = Vector3.zero;
		zero.x = num + num3 * num2;
		zero.y = 0f;
		nowTick.anchoredPosition = zero;
		nowTickText.text = nowTime.ToString("000.00");
	}

	private float CalcTickTime()
	{
		float num = endTime / zoom;
		float num2 = num / tickNum;
		if (num2 >= 1f)
		{
			int num3 = 0;
			float num4 = num;
			while (num4 > 10f)
			{
				num4 /= 10f;
				num3++;
			}
			num2 /= Mathf.Pow(10f, num3);
			num2 = ((num2 < 0.25f) ? 0.25f : ((!(num2 < 0.75f)) ? 1f : 0.5f));
			return num2 * Mathf.Pow(10f, num3);
		}
		if (num2 < 0.1f)
		{
			return 0.1f;
		}
		if (num2 < 0.25f)
		{
			return 0.5f;
		}
		if (num2 < 0.75f)
		{
			return 0.5f;
		}
		return 1f;
	}

	public void TimeBarMouseControl(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		Vector2 position = pointerEventData.position;
		if (pointerEventData.button == PointerEventData.InputButton.Left)
		{
			float a = keysParent.position.x + keysParent.rect.xMin + 10f;
			float b = keysParent.position.x + keysParent.rect.xMax - 10f;
			float value = Mathf.InverseLerp(a, b, position.x);
			value = Mathf.Clamp01(value);
			nowTime = endTime * value;
			CheckNowTime();
		}
		if (pointerEventData.button == PointerEventData.InputButton.Middle)
		{
			float num = 1f / scrollRect.content.rect.width;
			float num2 = pointerEventData.delta.x * num;
			scrollRect.horizontalScrollbar.value -= num2;
		}
	}

	public void KeyDrag()
	{
		Vector2 vector = Input.mousePosition;
		float a = keysParent.position.x + keysParent.rect.xMin + 10f;
		float b = keysParent.position.x + keysParent.rect.xMax - 10f;
		float value = Mathf.InverseLerp(a, b, vector.x);
		value = Mathf.Clamp01(value);
		float time = value * endTime;
		TimeEpsilon(ref time);
		draggingKey.time = time;
		float num = 10f;
		float num2 = keysMaxW * zoom;
		Vector2 anchoredPosition = vector;
		anchoredPosition.x = num + value * num2;
		anchoredPosition.y = 0f;
		RectTransform rectTransform = draggingKey.transform as RectTransform;
		rectTransform.anchoredPosition = anchoredPosition;
	}

	public void BeginKeyDrag(int id)
	{
		draggingKey = keys[id];
	}

	public void EndKeyDrag(int id)
	{
		if (draggingKey == keys[id])
		{
			float time = draggingKey.time;
			draggingKey.ChangeActionTime(time);
			draggingKey = null;
			edit.cutScene.SortActions();
			IsReputKeys = true;
		}
	}
}
