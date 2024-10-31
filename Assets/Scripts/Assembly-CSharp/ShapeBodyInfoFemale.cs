using System;
using System.Collections.Generic;
using IllusionUtility.SetUtility;
using UnityEngine;

public class ShapeBodyInfoFemale : ShapeInfoBase
{
	public enum DstBoneName
	{
		cf_J_ArmElbo_low_s_L = 0,
		cf_J_ArmElbo_low_s_R = 1,
		cf_J_ArmLow01_s_L = 2,
		cf_J_ArmLow01_s_R = 3,
		cf_J_ArmLow02_s_L = 4,
		cf_J_ArmLow02_s_R = 5,
		cf_J_ArmUp01_s_L = 6,
		cf_J_ArmUp01_s_R = 7,
		cf_J_ArmUp02_s_L = 8,
		cf_J_ArmUp02_s_R = 9,
		cf_J_ArmUp03_s_L = 10,
		cf_J_ArmUp03_s_R = 11,
		cf_J_Hand_s_L = 12,
		cf_J_Hand_s_R = 13,
		cf_J_Hand_Wrist_s_L = 14,
		cf_J_Hand_Wrist_s_R = 15,
		cf_J_Head_s = 16,
		cf_J_Kosi01_s = 17,
		cf_J_Kosi02_s = 18,
		cf_J_LegKnee_back_s_L = 19,
		cf_J_LegKnee_back_s_R = 20,
		cf_J_LegKnee_low_s_L = 21,
		cf_J_LegKnee_low_s_R = 22,
		cf_J_LegLow01_s_L = 23,
		cf_J_LegLow01_s_R = 24,
		cf_J_LegLow02_s_L = 25,
		cf_J_LegLow02_s_R = 26,
		cf_J_LegLow03_s_L = 27,
		cf_J_LegLow03_s_R = 28,
		cf_J_LegUp01_s_L = 29,
		cf_J_LegUp01_s_R = 30,
		cf_J_LegUp02_s_L = 31,
		cf_J_LegUp02_s_R = 32,
		cf_J_LegUp03_s_L = 33,
		cf_J_LegUp03_s_R = 34,
		cf_J_LegUpDam_s_L = 35,
		cf_J_LegUpDam_s_R = 36,
		cf_J_Mune_Nip01_s_L = 37,
		cf_J_Mune_Nip01_s_R = 38,
		cf_J_Mune_Nip02_s_L = 39,
		cf_J_Mune_Nip02_s_R = 40,
		cf_J_Mune_Nipacs01_L = 41,
		cf_J_Mune_Nipacs01_R = 42,
		cf_J_Mune00_d_L = 43,
		cf_J_Mune00_d_R = 44,
		cf_J_Mune00_s_L = 45,
		cf_J_Mune00_s_R = 46,
		cf_J_Mune00_t_L = 47,
		cf_J_Mune00_t_R = 48,
		cf_J_Mune01_s_L = 49,
		cf_J_Mune01_s_R = 50,
		cf_J_Mune01_t_L = 51,
		cf_J_Mune01_t_R = 52,
		cf_J_Mune02_s_L = 53,
		cf_J_Mune02_s_R = 54,
		cf_J_Mune02_t_L = 55,
		cf_J_Mune02_t_R = 56,
		cf_J_Mune03_s_L = 57,
		cf_J_Mune03_s_R = 58,
		cf_J_Mune04_s_L = 59,
		cf_J_Mune04_s_R = 60,
		cf_J_Neck_s = 61,
		cf_J_Shoulder02_s_L = 62,
		cf_J_Shoulder02_s_R = 63,
		cf_J_Siri_s_L = 64,
		cf_J_Siri_s_R = 65,
		cf_J_sk_siri_dam = 66,
		cf_J_sk_top = 67,
		cf_J_Spine01_s = 68,
		cf_J_Spine02_s = 69,
		cf_J_Spine03_s = 70,
		cf_N_height = 71,
		cf_J_sk_00_00_dam = 72,
		cf_J_sk_01_00_dam = 73,
		cf_J_sk_02_00_dam = 74,
		cf_J_sk_03_00_dam = 75,
		cf_J_sk_04_00_dam = 76,
		cf_J_sk_05_00_dam = 77,
		cf_J_sk_06_00_dam = 78,
		cf_J_sk_07_00_dam = 79,
		cf_hit_Mune02_s_L = 80,
		cf_hit_Mune02_s_R = 81,
		cf_hit_Kosi02_s = 82,
		cf_hit_LegUp01_s_L = 83,
		cf_hit_LegUp01_s_R = 84,
		cf_hit_Siri_s_L = 85,
		cf_hit_Siri_s_R = 86
	}

	public enum SrcBoneName
	{
		cf_s_ArmElbo_low_s_L = 0,
		cf_s_ArmElbo_low_s_R = 1,
		cf_s_ArmElbo_up_s_L = 2,
		cf_s_ArmElbo_up_s_R = 3,
		cf_s_ArmLow01_h_L = 4,
		cf_s_ArmLow01_h_R = 5,
		cf_s_ArmLow01_s_L = 6,
		cf_s_ArmLow01_s_R = 7,
		cf_s_ArmLow02_h_L = 8,
		cf_s_ArmLow02_h_R = 9,
		cf_s_ArmLow02_s_L = 10,
		cf_s_ArmLow02_s_R = 11,
		cf_s_ArmUp01_h_L = 12,
		cf_s_ArmUp01_h_R = 13,
		cf_s_ArmUp01_s_L = 14,
		cf_s_ArmUp01_s_R = 15,
		cf_s_ArmUp01_s_tx_L = 16,
		cf_s_ArmUp01_s_tx_R = 17,
		cf_s_ArmUp02_h_L = 18,
		cf_s_ArmUp02_h_R = 19,
		cf_s_ArmUp02_s_L = 20,
		cf_s_ArmUp02_s_R = 21,
		cf_s_ArmUp03_h_L = 22,
		cf_s_ArmUp03_h_R = 23,
		cf_s_ArmUp03_s_L = 24,
		cf_s_ArmUp03_s_R = 25,
		cf_s_Hand_h_L = 26,
		cf_s_Hand_h_R = 27,
		cf_s_Hand_Wrist_h_L = 28,
		cf_s_Hand_Wrist_h_R = 29,
		cf_s_Hand_Wrist_s_L = 30,
		cf_s_Hand_Wrist_s_R = 31,
		cf_s_Head_h = 32,
		cf_s_Head_s = 33,
		cf_s_height = 34,
		cf_s_Kosi01_h = 35,
		cf_s_Kosi01_s = 36,
		cf_s_Kosi01_s_sz = 37,
		cf_s_Kosi02_h = 38,
		cf_s_Kosi02_s = 39,
		cf_s_Kosi02_s_sz = 40,
		cf_s_LegKnee_back_s_L = 41,
		cf_s_LegKnee_back_s_R = 42,
		cf_s_LegKnee_h_L = 43,
		cf_s_LegKnee_h_R = 44,
		cf_s_LegKnee_low_s_L = 45,
		cf_s_LegKnee_low_s_R = 46,
		cf_s_LegKnee_up_s_L = 47,
		cf_s_LegKnee_up_s_R = 48,
		cf_s_LegLow01_h_L = 49,
		cf_s_LegLow01_h_R = 50,
		cf_s_LegLow01_s_L = 51,
		cf_s_LegLow01_s_R = 52,
		cf_s_LegLow02_h_L = 53,
		cf_s_LegLow02_h_R = 54,
		cf_s_LegLow02_s_L = 55,
		cf_s_LegLow02_s_R = 56,
		cf_s_LegLow03_s_L = 57,
		cf_s_LegLow03_s_R = 58,
		cf_s_LegUp01_blend_s_L = 59,
		cf_s_LegUp01_blend_s_R = 60,
		cf_s_LegUp01_blend_ss_L = 61,
		cf_s_LegUp01_blend_ss_R = 62,
		cf_s_LegUp01_h_L = 63,
		cf_s_LegUp01_h_R = 64,
		cf_s_LegUp01_s_L = 65,
		cf_s_LegUp01_s_R = 66,
		cf_s_LegUp02_blend_s_L = 67,
		cf_s_LegUp02_blend_s_R = 68,
		cf_s_LegUp02_h_L = 69,
		cf_s_LegUp02_h_R = 70,
		cf_s_LegUp02_s_L = 71,
		cf_s_LegUp02_s_R = 72,
		cf_s_LegUp03_blend_s_L = 73,
		cf_s_LegUp03_blend_s_R = 74,
		cf_s_LegUp03_h_L = 75,
		cf_s_LegUp03_h_R = 76,
		cf_s_LegUp03_s_L = 77,
		cf_s_LegUp03_s_R = 78,
		cf_s_LegUpDam_s_L = 79,
		cf_s_LegUpDam_s_R = 80,
		cf_s_Mune_Nip_dam_L = 81,
		cf_s_Mune_Nip_dam_R = 82,
		cf_s_Mune_Nip01_s_L = 83,
		cf_s_Mune_Nip01_s_R = 84,
		cf_s_Mune_Nip01_ss_L = 85,
		cf_s_Mune_Nip01_ss_R = 86,
		cf_s_Mune_Nip02_s_L = 87,
		cf_s_Mune_Nip02_s_R = 88,
		cf_s_Mune_Nipacs01_L = 89,
		cf_s_Mune_Nipacs01_R = 90,
		cf_s_Mune_Nipacs02_L = 91,
		cf_s_Mune_Nipacs02_R = 92,
		cf_s_Mune00_h_L = 93,
		cf_s_Mune00_h_R = 94,
		cf_s_Mune00_s_L = 95,
		cf_s_Mune00_s_R = 96,
		cf_s_Mune00_ss_02_L = 97,
		cf_s_Mune00_ss_02_R = 98,
		cf_s_Mune00_ss_02sz_L = 99,
		cf_s_Mune00_ss_02sz_R = 100,
		cf_s_Mune00_ss_03_L = 101,
		cf_s_Mune00_ss_03_R = 102,
		cf_s_Mune00_ss_03sz_L = 103,
		cf_s_Mune00_ss_03sz_R = 104,
		cf_s_Mune00_ss_ty_L = 105,
		cf_s_Mune00_ss_ty_R = 106,
		cf_s_Mune01_s_L = 107,
		cf_s_Mune01_s_R = 108,
		cf_s_Mune01_s_rx_L = 109,
		cf_s_Mune01_s_rx_R = 110,
		cf_s_Mune01_s_ry_L = 111,
		cf_s_Mune01_s_ry_R = 112,
		cf_s_Mune01_s_tx_L = 113,
		cf_s_Mune01_s_tx_R = 114,
		cf_s_Mune01_s_tz_L = 115,
		cf_s_Mune01_s_tz_R = 116,
		cf_s_Mune02_s_L = 117,
		cf_s_Mune02_s_R = 118,
		cf_s_Mune02_s_rx_L = 119,
		cf_s_Mune02_s_rx_R = 120,
		cf_s_Mune02_s_tz_L = 121,
		cf_s_Mune02_s_tz_R = 122,
		cf_s_Mune03_s_L = 123,
		cf_s_Mune03_s_R = 124,
		cf_s_Mune03_s_rx_L = 125,
		cf_s_Mune03_s_rx_R = 126,
		cf_s_Mune04_s_L = 127,
		cf_s_Mune04_s_R = 128,
		cf_s_Neck_h = 129,
		cf_s_Neck_s = 130,
		cf_s_Neck_s_sz = 131,
		cf_s_Shoulder_h_L = 132,
		cf_s_Shoulder_h_R = 133,
		cf_s_Shoulder02_h_L = 134,
		cf_s_Shoulder02_h_R = 135,
		cf_s_Shoulder02_s_L = 136,
		cf_s_Shoulder02_s_R = 137,
		cf_s_Shoulder02_s_tx_L = 138,
		cf_s_Shoulder02_s_tx_R = 139,
		cf_s_Siri_kosi01_s_L = 140,
		cf_s_Siri_kosi01_s_R = 141,
		cf_s_Siri_kosi02_s_L = 142,
		cf_s_Siri_kosi02_s_R = 143,
		cf_s_Siri_legup01_s_L = 144,
		cf_s_Siri_legup01_s_R = 145,
		cf_s_Siri_s_L = 146,
		cf_s_Siri_s_R = 147,
		cf_s_Siri_s_ty_L = 148,
		cf_s_Siri_s_ty_R = 149,
		cf_s_sk_siri_dam = 150,
		cf_s_sk_siri_ty_dam = 151,
		cf_s_sk_top_h = 152,
		cf_s_Spine01_h = 153,
		cf_s_Spine01_s = 154,
		cf_s_Spine01_s_sz = 155,
		cf_s_Spine01_s_ty = 156,
		cf_s_Spine02_h = 157,
		cf_s_Spine02_s = 158,
		cf_s_Spine02_s_sz = 159,
		cf_s_Spine03_h = 160,
		cf_s_Spine03_s = 161,
		cf_s_Spine03_s_sz = 162,
		cf_s_sk_00_sx01 = 163,
		cf_s_sk_00_sx02 = 164,
		cf_s_sk_00_sz01 = 165,
		cf_s_sk_00_sz02 = 166,
		cf_s_sk_01_sx01 = 167,
		cf_s_sk_01_sx02 = 168,
		cf_s_sk_01_sz01 = 169,
		cf_s_sk_01_sz02 = 170,
		cf_s_sk_02_sx01 = 171,
		cf_s_sk_02_sx02 = 172,
		cf_s_sk_02_sz01 = 173,
		cf_s_sk_02_sz02 = 174,
		cf_s_sk_03_sx01 = 175,
		cf_s_sk_03_sx02 = 176,
		cf_s_sk_03_sz01 = 177,
		cf_s_sk_03_sz02 = 178,
		cf_s_sk_04_sx01 = 179,
		cf_s_sk_04_sx02 = 180,
		cf_s_sk_04_sz01 = 181,
		cf_s_sk_04_sz02 = 182,
		cf_s_sk_05_sx01 = 183,
		cf_s_sk_05_sx02 = 184,
		cf_s_sk_05_sz01 = 185,
		cf_s_sk_05_sz02 = 186,
		cf_s_sk_06_sx01 = 187,
		cf_s_sk_06_sx02 = 188,
		cf_s_sk_06_sz01 = 189,
		cf_s_sk_06_sz02 = 190,
		cf_s_sk_07_sx01 = 191,
		cf_s_sk_07_sx02 = 192,
		cf_s_sk_07_sz01 = 193,
		cf_s_sk_07_sz02 = 194,
		cf_hit_Kosi02_Kosi01sx_a = 195,
		cf_hit_Kosi02_Kosi01sz_a = 196,
		cf_hit_Kosi02_Kosi02sx_a = 197,
		cf_hit_Kosi02_Kosi02sz_a = 198,
		cf_hit_LegUp01_Kosi02sz_a = 199,
		cf_hit_LegUp01_Kosi02sx_a = 200,
		cf_hit_Siri_Kosi02sz_a = 201,
		cf_hit_Siri_Kosi02sx_a = 202,
		cf_hit_Siri_size_a = 203,
		cf_hit_Siri_rot_a = 204,
		cf_hit_Mune_size_a = 205,
		cf_hit_Siri_LegUp01 = 206,
		cf_hit_height = 207
	}

	public const int UPDATE_MASK_BUST_L = 1;

	public const int UPDATE_MASK_BUST_R = 2;

	public const int UPDATE_MASK_NIP_L = 4;

	public const int UPDATE_MASK_NIP_R = 8;

	public const int UPDATE_MASK_ETC = 16;

	public const int UPDATE_MASK_ALL = 31;

	public int updateMask = 31;

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
			float num = dictSrcBoneInfo[195].vctPos.y + dictSrcBoneInfo[196].vctPos.y;
			float num2 = dictSrcBoneInfo[195].vctScl.y * dictSrcBoneInfo[196].vctScl.y;
			float x = dictSrcBoneInfo[196].vctScl.x;
			float z = dictSrcBoneInfo[196].vctPos.z;
			float num3 = dictSrcBoneInfo[197].vctPos.y + dictSrcBoneInfo[198].vctPos.y;
			float num4 = dictSrcBoneInfo[197].vctPos.z + dictSrcBoneInfo[198].vctPos.z;
			float num5 = dictSrcBoneInfo[197].vctScl.y * dictSrcBoneInfo[198].vctScl.y;
			float num6 = dictSrcBoneInfo[197].vctScl.x * dictSrcBoneInfo[198].vctScl.x;
			float num7 = dictSrcBoneInfo[199].vctPos.x + dictSrcBoneInfo[200].vctPos.x;
			float num8 = dictSrcBoneInfo[199].vctRot.z + dictSrcBoneInfo[200].vctRot.z;
			float x2 = dictSrcBoneInfo[199].vctScl.x * dictSrcBoneInfo[200].vctScl.x;
			float y = dictSrcBoneInfo[199].vctScl.y * dictSrcBoneInfo[200].vctScl.y;
			float num9 = dictSrcBoneInfo[201].vctScl.x * dictSrcBoneInfo[202].vctScl.x;
			float num10 = dictSrcBoneInfo[201].vctScl.y * dictSrcBoneInfo[202].vctScl.y;
			float x3 = dictSrcBoneInfo[202].vctPos.x;
			float num11 = dictSrcBoneInfo[201].vctPos.z + dictSrcBoneInfo[202].vctPos.z;
			float x4 = dictSrcBoneInfo[203].vctPos.x;
			float y2 = dictSrcBoneInfo[203].vctPos.y;
			float z2 = dictSrcBoneInfo[203].vctPos.z;
			float x5 = dictSrcBoneInfo[203].vctScl.x;
			float y3 = dictSrcBoneInfo[203].vctScl.y;
			float x6 = dictSrcBoneInfo[204].vctPos.x;
			float y4 = dictSrcBoneInfo[204].vctPos.y;
			float z3 = dictSrcBoneInfo[204].vctPos.z;
			float x7 = dictSrcBoneInfo[204].vctScl.x;
			float x8 = dictSrcBoneInfo[204].vctRot.x;
			float z4 = dictSrcBoneInfo[206].vctPos.z;
			float x9 = dictSrcBoneInfo[206].vctScl.x;
			if (((uint)updateMask & 0x10u) != 0)
			{
				dictDstBoneInfo[71].trfBone.SetLocalScale(dictSrcBoneInfo[34].vctScl.x, dictSrcBoneInfo[34].vctScl.y, dictSrcBoneInfo[34].vctScl.z);
				dictDstBoneInfo[67].trfBone.SetLocalScale(dictSrcBoneInfo[152].vctScl.x, 1f, dictSrcBoneInfo[152].vctScl.z);
				dictDstBoneInfo[72].trfBone.SetLocalPositionZ(dictSrcBoneInfo[163].vctPos.z + dictSrcBoneInfo[164].vctPos.z + dictSrcBoneInfo[165].vctPos.z + dictSrcBoneInfo[166].vctPos.z);
				dictDstBoneInfo[72].trfBone.SetLocalRotation(dictSrcBoneInfo[163].vctRot.x + dictSrcBoneInfo[165].vctRot.x + dictSrcBoneInfo[166].vctRot.x, dictSrcBoneInfo[163].vctRot.y, 0f);
				dictDstBoneInfo[73].trfBone.SetLocalPositionX(dictSrcBoneInfo[167].vctPos.x);
				dictDstBoneInfo[73].trfBone.SetLocalPositionZ(dictSrcBoneInfo[167].vctPos.z + dictSrcBoneInfo[168].vctPos.z + dictSrcBoneInfo[169].vctPos.z + dictSrcBoneInfo[170].vctPos.z);
				dictDstBoneInfo[73].trfBone.SetLocalRotation(dictSrcBoneInfo[167].vctRot.x + dictSrcBoneInfo[168].vctRot.x + dictSrcBoneInfo[169].vctRot.x + dictSrcBoneInfo[170].vctRot.x, dictSrcBoneInfo[167].vctRot.y, 0f);
				dictDstBoneInfo[74].trfBone.SetLocalPositionX(dictSrcBoneInfo[171].vctPos.x + dictSrcBoneInfo[172].vctPos.x);
				dictDstBoneInfo[74].trfBone.SetLocalPositionZ(dictSrcBoneInfo[171].vctPos.z + dictSrcBoneInfo[172].vctPos.z + dictSrcBoneInfo[173].vctPos.z + dictSrcBoneInfo[174].vctPos.z);
				dictDstBoneInfo[74].trfBone.SetLocalRotation(dictSrcBoneInfo[171].vctRot.x + dictSrcBoneInfo[172].vctRot.x, dictSrcBoneInfo[171].vctRot.y, 0f);
				dictDstBoneInfo[75].trfBone.SetLocalPositionX(dictSrcBoneInfo[175].vctPos.x);
				dictDstBoneInfo[75].trfBone.SetLocalPositionZ(dictSrcBoneInfo[175].vctPos.z + dictSrcBoneInfo[176].vctPos.z + dictSrcBoneInfo[177].vctPos.z + dictSrcBoneInfo[178].vctPos.z);
				dictDstBoneInfo[75].trfBone.SetLocalRotation(dictSrcBoneInfo[175].vctRot.x + dictSrcBoneInfo[176].vctRot.x + dictSrcBoneInfo[177].vctRot.x + dictSrcBoneInfo[178].vctRot.x, dictSrcBoneInfo[175].vctRot.y, 0f);
				dictDstBoneInfo[76].trfBone.SetLocalPositionZ(dictSrcBoneInfo[179].vctPos.z + dictSrcBoneInfo[180].vctPos.z + dictSrcBoneInfo[181].vctPos.z + dictSrcBoneInfo[182].vctPos.z);
				dictDstBoneInfo[76].trfBone.SetLocalRotation(dictSrcBoneInfo[179].vctRot.x + dictSrcBoneInfo[181].vctRot.x + dictSrcBoneInfo[182].vctRot.x, dictSrcBoneInfo[179].vctRot.y, 0f);
				dictDstBoneInfo[77].trfBone.SetLocalPositionX(dictSrcBoneInfo[183].vctPos.x + dictSrcBoneInfo[184].vctPos.x);
				dictDstBoneInfo[77].trfBone.SetLocalPositionZ(dictSrcBoneInfo[183].vctPos.z + dictSrcBoneInfo[184].vctPos.z + dictSrcBoneInfo[185].vctPos.z + dictSrcBoneInfo[186].vctPos.z);
				dictDstBoneInfo[77].trfBone.SetLocalRotation(dictSrcBoneInfo[183].vctRot.x + dictSrcBoneInfo[184].vctRot.x + dictSrcBoneInfo[185].vctRot.x + dictSrcBoneInfo[186].vctRot.x, dictSrcBoneInfo[183].vctRot.y, 0f);
				dictDstBoneInfo[78].trfBone.SetLocalPositionX(dictSrcBoneInfo[187].vctPos.x + dictSrcBoneInfo[188].vctPos.x);
				dictDstBoneInfo[78].trfBone.SetLocalPositionZ(dictSrcBoneInfo[187].vctPos.z + dictSrcBoneInfo[188].vctPos.z + dictSrcBoneInfo[189].vctPos.z + dictSrcBoneInfo[190].vctPos.z);
				dictDstBoneInfo[78].trfBone.SetLocalRotation(dictSrcBoneInfo[187].vctRot.x + dictSrcBoneInfo[188].vctRot.x, dictSrcBoneInfo[187].vctRot.y, 0f);
				dictDstBoneInfo[79].trfBone.SetLocalPositionX(dictSrcBoneInfo[191].vctPos.x + dictSrcBoneInfo[192].vctPos.x);
				dictDstBoneInfo[79].trfBone.SetLocalPositionZ(dictSrcBoneInfo[191].vctPos.z + dictSrcBoneInfo[192].vctPos.z + dictSrcBoneInfo[193].vctPos.z + dictSrcBoneInfo[194].vctPos.z);
				dictDstBoneInfo[79].trfBone.SetLocalRotation(dictSrcBoneInfo[191].vctRot.x + dictSrcBoneInfo[192].vctRot.x + dictSrcBoneInfo[193].vctRot.x + dictSrcBoneInfo[194].vctRot.x, dictSrcBoneInfo[191].vctRot.y, 0f);
				dictDstBoneInfo[66].trfBone.SetLocalPositionZ(dictSrcBoneInfo[151].vctPos.z + dictSrcBoneInfo[150].vctPos.z);
				dictDstBoneInfo[66].trfBone.SetLocalRotation(dictSrcBoneInfo[151].vctRot.x, 0f, 0f);
				dictDstBoneInfo[62].trfBone.SetLocalPositionX(dictSrcBoneInfo[138].vctPos.x);
				dictDstBoneInfo[62].trfBone.SetLocalScale(dictSrcBoneInfo[134].vctScl.x, dictSrcBoneInfo[136].vctScl.y * dictSrcBoneInfo[134].vctScl.z, dictSrcBoneInfo[136].vctScl.z * dictSrcBoneInfo[134].vctScl.y);
				dictDstBoneInfo[63].trfBone.SetLocalPositionX(dictSrcBoneInfo[139].vctPos.x);
				dictDstBoneInfo[63].trfBone.SetLocalScale(dictSrcBoneInfo[135].vctScl.x, dictSrcBoneInfo[137].vctScl.y * dictSrcBoneInfo[135].vctScl.z, dictSrcBoneInfo[137].vctScl.z * dictSrcBoneInfo[135].vctScl.y);
				dictDstBoneInfo[6].trfBone.SetLocalPositionX(dictSrcBoneInfo[16].vctPos.x);
				dictDstBoneInfo[6].trfBone.SetLocalPositionY(dictSrcBoneInfo[14].vctPos.y + dictSrcBoneInfo[16].vctPos.y);
				dictDstBoneInfo[6].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[14].vctRot.y, dictSrcBoneInfo[14].vctRot.z + dictSrcBoneInfo[16].vctRot.z);
				dictDstBoneInfo[6].trfBone.SetLocalScale(1f, dictSrcBoneInfo[14].vctScl.y * dictSrcBoneInfo[12].vctScl.y, dictSrcBoneInfo[14].vctScl.z * dictSrcBoneInfo[12].vctScl.z);
				dictDstBoneInfo[7].trfBone.SetLocalPositionX(dictSrcBoneInfo[17].vctPos.x);
				dictDstBoneInfo[7].trfBone.SetLocalPositionY(dictSrcBoneInfo[15].vctPos.y + dictSrcBoneInfo[17].vctPos.y);
				dictDstBoneInfo[7].trfBone.SetLocalRotation(0f, dictSrcBoneInfo[15].vctRot.y, dictSrcBoneInfo[15].vctRot.z + dictSrcBoneInfo[17].vctRot.z);
				dictDstBoneInfo[7].trfBone.SetLocalScale(1f, dictSrcBoneInfo[15].vctScl.y * dictSrcBoneInfo[13].vctScl.y, dictSrcBoneInfo[15].vctScl.z * dictSrcBoneInfo[13].vctScl.z);
				dictDstBoneInfo[8].trfBone.SetLocalScale(1f, dictSrcBoneInfo[20].vctScl.y * dictSrcBoneInfo[18].vctScl.y, dictSrcBoneInfo[20].vctScl.z * dictSrcBoneInfo[18].vctScl.z);
				dictDstBoneInfo[9].trfBone.SetLocalScale(1f, dictSrcBoneInfo[21].vctScl.y * dictSrcBoneInfo[19].vctScl.y, dictSrcBoneInfo[21].vctScl.z * dictSrcBoneInfo[19].vctScl.z);
				dictDstBoneInfo[10].trfBone.SetLocalScale(1f, dictSrcBoneInfo[24].vctScl.y * dictSrcBoneInfo[22].vctScl.y, dictSrcBoneInfo[24].vctScl.z * dictSrcBoneInfo[22].vctScl.z);
				dictDstBoneInfo[11].trfBone.SetLocalScale(1f, dictSrcBoneInfo[25].vctScl.y * dictSrcBoneInfo[23].vctScl.y, dictSrcBoneInfo[25].vctScl.z * dictSrcBoneInfo[23].vctScl.z);
				dictDstBoneInfo[0].trfBone.SetLocalScale(1f, dictSrcBoneInfo[2].vctScl.y * dictSrcBoneInfo[0].vctScl.y, dictSrcBoneInfo[2].vctScl.z * dictSrcBoneInfo[0].vctScl.z);
				dictDstBoneInfo[1].trfBone.SetLocalScale(1f, dictSrcBoneInfo[3].vctScl.y * dictSrcBoneInfo[1].vctScl.y, dictSrcBoneInfo[3].vctScl.z * dictSrcBoneInfo[1].vctScl.z);
				dictDstBoneInfo[2].trfBone.SetLocalScale(1f, dictSrcBoneInfo[6].vctScl.y * dictSrcBoneInfo[4].vctScl.y, dictSrcBoneInfo[6].vctScl.z * dictSrcBoneInfo[4].vctScl.z);
				dictDstBoneInfo[3].trfBone.SetLocalScale(1f, dictSrcBoneInfo[7].vctScl.y * dictSrcBoneInfo[5].vctScl.y, dictSrcBoneInfo[7].vctScl.z * dictSrcBoneInfo[5].vctScl.z);
				dictDstBoneInfo[4].trfBone.SetLocalScale(1f, dictSrcBoneInfo[10].vctScl.y * dictSrcBoneInfo[8].vctScl.y, dictSrcBoneInfo[10].vctScl.z * dictSrcBoneInfo[8].vctScl.z);
				dictDstBoneInfo[5].trfBone.SetLocalScale(1f, dictSrcBoneInfo[11].vctScl.y * dictSrcBoneInfo[9].vctScl.y, dictSrcBoneInfo[11].vctScl.z * dictSrcBoneInfo[9].vctScl.z);
				dictDstBoneInfo[12].trfBone.SetLocalScale(dictSrcBoneInfo[26].vctScl.x, dictSrcBoneInfo[26].vctScl.y, dictSrcBoneInfo[26].vctScl.z);
				dictDstBoneInfo[13].trfBone.SetLocalScale(dictSrcBoneInfo[27].vctScl.x, dictSrcBoneInfo[27].vctScl.y, dictSrcBoneInfo[27].vctScl.z);
				dictDstBoneInfo[14].trfBone.SetLocalScale(1f, dictSrcBoneInfo[30].vctScl.y * dictSrcBoneInfo[28].vctScl.y, dictSrcBoneInfo[30].vctScl.z * dictSrcBoneInfo[28].vctScl.z);
				dictDstBoneInfo[15].trfBone.SetLocalScale(1f, dictSrcBoneInfo[31].vctScl.y * dictSrcBoneInfo[29].vctScl.y, dictSrcBoneInfo[31].vctScl.z * dictSrcBoneInfo[29].vctScl.z);
				dictDstBoneInfo[17].trfBone.SetLocalScale(dictSrcBoneInfo[35].vctScl.x * dictSrcBoneInfo[36].vctScl.x, 1f, dictSrcBoneInfo[35].vctScl.z * dictSrcBoneInfo[37].vctScl.z);
				dictDstBoneInfo[18].trfBone.SetLocalScale(dictSrcBoneInfo[38].vctScl.x * dictSrcBoneInfo[39].vctScl.x, 1f, dictSrcBoneInfo[38].vctScl.z * dictSrcBoneInfo[40].vctScl.z);
				dictDstBoneInfo[68].trfBone.SetLocalScale(dictSrcBoneInfo[153].vctScl.x * dictSrcBoneInfo[154].vctScl.x, 1f, dictSrcBoneInfo[153].vctScl.z * dictSrcBoneInfo[155].vctScl.z);
				dictDstBoneInfo[68].trfBone.SetLocalPositionY(dictSrcBoneInfo[156].vctPos.y);
				dictDstBoneInfo[68].trfBone.SetLocalPositionZ(dictSrcBoneInfo[154].vctPos.z + dictSrcBoneInfo[155].vctPos.z);
				dictDstBoneInfo[69].trfBone.SetLocalScale(dictSrcBoneInfo[157].vctScl.x * dictSrcBoneInfo[158].vctScl.x, 1f, dictSrcBoneInfo[157].vctScl.z * dictSrcBoneInfo[159].vctScl.z);
				dictDstBoneInfo[70].trfBone.SetLocalScale(dictSrcBoneInfo[160].vctScl.x * dictSrcBoneInfo[161].vctScl.x, 1f, dictSrcBoneInfo[160].vctScl.z * dictSrcBoneInfo[162].vctScl.z);
				dictDstBoneInfo[61].trfBone.SetLocalScale(dictSrcBoneInfo[129].vctScl.x * dictSrcBoneInfo[130].vctScl.x, 1f, dictSrcBoneInfo[129].vctScl.z * dictSrcBoneInfo[131].vctScl.z);
				dictDstBoneInfo[16].trfBone.SetLocalScale(dictSrcBoneInfo[32].vctScl.x * dictSrcBoneInfo[33].vctScl.x, dictSrcBoneInfo[32].vctScl.y * dictSrcBoneInfo[33].vctScl.y, dictSrcBoneInfo[32].vctScl.z * dictSrcBoneInfo[33].vctScl.z);
			}
			if (((uint)updateMask & (true ? 1u : 0u)) != 0)
			{
				float x10 = dictSrcBoneInfo[205].vctPos.x;
				float y5 = dictSrcBoneInfo[205].vctPos.y;
				float z5 = dictSrcBoneInfo[205].vctPos.z;
				float x11 = dictSrcBoneInfo[205].vctScl.x;
				dictDstBoneInfo[47].trfBone.SetLocalPositionX(dictSrcBoneInfo[97].vctPos.x + dictSrcBoneInfo[101].vctPos.x);
				dictDstBoneInfo[47].trfBone.SetLocalPositionY(dictSrcBoneInfo[99].vctPos.y + dictSrcBoneInfo[105].vctPos.y);
				dictDstBoneInfo[47].trfBone.SetLocalPositionZ(dictSrcBoneInfo[99].vctPos.z + dictSrcBoneInfo[103].vctPos.z);
				dictDstBoneInfo[47].trfBone.SetLocalRotation(dictSrcBoneInfo[99].vctRot.x + dictSrcBoneInfo[103].vctRot.x + dictSrcBoneInfo[105].vctRot.x, dictSrcBoneInfo[101].vctRot.y + dictSrcBoneInfo[105].vctRot.y, 0f);
				dictDstBoneInfo[47].trfBone.SetLocalScale(dictSrcBoneInfo[93].vctScl.x, dictSrcBoneInfo[93].vctScl.y, dictSrcBoneInfo[93].vctScl.z);
				dictDstBoneInfo[45].trfBone.SetLocalPositionY(dictSrcBoneInfo[95].vctPos.y);
				dictDstBoneInfo[45].trfBone.SetLocalPositionZ(dictSrcBoneInfo[95].vctPos.z);
				dictDstBoneInfo[45].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[95].vctRot.z);
				dictDstBoneInfo[45].trfBone.SetLocalScale(dictSrcBoneInfo[95].vctScl.x, dictSrcBoneInfo[95].vctScl.y, dictSrcBoneInfo[95].vctScl.z);
				dictDstBoneInfo[43].trfBone.SetLocalPositionX(dictSrcBoneInfo[113].vctPos.x + dictSrcBoneInfo[111].vctPos.x);
				dictDstBoneInfo[43].trfBone.SetLocalPositionZ(0.072f + dictSrcBoneInfo[109].vctPos.z + dictSrcBoneInfo[111].vctPos.z);
				dictDstBoneInfo[43].trfBone.SetLocalRotation(dictSrcBoneInfo[109].vctRot.x, dictSrcBoneInfo[111].vctRot.y, 0f);
				dictDstBoneInfo[49].trfBone.SetLocalPositionY(dictSrcBoneInfo[107].vctPos.y);
				dictDstBoneInfo[49].trfBone.SetLocalRotation(dictSrcBoneInfo[107].vctRot.x, dictSrcBoneInfo[107].vctRot.y, dictSrcBoneInfo[107].vctRot.z);
				dictDstBoneInfo[49].trfBone.SetLocalScale(dictSrcBoneInfo[107].vctScl.x, dictSrcBoneInfo[107].vctScl.y, dictSrcBoneInfo[107].vctScl.z);
				dictDstBoneInfo[51].trfBone.SetLocalPositionX(dictSrcBoneInfo[107].vctPos.x);
				dictDstBoneInfo[51].trfBone.SetLocalPositionZ(0.03f + dictSrcBoneInfo[115].vctPos.z);
				dictDstBoneInfo[51].trfBone.SetLocalRotation(dictSrcBoneInfo[119].vctRot.x, 0f, 0f);
				dictDstBoneInfo[53].trfBone.SetLocalPositionY(dictSrcBoneInfo[117].vctPos.y);
				dictDstBoneInfo[53].trfBone.SetLocalPositionZ(dictSrcBoneInfo[117].vctPos.z);
				dictDstBoneInfo[53].trfBone.SetLocalRotation(dictSrcBoneInfo[117].vctRot.x, 0f, 0f);
				dictDstBoneInfo[53].trfBone.SetLocalScale(dictSrcBoneInfo[117].vctScl.x, dictSrcBoneInfo[117].vctScl.y, dictSrcBoneInfo[117].vctScl.z);
				dictDstBoneInfo[55].trfBone.SetLocalPositionZ(0.03f + dictSrcBoneInfo[121].vctPos.z);
				dictDstBoneInfo[55].trfBone.SetLocalRotation(dictSrcBoneInfo[125].vctRot.x, 0f, 0f);
				dictDstBoneInfo[57].trfBone.SetLocalPositionZ(dictSrcBoneInfo[123].vctPos.z);
				dictDstBoneInfo[57].trfBone.SetLocalScale(dictSrcBoneInfo[123].vctScl.x, dictSrcBoneInfo[123].vctScl.y, dictSrcBoneInfo[123].vctScl.z);
				dictDstBoneInfo[59].trfBone.SetLocalPositionZ(dictSrcBoneInfo[127].vctPos.z);
				dictDstBoneInfo[59].trfBone.SetLocalScale(dictSrcBoneInfo[127].vctScl.x, dictSrcBoneInfo[127].vctScl.y, dictSrcBoneInfo[127].vctScl.z);
				dictDstBoneInfo[37].trfBone.SetLocalPositionZ(dictSrcBoneInfo[85].vctPos.z + dictSrcBoneInfo[83].vctPos.z);
				dictDstBoneInfo[37].trfBone.SetLocalScale(dictSrcBoneInfo[85].vctScl.x * dictSrcBoneInfo[83].vctScl.x, dictSrcBoneInfo[85].vctScl.y * dictSrcBoneInfo[83].vctScl.y, dictSrcBoneInfo[85].vctScl.z);
				int num12 = 80;
				dictDstBoneInfo[80].trfBone.SetLocalPositionX(x10);
				dictDstBoneInfo[80].trfBone.SetLocalPositionY(y5);
				dictDstBoneInfo[80].trfBone.SetLocalPositionZ(z5);
				dictDstBoneInfo[80].trfBone.SetLocalScale(x11, 1f, x11);
			}
			if (((uint)updateMask & 4u) != 0)
			{
				dictDstBoneInfo[39].trfBone.SetLocalPositionZ(dictSrcBoneInfo[87].vctPos.z);
				dictDstBoneInfo[39].trfBone.SetLocalScale(dictSrcBoneInfo[87].vctScl.x, dictSrcBoneInfo[87].vctScl.y, dictSrcBoneInfo[87].vctScl.z);
				dictDstBoneInfo[41].trfBone.SetLocalPositionZ(dictSrcBoneInfo[89].vctPos.z + dictSrcBoneInfo[91].vctPos.z);
				dictDstBoneInfo[41].trfBone.SetLocalScale(dictSrcBoneInfo[81].vctScl.x * dictSrcBoneInfo[91].vctScl.x, dictSrcBoneInfo[81].vctScl.y * dictSrcBoneInfo[91].vctScl.y, dictSrcBoneInfo[81].vctScl.z * dictSrcBoneInfo[91].vctScl.z);
			}
			if (((uint)updateMask & 2u) != 0)
			{
				float x12 = dictSrcBoneInfo[205].vctPos.x;
				float y6 = dictSrcBoneInfo[205].vctPos.y;
				float z6 = dictSrcBoneInfo[205].vctPos.z;
				float x13 = dictSrcBoneInfo[205].vctScl.x;
				dictDstBoneInfo[48].trfBone.SetLocalPositionX(dictSrcBoneInfo[98].vctPos.x + dictSrcBoneInfo[102].vctPos.x);
				dictDstBoneInfo[48].trfBone.SetLocalPositionY(dictSrcBoneInfo[100].vctPos.y + dictSrcBoneInfo[106].vctPos.y);
				dictDstBoneInfo[48].trfBone.SetLocalPositionZ(dictSrcBoneInfo[100].vctPos.z + dictSrcBoneInfo[104].vctPos.z);
				dictDstBoneInfo[48].trfBone.SetLocalRotation(dictSrcBoneInfo[100].vctRot.x + dictSrcBoneInfo[104].vctRot.x + dictSrcBoneInfo[106].vctRot.x, dictSrcBoneInfo[102].vctRot.y + dictSrcBoneInfo[106].vctRot.y, 0f);
				dictDstBoneInfo[48].trfBone.SetLocalScale(dictSrcBoneInfo[94].vctScl.x, dictSrcBoneInfo[94].vctScl.y, dictSrcBoneInfo[94].vctScl.z);
				dictDstBoneInfo[46].trfBone.SetLocalPositionY(dictSrcBoneInfo[96].vctPos.y);
				dictDstBoneInfo[46].trfBone.SetLocalPositionZ(dictSrcBoneInfo[96].vctPos.z);
				dictDstBoneInfo[46].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[96].vctRot.z);
				dictDstBoneInfo[46].trfBone.SetLocalScale(dictSrcBoneInfo[96].vctScl.x, dictSrcBoneInfo[96].vctScl.y, dictSrcBoneInfo[96].vctScl.z);
				dictDstBoneInfo[44].trfBone.SetLocalPositionX(dictSrcBoneInfo[114].vctPos.x + dictSrcBoneInfo[112].vctPos.x);
				dictDstBoneInfo[44].trfBone.SetLocalPositionZ(0.072f + dictSrcBoneInfo[110].vctPos.z + dictSrcBoneInfo[112].vctPos.z);
				dictDstBoneInfo[44].trfBone.SetLocalRotation(dictSrcBoneInfo[110].vctRot.x, dictSrcBoneInfo[112].vctRot.y, 0f);
				dictDstBoneInfo[50].trfBone.SetLocalPositionY(dictSrcBoneInfo[108].vctPos.y);
				dictDstBoneInfo[50].trfBone.SetLocalRotation(dictSrcBoneInfo[108].vctRot.x, dictSrcBoneInfo[108].vctRot.y, dictSrcBoneInfo[108].vctRot.z);
				dictDstBoneInfo[50].trfBone.SetLocalScale(dictSrcBoneInfo[108].vctScl.x, dictSrcBoneInfo[108].vctScl.y, dictSrcBoneInfo[108].vctScl.z);
				dictDstBoneInfo[52].trfBone.SetLocalPositionX(dictSrcBoneInfo[108].vctPos.x);
				dictDstBoneInfo[52].trfBone.SetLocalPositionZ(0.03f + dictSrcBoneInfo[116].vctPos.z);
				dictDstBoneInfo[52].trfBone.SetLocalRotation(dictSrcBoneInfo[120].vctRot.x, 0f, 0f);
				dictDstBoneInfo[54].trfBone.SetLocalPositionY(dictSrcBoneInfo[118].vctPos.y);
				dictDstBoneInfo[54].trfBone.SetLocalPositionZ(dictSrcBoneInfo[118].vctPos.z);
				dictDstBoneInfo[54].trfBone.SetLocalRotation(dictSrcBoneInfo[118].vctRot.x, 0f, 0f);
				dictDstBoneInfo[54].trfBone.SetLocalScale(dictSrcBoneInfo[118].vctScl.x, dictSrcBoneInfo[118].vctScl.y, dictSrcBoneInfo[118].vctScl.z);
				dictDstBoneInfo[56].trfBone.SetLocalPositionZ(0.03f + dictSrcBoneInfo[122].vctPos.z);
				dictDstBoneInfo[56].trfBone.SetLocalRotation(dictSrcBoneInfo[126].vctRot.x, 0f, 0f);
				dictDstBoneInfo[58].trfBone.SetLocalPositionZ(dictSrcBoneInfo[124].vctPos.z);
				dictDstBoneInfo[58].trfBone.SetLocalScale(dictSrcBoneInfo[124].vctScl.x, dictSrcBoneInfo[124].vctScl.y, dictSrcBoneInfo[124].vctScl.z);
				dictDstBoneInfo[60].trfBone.SetLocalPositionZ(dictSrcBoneInfo[128].vctPos.z);
				dictDstBoneInfo[60].trfBone.SetLocalScale(dictSrcBoneInfo[128].vctScl.x, dictSrcBoneInfo[128].vctScl.y, dictSrcBoneInfo[128].vctScl.z);
				dictDstBoneInfo[38].trfBone.SetLocalPositionZ(dictSrcBoneInfo[86].vctPos.z + dictSrcBoneInfo[84].vctPos.z);
				dictDstBoneInfo[38].trfBone.SetLocalScale(dictSrcBoneInfo[86].vctScl.x * dictSrcBoneInfo[84].vctScl.x, dictSrcBoneInfo[86].vctScl.y * dictSrcBoneInfo[84].vctScl.y, dictSrcBoneInfo[86].vctScl.z);
				dictDstBoneInfo[81].trfBone.SetLocalPositionX(x12);
				dictDstBoneInfo[81].trfBone.SetLocalPositionY(y6);
				dictDstBoneInfo[81].trfBone.SetLocalPositionZ(z6);
				dictDstBoneInfo[81].trfBone.SetLocalScale(x13, 1f, x13);
			}
			if (((uint)updateMask & 8u) != 0)
			{
				dictDstBoneInfo[40].trfBone.SetLocalPositionZ(dictSrcBoneInfo[88].vctPos.z);
				dictDstBoneInfo[40].trfBone.SetLocalScale(dictSrcBoneInfo[88].vctScl.x, dictSrcBoneInfo[88].vctScl.y, dictSrcBoneInfo[88].vctScl.z);
				dictDstBoneInfo[42].trfBone.SetLocalPositionZ(dictSrcBoneInfo[90].vctPos.z + dictSrcBoneInfo[92].vctPos.z);
				dictDstBoneInfo[42].trfBone.SetLocalScale(dictSrcBoneInfo[82].vctScl.x * dictSrcBoneInfo[92].vctScl.x, dictSrcBoneInfo[82].vctScl.y * dictSrcBoneInfo[92].vctScl.y, dictSrcBoneInfo[82].vctScl.z * dictSrcBoneInfo[92].vctScl.z);
			}
			if (((uint)updateMask & 0x10u) != 0)
			{
				dictDstBoneInfo[35].trfBone.SetLocalScale(dictSrcBoneInfo[79].vctScl.x, 1f, dictSrcBoneInfo[79].vctScl.z);
				dictDstBoneInfo[36].trfBone.SetLocalScale(dictSrcBoneInfo[80].vctScl.x, 1f, dictSrcBoneInfo[80].vctScl.z);
				dictDstBoneInfo[29].trfBone.SetLocalPositionX(dictSrcBoneInfo[65].vctPos.x + dictSrcBoneInfo[59].vctPos.x);
				dictDstBoneInfo[29].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[59].vctRot.z);
				dictDstBoneInfo[29].trfBone.SetLocalScale(dictSrcBoneInfo[63].vctScl.x * dictSrcBoneInfo[65].vctScl.x * dictSrcBoneInfo[59].vctScl.x, 1f, dictSrcBoneInfo[63].vctScl.z * dictSrcBoneInfo[65].vctScl.z * dictSrcBoneInfo[61].vctScl.z);
				dictDstBoneInfo[30].trfBone.SetLocalPositionX(dictSrcBoneInfo[66].vctPos.x + dictSrcBoneInfo[60].vctPos.x);
				dictDstBoneInfo[30].trfBone.SetLocalRotation(0f, 0f, dictSrcBoneInfo[60].vctRot.z);
				dictDstBoneInfo[30].trfBone.SetLocalScale(dictSrcBoneInfo[64].vctScl.x * dictSrcBoneInfo[66].vctScl.x * dictSrcBoneInfo[60].vctScl.x, 1f, dictSrcBoneInfo[64].vctScl.z * dictSrcBoneInfo[66].vctScl.z * dictSrcBoneInfo[62].vctScl.z);
				dictDstBoneInfo[31].trfBone.SetLocalScale(dictSrcBoneInfo[69].vctScl.x * dictSrcBoneInfo[71].vctScl.x * dictSrcBoneInfo[67].vctScl.x, 1f, dictSrcBoneInfo[69].vctScl.z * dictSrcBoneInfo[71].vctScl.z * dictSrcBoneInfo[67].vctScl.z);
				dictDstBoneInfo[32].trfBone.SetLocalScale(dictSrcBoneInfo[70].vctScl.x * dictSrcBoneInfo[72].vctScl.x * dictSrcBoneInfo[68].vctScl.x, 1f, dictSrcBoneInfo[70].vctScl.z * dictSrcBoneInfo[72].vctScl.z * dictSrcBoneInfo[68].vctScl.z);
				dictDstBoneInfo[33].trfBone.SetLocalScale(dictSrcBoneInfo[75].vctScl.x * dictSrcBoneInfo[77].vctScl.x * dictSrcBoneInfo[73].vctScl.x, 1f, dictSrcBoneInfo[75].vctScl.z * dictSrcBoneInfo[77].vctScl.z * dictSrcBoneInfo[73].vctScl.z);
				dictDstBoneInfo[34].trfBone.SetLocalScale(dictSrcBoneInfo[76].vctScl.x * dictSrcBoneInfo[78].vctScl.x * dictSrcBoneInfo[74].vctScl.x, 1f, dictSrcBoneInfo[76].vctScl.z * dictSrcBoneInfo[78].vctScl.z * dictSrcBoneInfo[74].vctScl.z);
				dictDstBoneInfo[19].trfBone.SetLocalPositionZ(dictSrcBoneInfo[41].vctPos.z);
				dictDstBoneInfo[19].trfBone.SetLocalScale(dictSrcBoneInfo[41].vctScl.x, 1f, dictSrcBoneInfo[41].vctScl.z);
				dictDstBoneInfo[20].trfBone.SetLocalPositionZ(dictSrcBoneInfo[42].vctPos.z);
				dictDstBoneInfo[20].trfBone.SetLocalScale(dictSrcBoneInfo[42].vctScl.x, 1f, dictSrcBoneInfo[42].vctScl.z);
				dictDstBoneInfo[21].trfBone.SetLocalPositionZ(dictSrcBoneInfo[45].vctPos.z);
				dictDstBoneInfo[21].trfBone.SetLocalScale(dictSrcBoneInfo[47].vctScl.x * dictSrcBoneInfo[45].vctScl.x * dictSrcBoneInfo[43].vctScl.x, 1f, dictSrcBoneInfo[47].vctScl.z * dictSrcBoneInfo[45].vctScl.z * dictSrcBoneInfo[43].vctScl.z);
				dictDstBoneInfo[22].trfBone.SetLocalPositionZ(dictSrcBoneInfo[46].vctPos.z);
				dictDstBoneInfo[22].trfBone.SetLocalScale(dictSrcBoneInfo[48].vctScl.x * dictSrcBoneInfo[46].vctScl.x * dictSrcBoneInfo[44].vctScl.x, 1f, dictSrcBoneInfo[48].vctScl.z * dictSrcBoneInfo[46].vctScl.z * dictSrcBoneInfo[44].vctScl.z);
				dictDstBoneInfo[23].trfBone.SetLocalRotation(dictSrcBoneInfo[51].vctRot.x, 0f, 0f);
				dictDstBoneInfo[23].trfBone.SetLocalScale(dictSrcBoneInfo[49].vctScl.x * dictSrcBoneInfo[51].vctScl.x, 1f, dictSrcBoneInfo[49].vctScl.z * dictSrcBoneInfo[51].vctScl.z);
				dictDstBoneInfo[24].trfBone.SetLocalRotation(dictSrcBoneInfo[52].vctRot.x, 0f, 0f);
				dictDstBoneInfo[24].trfBone.SetLocalScale(dictSrcBoneInfo[50].vctScl.x * dictSrcBoneInfo[52].vctScl.x, 1f, dictSrcBoneInfo[50].vctScl.z * dictSrcBoneInfo[52].vctScl.z);
				dictDstBoneInfo[25].trfBone.SetLocalScale(dictSrcBoneInfo[53].vctScl.x * dictSrcBoneInfo[55].vctScl.x, 1f, dictSrcBoneInfo[53].vctScl.z * dictSrcBoneInfo[55].vctScl.z);
				dictDstBoneInfo[26].trfBone.SetLocalScale(dictSrcBoneInfo[54].vctScl.x * dictSrcBoneInfo[56].vctScl.x, 1f, dictSrcBoneInfo[54].vctScl.z * dictSrcBoneInfo[56].vctScl.z);
				dictDstBoneInfo[27].trfBone.SetLocalPositionX(dictSrcBoneInfo[57].vctPos.x);
				dictDstBoneInfo[27].trfBone.SetLocalPositionZ(dictSrcBoneInfo[57].vctPos.z);
				dictDstBoneInfo[27].trfBone.SetLocalRotation(dictSrcBoneInfo[57].vctRot.x, 0f, dictSrcBoneInfo[57].vctRot.z);
				dictDstBoneInfo[27].trfBone.SetLocalScale(dictSrcBoneInfo[57].vctScl.x, 1f, dictSrcBoneInfo[57].vctScl.z);
				dictDstBoneInfo[28].trfBone.SetLocalPositionX(dictSrcBoneInfo[58].vctPos.x);
				dictDstBoneInfo[28].trfBone.SetLocalPositionZ(dictSrcBoneInfo[57].vctPos.z);
				dictDstBoneInfo[28].trfBone.SetLocalRotation(dictSrcBoneInfo[58].vctRot.x, 0f, dictSrcBoneInfo[58].vctRot.z);
				dictDstBoneInfo[28].trfBone.SetLocalScale(dictSrcBoneInfo[58].vctScl.x, 1f, dictSrcBoneInfo[58].vctScl.z);
				dictDstBoneInfo[64].trfBone.SetLocalPosition(dictSrcBoneInfo[146].vctPos.x, dictSrcBoneInfo[148].vctPos.y + dictSrcBoneInfo[146].vctPos.y, dictSrcBoneInfo[144].vctPos.z + dictSrcBoneInfo[146].vctPos.z);
				dictDstBoneInfo[64].trfBone.SetLocalRotation(dictSrcBoneInfo[148].vctRot.x, 0f, 0f);
				dictDstBoneInfo[64].trfBone.SetLocalScale(dictSrcBoneInfo[142].vctScl.x * dictSrcBoneInfo[144].vctScl.x * dictSrcBoneInfo[146].vctScl.x, dictSrcBoneInfo[146].vctScl.y, dictSrcBoneInfo[140].vctScl.z * dictSrcBoneInfo[142].vctScl.z * dictSrcBoneInfo[144].vctScl.z * dictSrcBoneInfo[146].vctScl.z);
				dictDstBoneInfo[65].trfBone.SetLocalPosition(dictSrcBoneInfo[147].vctPos.x, dictSrcBoneInfo[149].vctPos.y + dictSrcBoneInfo[147].vctPos.y, dictSrcBoneInfo[145].vctPos.z + dictSrcBoneInfo[147].vctPos.z);
				dictDstBoneInfo[65].trfBone.SetLocalRotation(dictSrcBoneInfo[149].vctRot.x, 0f, 0f);
				dictDstBoneInfo[65].trfBone.SetLocalScale(dictSrcBoneInfo[143].vctScl.x * dictSrcBoneInfo[145].vctScl.x * dictSrcBoneInfo[147].vctScl.x, dictSrcBoneInfo[147].vctScl.y, dictSrcBoneInfo[141].vctScl.z * dictSrcBoneInfo[143].vctScl.z * dictSrcBoneInfo[145].vctScl.z * dictSrcBoneInfo[147].vctScl.z);
				dictDstBoneInfo[82].trfBone.SetLocalPositionY(num + num3);
				dictDstBoneInfo[82].trfBone.SetLocalPositionZ(z + num4);
				dictDstBoneInfo[82].trfBone.SetLocalScale(x * num6 * dictSrcBoneInfo[207].vctScl.x, num2 * num5, x * num6);
				dictDstBoneInfo[83].trfBone.SetLocalPositionX(0f - num7);
				dictDstBoneInfo[83].trfBone.SetLocalPositionY(dictSrcBoneInfo[199].vctPos.y);
				dictDstBoneInfo[83].trfBone.SetLocalRotation(dictSrcBoneInfo[199].vctRot.x, 0f, 0f - num8);
				dictDstBoneInfo[83].trfBone.SetLocalScale(x2, y, 1f);
				dictDstBoneInfo[84].trfBone.SetLocalPositionX(num7);
				dictDstBoneInfo[84].trfBone.SetLocalPositionY(dictSrcBoneInfo[199].vctPos.y);
				dictDstBoneInfo[84].trfBone.SetLocalRotation(dictSrcBoneInfo[199].vctRot.x, 0f, num8);
				dictDstBoneInfo[84].trfBone.SetLocalScale(x2, y, 1f);
				dictDstBoneInfo[85].trfBone.SetLocalPositionX(0f - (x3 + x4 + x6));
				dictDstBoneInfo[85].trfBone.SetLocalPositionY(y2 + y4);
				dictDstBoneInfo[85].trfBone.SetLocalPositionZ(num11 + z2 + z3 + z4);
				dictDstBoneInfo[85].trfBone.SetLocalRotation(x8, 0f, 0f);
				dictDstBoneInfo[85].trfBone.SetLocalScale(num9 * x5 * x7 * x9 / dictSrcBoneInfo[207].vctScl.x, num10 * y3, num9 * x5 * x7);
				dictDstBoneInfo[86].trfBone.SetLocalPositionX(x3 + x4 + x6);
				dictDstBoneInfo[86].trfBone.SetLocalPositionY(y2 + y4);
				dictDstBoneInfo[86].trfBone.SetLocalPositionZ(num11 + z2 + z3 + z4);
				dictDstBoneInfo[86].trfBone.SetLocalRotation(x8, 0f, 0f);
				dictDstBoneInfo[86].trfBone.SetLocalScale(num9 * x5 * x7 * x9 / dictSrcBoneInfo[207].vctScl.x, num10 * y3, num9 * x5 * x7);
			}
		}
	}
}
