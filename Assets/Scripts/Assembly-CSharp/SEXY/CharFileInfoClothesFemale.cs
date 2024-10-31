using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	[Serializable]
	public class CharFileInfoClothesFemale : CharFileInfoClothes
	{
		public bool swimType;

		public bool hideSwimOptTop;

		public bool hideSwimOptBot;

		public CharFileInfoClothesFemale()
			: base("【HoneySelectClothesFemale】", "coordinate/female/", Enum.GetNames(typeof(CharDefine.ClothesKindFemale)).Length)
		{
			MemberInitialize();
		}

		private new void MemberInitialize()
		{
			base.MemberInitialize();
			clothesTypeSex = 1;
			int[] array = new int[11]
			{
				101, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0
			};
			for (int i = 0; i < clothesKindNum; i++)
			{
				clothesId[i] = array[i];
				clothesColor[i] = new HSColorSet();
				clothesColor2[i] = new HSColorSet();
			}
			swimType = false;
			hideSwimOptTop = false;
			hideSwimOptBot = false;
		}

		public override bool Copy(CharFileInfoClothes srcData)
		{
			CharFileInfoClothesFemale charFileInfoClothesFemale = srcData as CharFileInfoClothesFemale;
			if (charFileInfoClothesFemale == null)
			{
				Debug.LogWarning("女の服装情報に男の服装情報を入れようとしてる？");
				return false;
			}
			for (int i = 0; i < clothesKindNum; i++)
			{
				clothesId[i] = charFileInfoClothesFemale.clothesId[i];
				clothesColor[i].Copy(charFileInfoClothesFemale.clothesColor[i]);
			}
			for (int j = 0; j < 10; j++)
			{
				base.accessory[j].Copy(srcData.accessory[j]);
			}
			swimType = charFileInfoClothesFemale.swimType;
			hideSwimOptTop = charFileInfoClothesFemale.hideSwimOptTop;
			hideSwimOptBot = charFileInfoClothesFemale.hideSwimOptBot;
			return true;
		}

		protected override bool LoadSub(BinaryReader br, int clothesVer, int colorVer)
		{
			swimType = br.ReadBoolean();
			hideSwimOptTop = br.ReadBoolean();
			hideSwimOptBot = br.ReadBoolean();
			return true;
		}
	}
}
