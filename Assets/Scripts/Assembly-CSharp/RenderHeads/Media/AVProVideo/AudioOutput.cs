using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu("AVPro Video/Audio Output", 400)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	public class AudioOutput : MonoBehaviour
	{
		public enum AudioOutputMode
		{
			Single = 0,
			Multiple = 1
		}

		public AudioOutputMode _audioOutputMode = AudioOutputMode.Multiple;

		[SerializeField]
		private MediaPlayer _mediaPlayer;

		private AudioSource _audioSource;

		[HideInInspector]
		public int _channelMask = -1;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			ChangeMediaPlayer(_mediaPlayer);
		}

		private void OnDestroy()
		{
			ChangeMediaPlayer(null);
		}

		private void Update()
		{
			if (_mediaPlayer != null && _mediaPlayer.Control != null && _mediaPlayer.Control.IsPlaying())
			{
				ApplyAudioSettings(_mediaPlayer, _audioSource);
			}
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
			switch (et)
			{
			case MediaPlayerEvent.EventType.Closing:
				_audioSource.Stop();
				break;
			case MediaPlayerEvent.EventType.Started:
				ApplyAudioSettings(_mediaPlayer, _audioSource);
				_audioSource.Play();
				break;
			}
		}

		private static void ApplyAudioSettings(MediaPlayer player, AudioSource audioSource)
		{
			if (player != null && player.Control != null)
			{
				float volume = player.Control.GetVolume();
				bool mute = player.Control.IsMuted();
				float playbackRate = player.Control.GetPlaybackRate();
				audioSource.volume = volume;
				audioSource.mute = mute;
				audioSource.pitch = playbackRate;
			}
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			AudioOutputManager.Instance.RequestAudio(this, _mediaPlayer, data, _channelMask, channels, _audioOutputMode);
		}
	}
}
