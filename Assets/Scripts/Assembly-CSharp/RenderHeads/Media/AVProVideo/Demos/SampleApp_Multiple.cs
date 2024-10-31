using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class SampleApp_Multiple : MonoBehaviour
	{
		public string m_videoPath = "BigBuckBunny_360p30.mp4";

		private int m_NumVideosAdded;

		private List<DisplayUGUI> m_aAddedVideos = new List<DisplayUGUI>();

		private void Update()
		{
			foreach (DisplayUGUI aAddedVideo in m_aAddedVideos)
			{
				if (aAddedVideo.gameObject != null && !aAddedVideo.gameObject.activeSelf && aAddedVideo._mediaPlayer != null && aAddedVideo._mediaPlayer.Control != null && aAddedVideo._mediaPlayer.Control.IsPlaying())
				{
					aAddedVideo.gameObject.SetActive(true);
				}
			}
		}

		private void UpdateVideosLayout()
		{
			GameObject gameObject = GameObject.Find("Canvas/Panel");
			RectTransform rectTransform = ((!gameObject) ? null : gameObject.GetComponent<RectTransform>());
			if ((bool)rectTransform)
			{
				Vector2 sizeDelta = rectTransform.sizeDelta;
				Vector2 vector = new Vector2(sizeDelta.x * 0.5f, sizeDelta.y * 0.5f);
				int count = m_aAddedVideos.Count;
				int num = Mathf.CeilToInt(Mathf.Sqrt(count));
				float num2 = 1f / (float)num * sizeDelta.x;
				float num3 = 1f / (float)num * sizeDelta.y;
				for (int i = 0; i < count; i++)
				{
					DisplayUGUI displayUGUI = m_aAddedVideos[i];
					int num4 = i % num;
					int num5 = i / num;
					displayUGUI.rectTransform.anchoredPosition = new Vector2(num2 * (float)num4 - vector.x, num3 * (float)(-num5) + vector.y);
					displayUGUI.rectTransform.sizeDelta = new Vector2(num2, num3);
				}
			}
		}

		public void AddVideoClicked()
		{
			m_NumVideosAdded++;
			GameObject gameObject = new GameObject("AVPro MediaPlayer " + m_NumVideosAdded);
			MediaPlayer mediaPlayer = gameObject.AddComponent<MediaPlayer>();
			mediaPlayer.m_VideoPath = m_videoPath;
			mediaPlayer.m_AutoStart = true;
			mediaPlayer.m_Loop = true;
			mediaPlayer.SetGuiPositionFromVideoIndex(m_NumVideosAdded - 1);
			mediaPlayer.SetDebugGuiEnabled(m_NumVideosAdded < 5);
			GameObject gameObject2 = GameObject.Find("Canvas/Panel");
			if (gameObject2 != null)
			{
				GameObject gameObject3 = new GameObject("AVPro Video uGUI " + m_NumVideosAdded);
				gameObject3.transform.parent = gameObject2.transform;
				gameObject3.SetActive(false);
				gameObject3.AddComponent<RectTransform>();
				gameObject3.AddComponent<CanvasRenderer>();
				DisplayUGUI displayUGUI = gameObject3.AddComponent<DisplayUGUI>();
				displayUGUI._mediaPlayer = mediaPlayer;
				displayUGUI._scaleMode = ScaleMode.StretchToFill;
				displayUGUI.rectTransform.localScale = Vector3.one;
				displayUGUI.rectTransform.pivot = new Vector2(0f, 1f);
				m_aAddedVideos.Add(displayUGUI);
				UpdateVideosLayout();
			}
		}

		public void RemoveVideoClicked()
		{
			if (m_aAddedVideos.Count > 0)
			{
				int index = Random.Range(0, m_aAddedVideos.Count);
				DisplayUGUI displayUGUI = m_aAddedVideos[index];
				if (displayUGUI._mediaPlayer != null)
				{
					displayUGUI._mediaPlayer.CloseVideo();
					Object.Destroy(displayUGUI._mediaPlayer.gameObject);
					displayUGUI._mediaPlayer = null;
				}
				Object.Destroy(displayUGUI.gameObject);
				m_aAddedVideos.RemoveAt(index);
				m_NumVideosAdded--;
			}
		}

		private void OnDestroy()
		{
			foreach (DisplayUGUI aAddedVideo in m_aAddedVideos)
			{
				if ((bool)aAddedVideo._mediaPlayer)
				{
					aAddedVideo._mediaPlayer = null;
				}
			}
			m_aAddedVideos.Clear();
		}
	}
}
