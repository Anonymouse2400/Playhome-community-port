using System;

namespace SEXY
{
	[Serializable]
	public abstract class BlockControlBase
	{
		public readonly int version;

		public readonly string tagName = string.Empty;

		public BlockControlBase(string _tagName, int _version)
		{
			tagName = _tagName;
			version = _version;
		}

		public abstract bool LoadBytes(byte[] data, int version);
	}
}
