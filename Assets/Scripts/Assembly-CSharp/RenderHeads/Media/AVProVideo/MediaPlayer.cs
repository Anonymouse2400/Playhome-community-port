using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Media Player", -100)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	public class MediaPlayer : MonoBehaviour
	{
		[Serializable]
		public class Setup
		{
			public bool displayDebugGUI;

			public bool persistent;
		}

		public enum FileLocation
		{
			AbsolutePathOrURL = 0,
			RelativeToProjectFolder = 1,
			RelativeToStreamingAssetsFolder = 2,
			RelativeToDataFolder = 3,
			RelativeToPeristentDataFolder = 4
		}

		[Serializable]
		public class PlatformOptions
		{
			public bool overridePath;

			public FileLocation pathLocation = FileLocation.RelativeToStreamingAssetsFolder;

			public string path;

			public virtual bool IsModified()
			{
				return overridePath;
			}
		}

		[Serializable]
		public class OptionsWindows : PlatformOptions
		{
			public Windows.VideoApi videoApi;

			public bool useHardwareDecoding = true;

			public bool useUnityAudio;

			public bool forceAudioResample = true;

			public int desiredAudioChannels = 2;

			public bool useTextureMips;

			public string forceAudioOutputDeviceName = string.Empty;

			public override bool IsModified()
			{
				return base.IsModified() || !useHardwareDecoding || useTextureMips || useUnityAudio || videoApi != 0 || !forceAudioResample || desiredAudioChannels != 2;
			}
		}

		[Serializable]
		public class OptionsMacOSX : PlatformOptions
		{
			[Multiline]
			public string httpHeaderJson;

			public override bool IsModified()
			{
				return base.IsModified() || !string.IsNullOrEmpty(httpHeaderJson);
			}
		}

		[Serializable]
		public class OptionsIOS : PlatformOptions
		{
			public bool useYpCbCr420Textures;

			[Multiline]
			public string httpHeaderJson;

			public override bool IsModified()
			{
				return base.IsModified() || !string.IsNullOrEmpty(httpHeaderJson) || useYpCbCr420Textures;
			}
		}

		[Serializable]
		public class OptionsTVOS : PlatformOptions
		{
			public bool useYpCbCr420Textures;

			[Multiline]
			public string httpHeaderJson;

			public override bool IsModified()
			{
				return base.IsModified() || !string.IsNullOrEmpty(httpHeaderJson) || useYpCbCr420Textures;
			}
		}

		[Serializable]
		public class OptionsAndroid : PlatformOptions
		{
			public bool useFastOesPath;

			public bool showPosterFrame;

			[Multiline]
			public string httpHeaderJson;

			[SerializeField]
			public int fileOffset;

			public override bool IsModified()
			{
				return base.IsModified() || fileOffset != 0 || useFastOesPath || showPosterFrame || !string.IsNullOrEmpty(httpHeaderJson);
			}
		}

		[Serializable]
		public class OptionsWindowsPhone : PlatformOptions
		{
			public bool useHardwareDecoding = true;

			public bool useUnityAudio;

			public bool forceAudioResample = true;

			public int desiredAudioChannels = 2;

			public bool useTextureMips;

			public override bool IsModified()
			{
				return base.IsModified() || !useHardwareDecoding || useTextureMips || useUnityAudio || !forceAudioResample || desiredAudioChannels != 2;
			}
		}

		[Serializable]
		public class OptionsWindowsUWP : PlatformOptions
		{
			public bool useHardwareDecoding = true;

			public bool useUnityAudio;

			public bool forceAudioResample = true;

			public int desiredAudioChannels = 2;

			public bool useTextureMips;

			public override bool IsModified()
			{
				return base.IsModified() || !useHardwareDecoding || useTextureMips || useUnityAudio || !forceAudioResample || desiredAudioChannels != 2;
			}
		}

		[Serializable]
		public class OptionsWebGL : PlatformOptions
		{
		}

		public FileLocation m_VideoLocation = FileLocation.RelativeToStreamingAssetsFolder;

		public string m_VideoPath;

		public bool m_AutoOpen = true;

		public bool m_AutoStart = true;

		public bool m_Loop;

		[Range(0f, 1f)]
		public float m_Volume = 1f;

		[SerializeField]
		[Range(-1f, 1f)]
		private float m_Balance;

		public bool m_Muted;

		[SerializeField]
		[Range(-4f, 4f)]
		public float m_PlaybackRate = 1f;

		[SerializeField]
		private bool m_DebugGui;

		[SerializeField]
		private bool m_DebugGuiControls = true;

		[SerializeField]
		private bool m_Persistent;

		public StereoPacking m_StereoPacking;

		public AlphaPacking m_AlphaPacking;

		public bool m_DisplayDebugStereoColorTint;

		public FilterMode m_FilterMode = FilterMode.Bilinear;

		public TextureWrapMode m_WrapMode = TextureWrapMode.Clamp;

		[Range(0f, 16f)]
		public int m_AnisoLevel;

		[SerializeField]
		private bool m_LoadSubtitles;

		[SerializeField]
		private FileLocation m_SubtitleLocation = FileLocation.RelativeToStreamingAssetsFolder;

		private FileLocation m_queueSubtitleLocation;

		[SerializeField]
		private string m_SubtitlePath;

		private string m_queueSubtitlePath;

		private Coroutine m_loadSubtitlesRoutine;

		[SerializeField]
		private MediaPlayerEvent m_events;

		private IMediaControl m_Control;

		private IMediaProducer m_Texture;

		private IMediaInfo m_Info;

		private IMediaPlayer m_Player;

		private IMediaSubtitles m_Subtitles;

		private IDisposable m_Dispose;

		private bool m_VideoOpened;

		private bool m_AutoStartTriggered;

		private bool m_WasPlayingOnPause;

		private Coroutine _renderingCoroutine;

		private const int s_GuiDepth = -1000;

		private const float s_GuiScale = 1.5f;

		private const int s_GuiStartWidth = 10;

		private const int s_GuiWidth = 180;

		private int m_GuiPositionX = 10;

		private static bool s_GlobalStartup;

		private bool m_EventFired_ReadyToPlay;

		private bool m_EventFired_Started;

		private bool m_EventFired_FirstFrameReady;

		private bool m_EventFired_FinishedPlaying;

		private bool m_EventFired_MetaDataReady;

		private int m_previousSubtitleIndex = -1;

		private bool m_isPlaybackStalled;

		[SerializeField]
		private OptionsWindows _optionsWindows = new OptionsWindows();

		[SerializeField]
		private OptionsMacOSX _optionsMacOSX = new OptionsMacOSX();

		[SerializeField]
		private OptionsIOS _optionsIOS = new OptionsIOS();

		[SerializeField]
		private OptionsTVOS _optionsTVOS = new OptionsTVOS();

		[SerializeField]
		private OptionsAndroid _optionsAndroid = new OptionsAndroid();

		[SerializeField]
		private OptionsWindowsPhone _optionsWindowsPhone = new OptionsWindowsPhone();

		[SerializeField]
		private OptionsWindowsUWP _optionsWindowsUWP = new OptionsWindowsUWP();

		[SerializeField]
		private OptionsWebGL _optionsWebGL = new OptionsWebGL();

		public bool DisplayDebugGUI
		{
			get
			{
				return m_DebugGui;
			}
			set
			{
				m_DebugGui = value;
			}
		}

		public bool DisplayDebugGUIControls
		{
			get
			{
				return m_DebugGuiControls;
			}
			set
			{
				m_DebugGuiControls = value;
			}
		}

		public bool Persistent
		{
			get
			{
				return m_Persistent;
			}
			set
			{
				m_Persistent = value;
			}
		}

		public IMediaInfo Info
		{
			get
			{
				return m_Info;
			}
		}

		public IMediaControl Control
		{
			get
			{
				return m_Control;
			}
		}

		public IMediaPlayer Player
		{
			get
			{
				return m_Player;
			}
		}

		public virtual IMediaProducer TextureProducer
		{
			get
			{
				return m_Texture;
			}
		}

		public virtual IMediaSubtitles Subtitles
		{
			get
			{
				return m_Subtitles;
			}
		}

		public MediaPlayerEvent Events
		{
			get
			{
				if (m_events == null)
				{
					m_events = new MediaPlayerEvent();
				}
				return m_events;
			}
		}

		public bool VideoOpened
		{
			get
			{
				return m_VideoOpened;
			}
		}

		public OptionsWindows PlatformOptionsWindows
		{
			get
			{
				return _optionsWindows;
			}
		}

		public OptionsMacOSX PlatformOptionsMacOSX
		{
			get
			{
				return _optionsMacOSX;
			}
		}

		public OptionsIOS PlatformOptionsIOS
		{
			get
			{
				return _optionsIOS;
			}
		}

		public OptionsTVOS PlatformOptionsTVOS
		{
			get
			{
				return _optionsTVOS;
			}
		}

		public OptionsAndroid PlatformOptionsAndroid
		{
			get
			{
				return _optionsAndroid;
			}
		}

		public OptionsWindowsPhone PlatformOptionsWindowsPhone
		{
			get
			{
				return _optionsWindowsPhone;
			}
		}

		public OptionsWindowsUWP PlatformOptionsWindowsUWP
		{
			get
			{
				return _optionsWindowsUWP;
			}
		}

		public OptionsWebGL PlatformOptionsWebGL
		{
			get
			{
				return _optionsWebGL;
			}
		}

		public bool SubtitlesEnabled
		{
			get
			{
				return m_LoadSubtitles;
			}
		}

		public string SubtitlePath
		{
			get
			{
				return m_SubtitlePath;
			}
		}

		public FileLocation SubtitleLocation
		{
			get
			{
				return m_SubtitleLocation;
			}
		}

		private void Awake()
		{
			if (m_Persistent)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		protected void Initialise()
		{
			BaseMediaPlayer baseMediaPlayer = CreatePlatformMediaPlayer();
			if (baseMediaPlayer != null)
			{
				m_Control = baseMediaPlayer;
				m_Texture = baseMediaPlayer;
				m_Info = baseMediaPlayer;
				m_Player = baseMediaPlayer;
				m_Subtitles = baseMediaPlayer;
				m_Dispose = baseMediaPlayer;
				if (!s_GlobalStartup)
				{
					Helper.LogInfo(string.Format("Initialising AVPro Video (script v{0} plugin v{1}) on {2}/{3} (MT {4}) on {5}", "1.6.10", baseMediaPlayer.GetVersion(), SystemInfo.graphicsDeviceName, SystemInfo.graphicsDeviceVersion, SystemInfo.graphicsMultiThreaded, Application.platform));
					s_GlobalStartup = true;
				}
			}
		}

		private void Start()
		{
			if (m_Control == null)
			{
				Initialise();
			}
			if (m_Control == null)
			{
				return;
			}
			if (m_AutoOpen)
			{
				OpenVideoFromFile();
				if (m_LoadSubtitles && m_Subtitles != null && !string.IsNullOrEmpty(m_SubtitlePath))
				{
					EnableSubtitles(m_SubtitleLocation, m_SubtitlePath);
				}
			}
			StartRenderCoroutine();
		}

		public bool OpenVideoFromFile(FileLocation location, string path, bool autoPlay = true)
		{
			m_VideoLocation = location;
			m_VideoPath = path;
			m_AutoStart = autoPlay;
			if (m_Control == null)
			{
				Initialise();
			}
			return OpenVideoFromFile();
		}

		public bool OpenVideoFromBuffer(byte[] buffer, bool autoPlay = true)
		{
			m_VideoLocation = FileLocation.AbsolutePathOrURL;
			m_VideoPath = "buffer";
			m_AutoStart = autoPlay;
			if (m_Control == null)
			{
				Initialise();
			}
			return OpenVideoFromBufferInternal(buffer);
		}

		public bool EnableSubtitles(FileLocation fileLocation, string filePath)
		{
			bool result = false;
			if (m_Subtitles != null)
			{
				if (!string.IsNullOrEmpty(filePath))
				{
					string platformFilePath = GetPlatformFilePath(GetPlatform(), ref filePath, ref fileLocation);
					bool flag = true;
					if (platformFilePath.Contains("://"))
					{
						flag = false;
					}
					if (flag && !File.Exists(platformFilePath))
					{
						Debug.LogError("[AVProVideo] Subtitle file not found: " + platformFilePath, this);
					}
					else
					{
						Helper.LogInfo("Opening subtitles " + platformFilePath, this);
						m_previousSubtitleIndex = -1;
						try
						{
							if (platformFilePath.Contains("://"))
							{
								if (m_loadSubtitlesRoutine != null)
								{
									StopCoroutine(m_loadSubtitlesRoutine);
									m_loadSubtitlesRoutine = null;
								}
								m_loadSubtitlesRoutine = StartCoroutine(LoadSubtitlesCoroutine(platformFilePath, fileLocation, filePath));
							}
							else
							{
								string data = File.ReadAllText(platformFilePath);
								if (m_Subtitles.LoadSubtitlesSRT(data))
								{
									m_SubtitleLocation = fileLocation;
									m_SubtitlePath = filePath;
									m_LoadSubtitles = false;
									result = true;
								}
								else
								{
									Debug.LogError("[AVProVideo] Failed to load subtitles" + platformFilePath, this);
								}
							}
						}
						catch (Exception exception)
						{
							Debug.LogError("[AVProVideo] Failed to load subtitles " + platformFilePath, this);
							Debug.LogException(exception, this);
						}
					}
				}
				else
				{
					Debug.LogError("[AVProVideo] No subtitle file path specified", this);
				}
			}
			else
			{
				m_queueSubtitleLocation = fileLocation;
				m_queueSubtitlePath = filePath;
			}
			return result;
		}

		private IEnumerator LoadSubtitlesCoroutine(string url, FileLocation fileLocation, string filePath)
		{
			WWW www = new WWW(url);
			yield return www;
			string subtitleData = string.Empty;
			if (string.IsNullOrEmpty(www.error))
			{
				subtitleData = www.text;
			}
			else
			{
				Debug.LogError("[AVProVideo] Error loading subtitles '" + www.error + "' from " + url);
			}
			if (m_Subtitles.LoadSubtitlesSRT(subtitleData))
			{
				m_SubtitleLocation = fileLocation;
				m_SubtitlePath = filePath;
				m_LoadSubtitles = false;
			}
			else
			{
				Debug.LogError("[AVProVideo] Failed to load subtitles" + url, this);
			}
			m_loadSubtitlesRoutine = null;
		}

		public void DisableSubtitles()
		{
			if (m_loadSubtitlesRoutine != null)
			{
				StopCoroutine(m_loadSubtitlesRoutine);
				m_loadSubtitlesRoutine = null;
			}
			if (m_Subtitles != null)
			{
				m_previousSubtitleIndex = -1;
				m_LoadSubtitles = false;
				m_Subtitles.LoadSubtitlesSRT(string.Empty);
			}
			else
			{
				m_queueSubtitlePath = string.Empty;
			}
		}

		private bool OpenVideoFromBufferInternal(byte[] buffer)
		{
			bool result = false;
			if (m_Control != null)
			{
				CloseVideo();
				m_VideoOpened = true;
				m_AutoStartTriggered = !m_AutoStart;
				m_EventFired_MetaDataReady = false;
				m_EventFired_ReadyToPlay = false;
				m_EventFired_Started = false;
				m_EventFired_FirstFrameReady = false;
				m_previousSubtitleIndex = -1;
				Helper.LogInfo("Opening buffer of length " + buffer.Length, this);
				if (!m_Control.OpenVideoFromBuffer(buffer))
				{
					Debug.LogError("[AVProVideo] Failed to open buffer", this);
				}
				else
				{
					SetPlaybackOptions();
					result = true;
					StartRenderCoroutine();
				}
			}
			return result;
		}

		private bool OpenVideoFromFile()
		{
			bool result = false;
			if (m_Control != null)
			{
				CloseVideo();
				m_VideoOpened = true;
				m_AutoStartTriggered = !m_AutoStart;
				m_EventFired_MetaDataReady = false;
				m_EventFired_ReadyToPlay = false;
				m_EventFired_Started = false;
				m_EventFired_FirstFrameReady = false;
				m_previousSubtitleIndex = -1;
				long platformFileOffset = GetPlatformFileOffset();
				string platformFilePath = GetPlatformFilePath(GetPlatform(), ref m_VideoPath, ref m_VideoLocation);
				if (!string.IsNullOrEmpty(m_VideoPath))
				{
					string httpHeaderJson = null;
					bool flag = true;
					if (platformFilePath.Contains("://"))
					{
						flag = false;
						httpHeaderJson = GetPlatformHttpHeaderJson();
					}
					if (flag && !File.Exists(platformFilePath))
					{
						Debug.LogError("[AVProVideo] File not found: " + platformFilePath, this);
					}
					else
					{
						Helper.LogInfo("Opening " + platformFilePath + " (offset " + platformFileOffset + ")", this);
						if (!m_Control.OpenVideoFromFile(platformFilePath, platformFileOffset, httpHeaderJson))
						{
							Debug.LogError("[AVProVideo] Failed to open " + platformFilePath, this);
						}
						else
						{
							SetPlaybackOptions();
							result = true;
							StartRenderCoroutine();
						}
					}
				}
				else
				{
					Debug.LogError("[AVProVideo] No file path specified", this);
				}
			}
			return result;
		}

		private void SetPlaybackOptions()
		{
			if (m_Control != null)
			{
				m_Control.SetLooping(m_Loop);
				m_Control.SetVolume(m_Volume);
				m_Control.SetBalance(m_Balance);
				m_Control.SetPlaybackRate(m_PlaybackRate);
				m_Control.MuteAudio(m_Muted);
				m_Control.SetTextureProperties(m_FilterMode, m_WrapMode, m_AnisoLevel);
			}
		}

		public void CloseVideo()
		{
			if (m_Control != null)
			{
				if (m_events != null)
				{
					m_events.Invoke(this, MediaPlayerEvent.EventType.Closing, ErrorCode.None);
				}
				m_AutoStartTriggered = false;
				m_VideoOpened = false;
				m_EventFired_ReadyToPlay = false;
				m_EventFired_Started = false;
				m_EventFired_MetaDataReady = false;
				m_EventFired_FirstFrameReady = false;
				if (m_loadSubtitlesRoutine != null)
				{
					StopCoroutine(m_loadSubtitlesRoutine);
					m_loadSubtitlesRoutine = null;
				}
				m_previousSubtitleIndex = -1;
				m_isPlaybackStalled = false;
				m_Control.CloseVideo();
			}
			StopRenderCoroutine();
		}

		public void Play()
		{
			if (m_Control != null && m_Control.CanPlay())
			{
				m_Control.Play();
				m_EventFired_ReadyToPlay = true;
			}
			else
			{
				m_AutoStart = true;
			}
		}

		public void Pause()
		{
			if (m_Control != null && m_Control.IsPlaying())
			{
				m_Control.Pause();
			}
		}

		public void Stop()
		{
			if (m_Control != null)
			{
				m_Control.Stop();
			}
		}

		public void Rewind(bool pause)
		{
			if (m_Control != null)
			{
				if (pause)
				{
					Pause();
				}
				m_Control.Rewind();
			}
		}

		private void Update()
		{
			if (m_Control != null)
			{
				if (m_VideoOpened && m_AutoStart && !m_AutoStartTriggered && m_Control.CanPlay())
				{
					m_AutoStartTriggered = true;
					Play();
				}
				if (_renderingCoroutine == null && m_Control.CanPlay())
				{
					StartRenderCoroutine();
				}
				if (m_Subtitles != null && !string.IsNullOrEmpty(m_queueSubtitlePath))
				{
					EnableSubtitles(m_queueSubtitleLocation, m_queueSubtitlePath);
					m_queueSubtitlePath = string.Empty;
				}
				m_Player.Update();
				UpdateErrors();
				UpdateEvents();
			}
		}

		private void OnEnable()
		{
			if (m_Control != null && m_WasPlayingOnPause)
			{
				m_AutoStart = true;
				m_AutoStartTriggered = false;
				m_WasPlayingOnPause = false;
			}
			if (m_Player != null)
			{
				m_Player.OnEnable();
			}
			StartRenderCoroutine();
		}

		private void OnDisable()
		{
			if (m_Control != null && m_Control.IsPlaying())
			{
				m_WasPlayingOnPause = true;
				Pause();
			}
			StopRenderCoroutine();
		}

		private void OnDestroy()
		{
			CloseVideo();
			if (m_Dispose != null)
			{
				m_Dispose.Dispose();
				m_Dispose = null;
			}
			m_Control = null;
			m_Texture = null;
			m_Info = null;
			m_Player = null;
		}

		private void OnApplicationQuit()
		{
			if (!s_GlobalStartup)
			{
				return;
			}
			Helper.LogInfo("Shutdown");
			MediaPlayer[] array = Resources.FindObjectsOfTypeAll<MediaPlayer>();
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].CloseVideo();
				}
			}
			WindowsMediaPlayer.DeinitPlatform();
			s_GlobalStartup = false;
		}

		private void StartRenderCoroutine()
		{
			if (_renderingCoroutine == null)
			{
				_renderingCoroutine = StartCoroutine(FinalRenderCapture());
			}
		}

		private void StopRenderCoroutine()
		{
			if (_renderingCoroutine != null)
			{
				StopCoroutine(_renderingCoroutine);
				_renderingCoroutine = null;
			}
		}

		private IEnumerator FinalRenderCapture()
		{
			YieldInstruction wait = new WaitForEndOfFrame();
			while (Application.isPlaying)
			{
				yield return wait;
				if (base.enabled && m_Player != null)
				{
					m_Player.Render();
				}
			}
		}

		public static Platform GetPlatform()
		{
			Platform platform = Platform.Unknown;
			return Platform.Windows;
		}

		public PlatformOptions GetCurrentPlatformOptions()
		{
			PlatformOptions platformOptions = null;
			return _optionsWindows;
		}

		public static string GetPath(FileLocation location)
		{
			string result = string.Empty;
			switch (location)
			{
			case FileLocation.RelativeToDataFolder:
				result = Application.dataPath;
				break;
			case FileLocation.RelativeToPeristentDataFolder:
				result = Application.persistentDataPath;
				break;
			case FileLocation.RelativeToProjectFolder:
			{
				string path = "..";
				result = Path.GetFullPath(Path.Combine(Application.dataPath, path));
				result = result.Replace('\\', '/');
				break;
			}
			case FileLocation.RelativeToStreamingAssetsFolder:
				result = Application.streamingAssetsPath;
				break;
			}
			return result;
		}

		public static string GetFilePath(string path, FileLocation location)
		{
			string result = string.Empty;
			if (!string.IsNullOrEmpty(path))
			{
				switch (location)
				{
				case FileLocation.AbsolutePathOrURL:
					result = path;
					break;
				case FileLocation.RelativeToProjectFolder:
				case FileLocation.RelativeToStreamingAssetsFolder:
				case FileLocation.RelativeToDataFolder:
				case FileLocation.RelativeToPeristentDataFolder:
					result = Path.Combine(GetPath(location), path);
					break;
				}
			}
			return result;
		}

		private long GetPlatformFileOffset()
		{
			return 0L;
		}

		private string GetPlatformHttpHeaderJson()
		{
			string text = null;
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Trim();
			}
			return text;
		}

		private string GetPlatformFilePath(Platform platform, ref string filePath, ref FileLocation fileLocation)
		{
			string empty = string.Empty;
			if (platform != Platform.Unknown)
			{
				PlatformOptions currentPlatformOptions = GetCurrentPlatformOptions();
				if (currentPlatformOptions != null && currentPlatformOptions.overridePath)
				{
					filePath = currentPlatformOptions.path;
					fileLocation = currentPlatformOptions.pathLocation;
				}
			}
			return GetFilePath(filePath, fileLocation);
		}

		public virtual BaseMediaPlayer CreatePlatformMediaPlayer()
		{
			BaseMediaPlayer baseMediaPlayer = null;
			WindowsMediaPlayer.InitialisePlatform();
			baseMediaPlayer = new WindowsMediaPlayer(_optionsWindows.videoApi, _optionsWindows.useHardwareDecoding, _optionsWindows.useTextureMips, _optionsWindows.forceAudioOutputDeviceName, _optionsWindows.useUnityAudio, _optionsWindows.forceAudioResample, _optionsWindows.desiredAudioChannels);
			if (baseMediaPlayer == null)
			{
				Debug.LogError(string.Format("[AVProVideo] Not supported on this platform {0} {1} {2} {3}.  Using null media player!", Application.platform, SystemInfo.deviceModel, SystemInfo.processorType, SystemInfo.operatingSystem));
				baseMediaPlayer = new NullMediaPlayer();
			}
			return baseMediaPlayer;
		}

		private bool ForceWaitForNewFrame(int lastFrameCount, float timeoutMs)
		{
			bool result = false;
			DateTime now = DateTime.Now;
			int num = 0;
			while (Control != null && (DateTime.Now - now).TotalMilliseconds < (double)timeoutMs)
			{
				m_Player.Update();
				if (lastFrameCount != TextureProducer.GetTextureFrameCount())
				{
					result = true;
					break;
				}
				num++;
			}
			m_Player.Render();
			return result;
		}

		private void UpdateErrors()
		{
			ErrorCode lastError = m_Control.GetLastError();
			if (lastError != 0)
			{
				Debug.LogError("[AVProVideo] Error: " + Helper.GetErrorMessage(lastError));
				if (m_events != null)
				{
					m_events.Invoke(this, MediaPlayerEvent.EventType.Error, lastError);
				}
			}
		}

		private void UpdateEvents()
		{
			if (m_events == null || m_Control == null)
			{
				return;
			}
			m_EventFired_FinishedPlaying = FireEventIfPossible(MediaPlayerEvent.EventType.FinishedPlaying, m_EventFired_FinishedPlaying);
			if (m_EventFired_Started && m_Control != null && !m_Control.IsPlaying())
			{
				m_EventFired_Started = false;
			}
			if (m_EventFired_FinishedPlaying && m_Control != null && m_Control.IsPlaying() && !m_Control.IsFinished())
			{
				bool flag = true;
				flag = false;
				float num = 1000f / m_Info.GetVideoFrameRate();
				if (m_Info.GetDurationMs() - m_Control.GetCurrentTimeMs() > num)
				{
					flag = true;
				}
				if (flag)
				{
					m_EventFired_FinishedPlaying = false;
				}
			}
			m_EventFired_MetaDataReady = FireEventIfPossible(MediaPlayerEvent.EventType.MetaDataReady, m_EventFired_MetaDataReady);
			m_EventFired_ReadyToPlay = FireEventIfPossible(MediaPlayerEvent.EventType.ReadyToPlay, m_EventFired_ReadyToPlay);
			m_EventFired_Started = FireEventIfPossible(MediaPlayerEvent.EventType.Started, m_EventFired_Started);
			m_EventFired_FirstFrameReady = FireEventIfPossible(MediaPlayerEvent.EventType.FirstFrameReady, m_EventFired_FirstFrameReady);
			if (FireEventIfPossible(MediaPlayerEvent.EventType.SubtitleChange, false))
			{
				m_previousSubtitleIndex = m_Subtitles.GetSubtitleIndex();
			}
			bool flag2 = m_Info.IsPlaybackStalled();
			if (flag2 != m_isPlaybackStalled)
			{
				m_isPlaybackStalled = flag2;
				MediaPlayerEvent.EventType eventType = ((!m_isPlaybackStalled) ? MediaPlayerEvent.EventType.Unstalled : MediaPlayerEvent.EventType.Stalled);
				FireEventIfPossible(eventType, false);
			}
		}

		private bool FireEventIfPossible(MediaPlayerEvent.EventType eventType, bool hasFired)
		{
			if (CanFireEvent(eventType, hasFired))
			{
				hasFired = true;
				m_events.Invoke(this, eventType, ErrorCode.None);
			}
			return hasFired;
		}

		private bool CanFireEvent(MediaPlayerEvent.EventType et, bool hasFired)
		{
			bool result = false;
			if (m_events != null && m_Control != null && !hasFired)
			{
				switch (et)
				{
				case MediaPlayerEvent.EventType.FinishedPlaying:
					result = (!m_Control.IsLooping() && m_Control.CanPlay() && m_Control.IsFinished()) || (m_Control.GetCurrentTimeMs() > m_Info.GetDurationMs() && !m_Control.IsLooping());
					break;
				case MediaPlayerEvent.EventType.MetaDataReady:
					result = m_Control.HasMetaData();
					break;
				case MediaPlayerEvent.EventType.FirstFrameReady:
					result = m_Texture != null && m_Control.CanPlay() && m_Texture.GetTextureFrameCount() > 0;
					break;
				case MediaPlayerEvent.EventType.ReadyToPlay:
					result = !m_Control.IsPlaying() && m_Control.CanPlay() && !m_AutoStart;
					break;
				case MediaPlayerEvent.EventType.Started:
					result = m_Control.IsPlaying();
					break;
				case MediaPlayerEvent.EventType.SubtitleChange:
					result = m_previousSubtitleIndex != m_Subtitles.GetSubtitleIndex();
					break;
				case MediaPlayerEvent.EventType.Stalled:
					result = m_Info.IsPlaybackStalled();
					break;
				case MediaPlayerEvent.EventType.Unstalled:
					result = !m_Info.IsPlaybackStalled();
					break;
				}
			}
			return result;
		}

		private void OnApplicationFocus(bool focusStatus)
		{
		}

		private void OnApplicationPause(bool pauseStatus)
		{
		}

		[ContextMenu("Save Frame To PNG")]
		public void SaveFrameToPng()
		{
			Texture2D texture2D = ExtractFrame(null);
			if (texture2D != null)
			{
				byte[] array = texture2D.EncodeToPNG();
				if (array != null)
				{
					string text = Mathf.FloorToInt(Control.GetCurrentTimeMs()).ToString("D8");
					File.WriteAllBytes("frame-" + text + ".png", array);
				}
				UnityEngine.Object.Destroy(texture2D);
			}
		}

		public Texture2D ExtractFrame(Texture2D target, float timeSeconds = -1f, bool accurateSeek = true, int timeoutMs = 1000)
		{
			Texture2D result = target;
			Texture texture = ExtractFrame(timeSeconds, accurateSeek, timeoutMs);
			if (texture != null)
			{
				result = Helper.GetReadableTexture(texture, TextureProducer.RequiresVerticalFlip(), Helper.GetOrientation(Info.GetTextureTransform()), target);
			}
			return result;
		}

		private Texture ExtractFrame(float timeSeconds = -1f, bool accurateSeek = true, int timeoutMs = 1000)
		{
			Texture result = null;
			if (m_Control != null)
			{
				if (timeSeconds >= 0f)
				{
					Pause();
					float num = timeSeconds * 1000f;
					if (TextureProducer.GetTexture() != null && m_Control.GetCurrentTimeMs() == num)
					{
						result = TextureProducer.GetTexture();
					}
					else
					{
						int textureFrameCount = TextureProducer.GetTextureFrameCount();
						if (accurateSeek)
						{
							m_Control.Seek(num);
						}
						else
						{
							m_Control.SeekFast(num);
						}
						ForceWaitForNewFrame(textureFrameCount, timeoutMs);
						result = TextureProducer.GetTexture();
					}
				}
				else
				{
					result = TextureProducer.GetTexture();
				}
			}
			return result;
		}

		public void SetGuiPositionFromVideoIndex(int index)
		{
			m_GuiPositionX = Mathf.FloorToInt(15f + (float)(180 * index) * 1.5f);
		}

		public void SetDebugGuiEnabled(bool bEnabled)
		{
			m_DebugGui = bEnabled;
		}

		private void OnGUI()
		{
			if (m_Info == null || !m_DebugGui)
			{
				return;
			}
			GUI.depth = -1000;
			GUI.matrix = Matrix4x4.TRS(new Vector3(m_GuiPositionX, 10f, 0f), Quaternion.identity, new Vector3(1.5f, 1.5f, 1f));
			GUILayout.BeginVertical("box", GUILayout.MaxWidth(180f));
			GUILayout.Label(Path.GetFileName(m_VideoPath));
			GUILayout.Label("Dimensions: " + m_Info.GetVideoWidth() + "x" + m_Info.GetVideoHeight() + "@" + m_Info.GetVideoFrameRate().ToString("F2"));
			GUILayout.Label("Time: " + (m_Control.GetCurrentTimeMs() * 0.001f).ToString("F1") + "s / " + (m_Info.GetDurationMs() * 0.001f).ToString("F1") + "s");
			GUILayout.Label("Rate: " + m_Info.GetVideoDisplayRate().ToString("F2") + "Hz");
			if (TextureProducer != null && TextureProducer.GetTexture() != null)
			{
				GUILayout.BeginHorizontal();
				Rect rect = GUILayoutUtility.GetRect(32f, 32f);
				GUILayout.Space(8f);
				Rect rect2 = GUILayoutUtility.GetRect(32f, 32f);
				Matrix4x4 matrix = GUI.matrix;
				if (TextureProducer.RequiresVerticalFlip())
				{
					GUIUtility.ScaleAroundPivot(new Vector2(1f, -1f), new Vector2(0f, rect.y + rect.height / 2f));
				}
				GUI.DrawTexture(rect, TextureProducer.GetTexture(), ScaleMode.ScaleToFit, false);
				GUI.DrawTexture(rect2, TextureProducer.GetTexture(), ScaleMode.ScaleToFit, true);
				GUI.matrix = matrix;
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			if (m_DebugGuiControls)
			{
				GUILayout.BeginHorizontal();
				if (m_Control.IsPaused())
				{
					if (GUILayout.Button("Play", GUILayout.Width(50f)))
					{
						m_Control.Play();
					}
				}
				else if (GUILayout.Button("Pause", GUILayout.Width(50f)))
				{
					m_Control.Pause();
				}
				float durationMs = m_Info.GetDurationMs();
				float currentTimeMs = m_Control.GetCurrentTimeMs();
				float num = GUILayout.HorizontalSlider(currentTimeMs, 0f, durationMs);
				if (num != currentTimeMs)
				{
					m_Control.Seek(num);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
	}
}
