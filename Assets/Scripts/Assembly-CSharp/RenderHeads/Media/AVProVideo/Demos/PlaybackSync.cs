using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class PlaybackSync : MonoBehaviour
	{
		private enum State
		{
			Loading = 0,
			Playing = 1,
			Finished = 2
		}

		public MediaPlayer _masterPlayer;

		public MediaPlayer[] _slavePlayers;

		public float _toleranceMs = 30f;

		public bool _matchVideo = true;

		public bool _muteSlaves = true;

		private State _state;

		private void Start()
		{
			for (int i = 0; i < _slavePlayers.Length; i++)
			{
				_slavePlayers[i].m_Muted = true;
				if (_matchVideo)
				{
					_slavePlayers[i].OpenVideoFromFile(_masterPlayer.m_VideoLocation, _masterPlayer.m_VideoPath, false);
				}
			}
		}

		private void LateUpdate()
		{
			if (_state == State.Loading && IsAllVideosLoaded())
			{
				_masterPlayer.Play();
				for (int i = 0; i < _slavePlayers.Length; i++)
				{
					_slavePlayers[i].Play();
				}
				_state = State.Playing;
			}
			if (_state == State.Finished)
			{
				Debug.Log("Do Something");
			}
			else
			{
				if (_state != State.Playing)
				{
					return;
				}
				if (_masterPlayer.Control.IsPlaying())
				{
					float currentTimeMs = _masterPlayer.Control.GetCurrentTimeMs();
					for (int j = 0; j < _slavePlayers.Length; j++)
					{
						MediaPlayer mediaPlayer = _slavePlayers[j];
						float currentTimeMs2 = mediaPlayer.Control.GetCurrentTimeMs();
						float num = Mathf.Abs(currentTimeMs - currentTimeMs2);
						if (num > _toleranceMs)
						{
							mediaPlayer.Control.SeekFast(currentTimeMs + _toleranceMs * 0.5f);
							if (mediaPlayer.Control.IsPaused())
							{
								mediaPlayer.Play();
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < _slavePlayers.Length; k++)
					{
						MediaPlayer mediaPlayer2 = _slavePlayers[k];
						mediaPlayer2.Pause();
					}
				}
				if (IsPlaybackFinished(_masterPlayer))
				{
					_state = State.Finished;
				}
			}
		}

		private bool IsAllVideosLoaded()
		{
			bool result = false;
			if (IsVideoLoaded(_masterPlayer))
			{
				result = true;
				for (int i = 0; i < _slavePlayers.Length; i++)
				{
					if (!IsVideoLoaded(_slavePlayers[i]))
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		private static bool IsVideoLoaded(MediaPlayer player)
		{
			return player != null && player.Control != null && player.Control.HasMetaData() && player.Control.CanPlay() && player.TextureProducer.GetTextureFrameCount() > 0;
		}

		private static bool IsPlaybackFinished(MediaPlayer player)
		{
			bool result = false;
			if (player != null && player.Control != null && player.Control.IsFinished())
			{
				result = true;
			}
			return result;
		}
	}
}
