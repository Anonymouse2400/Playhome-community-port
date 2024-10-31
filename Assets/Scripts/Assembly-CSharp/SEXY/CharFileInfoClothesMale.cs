using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	[Serializable]
	public class CharFileInfoClothesMale : CharFileInfoClothes
	{
		public CharFileInfoClothesMale()
			: base("【HoneySelectClothesMale】", "coordinate/male/", Enum.GetNames(typeof(CharDefine.ClothesKindMale)).Length)
		{
			MemberInitialize();
		}

		private new void MemberInitialize()
		{
			base.MemberInitialize();
			clothesTypeSex = 0;
			int[] array = new int[2];
			for (int i = 0; i < clothesId.Length; i++)
			{
				clothesId[i] = array[i];
				clothesColor[i] = new HSColorSet();
				clothesColor2[i] = new HSColorSet();
			}
		}

		public override bool Copy(CharFileInfoClothes srcData)
		{
			CharFileInfoClothesMale charFileInfoClothesMale = srcData as CharFileInfoClothesMale;
			if (charFileInfoClothesMale == null)
			{
				Debug.LogWarning("男の服装情報に女の服装情報を入れようとしてる？");
				return false;
			}
			for (int i = 0; i < clothesKindNum; i++)
			{
				clothesId[i] = charFileInfoClothesMale.clothesId[i];
				clothesColor[i].Copy(charFileInfoClothesMale.clothesColor[i]);
			}
			for (int j = 0; j < 10; j++)
			{
				base.accessory[j].Copy(srcData.accessory[j]);
			}
			return true;
		}

		protected override bool LoadSub(BinaryReader br, int clothesVer, int colorVer)
		{
			return true;
		}
	}
}
