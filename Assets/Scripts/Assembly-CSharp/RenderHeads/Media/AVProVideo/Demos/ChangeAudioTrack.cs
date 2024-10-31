using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class ChangeAudioTrack : MonoBehaviour
	{
		public MediaPlayer _mediaPlayer;

		public int _trackIndex;

		private bool _isQueued;

		private void OnEnable()
		{
			_isQueued = true;
		}

		private void Update()
		{
			if (_isQueued && IsVideoLoaded())
			{
				DoChangeAudioTrack(_mediaPlayer, _trackIndex);
				_isQueued = false;
			}
		}

		private bool IsVideoLoaded()
		{
			return _mediaPlayer != null && _mediaPlayer.Info != null && _mediaPlayer.Control.HasMetaData();
		}

		private static bool DoChangeAudioTrack(MediaPlayer mp, int index)
		{
			bool result = false;
			int audioTrackCount = mp.Info.GetAudioTrackCount();
			if (index >= 0 && index < audioTrackCount)
			{
				mp.Control.SetAudioTrack(index);
				result = true;
			}
			else
			{
				Debug.LogWarning("[AVProVideo] Audio track index is out of range: " + index);
			}
			return result;
		}
	}
}
