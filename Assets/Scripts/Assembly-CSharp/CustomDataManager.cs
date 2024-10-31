using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Character;
using UnityEngine;

public static class CustomDataManager
{
	private static Dictionary<int, HeadData> heads_Female = new Dictionary<int, HeadData>();

	private static Dictionary<int, HeadData> heads_Male = new Dictionary<int, HeadData>();

	private static Dictionary<int, TextAsset> faceShapes_Female = new Dictionary<int, TextAsset>();

	private static TextAsset faceCategory_Female;

	private static Dictionary<int, TextAsset> faceShapes_Male = new Dictionary<int, TextAsset>();

	private static TextAsset faceCategory_Male;

	private static Dictionary<int, PrefabData> faceSkins_Female = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> faceSkins_Male = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> faceDetails_Female = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> faceDetails_Male = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> eyebrow_Female = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> eyebrow_Male = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> eyelash = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> eye_Female = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> eye_Male = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> eyehighlight = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> beard = new Dictionary<int, PrefabData>();

	private static Dictionary<int, CombineTextureData> faceTattoo_Female = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, CombineTextureData> faceTattoo_Male = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, CombineTextureData> cheek = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, CombineTextureData> eyeShadow = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, CombineTextureData> lip = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, CombineTextureData> mole = new Dictionary<int, CombineTextureData>();

	private static TextAsset bodyShapes_Female;

	private static TextAsset bodyCategory_Female;

	private static TextAsset bodyShapes_Male;

	private static TextAsset bodyCategory_Male;

	private static Dictionary<int, PrefabData> bodySkins_Female = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> bodySkins_Male = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> bodyDetails_Female = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> bodyDetails_Male = new Dictionary<int, PrefabData>();

	private static Dictionary<int, PrefabData> nip = new Dictionary<int, PrefabData>();

	private static Dictionary<int, UnderhairData> underhair = new Dictionary<int, UnderhairData>();

	private static Dictionary<int, CombineTextureData> sunburn = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, CombineTextureData> bodyTattoo_Female = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, CombineTextureData> bodyTattoo_Male = new Dictionary<int, CombineTextureData>();

	private static Dictionary<int, HairData> hair_f = new Dictionary<int, HairData>();

	private static Dictionary<int, HairData> hair_s = new Dictionary<int, HairData>();

	private static Dictionary<int, BackHairData> hair_b = new Dictionary<int, BackHairData>();

	private static Dictionary<int, BackHairData> hair_Male = new Dictionary<int, BackHairData>();

	private static Dictionary<int, WearData>[] wears_Female = new Dictionary<int, WearData>[11];

	private static Dictionary<int, WearData>[] wears_Male = new Dictionary<int, WearData>[11];

	public static readonly int[] defWears_Female = new int[11];

	public static readonly int[] defWears_Male = new int[11]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		4
	};

	public static readonly int[] noWears_Female = new int[11]
	{
		101, 0, 0, 0, 50, 0, 0, 0, 0, 0,
		0
	};

	public static readonly int[] noWears_Male = new int[11]
	{
		-1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0
	};

	private static Dictionary<int, AccessoryData>[] accessories = new Dictionary<int, AccessoryData>[12];

	public static Shader multiplayBlendShader_2;

	public static Shader multiplayBlendShader_3;

	public static Shader multiplayBlendShader_6;

	public static Shader normalBlendShader_2;

	public static Shader normalAddShader_2;

	public static Shader hsvOffset;

	public static Shader skinBlendShader_Body;

	public static Shader skinBlendShader_Face;

	public static Shader skinBlendShader_Male;

	public static readonly int _SpecColor = Shader.PropertyToID("_SpecColor");

	public static readonly int _Metallic = Shader.PropertyToID("_Metallic");

	public static readonly int _Smoothness = Shader.PropertyToID("_Smoothness");

	public static readonly int _Color_3 = Shader.PropertyToID("_Color_3");

	public static readonly int _SpecColor_3 = Shader.PropertyToID("_SpecColor_3");

	public static readonly int _CuticleColor = Shader.PropertyToID("_CuticleColor");

	public static readonly int _CuticleExp = Shader.PropertyToID("_CuticleExp");

	public static readonly int _FrenelColor = Shader.PropertyToID("_FrenelColor");

	public static readonly int _FrenelExp = Shader.PropertyToID("_FrenelExp");

	public static readonly int _Metal = Shader.PropertyToID("_Metal");

	public static readonly int _Roughness = Shader.PropertyToID("_Roughness");

	public static readonly int _Specular = Shader.PropertyToID("_Specular");

	public static readonly int _Gloss = Shader.PropertyToID("_Gloss");

	private static Dictionary<string, string> wearLiquids = new Dictionary<string, string>();

	public static bool HadSetUp { get; private set; }

	public static Dictionary<int, HeadData> Heads_Female
	{
		get
		{
			return heads_Female;
		}
	}

	public static Dictionary<int, HeadData> Heads_Male
	{
		get
		{
			return heads_Male;
		}
	}

	public static TextAsset FaceCategory_Female
	{
		get
		{
			return faceCategory_Female;
		}
	}

	public static TextAsset FaceCategory_Male
	{
		get
		{
			return faceCategory_Male;
		}
	}

	public static Dictionary<int, PrefabData> FaceSkins_Female
	{
		get
		{
			return faceSkins_Female;
		}
	}

	public static Dictionary<int, PrefabData> FaceSkins_Male
	{
		get
		{
			return faceSkins_Male;
		}
	}

	public static Dictionary<int, PrefabData> FaceDetails_Female
	{
		get
		{
			return faceDetails_Female;
		}
	}

	public static Dictionary<int, PrefabData> FaceDetails_Male
	{
		get
		{
			return faceDetails_Male;
		}
	}

	public static Dictionary<int, PrefabData> Eyebrow_Female
	{
		get
		{
			return eyebrow_Female;
		}
	}

	public static Dictionary<int, PrefabData> Eyebrow_Male
	{
		get
		{
			return eyebrow_Male;
		}
	}

	public static Dictionary<int, PrefabData> Eyelash
	{
		get
		{
			return eyelash;
		}
	}

	public static Dictionary<int, PrefabData> Eye_Female
	{
		get
		{
			return eye_Female;
		}
	}

	public static Dictionary<int, PrefabData> Eye_Male
	{
		get
		{
			return eye_Male;
		}
	}

	public static Dictionary<int, PrefabData> Eyehighlight
	{
		get
		{
			return eyehighlight;
		}
	}

	public static Dictionary<int, PrefabData> Beard
	{
		get
		{
			return beard;
		}
	}

	public static Dictionary<int, CombineTextureData> FaceTattoo_Female
	{
		get
		{
			return faceTattoo_Female;
		}
	}

	public static Dictionary<int, CombineTextureData> FaceTattoo_Male
	{
		get
		{
			return faceTattoo_Male;
		}
	}

	public static Dictionary<int, CombineTextureData> Cheek
	{
		get
		{
			return cheek;
		}
	}

	public static Dictionary<int, CombineTextureData> EyeShadow
	{
		get
		{
			return eyeShadow;
		}
	}

	public static Dictionary<int, CombineTextureData> Lip
	{
		get
		{
			return lip;
		}
	}

	public static Dictionary<int, CombineTextureData> Mole
	{
		get
		{
			return mole;
		}
	}

	public static TextAsset BodyShapes_Female
	{
		get
		{
			return bodyShapes_Female;
		}
	}

	public static TextAsset BodyCategory_Female
	{
		get
		{
			return bodyCategory_Female;
		}
	}

	public static TextAsset BodyShapes_Male
	{
		get
		{
			return bodyShapes_Male;
		}
	}

	public static TextAsset BodyCategory_Male
	{
		get
		{
			return bodyCategory_Male;
		}
	}

	public static Dictionary<int, PrefabData> BodySkins_Female
	{
		get
		{
			return bodySkins_Female;
		}
	}

	public static Dictionary<int, PrefabData> BodySkins_Male
	{
		get
		{
			return bodySkins_Male;
		}
	}

	public static Dictionary<int, PrefabData> BodyDetails_Female
	{
		get
		{
			return bodyDetails_Female;
		}
	}

	public static Dictionary<int, PrefabData> BodyDetails_Male
	{
		get
		{
			return bodyDetails_Male;
		}
	}

	public static Dictionary<int, PrefabData> Nip
	{
		get
		{
			return nip;
		}
	}

	public static Dictionary<int, UnderhairData> Underhair
	{
		get
		{
			return underhair;
		}
	}

	public static Dictionary<int, CombineTextureData> Sunburn
	{
		get
		{
			return sunburn;
		}
	}

	public static Dictionary<int, CombineTextureData> BodyTattoo_Female
	{
		get
		{
			return bodyTattoo_Female;
		}
	}

	public static Dictionary<int, CombineTextureData> BodyTattoo_Male
	{
		get
		{
			return bodyTattoo_Male;
		}
	}

	public static Dictionary<int, HairData> Hair_f
	{
		get
		{
			return hair_f;
		}
	}

	public static Dictionary<int, HairData> Hair_s
	{
		get
		{
			return hair_s;
		}
	}

	public static Dictionary<int, BackHairData> Hair_b
	{
		get
		{
			return hair_b;
		}
	}

	public static Dictionary<int, BackHairData> Hair_Male
	{
		get
		{
			return hair_Male;
		}
	}

	public static Dictionary<string, string> WearLiquids
	{
		get
		{
			return wearLiquids;
		}
	}

	public static void Setup()
	{
		if (HadSetUp)
		{
			Debug.LogWarning("セットアップ済み");
			return;
		}
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", "shape_list");
		faceShapes_Female.Add(0, assetBundleController.LoadAsset<TextAsset>("cf_anmShapeHead_01"));
		faceShapes_Female.Add(1, assetBundleController.LoadAsset<TextAsset>("cf_anmShapeHead_02"));
		faceShapes_Female.Add(2, assetBundleController.LoadAsset<TextAsset>("cf_anmShapeHead_01"));
		faceShapes_Female.Add(3, assetBundleController.LoadAsset<TextAsset>("cf_anmShapeHead_01"));
		faceCategory_Female = assetBundleController.LoadAsset<TextAsset>("cf_customhead");
		bodyShapes_Female = assetBundleController.LoadAsset<TextAsset>("cf_anmShapeBody");
		bodyCategory_Female = assetBundleController.LoadAsset<TextAsset>("cf_custombody");
		faceShapes_Male.Add(0, assetBundleController.LoadAsset<TextAsset>("cm_anmShapeHead_01"));
		faceShapes_Male.Add(1, assetBundleController.LoadAsset<TextAsset>("cm_anmShapeHead_01"));
		faceCategory_Male = assetBundleController.LoadAsset<TextAsset>("cm_customhead");
		bodyShapes_Male = assetBundleController.LoadAsset<TextAsset>("cm_anmShapeBody");
		bodyCategory_Male = assetBundleController.LoadAsset<TextAsset>("cm_custombody");
		assetBundleController.Close();
		HeadData[] array = new HeadData[4]
		{
			new HeadData(0, "ヘッドA01", "FemaleHead_A1", 0, false),
			new HeadData(2, "ヘッドA02", "FemaleHead_A2", 1, false),
			new HeadData(3, "ヘッドA03", "FemaleHead_A3", 2, false),
			new HeadData(1, "ヘッドB", "FemaleHead_B", 3, false)
		};
		HeadData[] array2 = array;
		foreach (HeadData headData in array2)
		{
			heads_Female.Add(headData.id, headData);
		}
		heads_Male.Add(0, new HeadData(0, "標準", "MaleHead_A", 0, false));
		heads_Male.Add(1, new HeadData(1, "ほっそり", "MaleHead_B", 1, false));
		CustomDataSetupLoader<PrefabData> customDataSetupLoader = new CustomDataSetupLoader<PrefabData>(CustomDataSetupLoader<PrefabData>.SetupAction_Prefab);
		CustomDataSetupLoader<HairData> customDataSetupLoader2 = new CustomDataSetupLoader<HairData>(CustomDataSetupLoader<HairData>.SetupAction_Hair);
		CustomDataSetupLoader<BackHairData> customDataSetupLoader3 = new CustomDataSetupLoader<BackHairData>(CustomDataSetupLoader<BackHairData>.SetupAction_BackHair);
		CustomDataSetupLoader<CombineTextureData> customDataSetupLoader4 = new CustomDataSetupLoader<CombineTextureData>(CustomDataSetupLoader<CombineTextureData>.SetupAction_CombineTexture);
		CustomDataSetupLoader<UnderhairData> customDataSetupLoader5 = new CustomDataSetupLoader<UnderhairData>(CustomDataSetupLoader<UnderhairData>.SetupAction_UnderHair);
		CustomDataSetupLoader<WearData> customDataSetupLoader6 = new CustomDataSetupLoader<WearData>(CustomDataSetupLoader<WearData>.SetupAction_Wear);
		CustomDataSetupLoader<AccessoryData> customDataSetupLoader7 = new CustomDataSetupLoader<AccessoryData>(CustomDataSetupLoader<AccessoryData>.SetupAction_Accessory);
		customDataSetupLoader.Setup_Search(faceSkins_Female, "custommaterial/cf_m_face_*");
		customDataSetupLoader.Setup_Search(faceSkins_Male, "custommaterial/cm_m_face_*");
		customDataSetupLoader.Setup_Search(faceDetails_Female, "customtexture/cf_t_detail_f_*");
		customDataSetupLoader.Setup_Search(faceDetails_Male, "customtexture/cm_t_detail_f_*");
		customDataSetupLoader.Setup_Search(eyebrow_Female, "custommaterial/cf_m_eyebrow_*");
		customDataSetupLoader.Setup_Search(eyebrow_Male, "custommaterial/cm_m_eyebrow_*");
		customDataSetupLoader.Setup_Search(eyelash, "custommaterial/cf_m_eyelashes_*");
		customDataSetupLoader.Setup_Search(eye_Female, "custommaterial/cf_m_eye_*");
		customDataSetupLoader.Setup_Search(eye_Male, "custommaterial/cm_m_eye_*");
		customDataSetupLoader.Setup_Search(eyehighlight, "custommaterial/cf_m_eyehi_*");
		customDataSetupLoader.Setup_Search(beard, "custommaterial/cm_m_beard_*");
		customDataSetupLoader4.Setup_Search(faceTattoo_Female, "customtexture/cf_t_tattoo_f_*");
		customDataSetupLoader4.Setup_Search(faceTattoo_Male, "customtexture/cm_t_tattoo_f_*");
		customDataSetupLoader4.Setup_Search(cheek, "customtexture/cf_t_cheek_*");
		customDataSetupLoader4.Setup_Search(eyeShadow, "customtexture/cf_t_eyeshadow_*");
		customDataSetupLoader4.Setup_Search(lip, "customtexture/cf_t_lip_*");
		customDataSetupLoader4.Setup_Search(mole, "customtexture/cf_t_mole_*");
		customDataSetupLoader.Setup_Search(bodySkins_Female, "custommaterial/cf_m_body_*");
		customDataSetupLoader.Setup_Search(bodySkins_Male, "custommaterial/cm_m_body_*");
		customDataSetupLoader.Setup_Search(bodyDetails_Female, "customtexture/cm_t_detail_b_*");
		customDataSetupLoader.Setup_Search(bodyDetails_Male, "customtexture/cm_t_detail_b_*");
		customDataSetupLoader.Setup_Search(nip, "custommaterial/cf_m_nip_*");
		customDataSetupLoader5.Setup_Search(underhair, "custommaterial/cf_m_underhair_*");
		customDataSetupLoader4.Setup_Search(sunburn, "customtexture/cf_t_sunburn_*");
		customDataSetupLoader4.Setup_Search(sunburn, "customtexture/cm_t_sunburn_*");
		customDataSetupLoader4.Setup_Search(bodyTattoo_Female, "customtexture/cf_t_tattoo_b_*");
		customDataSetupLoader4.Setup_Search(bodyTattoo_Male, "customtexture/cm_t_tattoo_b_*");
		customDataSetupLoader3.Setup_Search(hair_b, "hair/cf_hair_b*");
		customDataSetupLoader2.Setup_Search(hair_f, "hair/cf_hair_f*");
		customDataSetupLoader2.Setup_Search(hair_s, "hair/cf_hair_s*");
		customDataSetupLoader3.Setup_Search(hair_Male, "hair/cm_hair_*");
		string[] array3 = new string[11]
		{
			"wear/cf_top_*", "wear/cf_bot_*", "wear/cf_bra_*", "wear/cf_shorts_*", "wear/cf_swim_*", "wear/cf_swimtop_*", "wear/cf_swimbot_*", "wear/cf_gloves_*", "wear/cf_panst_*", "wear/cf_socks_*",
			"wear/cf_shoes_*"
		};
		for (int j = 0; j < 11; j++)
		{
			wears_Female[j] = new Dictionary<int, WearData>();
			customDataSetupLoader6.Setup_Search(wears_Female[j], array3[j]);
		}
		string[] array4 = new string[11]
		{
			"wear/cm_body_*",
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			"wear/cm_shoes_*"
		};
		for (int k = 0; k < 11; k++)
		{
			wears_Male[k] = new Dictionary<int, WearData>();
			if (array4[k].Length != 0)
			{
				customDataSetupLoader6.Setup_Search(wears_Male[k], array4[k]);
			}
		}
		string[] array5 = new string[12]
		{
			"accessory/ca_head_*", "accessory/ca_ear_*", "accessory/ca_megane_*", "accessory/ca_face_*", "accessory/ca_neck_*", "accessory/ca_shoulder_*", "accessory/ca_breast_*", "accessory/ca_waist_*", "accessory/ca_back_*", "accessory/ca_arm_*",
			"accessory/ca_hand_*", "accessory/ca_leg_*"
		};
		for (int l = 0; l < 12; l++)
		{
			accessories[l] = new Dictionary<int, AccessoryData>();
			customDataSetupLoader7.Setup_Search(accessories[l], array5[l]);
		}
		string[] array6 = new string[12]
		{
			"ca_head_ph00", "ca_ear_ph00", "ca_megane_ph00", "ca_face_ph00", "ca_neck_ph00", "ca_shoulder_ph00", "ca_breast_ph00", "ca_waist_ph00", "ca_back_ph00", "ca_arm_ph00",
			"ca_hand_ph00", "ca_leg_ph00"
		};
		for (int m = 0; m < 12; m++)
		{
			customDataSetupLoader7.Setup_Virtual(accessories[m], array6[m]);
		}
		Setup_SearchLiquidWear(wearLiquids, "wearliquid/cf_liquid*");
		LoadIsNewData();
		HadSetUp = true;
	}

	public static T_Data GetData_NoDataIsZeroData<T_Data>(Dictionary<int, T_Data> datas, int id) where T_Data : class
	{
		if (datas.Count == 0)
		{
			Debug.LogError("データなし");
			return (T_Data)null;
		}
		if (datas.ContainsKey(id))
		{
			return datas[id];
		}
		return datas.Values.First();
	}

	public static T_Data GetData_NoDataIsDefoData<T_Data>(Dictionary<int, T_Data> datas, int id, int def) where T_Data : class
	{
		if (datas.Count == 0)
		{
			Debug.LogError("データなし");
			return (T_Data)null;
		}
		if (datas.ContainsKey(id))
		{
			return datas[id];
		}
		if (datas.ContainsKey(def))
		{
			Debug.Log("デフォデータ使用");
			return datas[def];
		}
		Debug.LogError("デフォデータすらない");
		return datas.Values.First();
	}

	public static T_Data GetData_NoDataIsNull<T_Data>(Dictionary<int, T_Data> datas, int id) where T_Data : class
	{
		if (datas.ContainsKey(id))
		{
			return datas[id];
		}
		return (T_Data)null;
	}

	public static bool HasData<T_Data>(Dictionary<int, T_Data> datas, int id) where T_Data : class
	{
		return datas.ContainsKey(id);
	}

	public static HeadData GetHead_Female(int id)
	{
		return GetData_NoDataIsZeroData(heads_Female, id);
	}

	public static HeadData GetHead_Male(int id)
	{
		return GetData_NoDataIsZeroData(heads_Male, id);
	}

	public static TextAsset GetFaceShapes_Female(int id)
	{
		return GetData_NoDataIsNull(faceShapes_Female, id);
	}

	public static TextAsset GetFaceShapes_Male(int id)
	{
		return GetData_NoDataIsNull(faceShapes_Male, id);
	}

	public static HairData GetHair_Female(HAIR_TYPE part, int id)
	{
		switch (part)
		{
		case HAIR_TYPE.FRONT:
			return GetHair_Front(id);
		case HAIR_TYPE.SIDE:
			return GetHair_Side(id);
		case HAIR_TYPE.BACK:
			return GetHair_Back(id);
		default:
			return null;
		}
	}

	public static HairData GetHair_Front(int id)
	{
		return GetData_NoDataIsNull(hair_f, id);
	}

	public static HairData GetHair_Side(int id)
	{
		return GetData_NoDataIsNull(hair_s, id);
	}

	public static BackHairData GetHair_Back(int id)
	{
		return GetData_NoDataIsNull(hair_b, id);
	}

	public static BackHairData GetHair_Male(int id)
	{
		return GetData_NoDataIsNull(hair_Male, id);
	}

	public static PrefabData GetBodySkin_Female(int id)
	{
		return GetData_NoDataIsZeroData(bodySkins_Female, id);
	}

	public static PrefabData GetBodySkin_Male(int id)
	{
		return GetData_NoDataIsZeroData(bodySkins_Male, id);
	}

	public static PrefabData GetBodyDetail_Female(int id)
	{
		return GetData_NoDataIsZeroData(bodyDetails_Female, id);
	}

	public static PrefabData GetBodyDetail_Male(int id)
	{
		return GetData_NoDataIsZeroData(bodyDetails_Male, id);
	}

	public static PrefabData GetFaceSkin_Female(int id)
	{
		return GetData_NoDataIsZeroData(faceSkins_Female, id);
	}

	public static PrefabData GetFaceSkin_Male(int id)
	{
		return GetData_NoDataIsZeroData(faceSkins_Male, id);
	}

	public static PrefabData GetFaceDetail_Female(int id)
	{
		return GetData_NoDataIsZeroData(faceDetails_Female, id);
	}

	public static PrefabData GetFaceDetail_Male(int id)
	{
		return GetData_NoDataIsZeroData(faceDetails_Male, id);
	}

	public static PrefabData GetEyebrow_Female(int id)
	{
		return GetData_NoDataIsZeroData(eyebrow_Female, id);
	}

	public static PrefabData GetEyebrow_Male(int id)
	{
		return GetData_NoDataIsZeroData(eyebrow_Male, id);
	}

	public static PrefabData GetEyelash(int id)
	{
		return GetData_NoDataIsZeroData(eyelash, id);
	}

	public static PrefabData GetEye_Female(int id)
	{
		return GetData_NoDataIsZeroData(eye_Female, id);
	}

	public static PrefabData GetEye_Male(int id)
	{
		return GetData_NoDataIsZeroData(eye_Male, id);
	}

	public static PrefabData GetEyeHighlight(int id)
	{
		return GetData_NoDataIsZeroData(eyehighlight, id);
	}

	public static PrefabData GetBeard(int id)
	{
		return GetData_NoDataIsNull(beard, id);
	}

	public static PrefabData GetNip(int id)
	{
		return GetData_NoDataIsZeroData(nip, id);
	}

	public static UnderhairData GetUnderhair(int id)
	{
		return GetData_NoDataIsNull(underhair, id);
	}

	public static CombineTextureData GetFaceTattoo_Female(int id)
	{
		return GetData_NoDataIsNull(faceTattoo_Female, id);
	}

	public static CombineTextureData GetFaceTattoo_Male(int id)
	{
		return GetData_NoDataIsNull(faceTattoo_Male, id);
	}

	public static CombineTextureData GetCheek(int id)
	{
		return GetData_NoDataIsNull(cheek, id);
	}

	public static CombineTextureData GetEyeShadow(int id)
	{
		return GetData_NoDataIsNull(eyeShadow, id);
	}

	public static CombineTextureData GetLip(int id)
	{
		return GetData_NoDataIsNull(lip, id);
	}

	public static CombineTextureData GetMole(int id)
	{
		return GetData_NoDataIsNull(mole, id);
	}

	public static CombineTextureData GetSunburn(int id)
	{
		return GetData_NoDataIsNull(sunburn, id);
	}

	public static CombineTextureData GetBodyTattoo_Female(int id)
	{
		return GetData_NoDataIsNull(bodyTattoo_Female, id);
	}

	public static CombineTextureData GetBodyTattoo_Male(int id)
	{
		return GetData_NoDataIsNull(bodyTattoo_Male, id);
	}

	public static Dictionary<int, WearData> GetWearDictionary_Female(WEAR_TYPE type)
	{
		return wears_Female[(int)type];
	}

	public static Dictionary<int, WearData> GetWearDictionary_Male(WEAR_TYPE type)
	{
		return wears_Male[(int)type];
	}

	public static WearData GetWearData(SEX sex, WEAR_TYPE type, int id)
	{
		return (sex != 0) ? GetWearData_Male(type, id) : GetWearData_Female(type, id);
	}

	public static WearData GetWearData_Female(WEAR_TYPE type, int id)
	{
		if (wears_Female[(int)type] != null)
		{
			return GetData_NoDataIsZeroData(wears_Female[(int)type], id);
		}
		return null;
	}

	public static WearData GetWearData_Male(WEAR_TYPE type, int id)
	{
		if (wears_Male[(int)type] != null)
		{
			if (type == WEAR_TYPE.TOP)
			{
				return GetData_NoDataIsDefoData(wears_Male[(int)type], id, defWears_Male[(int)type]);
			}
			return GetData_NoDataIsNull(wears_Male[(int)type], id);
		}
		return null;
	}

	public static bool HasWearData(SEX sex, WEAR_TYPE type, int id)
	{
		return (sex != 0) ? HasWearData_Male(type, id) : HasWearData_Female(type, id);
	}

	private static bool HasWearData_Female(WEAR_TYPE type, int id)
	{
		if (wears_Female[(int)type] != null)
		{
			return HasData(wears_Female[(int)type], id);
		}
		return false;
	}

	private static bool HasWearData_Male(WEAR_TYPE type, int id)
	{
		if (wears_Female[(int)type] != null)
		{
			return HasData(wears_Male[(int)type], id);
		}
		return false;
	}

	public static int GetWearFirstData(SEX sex, WEAR_TYPE type, int id)
	{
		return (sex != 0) ? GetWearFirstData_Male(type, id) : GetWearFirstData_Female(type, id);
	}

	private static int GetWearFirstData_Female(WEAR_TYPE type, int id)
	{
		if (wears_Female[(int)type] != null && wears_Female[(int)type].Count > 0)
		{
			return wears_Female[(int)type].Values.First().id;
		}
		return -1;
	}

	private static int GetWearFirstData_Male(WEAR_TYPE type, int id)
	{
		if (wears_Male[(int)type] != null && wears_Male[(int)type].Count > 0)
		{
			return wears_Male[(int)type].Values.First().id;
		}
		return -1;
	}

	public static Dictionary<int, AccessoryData> GetAccessoryDictionary(ACCESSORY_TYPE type)
	{
		if (type != ACCESSORY_TYPE.NONE)
		{
			return accessories[(int)type];
		}
		return null;
	}

	public static AccessoryData GetAcceData(ACCESSORY_TYPE type, int id)
	{
		if (type != ACCESSORY_TYPE.NONE && accessories[(int)type] != null)
		{
			return GetData_NoDataIsNull(accessories[(int)type], id);
		}
		return null;
	}

	public static bool HasAcceData(ACCESSORY_TYPE type, int id)
	{
		if (type != ACCESSORY_TYPE.NONE && accessories[(int)type] != null)
		{
			return HasData(accessories[(int)type], id);
		}
		return false;
	}

	public static void Setup_SearchLiquidWear(Dictionary<string, string> datas, string search)
	{
		string text = string.Empty;
		int num = search.LastIndexOf("/");
		if (num != -1)
		{
			text = search.Substring(0, num);
			search = search.Remove(0, num + 1);
		}
		//string path = GlobalData.assetBundlePath + "/" + text;
        string path = Application.persistentDataPath + "/abdata" + "/" + text;
        string[] files = Directory.GetFiles(path, search, SearchOption.TopDirectoryOnly);
		string[] array = files;
		foreach (string path2 in array)
		{
			string extension = Path.GetExtension(path2);
			if (extension.Length == 0)
			{
				string text2 = Path.GetFileNameWithoutExtension(path2);
				if (text.Length > 0)
				{
					text2 = text + "/" + text2;
				}
                //AssetBundle assetBundle = AssetBundle.LoadFromFile(GlobalData.assetBundlePath + "/" + text2);
                AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/abdata" + "/" + text2);
                string[] allAssetNames = assetBundle.GetAllAssetNames();
				assetBundle.Unload(true);
				string[] array2 = allAssetNames;
				foreach (string path3 in array2)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path3);
					datas.Add(fileNameWithoutExtension, text2);
				}
			}
		}
	}

	public static void SaveIsNewData()
	{
        //string path = Application.dataPath + "/../UserData/Save/CheckedData";
        string path = Application.persistentDataPath + "/UserData/Save/CheckedData";
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				SaveIsNewData(binaryWriter);
				binaryWriter.Close();
			}
			fileStream.Close();
		}
	}

	private static void SaveIsNewData(BinaryWriter writer)
	{
		SaveIsNewData(writer, heads_Female);
		SaveIsNewData(writer, heads_Male);
		SaveIsNewData(writer, faceSkins_Female);
		SaveIsNewData(writer, faceSkins_Male);
		SaveIsNewData(writer, faceDetails_Female);
		SaveIsNewData(writer, faceDetails_Male);
		SaveIsNewData(writer, eyebrow_Female);
		SaveIsNewData(writer, eyebrow_Male);
		SaveIsNewData(writer, eyelash);
		SaveIsNewData(writer, eye_Female);
		SaveIsNewData(writer, eye_Male);
		SaveIsNewData(writer, eyehighlight);
		SaveIsNewData(writer, beard);
		SaveIsNewData(writer, faceTattoo_Female);
		SaveIsNewData(writer, faceTattoo_Male);
		SaveIsNewData(writer, cheek);
		SaveIsNewData(writer, eyeShadow);
		SaveIsNewData(writer, lip);
		SaveIsNewData(writer, mole);
		SaveIsNewData(writer, bodySkins_Female);
		SaveIsNewData(writer, bodySkins_Male);
		SaveIsNewData(writer, bodyDetails_Female);
		SaveIsNewData(writer, bodyDetails_Male);
		SaveIsNewData(writer, nip);
		SaveIsNewData(writer, underhair);
		SaveIsNewData(writer, sunburn);
		SaveIsNewData(writer, bodyTattoo_Female);
		SaveIsNewData(writer, bodyTattoo_Male);
		SaveIsNewData(writer, hair_f);
		SaveIsNewData(writer, hair_s);
		SaveIsNewData(writer, hair_b);
		SaveIsNewData(writer, hair_Male);
		for (int i = 0; i < wears_Female.Length; i++)
		{
			SaveIsNewData(writer, wears_Female[i]);
		}
		for (int j = 0; j < wears_Male.Length; j++)
		{
			SaveIsNewData(writer, wears_Male[j]);
		}
	}

	private static void SaveIsNewData<T>(BinaryWriter writer, Dictionary<int, T> dic) where T : ItemDataBase
	{
		List<int> list = new List<int>();
		foreach (T value in dic.Values)
		{
			if (!value.isNew)
			{
				list.Add(value.id);
			}
		}
		writer.Write(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			writer.Write(list[i]);
		}
	}

	public static void LoadIsNewData()
	{
        //string path = Application.dataPath + "/../UserData/Save/CheckedData";
        string path = Application.persistentDataPath + "/UserData/Save/CheckedData";
        if (!File.Exists(path))
		{
			return;
		}
		using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			using (BinaryReader binaryReader = new BinaryReader(fileStream))
			{
				LoadIsNewData(binaryReader);
				binaryReader.Close();
			}
			fileStream.Close();
		}
	}

	private static void LoadIsNewData(BinaryReader reader)
	{
		LoadIsNewData(reader, heads_Female);
		LoadIsNewData(reader, heads_Male);
		LoadIsNewData(reader, faceSkins_Female);
		LoadIsNewData(reader, faceSkins_Male);
		LoadIsNewData(reader, faceDetails_Female);
		LoadIsNewData(reader, faceDetails_Male);
		LoadIsNewData(reader, eyebrow_Female);
		LoadIsNewData(reader, eyebrow_Male);
		LoadIsNewData(reader, eyelash);
		LoadIsNewData(reader, eye_Female);
		LoadIsNewData(reader, eye_Male);
		LoadIsNewData(reader, eyehighlight);
		LoadIsNewData(reader, beard);
		LoadIsNewData(reader, faceTattoo_Female);
		LoadIsNewData(reader, faceTattoo_Male);
		LoadIsNewData(reader, cheek);
		LoadIsNewData(reader, eyeShadow);
		LoadIsNewData(reader, lip);
		LoadIsNewData(reader, mole);
		LoadIsNewData(reader, bodySkins_Female);
		LoadIsNewData(reader, bodySkins_Male);
		LoadIsNewData(reader, bodyDetails_Female);
		LoadIsNewData(reader, bodyDetails_Male);
		LoadIsNewData(reader, nip);
		LoadIsNewData(reader, underhair);
		LoadIsNewData(reader, sunburn);
		LoadIsNewData(reader, bodyTattoo_Female);
		LoadIsNewData(reader, bodyTattoo_Male);
		LoadIsNewData(reader, hair_f);
		LoadIsNewData(reader, hair_s);
		LoadIsNewData(reader, hair_b);
		LoadIsNewData(reader, hair_Male);
		for (int i = 0; i < wears_Female.Length; i++)
		{
			LoadIsNewData(reader, wears_Female[i]);
		}
		for (int j = 0; j < wears_Male.Length; j++)
		{
			LoadIsNewData(reader, wears_Male[j]);
		}
	}

	private static void LoadIsNewData<T>(BinaryReader reader, Dictionary<int, T> dic) where T : ItemDataBase
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			int key = reader.ReadInt32();
			if (dic.ContainsKey(key))
			{
				dic[key].isNew = false;
			}
			else
			{
				Debug.LogWarning("以前持ってたデータが消えた？");
			}
		}
	}
}
