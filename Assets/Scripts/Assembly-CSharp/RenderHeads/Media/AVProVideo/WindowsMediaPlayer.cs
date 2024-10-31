using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	public sealed class WindowsMediaPlayer : BaseMediaPlayer
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct Native
		{
			public enum RenderThreadEvent
			{
				UpdateAllTextures = 0,
				FreeTextures = 1
			}

			public const int PluginID = 262537216;

			[DllImport("AVProVideo")]
			public static extern bool Init(bool linearColorSpace, bool isD3D11NoSingleThreaded);

			[DllImport("AVProVideo")]
			public static extern void Deinit();

			[DllImport("AVProVideo")]
			public static extern IntPtr GetPluginVersion();

			[DllImport("AVProVideo")]
			public static extern bool IsTrialVersion();

			[DllImport("AVProVideo")]
			public static extern IntPtr OpenSource(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string path, int videoApiIndex, bool useHardwareDecoding, bool generateTextureMips, [MarshalAs(UnmanagedType.LPWStr)] string forceAudioOutputDeviceName, bool useUnityAudio, bool forceResample, int channels);

			[DllImport("AVProVideo")]
			public static extern IntPtr OpenSourceFromBuffer(IntPtr instance, byte[] buffer, ulong bufferLength, int videoApiIndex, bool useHardwareDecoding, bool generateTextureMips, [MarshalAs(UnmanagedType.LPWStr)] string forceAudioOutputDeviceName, bool useUnityAudio);

			[DllImport("AVProVideo")]
			public static extern void CloseSource(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern IntPtr GetPlayerDescription(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern int GetLastErrorCode(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern void Play(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern void Pause(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern void SetMuted(IntPtr instance, bool muted);

			[DllImport("AVProVideo")]
			public static extern void SetVolume(IntPtr instance, float volume);

			[DllImport("AVProVideo")]
			public static extern void SetBalance(IntPtr instance, float volume);

			[DllImport("AVProVideo")]
			public static extern void SetLooping(IntPtr instance, bool looping);

			[DllImport("AVProVideo")]
			public static extern bool HasVideo(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool HasAudio(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern int GetWidth(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern int GetHeight(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern float GetFrameRate(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern float GetDuration(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern int GetAudioTrackCount(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool IsPlaybackStalled(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool HasMetaData(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool CanPlay(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool IsSeeking(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool IsFinished(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool IsBuffering(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern float GetCurrentTime(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern void SetCurrentTime(IntPtr instance, float time, bool fast);

			[DllImport("AVProVideo")]
			public static extern float GetPlaybackRate(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern void SetPlaybackRate(IntPtr instance, float rate);

			[DllImport("AVProVideo")]
			public static extern int GetAudioTrack(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern void SetAudioTrack(IntPtr instance, int index);

			[DllImport("AVProVideo")]
			public static extern float GetBufferingProgress(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern int GetBufferedRanges(IntPtr instance, float[] timeArray, int arrayCount);

			[DllImport("AVProVideo")]
			public static extern void Update(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern IntPtr GetTexturePointer(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern bool IsTextureTopDown(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern int GetTextureFrameCount(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern long GetTextureTimeStamp(IntPtr instance);

			[DllImport("AVProVideo")]
			public static extern IntPtr GetRenderEventFunc_UpdateAllTextures();

			[DllImport("AVProVideo")]
			public static extern IntPtr GetRenderEventFunc_FreeTextures();

			[DllImport("AVProVideo")]
			public static extern void SetUnityAudioEnabled(IntPtr instance, bool enabled);

			[DllImport("AVProVideo")]
			public static extern void GrabAudio(IntPtr instance, float[] buffer, int floatCount, int channelCount);

			[DllImport("AVProVideo")]
			public static extern int GetAudioChannelCount(IntPtr instance);
		}

		private bool _forceAudioResample = true;

		private int _desiredAudioChannels = 2;

		private bool _useUnityAudio;

		private string _audioDeviceOutputName = string.Empty;

		private bool _isPlaying;

		private bool _isPaused;

		private bool _audioMuted;

		private float _volume = 1f;

		private float _balance;

		private bool _bLoop;

		private bool _canPlay;

		private bool _hasMetaData;

		private int _width;

		private int _height;

		private float _frameRate;

		private bool _hasAudio;

		private bool _hasVideo;

		private bool _isTextureTopDown = true;

		private IntPtr _nativeTexture = IntPtr.Zero;

		private Texture2D _texture;

		private IntPtr _instance = IntPtr.Zero;

		private float _displayRateTimer;

		private int _lastFrameCount;

		private float _displayRate = 1f;

		private Windows.VideoApi _videoApi;

		private bool _useHardwareDecoding = true;

		private bool _useTextureMips;

		private int _queueSetAudioTrackIndex = -1;

		private bool _supportsLinearColorSpace = true;

		private int _bufferedTimeRangeCount;

		private float[] _bufferedTimeRanges = new float[0];

		private static bool _isInitialised;

		private static string _version = "Plug-in not yet initialised";

		private static IntPtr _nativeFunction_UpdateAllTextures;

		private static IntPtr _nativeFunction_FreeTextures;

		private int _textureQuality = QualitySettings.masterTextureLimit;

		public WindowsMediaPlayer(Windows.VideoApi videoApi, bool useHardwareDecoding, bool useTextureMips, string audioDeviceOutputName, bool useUnityAudio, bool forceResample, int channels)
		{
			SetOptions(videoApi, useHardwareDecoding, useTextureMips, audioDeviceOutputName, useUnityAudio, forceResample, channels);
		}

		public static void InitialisePlatform()
		{
			if (!_isInitialised)
			{
				if (!Native.Init(QualitySettings.activeColorSpace == ColorSpace.Linear, true))
				{
					Debug.LogError("[AVProVideo] Failing to initialise platform");
					return;
				}
				_version = GetPluginVersion();
				_nativeFunction_UpdateAllTextures = Native.GetRenderEventFunc_UpdateAllTextures();
				_nativeFunction_FreeTextures = Native.GetRenderEventFunc_FreeTextures();
			}
		}

		public static void DeinitPlatform()
		{
			Native.Deinit();
		}

		public override int GetNumAudioChannels()
		{
			return Native.GetAudioChannelCount(_instance);
		}

		private static int CalculateClosestViableChannelNumber(int channels)
		{
			int num = 1;
			switch (AudioSettings.speakerMode)
			{
			case AudioSpeakerMode.Mono:
				num = 1;
				break;
			case AudioSpeakerMode.Stereo:
				num = 2;
				break;
			case AudioSpeakerMode.Quad:
				num = 4;
				break;
			case AudioSpeakerMode.Mode5point1:
				num = 6;
				break;
			case AudioSpeakerMode.Surround:
				num = 5;
				break;
			case AudioSpeakerMode.Mode7point1:
				num = 8;
				break;
			default:
				num = 2;
				break;
			}
			channels = Mathf.Clamp(channels, 1, num);
			int[] array = new int[6] { 8, 6, 5, 4, 2, 1 };
			for (int i = 0; i < array.Length; i++)
			{
				if (channels >= array[i])
				{
					return array[i];
				}
			}
			return 2;
		}

		public void SetOptions(Windows.VideoApi videoApi, bool useHardwareDecoding, bool useTextureMips, string audioDeviceOutputName, bool useUnityAudio, bool forceResample, int channels)
		{
			_videoApi = videoApi;
			_useHardwareDecoding = useHardwareDecoding;
			_useTextureMips = useTextureMips;
			_audioDeviceOutputName = audioDeviceOutputName;
			_useUnityAudio = useUnityAudio;
			_forceAudioResample = forceResample;
			_desiredAudioChannels = channels;
		}

		public override string GetVersion()
		{
			return _version;
		}

		public override bool OpenVideoFromFile(string path, long offset, string httpHeaderJson)
		{
			CloseVideo();
			int channels = CalculateClosestViableChannelNumber(_desiredAudioChannels);
			_instance = Native.OpenSource(_instance, path, (int)_videoApi, _useHardwareDecoding, _useTextureMips, _audioDeviceOutputName, _useUnityAudio, _forceAudioResample, channels);
			if (_instance == IntPtr.Zero)
			{
				DisplayLoadFailureSuggestion(path);
				return false;
			}
			Native.SetUnityAudioEnabled(_instance, _useUnityAudio);
			return true;
		}

		public override bool OpenVideoFromBuffer(byte[] buffer)
		{
			CloseVideo();
			_instance = Native.OpenSourceFromBuffer(_instance, buffer, (ulong)buffer.Length, (int)_videoApi, _useHardwareDecoding, _useTextureMips, _audioDeviceOutputName, _useUnityAudio);
			if (_instance == IntPtr.Zero)
			{
				return false;
			}
			Native.SetUnityAudioEnabled(_instance, _useUnityAudio);
			return true;
		}

		private void DisplayLoadFailureSuggestion(string path)
		{
			if ((_videoApi == Windows.VideoApi.DirectShow || SystemInfo.operatingSystem.Contains("Windows 7") || SystemInfo.operatingSystem.Contains("Windows Vista") || SystemInfo.operatingSystem.Contains("Windows XP")) && path.Contains(".mp4"))
			{
				Debug.LogWarning("[AVProVideo] The native Windows DirectShow H.264 decoder doesn't support videos with resolution above 1920x1080. You may need to reduce your video resolution, switch to another codec (such as DivX or Hap), or install 3rd party DirectShow codec (eg LAV Filters).  This shouldn't be a problem for Windows 8 and above as it has a native limitation of 3840x2160.");
			}
		}

		public override void CloseVideo()
		{
			_width = 0;
			_height = 0;
			_frameRate = 0f;
			_hasAudio = (_hasVideo = false);
			_hasMetaData = false;
			_canPlay = false;
			_isPaused = false;
			_isPlaying = false;
			_bLoop = false;
			_audioMuted = false;
			_volume = 1f;
			_balance = 0f;
			_lastFrameCount = 0;
			_displayRate = 0f;
			_displayRateTimer = 0f;
			_queueSetAudioTrackIndex = -1;
			_supportsLinearColorSpace = true;
			_lastError = ErrorCode.None;
			_nativeTexture = IntPtr.Zero;
			if (_texture != null)
			{
				UnityEngine.Object.Destroy(_texture);
				_texture = null;
			}
			if (_instance != IntPtr.Zero)
			{
				Native.CloseSource(_instance);
				_instance = IntPtr.Zero;
			}
			IssueRenderThreadEvent(Native.RenderThreadEvent.FreeTextures);
		}

		public override void SetLooping(bool looping)
		{
			_bLoop = looping;
			Native.SetLooping(_instance, looping);
		}

		public override bool IsLooping()
		{
			return _bLoop;
		}

		public override bool HasMetaData()
		{
			return _hasMetaData;
		}

		public override bool HasAudio()
		{
			return _hasAudio;
		}

		public override bool HasVideo()
		{
			return _hasVideo;
		}

		public override bool CanPlay()
		{
			return _canPlay;
		}

		public override void Play()
		{
			_isPlaying = true;
			_isPaused = false;
			Native.Play(_instance);
		}

		public override void Pause()
		{
			_isPlaying = false;
			_isPaused = true;
			Native.Pause(_instance);
		}

		public override void Stop()
		{
			_isPlaying = false;
			_isPaused = false;
			Native.Pause(_instance);
		}

		public override bool IsSeeking()
		{
			return Native.IsSeeking(_instance);
		}

		public override bool IsPlaying()
		{
			return _isPlaying;
		}

		public override bool IsPaused()
		{
			return _isPaused;
		}

		public override bool IsFinished()
		{
			return Native.IsFinished(_instance);
		}

		public override bool IsBuffering()
		{
			return Native.IsBuffering(_instance);
		}

		public override float GetDurationMs()
		{
			return Native.GetDuration(_instance) * 1000f;
		}

		public override int GetVideoWidth()
		{
			return _width;
		}

		public override int GetVideoHeight()
		{
			return _height;
		}

		public override float GetVideoFrameRate()
		{
			return _frameRate;
		}

		public override float GetVideoDisplayRate()
		{
			return _displayRate;
		}

		public override Texture GetTexture(int index)
		{
			Texture result = null;
			if (Native.GetTextureFrameCount(_instance) > 0)
			{
				result = _texture;
			}
			return result;
		}

		public override int GetTextureFrameCount()
		{
			return Native.GetTextureFrameCount(_instance);
		}

		public override long GetTextureTimeStamp()
		{
			return Native.GetTextureTimeStamp(_instance);
		}

		public override bool RequiresVerticalFlip()
		{
			return _isTextureTopDown;
		}

		public override void Rewind()
		{
			Seek(0f);
		}

		public override void Seek(float timeMs)
		{
			Native.SetCurrentTime(_instance, timeMs / 1000f, false);
		}

		public override void SeekFast(float timeMs)
		{
			Native.SetCurrentTime(_instance, timeMs / 1000f, true);
		}

		public override float GetCurrentTimeMs()
		{
			return Native.GetCurrentTime(_instance) * 1000f;
		}

		public override void SetPlaybackRate(float rate)
		{
			Native.SetPlaybackRate(_instance, rate);
		}

		public override float GetPlaybackRate()
		{
			return Native.GetPlaybackRate(_instance);
		}

		public override float GetBufferingProgress()
		{
			return Native.GetBufferingProgress(_instance);
		}

		public override int GetBufferedTimeRangeCount()
		{
			return _bufferedTimeRangeCount;
		}

		public override bool GetBufferedTimeRange(int index, ref float startTimeMs, ref float endTimeMs)
		{
			bool result = false;
			if (index >= 0 && index < _bufferedTimeRangeCount)
			{
				result = true;
				startTimeMs = 1000f * _bufferedTimeRanges[index * 2];
				endTimeMs = 1000f * _bufferedTimeRanges[index * 2 + 1];
			}
			return result;
		}

		public override void MuteAudio(bool bMuted)
		{
			_audioMuted = bMuted;
			Native.SetMuted(_instance, _audioMuted);
		}

		public override bool IsMuted()
		{
			return _audioMuted;
		}

		public override void SetVolume(float volume)
		{
			_volume = volume;
			Native.SetVolume(_instance, volume);
		}

		public override float GetVolume()
		{
			return _volume;
		}

		public override void SetBalance(float balance)
		{
			_balance = balance;
			Native.SetBalance(_instance, balance);
		}

		public override float GetBalance()
		{
			return _balance;
		}

		public override int GetAudioTrackCount()
		{
			return Native.GetAudioTrackCount(_instance);
		}

		public override int GetCurrentAudioTrack()
		{
			return Native.GetAudioTrack(_instance);
		}

		public override void SetAudioTrack(int index)
		{
			_queueSetAudioTrackIndex = index;
		}

		public override int GetVideoTrackCount()
		{
			int result = 0;
			if (HasVideo())
			{
				result = 1;
			}
			return result;
		}

		public override bool IsPlaybackStalled()
		{
			return Native.IsPlaybackStalled(_instance);
		}

		public override string GetCurrentAudioTrackId()
		{
			return string.Empty;
		}

		public override int GetCurrentAudioTrackBitrate()
		{
			return 0;
		}

		public override int GetCurrentVideoTrack()
		{
			return 0;
		}

		public override void SetVideoTrack(int index)
		{
		}

		public override string GetCurrentVideoTrackId()
		{
			return string.Empty;
		}

		public override int GetCurrentVideoTrackBitrate()
		{
			return 0;
		}

		public override void Update()
		{
			Native.Update(_instance);
			_lastError = (ErrorCode)Native.GetLastErrorCode(_instance);
			if (_queueSetAudioTrackIndex >= 0 && _hasAudio)
			{
				Native.SetAudioTrack(_instance, _queueSetAudioTrackIndex);
				_queueSetAudioTrackIndex = -1;
			}
			_bufferedTimeRangeCount = Native.GetBufferedRanges(_instance, _bufferedTimeRanges, _bufferedTimeRanges.Length / 2);
			if (_bufferedTimeRangeCount > _bufferedTimeRanges.Length / 2)
			{
				_bufferedTimeRanges = new float[_bufferedTimeRangeCount * 2];
				_bufferedTimeRangeCount = Native.GetBufferedRanges(_instance, _bufferedTimeRanges, _bufferedTimeRanges.Length / 2);
			}
			UpdateSubtitles();
			if (!_canPlay)
			{
				if (!_hasMetaData && Native.HasMetaData(_instance))
				{
					if (Native.HasVideo(_instance))
					{
						_width = Native.GetWidth(_instance);
						_height = Native.GetHeight(_instance);
						_frameRate = Native.GetFrameRate(_instance);
						if (_width > 0 && _height > 0)
						{
							_hasVideo = true;
							if (Mathf.Max(_width, _height) > SystemInfo.maxTextureSize)
							{
								Debug.LogError(string.Format("[AVProVideo] Video dimensions ({0}x{1}) larger than maxTextureSize ({2} for current build target)", _width, _height, SystemInfo.maxTextureSize));
								_width = (_height = 0);
								_hasVideo = false;
							}
						}
						if (_hasVideo && Native.HasAudio(_instance))
						{
							_hasAudio = true;
						}
					}
					else if (Native.HasAudio(_instance))
					{
						_hasAudio = true;
					}
					if (_hasVideo || _hasAudio)
					{
						_hasMetaData = true;
					}
					_playerDescription = Marshal.PtrToStringAnsi(Native.GetPlayerDescription(_instance));
					_supportsLinearColorSpace = !_playerDescription.Contains("MF-MediaEngine-Hardware");
					Helper.LogInfo("Using playback path: " + _playerDescription + " (" + _width + "x" + _height + "@" + GetVideoFrameRate().ToString("F2") + ")");
					if (_hasVideo)
					{
						OnTextureSizeChanged();
					}
				}
				if (_hasMetaData)
				{
					_canPlay = Native.CanPlay(_instance);
				}
			}
			if (!_hasVideo)
			{
				return;
			}
			IntPtr texturePointer = Native.GetTexturePointer(_instance);
			if (_texture != null && _nativeTexture != IntPtr.Zero && _nativeTexture != texturePointer)
			{
				_width = Native.GetWidth(_instance);
				_height = Native.GetHeight(_instance);
				if (texturePointer == IntPtr.Zero || _width != _texture.width || _height != _texture.height)
				{
					if (_width != _texture.width || _height != _texture.height)
					{
						Helper.LogInfo("Texture size changed: " + _width + " X " + _height);
						OnTextureSizeChanged();
					}
					_nativeTexture = IntPtr.Zero;
					UnityEngine.Object.Destroy(_texture);
					_texture = null;
				}
				else if (_nativeTexture != texturePointer)
				{
					_texture.UpdateExternalTexture(texturePointer);
					_nativeTexture = texturePointer;
				}
			}
			if (_textureQuality != QualitySettings.masterTextureLimit)
			{
				if (_texture != null && _nativeTexture != IntPtr.Zero && _texture.GetNativeTexturePtr() == IntPtr.Zero)
				{
					_texture.UpdateExternalTexture(_nativeTexture);
				}
				_textureQuality = QualitySettings.masterTextureLimit;
			}
			if (_texture == null && _width > 0 && _height > 0 && texturePointer != IntPtr.Zero)
			{
				_isTextureTopDown = Native.IsTextureTopDown(_instance);
				_texture = Texture2D.CreateExternalTexture(_width, _height, TextureFormat.RGBA32, _useTextureMips, false, texturePointer);
				if (_texture != null)
				{
					_nativeTexture = texturePointer;
					ApplyTextureProperties(_texture);
				}
				else
				{
					Debug.LogError("[AVProVideo] Failed to create texture");
				}
			}
		}

		private void OnTextureSizeChanged()
		{
			if ((_width == 720 || _height == 480) && _playerDescription.Contains("DirectShow"))
			{
				Debug.LogWarning("[AVProVideo] If video fails to play then it may be due to the resolution being higher than 1920x1080 which is the limitation of the Microsoft DirectShow H.264 decoder.\nTo resolve this you can either use Windows 8 or above (and disable 'Force DirectShow' option), resize your video, use a different codec (such as Hap or DivX), or install a 3rd party H.264 decoder such as LAV Filters.");
			}
			else if ((_width > 1920 || _height > 1080) && !_playerDescription.Contains("MF-MediaEngine-Software"))
			{
			}
		}

		private void UpdateDisplayFrameRate()
		{
			_displayRateTimer += Time.deltaTime;
			if (_displayRateTimer >= 0.5f)
			{
				int textureFrameCount = Native.GetTextureFrameCount(_instance);
				_displayRate = (float)(textureFrameCount - _lastFrameCount) / _displayRateTimer;
				_displayRateTimer -= 0.5f;
				if (_displayRateTimer >= 0.5f)
				{
					_displayRateTimer = 0f;
				}
				_lastFrameCount = textureFrameCount;
			}
		}

		public override void Render()
		{
			UpdateDisplayFrameRate();
			IssueRenderThreadEvent(Native.RenderThreadEvent.UpdateAllTextures);
		}

		public override void Dispose()
		{
			CloseVideo();
		}

		public override void GrabAudio(float[] buffer, int floatCount, int channelCount)
		{
			Native.GrabAudio(_instance, buffer, floatCount, channelCount);
		}

		public override bool PlayerSupportsLinearColorSpace()
		{
			return _supportsLinearColorSpace;
		}

		private static void IssueRenderThreadEvent(Native.RenderThreadEvent renderEvent)
		{
			switch (renderEvent)
			{
			case Native.RenderThreadEvent.UpdateAllTextures:
				GL.IssuePluginEvent(_nativeFunction_UpdateAllTextures, 0);
				break;
			case Native.RenderThreadEvent.FreeTextures:
				GL.IssuePluginEvent(_nativeFunction_FreeTextures, 0);
				break;
			}
		}

		private static string GetPluginVersion()
		{
			return Marshal.PtrToStringAnsi(Native.GetPluginVersion());
		}

		public override void OnEnable()
		{
			base.OnEnable();
			if (_texture != null && _nativeTexture != IntPtr.Zero && _texture.GetNativeTexturePtr() == IntPtr.Zero)
			{
				_texture.UpdateExternalTexture(_nativeTexture);
			}
			_textureQuality = QualitySettings.masterTextureLimit;
		}
	}
}
