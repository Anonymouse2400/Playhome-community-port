using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Utility;

public class Head : CharaShapeCustomBase
{
	protected Transform parent;

	protected Renderer rend_eyebrow;

	protected Renderer rend_eyelash;

	protected Renderer rend_eye_L;

	protected Renderer rend_eye_R;

	protected Renderer rend_eyeHighlight_L;

	protected Renderer rend_eyeHighlight_R;

	protected Renderer rend_beard;

	protected Renderer rend_tongue;

	protected List<Renderer> rends_other = new List<Renderer>();

	private static readonly string name_skin_Female = "cf_O_head";

	private static readonly string name_eyebrow_Female = "cf_O_mayuge";

	private static readonly string name_eyelash_Female = "cf_O_matuge";

	private static readonly string name_eye_L_Female = "cf_O_eyewhite_L";

	private static readonly string name_eye_R_Female = "cf_O_eyewhite_R";

	private static readonly string name_eyeHighlight_L_Female = "cf_O_eyehikari_L";

	private static readonly string name_eyeHighlight_R_Female = "cf_O_eyehikari_R";

	private static readonly string name_faceMaterial_Female = "cf_m_face";

	private static readonly string name_tongue_Female = "cf_O_sita";

	private static readonly string name_skin_Male = "cm_O_head";

	private static readonly string name_eyebrow_Male = "cm_O_mayuge";

	private static readonly string name_eye_L_Male = "cm_O_eye_L";

	private static readonly string name_eye_R_Male = "cm_O_eye_R";

	private static readonly string name_eyeHighlight_L_Male = "cm_O_eyeHi_L";

	private static readonly string name_eyeHighlight_R_Male = "cm_O_eyeHi_R";

	private static readonly string name_tongue_Male = "cm_O_sita";

	private static readonly string name_faceMaterial_Male = "cm_M_head";

	private static readonly string name_beard = "O_hige00";

	protected Material customHighlightMat_Eye_L;

	protected Material customHighlightMat_Eye_R;

	protected Material customHighlightMat_Eyebrow;

	protected bool showTongue = true;

	private Texture2D tattooTex;

	private Texture2D eyeshadowTex;

	private Texture2D cheekTex;

	private Texture2D lipTex;

	private Texture2D moleTex;

	public Transform Parent
	{
		get
		{
			return parent;
		}
	}

	public Renderer Rend_eyebrow
	{
		get
		{
			return rend_eyebrow;
		}
	}

	public Renderer Rend_eyelash
	{
		get
		{
			return rend_eyelash;
		}
	}

	public Renderer Rend_eye_L
	{
		get
		{
			return rend_eye_L;
		}
	}

	public Renderer Rend_eye_R
	{
		get
		{
			return rend_eye_R;
		}
	}

	public Renderer Rend_eyeHighlight_L
	{
		get
		{
			return rend_eyeHighlight_L;
		}
	}

	public Renderer Rend_eyeHighlight_R
	{
		get
		{
			return rend_eyeHighlight_R;
		}
	}

	public Renderer Rend_beard
	{
		get
		{
			return rend_beard;
		}
	}

	public Renderer Rend_tongue
	{
		get
		{
			return rend_tongue;
		}
	}

	public Material CustomHighlightMat_Eye_L
	{
		get
		{
			return customHighlightMat_Eye_L;
		}
	}

	public Material CustomHighlightMat_Eye_R
	{
		get
		{
			return customHighlightMat_Eye_R;
		}
	}

	public Material CustomHighlightMat_Eyebrow
	{
		get
		{
			return customHighlightMat_Eyebrow;
		}
	}

	public bool MouthReset { get; private set; }

	public Head(Human human, Transform parent)
		: base(human)
	{
		MouthReset = false;
		this.parent = parent;
		if (human.sex == SEX.FEMALE)
		{
			info = new ShapeHeadInfoFemale();
		}
		else if (human.sex == SEX.MALE)
		{
			info = new ShapeHeadInfoMale();
		}
	}

	public void Load(HeadParameter param)
	{
		if (obj != null)
		{
			obj.transform.SetParent(null);
            UnityEngine.Object.Destroy(obj);
		}
		HeadData headData = null;
		if (human.sex == SEX.FEMALE)
		{
			headData = CustomDataManager.GetHead_Female(param.headID);
		}
		else if (human.sex == SEX.MALE)
		{
			headData = CustomDataManager.GetHead_Male(param.headID);
		}
		headData.isNew = false;
		obj = ResourceUtility.CreateInstance<GameObject>(headData.path);
		obj.transform.SetParent(parent, false);
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localPosition = Vector3.zero;
		string text = ((human.sex != 0) ? name_skin_Male : name_skin_Female);
		string text2 = ((human.sex != 0) ? name_eyebrow_Male : name_eyebrow_Female);
		string text3 = ((human.sex != 0) ? name_eye_L_Male : name_eye_L_Female);
		string text4 = ((human.sex != 0) ? name_eye_R_Male : name_eye_R_Female);
		string text5 = ((human.sex != 0) ? name_eyeHighlight_L_Male : name_eyeHighlight_L_Female);
		string text6 = ((human.sex != 0) ? name_eyeHighlight_R_Male : name_eyeHighlight_R_Female);
		string text7 = ((human.sex != 0) ? name_tongue_Male : name_tongue_Female);
		string name_skinMaterial = ((human.sex != 0) ? name_faceMaterial_Male : name_faceMaterial_Female);
		rends_other.Clear();
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name == text)
			{
				rend_skin = componentsInChildren[i];
			}
			else if (componentsInChildren[i].name == text2)
			{
				rend_eyebrow = componentsInChildren[i];
			}
			else if (componentsInChildren[i].name == text3)
			{
				rend_eye_L = componentsInChildren[i];
			}
			else if (componentsInChildren[i].name == text4)
			{
				rend_eye_R = componentsInChildren[i];
			}
			else if (componentsInChildren[i].name == text5)
			{
				rend_eyeHighlight_L = componentsInChildren[i];
			}
			else if (componentsInChildren[i].name == text6)
			{
				rend_eyeHighlight_R = componentsInChildren[i];
			}
			else if (componentsInChildren[i].name == text7)
			{
				rend_tongue = componentsInChildren[i];
			}
			else
			{
				rends_other.Add(componentsInChildren[i]);
			}
		}
		if (human.sex == SEX.FEMALE)
		{
			rend_eyelash = Transform_Utility.FindTransform(obj.transform, name_eyelash_Female).GetComponent<Renderer>();
		}
		else if (human.sex == SEX.MALE)
		{
			rend_beard = Transform_Utility.FindTransform(obj.transform, name_beard).GetComponent<Renderer>();
		}
		SetShowTongue(showTongue);
		anime = obj.GetComponentInChildren<Animator>();
		Setup_MaterialAndTexture(rend_skin, name_skinMaterial);
		if (human.sex == SEX.FEMALE)
		{
			info.InitShapeInfo(CustomDataManager.GetFaceShapes_Female(param.headID), CustomDataManager.FaceCategory_Female, obj.transform);
		}
		else if (human.sex == SEX.MALE)
		{
			info.InitShapeInfo(CustomDataManager.GetFaceShapes_Male(param.headID), CustomDataManager.FaceCategory_Male, obj.transform);
		}
		ShapeApply();
		ChangeEyebrow();
		ChangeEyelash();
		ChangeEye_LR();
		ChangeEyeHighlight();
		ChangeBeard();
		LoadSkin();
		UpdateCustomHighlightMaterial();
	}

	public override void ShapeApply()
	{
		CustomParameter customParam = human.customParam;
		for (int i = 0; i < customParam.head.shapeVals.Length; i++)
		{
			info.ChangeValue(i, customParam.head.shapeVals[i]);
		}
		info.Update();
		base.IsShapeChange = false;
	}

	public override void SetShape(int category, float value)
	{
		human.customParam.head.shapeVals[category] = value;
		base.IsShapeChange = true;
	}

	public override float GetShape(int category)
	{
		return human.customParam.head.shapeVals[category];
	}

	public void ChangeSkinMaterial()
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		HeadParameter head = human.customParam.head;
		PrefabData prefabData = null;
		if (human.sex == SEX.FEMALE)
		{
			prefabData = CustomDataManager.GetFaceSkin_Female(head.faceTexID);
		}
		else if (human.sex == SEX.MALE)
		{
			prefabData = CustomDataManager.GetFaceSkin_Male(head.faceTexID);
		}
		prefabData.isNew = false;
		assetBundleController.OpenFromFile(prefabData.assetbundleDir, prefabData.assetbundleName);
		Material mat = assetBundleController.LoadAsset<Material>(prefabData.prefab);
		skinMaterial.CopyPropertiesFromMaterial(mat);
		assetBundleController.Close();
		skinBaseTex = skinMaterial.mainTexture;
		skinNormalTex = skinMaterial.GetTexture("_BumpMap");
		RendSkinTexture();
		RendNormalTexture();
		human.UpdateSkinMaterial();
		Resources.UnloadUnusedAssets();
	}

	public void LoadSkin()
	{
		ChangeSkinMaterial();
		ChangeTattoo();
		ChangeEyeShadow();
		ChangeCheek();
		ChangeLip();
		ChangeMole();
		RendSkinTexture();
	}

	public void ChangeEyebrow()
	{
		HeadParameter head = human.customParam.head;
		PrefabData prefabData = null;
		if (human.sex == SEX.FEMALE)
		{
			prefabData = CustomDataManager.GetEyebrow_Female(head.eyeBrowID);
		}
		else if (human.sex == SEX.MALE)
		{
			prefabData = CustomDataManager.GetEyebrow_Male(head.eyeBrowID);
		}
		prefabData.isNew = false;
		rend_eyebrow.material = AssetBundleLoader.LoadAsset<Material>(prefabData.assetbundleDir, prefabData.assetbundleName, prefabData.prefab);
		ChangeEyebrowColor();
	}

	public void ChangeEyebrowColor()
	{
		HeadParameter head = human.customParam.head;
		head.eyeBrowColor.SetToMaterial(rend_eyebrow.material);
	}

	public void ChangeEyelash()
	{
		if (human.sex == SEX.FEMALE)
		{
			HeadParameter head = human.customParam.head;
			PrefabData eyelash = CustomDataManager.GetEyelash(head.eyeLashID);
			eyelash.isNew = false;
			rend_eyelash.material = AssetBundleLoader.LoadAsset<Material>(eyelash.assetbundleDir, eyelash.assetbundleName, eyelash.prefab);
			ChangeEyelashColor();
		}
	}

	public void ChangeEyelashColor()
	{
		if (human.sex == SEX.FEMALE)
		{
			HeadParameter head = human.customParam.head;
			head.eyeLashColor.SetToMaterial(rend_eyelash.material);
		}
	}

	public void ChangeEye_L()
	{
		HeadParameter head = human.customParam.head;
		PrefabData prefabData = null;
		if (human.sex == SEX.FEMALE)
		{
			prefabData = CustomDataManager.GetEye_Female(head.eyeID_L);
		}
		else if (human.sex == SEX.MALE)
		{
			prefabData = CustomDataManager.GetEye_Male(head.eyeID_L);
		}
		prefabData.isNew = false;
		rend_eye_L.material = AssetBundleLoader.LoadAsset<Material>(prefabData.assetbundleDir, prefabData.assetbundleName, prefabData.prefab);
		ChangeEyeColor_L();
	}

	public void ChangeEye_R()
	{
		HeadParameter head = human.customParam.head;
		PrefabData prefabData = null;
		if (human.sex == SEX.FEMALE)
		{
			prefabData = CustomDataManager.GetEye_Female(head.eyeID_R);
		}
		else if (human.sex == SEX.MALE)
		{
			prefabData = CustomDataManager.GetEye_Male(head.eyeID_R);
		}
		prefabData.isNew = false;
		rend_eye_R.material = AssetBundleLoader.LoadAsset<Material>(prefabData.assetbundleDir, prefabData.assetbundleName, prefabData.prefab);
		ChangeEyeColor_R();
	}

	public void ChangeEye_LR()
	{
		ChangeEye_L();
		ChangeEye_R();
	}

	public void ChangeEyeColor_L()
	{
		HeadParameter head = human.customParam.head;
		rend_eye_L.material.SetColor("_ScleraColor", head.eyeScleraColorL);
		rend_eye_L.material.SetColor("_IrisColor", head.eyeIrisColorL);
		rend_eye_L.material.SetFloat("_IrisPupilSize", head.eyePupilDilationL);
		float value = CalcIrisScatterIntensity(head.eyeEmissiveL);
		float value2 = CalcIrisScatterPower(head.eyeEmissiveL);
		rend_eye_L.material.SetFloat("_IrisScatterIntensity", value);
		rend_eye_L.material.SetFloat("_IrisScatterPower", value2);
	}

	public void ChangeEyeColor_R()
	{
		HeadParameter head = human.customParam.head;
		rend_eye_R.material.SetColor("_ScleraColor", head.eyeScleraColorR);
		rend_eye_R.material.SetColor("_IrisColor", head.eyeIrisColorR);
		rend_eye_R.material.SetFloat("_IrisPupilSize", head.eyePupilDilationR);
		float value = CalcIrisScatterIntensity(head.eyeEmissiveR);
		float value2 = CalcIrisScatterPower(head.eyeEmissiveR);
		rend_eye_R.material.SetFloat("_IrisScatterIntensity", value);
		rend_eye_R.material.SetFloat("_IrisScatterPower", value2);
	}

	private float CalcIrisScatterIntensity(float emissive)
	{
		if (emissive < 0.5f)
		{
			return Mathf.Lerp(0f, 5.5f, emissive * 2f);
		}
		return Mathf.Lerp(5.5f, 20f, (emissive - 0.5f) * 2f);
	}

	private float CalcIrisScatterPower(float emissive)
	{
		if (emissive < 0.5f)
		{
			return Mathf.Lerp(8f, 4.56f, emissive * 2f);
		}
		return Mathf.Lerp(4.56f, 1.5f, (emissive - 0.5f) * 2f);
	}

	public void ChangeEyeColor_LR()
	{
		ChangeEyeColor_L();
		ChangeEyeColor_R();
	}

	public void ChangeEyeHighlight()
	{
		if (human.sex == SEX.FEMALE)
		{
			HeadParameter head = human.customParam.head;
			PrefabData eyeHighlight = CustomDataManager.GetEyeHighlight(head.eyeHighlightTexID);
			eyeHighlight.isNew = false;
			Material material = AssetBundleLoader.LoadAsset<Material>(eyeHighlight.assetbundleDir, eyeHighlight.assetbundleName, eyeHighlight.prefab);
			rend_eyeHighlight_L.material = material;
			rend_eyeHighlight_R.material = material;
		}
		ChangeEyeHighlightColor();
	}

	public void ChangeEyeHighlightColor()
	{
		HeadParameter head = human.customParam.head;
		head.eyeHighlightColor.SetToMaterial(rend_eyeHighlight_L.material);
		head.eyeHighlightColor.SetToMaterial(rend_eyeHighlight_R.material);
	}

	public bool IsGlossEyeHighlight()
	{
		return ColorParameter_EyeHighlight.CheckGloss(rend_eyeHighlight_L.material);
	}

	public void ChangeBeard()
	{
		if (human.sex != SEX.MALE)
		{
			return;
		}
		HeadParameter head = human.customParam.head;
		PrefabData beard = CustomDataManager.GetBeard(head.beardID);
		beard.isNew = false;
		Material material = null;
		if (beard.prefab != "-")
		{
			material = AssetBundleLoader.LoadAsset<Material>(beard.assetbundleDir, beard.assetbundleName, beard.prefab);
			if (material != null)
			{
				rend_beard.material = material;
			}
		}
		rend_beard.gameObject.SetActive(material != null);
		ChangeBeardColor();
	}

	public void ChangeBeardColor()
	{
		if (human.sex == SEX.MALE)
		{
			HeadParameter head = human.customParam.head;
			rend_beard.material.color = head.beardColor;
		}
	}

	public void RendSkinTexture()
	{
		if (human.sex == SEX.FEMALE)
		{
			RendSkinTexture_Female();
		}
		else if (human.sex == SEX.MALE)
		{
			RendSkinTexture_Male();
		}
	}

	public void RendSkinTexture_Female()
	{
		HeadParameter head = human.customParam.head;
		CombineTextureData faceTattoo_Female = CustomDataManager.GetFaceTattoo_Female(head.tattooID);
		CombineTextureData eyeShadow = CustomDataManager.GetEyeShadow(head.eyeshadowTexID);
		CombineTextureData cheek = CustomDataManager.GetCheek(head.cheekTexID);
		CombineTextureData lip = CustomDataManager.GetLip(head.lipTexID);
		CombineTextureData mole = CustomDataManager.GetMole(head.moleTexID);
		Color white = Color.white;
		Color tattooColor = head.tattooColor;
		Color eyeshadowColor = head.eyeshadowColor;
		Color cheekColor = head.cheekColor;
		Color lipColor = head.lipColor;
		Color moleColor = head.moleColor;
		Material material = new Material(CustomDataManager.skinBlendShader_Face);
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = true;
		Graphics.SetRenderTarget(skinTex);
		GL.Clear(false, true, Color.black);
		Graphics.SetRenderTarget(null);
		if (tattooTex == null)
		{
			tattooColor.a = 0f;
		}
		if (eyeshadowTex == null)
		{
			eyeshadowColor.a = 0f;
		}
		if (cheekTex == null)
		{
			cheekColor.a = 0f;
		}
		if (lipTex == null)
		{
			lipColor.a = 0f;
		}
		if (moleTex == null)
		{
			moleColor.a = 0f;
		}
		material.SetTexture("_BaseTex", skinBaseTex);
		material.SetFloat("_OffsetH", human.customParam.body.skinColor.offset_h);
		material.SetFloat("_OffsetS", human.customParam.body.skinColor.offset_s);
		material.SetFloat("_OffsetV", human.customParam.body.skinColor.offset_v);
		material.SetTexture("_CheekTex", cheekTex);
		if (cheekTex != null)
		{
			SetTattooOffsetAndTiling(material, "_CheekTex", 1024, 1024, cheekTex.width, cheekTex.height, cheek.pos.x, cheek.pos.y);
		}
		material.SetColor("_CheekColor", cheekColor);
		material.SetTexture("_EyeShadowTex", eyeshadowTex);
		if (eyeshadowTex != null)
		{
			SetTattooOffsetAndTiling(material, "_EyeShadowTex", 1024, 1024, eyeshadowTex.width, eyeshadowTex.height, eyeShadow.pos.x, eyeShadow.pos.y);
		}
		material.SetColor("_EyeShadowColor", eyeshadowColor);
		material.SetTexture("_LipTex", lipTex);
		if (lipTex != null)
		{
			SetTattooOffsetAndTiling(material, "_LipTex", 1024, 1024, lipTex.width, lipTex.height, lip.pos.x, lip.pos.y);
		}
		material.SetColor("_LipColor", lipColor);
		material.SetTexture("_MoleTex", moleTex);
		if (moleTex != null)
		{
			SetTattooOffsetAndTiling(material, "_MoleTex", 1024, 1024, moleTex.width, moleTex.height, mole.pos.x, mole.pos.y);
		}
		material.SetColor("_MoleColor", moleColor);
		material.SetTexture("_TattooTex", tattooTex);
		if (tattooTex != null)
		{
			SetTattooOffsetAndTiling(material, "_TattooTex", 1024, 1024, tattooTex.width, tattooTex.height, faceTattoo_Female.pos.x, faceTattoo_Female.pos.y);
		}
		material.SetColor("_TattooColor", tattooColor);
		Graphics.Blit(skinBaseTex, skinTex, material, 0);
		GL.sRGBWrite = sRGBWrite;
		skinMaterial.mainTexture = skinTex;
		if (faceTattoo_Female != null)
		{
			faceTattoo_Female.isNew = false;
		}
		if (eyeShadow != null)
		{
			eyeShadow.isNew = false;
		}
		if (cheek != null)
		{
			cheek.isNew = false;
		}
		if (lip != null)
		{
			lip.isNew = false;
		}
		if (mole != null)
		{
			mole.isNew = false;
		}
        UnityEngine.Object.Destroy(material);
	}

	public void RendSkinTexture_Male()
	{
		HeadParameter head = human.customParam.head;
		CombineTextureData faceTattoo_Male = CustomDataManager.GetFaceTattoo_Male(head.tattooID);
		Texture2D texture2D = ((faceTattoo_Male != null) ? AssetBundleLoader.LoadAsset<Texture2D>(faceTattoo_Male.assetbundleDir, faceTattoo_Male.assetbundleName, faceTattoo_Male.textureName) : null);
		Color tattooColor = head.tattooColor;
		Material material = new Material(CustomDataManager.skinBlendShader_Male);
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = true;
		Graphics.SetRenderTarget(skinTex);
		GL.Clear(false, true, Color.black);
		Graphics.SetRenderTarget(null);
		if (texture2D == null)
		{
			tattooColor.a = 0f;
		}
		material.SetTexture("_BaseTex", skinBaseTex);
		material.SetFloat("_OffsetH", human.customParam.body.skinColor.offset_h);
		material.SetFloat("_OffsetS", human.customParam.body.skinColor.offset_s);
		material.SetFloat("_OffsetV", human.customParam.body.skinColor.offset_v);
		material.SetTexture("_TattooTex", texture2D);
		if (texture2D != null)
		{
			SetTattooOffsetAndTiling(material, "_TattooTex", 1024, 1024, texture2D.width, texture2D.height, faceTattoo_Male.pos.x, faceTattoo_Male.pos.y);
		}
		material.SetColor("_TattooColor", tattooColor);
		Graphics.Blit(skinBaseTex, skinTex, material, 0);
		GL.sRGBWrite = sRGBWrite;
		skinMaterial.mainTexture = skinTex;
		if (faceTattoo_Male != null)
		{
			faceTattoo_Male.isNew = false;
		}
        UnityEngine.Object.Destroy(material);
	}

	public void RendNormalTexture()
	{
		HeadParameter head = human.customParam.head;
		Material material = new Material(CustomDataManager.normalAddShader_2);
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = true;
		Graphics.SetRenderTarget(normalTex);
		GL.Clear(false, true, Color.white);
		Graphics.SetRenderTarget(null);
		PrefabData prefabData = ((human.sex != 0) ? CustomDataManager.GetFaceDetail_Male(head.detailID) : CustomDataManager.GetFaceDetail_Female(head.detailID));
		Texture2D texture2D = ((prefabData != null) ? AssetBundleLoader.LoadAsset<Texture2D>(prefabData.assetbundleDir, prefabData.assetbundleName, prefabData.prefab) : null);
		if (texture2D != null)
		{
			material.SetTexture("_BlendTexA", skinNormalTex);
			material.SetTexture("_BlendTexB", texture2D);
			material.SetFloat("_Rate", head.detailWeight);
			Graphics.Blit(skinNormalTex, normalTex, material, 0);
			skinMaterial.SetTexture("_BumpMap", normalTex);
		}
		else
		{
			skinMaterial.SetTexture("_BumpMap", skinNormalTex);
		}
		GL.sRGBWrite = sRGBWrite;
        UnityEngine.Object.Destroy(material);
		if (prefabData != null)
		{
			prefabData.isNew = false;
		}
	}

	public override void CreateCustomHighlightMaterial()
	{
		CreateCustomHighlightMaterial(ref customHighlightMat_Skin, rend_skin);
		CreateCustomHighlightMaterial(ref customHighlightMat_Eye_L, rend_eye_L);
		CreateCustomHighlightMaterial(ref customHighlightMat_Eye_R, rend_eye_R);
		CreateCustomHighlightMaterial(ref customHighlightMat_Eyebrow, rend_eyebrow);
	}

	public override void DeleteCustomHighlightMaterial()
	{
		DeleteCustomHighlightMaterial(ref customHighlightMat_Skin, rend_skin);
		DeleteCustomHighlightMaterial(ref customHighlightMat_Eye_L, rend_eye_L);
		DeleteCustomHighlightMaterial(ref customHighlightMat_Eye_R, rend_eye_R);
		DeleteCustomHighlightMaterial(ref customHighlightMat_Eyebrow, rend_eyebrow);
	}

	public void ResetMouth(bool reset)
	{
		MouthReset = reset;
		if (human.sex == SEX.FEMALE)
		{
			int[] array = new int[23]
			{
				3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
				13, 14, 15, 16, 17, 18, 55, 56, 57, 58,
				59, 60, 61
			};
			foreach (int num in array)
			{
				float value = ((!reset) ? human.customParam.head.shapeVals[num] : 0.5f);
				info.ChangeValue(num, value);
			}
			info.Update();
		}
		else
		{
			int[] array2 = new int[19]
			{
				3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
				13, 14, 55, 56, 57, 58, 59, 60, 61
			};
			foreach (int num2 in array2)
			{
				float value2 = ((!reset) ? human.customParam.head.shapeVals[num2] : 0.5f);
				info.ChangeValue(num2, value2);
			}
			info.Update();
		}
	}

	public void SetShowTongue(bool show)
	{
		showTongue = show;
		if (rend_tongue != null)
		{
			rend_tongue.enabled = show;
		}
	}

	public void ChangeShow(bool show)
	{
		rend_skin.enabled = show;
		rend_eyebrow.enabled = show;
		rend_eye_L.enabled = show;
		rend_eye_R.enabled = show;
		rend_eyeHighlight_L.enabled = show;
		rend_eyeHighlight_R.enabled = show;
		if (rend_eyelash != null)
		{
			rend_eyelash.enabled = show;
		}
		if (rend_beard != null)
		{
			rend_beard.enabled = show;
		}
		for (int i = 0; i < rends_other.Count; i++)
		{
			rends_other[i].enabled = show;
		}
	}

	public void ChangeTattoo()
	{
		int tattooID = human.customParam.head.tattooID;
		CombineTextureData combineTextureData = ((human.sex != 0) ? CustomDataManager.GetFaceTattoo_Male(tattooID) : CustomDataManager.GetFaceTattoo_Female(tattooID));
		tattooTex = ((combineTextureData != null) ? AssetBundleLoader.LoadAsset<Texture2D>(combineTextureData.assetbundleDir, combineTextureData.assetbundleName, combineTextureData.textureName) : null);
	}

	public void ChangeEyeShadow()
	{
		CombineTextureData eyeShadow = CustomDataManager.GetEyeShadow(human.customParam.head.eyeshadowTexID);
		eyeshadowTex = ((eyeShadow != null) ? AssetBundleLoader.LoadAsset<Texture2D>(eyeShadow.assetbundleDir, eyeShadow.assetbundleName, eyeShadow.textureName) : null);
	}

	public void ChangeCheek()
	{
		CombineTextureData cheek = CustomDataManager.GetCheek(human.customParam.head.cheekTexID);
		cheekTex = ((cheek != null) ? AssetBundleLoader.LoadAsset<Texture2D>(cheek.assetbundleDir, cheek.assetbundleName, cheek.textureName) : null);
	}

	public void ChangeLip()
	{
		CombineTextureData lip = CustomDataManager.GetLip(human.customParam.head.lipTexID);
		lipTex = ((lip != null) ? AssetBundleLoader.LoadAsset<Texture2D>(lip.assetbundleDir, lip.assetbundleName, lip.textureName) : null);
	}

	public void ChangeMole()
	{
		CombineTextureData mole = CustomDataManager.GetMole(human.customParam.head.moleTexID);
		moleTex = ((mole != null) ? AssetBundleLoader.LoadAsset<Texture2D>(mole.assetbundleDir, mole.assetbundleName, mole.textureName) : null);
	}
}
