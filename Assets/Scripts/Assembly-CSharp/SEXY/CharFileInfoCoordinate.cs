using System;
using System.Collections.Generic;
using System.IO;

namespace SEXY
{
	[Serializable]
	public abstract class CharFileInfoCoordinate : BlockControlBase
	{
		public int coordinateLoadVersion;

		protected Dictionary<CharDefine.CoordinateType, CharFileInfoClothes> dictClothesInfo = new Dictionary<CharDefine.CoordinateType, CharFileInfoClothes>();

		public CharFileInfoCoordinate()
			: base("コーディネート情報", 1)
		{
		}

		public bool SetInfo(CharDefine.CoordinateType type, CharFileInfoClothes info)
		{
			return dictClothesInfo[type].Copy(info);
		}

		public CharFileInfoClothes GetInfo(CharDefine.CoordinateType type)
		{
			CharFileInfoClothes value = null;
			dictClothesInfo.TryGetValue(type, out value);
			return value;
		}

		public override bool LoadBytes(byte[] data, int coordinateVer)
		{
			using (MemoryStream input = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(input))
				{
					return LoadSub(br, coordinateVer);
				}
			}
		}

		protected abstract bool LoadSub(BinaryReader br, int coordinateVer);
	}
}
