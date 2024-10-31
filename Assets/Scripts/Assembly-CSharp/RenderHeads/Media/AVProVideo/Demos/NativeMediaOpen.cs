using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class NativeMediaOpen : MonoBehaviour
	{
		public MediaPlayer player;

		private void Start()
		{
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Open Video File"))
			{
			}
			if (player != null)
			{
				GUILayout.Label("Currently Playing: " + player.m_VideoPath);
			}
		}

		private void Update()
		{
		}
	}
}
