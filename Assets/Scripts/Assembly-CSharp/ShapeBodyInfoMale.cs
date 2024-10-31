using System;
using System.Collections.Generic;
using IllusionUtility.SetUtility;
using UnityEngine;

public class ShapeBodyInfoMale : ShapeInfoBase
{
	public enum DstBoneName
	{
		cm_N_height = 0,
		cm_J_Shoulder_s_L = 1,
		cm_J_Shoulder_s_R = 2,
		cm_J_Shoulder02_s_L = 3,
		cm_J_Shoulder02_s_R = 4,
		cm_J_ArmUp01_s_L = 5,
		cm_J_ArmUp01_s_R = 6,
		cm_J_ArmUp02_s_L = 7,
		cm_J_ArmUp02_s_R = 8,
		cm_J_ArmUp03_s_L = 9,
		cm_J_ArmUp03_s_R = 10,
		cm_J_ArmElbo_low_s_L = 11,
		cm_J_ArmElbo_low_s_R = 12,
		cm_J_ArmLow01_s_L = 13,
		cm_J_ArmLow01_s_R = 14,
		cm_J_ArmLow02_s_L = 15,
		cm_J_ArmLow02_s_R = 16,
		cm_J_Hand_s_L = 17,
		cm_J_Hand_s_R = 18,
		cm_J_Hand_Wrist_s_L = 19,
		cm_J_Hand_Wrist_s_R = 20,
		cm_J_Kosi01_s = 21,
		cm_J_Kosi02_s = 22,
		cm_J_Spine01_s = 23,
		cm_J_Spine02_s = 24,
		cm_J_Spine03_s = 25,
		cm_J_Neck_s = 26,
		cm_J_Head_s = 27,
		cm_J_dan_s = 28,
		cm_J_LegUpDam_s_L = 29,
		cm_J_LegUpDam_s_R = 30,
		cm_J_LegUp01_s_L = 31,
		cm_J_LegUp01_s_R = 32,
		cm_J_LegUp02_s_L = 33,
		cm_J_LegUp02_s_R = 34,
		cm_J_LegUp03_s_L = 35,
		cm_J_LegUp03_s_R = 36,
		cm_J_LegKnee_low_s_L = 37,
		cm_J_LegKnee_low_s_R = 38,
		cm_J_LegLow01_s_L = 39,
		cm_J_LegLow01_s_R = 40,
		cm_J_LegLow02_s_L = 41,
		cm_J_LegLow02_s_R = 42,
		cm_J_LegLow03_s_L = 43,
		cm_J_LegLow03_s_R = 44,
		cm_J_Mune00_s_L = 45,
		cm_J_Mune00_s_R = 46,
		cm_J_Mune01_s_L = 47,
		cm_J_Mune01_s_R = 48,
		cm_J_Siri_s_L = 49,
		cm_J_Siri_s_R = 50,
		cm_J_Belly_dam0 = 51,
		cm_J_Belly01_s = 52,
		cm_J_Belly02_s = 53,
		cm_J_Belly03_s = 54
	}

	public enum SrcBoneName
	{
		cm_S_height = 0,
		cm_S_heightaid = 1,
		cm_S_Mune01_s_L = 2,
		cm_S_Mune00_s_L = 3,
		cm_S_Mune01_s_R = 4,
		cm_S_Mune00_s_R = 5,
		cm_S_Mune00_ss_ty = 6,
		cm_S_Mune01_s_ry_L = 7,
		cm_S_Mune01_s_ry_R = 8,
		cm_S_Mune01_s_rx_L = 9,
		cm_S_Mune01_s_rx_R = 10,
		cm_S_Head_s = 11,
		cm_S_Neck_s = 12,
		cm_S_Spine03_s = 13,
		cm_S_ArmUp01_s_tx_L = 14,
		cm_S_Shoulder_s_L = 15,
		cm_S_Shoulder02_s_tx_L = 16,
		cm_S_Mune00_ss_03_L = 17,
		cm_S_ArmUp01_s_tx_R = 18,
		cm_S_Shoulder_s_R = 19,
		cm_S_Shoulder02_s_tx_R = 20,
		cm_S_Mune00_ss_03_R = 21,
		cm_S_Spine02_s = 22,
		cm_S_Mune00_ss_02_L = 23,
		cm_S_Mune00_ss_02_R = 24,
		cm_S_Spine01_s = 25,
		cm_S_Kosi01_s = 26,
		cm_S_Belly_dam0 = 27,
		cm_S_LegUpDam_ss_L = 28,
		cm_S_Siri_kosi01_s_L = 29,
		cm_S_LegUpDam_ss_R = 30,
		cm_S_Siri_kosi01_s_R = 31,
		cm_S_Kosi02_s = 32,
		cm_S_dan_s = 33,
		cm_S_Siri_kosi02_s_L = 34,
		cm_S_LegUp01_blend_s_L = 35,
		cm_S_Siri_kosi02_s_R = 36,
		cm_S_LegUp01_blend_s_R = 37,
		cm_S_Belly01_s = 38,
		cm_S_Belly02_s = 39,
		cm_S_Belly03_s = 40,
		cm_S_Siri_s_L = 41,
		cm_S_Siri_s_R = 42,
		cm_S_LegUp01_s_L = 43,
		cm_S_LegUp02_s_L = 44,
		cm_S_Siri_legup01_s_L = 45,
		cm_S_LegUpDam_s_L = 46,
		cm_S_LegUp01_s_R = 47,
		cm_S_LegUp02_s_R = 48,
		cm_S_Siri_legup01_s_R = 49,
		cm_S_LegUpDam_s_R = 50,
		cm_S_LegUp02_blend_s_L = 51,
		cm_S_LegUp03_s_L = 52,
		cm_S_LegKnee_up_s_L = 53,
		cm_S_LegUp02_blend_s_R = 54,
		cm_S_LegUp03_s_R = 55,
		cm_S_LegKnee_up_s_R = 56,
		cm_S_LegLow01_s_L = 57,
		cm_S_LegKnee_low_s_L = 58,
		cm_S_LegUp03_blend_s_L = 59,
		cm_S_LegLow01_s_R = 60,
		cm_S_LegKnee_low_s_R = 61,
		cm_S_LegUp03_blend_s_R = 62,
		cm_S_LegLow02_s_L = 63,
		cm_S_LegLow02_s_R = 64,
		cm_S_LegLow03_s = 65,
		cm_S_Shoulder02_s_L = 66,
		cm_S_ArmUp01_s_L = 67,
		cm_S_ArmUp02_s_L = 68,
		cm_S_ArmUp03_s_L = 69,
		cm_S_ArmElbo_up_s_L = 70,
		cm_S_Shoulder02_s_R = 71,
		cm_S_ArmUp01_s_R = 72,
		cm_S_ArmUp02_s_R = 73,
		cm_S_ArmUp03_s_R = 74,
		cm_S_ArmElbo_up_s_R = 75,
		cm_S_ArmLow01_s_L = 76,
		cm_S_ArmLow02_s_L = 77,
		cm_S_ArmElbo_low_s_L = 78,
		cm_S_Hand_Wrist_s_L = 79,
		cm_S_ArmLow01_s_R = 80,
		cm_S_ArmLow02_s_R = 81,
		cm_S_ArmElbo_low_s_R = 82,
		cm_S_Hand_Wrist_s_R = 83
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
			dictDstBoneInfo[0].trfBone.SetLocalScale(dictSrcBoneInfo[0].vctScl.x, dictSrcBoneInfo[0].vctScl.y, dictSrcBoneInfo[0].vctScl.z);
			dictDstBoneInfo[1].trfBone.SetLocalPositionY(dictSrcBoneInfo[15].vctPos.y);
			dictDstBoneInfo[1].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[15].vctScl.x, dictSrcBoneInfo[1].vctScl.y * dictSrcBoneInfo[15].vctScl.y, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[15].vctScl.z);
			dictDstBoneInfo[2].trfBone.SetLocalPositionY(dictSrcBoneInfo[19].vctPos.y);
			dictDstBoneInfo[2].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[19].vctScl.x, dictSrcBoneInfo[1].vctScl.y * dictSrcBoneInfo[19].vctScl.y, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[19].vctScl.z);
			dictDstBoneInfo[3].trfBone.SetLocalPositionX(dictSrcBoneInfo[16].vctPos.x);
			dictDstBoneInfo[3].trfBone.SetLocalPositionY(dictSrcBoneInfo[16].vctPos.y);
			dictDstBoneInfo[3].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[16].vctRot.z);
			dictDstBoneInfo[3].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x, dictSrcBoneInfo[66].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[66].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[4].trfBone.SetLocalPositionX(dictSrcBoneInfo[20].vctPos.x);
			dictDstBoneInfo[4].trfBone.SetLocalPositionY(dictSrcBoneInfo[20].vctPos.y);
			dictDstBoneInfo[4].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[20].vctRot.z);
			dictDstBoneInfo[4].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x, dictSrcBoneInfo[71].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[71].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[5].trfBone.SetLocalPositionX(dictSrcBoneInfo[14].vctPos.x);
			dictDstBoneInfo[5].trfBone.SetLocalPositionY(dictSrcBoneInfo[67].vctPos.y + dictSrcBoneInfo[14].vctPos.y);
			dictDstBoneInfo[5].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[67].vctRot.y, dictSrcBoneInfo[67].vctRot.z + dictSrcBoneInfo[14].vctRot.z);
			dictDstBoneInfo[5].trfBone.SetLocalScale(1f, dictSrcBoneInfo[67].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[67].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[6].trfBone.SetLocalPositionX(dictSrcBoneInfo[18].vctPos.x);
			dictDstBoneInfo[6].trfBone.SetLocalPositionY(dictSrcBoneInfo[72].vctPos.y + dictSrcBoneInfo[18].vctPos.y);
			dictDstBoneInfo[6].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[72].vctRot.y, dictSrcBoneInfo[72].vctRot.z + dictSrcBoneInfo[18].vctRot.z);
			dictDstBoneInfo[6].trfBone.SetLocalScale(1f, dictSrcBoneInfo[72].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[72].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[7].trfBone.SetLocalScale(1f, dictSrcBoneInfo[68].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[68].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[8].trfBone.SetLocalScale(1f, dictSrcBoneInfo[73].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[73].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[9].trfBone.SetLocalScale(1f, dictSrcBoneInfo[69].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[69].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[10].trfBone.SetLocalScale(1f, dictSrcBoneInfo[74].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[74].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[11].trfBone.SetLocalScale(1f, dictSrcBoneInfo[70].vctScl.y * dictSrcBoneInfo[78].vctScl.y, dictSrcBoneInfo[70].vctScl.z * dictSrcBoneInfo[78].vctScl.z);
			dictDstBoneInfo[12].trfBone.SetLocalScale(1f, dictSrcBoneInfo[75].vctScl.y * dictSrcBoneInfo[82].vctScl.y, dictSrcBoneInfo[75].vctScl.z * dictSrcBoneInfo[82].vctScl.z);
			dictDstBoneInfo[13].trfBone.SetLocalScale(1f, dictSrcBoneInfo[76].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[76].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[14].trfBone.SetLocalScale(1f, dictSrcBoneInfo[80].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[80].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[15].trfBone.SetLocalScale(1f, dictSrcBoneInfo[77].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[77].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[16].trfBone.SetLocalScale(1f, dictSrcBoneInfo[81].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[81].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[17].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x, dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[18].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x, dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[19].trfBone.SetLocalScale(1f, dictSrcBoneInfo[79].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[79].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[20].trfBone.SetLocalScale(1f, dictSrcBoneInfo[83].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[83].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[21].trfBone.SetLocalPositionZ(dictSrcBoneInfo[26].vctPos.z);
			dictDstBoneInfo[21].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[26].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[26].vctScl.z);
			dictDstBoneInfo[22].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[32].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[32].vctScl.z);
			dictDstBoneInfo[23].trfBone.SetLocalPositionZ(dictSrcBoneInfo[25].vctPos.z);
			dictDstBoneInfo[23].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[25].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[25].vctScl.z);
			dictDstBoneInfo[24].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[22].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[22].vctScl.z);
			dictDstBoneInfo[25].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[13].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[13].vctScl.z);
			dictDstBoneInfo[26].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[12].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[12].vctScl.z);
			dictDstBoneInfo[27].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[11].vctScl.x, dictSrcBoneInfo[1].vctScl.y * dictSrcBoneInfo[11].vctScl.y, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[11].vctScl.z);
			dictDstBoneInfo[28].trfBone.SetLocalPositionZ(dictSrcBoneInfo[33].vctPos.z);
			dictDstBoneInfo[29].trfBone.SetLocalScale(dictSrcBoneInfo[46].vctScl.x * dictSrcBoneInfo[28].vctScl.x, 1f, dictSrcBoneInfo[46].vctScl.z * dictSrcBoneInfo[28].vctScl.z);
			dictDstBoneInfo[30].trfBone.SetLocalScale(dictSrcBoneInfo[50].vctScl.x * dictSrcBoneInfo[30].vctScl.x, 1f, dictSrcBoneInfo[50].vctScl.z * dictSrcBoneInfo[30].vctScl.z);
			dictDstBoneInfo[31].trfBone.SetLocalPositionX(dictSrcBoneInfo[43].vctPos.x + dictSrcBoneInfo[35].vctPos.x);
			dictDstBoneInfo[31].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[43].vctRot.z + dictSrcBoneInfo[35].vctRot.z);
			dictDstBoneInfo[31].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[43].vctScl.x * dictSrcBoneInfo[35].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[43].vctScl.z * dictSrcBoneInfo[35].vctScl.z);
			dictDstBoneInfo[32].trfBone.SetLocalPositionX(dictSrcBoneInfo[47].vctPos.x + dictSrcBoneInfo[37].vctPos.x);
			dictDstBoneInfo[32].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[47].vctRot.z + dictSrcBoneInfo[37].vctRot.z);
			dictDstBoneInfo[32].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[47].vctScl.x * dictSrcBoneInfo[37].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[47].vctScl.z * dictSrcBoneInfo[37].vctScl.z);
			dictDstBoneInfo[33].trfBone.SetLocalPositionX(dictSrcBoneInfo[44].vctPos.x);
			dictDstBoneInfo[33].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[44].vctScl.x * dictSrcBoneInfo[51].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[44].vctScl.z * dictSrcBoneInfo[51].vctScl.z);
			dictDstBoneInfo[34].trfBone.SetLocalPositionX(dictSrcBoneInfo[48].vctPos.x);
			dictDstBoneInfo[34].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[48].vctScl.x * dictSrcBoneInfo[54].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[48].vctScl.z * dictSrcBoneInfo[54].vctScl.z);
			dictDstBoneInfo[35].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[52].vctScl.x * dictSrcBoneInfo[59].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[52].vctScl.z * dictSrcBoneInfo[59].vctScl.z);
			dictDstBoneInfo[36].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[55].vctScl.x * dictSrcBoneInfo[62].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[55].vctScl.z * dictSrcBoneInfo[62].vctScl.z);
			dictDstBoneInfo[37].trfBone.SetLocalPositionZ(dictSrcBoneInfo[58].vctPos.z);
			dictDstBoneInfo[37].trfBone.SetLocalScale(dictSrcBoneInfo[53].vctScl.x * dictSrcBoneInfo[58].vctScl.x * dictSrcBoneInfo[1].vctScl.x, 1f, dictSrcBoneInfo[53].vctScl.z * dictSrcBoneInfo[58].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[38].trfBone.SetLocalPositionZ(dictSrcBoneInfo[61].vctPos.z);
			dictDstBoneInfo[38].trfBone.SetLocalScale(dictSrcBoneInfo[56].vctScl.x * dictSrcBoneInfo[61].vctScl.x * dictSrcBoneInfo[1].vctScl.x, 1f, dictSrcBoneInfo[56].vctScl.z * dictSrcBoneInfo[61].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
			dictDstBoneInfo[39].trfBone.SetLocalRotation(dictSrcBoneInfo[57].vctRot.x, 0f, 0f);
			dictDstBoneInfo[39].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[57].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[57].vctScl.z);
			dictDstBoneInfo[40].trfBone.SetLocalRotation(dictSrcBoneInfo[60].vctRot.x, 0f, 0f);
			dictDstBoneInfo[40].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[60].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[60].vctScl.z);
			dictDstBoneInfo[41].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[63].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[63].vctScl.z);
			dictDstBoneInfo[42].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[64].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[64].vctScl.z);
			dictDstBoneInfo[43].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[65].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[65].vctScl.z);
			dictDstBoneInfo[44].trfBone.SetLocalScale(dictSrcBoneInfo[1].vctScl.x * dictSrcBoneInfo[65].vctScl.x, 1f, dictSrcBoneInfo[1].vctScl.z * dictSrcBoneInfo[65].vctScl.z);
			dictDstBoneInfo[45].trfBone.SetLocalPositionX(dictSrcBoneInfo[17].vctPos.x);
			dictDstBoneInfo[45].trfBone.SetLocalPositionY(dictSrcBoneInfo[6].vctPos.y + dictSrcBoneInfo[17].vctPos.y);
			dictDstBoneInfo[45].trfBone.SetLocalPositionZ(dictSrcBoneInfo[3].vctPos.z + dictSrcBoneInfo[23].vctPos.z + dictSrcBoneInfo[17].vctPos.z);
			dictDstBoneInfo[45].trfBone.SetLocalRotation(dictSrcBoneInfo[3].vctRot.x + dictSrcBoneInfo[9].vctRot.x + dictSrcBoneInfo[23].vctRot.x + dictSrcBoneInfo[17].vctRot.x, dictSrcBoneInfo[3].vctRot.y + dictSrcBoneInfo[7].vctRot.y + dictSrcBoneInfo[17].vctRot.y, 0f);
			dictDstBoneInfo[45].trfBone.SetLocalScale(dictSrcBoneInfo[3].vctScl.x * dictSrcBoneInfo[17].vctScl.x, dictSrcBoneInfo[3].vctScl.y * dictSrcBoneInfo[17].vctScl.y, dictSrcBoneInfo[3].vctScl.z * dictSrcBoneInfo[17].vctScl.z);
			dictDstBoneInfo[46].trfBone.SetLocalPositionX(dictSrcBoneInfo[21].vctPos.x);
			dictDstBoneInfo[46].trfBone.SetLocalPositionY(dictSrcBoneInfo[6].vctPos.y + dictSrcBoneInfo[21].vctPos.y);
			dictDstBoneInfo[46].trfBone.SetLocalPositionZ(dictSrcBoneInfo[5].vctPos.z + dictSrcBoneInfo[24].vctPos.z + dictSrcBoneInfo[21].vctPos.z);
			dictDstBoneInfo[46].trfBone.SetLocalRotation(dictSrcBoneInfo[5].vctRot.x + dictSrcBoneInfo[10].vctRot.x + dictSrcBoneInfo[24].vctRot.x + dictSrcBoneInfo[21].vctRot.x, dictSrcBoneInfo[5].vctRot.y + dictSrcBoneInfo[8].vctRot.y + dictSrcBoneInfo[21].vctRot.y, 0f);
			dictDstBoneInfo[46].trfBone.SetLocalScale(dictSrcBoneInfo[5].vctScl.x * dictSrcBoneInfo[21].vctScl.x, dictSrcBoneInfo[5].vctScl.y * dictSrcBoneInfo[21].vctScl.y, dictSrcBoneInfo[5].vctScl.z * dictSrcBoneInfo[21].vctScl.z);
			dictDstBoneInfo[47].trfBone.SetLocalPositionX(dictSrcBoneInfo[2].vctPos.x);
			dictDstBoneInfo[47].trfBone.SetLocalPositionY(dictSrcBoneInfo[2].vctPos.y);
			dictDstBoneInfo[47].trfBone.SetLocalPositionZ(dictSrcBoneInfo[2].vctPos.z);
			dictDstBoneInfo[47].trfBone.SetLocalRotation(dictSrcBoneInfo[2].vctRot.x, dictSrcBoneInfo[2].vctRot.y, 0f);
			dictDstBoneInfo[47].trfBone.SetLocalScale(dictSrcBoneInfo[2].vctScl.x, dictSrcBoneInfo[2].vctScl.y, dictSrcBoneInfo[2].vctScl.z);
			dictDstBoneInfo[48].trfBone.SetLocalPositionX(dictSrcBoneInfo[4].vctPos.x);
			dictDstBoneInfo[48].trfBone.SetLocalPositionY(dictSrcBoneInfo[4].vctPos.y);
			dictDstBoneInfo[48].trfBone.SetLocalPositionZ(dictSrcBoneInfo[4].vctPos.z);
			dictDstBoneInfo[48].trfBone.SetLocalRotation(dictSrcBoneInfo[4].vctRot.x, dictSrcBoneInfo[4].vctRot.y, 0f);
			dictDstBoneInfo[48].trfBone.SetLocalScale(dictSrcBoneInfo[4].vctScl.x, dictSrcBoneInfo[4].vctScl.y, dictSrcBoneInfo[4].vctScl.z);
			dictDstBoneInfo[49].trfBone.SetLocalPositionX(dictSrcBoneInfo[41].vctPos.x);
			dictDstBoneInfo[49].trfBone.SetLocalPositionY(dictSrcBoneInfo[41].vctPos.y);
			dictDstBoneInfo[49].trfBone.SetLocalPositionZ(dictSrcBoneInfo[41].vctPos.z + dictSrcBoneInfo[45].vctPos.z + dictSrcBoneInfo[34].vctPos.z);
			dictDstBoneInfo[49].trfBone.SetLocalScale(dictSrcBoneInfo[41].vctScl.x * dictSrcBoneInfo[45].vctScl.x * dictSrcBoneInfo[34].vctScl.x, dictSrcBoneInfo[41].vctScl.y, dictSrcBoneInfo[41].vctScl.z * dictSrcBoneInfo[45].vctScl.z * dictSrcBoneInfo[29].vctScl.z * dictSrcBoneInfo[34].vctScl.z);
			dictDstBoneInfo[50].trfBone.SetLocalPositionX(dictSrcBoneInfo[42].vctPos.x);
			dictDstBoneInfo[50].trfBone.SetLocalPositionY(dictSrcBoneInfo[42].vctPos.y);
			dictDstBoneInfo[50].trfBone.SetLocalPositionZ(dictSrcBoneInfo[42].vctPos.z + dictSrcBoneInfo[49].vctPos.z + dictSrcBoneInfo[36].vctPos.z);
			dictDstBoneInfo[50].trfBone.SetLocalScale(dictSrcBoneInfo[42].vctScl.x * dictSrcBoneInfo[49].vctScl.x * dictSrcBoneInfo[36].vctScl.x, dictSrcBoneInfo[42].vctScl.y, dictSrcBoneInfo[42].vctScl.z * dictSrcBoneInfo[49].vctScl.z * dictSrcBoneInfo[31].vctScl.z * dictSrcBoneInfo[36].vctScl.z);
			dictDstBoneInfo[51].trfBone.SetLocalPositionZ(dictSrcBoneInfo[27].vctPos.z);
			dictDstBoneInfo[51].trfBone.SetLocalScale(1f, dictSrcBoneInfo[27].vctScl.y, 1f);
			dictDstBoneInfo[52].trfBone.SetLocalPositionZ(dictSrcBoneInfo[38].vctPos.z);
			dictDstBoneInfo[52].trfBone.SetLocalRotation(dictSrcBoneInfo[38].vctRot.x, 0f, 0f);
			dictDstBoneInfo[52].trfBone.SetLocalScale(dictSrcBoneInfo[38].vctScl.x, dictSrcBoneInfo[38].vctScl.y, dictSrcBoneInfo[38].vctScl.z);
			dictDstBoneInfo[53].trfBone.SetLocalPositionZ(dictSrcBoneInfo[39].vctPos.z);
			dictDstBoneInfo[53].trfBone.SetLocalScale(dictSrcBoneInfo[39].vctScl.x, dictSrcBoneInfo[39].vctScl.y, dictSrcBoneInfo[39].vctScl.z);
			dictDstBoneInfo[54].trfBone.SetLocalPositionZ(dictSrcBoneInfo[40].vctPos.z);
			dictDstBoneInfo[54].trfBone.SetLocalScale(dictSrcBoneInfo[40].vctScl.x, dictSrcBoneInfo[40].vctScl.y, dictSrcBoneInfo[40].vctScl.z);
		}
	}
}
