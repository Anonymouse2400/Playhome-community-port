using System;
using System.IO;

namespace SEXY
{
	[Serializable]
	public class CharFileInfoCoordinateMale : CharFileInfoCoordinate
	{
		public CharFileInfoCoordinateMale()
		{
			for (int i = 0; i < Enum.GetNames(typeof(CharDefine.CoordinateType)).Length; i++)
			{
				CharFileInfoClothesMale value = new CharFileInfoClothesMale();
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
				CharFileInfoClothesMale charFileInfoClothesMale = new CharFileInfoClothesMale();
				charFileInfoClothesMale.LoadWithoutPNG(br);
				dictClothesInfo[(CharDefine.CoordinateType)key] = charFileInfoClothesMale;
			}
			return true;
		}
	}
}
