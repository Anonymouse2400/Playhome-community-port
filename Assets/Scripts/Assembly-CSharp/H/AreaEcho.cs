using System;
using UnityEngine;
using UnityEngine.UI;

namespace H
{
	internal class AreaEcho
	{
		private Image echoImg;

		private RectTransform area;

		private float rate;

		private float maxSize = 200f;

		private float speed = 0.5f;

		public bool show { get; private set; }

		public AreaEcho(Image image, RectTransform area)
		{
			echoImg = image;
			this.area = area;
			show = false;
			echoImg.enabled = false;
		}

		public void Set(Vector2 pos)
		{
			show = true;
			rate = 0f;
			echoImg.rectTransform.anchoredPosition = pos;
			echoImg.enabled = true;
		}

		public void Update()
		{
			if (show)
			{
				rate += Time.deltaTime * speed;
				if (rate >= 1f)
				{
					show = false;
					echoImg.enabled = false;
				}
				float num = maxSize * rate;
				echoImg.rectTransform.sizeDelta = Vector2.one * num;
				Color color = echoImg.color;
				color.a = 1f - rate;
				echoImg.color = color;
			}
		}
	}
}
