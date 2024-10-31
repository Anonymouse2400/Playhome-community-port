using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class StartEndPoint : MonoBehaviour
	{
		public MediaPlayer _mediaPlayer;

		public float _startPointSeconds;

		public float _endPointSeconds;

		public bool _loop;

		[Tooltip("If looping is enabled, this is the time the video rewinds to when it reaches the end point")]
		public float _startLoopSeconds;

		private bool _isStartQueued;

		private void OnEnable()
		{
			_isStartQueued = true;
		}

		private void Update()
		{
			if (IsVideoLoaded(_mediaPlayer))
			{
				if (_isStartQueued)
				{
					DoStart(_mediaPlayer, _startPointSeconds);
					_isStartQueued = false;
				}
				else if (!_loop)
				{
					DoCheckEnd(_mediaPlayer, _endPointSeconds);
				}
				else
				{
					DoCheckLoop(_mediaPlayer, _endPointSeconds, _startLoopSeconds);
				}
			}
		}

		private static bool IsVideoLoaded(MediaPlayer mp)
		{
			return mp != null && mp.Control != null && mp.Control.HasMetaData();
		}

		private static void DoStart(MediaPlayer mp, float positionSeconds)
		{
			mp.Control.Seek(positionSeconds * 1000f);
			mp.Play();
		}

		private static void DoCheckEnd(MediaPlayer mp, float positionSeconds)
		{
			if (mp.Control.IsPlaying() && mp.Control.GetCurrentTimeMs() >= positionSeconds * 1000f)
			{
				mp.Pause();
			}
		}

		private static void DoCheckLoop(MediaPlayer mp, float positionSeconds, float positionLoop)
		{
			if (mp.Control.IsPlaying() && mp.Control.GetCurrentTimeMs() >= positionSeconds * 1000f)
			{
				mp.Control.Seek(positionLoop * 1000f);
			}
		}
	}
}
