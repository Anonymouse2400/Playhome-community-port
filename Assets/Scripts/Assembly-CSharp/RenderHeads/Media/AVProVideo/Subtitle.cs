namespace RenderHeads.Media.AVProVideo
{
	public class Subtitle
	{
		public int index;

		public string text;

		public int timeStartMs;

		public int timeEndMs;

		public bool IsBefore(float time)
		{
			return time > (float)timeStartMs && time > (float)timeEndMs;
		}

		public bool IsTime(float time)
		{
			return time >= (float)timeStartMs && time < (float)timeEndMs;
		}
	}
}
