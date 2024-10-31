using System.IO;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo.Demos
{
	public class LoadFromBuffer : MonoBehaviour
	{
		[SerializeField]
		private MediaPlayer _mp;

		[SerializeField]
		private string _filename;

		private void Start()
		{
			if (!(_mp != null))
			{
				return;
			}
			byte[] array = null;
			using (FileStream input = new FileStream(_filename, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					long length = new FileInfo(_filename).Length;
					array = binaryReader.ReadBytes((int)length);
				}
			}
			if (array != null)
			{
				_mp.OpenVideoFromBuffer(array);
			}
		}
	}
}
