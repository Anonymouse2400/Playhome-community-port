using System.IO;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class FrameExtract : MonoBehaviour
	{
		private const int NumFrames = 8;

		public MediaPlayer _mediaPlayer;

		public bool _accurateSeek;

		public int _timeoutMs = 250;

		public GUISkin _skin;

		public bool _saveToJPG;

		private string _filenamePrefix;

		private float _timeStepSeconds;

		private int _frameIndex = -1;

		private Texture2D _texture;

		private RenderTexture _displaySheet;

		private void Start()
		{
			_mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
			_displaySheet = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
			_displaySheet.useMipMap = false;
			_displaySheet.autoGenerateMips = false;
			_displaySheet.antiAliasing = 1;
			_displaySheet.Create();
			RenderTexture.active = _displaySheet;
			GL.Clear(false, true, Color.black, 0f);
			RenderTexture.active = null;
		}

		public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
		{
			switch (et)
			{
			case MediaPlayerEvent.EventType.MetaDataReady:
				mp.Play();
				mp.Pause();
				break;
			case MediaPlayerEvent.EventType.FirstFrameReady:
				OnNewMediaReady();
				break;
			}
		}

		private void OnNewMediaReady()
		{
			IMediaInfo info = _mediaPlayer.Info;
			if (_texture != null)
			{
				Object.Destroy(_texture);
				_texture = null;
			}
			int videoWidth = info.GetVideoWidth();
			int videoHeight = info.GetVideoHeight();
			_texture = new Texture2D(videoWidth, videoHeight, TextureFormat.ARGB32, false);
			_timeStepSeconds = _mediaPlayer.Info.GetDurationMs() / 1000f / 8f;
			_filenamePrefix = Path.GetFileName(_mediaPlayer.m_VideoPath);
		}

		private void OnDestroy()
		{
			if (_texture != null)
			{
				Object.Destroy(_texture);
				_texture = null;
			}
			if (_displaySheet != null)
			{
				RenderTexture.ReleaseTemporary(_displaySheet);
				_displaySheet = null;
			}
		}

		private void Update()
		{
			if (_texture != null && _frameIndex >= 0 && _frameIndex < 8)
			{
				ExtractNextFrame();
				_frameIndex++;
			}
		}

		private void ExtractNextFrame()
		{
			float timeSeconds = (float)_frameIndex * _timeStepSeconds;
			_texture = _mediaPlayer.ExtractFrame(_texture, timeSeconds, _accurateSeek, _timeoutMs);
			if (_saveToJPG)
			{
				string text = _filenamePrefix + "-" + _frameIndex + ".jpg";
				Debug.Log("Writing frame to file: " + text);
				File.WriteAllBytes(text, _texture.EncodeToJPG());
			}
			GL.PushMatrix();
			RenderTexture.active = _displaySheet;
			GL.LoadPixelMatrix(0f, _displaySheet.width, _displaySheet.height, 0f);
			Rect sourceRect = new Rect(0f, 0f, 1f, 1f);
			float num = 8f;
			float num2 = (float)_displaySheet.width / 8f - num;
			float num3 = num2 / ((float)_texture.width / (float)_texture.height);
			float x = (num2 + num) * (float)_frameIndex;
			Rect screenRect = new Rect(x, (float)_displaySheet.height / 2f - num3 / 2f, num2, num3);
			Graphics.DrawTexture(screenRect, _texture, sourceRect, 0, 0, 0, 0);
			RenderTexture.active = null;
			GL.PopMatrix();
			GL.InvalidateState();
		}

		private void OnGUI()
		{
			GUI.skin = _skin;
			if (_displaySheet != null)
			{
				GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _displaySheet, ScaleMode.ScaleToFit, false);
			}
			float num = 4f * ((float)Screen.height / 1080f);
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num, num, 1f));
			GUILayout.Space(16f);
			GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Width((float)Screen.width / num));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Start Extracting Frames"))
			{
				_frameIndex = 0;
				RenderTexture.active = _displaySheet;
				GL.Clear(false, true, Color.black, 0f);
				RenderTexture.active = null;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
}
