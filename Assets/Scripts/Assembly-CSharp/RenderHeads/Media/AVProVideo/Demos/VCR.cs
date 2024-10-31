using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class VCR : MonoBehaviour
	{
		public MediaPlayer _mediaPlayer;

		public MediaPlayer _mediaPlayerB;

		public DisplayUGUI _mediaDisplay;

		public Slider _videoSeekSlider;

		private float _setVideoSeekSliderValue;

		private bool _wasPlayingOnScrub;

		public Slider _audioVolumeSlider;

		private float _setAudioVolumeSliderValue;

		public Toggle _AutoStartToggle;

		public Toggle _MuteToggle;

		public MediaPlayer.FileLocation _location = MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder;

		public string _folder = "AVProVideoDemos/";

		public string[] _videoFiles = new string[2] { "BigBuckBunny_720p30.mp4", "SampleSphere.mp4" };

		private int _VideoIndex;

		private MediaPlayer _loadingPlayer;

		public MediaPlayer PlayingPlayer
		{
			get
			{
				if (LoadingPlayer == _mediaPlayer)
				{
					return _mediaPlayerB;
				}
				return _mediaPlayer;
			}
		}

		public MediaPlayer LoadingPlayer
		{
			get
			{
				return _loadingPlayer;
			}
		}

		private void SwapPlayers()
		{
			PlayingPlayer.Control.Pause();
			if (LoadingPlayer == _mediaPlayer)
			{
				_loadingPlayer = _mediaPlayerB;
			}
			else
			{
				_loadingPlayer = _mediaPlayer;
			}
			_mediaDisplay.CurrentMediaPlayer = PlayingPlayer;
		}

		public void OnOpenVideoFile()
		{
			LoadingPlayer.m_VideoPath = Path.Combine(_folder, _videoFiles[_VideoIndex]);
			_VideoIndex = (_VideoIndex + 1) % _videoFiles.Length;
			if (string.IsNullOrEmpty(LoadingPlayer.m_VideoPath))
			{
				LoadingPlayer.CloseVideo();
				_VideoIndex = 0;
			}
			else
			{
				LoadingPlayer.OpenVideoFromFile(_location, LoadingPlayer.m_VideoPath, _AutoStartToggle.isOn);
			}
		}

		public void OnAutoStartChange()
		{
			if ((bool)PlayingPlayer && (bool)_AutoStartToggle && _AutoStartToggle.enabled && PlayingPlayer.m_AutoStart != _AutoStartToggle.isOn)
			{
				PlayingPlayer.m_AutoStart = _AutoStartToggle.isOn;
			}
			if ((bool)LoadingPlayer && (bool)_AutoStartToggle && _AutoStartToggle.enabled && LoadingPlayer.m_AutoStart != _AutoStartToggle.isOn)
			{
				LoadingPlayer.m_AutoStart = _AutoStartToggle.isOn;
			}
		}

		public void OnMuteChange()
		{
			if ((bool)PlayingPlayer)
			{
				PlayingPlayer.Control.MuteAudio(_MuteToggle.isOn);
			}
			if ((bool)LoadingPlayer)
			{
				LoadingPlayer.Control.MuteAudio(_MuteToggle.isOn);
			}
		}

		public void OnPlayButton()
		{
			if ((bool)PlayingPlayer)
			{
				PlayingPlayer.Control.Play();
			}
		}

		public void OnPauseButton()
		{
			if ((bool)PlayingPlayer)
			{
				PlayingPlayer.Control.Pause();
			}
		}

		public void OnVideoSeekSlider()
		{
			if ((bool)PlayingPlayer && (bool)_videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue)
			{
				PlayingPlayer.Control.Seek(_videoSeekSlider.value * PlayingPlayer.Info.GetDurationMs());
			}
		}

		public void OnVideoSliderDown()
		{
			if ((bool)PlayingPlayer)
			{
				_wasPlayingOnScrub = PlayingPlayer.Control.IsPlaying();
				if (_wasPlayingOnScrub)
				{
					PlayingPlayer.Control.Pause();
				}
				OnVideoSeekSlider();
			}
		}

		public void OnVideoSliderUp()
		{
			if ((bool)PlayingPlayer && _wasPlayingOnScrub)
			{
				PlayingPlayer.Control.Play();
				_wasPlayingOnScrub = false;
			}
		}

		public void OnAudioVolumeSlider()
		{
			if ((bool)PlayingPlayer && (bool)_audioVolumeSlider && _audioVolumeSlider.value != _setAudioVolumeSliderValue)
			{
				PlayingPlayer.Control.SetVolume(_audioVolumeSlider.value);
			}
			if ((bool)LoadingPlayer && (bool)_audioVolumeSlider && _audioVolumeSlider.value != _setAudioVolumeSliderValue)
			{
				LoadingPlayer.Control.SetVolume(_audioVolumeSlider.value);
			}
		}

		public void OnRewindButton()
		{
			if ((bool)PlayingPlayer)
			{
				PlayingPlayer.Control.Rewind();
			}
		}

		private void Awake()
		{
			_loadingPlayer = _mediaPlayerB;
		}

		private void Start()
		{
			if ((bool)PlayingPlayer)
			{
				PlayingPlayer.Events.AddListener(OnVideoEvent);
				if ((bool)LoadingPlayer)
				{
					LoadingPlayer.Events.AddListener(OnVideoEvent);
				}
				if ((bool)_audioVolumeSlider && PlayingPlayer.Control != null)
				{
					float value = (_setAudioVolumeSliderValue = PlayingPlayer.Control.GetVolume());
					_audioVolumeSlider.value = value;
				}
				_AutoStartToggle.isOn = PlayingPlayer.m_AutoStart;
				if (PlayingPlayer.m_AutoOpen)
				{
				}
				OnOpenVideoFile();
			}
		}

		private void Update()
		{
			if ((bool)PlayingPlayer && PlayingPlayer.Info != null && PlayingPlayer.Info.GetDurationMs() > 0f)
			{
				float currentTimeMs = PlayingPlayer.Control.GetCurrentTimeMs();
				float durationMs = PlayingPlayer.Info.GetDurationMs();
				float value = (_setVideoSeekSliderValue = Mathf.Clamp(currentTimeMs / durationMs, 0f, 1f));
				_videoSeekSlider.value = value;
			}
		}

		public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
		{
			switch (et)
			{
			case MediaPlayerEvent.EventType.FirstFrameReady:
				SwapPlayers();
				break;
			}
			Debug.Log("Event: " + et);
		}
	}
}
