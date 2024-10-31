using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class ChangeVideoExample : MonoBehaviour
	{
		public MediaPlayer mp;

		public void NewVideo(string filePath)
		{
			mp.m_AutoStart = true;
			mp.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, filePath, false);
		}

		private void OnGUI()
		{
			if (GUILayout.Button("video1"))
			{
				NewVideo("video1.mp4");
			}
			if (GUILayout.Button("video2"))
			{
				NewVideo("video2.mp4");
			}
		}
	}
}
