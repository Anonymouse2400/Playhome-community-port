using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Subtitles uGUI", 201)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	public class SubtitlesUGUI : MonoBehaviour
	{
		[SerializeField]
		private MediaPlayer _mediaPlayer;

		[SerializeField]
		private Text _text;

		private void Start()
		{
			ChangeMediaPlayer(_mediaPlayer);
		}

		private void OnDestroy()
		{
			ChangeMediaPlayer(null);
		}

		public void ChangeMediaPlayer(MediaPlayer newPlayer)
		{
			if (_mediaPlayer != null)
			{
				_mediaPlayer.Events.RemoveListener(OnMediaPlayerEvent);
				_mediaPlayer = null;
			}
			_mediaPlayer = newPlayer;
			if (_mediaPlayer != null)
			{
				_mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
			}
		}

		private void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
		{
			if (et == MediaPlayerEvent.EventType.SubtitleChange)
			{
				string subtitleText = _mediaPlayer.Subtitles.GetSubtitleText();
				subtitleText = subtitleText.Replace("<font color=", "<color=");
				subtitleText = subtitleText.Replace("</font>", "</color>");
				subtitleText = subtitleText.Replace("<u>", string.Empty);
				subtitleText = subtitleText.Replace("</u>", string.Empty);
				_text.text = subtitleText;
			}
		}
	}
}
