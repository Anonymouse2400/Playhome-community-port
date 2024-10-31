using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace H
{
	public class MixController : H_InputBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IScrollHandler, IEventSystemHandler
	{
		public enum MODE
		{
			MANUAL = 0,
			SEMI_AUTO = 1,
			FULL_AUTO = 2,
			NUM = 3
		}

		[SerializeField]
		private H_Scene h_scene;

		[SerializeField]
		private RectTransform area;

		[SerializeField]
		private Image pointImg;

		[SerializeField]
		private Image hitAreaImg;

		[SerializeField]
		private Image echoImg;

		[SerializeField]
		private Text text;

		[SerializeField]
		private float moveRate = 1f;

		[SerializeField]
		private GameObject sliderRoot;

		private Slider slider;

		[SerializeField]
		private float wheelSpeed = 0.1f;

		[SerializeField]
		private Toggle[] toggles;

		private bool isDragging;

		private AutoMode autoMode = new AutoMode();

		[SerializeField]
		[Range(0f, 1f)]
		private float autoChangeSpeed = 0.1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float autoDistanceThreshold = 0.2f;

		private bool dragEnable;

		private HitArea hitArea;

		private AreaEcho areaEcho;

		public MODE mode { get; private set; }

		private void Start()
		{
			mode = MODE.MANUAL;
			slider = sliderRoot.GetComponentInChildren<Slider>();
			hitArea = new HitArea(hitAreaImg, area);
			areaEcho = new AreaEcho(echoImg, area);
		}

		public void ChangeStyle()
		{
			hitArea.Setup(h_scene.mainMembers.HasHitArea());
		}

		protected override void ChangeState(H_STATE state)
		{
			base.ChangeState(state);
			string text = string.Empty;
			pose = 1f;
			stroke = -1f;
			Vector2 anchoredPosition = new Vector2(pose * area.rect.xMax, stroke * area.rect.yMax);
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, area.rect.xMin, area.rect.xMax);
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, area.rect.yMin, area.rect.yMax);
			pointImg.rectTransform.anchoredPosition = anchoredPosition;
			autoMode.StartWander(new Vector2(pose, stroke), AutoGageVal(), autoDistanceThreshold);
			switch (state)
			{
			case H_STATE.PRE_INSERT_WAIT:
				text = "Click\nInsert\n";
				break;
			case H_STATE.PRE_TOUCH_WAIT:
				text = ((members.StyleData.type != H_StyleData.TYPE.PETTING) ? "Click\nto\nstart service" : "Click\nto\nstart caressing");
				break;
			case H_STATE.INSERTED_WAIT:
				text = "Click\n\nto start the piston.";

                break;
			case H_STATE.LOOP:
				text = "Drag\nto\nchange posture";
				break;
			case H_STATE.IN_XTC_AFTER_WAIT:
				text = "Click\nto\nresume";
				break;
			case H_STATE.OUT_XTC_AFTER_WAIT:
				text = ((members.StyleData.type != 0) ? "Click\nto\nresume" : "Click\nto\nreinsert");
				break;
			case H_STATE.EXTRACTED_WAIT:
			case H_STATE.COUGH_WAIT:
			case H_STATE.VOMIT_WAIT:
			case H_STATE.DRINK_WAIT:
				text = "Click\nto\nreinsert";
				break;
			}
			dragEnable = state == H_STATE.LOOP;
			SetText(text);
			if (mode == MODE.FULL_AUTO && (state == H_STATE.PRE_INSERT_WAIT || state == H_STATE.PRE_TOUCH_WAIT || state == H_STATE.INSERTED_WAIT || state == H_STATE.LOOP))
			{
				((Behaviour)(object)this.text).enabled = false;
			}
		}

		public override void Update()
		{
			base.Update();
			if (members == null)
			{
				return;
			}
			for (int i = 0; i < toggles.Length; i++)
			{
				if (toggles[i].isOn && mode != (MODE)i)
				{
					ChangeMode((MODE)i);
					break;
				}
			}
			if (isDragging && !Input.GetMouseButton(0))
			{
				isDragging = false;
			}
			if (!dragEnable)
			{
				isDragging = false;
			}
			if (mode == MODE.FULL_AUTO && dragEnable)
			{
				float animeSpeed = h_scene.mainMembers.GetFemale(0).body.Anime.speed;
				autoMode.Update(animeSpeed, autoChangeSpeed, AutoGageVal(), autoDistanceThreshold);
				pose = autoMode.nowPos.x;
				stroke = autoMode.nowPos.y;
				Vector2 anchoredPosition = new Vector2(pose * area.rect.xMax, stroke * area.rect.yMax);
				anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, area.rect.xMin, area.rect.xMax);
				anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, area.rect.yMin, area.rect.yMax);
				pointImg.rectTransform.anchoredPosition = anchoredPosition;
			}
			else if (isDragging)
			{
				Vector2 vector = Vector3.zero;
				vector.x = Input.GetAxis("Mouse X");
				vector.y = Input.GetAxis("Mouse Y");
				vector *= moveRate;
				Vector2 anchoredPosition2 = pointImg.rectTransform.anchoredPosition + vector;
				anchoredPosition2.x = Mathf.Clamp(anchoredPosition2.x, area.rect.xMin, area.rect.xMax);
				anchoredPosition2.y = Mathf.Clamp(anchoredPosition2.y, area.rect.yMin, area.rect.yMax);
				pointImg.rectTransform.anchoredPosition = anchoredPosition2;
				float t = Mathf.InverseLerp(area.rect.xMin, area.rect.xMax, anchoredPosition2.x);
				float t2 = Mathf.InverseLerp(area.rect.yMin, area.rect.yMax, anchoredPosition2.y);
				pose = Mathf.Lerp(-1f, 1f, t);
				stroke = Mathf.Lerp(-1f, 1f, t2);
				GameCursor.Lock();
			}
			if (mode == MODE.MANUAL)
			{
				bool interactable = slider.interactable;
				bool flag = (bool)EventSystem.current && EventSystem.current.IsPointerOverGameObject();
				if (slider.image.canvas != null && slider.image.canvas.enabled && !flag)
				{
					slider.value += Input.mouseScrollDelta.y * wheelSpeed;
				}
			}
			else
			{
				slider.value = stroke;
			}
			pointImg.enabled = dragEnable;
			members.param.hit = false;
			members.param.hitEnableStyle = hitArea.Enable;
			if (dragEnable)
			{
				if (!areaEcho.show && hitArea.Check(new Vector2(pose, stroke)))
				{
					areaEcho.Set(pointImg.rectTransform.anchoredPosition);
				}
				members.param.hit = areaEcho.show;
			}
			if (areaEcho.show)
			{
				areaEcho.Update();
			}
			hitArea.SetShow(dragEnable && GlobalData.PlayData.unlockShowHitArea);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!isDragging)
			{
				h_scene.OnInput(H_INPUT.CLICK_PAD);
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (dragEnable)
			{
				isDragging = true;
				GameCursor.Lock();
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (dragEnable)
			{
				GameCursor.Lock();
				isDragging = true;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			isDragging = false;
		}

		private void SetText(string str)
		{
			((Behaviour)(object)text).enabled = str != null && str.Length > 0;
			text.text = str;
		}

		public void ChangeSliderSpeed(float val)
		{
			speed = val;
		}

		public void ChangeMode(MODE newMode)
		{
			mode = newMode;
			sliderRoot.SetActive(mode == MODE.MANUAL);
			if (newMode == MODE.FULL_AUTO)
			{
				autoMode.StartWander(new Vector2(pose, stroke), AutoGageVal(), autoDistanceThreshold);
				((Behaviour)(object)text).enabled = !dragEnable;
			}
			else
			{
				((Behaviour)(object)text).enabled = true;
			}
		}

		public void OnScroll(PointerEventData eventData)
		{
			if (mode == MODE.MANUAL)
			{
				bool interactable = slider.interactable;
				if (slider.image.canvas != null && slider.image.canvas.enabled)
				{
					slider.value += eventData.scrollDelta.y * wheelSpeed;
				}
			}
		}

		private float AutoGageVal()
		{
			if (h_scene.mainMembers.StyleData != null && h_scene.mainMembers.StyleData.type != H_StyleData.TYPE.SERVICE)
			{
				return h_scene.mainMembers.FemaleGageVal;
			}
			return 0.5f;
		}
	}
}
