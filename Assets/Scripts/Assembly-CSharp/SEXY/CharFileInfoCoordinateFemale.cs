using System;
using System.IO;

namespace SEXY
{
	[Serializable]
	public class CharFileInfoCoordinateFemale : CharFileInfoCoordinate
	{
		public CharFileInfoCoordinateFemale()
		{
			for (int i = 0; i < Enum.GetNames(typeof(CharDefine.CoordinateType)).Length; i++)
			{
				CharFileInfoClothesFemale value = new CharFileInfoClothesFemale();
				dictClothesInfo[(CharDefine.CoordinateType)i] = value;
			}
		}

		protected override bool LoadSub(BinaryReader br, int coordinateVer)
		{
			dictClothesInfo.Clear();
			int num = br.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int key = br.ReadInt32();
				CharFileInfoClothesFemale charFileInfoClothesFemale = new CharFileInfoClothesFemale();
				charFileInfoClothesFemale.LoadWithoutPNG(br);
				dictClothesInfo[(CharDefine.CoordinateType)key] = charFileInfoClothesFemale;
			}
			return true;
		}
	}
}
