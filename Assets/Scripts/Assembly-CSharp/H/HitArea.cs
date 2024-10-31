using System;
using UnityEngine;
using UnityEngine.UI;

namespace H
{
	internal class HitArea
	{
		private Image showImg;

		private RectTransform area;

		public Vector2 center = Vector2.zero;

		public float size = 1f;

		public bool Enable { get; private set; }

		public HitArea(Image image, RectTransform area)
		{
			showImg = image;
			this.area = area;
			showImg.enabled = false;
			Enable = false;
		}

		public void Setup(bool enable)
		{
			Enable = enable;
			if (Enable)
			{
				center.x = UnityEngine.Random.Range(-1f, 1f);
				center.y = UnityEngine.Random.Range(-1f, 1f);
				size = UnityEngine.Random.Range(0.5f, 1f);
				showImg.rectTransform.anchoredPosition = center * area.rect.xMax;
				float num = size * area.rect.xMax * 2f;
				showImg.rectTransform.sizeDelta = Vector2.one * num;
			}
		}

		public bool Check(Vector2 pos)
		{
			if (Enable)
			{
				return Vector2.Distance(center, pos) <= size;
			}
			return false;
		}

		public void SetShow(bool show)
		{
			showImg.enabled = Enable && show;
		}
	}
}
