using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	public abstract class BaseMediaPlayer : IMediaPlayer, IMediaControl, IMediaInfo, IMediaProducer, IMediaSubtitles, IDisposable
	{
		protected string _playerDescription = string.Empty;

		protected ErrorCode _lastError;

		protected FilterMode _defaultTextureFilterMode = FilterMode.Bilinear;

		protected TextureWrapMode _defaultTextureWrapMode = TextureWrapMode.Clamp;

		protected int _defaultTextureAnisoLevel = 1;

		protected List<Subtitle> _subtitles;

		protected Subtitle _currentSubtitle;

		public abstract string GetVersion();

		public abstract bool OpenVideoFromFile(string path, long offset, string httpHeaderJson);

		public virtual bool OpenVideoFromBuffer(byte[] buffer)
		{
			return false;
		}

		public abstract void CloseVideo();

		public abstract void SetLooping(bool bLooping);

		public abstract bool IsLooping();

		public abstract bool HasMetaData();

		public abstract bool CanPlay();

		public abstract void Play();

		public abstract void Pause();

		public abstract void Stop();

		public abstract void Rewind();

		public abstract void Seek(float timeMs);

		public abstract void SeekFast(float timeMs);

		public abstract float GetCurrentTimeMs();

		public abstract float GetPlaybackRate();

		public abstract void SetPlaybackRate(float rate);

		public abstract float GetDurationMs();

		public abstract int GetVideoWidth();

		public abstract int GetVideoHeight();

		public abstract float GetVideoDisplayRate();

		public abstract bool HasAudio();

		public abstract bool HasVideo();

		public abstract bool IsSeeking();

		public abstract bool IsPlaying();

		public abstract bool IsPaused();

		public abstract bool IsFinished();

		public abstract bool IsBuffering();

		public virtual int GetTextureCount()
		{
			return 1;
		}

		public abstract Texture GetTexture(int index = 0);

		public abstract int GetTextureFrameCount();

		public virtual long GetTextureTimeStamp()
		{
			return long.MinValue;
		}

		public abstract bool RequiresVerticalFlip();

		public virtual float[] GetTextureTransform()
		{
			return new float[6] { 1f, 0f, 0f, 1f, 0f, 0f };
		}

		public abstract void MuteAudio(bool bMuted);

		public abstract bool IsMuted();

		public abstract void SetVolume(float volume);

		public virtual void SetBalance(float balance)
		{
		}

		public abstract float GetVolume();

		public virtual float GetBalance()
		{
			return 0f;
		}

		public abstract int GetAudioTrackCount();

		public abstract int GetCurrentAudioTrack();

		public abstract void SetAudioTrack(int index);

		public abstract string GetCurrentAudioTrackId();

		public abstract int GetCurrentAudioTrackBitrate();

		public virtual int GetNumAudioChannels()
		{
			return -1;
		}

		public abstract int GetVideoTrackCount();

		public abstract int GetCurrentVideoTrack();

		public abstract void SetVideoTrack(int index);

		public abstract string GetCurrentVideoTrackId();

		public abstract int GetCurrentVideoTrackBitrate();

		public abstract float GetVideoFrameRate();

		public abstract float GetBufferingProgress();

		public abstract void Update();

		public abstract void Render();

		public abstract void Dispose();

		public ErrorCode GetLastError()
		{
			return _lastError;
		}

		public string GetPlayerDescription()
		{
			return _playerDescription;
		}

		public virtual bool PlayerSupportsLinearColorSpace()
		{
			return true;
		}

		public virtual int GetBufferedTimeRangeCount()
		{
			return 0;
		}

		public virtual bool GetBufferedTimeRange(int index, ref float startTimeMs, ref float endTimeMs)
		{
			return false;
		}

		public void SetTextureProperties(FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, int anisoLevel = 0)
		{
			_defaultTextureFilterMode = filterMode;
			_defaultTextureWrapMode = wrapMode;
			_defaultTextureAnisoLevel = anisoLevel;
			ApplyTextureProperties(GetTexture());
		}

		protected virtual void ApplyTextureProperties(Texture texture)
		{
			if (texture != null)
			{
				texture.filterMode = _defaultTextureFilterMode;
				texture.wrapMode = _defaultTextureWrapMode;
				texture.anisoLevel = _defaultTextureAnisoLevel;
			}
		}

		public virtual void GrabAudio(float[] buffer, int floatCount, int channelCount)
		{
		}

		public virtual bool IsPlaybackStalled()
		{
			return false;
		}

		public bool LoadSubtitlesSRT(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				_subtitles = null;
				_currentSubtitle = null;
			}
			else
			{
				_subtitles = Helper.LoadSubtitlesSRT(data);
			}
			return _subtitles != null;
		}

		public virtual void UpdateSubtitles()
		{
			if (_subtitles == null)
			{
				return;
			}
			float currentTimeMs = GetCurrentTimeMs();
			int num = 0;
			if (_currentSubtitle != null && !_currentSubtitle.IsTime(currentTimeMs))
			{
				if (currentTimeMs > (float)_currentSubtitle.timeEndMs)
				{
					num = _currentSubtitle.index + 1;
				}
				_currentSubtitle = null;
			}
			if (_currentSubtitle != null)
			{
				return;
			}
			for (int i = num; i < _subtitles.Count; i++)
			{
				if (_subtitles[i].IsTime(currentTimeMs))
				{
					_currentSubtitle = _subtitles[i];
					break;
				}
			}
		}

		public virtual int GetSubtitleIndex()
		{
			int result = -1;
			if (_currentSubtitle != null)
			{
				result = _currentSubtitle.index;
			}
			return result;
		}

		public virtual string GetSubtitleText()
		{
			string result = string.Empty;
			if (_currentSubtitle != null)
			{
				result = _currentSubtitle.text;
			}
			return result;
		}

		public virtual void OnEnable()
		{
		}
	}
}
