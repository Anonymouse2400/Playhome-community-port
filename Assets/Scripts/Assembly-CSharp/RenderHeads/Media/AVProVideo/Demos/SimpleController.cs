using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class SimpleController : MonoBehaviour
	{
		public string _folder = "AVProVideoSamples/";

		public string[] _filenames = new string[3] { "SampleSphere.mp4", "BigBuckBunny_360p30.mp3", "BigBuckBunny_720p30.mp4" };

		public string[] _streams;

		public MediaPlayer _mediaPlayer;

		public DisplayIMGUI _display;

		public GUISkin _guiSkin;

		private int _width;

		private int _height;

		private float _durationSeconds;

		public bool _useFading = true;

		private Queue<string> _eventLog = new Queue<string>(8);

		private float _eventTimer = 1f;

		private MediaPlayer.FileLocation _nextVideoLocation;

		private string _nextVideoPath;

		private void Start()
		{
			_mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
		}

		public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
		{
			switch (et)
			{
			case MediaPlayerEvent.EventType.MetaDataReady:
				GatherProperties();
				break;
			}
			AddEvent(et);
		}

		private void AddEvent(MediaPlayerEvent.EventType et)
		{
			Debug.Log("[SimpleController] Event: " + et);
			_eventLog.Enqueue(et.ToString());
			if (_eventLog.Count > 5)
			{
				_eventLog.Dequeue();
				_eventTimer = 1f;
			}
		}

		private void GatherProperties()
		{
			if (_mediaPlayer != null && _mediaPlayer.Info != null)
			{
				_width = _mediaPlayer.Info.GetVideoWidth();
				_height = _mediaPlayer.Info.GetVideoHeight();
				_durationSeconds = _mediaPlayer.Info.GetDurationMs() / 1000f;
			}
		}

		private void Update()
		{
			if (!_useFading && _display != null && _display._mediaPlayer != null && _display._mediaPlayer.Control != null)
			{
				_display._color = Color.white;
				_display._mediaPlayer.Control.SetVolume(1f);
			}
			if (_eventLog != null && _eventLog.Count > 0)
			{
				_eventTimer -= Time.deltaTime;
				if (_eventTimer < 0f)
				{
					_eventLog.Dequeue();
					_eventTimer = 1f;
				}
			}
		}

		private void LoadVideo(string filePath, bool url = false)
		{
			if (!url)
			{
				_nextVideoLocation = MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder;
			}
			else
			{
				_nextVideoLocation = MediaPlayer.FileLocation.AbsolutePathOrURL;
			}
			_nextVideoPath = filePath;
			if (!_useFading)
			{
				if (!_mediaPlayer.OpenVideoFromFile(_nextVideoLocation, _nextVideoPath, _mediaPlayer.m_AutoStart))
				{
					Debug.LogError("Failed to open video!");
				}
			}
			else
			{
				StartCoroutine("LoadVideoWithFading");
			}
		}

		private static bool VideoIsReady(MediaPlayer mp)
		{
			return mp != null && mp.TextureProducer != null && mp.TextureProducer.GetTextureFrameCount() <= 0;
		}

		private static bool AudioIsReady(MediaPlayer mp)
		{
			return mp != null && mp.Control != null && mp.Control.CanPlay() && mp.Info.HasAudio() && !mp.Info.HasVideo();
		}

		private IEnumerator LoadVideoWithFading()
		{
			float fade3 = 0.25f;
			while (fade3 > 0f && Application.isPlaying)
			{
				fade3 -= Time.deltaTime;
				fade3 = Mathf.Clamp(fade3, 0f, 0.25f);
				_display._color = new Color(1f, 1f, 1f, fade3 / 0.25f);
				_display._mediaPlayer.Control.SetVolume(fade3 / 0.25f);
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			if (Application.isPlaying)
			{
				if (!_mediaPlayer.OpenVideoFromFile(_nextVideoLocation, _nextVideoPath, _mediaPlayer.m_AutoStart))
				{
					Debug.LogError("Failed to open video!");
				}
				else
				{
					while (Application.isPlaying && (VideoIsReady(_mediaPlayer) || AudioIsReady(_mediaPlayer)))
					{
						yield return null;
					}
					yield return new WaitForEndOfFrame();
					yield return new WaitForEndOfFrame();
					yield return new WaitForEndOfFrame();
				}
			}
			while (fade3 < 0.25f && Application.isPlaying)
			{
				fade3 += Time.deltaTime;
				fade3 = Mathf.Clamp(fade3, 0f, 0.25f);
				_display._color = new Color(1f, 1f, 1f, fade3 / 0.25f);
				_display._mediaPlayer.Control.SetVolume(fade3 / 0.25f);
				yield return null;
			}
		}

		private void OnGUI()
		{
			if (_mediaPlayer == null)
			{
				return;
			}
			GUI.depth = -10;
			if (_guiSkin != null)
			{
				GUI.skin = _guiSkin;
			}
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width / 960f, (float)Screen.height / 540f, 1f));
			GUILayout.BeginVertical("box");
			if (_mediaPlayer.Control != null)
			{
				GUILayout.Label("Loaded: " + _mediaPlayer.m_VideoPath);
				GUILayout.Label(string.Format("Size: {0}x{1} FPS: {3} Duration: {2}ms", _width, _height, _mediaPlayer.Info.GetDurationMs(), _mediaPlayer.Info.GetVideoFrameRate().ToString("F2")));
				GUILayout.Label("Updates: " + _mediaPlayer.TextureProducer.GetTextureFrameCount() + "    Rate: " + _mediaPlayer.Info.GetVideoDisplayRate().ToString("F1"));
				GUILayout.BeginHorizontal();
				_useFading = GUILayout.Toggle(_useFading, "Fade to Black During Loading");
				_mediaPlayer.m_AutoStart = GUILayout.Toggle(_mediaPlayer.m_AutoStart, "Auto Play After Load");
				bool loop = _mediaPlayer.m_Loop;
				bool flag = GUILayout.Toggle(loop, "Loop");
				if (flag != loop)
				{
					_mediaPlayer.m_Loop = flag;
					_mediaPlayer.Control.SetLooping(flag);
				}
				GUILayout.EndHorizontal();
				int num = (int)_mediaPlayer.Control.GetCurrentTimeMs();
				int num2 = (int)GUILayout.HorizontalSlider(num, 0f, _durationSeconds * 1000f);
				Rect lastRect = GUILayoutUtility.GetLastRect();
				float x = GUI.skin.horizontalSliderThumb.CalcSize(GUIContent.none).x;
				Rect position = lastRect;
				GUI.color = Color.green;
				position.xMin += x;
				position.y = position.yMax - 4f;
				position.width -= x * 1f;
				position.width *= _mediaPlayer.Control.GetBufferingProgress();
				position.height = 4f;
				GUI.DrawTexture(position, Texture2D.whiteTexture, ScaleMode.StretchToFill);
				GUI.color = Color.green;
				int bufferedTimeRangeCount = _mediaPlayer.Control.GetBufferedTimeRangeCount();
				for (int i = 0; i < bufferedTimeRangeCount; i++)
				{
					float startTimeMs = 0f;
					float endTimeMs = 0f;
					if (_mediaPlayer.Control.GetBufferedTimeRange(i, ref startTimeMs, ref endTimeMs))
					{
						position.xMin = x + lastRect.x + (lastRect.width - x * 1f) * (startTimeMs / (_durationSeconds * 1000f));
						position.xMax = x + lastRect.x + (lastRect.width - x * 1f) * (endTimeMs / (_durationSeconds * 1000f));
						GUI.DrawTexture(position, Texture2D.whiteTexture, ScaleMode.StretchToFill);
					}
				}
				GUI.color = Color.white;
				if (num2 != num)
				{
					_mediaPlayer.Control.Seek(num2);
				}
				if (!_mediaPlayer.Control.IsPlaying())
				{
					if (GUILayout.Button("Play"))
					{
						_mediaPlayer.Control.Play();
					}
				}
				else if (GUILayout.Button("Pause"))
				{
					_mediaPlayer.Control.Pause();
				}
				GUILayout.BeginHorizontal();
				int audioTrackCount = _mediaPlayer.Info.GetAudioTrackCount();
				int currentAudioTrack = _mediaPlayer.Control.GetCurrentAudioTrack();
				for (int j = 0; j < audioTrackCount; j++)
				{
					if (j == currentAudioTrack)
					{
						GUI.color = Color.green;
					}
					if (GUILayout.Button("Audio Track #" + (j + 1)))
					{
						_mediaPlayer.Control.SetAudioTrack(j);
					}
					GUI.color = Color.white;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.Label("Select a new file to play:");
			int num3 = GUILayout.SelectionGrid(-1, _filenames, 3);
			if (num3 >= 0)
			{
				LoadVideo(Path.Combine(_folder, _filenames[num3]));
			}
			GUILayout.Space(8f);
			GUILayout.Label("Select a new stream to play:");
			int num4 = GUILayout.SelectionGrid(-1, _streams, 1);
			if (num4 >= 0)
			{
				LoadVideo(_streams[num4], true);
			}
			GUILayout.Space(8f);
			GUILayout.Label("Recent Events: ");
			GUILayout.BeginVertical("box");
			int num5 = 0;
			foreach (string item in _eventLog)
			{
				GUI.color = Color.white;
				if (num5 == 0)
				{
					GUI.color = new Color(1f, 1f, 1f, _eventTimer);
				}
				GUILayout.Label(item);
				num5++;
			}
			GUILayout.EndVertical();
			GUI.color = Color.white;
			GUILayout.EndVertical();
		}
	}
}
