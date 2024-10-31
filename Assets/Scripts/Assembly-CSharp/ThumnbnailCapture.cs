using System;
using System.Collections.Generic;
using System.IO;
using Character;
using UnityEngine;

public class ThumnbnailCapture : MonoBehaviour
{
	public enum TYPE
	{
		NONE = -1,
		HAIR_SET = 0,
		HAIR_BACK = 1,
		HAIR_SIDE = 2,
		HAIR_FRONT = 3,
		HAIR_MALE = 4,
		WEAR_TOP = 5,
		WEAR_BOTTOM = 6,
		WEAR_BRA = 7,
		WEAR_SHORTS = 8,
		WEAR_SWIM = 9,
		WEAR_SWIM_TOP = 10,
		WEAR_SWIM_BOTTOM = 11,
		WEAR_GLOVE = 12,
		WEAR_PANST = 13,
		WEAR_SOCKS = 14,
		WEAR_SHOES = 15,
		WEAR_MALE_TOP = 16,
		WEAR_MALE_SHOES = 17,
		FACE_SKIN_F = 18,
		FACE_DETAIL_F = 19,
		MOLE = 20,
		EYESHADOE = 21,
		CHEEK = 22,
		LIP = 23,
		FACE_TATTO = 24,
		FACE_SKIN_M = 25,
		FACE_DETAIL_M = 26,
		FACE_TATTO_M = 27,
		BEARD = 28,
		EYEBROW_F = 29,
		EYELASH_F = 30,
		EYE_F = 31,
		EYEBROW_M = 32,
		EYE_M = 33,
		BODY_SKIN_F = 34,
		BODY_SKIN_M = 35,
		UNDER_HAIR = 36,
		BODY_TATTO_F = 37,
		BODY_TATTO_M = 38,
		BODY_SUNBURN = 39,
		NUM = 40
	}

	private string[] folderNames = new string[40]
	{
		"Thumbnail_HairSet", "Thumbnail_HairBack", "Thumbnail_HairSide", "Thumbnail_HairFront", "Thumbnail_HairMale", "Thumbnail_WearTop", "Thumbnail_WearBot", "Thumbnail_WearBra", "Thumbnail_WearShorts", "Thumbnail_WearSwim",
		"Thumbnail_WearSwimTop", "Thumbnail_WearSwimBot", "Thumbnail_WearGlove", "Thumbnail_WearPanst", "Thumbnail_WearSocks", "Thumbnail_WearShoes", "Thumbnail_WearMaleTop", "Thumbnail_WearMaleShoes", "Thumnbs_FaceSkin", "Thumnbs_FaceDetail_F",
		"Thumnbs_Mole", "Thumnbs_EyeShadow", "Thumnbs_Cheek", "Thumnbs_Lip", "Thumnbs_FaceTattoo", "Thumnbs_FaceSkin_M", "Thumnbs_FaceDetail_M", "Thumnbs_FaceTattoo_M", "Thumnbs_Beard", "Thumnbs_eyebrow",
		"Thumnbs_eyelash", "Thumnbs_eye", "Thumnbs_eyebrow_m", "Thumnbs_eye_m", "thumnbs_bodyskin", "thumnbs_bodyskin_m", "thumnbs_underhair", "thumnbs_bodytattoo_f", "thumnbs_bodytattoo_m", "thumnbs_sunburn"
	};

	private Human human;

	private bool capturing;

	private int step;

	private List<int> keys = new List<int>();

	private string folderName = "Thumnbnail";

	private string dataName = string.Empty;

	public string cardF = "DefkoB.png";

	public string cardM = "Defo.png";

	public int superSize = 1;

	public TYPE type = TYPE.NONE;

	public Canvas canvas;

	public Camera[] cameras = new Camera[40];

	public KeyCode quickCapKey = KeyCode.P;

	private void Start()
	{
		human = GetComponentInChildren<Human>();
		if (human.sex == SEX.FEMALE)
		{
			if (cardF.Length > 0)
			{
				string file = Application.persistentDataPath + "/UserData/Chara/Female/" + cardF;
				human.Load(file);
			}
			else
			{
				human.Apply();
			}
			human.blink.Hold = true;
			human.body.bustDynamicBone_L.enabled = false;
			human.body.bustDynamicBone_R.enabled = false;
		}
		else
		{
			if (cardM.Length > 0)
			{
				string file2 = Application.persistentDataPath + "/UserData/Chara/Male/" + cardM;
				human.Load(file2);
			}
			else
			{
				human.Apply();
			}
			Male male = human as Male;
			male.SetShowTinWithWear(false);
		}
		human.body.Anime.speed = 1f;
	}

	public void CaptureStart()
	{
		human.blink.Hold = true;
		capturing = true;
		step = 0;
		keys.Clear();
		human.gameObject.SetActive(true);
		if (type == TYPE.HAIR_SET)
		{
			foreach (KeyValuePair<int, BackHairData> item in CustomDataManager.Hair_b)
			{
				if (item.Value.isSet)
				{
					keys.Add(item.Key);
				}
			}
		}
		else if (type == TYPE.HAIR_BACK)
		{
			foreach (KeyValuePair<int, BackHairData> item2 in CustomDataManager.Hair_b)
			{
				if (!item2.Value.isSet)
				{
					keys.Add(item2.Key);
				}
			}
		}
		else if (type == TYPE.HAIR_SIDE)
		{
			ToList(CustomDataManager.Hair_s);
		}
		else if (type == TYPE.HAIR_FRONT)
		{
			ToList(CustomDataManager.Hair_f);
		}
		else if (type == TYPE.HAIR_MALE)
		{
			ToList(CustomDataManager.Hair_Male);
			Male male = human as Male;
			male.ChangeMaleShow(MALE_SHOW.NUDE);
		}
		else if (type >= TYPE.WEAR_TOP && type <= TYPE.WEAR_SHOES)
		{
			WEAR_TYPE wEAR_TYPE = (WEAR_TYPE)(type - 5);
			ToList(CustomDataManager.GetWearDictionary_Female(wEAR_TYPE));
		}
		else if (type == TYPE.WEAR_MALE_TOP)
		{
			ToList(CustomDataManager.GetWearDictionary_Male(WEAR_TYPE.TOP));
			Male male2 = human as Male;
			male2.ChangeMaleShow(MALE_SHOW.CLOTHING);
		}
		else if (type == TYPE.WEAR_MALE_SHOES)
		{
			ToList(CustomDataManager.GetWearDictionary_Male(WEAR_TYPE.SHOES));
			Male male3 = human as Male;
			male3.ChangeMaleShow(MALE_SHOW.CLOTHING);
			male3.SetWearShoes(true);
		}
		else if (type == TYPE.FACE_SKIN_F)
		{
			ToList(CustomDataManager.FaceSkins_Female);
		}
		else if (type == TYPE.FACE_DETAIL_F)
		{
			ToList(CustomDataManager.FaceDetails_Female);
		}
		else if (type == TYPE.MOLE)
		{
			ToList(CustomDataManager.Mole);
		}
		else if (type == TYPE.EYESHADOE)
		{
			ToList(CustomDataManager.EyeShadow);
		}
		else if (type == TYPE.CHEEK)
		{
			ToList(CustomDataManager.Cheek);
		}
		else if (type == TYPE.LIP)
		{
			ToList(CustomDataManager.Lip);
		}
		else if (type == TYPE.FACE_TATTO)
		{
			ToList(CustomDataManager.FaceTattoo_Female);
		}
		else if (type == TYPE.FACE_SKIN_M)
		{
			ToList(CustomDataManager.FaceSkins_Male);
			((Male)human).ChangeMaleShow(MALE_SHOW.NUDE);
		}
		else if (type == TYPE.FACE_DETAIL_M)
		{
			ToList(CustomDataManager.FaceDetails_Male);
			((Male)human).ChangeMaleShow(MALE_SHOW.NUDE);
		}
		else if (type == TYPE.FACE_TATTO_M)
		{
			ToList(CustomDataManager.FaceTattoo_Male);
			((Male)human).ChangeMaleShow(MALE_SHOW.NUDE);
		}
		else if (type == TYPE.BEARD)
		{
			ToList(CustomDataManager.Beard);
			((Male)human).ChangeMaleShow(MALE_SHOW.NUDE);
		}
		else if (type == TYPE.EYEBROW_F)
		{
			ToList(CustomDataManager.Eyebrow_Female);
		}
		else if (type == TYPE.EYEBROW_M)
		{
			ToList(CustomDataManager.Eyebrow_Male);
		}
		else if (type == TYPE.EYELASH_F)
		{
			ToList(CustomDataManager.Eyelash);
		}
		else if (type == TYPE.EYE_F)
		{
			ToList(CustomDataManager.Eye_Female);
		}
		else if (type == TYPE.EYE_M)
		{
			ToList(CustomDataManager.Eye_Male);
		}
		else if (type == TYPE.BODY_SKIN_F)
		{
			ToList(CustomDataManager.BodySkins_Female);
		}
		else if (type == TYPE.BODY_SKIN_M)
		{
			ToList(CustomDataManager.BodySkins_Male);
			((Male)human).ChangeMaleShow(MALE_SHOW.NUDE);
		}
		else if (type == TYPE.UNDER_HAIR)
		{
			ToList(CustomDataManager.Underhair);
		}
		else if (type == TYPE.BODY_TATTO_F)
		{
			ToList(CustomDataManager.BodyTattoo_Female);
		}
		else if (type == TYPE.BODY_TATTO_M)
		{
			ToList(CustomDataManager.BodyTattoo_Male);
			((Male)human).ChangeMaleShow(MALE_SHOW.NUDE);
		}
		else if (type == TYPE.BODY_SUNBURN)
		{
			ToList(CustomDataManager.Sunburn);
		}
		for (int i = 0; i < cameras.Length; i++)
		{
			if (cameras[i] != null)
			{
				cameras[i].gameObject.SetActive(i == (int)type);
			}
		}
		SetAnime();
		if ((int)type < cameras.Length)
		{
			canvas.worldCamera = cameras[(int)type];
		}
		else
		{
			canvas.worldCamera = null;
		}
		if (canvas.worldCamera == null)
		{
			canvas.worldCamera = Camera.main;
		}
		folderName = folderNames[(int)type];
	}

	public void LoadHuman()
	{
		if (human.sex == SEX.FEMALE)
		{
			if (cardF.Length > 0)
			{
				string file = Application.persistentDataPath + "/UserData/Chara/Female/" + cardF;
				human.Load(file);
			}
			else
			{
				human.Apply();
			}
			human.blink.Hold = true;
			human.body.bustDynamicBone_L.enabled = false;
			human.body.bustDynamicBone_R.enabled = false;
		}
		else
		{
			if (cardM.Length > 0)
			{
				string file2 = Application.persistentDataPath + "/UserData/Chara/Male/" + cardM;
				human.Load(file2);
			}
			else
			{
				human.Apply();
			}
			Male male = human as Male;
			male.SetShowTinWithWear(false);
		}
		human.body.Anime.speed = 1f;
	}

	private void ToList<TValue>(Dictionary<int, TValue> dic)
	{
		foreach (int key in dic.Keys)
		{
			keys.Add(key);
		}
	}

	private void SetAnime()
	{
		if (human.sex == SEX.FEMALE)
		{
			if (type == TYPE.WEAR_GLOVE)
			{
				human.body.Anime.Play("tachi_pose_07");
			}
			else if (type == TYPE.WEAR_SOCKS || type == TYPE.WEAR_SHOES)
			{
				human.body.Anime.Play("tachi_pose_12");
			}
			else if (type == TYPE.WEAR_SHORTS)
			{
				human.body.Anime.Play("tachi_pose_04");
			}
			else if (type >= TYPE.WEAR_TOP && type <= TYPE.WEAR_SHOES)
			{
				human.body.Anime.Play("mannequin");
			}
			else
			{
				human.body.Anime.Play("pose_D");
			}
		}
		else if (type == TYPE.BODY_SKIN_M || type == TYPE.BODY_TATTO_M)
		{
			human.body.Anime.Play("tachi_pose_00");
		}
		else
		{
			human.body.Anime.Play("tachi_pose_01");
		}
		human.body.Anime.speed = 0f;
	}

	private void Update()
	{
		if (!Update_ManualCap())
		{
			Update_AutoCap();
		}
	}

	private bool Update_ManualCap()
	{
		if (type == TYPE.MOLE)
		{
			if (capturing)
			{
				step++;
				if (step < keys.Count)
				{
					human.customParam.head.moleTexID = keys[step];
					human.Apply();
					dataName = CustomDataManager.GetMole(keys[step]).textureName;
				}
				capturing = false;
			}
			if (Input.GetKeyDown(quickCapKey))
			{
				Capture();
				capturing = true;
			}
			return true;
		}
		if (type == TYPE.BODY_TATTO_F)
		{
			if (capturing)
			{
				step++;
				if (step < keys.Count)
				{
					human.customParam.body.tattooID = keys[step];
					human.Apply();
					dataName = CustomDataManager.GetBodyTattoo_Female(keys[step]).textureName;
				}
				capturing = false;
			}
			if (Input.GetKeyDown(quickCapKey))
			{
				Capture();
				capturing = true;
			}
			return true;
		}
		if (type == TYPE.BODY_TATTO_M)
		{
			if (capturing)
			{
				step++;
				if (step < keys.Count)
				{
					human.customParam.body.tattooID = keys[step];
					human.Apply();
					dataName = CustomDataManager.GetBodyTattoo_Male(keys[step]).textureName;
				}
				capturing = false;
			}
			if (Input.GetKeyDown(quickCapKey))
			{
				Capture();
				capturing = true;
			}
			return true;
		}
		return false;
	}

	private void Update_AutoCap()
	{
		if (!capturing)
		{
			return;
		}
		if (step >= keys.Count)
		{
			capturing = false;
			return;
		}
		if (type == TYPE.HAIR_SET)
		{
			human.customParam.hair.parts[0].ID = keys[step];
			dataName = CustomDataManager.GetHair_Back(keys[step]).prefab;
			human.ApplyHair();
			DynamicBoneDisable();
			step++;
		}
		else if (type == TYPE.HAIR_BACK)
		{
			human.customParam.hair.parts[0].ID = keys[step];
			dataName = CustomDataManager.GetHair_Back(keys[step]).prefab;
			human.ApplyHair();
			DynamicBoneDisable();
			step++;
		}
		else if (type == TYPE.HAIR_SIDE)
		{
			human.customParam.hair.parts[2].ID = keys[step];
			dataName = CustomDataManager.GetHair_Side(keys[step]).prefab;
			human.ApplyHair();
			DynamicBoneDisable();
			step++;
		}
		else if (type == TYPE.HAIR_FRONT)
		{
			human.customParam.hair.parts[1].ID = keys[step];
			dataName = CustomDataManager.GetHair_Front(keys[step]).prefab;
			human.ApplyHair();
			DynamicBoneDisable();
			step++;
		}
		else if (type == TYPE.HAIR_MALE)
		{
			human.customParam.hair.parts[0].ID = keys[step];
			dataName = CustomDataManager.GetHair_Male(keys[step]).prefab;
			human.ApplyHair();
			DynamicBoneDisable();
			step++;
		}
		else if (type >= TYPE.WEAR_TOP && type <= TYPE.WEAR_SHOES)
		{
			WEAR_TYPE wEAR_TYPE = (WEAR_TYPE)(type - 5);
			dataName = CustomDataManager.GetWearData(human.sex, wEAR_TYPE, keys[step]).prefab;
			human.customParam.wear.wears[(int)wEAR_TYPE].id = keys[step];
			human.customParam.wear.wears[(int)wEAR_TYPE].color = null;
			human.customParam.wear.isSwimwear = type >= TYPE.WEAR_SWIM && type <= TYPE.WEAR_SWIM_BOTTOM;
			human.wears.WearInstantiate(wEAR_TYPE, human.body.SkinMaterial, human.body.CustomHighlightMat_Skin);
			DynamicBoneDisable();
			step++;
		}
		else if (type == TYPE.WEAR_MALE_TOP)
		{
			WEAR_TYPE wEAR_TYPE2 = WEAR_TYPE.TOP;
			dataName = CustomDataManager.GetWearData(human.sex, wEAR_TYPE2, keys[step]).prefab;
			human.customParam.wear.wears[(int)wEAR_TYPE2].id = keys[step];
			human.customParam.wear.wears[(int)wEAR_TYPE2].color = null;
			human.customParam.wear.isSwimwear = type >= TYPE.WEAR_SWIM && type <= TYPE.WEAR_SWIM_BOTTOM;
			human.wears.WearInstantiate(wEAR_TYPE2, human.body.SkinMaterial, human.body.CustomHighlightMat_Skin);
			DynamicBoneDisable();
			step++;
		}
		else if (type == TYPE.WEAR_MALE_SHOES)
		{
			WEAR_TYPE wEAR_TYPE3 = WEAR_TYPE.SHOES;
			dataName = CustomDataManager.GetWearData(human.sex, wEAR_TYPE3, keys[step]).prefab;
			human.customParam.wear.wears[(int)wEAR_TYPE3].id = keys[step];
			human.customParam.wear.wears[(int)wEAR_TYPE3].color = null;
			human.customParam.wear.isSwimwear = type >= TYPE.WEAR_SWIM && type <= TYPE.WEAR_SWIM_BOTTOM;
			human.wears.WearInstantiate(wEAR_TYPE3, human.body.SkinMaterial, human.body.CustomHighlightMat_Skin);
			DynamicBoneDisable();
			step++;
		}
		else if (type == TYPE.FACE_SKIN_F)
		{
			dataName = CustomDataManager.GetFaceSkin_Female(keys[step]).prefab;
			human.customParam.head.faceTexID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.FACE_DETAIL_F)
		{
			dataName = CustomDataManager.GetFaceDetail_Female(keys[step]).prefab;
			human.customParam.head.detailID = keys[step];
			human.customParam.head.detailWeight = 1f;
			human.Apply();
			step++;
		}
		else if (type == TYPE.MOLE)
		{
			dataName = CustomDataManager.GetMole(keys[step]).textureName;
			human.customParam.head.moleTexID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.EYESHADOE)
		{
			dataName = CustomDataManager.GetEyeShadow(keys[step]).textureName;
			human.customParam.head.eyeshadowTexID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.CHEEK)
		{
			dataName = CustomDataManager.GetCheek(keys[step]).textureName;
			human.customParam.head.cheekTexID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.LIP)
		{
			dataName = CustomDataManager.GetLip(keys[step]).textureName;
			human.customParam.head.lipTexID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.FACE_TATTO)
		{
			dataName = CustomDataManager.GetFaceTattoo_Female(keys[step]).textureName;
			human.customParam.head.tattooID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.FACE_SKIN_M)
		{
			dataName = CustomDataManager.GetFaceSkin_Male(keys[step]).prefab;
			human.customParam.head.faceTexID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.FACE_DETAIL_M)
		{
			dataName = CustomDataManager.GetFaceDetail_Male(keys[step]).prefab;
			human.customParam.head.detailID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.FACE_TATTO_M)
		{
			dataName = CustomDataManager.GetFaceTattoo_Male(keys[step]).textureName;
			human.customParam.head.tattooID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.BEARD)
		{
			dataName = CustomDataManager.GetBeard(keys[step]).prefab;
			human.customParam.head.beardID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.EYEBROW_F)
		{
			dataName = CustomDataManager.GetEyebrow_Female(keys[step]).prefab;
			human.customParam.head.eyeBrowID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.EYEBROW_M)
		{
			dataName = CustomDataManager.GetEyebrow_Male(keys[step]).prefab;
			human.customParam.head.eyeBrowID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.EYELASH_F)
		{
			dataName = CustomDataManager.GetEyelash(keys[step]).prefab;
			human.customParam.head.eyeLashID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.EYE_F)
		{
			dataName = CustomDataManager.GetEye_Female(keys[step]).prefab;
			human.customParam.head.eyeID_L = keys[step];
			human.customParam.head.eyeID_R = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.EYE_M)
		{
			dataName = CustomDataManager.GetEye_Male(keys[step]).prefab;
			human.customParam.head.eyeID_L = keys[step];
			human.customParam.head.eyeID_R = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.BODY_SKIN_F)
		{
			dataName = CustomDataManager.GetBodySkin_Female(keys[step]).prefab;
			human.customParam.body.bodyID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.BODY_SKIN_M)
		{
			dataName = CustomDataManager.GetBodySkin_Male(keys[step]).prefab;
			human.customParam.body.bodyID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.UNDER_HAIR)
		{
			dataName = CustomDataManager.GetUnderhair(keys[step]).prefab;
			human.customParam.body.underhairID = keys[step];
			human.Apply();
			step++;
		}
		else if (type == TYPE.BODY_SUNBURN)
		{
			dataName = CustomDataManager.GetSunburn(keys[step]).textureName;
			human.customParam.body.sunburnID = keys[step];
			human.Apply();
			step++;
		}
		if (human.blink != null)
		{
			human.blink.Hold = true;
		}
		Capture();
	}

	private void Capture()
	{
		string text = Application.persistentDataPath + "/" + folderName;
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string filename = text + "/" + dataName + ".png";
		Application.CaptureScreenshot(filename, superSize);
	}

	private void DynamicBoneDisable()
	{
		DynamicBone[] componentsInChildren = GetComponentsInChildren<DynamicBone>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
	}
}
