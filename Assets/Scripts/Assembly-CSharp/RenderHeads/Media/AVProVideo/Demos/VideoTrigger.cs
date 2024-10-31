using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class VideoTrigger : MonoBehaviour
	{
		[SerializeField]
		private MediaPlayer _mediaPlayer;

		[SerializeField]
		private float _fadeTimeMs = 500f;

		private float _fade;

		private float _fadeDirection;

		private void OnTriggerEnter(Collider other)
		{
			if (_mediaPlayer != null)
			{
				_mediaPlayer.Play();
				_fadeDirection = 1f;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (_mediaPlayer != null)
			{
				_fadeDirection = -1f;
			}
		}

		private void Update()
		{
			if (_fadeDirection != 0f)
			{
				float num = 1000f / _fadeTimeMs;
				_fade += Time.deltaTime * _fadeDirection * num;
				if (_fade <= 0f)
				{
					_mediaPlayer.Rewind(true);
					_fadeDirection = 0f;
				}
				else if (_fade >= 1f)
				{
					_fadeDirection = 0f;
				}
				_fade = Mathf.Clamp01(_fade);
				if (_mediaPlayer != null && _mediaPlayer.Control != null)
				{
					_mediaPlayer.Control.SetVolume(_fade);
				}
			}
		}
	}
}
