using System;
using System.Collections.Generic;

namespace RenderHeads.Media.AVProVideo
{
	public class AudioOutputManager
	{
		private static AudioOutputManager _instance;

		private Dictionary<MediaPlayer, HashSet<AudioOutput>> _accessTrackers;

		private Dictionary<MediaPlayer, float[]> _pcmData;

		public static AudioOutputManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AudioOutputManager();
				}
				return _instance;
			}
		}

		private AudioOutputManager()
		{
			_accessTrackers = new Dictionary<MediaPlayer, HashSet<AudioOutput>>();
			_pcmData = new Dictionary<MediaPlayer, float[]>();
		}

		public void RequestAudio(AudioOutput _outputComponent, MediaPlayer mediaPlayer, float[] data, int channelMask, int totalChannels, AudioOutput.AudioOutputMode audioOutputMode)
		{
			if (mediaPlayer == null || mediaPlayer.Control == null || !mediaPlayer.Control.IsPlaying())
			{
				return;
			}
			int numAudioChannels = mediaPlayer.Control.GetNumAudioChannels();
			if (!_accessTrackers.ContainsKey(mediaPlayer))
			{
				_accessTrackers[mediaPlayer] = new HashSet<AudioOutput>();
			}
			if (_accessTrackers[mediaPlayer].Contains(_outputComponent) || _accessTrackers[mediaPlayer].Count == 0 || _pcmData[mediaPlayer] == null)
			{
				_accessTrackers[mediaPlayer].Clear();
				int num = data.Length / totalChannels * numAudioChannels;
				_pcmData[mediaPlayer] = new float[num];
				GrabAudio(mediaPlayer, _pcmData[mediaPlayer], numAudioChannels);
				_accessTrackers[mediaPlayer].Add(_outputComponent);
			}
			int num2 = Math.Min(data.Length / totalChannels, _pcmData[mediaPlayer].Length / numAudioChannels);
			int num3 = 0;
			int num4 = 0;
			switch (audioOutputMode)
			{
			case AudioOutput.AudioOutputMode.Multiple:
			{
				int num6 = Math.Min(numAudioChannels, totalChannels);
				for (int l = 0; l < num2; l++)
				{
					for (int m = 0; m < num6; m++)
					{
						if (((1 << m) & channelMask) > 0)
						{
							data[num4 + m] = _pcmData[mediaPlayer][num3 + m];
						}
					}
					num3 += numAudioChannels;
					num4 += totalChannels;
				}
				break;
			}
			case AudioOutput.AudioOutputMode.Single:
			{
				int num5 = 0;
				for (int i = 0; i < 8; i++)
				{
					if ((channelMask & (1 << i)) > 0)
					{
						num5 = i;
						break;
					}
				}
				if (num5 >= numAudioChannels)
				{
					break;
				}
				for (int j = 0; j < num2; j++)
				{
					for (int k = 0; k < totalChannels; k++)
					{
						data[num4 + k] = _pcmData[mediaPlayer][num3 + num5];
					}
					num3 += numAudioChannels;
					num4 += totalChannels;
				}
				break;
			}
			}
		}

		private void GrabAudio(MediaPlayer player, float[] data, int channels)
		{
			player.Control.GrabAudio(data, data.Length, channels);
		}
	}
}
