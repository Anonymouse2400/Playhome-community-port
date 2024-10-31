using System;
using System.Collections.Generic;
using IllusionUtility.SetUtility;
using UnityEngine;

public class ShapeHeadInfoMale : ShapeInfoBase
{
	public enum DstBoneName
	{
		cm_J_Mayu_L = 0,
		cm_J_Mayu_R = 1,
		cm_J_MayuMid_s_L = 2,
		cm_J_MayuMid_s_R = 3,
		cm_J_MayuTip_s_L = 4,
		cm_J_MayuTip_s_R = 5,
		cm_J_Mayu_C = 6,
		cm_J_Eye_t_L = 7,
		cm_J_Eye_t_R = 8,
		cm_J_Eye_s_L = 9,
		cm_J_Eye_r_L = 10,
		cm_J_Eye_s_R = 11,
		cm_J_Eye_r_R = 12,
		cm_J_Eye01_L = 13,
		cm_J_Eye02_L = 14,
		cm_J_Eye03_L = 15,
		cm_J_Eye04_L = 16,
		cm_J_Eye01_R = 17,
		cm_J_Eye02_R = 18,
		cm_J_Eye03_R = 19,
		cm_J_Eye04_R = 20,
		cm_J_EyePos_rz_L = 21,
		cm_J_eye_rs_L = 22,
		cm_J_EyePos_rz_R = 23,
		cm_J_eye_rs_R = 24,
		cm_J_pupil_s_L = 25,
		cm_J_pupil_s_R = 26,
		cm_J_FaceBase = 27,
		cm_J_FaceUp_ty = 28,
		cm_J_FaceUp_tz = 29,
		cm_J_CheekUp_L = 30,
		cm_J_CheekUp_R = 31,
		cm_J_CheekLow_L = 32,
		cm_J_CheekLow_R = 33,
		cm_J_FaceLowBase = 34,
		cm_J_FaceLow_s = 35,
		cm_J_MouthBase_tr = 36,
		cm_J_MouthBase_s = 37,
		cm_J_Mouthup = 38,
		cm_J_MouthLow = 39,
		cm_J_Mouth_L = 40,
		cm_J_Mouth_R = 41,
		cm_J_ChinLow = 42,
		cm_J_Chin_rs = 43,
		cm_J_ChinTip_s = 44,
		cm_J_NoseBridge_t = 45,
		cm_J_NoseBridge_s = 46,
		cm_J_NoseBase_trs = 47,
		cm_J_NoseBase_s = 48,
		cm_J_NoseWing_tx_L = 49,
		cm_J_NoseWing_tx_R = 50,
		cm_J_Nose_tip = 51,
		cm_J_Nose_t = 52,
		cm_J_megane = 53,
		cm_J_EarBase_s_L = 54,
		cm_J_EarUp_L = 55,
		cm_J_EarLow_L = 56,
		cm_J_EarBase_s_R = 57,
		cm_J_EarUp_R = 58,
		cm_J_EarLow_R = 59,
		cm_J_EarRing_L = 60,
		cm_J_EarRing_R = 61
	}

	public enum SrcBoneName
	{
		cm_S_Mayu_L = 0,
		cm_S_Mayu_ty = 1,
		cm_S_Mayu_drx_L = 2,
		cm_S_Mayu_dtz = 3,
		cm_S_Mayu_R = 4,
		cm_S_Mayu_drx_R = 5,
		cm_S_MayuMid_s_L = 6,
		cm_S_MayuMid_s_R = 7,
		cm_S_MayuTip_s_L = 8,
		cm_S_MayuTip_s_R = 9,
		cm_S_Mayu_rz_C = 10,
		cm_S_MayuMid_C = 11,
		cm_S_Mayu_C_nosetz = 12,
		cm_S_Eye_tx_L = 13,
		cm_S_Eye_ty = 14,
		cm_S_Eye_tz = 15,
		cm_S_Eye_rz_L = 16,
		cm_S_Eye_tx_R = 17,
		cm_S_Eye_rz_R = 18,
		cm_S_Eye_sx_L = 19,
		cm_S_Eye_sy_L = 20,
		cm_S_Eye_ry_L = 21,
		cm_S_Eye_sx_R = 22,
		cm_S_Eye_sy_R = 23,
		cm_S_Eye_ry_R = 24,
		cm_S_Eye01_rx_L = 25,
		cm_S_Eye01_L = 26,
		cm_S_Eye02_L = 27,
		cm_S_Eye02_ry_L = 28,
		cm_S_Eye03_L = 29,
		cm_S_Eye03_rx_L = 30,
		cm_S_Eye04_L = 31,
		cm_S_Eye04_ry_L = 32,
		cm_S_Eye01_rx_R = 33,
		cm_S_Eye01_R = 34,
		cm_S_Eye02_R = 35,
		cm_S_Eye02_ry_R = 36,
		cm_S_Eye03_R = 37,
		cm_S_Eye03_rx_R = 38,
		cm_S_Eye04_R = 39,
		cm_S_Eye04_ry_R = 40,
		cm_S_EyePos_rz_L = 41,
		cm_S_EyePos_L = 42,
		cm_S_EyePos_rz_R = 43,
		cm_S_EyePos_R = 44,
		cm_S_eye_sy_L = 45,
		cm_S_eye_ssx_L = 46,
		cm_S_eye_ssy_L = 47,
		cm_S_eye_sx_L = 48,
		cm_S_eye_tz = 49,
		cm_S_eye_sy_R = 50,
		cm_S_eye_ssx_R = 51,
		cm_S_eye_ssy_R = 52,
		cm_S_eye_sx_R = 53,
		cm_S_FaceBase_sx = 54,
		cm_S_FaceUp_ty = 55,
		cm_S_FaceUp_tz = 56,
		cm_S_CheekUp_tx_L = 57,
		cm_S_CheekUp_ty = 58,
		cm_S_CheekUp_dty = 59,
		cm_S_CheekUp_tz_00 = 60,
		cm_S_CheekUp_tz_01 = 61,
		cm_S_CheekUp_tx_R = 62,
		cm_S_CheekLow_tx_L = 63,
		cm_S_CheekLow_ty = 64,
		cm_S_CheekLow_tz = 65,
		cm_S_CheekLow_tx_R = 66,
		cm_S_FaceLow_tz = 67,
		cm_S_FaceLow_sx = 68,
		cm_S_MouthBase_ty = 69,
		cm_S_MouthBase_tz = 70,
		cm_S_MouthBase_sx = 71,
		cm_S_MouthBase_sy = 72,
		cm_S_Mouthup = 73,
		cm_S_MouthLow = 74,
		cm_S_Mouth_L = 75,
		cm_S_Mouth_R = 76,
		cm_S_ChinLow = 77,
		cm_S_Chin_ty = 78,
		cm_S_Chin_rx = 79,
		cm_S_Chin_tz = 80,
		cm_S_Chin_sx = 81,
		cm_S_ChinTip_ty = 82,
		cm_S_ChinTip_tz = 83,
		cm_S_ChinTip_sx = 84,
		cm_S_NoseBridge_ty = 85,
		cm_S_NoseBridge_tz_00 = 86,
		cm_S_NoseBridge_tz_01 = 87,
		cm_S_NoseBridge_rx = 88,
		cm_S_NoseBridge_sx = 89,
		cm_S_NoseBase_rx = 90,
		cm_S_NoseBase_ty = 91,
		cm_S_NoseBase = 92,
		cm_S_NoseBase_tz = 93,
		cm_S_NoseBase_sx = 94,
		cm_S_NoseWing_tx_L = 95,
		cm_S_NoseWing_ty = 96,
		cm_S_NoseWing_tz = 97,
		cm_S_NoseWing_rx = 98,
		cm_S_NoseWing_rz_L = 99,
		cm_S_NoseWing_tx_R = 100,
		cm_S_NoseWing_rz_R = 101,
		cm_S_Nose_tip = 102,
		cm_S_Nose_rx = 103,
		cm_S_Nose_tz = 104,
		cm_S_Nose_sx = 105,
		cm_S_megane_ty_nose = 106,
		cm_S_megane_rx_nosebridge = 107,
		cm_S_megane_ty_eye = 108,
		cm_S_megane_tz_nosebridge = 109,
		cm_S_EarBase_ry_L = 110,
		cm_S_EarBase_rz_L = 111,
		cm_S_EarBase_s_L = 112,
		cm_S_EarUp_L = 113,
		cm_S_EarLow_L = 114,
		cm_S_EarBase_ry_R = 115,
		cm_S_EarBase_rz_R = 116,
		cm_S_EarBase_s_R = 117,
		cm_S_EarUp_R = 118,
		cm_S_EarLow_R = 119,
		cm_S_EarRing_L = 120,
		cm_S_EarRing_rz_L = 121,
		cm_S_EarRing_s_L = 122,
		cm_S_EarRing_R = 123,
		cm_S_EarRing_rz_R = 124,
		cm_S_EarRing_s_R = 125
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
			dictDstBoneInfo[0].trfBone.SetLocalPositionX(dictSrcBoneInfo[0].vctPos.x);
			dictDstBoneInfo[0].trfBone.SetLocalPositionY(dictSrcBoneInfo[1].vctPos.y);
			dictDstBoneInfo[0].trfBone.SetLocalPositionZ(dictSrcBoneInfo[1].vctPos.z + dictSrcBoneInfo[0].vctPos.z + dictSrcBoneInfo[2].vctPos.z + dictSrcBoneInfo[3].vctPos.z);
			dictDstBoneInfo[0].trfBone.SetLocalRotation(dictSrcBoneInfo[1].vctRot.x + dictSrcBoneInfo[3].vctRot.x, dictSrcBoneInfo[0].vctRot.y + dictSrcBoneInfo[2].vctRot.y, dictSrcBoneInfo[2].vctRot.z);
			dictDstBoneInfo[1].trfBone.SetLocalPositionX(dictSrcBoneInfo[4].vctPos.x);
			dictDstBoneInfo[1].trfBone.SetLocalPositionY(dictSrcBoneInfo[1].vctPos.y);
			dictDstBoneInfo[1].trfBone.SetLocalPositionZ(dictSrcBoneInfo[1].vctPos.z + dictSrcBoneInfo[4].vctPos.z + dictSrcBoneInfo[5].vctPos.z + dictSrcBoneInfo[3].vctPos.z);
			dictDstBoneInfo[1].trfBone.SetLocalRotation(dictSrcBoneInfo[1].vctRot.x + dictSrcBoneInfo[3].vctRot.x, dictSrcBoneInfo[4].vctRot.y + dictSrcBoneInfo[5].vctRot.y, dictSrcBoneInfo[5].vctRot.z);
			dictDstBoneInfo[2].trfBone.SetLocalPositionY(dictSrcBoneInfo[6].vctPos.y);
			dictDstBoneInfo[2].trfBone.SetLocalPositionZ(dictSrcBoneInfo[6].vctPos.z);
			dictDstBoneInfo[2].trfBone.SetLocalRotation(dictSrcBoneInfo[6].vctRot.x, dictSrcBoneInfo[6].vctRot.y, dictSrcBoneInfo[6].vctRot.z);
			dictDstBoneInfo[3].trfBone.SetLocalPositionY(dictSrcBoneInfo[7].vctPos.y);
			dictDstBoneInfo[3].trfBone.SetLocalPositionZ(dictSrcBoneInfo[7].vctPos.z);
			dictDstBoneInfo[3].trfBone.SetLocalRotation(dictSrcBoneInfo[7].vctRot.x, dictSrcBoneInfo[7].vctRot.y, dictSrcBoneInfo[7].vctRot.z);
			dictDstBoneInfo[4].trfBone.SetLocalPositionY(dictSrcBoneInfo[8].vctPos.y);
			dictDstBoneInfo[4].trfBone.SetLocalPositionZ(dictSrcBoneInfo[8].vctPos.z);
			dictDstBoneInfo[4].trfBone.SetLocalRotation(dictSrcBoneInfo[8].vctRot.x, dictSrcBoneInfo[8].vctRot.y, dictSrcBoneInfo[8].vctRot.z);
			dictDstBoneInfo[5].trfBone.SetLocalPositionY(dictSrcBoneInfo[9].vctPos.y);
			dictDstBoneInfo[5].trfBone.SetLocalPositionZ(dictSrcBoneInfo[9].vctPos.z);
			dictDstBoneInfo[5].trfBone.SetLocalRotation(dictSrcBoneInfo[9].vctRot.x, dictSrcBoneInfo[9].vctRot.y, dictSrcBoneInfo[9].vctRot.z);
			dictDstBoneInfo[6].trfBone.SetLocalPositionX(dictSrcBoneInfo[10].vctPos.x);
			dictDstBoneInfo[6].trfBone.SetLocalPositionY(dictSrcBoneInfo[1].vctPos.y + dictSrcBoneInfo[10].vctPos.y + dictSrcBoneInfo[11].vctPos.y);
			dictDstBoneInfo[6].trfBone.SetLocalPositionZ(dictSrcBoneInfo[1].vctPos.z + dictSrcBoneInfo[10].vctPos.z + dictSrcBoneInfo[11].vctPos.z + dictSrcBoneInfo[0].vctPos.z + dictSrcBoneInfo[12].vctPos.z + dictSrcBoneInfo[3].vctPos.z);
			dictDstBoneInfo[6].trfBone.SetLocalRotation(dictSrcBoneInfo[1].vctRot.x + dictSrcBoneInfo[10].vctRot.x + dictSrcBoneInfo[11].vctRot.x + dictSrcBoneInfo[12].vctRot.x, 0f, 0f);
			dictDstBoneInfo[7].trfBone.SetLocalPositionX(dictSrcBoneInfo[13].vctPos.x);
			dictDstBoneInfo[7].trfBone.SetLocalPositionY(dictSrcBoneInfo[14].vctPos.y);
			dictDstBoneInfo[7].trfBone.SetLocalPositionZ(dictSrcBoneInfo[15].vctPos.z);
			dictDstBoneInfo[7].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[16].vctRot.z);
			dictDstBoneInfo[8].trfBone.SetLocalPositionX(dictSrcBoneInfo[17].vctPos.x);
			dictDstBoneInfo[8].trfBone.SetLocalPositionY(dictSrcBoneInfo[14].vctPos.y);
			dictDstBoneInfo[8].trfBone.SetLocalPositionZ(dictSrcBoneInfo[15].vctPos.z);
			dictDstBoneInfo[8].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[18].vctRot.z);
			dictDstBoneInfo[9].trfBone.SetLocalScale(dictSrcBoneInfo[19].vctScl.x, dictSrcBoneInfo[20].vctScl.y, 1f);
			dictDstBoneInfo[10].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[21].vctRot.y, 0f);
			dictDstBoneInfo[11].trfBone.SetLocalScale(dictSrcBoneInfo[22].vctScl.x, dictSrcBoneInfo[23].vctScl.y, 1f);
			dictDstBoneInfo[12].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[24].vctRot.y, 0f);
			dictDstBoneInfo[13].trfBone.SetLocalRotation(dictSrcBoneInfo[25].vctRot.x, dictSrcBoneInfo[26].vctRot.y + dictSrcBoneInfo[25].vctRot.y, 0f);
			dictDstBoneInfo[14].trfBone.SetLocalRotation(dictSrcBoneInfo[27].vctRot.x, dictSrcBoneInfo[28].vctRot.y, dictSrcBoneInfo[28].vctRot.z);
			dictDstBoneInfo[15].trfBone.SetLocalPositionX(dictSrcBoneInfo[29].vctPos.x);
			dictDstBoneInfo[15].trfBone.SetLocalRotation(dictSrcBoneInfo[30].vctRot.x, dictSrcBoneInfo[29].vctRot.y, 0f);
			dictDstBoneInfo[16].trfBone.SetLocalRotation(dictSrcBoneInfo[31].vctRot.x, dictSrcBoneInfo[32].vctRot.y, dictSrcBoneInfo[32].vctRot.z);
			dictDstBoneInfo[17].trfBone.SetLocalRotation(dictSrcBoneInfo[33].vctRot.x, dictSrcBoneInfo[34].vctRot.y + dictSrcBoneInfo[33].vctRot.y, 0f);
			dictDstBoneInfo[18].trfBone.SetLocalRotation(dictSrcBoneInfo[35].vctRot.x, dictSrcBoneInfo[36].vctRot.y, dictSrcBoneInfo[36].vctRot.z);
			dictDstBoneInfo[19].trfBone.SetLocalPositionX(dictSrcBoneInfo[37].vctPos.x);
			dictDstBoneInfo[19].trfBone.SetLocalRotation(dictSrcBoneInfo[38].vctRot.x, dictSrcBoneInfo[37].vctRot.y, 0f);
			dictDstBoneInfo[20].trfBone.SetLocalRotation(dictSrcBoneInfo[39].vctRot.x, dictSrcBoneInfo[40].vctRot.y, dictSrcBoneInfo[40].vctRot.z);
			dictDstBoneInfo[21].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[41].vctRot.z);
			dictDstBoneInfo[22].trfBone.SetLocalRotation(dictSrcBoneInfo[42].vctRot.x, 0f, 0f);
			dictDstBoneInfo[23].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[43].vctRot.z);
			dictDstBoneInfo[24].trfBone.SetLocalRotation(dictSrcBoneInfo[44].vctRot.x, 0f, 0f);
			dictDstBoneInfo[25].trfBone.SetLocalPositionY(dictSrcBoneInfo[45].vctPos.y);
			dictDstBoneInfo[25].trfBone.SetLocalPositionZ(dictSrcBoneInfo[46].vctPos.z + dictSrcBoneInfo[47].vctPos.z + dictSrcBoneInfo[48].vctPos.z + dictSrcBoneInfo[45].vctPos.z + dictSrcBoneInfo[49].vctPos.z);
			dictDstBoneInfo[25].trfBone.SetLocalScale(dictSrcBoneInfo[46].vctScl.x * dictSrcBoneInfo[47].vctScl.x * dictSrcBoneInfo[48].vctScl.x, dictSrcBoneInfo[46].vctScl.y * dictSrcBoneInfo[47].vctScl.y * dictSrcBoneInfo[45].vctScl.y, dictSrcBoneInfo[48].vctScl.z * dictSrcBoneInfo[45].vctScl.z);
			dictDstBoneInfo[26].trfBone.SetLocalPositionY(dictSrcBoneInfo[50].vctPos.y);
			dictDstBoneInfo[26].trfBone.SetLocalPositionZ(dictSrcBoneInfo[51].vctPos.z + dictSrcBoneInfo[52].vctPos.z + dictSrcBoneInfo[53].vctPos.z + dictSrcBoneInfo[50].vctPos.z + dictSrcBoneInfo[49].vctPos.z);
			dictDstBoneInfo[26].trfBone.SetLocalScale(dictSrcBoneInfo[51].vctScl.x * dictSrcBoneInfo[52].vctScl.x * dictSrcBoneInfo[53].vctScl.x, dictSrcBoneInfo[51].vctScl.y * dictSrcBoneInfo[52].vctScl.y * dictSrcBoneInfo[50].vctScl.y, dictSrcBoneInfo[53].vctScl.z * dictSrcBoneInfo[50].vctScl.z);
			dictDstBoneInfo[27].trfBone.SetLocalScale(dictSrcBoneInfo[54].vctScl.x, 1f, 1f);
			dictDstBoneInfo[28].trfBone.SetLocalPositionY(dictSrcBoneInfo[55].vctPos.y);
			dictDstBoneInfo[29].trfBone.SetLocalPositionZ(dictSrcBoneInfo[56].vctPos.z);
			dictDstBoneInfo[30].trfBone.SetLocalPositionX(dictSrcBoneInfo[57].vctPos.x + dictSrcBoneInfo[58].vctPos.x);
			dictDstBoneInfo[30].trfBone.SetLocalPositionY(dictSrcBoneInfo[58].vctPos.y + dictSrcBoneInfo[59].vctPos.y);
			dictDstBoneInfo[30].trfBone.SetLocalPositionZ(dictSrcBoneInfo[60].vctPos.z + dictSrcBoneInfo[61].vctPos.z);
			dictDstBoneInfo[30].trfBone.SetLocalScale(dictSrcBoneInfo[58].vctScl.x, 1f, 1f);
			dictDstBoneInfo[31].trfBone.SetLocalPositionX(dictSrcBoneInfo[62].vctPos.x - dictSrcBoneInfo[58].vctPos.x);
			dictDstBoneInfo[31].trfBone.SetLocalPositionY(dictSrcBoneInfo[58].vctPos.y + dictSrcBoneInfo[59].vctPos.y);
			dictDstBoneInfo[31].trfBone.SetLocalPositionZ(dictSrcBoneInfo[60].vctPos.z + dictSrcBoneInfo[61].vctPos.z);
			dictDstBoneInfo[31].trfBone.SetLocalScale(dictSrcBoneInfo[58].vctScl.x, 1f, 1f);
			dictDstBoneInfo[32].trfBone.SetLocalPositionX(dictSrcBoneInfo[63].vctPos.x);
			dictDstBoneInfo[32].trfBone.SetLocalPositionY(dictSrcBoneInfo[64].vctPos.y);
			dictDstBoneInfo[32].trfBone.SetLocalPositionZ(dictSrcBoneInfo[65].vctPos.z);
			dictDstBoneInfo[33].trfBone.SetLocalPositionX(dictSrcBoneInfo[66].vctPos.x);
			dictDstBoneInfo[33].trfBone.SetLocalPositionY(dictSrcBoneInfo[64].vctPos.y);
			dictDstBoneInfo[33].trfBone.SetLocalPositionZ(dictSrcBoneInfo[65].vctPos.z);
			dictDstBoneInfo[34].trfBone.SetLocalPositionZ(dictSrcBoneInfo[67].vctPos.z);
			dictDstBoneInfo[35].trfBone.SetLocalScale(dictSrcBoneInfo[68].vctScl.x, 1f, 1f);
			dictDstBoneInfo[36].trfBone.SetLocalPositionY(dictSrcBoneInfo[69].vctPos.y);
			dictDstBoneInfo[36].trfBone.SetLocalPositionZ(dictSrcBoneInfo[69].vctPos.z + dictSrcBoneInfo[70].vctPos.z);
			dictDstBoneInfo[37].trfBone.SetLocalScale(dictSrcBoneInfo[71].vctScl.x, dictSrcBoneInfo[72].vctScl.y, 1f);
			dictDstBoneInfo[38].trfBone.SetLocalPositionY(dictSrcBoneInfo[73].vctPos.y);
			dictDstBoneInfo[38].trfBone.SetLocalPositionZ(dictSrcBoneInfo[73].vctPos.z);
			dictDstBoneInfo[38].trfBone.SetLocalRotation(dictSrcBoneInfo[73].vctRot.x, 0f, 0f);
			dictDstBoneInfo[38].trfBone.SetLocalScale(1f, dictSrcBoneInfo[73].vctScl.y, dictSrcBoneInfo[73].vctScl.z);
			dictDstBoneInfo[39].trfBone.SetLocalPositionY(dictSrcBoneInfo[74].vctPos.y);
			dictDstBoneInfo[39].trfBone.SetLocalPositionZ(dictSrcBoneInfo[74].vctPos.z);
			dictDstBoneInfo[39].trfBone.SetLocalScale(dictSrcBoneInfo[74].vctScl.x, 1f, 1f);
			dictDstBoneInfo[40].trfBone.SetLocalPositionY(dictSrcBoneInfo[75].vctPos.y);
			dictDstBoneInfo[40].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[75].vctRot.z);
			dictDstBoneInfo[41].trfBone.SetLocalPositionY(dictSrcBoneInfo[76].vctPos.y);
			dictDstBoneInfo[41].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[76].vctRot.z);
			dictDstBoneInfo[42].trfBone.SetLocalPositionY(dictSrcBoneInfo[77].vctPos.y);
			dictDstBoneInfo[43].trfBone.SetLocalPositionY(dictSrcBoneInfo[78].vctPos.y + dictSrcBoneInfo[79].vctPos.y);
			dictDstBoneInfo[43].trfBone.SetLocalPositionZ(dictSrcBoneInfo[80].vctPos.z + dictSrcBoneInfo[79].vctPos.z);
			dictDstBoneInfo[43].trfBone.SetLocalRotation(dictSrcBoneInfo[79].vctRot.x, 0f, 0f);
			dictDstBoneInfo[43].trfBone.SetLocalScale(dictSrcBoneInfo[81].vctScl.x, 1f, 1f);
			dictDstBoneInfo[44].trfBone.SetLocalPositionY(dictSrcBoneInfo[82].vctPos.y);
			dictDstBoneInfo[44].trfBone.SetLocalPositionZ(dictSrcBoneInfo[83].vctPos.z);
			dictDstBoneInfo[44].trfBone.SetLocalScale(dictSrcBoneInfo[84].vctScl.x, dictSrcBoneInfo[84].vctScl.y, dictSrcBoneInfo[84].vctScl.z);
			dictDstBoneInfo[45].trfBone.SetLocalPositionY(dictSrcBoneInfo[85].vctPos.y);
			dictDstBoneInfo[45].trfBone.SetLocalPositionZ(dictSrcBoneInfo[86].vctPos.z + dictSrcBoneInfo[87].vctPos.z + dictSrcBoneInfo[85].vctPos.z + dictSrcBoneInfo[88].vctPos.z);
			dictDstBoneInfo[45].trfBone.SetLocalRotation(dictSrcBoneInfo[88].vctRot.x, 0f, 0f);
			dictDstBoneInfo[46].trfBone.SetLocalScale(dictSrcBoneInfo[89].vctScl.x, 1f, 1f);
			dictDstBoneInfo[47].trfBone.SetLocalPositionY(dictSrcBoneInfo[90].vctPos.y + dictSrcBoneInfo[91].vctPos.y + dictSrcBoneInfo[92].vctPos.y);
			dictDstBoneInfo[47].trfBone.SetLocalPositionZ(dictSrcBoneInfo[90].vctPos.z + dictSrcBoneInfo[93].vctPos.z + dictSrcBoneInfo[92].vctPos.z);
			dictDstBoneInfo[48].trfBone.SetLocalRotation(dictSrcBoneInfo[90].vctRot.x, 0f, 0f);
			dictDstBoneInfo[48].trfBone.SetLocalScale(dictSrcBoneInfo[94].vctScl.x, dictSrcBoneInfo[94].vctScl.y, dictSrcBoneInfo[94].vctScl.z);
			dictDstBoneInfo[49].trfBone.SetLocalPositionX(dictSrcBoneInfo[95].vctPos.x);
			dictDstBoneInfo[49].trfBone.SetLocalPositionY(dictSrcBoneInfo[96].vctPos.y);
			dictDstBoneInfo[49].trfBone.SetLocalPositionZ(dictSrcBoneInfo[97].vctPos.z);
			dictDstBoneInfo[49].trfBone.SetLocalRotation(dictSrcBoneInfo[98].vctRot.x, 0f, dictSrcBoneInfo[99].vctRot.z);
			dictDstBoneInfo[50].trfBone.SetLocalPositionX(dictSrcBoneInfo[100].vctPos.x);
			dictDstBoneInfo[50].trfBone.SetLocalPositionY(dictSrcBoneInfo[96].vctPos.y);
			dictDstBoneInfo[50].trfBone.SetLocalPositionZ(dictSrcBoneInfo[97].vctPos.z);
			dictDstBoneInfo[50].trfBone.SetLocalRotation(dictSrcBoneInfo[98].vctRot.x, 0f, dictSrcBoneInfo[101].vctRot.z);
			dictDstBoneInfo[51].trfBone.SetLocalPositionY(dictSrcBoneInfo[102].vctPos.y);
			dictDstBoneInfo[51].trfBone.SetLocalPositionZ(dictSrcBoneInfo[102].vctPos.z);
			dictDstBoneInfo[51].trfBone.SetLocalScale(dictSrcBoneInfo[102].vctScl.x, dictSrcBoneInfo[102].vctScl.y, dictSrcBoneInfo[102].vctScl.z);
			dictDstBoneInfo[52].trfBone.SetLocalPositionY(dictSrcBoneInfo[103].vctPos.y);
			dictDstBoneInfo[52].trfBone.SetLocalPositionZ(dictSrcBoneInfo[104].vctPos.z);
			dictDstBoneInfo[52].trfBone.SetLocalRotation(dictSrcBoneInfo[103].vctRot.x, 0f, 0f);
			dictDstBoneInfo[52].trfBone.SetLocalScale(dictSrcBoneInfo[105].vctScl.x, dictSrcBoneInfo[105].vctScl.y, dictSrcBoneInfo[105].vctScl.z);
			dictDstBoneInfo[53].trfBone.SetLocalPositionY(dictSrcBoneInfo[106].vctPos.y + dictSrcBoneInfo[107].vctPos.y + dictSrcBoneInfo[108].vctPos.y);
			dictDstBoneInfo[53].trfBone.SetLocalPositionZ(dictSrcBoneInfo[106].vctPos.z + dictSrcBoneInfo[109].vctPos.z + dictSrcBoneInfo[108].vctPos.z);
			dictDstBoneInfo[53].trfBone.SetLocalRotation(dictSrcBoneInfo[106].vctRot.x + dictSrcBoneInfo[107].vctRot.x + dictSrcBoneInfo[108].vctRot.x, 0f, 0f);
			dictDstBoneInfo[54].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[110].vctRot.y, dictSrcBoneInfo[111].vctRot.z);
			dictDstBoneInfo[54].trfBone.SetLocalScale(dictSrcBoneInfo[112].vctScl.x, dictSrcBoneInfo[112].vctScl.y, dictSrcBoneInfo[112].vctScl.z);
			dictDstBoneInfo[55].trfBone.SetLocalPositionX(dictSrcBoneInfo[113].vctPos.x);
			dictDstBoneInfo[55].trfBone.SetLocalPositionY(dictSrcBoneInfo[113].vctPos.y);
			dictDstBoneInfo[55].trfBone.SetLocalPositionZ(dictSrcBoneInfo[113].vctPos.z);
			dictDstBoneInfo[55].trfBone.SetLocalRotation(dictSrcBoneInfo[113].vctRot.x, dictSrcBoneInfo[113].vctRot.y, 0f);
			dictDstBoneInfo[55].trfBone.SetLocalScale(dictSrcBoneInfo[113].vctScl.x, dictSrcBoneInfo[113].vctScl.y, dictSrcBoneInfo[113].vctScl.z);
			dictDstBoneInfo[56].trfBone.SetLocalPositionY(dictSrcBoneInfo[114].vctPos.y);
			dictDstBoneInfo[56].trfBone.SetLocalPositionZ(dictSrcBoneInfo[114].vctPos.z);
			dictDstBoneInfo[56].trfBone.SetLocalScale(dictSrcBoneInfo[114].vctScl.x, dictSrcBoneInfo[114].vctScl.y, dictSrcBoneInfo[114].vctScl.z);
			dictDstBoneInfo[57].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[115].vctRot.y, dictSrcBoneInfo[116].vctRot.z);
			dictDstBoneInfo[57].trfBone.SetLocalScale(dictSrcBoneInfo[117].vctScl.x, dictSrcBoneInfo[117].vctScl.y, dictSrcBoneInfo[117].vctScl.z);
			dictDstBoneInfo[58].trfBone.SetLocalPositionX(dictSrcBoneInfo[118].vctPos.x);
			dictDstBoneInfo[58].trfBone.SetLocalPositionY(dictSrcBoneInfo[118].vctPos.y);
			dictDstBoneInfo[58].trfBone.SetLocalPositionZ(dictSrcBoneInfo[118].vctPos.z);
			dictDstBoneInfo[58].trfBone.SetLocalRotation(dictSrcBoneInfo[118].vctRot.x, dictSrcBoneInfo[118].vctRot.y, 0f);
			dictDstBoneInfo[58].trfBone.SetLocalScale(dictSrcBoneInfo[118].vctScl.x, dictSrcBoneInfo[118].vctScl.y, dictSrcBoneInfo[118].vctScl.z);
			dictDstBoneInfo[59].trfBone.SetLocalPositionY(dictSrcBoneInfo[119].vctPos.y);
			dictDstBoneInfo[59].trfBone.SetLocalPositionZ(dictSrcBoneInfo[119].vctPos.z);
			dictDstBoneInfo[59].trfBone.SetLocalScale(dictSrcBoneInfo[119].vctScl.x, dictSrcBoneInfo[119].vctScl.y, dictSrcBoneInfo[119].vctScl.z);
			dictDstBoneInfo[60].trfBone.SetLocalPositionY(dictSrcBoneInfo[120].vctPos.y);
			dictDstBoneInfo[60].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[121].vctRot.z);
			dictDstBoneInfo[60].trfBone.SetLocalScale(dictSrcBoneInfo[122].vctScl.x, dictSrcBoneInfo[122].vctScl.y, dictSrcBoneInfo[122].vctScl.z);
			dictDstBoneInfo[61].trfBone.SetLocalPositionY(dictSrcBoneInfo[123].vctPos.y);
			dictDstBoneInfo[61].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[124].vctRot.z);
			dictDstBoneInfo[61].trfBone.SetLocalScale(dictSrcBoneInfo[125].vctScl.x, dictSrcBoneInfo[125].vctScl.y, dictSrcBoneInfo[125].vctScl.z);
		}
	}
}
