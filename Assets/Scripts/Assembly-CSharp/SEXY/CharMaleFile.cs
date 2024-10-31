using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	public class CharMaleFile : CharFile
	{
		public CharFileInfoCustomMale maleCustomInfo { get; private set; }

		public CharFileInfoClothesMale maleClothesInfo { get; private set; }

		public CharFileInfoCoordinateMale maleCoordinateInfo { get; private set; }

		public CharMaleFile()
			: base("【HoneySelectCharaMale】", "chara/male/")
		{
			base.customInfo = (maleCustomInfo = new CharFileInfoCustomMale());
			base.clothesInfo = (maleClothesInfo = new CharFileInfoClothesMale());
			base.coordinateInfo = (maleCoordinateInfo = new CharFileInfoCoordinateMale());
		}

		public override bool LoadFromSBPR(BinaryReader br)
		{
			CharSave_SexyBeachPR charSave_SexyBeachPR = new CharSave_SexyBeachPR(CharSave_SexyBeachPR.ConvertType.HoneySelect);
			if (!charSave_SexyBeachPR.LoadCharaFile(br))
			{
				return false;
			}
			CharSave_SexyBeachPR.SaveData savedata = charSave_SexyBeachPR.savedata;
			if (savedata.sex == 1)
			{
				Debug.LogWarning("キャラファイルの性別が違います");
				return false;
			}
			FromSBPR(savedata);
			return true;
		}

		private void FromSBPR(CharSave_SexyBeachPR.SaveData savedata)
		{
			charaFileName = savedata.fileName;
			charaFilePNG = savedata.pngData;
			maleCustomInfo.personality = savedata.personality;
			maleCustomInfo.name = savedata.name;
			maleCustomInfo.headId = savedata.headId;
			for (int i = 0; i < savedata.shapeBody.Length; i++)
			{
				maleCustomInfo.shapeValueBody[i] = Mathf.Clamp(savedata.shapeBody[i], 0f, 1f);
			}
			for (int j = 0; j < savedata.shapeFace.Length; j++)
			{
				maleCustomInfo.shapeValueFace[j] = Mathf.Clamp(savedata.shapeFace[j], 0f, 1f);
			}
			for (int k = 0; k < maleCustomInfo.hairId.Length; k++)
			{
				maleCustomInfo.hairId[k] = savedata.hairId[k];
				savedata.hairColor[k].CopyToHoneySelect(maleCustomInfo.hairColor[k]);
				savedata.hairAcsColor[k].CopyToHoneySelect(maleCustomInfo.hairAcsColor[k]);
			}
			maleCustomInfo.texFaceId = savedata.texFaceId;
			maleCustomInfo.matBeardId = savedata.beardId;
			savedata.beardColor.CopyToHoneySelect(maleCustomInfo.beardColor);
			savedata.skinColor.CopyToHoneySelect(maleCustomInfo.skinColor);
			maleCustomInfo.texTattoo_fId = savedata.texTattoo_fId;
			savedata.tattoo_fColor.CopyToHoneySelect(maleCustomInfo.tattoo_fColor);
			maleCustomInfo.matEyebrowId = savedata.matEyebrowId;
			savedata.eyebrowColor.CopyToHoneySelect(maleCustomInfo.eyebrowColor);
			maleCustomInfo.matEyeLId = savedata.matEyeLId;
			savedata.eyeLColor.CopyToHoneySelect(maleCustomInfo.eyeLColor);
			maleCustomInfo.matEyeRId = savedata.matEyeRId;
			savedata.eyeRColor.CopyToHoneySelect(maleCustomInfo.eyeRColor);
			savedata.eyeWColor.CopyToHoneySelect(maleCustomInfo.eyeWColor);
			maleCustomInfo.texFaceDetailId = 1;
			maleCustomInfo.faceDetailWeight = savedata.faceDetailWeight;
			maleCustomInfo.texBodyId = savedata.texBodyId;
			maleCustomInfo.texTattoo_bId = savedata.texTattoo_bId;
			savedata.tattoo_bColor.CopyToHoneySelect(maleCustomInfo.tattoo_bColor);
			maleCustomInfo.texBodyDetailId = savedata.texBodyId + 1;
			maleCustomInfo.bodyDetailWeight = savedata.bodyDetailWeight;
			CharFileInfoClothesMale charFileInfoClothesMale = new CharFileInfoClothesMale();
			charFileInfoClothesMale.clothesId[0] = savedata.coord[0].clothesTopId;
			savedata.coord[0].clothesTopColor.CopyToHoneySelect(charFileInfoClothesMale.clothesColor[0]);
			charFileInfoClothesMale.clothesId[1] = savedata.coord[0].shoesId;
			savedata.coord[0].shoesColor.CopyToHoneySelect(charFileInfoClothesMale.clothesColor[1]);
			for (int l = 0; l < 5; l++)
			{
				charFileInfoClothesMale.accessory[l].type = savedata.accessory[0, l].accessoryType;
				charFileInfoClothesMale.accessory[l].id = savedata.accessory[0, l].accessoryId;
				charFileInfoClothesMale.accessory[l].parentKey = savedata.accessory[0, l].parentKey;
				charFileInfoClothesMale.accessory[l].addPos = savedata.accessory[0, l].plusPos;
				charFileInfoClothesMale.accessory[l].addRot = savedata.accessory[0, l].plusRot;
				charFileInfoClothesMale.accessory[l].addScl = savedata.accessory[0, l].plusScl;
				savedata.accessoryColor[0, l].CopyToHoneySelect(charFileInfoClothesMale.accessory[l].color);
			}
			maleCoordinateInfo.SetInfo(CharDefine.CoordinateType.type00, charFileInfoClothesMale);
			CharFileInfoClothesMale charFileInfoClothesMale2 = new CharFileInfoClothesMale();
			charFileInfoClothesMale2.clothesId[0] = savedata.coord[1].clothesTopId;
			savedata.coord[1].clothesTopColor.CopyToHoneySelect(charFileInfoClothesMale2.clothesColor[0]);
			charFileInfoClothesMale2.clothesId[1] = savedata.coord[1].shoesId;
			savedata.coord[1].shoesColor.CopyToHoneySelect(charFileInfoClothesMale2.clothesColor[1]);
			for (int m = 0; m < 5; m++)
			{
				charFileInfoClothesMale2.accessory[m].type = savedata.accessory[1, m].accessoryType;
				charFileInfoClothesMale2.accessory[m].id = savedata.accessory[1, m].accessoryId;
				charFileInfoClothesMale2.accessory[m].parentKey = savedata.accessory[1, m].parentKey;
				charFileInfoClothesMale2.accessory[m].addPos = savedata.accessory[1, m].plusPos;
				charFileInfoClothesMale2.accessory[m].addRot = savedata.accessory[1, m].plusRot;
				charFileInfoClothesMale2.accessory[m].addScl = savedata.accessory[1, m].plusScl;
				savedata.accessoryColor[1, m].CopyToHoneySelect(charFileInfoClothesMale2.accessory[m].color);
			}
			maleCoordinateInfo.SetInfo(CharDefine.CoordinateType.type02, charFileInfoClothesMale2);
		}
	}
}
