using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	public interface IMediaProducer
	{
		int GetTextureCount();

		Texture GetTexture(int index = 0);

		int GetTextureFrameCount();

		long GetTextureTimeStamp();

		bool RequiresVerticalFlip();
	}
}
