using System;

namespace RenderHeads.Media.AVProVideo
{
	public static class Windows
	{
		public enum VideoApi
		{
			MediaFoundation = 0,
			DirectShow = 1
		}

		public const string AudioDeviceOutputName_Vive = "HTC VIVE USB Audio";

		public const string AudioDeviceOutputName_Rift = "Rift Audio";
	}
}
