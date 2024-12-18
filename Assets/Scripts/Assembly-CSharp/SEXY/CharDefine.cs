using System;

namespace SEXY
{
	public static class CharDefine
	{
		public enum HairKindFemale
		{
			back = 0,
			front = 1,
			side = 2,
			option = 3
		}

		public enum HairKindMale
		{
			back = 0
		}

		public enum ClothesKindFemale
		{
			top = 0,
			bot = 1,
			bra = 2,
			shorts = 3,
			swim = 4,
			swimTop = 5,
			swimBot = 6,
			gloves = 7,
			panst = 8,
			socks = 9,
			shoes = 10
		}

		public enum ClothesKindMale
		{
			clothes = 0,
			shoes = 1
		}

		public enum ClothesStateKindFemale
		{
			top = 0,
			bot = 1,
			bra = 2,
			shorts = 3,
			swimsuitTop = 4,
			swimsuitBot = 5,
			swimClothesTop = 6,
			swimClothesBot = 7,
			gloves = 8,
			panst = 9,
			socks = 10,
			shoes = 11
		}

		public enum ClothesStateKindMale
		{
			clothes = 0,
			shoes = 1
		}

		public enum CoordinateType
		{
			type00 = 0,
			type01 = 1,
			type02 = 2
		}

		public enum SiruObjKind
		{
			top = 0,
			bot = 1,
			bra = 2,
			shorts = 3,
			swim = 4
		}

		public enum SiruParts
		{
			SiruKao = 0,
			SiruFrontUp = 1,
			SiruFrontDown = 2,
			SiruBackUp = 3,
			SiruBackDown = 4
		}

		public const int CharaFileProductNo = 11;

		public const int CharaFileVersion = 2;

		public const int PreviewVersion = 4;

		public const int CustomVersion = 4;

		public const int ClothesVersion = 3;

		public const int CoordinateVersion = 1;

		public const int StatusVersion = 4;

		public const int ParameterVersion = 5;

		public const int CustomFileVersion = 1;

		public const string CharaFileFemaleDir = "chara/female/";

		public const string CharaFileMaleDir = "chara/male/";

		public const string ClothesFileFemaleDir = "coordinate/female/";

		public const string ClothesFileMaleDir = "coordinate/male/";

		public const string CustomFileFemaleDir = "custom/female/";

		public const string CustomFileMaleDir = "custom/male/";

		public const string CharaFemaleFileMark = "【HoneySelectCharaFemale】";

		public const string CharaMaleFileMark = "【HoneySelectCharaMale】";

		public const string ClothesFemaleFileMark = "【HoneySelectClothesFemale】";

		public const string ClothesMaleFileMark = "【HoneySelectClothesMale】";

		public const string CustomFileMark = "【HoneySelectCustomFile】";

		public const int AccessorySlotNum = 10;

		public static readonly string[] cf_bodyshapename = new string[33]
		{
			"身長", "胸サイズ", "胸上下位置", "胸の左右開き", "胸の左右位置", "胸上下角度", "胸の尖り", "乳輪の膨らみ", "乳首太さ", "頭サイズ",
			"首周り幅", "首周り奥", "胴体肩周り幅", "胴体肩周り奥", "胴体上幅", "胴体上奥", "胴体下幅", "胴体下奥", "ウエスト位置", "腰上幅",
			"腰上奥", "腰下幅", "腰下奥", "尻", "尻角度", "太もも上", "太もも下", "ふくらはぎ", "足首", "肩",
			"上腕", "前腕", "乳首立ち"
		};

		public static readonly int cf_bodyshape_BustNo = 1;

		public static readonly string[] cf_headshapename = new string[67]
		{
			"顔全体横幅", "顔上部前後", "顔上部上下", "顔下部前後", "顔下部横幅", "顎横幅", "顎上下", "顎前後", "顎角度", "顎下部上下",
			"顎先幅", "顎先上下", "顎先前後", "頬下部上下", "頬下部前後", "頬下部幅", "頬上部上下", "頬上部前後", "頬上部幅", "眉毛上下",
			"眉毛横位置", "眉毛角度Z軸", "眉毛内側形状", "眉毛外側形状", "目上下", "目横位置", "目前後", "目の横幅", "目の縦幅", "目の角度Z軸",
			"目の角度Y軸", "目頭左右位置", "目尻左右位置", "目頭上下位置", "目尻上下位置", "まぶた形状１", "まぶた形状２", "瞳の上下調整", "瞳の横幅", "瞳の縦幅",
			"鼻全体上下", "鼻全体前後", "鼻全体角度X軸", "鼻全体横幅", "鼻筋高さ", "鼻筋横幅", "鼻筋形状", "小鼻横幅", "小鼻上下", "小鼻前後",
			"小鼻角度X軸", "小鼻角度Z軸", "鼻先高さ", "鼻先角度X軸", "鼻先サイズ", "口上下", "口横幅", "口縦幅", "口角度X軸", "口形状上",
			"口形状下", "口形状口角", "耳サイズ", "耳角度Y軸", "耳角度Z軸", "耳上部形状", "耳下部形状"
		};

		public static readonly string[] cm_bodyshapename = new string[21]
		{
			"身長", "胸サイズ", "胸上下位置", "胸の左右開き", "胸上下角度", "頭サイズ", "首周り", "胴体肩周り", "胴体上", "胴体下",
			"腰上", "腰下", "腹", "尻", "太もも上", "太もも下", "脛", "足首", "肩", "上腕",
			"前腕"
		};

		public static readonly string[] cm_headshapename = new string[67]
		{
			"顔全体横幅", "顔上部前後", "顔上部上下", "顔下部前後", "顔下部横幅", "顎横幅", "顎上下", "顎前後", "顎角度", "顎下部上下",
			"顎先幅", "顎先上下", "顎先前後", "頬下部上下", "頬下部前後", "頬下部幅", "頬上部上下", "頬上部前後", "頬上部幅", "眉毛上下",
			"眉毛横位置", "眉毛角度Z軸", "眉毛内側形状", "眉毛外側形状", "目上下", "目横位置", "目前後", "目の横幅", "目の縦幅", "目の角度Z軸",
			"目の角度Y軸", "目頭左右位置", "目尻左右位置", "目頭上下位置", "目尻上下位置", "まぶた形状１", "まぶた形状２", "瞳の上下調整", "瞳の横幅", "瞳の縦幅",
			"鼻全体上下", "鼻全体前後", "鼻全体角度X軸", "鼻全体横幅", "鼻筋高さ", "鼻筋横幅", "鼻筋形状", "小鼻横幅", "小鼻上下", "小鼻前後",
			"小鼻角度X軸", "小鼻角度Z軸", "鼻先高さ", "鼻先角度X軸", "鼻先サイズ", "口上下", "口横幅", "口縦幅", "口角度X軸", "口形状上",
			"口形状下", "口形状口角", "耳サイズ", "耳角度Y軸", "耳角度Z軸", "耳上部形状", "耳下部形状"
		};

		public static readonly string[] AccessoryTypeName = new string[13]
		{
			"なし", "頭", "耳", "眼", "顔", "首", "肩", "胸", "腰", "背",
			"腕", "手", "脚"
		};

		public const float VoicePitchMin = 0.94f;

		public const float VoicePitchMax = 1.06f;
	}
}
