using System;
using System.Collections.Generic;
using IllusionUtility.SetUtility;
using UnityEngine;

public class ShapeHeadInfoFemale : ShapeInfoBase
{
	public enum DstBoneName
	{
		cf_J_CheekLow_L = 0,
		cf_J_CheekLow_R = 1,
		cf_J_CheekUp_L = 2,
		cf_J_CheekUp_R = 3,
		cf_J_Chin_rs = 4,
		cf_J_ChinLow = 5,
		cf_J_ChinTip_s = 6,
		cf_J_EarBase_s_L = 7,
		cf_J_EarBase_s_R = 8,
		cf_J_EarLow_L = 9,
		cf_J_EarLow_R = 10,
		cf_J_EarRing_L = 11,
		cf_J_EarRing_R = 12,
		cf_J_EarUp_L = 13,
		cf_J_EarUp_R = 14,
		cf_J_Eye_r_L = 15,
		cf_J_Eye_r_R = 16,
		cf_J_eye_rs_L = 17,
		cf_J_eye_rs_R = 18,
		cf_J_Eye_s_L = 19,
		cf_J_Eye_s_R = 20,
		cf_J_Eye_t_L = 21,
		cf_J_Eye_t_R = 22,
		cf_J_Eye01_L = 23,
		cf_J_Eye01_R = 24,
		cf_J_Eye02_L = 25,
		cf_J_Eye02_R = 26,
		cf_J_Eye03_L = 27,
		cf_J_Eye03_R = 28,
		cf_J_Eye04_L = 29,
		cf_J_Eye04_R = 30,
		cf_J_EyePos_rz_L = 31,
		cf_J_EyePos_rz_R = 32,
		cf_J_FaceBase = 33,
		cf_J_FaceLow_s = 34,
		cf_J_FaceLowBase = 35,
		cf_J_FaceUp_ty = 36,
		cf_J_FaceUp_tz = 37,
		cf_J_Mayu_L = 38,
		cf_J_Mayu_R = 39,
		cf_J_MayuMid_s_L = 40,
		cf_J_MayuMid_s_R = 41,
		cf_J_MayuTip_s_L = 42,
		cf_J_MayuTip_s_R = 43,
		cf_J_megane = 44,
		cf_J_Mouth_L = 45,
		cf_J_Mouth_R = 46,
		cf_J_MouthLow = 47,
		cf_J_Mouthup = 48,
		cf_J_MouthBase_s = 49,
		cf_J_MouthBase_tr = 50,
		cf_J_Nose_t = 51,
		cf_J_Nose_tip = 52,
		cf_J_NoseBase_s = 53,
		cf_J_NoseBase_trs = 54,
		cf_J_NoseBridge_s = 55,
		cf_J_NoseBridge_t = 56,
		cf_J_NoseWing_tx_L = 57,
		cf_J_NoseWing_tx_R = 58,
		cf_J_pupil_s_L = 59,
		cf_J_pupil_s_R = 60,
		cf_J_MouthCavity = 61
	}

	public enum SrcBoneName
	{
		cf_s_CheekLow_tx_L = 0,
		cf_s_CheekLow_tx_R = 1,
		cf_s_CheekLow_ty = 2,
		cf_s_CheekLow_tz = 3,
		cf_s_CheekUp_tx_L = 4,
		cf_s_CheekUp_tx_R = 5,
		cf_s_CheekUp_ty = 6,
		cf_s_CheekUp_tz_00 = 7,
		cf_s_CheekUp_tz_01 = 8,
		cf_s_Chin_rx = 9,
		cf_s_Chin_sx = 10,
		cf_s_Chin_ty = 11,
		cf_s_Chin_tz = 12,
		cf_s_ChinLow = 13,
		cf_s_ChinTip_sx = 14,
		cf_s_ChinTip_ty = 15,
		cf_s_ChinTip_tz = 16,
		cf_s_EarBase_ry_L = 17,
		cf_s_EarBase_ry_R = 18,
		cf_s_EarBase_rz_L = 19,
		cf_s_EarBase_rz_R = 20,
		cf_s_EarBase_s_L = 21,
		cf_s_EarBase_s_R = 22,
		cf_s_EarLow_L = 23,
		cf_s_EarLow_R = 24,
		cf_s_EarRing_L = 25,
		cf_s_EarRing_R = 26,
		cf_s_EarRing_rz_L = 27,
		cf_s_EarRing_rz_R = 28,
		cf_s_EarRing_s_L = 29,
		cf_s_EarRing_s_R = 30,
		cf_s_EarUp_L = 31,
		cf_s_EarUp_R = 32,
		cf_s_Eye_ry_L = 33,
		cf_s_Eye_ry_R = 34,
		cf_s_Eye_rz_L = 35,
		cf_s_Eye_rz_R = 36,
		cf_s_Eye_sx_L = 37,
		cf_s_Eye_sx_R = 38,
		cf_s_Eye_sy_L = 39,
		cf_s_Eye_sy_R = 40,
		cf_s_Eye_tx_L = 41,
		cf_s_Eye_tx_R = 42,
		cf_s_Eye_ty = 43,
		cf_s_Eye_tz = 44,
		cf_s_Eye01_L = 45,
		cf_s_Eye01_R = 46,
		cf_s_Eye01_rx_L = 47,
		cf_s_Eye01_rx_R = 48,
		cf_s_Eye02_L = 49,
		cf_s_Eye02_R = 50,
		cf_s_Eye02_ry_L = 51,
		cf_s_Eye02_ry_R = 52,
		cf_s_Eye03_L = 53,
		cf_s_Eye03_R = 54,
		cf_s_Eye03_rx_L = 55,
		cf_s_Eye03_rx_R = 56,
		cf_s_Eye04_L = 57,
		cf_s_Eye04_R = 58,
		cf_s_Eye04_ry_L = 59,
		cf_s_Eye04_ry_R = 60,
		cf_s_EyePos_L = 61,
		cf_s_EyePos_R = 62,
		cf_s_EyePos_rz_L = 63,
		cf_s_EyePos_rz_R = 64,
		cf_s_FaceBase_sx = 65,
		cf_s_FaceLow_sx = 66,
		cf_s_FaceLow_tz = 67,
		cf_s_FaceUp_ty = 68,
		cf_s_FaceUp_tz = 69,
		cf_s_Mayu_L = 70,
		cf_s_Mayu_R = 71,
		cf_s_Mayu_tx_L = 72,
		cf_s_Mayu_tx_R = 73,
		cf_s_Mayu_ty = 74,
		cf_s_Mayu02_L = 75,
		cf_s_Mayu02_R = 76,
		cf_s_MayuMid_s_L = 77,
		cf_s_MayuMid_s_R = 78,
		cf_s_MayuTip_s_L = 79,
		cf_s_MayuTip_s_R = 80,
		cf_s_megane_rx_nosebridge = 81,
		cf_s_megane_ty_eye = 82,
		cf_s_megane_ty_nose = 83,
		cf_s_megane_tz_nosebridge = 84,
		cf_s_MouthBase_tz = 85,
		cf_s_Mouthup = 86,
		cf_s_Mouth_L = 87,
		cf_s_Mouth_R = 88,
		cf_s_MouthBase_sx = 89,
		cf_s_MouthBase_sy = 90,
		cf_s_MouthBase_ty = 91,
		cf_s_MouthLow = 92,
		cf_s_Nose_rx = 93,
		cf_s_Nose_tip = 94,
		cf_s_Nose_tz = 95,
		cf_s_NoseBase = 96,
		cf_s_NoseBase_rx = 97,
		cf_s_NoseBase_sx = 98,
		cf_s_NoseBase_ty = 99,
		cf_s_NoseBase_tz = 100,
		cf_s_NoseBridge_rx = 101,
		cf_s_NoseBridge_sx = 102,
		cf_s_NoseBridge_ty = 103,
		cf_s_NoseBridge_tz_00 = 104,
		cf_s_NoseBridge_tz_01 = 105,
		cf_s_NoseWing_rx = 106,
		cf_s_NoseWing_rz_L = 107,
		cf_s_NoseWing_rz_R = 108,
		cf_s_NoseWing_tx_L = 109,
		cf_s_NoseWing_tx_R = 110,
		cf_s_NoseWing_ty = 111,
		cf_s_NoseWing_tz = 112,
		cf_s_pupil_ssx_L = 113,
		cf_s_pupil_ssx_R = 114,
		cf_s_pupil_ssy_L = 115,
		cf_s_pupil_ssy_R = 116,
		cf_s_pupil_sx_L = 117,
		cf_s_pupil_sx_R = 118,
		cf_s_pupil_sy_L = 119,
		cf_s_pupil_sy_R = 120,
		cf_s_MouthC_ty = 121,
		cf_s_MouthC_tz = 122,
		cf_s_MayuMid_ftz = 123,
		cf_s_MayuMid_ntz = 124,
		cf_s_MayuMidre_tz = 125,
		cf_s_MayuTip_esx = 126
	}

	public override void InitShapeInfo(string assetBundleAnmKey, string assetBundleCategory, string anmKeyInfoPath, string cateInfoPath, Transform trfObj)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		DstBoneName[] array = (DstBoneName[])Enum.GetValues(typeof(DstBoneName));
		DstBoneName[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			DstBoneName value = array2[i];
			dictionary[value.ToString()] = (int)value;
		}
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		SrcBoneName[] array3 = (SrcBoneName[])Enum.GetValues(typeof(SrcBoneName));
		SrcBoneName[] array4 = array3;
		for (int j = 0; j < array4.Length; j++)
		{
			SrcBoneName value2 = array4[j];
			dictionary2[value2.ToString()] = (int)value2;
		}
		InitShapeInfoBase(assetBundleAnmKey, assetBundleCategory, anmKeyInfoPath, cateInfoPath, trfObj, dictionary, dictionary2);
		base.InitEnd = true;
	}

	public override void InitShapeInfo(TextAsset anmKeyTxt, TextAsset categoryTxt, Transform trfObj)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		DstBoneName[] array = (DstBoneName[])Enum.GetValues(typeof(DstBoneName));
		DstBoneName[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			DstBoneName value = array2[i];
			dictionary[value.ToString()] = (int)value;
		}
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		SrcBoneName[] array3 = (SrcBoneName[])Enum.GetValues(typeof(SrcBoneName));
		SrcBoneName[] array4 = array3;
		for (int j = 0; j < array4.Length; j++)
		{
			SrcBoneName value2 = array4[j];
			dictionary2[value2.ToString()] = (int)value2;
		}
		InitShapeInfoBase(anmKeyTxt, categoryTxt, trfObj, dictionary, dictionary2);
		base.InitEnd = true;
	}

	public override void Update()
	{
		if (base.InitEnd && dictSrcBoneInfo.Count != 0)
		{
			dictDstBoneInfo[61].trfBone.SetLocalPositionY(dictSrcBoneInfo[121].vctPos.y);
			dictDstBoneInfo[61].trfBone.SetLocalPositionZ(dictSrcBoneInfo[122].vctPos.z + dictSrcBoneInfo[121].vctPos.z);
			dictDstBoneInfo[38].trfBone.SetLocalPositionX(dictSrcBoneInfo[72].vctPos.x);
			dictDstBoneInfo[38].trfBone.SetLocalPositionY(dictSrcBoneInfo[74].vctPos.y);
			dictDstBoneInfo[38].trfBone.SetLocalPositionZ(dictSrcBoneInfo[74].vctPos.z + dictSrcBoneInfo[70].vctPos.z + dictSrcBoneInfo[72].vctPos.z + dictSrcBoneInfo[75].vctPos.z);
			dictDstBoneInfo[38].trfBone.SetLocalRotation(dictSrcBoneInfo[74].vctRot.x + dictSrcBoneInfo[70].vctRot.x + dictSrcBoneInfo[75].vctRot.x, dictSrcBoneInfo[70].vctRot.y + dictSrcBoneInfo[72].vctRot.y, dictSrcBoneInfo[70].vctRot.z);
			dictDstBoneInfo[39].trfBone.SetLocalPositionX(dictSrcBoneInfo[73].vctPos.x);
			dictDstBoneInfo[39].trfBone.SetLocalPositionY(dictSrcBoneInfo[74].vctPos.y);
			dictDstBoneInfo[39].trfBone.SetLocalPositionZ(dictSrcBoneInfo[74].vctPos.z + dictSrcBoneInfo[71].vctPos.z + dictSrcBoneInfo[73].vctPos.z + dictSrcBoneInfo[76].vctPos.z);
			dictDstBoneInfo[39].trfBone.SetLocalRotation(dictSrcBoneInfo[74].vctRot.x + dictSrcBoneInfo[71].vctRot.x + dictSrcBoneInfo[76].vctRot.x, dictSrcBoneInfo[71].vctRot.y + dictSrcBoneInfo[73].vctRot.y, dictSrcBoneInfo[71].vctRot.z);
			dictDstBoneInfo[40].trfBone.SetLocalPositionY(dictSrcBoneInfo[77].vctPos.y);
			dictDstBoneInfo[40].trfBone.SetLocalPositionZ(dictSrcBoneInfo[77].vctPos.z + dictSrcBoneInfo[123].vctPos.z + dictSrcBoneInfo[124].vctPos.z + dictSrcBoneInfo[125].vctPos.z);
			dictDstBoneInfo[40].trfBone.SetLocalRotation(dictSrcBoneInfo[77].vctRot.x + dictSrcBoneInfo[123].vctRot.x + dictSrcBoneInfo[124].vctRot.x + dictSrcBoneInfo[125].vctRot.x, dictSrcBoneInfo[77].vctRot.y + dictSrcBoneInfo[125].vctRot.y, dictSrcBoneInfo[77].vctRot.z);
			dictDstBoneInfo[41].trfBone.SetLocalPositionY(dictSrcBoneInfo[78].vctPos.y);
			dictDstBoneInfo[41].trfBone.SetLocalPositionZ(dictSrcBoneInfo[78].vctPos.z + dictSrcBoneInfo[123].vctPos.z + dictSrcBoneInfo[124].vctPos.z + dictSrcBoneInfo[125].vctPos.z);
			dictDstBoneInfo[41].trfBone.SetLocalRotation(dictSrcBoneInfo[78].vctRot.x + dictSrcBoneInfo[123].vctRot.x + dictSrcBoneInfo[124].vctRot.x + dictSrcBoneInfo[125].vctRot.x, dictSrcBoneInfo[78].vctRot.y - dictSrcBoneInfo[125].vctRot.y, dictSrcBoneInfo[78].vctRot.z);
			dictDstBoneInfo[42].trfBone.SetLocalPositionY(dictSrcBoneInfo[79].vctPos.y);
			dictDstBoneInfo[42].trfBone.SetLocalPositionZ(dictSrcBoneInfo[79].vctPos.z + dictSrcBoneInfo[126].vctPos.z);
			dictDstBoneInfo[42].trfBone.SetLocalRotation(dictSrcBoneInfo[79].vctRot.x + dictSrcBoneInfo[126].vctRot.x, dictSrcBoneInfo[79].vctRot.y, dictSrcBoneInfo[79].vctRot.z);
			dictDstBoneInfo[43].trfBone.SetLocalPositionY(dictSrcBoneInfo[80].vctPos.y);
			dictDstBoneInfo[43].trfBone.SetLocalPositionZ(dictSrcBoneInfo[80].vctPos.z + dictSrcBoneInfo[126].vctPos.z);
			dictDstBoneInfo[43].trfBone.SetLocalRotation(dictSrcBoneInfo[80].vctRot.x + dictSrcBoneInfo[126].vctRot.x, dictSrcBoneInfo[80].vctRot.y, dictSrcBoneInfo[80].vctRot.z);
			dictDstBoneInfo[21].trfBone.SetLocalPositionX(dictSrcBoneInfo[41].vctPos.x);
			dictDstBoneInfo[21].trfBone.SetLocalPositionY(dictSrcBoneInfo[43].vctPos.y);
			dictDstBoneInfo[21].trfBone.SetLocalPositionZ(dictSrcBoneInfo[44].vctPos.z);
			dictDstBoneInfo[21].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[35].vctRot.z);
			dictDstBoneInfo[22].trfBone.SetLocalPositionX(dictSrcBoneInfo[42].vctPos.x);
			dictDstBoneInfo[22].trfBone.SetLocalPositionY(dictSrcBoneInfo[43].vctPos.y);
			dictDstBoneInfo[22].trfBone.SetLocalPositionZ(dictSrcBoneInfo[44].vctPos.z);
			dictDstBoneInfo[22].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[36].vctRot.z);
			dictDstBoneInfo[19].trfBone.SetLocalScale(dictSrcBoneInfo[37].vctScl.x, dictSrcBoneInfo[39].vctScl.y, 1f);
			dictDstBoneInfo[15].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[33].vctRot.y, 0f);
			dictDstBoneInfo[20].trfBone.SetLocalScale(dictSrcBoneInfo[38].vctScl.x, dictSrcBoneInfo[40].vctScl.y, 1f);
			dictDstBoneInfo[16].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[34].vctRot.y, 0f);
			dictDstBoneInfo[23].trfBone.SetLocalRotation(dictSrcBoneInfo[47].vctRot.x, dictSrcBoneInfo[45].vctRot.y + dictSrcBoneInfo[47].vctRot.y, 0f);
			dictDstBoneInfo[25].trfBone.SetLocalRotation(dictSrcBoneInfo[49].vctRot.x, dictSrcBoneInfo[51].vctRot.y, dictSrcBoneInfo[51].vctRot.z);
			dictDstBoneInfo[27].trfBone.SetLocalPositionX(dictSrcBoneInfo[53].vctPos.x);
			dictDstBoneInfo[27].trfBone.SetLocalRotation(dictSrcBoneInfo[55].vctRot.x, dictSrcBoneInfo[53].vctRot.y, 0f);
			dictDstBoneInfo[29].trfBone.SetLocalRotation(dictSrcBoneInfo[57].vctRot.x, dictSrcBoneInfo[59].vctRot.y, dictSrcBoneInfo[59].vctRot.z);
			dictDstBoneInfo[24].trfBone.SetLocalRotation(dictSrcBoneInfo[48].vctRot.x, dictSrcBoneInfo[46].vctRot.y + dictSrcBoneInfo[48].vctRot.y, 0f);
			dictDstBoneInfo[26].trfBone.SetLocalRotation(dictSrcBoneInfo[50].vctRot.x, dictSrcBoneInfo[52].vctRot.y, dictSrcBoneInfo[52].vctRot.z);
			dictDstBoneInfo[28].trfBone.SetLocalPositionX(dictSrcBoneInfo[54].vctPos.x);
			dictDstBoneInfo[28].trfBone.SetLocalRotation(dictSrcBoneInfo[56].vctRot.x, dictSrcBoneInfo[54].vctRot.y, 0f);
			dictDstBoneInfo[30].trfBone.SetLocalRotation(dictSrcBoneInfo[58].vctRot.x, dictSrcBoneInfo[60].vctRot.y, dictSrcBoneInfo[60].vctRot.z);
			dictDstBoneInfo[31].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[63].vctRot.z);
			dictDstBoneInfo[17].trfBone.SetLocalRotation(dictSrcBoneInfo[61].vctRot.x, 0f, 0f);
			dictDstBoneInfo[32].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[64].vctRot.z);
			dictDstBoneInfo[18].trfBone.SetLocalRotation(dictSrcBoneInfo[62].vctRot.x, 0f, 0f);
			dictDstBoneInfo[59].trfBone.SetLocalPositionY(dictSrcBoneInfo[119].vctPos.y);
			dictDstBoneInfo[59].trfBone.SetLocalPositionZ(dictSrcBoneInfo[113].vctPos.z + dictSrcBoneInfo[115].vctPos.z + dictSrcBoneInfo[117].vctPos.z + dictSrcBoneInfo[119].vctPos.z);
			dictDstBoneInfo[59].trfBone.SetLocalScale(dictSrcBoneInfo[113].vctScl.x * dictSrcBoneInfo[115].vctScl.x * dictSrcBoneInfo[117].vctScl.x, dictSrcBoneInfo[113].vctScl.y * dictSrcBoneInfo[115].vctScl.y * dictSrcBoneInfo[119].vctScl.y, dictSrcBoneInfo[117].vctScl.z * dictSrcBoneInfo[119].vctScl.z);
			dictDstBoneInfo[60].trfBone.SetLocalPositionY(dictSrcBoneInfo[120].vctPos.y);
			dictDstBoneInfo[60].trfBone.SetLocalPositionZ(dictSrcBoneInfo[114].vctPos.z + dictSrcBoneInfo[116].vctPos.z + dictSrcBoneInfo[118].vctPos.z + dictSrcBoneInfo[120].vctPos.z);
			dictDstBoneInfo[60].trfBone.SetLocalScale(dictSrcBoneInfo[114].vctScl.x * dictSrcBoneInfo[116].vctScl.x * dictSrcBoneInfo[118].vctScl.x, dictSrcBoneInfo[114].vctScl.y * dictSrcBoneInfo[116].vctScl.y * dictSrcBoneInfo[120].vctScl.y, dictSrcBoneInfo[118].vctScl.z * dictSrcBoneInfo[120].vctScl.z);
			dictDstBoneInfo[33].trfBone.SetLocalScale(dictSrcBoneInfo[65].vctScl.x, 1f, 1f);
			dictDstBoneInfo[36].trfBone.SetLocalPositionY(dictSrcBoneInfo[68].vctPos.y);
			dictDstBoneInfo[37].trfBone.SetLocalPositionZ(dictSrcBoneInfo[69].vctPos.z);
			dictDstBoneInfo[2].trfBone.SetLocalPositionX(dictSrcBoneInfo[4].vctPos.x);
			dictDstBoneInfo[2].trfBone.SetLocalPositionY(dictSrcBoneInfo[6].vctPos.y);
			dictDstBoneInfo[2].trfBone.SetLocalPositionZ(dictSrcBoneInfo[7].vctPos.z + dictSrcBoneInfo[8].vctPos.z);
			dictDstBoneInfo[3].trfBone.SetLocalPositionX(dictSrcBoneInfo[5].vctPos.x);
			dictDstBoneInfo[3].trfBone.SetLocalPositionY(dictSrcBoneInfo[6].vctPos.y);
			dictDstBoneInfo[3].trfBone.SetLocalPositionZ(dictSrcBoneInfo[7].vctPos.z + dictSrcBoneInfo[8].vctPos.z);
			dictDstBoneInfo[0].trfBone.SetLocalPositionX(dictSrcBoneInfo[0].vctPos.x);
			dictDstBoneInfo[0].trfBone.SetLocalPositionY(dictSrcBoneInfo[2].vctPos.y);
			dictDstBoneInfo[0].trfBone.SetLocalPositionZ(dictSrcBoneInfo[3].vctPos.z);
			dictDstBoneInfo[1].trfBone.SetLocalPositionX(dictSrcBoneInfo[1].vctPos.x);
			dictDstBoneInfo[1].trfBone.SetLocalPositionY(dictSrcBoneInfo[2].vctPos.y);
			dictDstBoneInfo[1].trfBone.SetLocalPositionZ(dictSrcBoneInfo[3].vctPos.z);
			dictDstBoneInfo[35].trfBone.SetLocalPositionZ(dictSrcBoneInfo[67].vctPos.z);
			dictDstBoneInfo[34].trfBone.SetLocalScale(dictSrcBoneInfo[66].vctScl.x, 1f, 1f);
			dictDstBoneInfo[50].trfBone.SetLocalPositionY(dictSrcBoneInfo[91].vctPos.y);
			dictDstBoneInfo[50].trfBone.SetLocalPositionZ(dictSrcBoneInfo[91].vctPos.z + dictSrcBoneInfo[85].vctPos.z);
			dictDstBoneInfo[49].trfBone.SetLocalScale(dictSrcBoneInfo[89].vctScl.x, dictSrcBoneInfo[90].vctScl.y, 1f);
			dictDstBoneInfo[48].trfBone.SetLocalPositionY(dictSrcBoneInfo[86].vctPos.y);
			dictDstBoneInfo[47].trfBone.SetLocalPositionY(dictSrcBoneInfo[92].vctPos.y);
			dictDstBoneInfo[47].trfBone.SetLocalPositionZ(dictSrcBoneInfo[92].vctPos.z);
			dictDstBoneInfo[47].trfBone.SetLocalScale(dictSrcBoneInfo[92].vctScl.x, 1f, 1f);
			dictDstBoneInfo[45].trfBone.SetLocalPositionY(dictSrcBoneInfo[87].vctPos.y);
			dictDstBoneInfo[45].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[87].vctRot.z);
			dictDstBoneInfo[46].trfBone.SetLocalPositionY(dictSrcBoneInfo[88].vctPos.y);
			dictDstBoneInfo[46].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[88].vctRot.z);
			dictDstBoneInfo[5].trfBone.SetLocalPositionY(dictSrcBoneInfo[13].vctPos.y);
			dictDstBoneInfo[4].trfBone.SetLocalPositionY(dictSrcBoneInfo[11].vctPos.y + dictSrcBoneInfo[9].vctPos.y);
			dictDstBoneInfo[4].trfBone.SetLocalPositionZ(dictSrcBoneInfo[12].vctPos.z + dictSrcBoneInfo[9].vctPos.z);
			dictDstBoneInfo[4].trfBone.SetLocalRotation(dictSrcBoneInfo[9].vctRot.x, 0f, 0f);
			dictDstBoneInfo[4].trfBone.SetLocalScale(dictSrcBoneInfo[10].vctScl.x, 1f, 1f);
			dictDstBoneInfo[6].trfBone.SetLocalPositionY(dictSrcBoneInfo[15].vctPos.y);
			dictDstBoneInfo[6].trfBone.SetLocalPositionZ(dictSrcBoneInfo[16].vctPos.z);
			dictDstBoneInfo[6].trfBone.SetLocalScale(dictSrcBoneInfo[14].vctScl.x, dictSrcBoneInfo[14].vctScl.y, dictSrcBoneInfo[14].vctScl.z);
			dictDstBoneInfo[56].trfBone.SetLocalPositionY(dictSrcBoneInfo[103].vctPos.y);
			dictDstBoneInfo[56].trfBone.SetLocalPositionZ(dictSrcBoneInfo[104].vctPos.z + dictSrcBoneInfo[105].vctPos.z + dictSrcBoneInfo[103].vctPos.z + dictSrcBoneInfo[101].vctPos.z);
			dictDstBoneInfo[56].trfBone.SetLocalRotation(dictSrcBoneInfo[101].vctRot.x, 0f, 0f);
			dictDstBoneInfo[55].trfBone.SetLocalScale(dictSrcBoneInfo[102].vctScl.x, 1f, 1f);
			dictDstBoneInfo[54].trfBone.SetLocalPositionY(dictSrcBoneInfo[97].vctPos.y + dictSrcBoneInfo[99].vctPos.y + dictSrcBoneInfo[96].vctPos.y);
			dictDstBoneInfo[54].trfBone.SetLocalPositionZ(dictSrcBoneInfo[97].vctPos.z + dictSrcBoneInfo[100].vctPos.z + dictSrcBoneInfo[96].vctPos.z);
			dictDstBoneInfo[53].trfBone.SetLocalRotation(dictSrcBoneInfo[97].vctRot.x + dictSrcBoneInfo[96].vctRot.x, 0f, 0f);
			dictDstBoneInfo[53].trfBone.SetLocalScale(dictSrcBoneInfo[98].vctScl.x, dictSrcBoneInfo[98].vctScl.y, dictSrcBoneInfo[98].vctScl.z);
			dictDstBoneInfo[57].trfBone.SetLocalPositionX(dictSrcBoneInfo[109].vctPos.x);
			dictDstBoneInfo[57].trfBone.SetLocalPositionY(dictSrcBoneInfo[111].vctPos.y);
			dictDstBoneInfo[57].trfBone.SetLocalPositionZ(dictSrcBoneInfo[112].vctPos.z);
			dictDstBoneInfo[57].trfBone.SetLocalRotation(dictSrcBoneInfo[106].vctRot.x, 0f, dictSrcBoneInfo[107].vctRot.z);
			dictDstBoneInfo[58].trfBone.SetLocalPositionX(dictSrcBoneInfo[110].vctPos.x);
			dictDstBoneInfo[58].trfBone.SetLocalPositionY(dictSrcBoneInfo[111].vctPos.y);
			dictDstBoneInfo[58].trfBone.SetLocalPositionZ(dictSrcBoneInfo[112].vctPos.z);
			dictDstBoneInfo[58].trfBone.SetLocalRotation(dictSrcBoneInfo[106].vctRot.x, 0f, dictSrcBoneInfo[108].vctRot.z);
			dictDstBoneInfo[52].trfBone.SetLocalPositionY(dictSrcBoneInfo[94].vctPos.y);
			dictDstBoneInfo[52].trfBone.SetLocalPositionZ(dictSrcBoneInfo[94].vctPos.z);
			dictDstBoneInfo[52].trfBone.SetLocalScale(dictSrcBoneInfo[94].vctScl.x, dictSrcBoneInfo[94].vctScl.y, dictSrcBoneInfo[94].vctScl.z);
			dictDstBoneInfo[51].trfBone.SetLocalPositionY(dictSrcBoneInfo[93].vctPos.y);
			dictDstBoneInfo[51].trfBone.SetLocalPositionZ(dictSrcBoneInfo[95].vctPos.z);
			dictDstBoneInfo[51].trfBone.SetLocalRotation(dictSrcBoneInfo[93].vctRot.x, 0f, 0f);
			dictDstBoneInfo[44].trfBone.SetLocalPositionY(dictSrcBoneInfo[83].vctPos.y + dictSrcBoneInfo[81].vctPos.y + dictSrcBoneInfo[82].vctPos.y);
			dictDstBoneInfo[44].trfBone.SetLocalPositionZ(dictSrcBoneInfo[83].vctPos.z + dictSrcBoneInfo[84].vctPos.z + dictSrcBoneInfo[82].vctPos.z);
			dictDstBoneInfo[44].trfBone.SetLocalRotation(dictSrcBoneInfo[83].vctRot.x + dictSrcBoneInfo[81].vctRot.x + dictSrcBoneInfo[82].vctRot.x, 0f, 0f);
			dictDstBoneInfo[7].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[17].vctRot.y, dictSrcBoneInfo[19].vctRot.z);
			dictDstBoneInfo[7].trfBone.SetLocalScale(dictSrcBoneInfo[21].vctScl.x, dictSrcBoneInfo[21].vctScl.y, dictSrcBoneInfo[21].vctScl.z);
			dictDstBoneInfo[13].trfBone.SetLocalPositionX(dictSrcBoneInfo[31].vctPos.x);
			dictDstBoneInfo[13].trfBone.SetLocalPositionY(dictSrcBoneInfo[31].vctPos.y);
			dictDstBoneInfo[13].trfBone.SetLocalPositionZ(dictSrcBoneInfo[31].vctPos.z);
			dictDstBoneInfo[13].trfBone.SetLocalRotation(dictSrcBoneInfo[31].vctRot.x, dictSrcBoneInfo[31].vctRot.y, 0f);
			dictDstBoneInfo[13].trfBone.SetLocalScale(dictSrcBoneInfo[31].vctScl.x, dictSrcBoneInfo[31].vctScl.y, dictSrcBoneInfo[31].vctScl.z);
			dictDstBoneInfo[9].trfBone.SetLocalPositionY(dictSrcBoneInfo[23].vctPos.y);
			dictDstBoneInfo[9].trfBone.SetLocalPositionZ(dictSrcBoneInfo[23].vctPos.z);
			dictDstBoneInfo[9].trfBone.SetLocalScale(dictSrcBoneInfo[23].vctScl.x, dictSrcBoneInfo[23].vctScl.y, dictSrcBoneInfo[23].vctScl.z);
			dictDstBoneInfo[8].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[18].vctRot.y, dictSrcBoneInfo[20].vctRot.z);
			dictDstBoneInfo[8].trfBone.SetLocalScale(dictSrcBoneInfo[22].vctScl.x, dictSrcBoneInfo[22].vctScl.y, dictSrcBoneInfo[22].vctScl.z);
			dictDstBoneInfo[14].trfBone.SetLocalPositionX(dictSrcBoneInfo[32].vctPos.x);
			dictDstBoneInfo[14].trfBone.SetLocalPositionY(dictSrcBoneInfo[32].vctPos.y);
			dictDstBoneInfo[14].trfBone.SetLocalPositionZ(dictSrcBoneInfo[32].vctPos.z);
			dictDstBoneInfo[14].trfBone.SetLocalRotation(dictSrcBoneInfo[32].vctRot.x, dictSrcBoneInfo[32].vctRot.y, 0f);
			dictDstBoneInfo[14].trfBone.SetLocalScale(dictSrcBoneInfo[32].vctScl.x, dictSrcBoneInfo[32].vctScl.y, dictSrcBoneInfo[32].vctScl.z);
			dictDstBoneInfo[10].trfBone.SetLocalPositionY(dictSrcBoneInfo[24].vctPos.y);
			dictDstBoneInfo[10].trfBone.SetLocalPositionZ(dictSrcBoneInfo[24].vctPos.z);
			dictDstBoneInfo[10].trfBone.SetLocalScale(dictSrcBoneInfo[24].vctScl.x, dictSrcBoneInfo[24].vctScl.y, dictSrcBoneInfo[24].vctScl.z);
			dictDstBoneInfo[11].trfBone.SetLocalPositionY(dictSrcBoneInfo[25].vctPos.y);
			dictDstBoneInfo[11].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[27].vctRot.z);
			dictDstBoneInfo[11].trfBone.SetLocalScale(dictSrcBoneInfo[29].vctScl.x, dictSrcBoneInfo[29].vctScl.y, dictSrcBoneInfo[29].vctScl.z);
			dictDstBoneInfo[12].trfBone.SetLocalPositionY(dictSrcBoneInfo[26].vctPos.y);
			dictDstBoneInfo[12].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[28].vctRot.z);
			dictDstBoneInfo[12].trfBone.SetLocalScale(dictSrcBoneInfo[30].vctScl.x, dictSrcBoneInfo[30].vctScl.y, dictSrcBoneInfo[30].vctScl.z);
		}
	}
}
