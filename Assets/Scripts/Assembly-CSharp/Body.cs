using System;
using Character;
using UnityEngine;

public class Body : CharaShapeCustomBase
{
	private const int UNDERHAIR_SUB_NUM = 4;

	private Renderer rend_nipL;

	private Renderer rend_nipR;

	private Renderer rend_underhair;

	private Renderer rend_nail;

	private int nailMaterialNo;

	private int manicureMaterialNo;

	private Renderer rend_tongue;

	private GameObject[] obj_underhair_sub = new GameObject[4];

	private bool showTongue = true;

	private Texture nipBaseTex;

	private RenderTexture nipRendTex;

	private Texture nailBaseTex;

	private RenderTexture nailRendTex;

	private Texture sunburnTex;

	private Texture tattooTex;

	private static readonly string name_nipL = "cf_O_tikubiL_00";

	private static readonly string name_nipR = "cf_O_tikubiR_00";

	private static readonly string[] name_underhair_sub = new string[4] { "N_mnpk_bara00_00", "N_mnpk_bara01_00", "N_mnpk_bara02_00", "N_mnpk_bara03_00" };

	private static readonly string name_underhair_F = "cf_O_mnpk";

	private static readonly string name_underhair_M = "O_mnpk";

	private static readonly string name_skin_F = "cf_O_body_00";

	private static readonly string name_skin_M = "O_body";

	private const string name_skinMaterial_F = "cf_m_body";

	private const string name_skinMaterial_M = "cm_m_body";

	private static readonly string name_baseBoneRoot_F = "p_cf_anim/cf_J_Root";

	private static readonly string name_baseBoneRoot_M = "p_cm_anim/cm_J_Root";

	private static readonly string name_bodyMeshRoot_F = "cf_body_00/cf_N_O_root";

	private static readonly string name_mnpbMeshRoot_F = "cf_body_mnpb/cf_N_O_root";

	private static readonly string name_bodyMeshRoot_M = "cm_body_00/cm_N_O_root";

	private static readonly string name_mnpbMeshRoot_M = "cm_body_mnpb/cm_N_O_root";

	private static readonly string name_nail_F = "cf_O_nail";

	private static readonly string name_tongue_F = "cf_O_tang";

	private static readonly string name_tongue_M = "cm_O_tang";

	public DynamicBone_Ver02 bustDynamicBone_L { get; private set; }

	public DynamicBone_Ver02 bustDynamicBone_R { get; private set; }

	public Transform AnimatedBoneRoot { get; private set; }

	public BustSoft bustSoft { get; private set; }

	public BustGravity bustWeight { get; private set; }

	private Material NailMaterial
	{
		get
		{
			return rend_nail.materials[nailMaterialNo];
		}
	}

	private Material ManicureMaterial
	{
		get
		{
			return rend_nail.materials[manicureMaterialNo];
		}
	}

	public Body(Human human, DynamicBone_Ver02 bustDynamicBone_L, DynamicBone_Ver02 bustDynamicBone_R)
		: base(human)
	{
		base.human = human;
		if (human.sex == SEX.FEMALE)
		{
			info = new ShapeBodyInfoFemale();
			bustSoft = new BustSoft(human);
			bustWeight = new BustGravity(human);
		}
		else if (human.sex == SEX.MALE)
		{
			info = new ShapeBodyInfoMale();
			bustSoft = null;
			bustWeight = null;
		}
		this.bustDynamicBone_L = bustDynamicBone_L;
		this.bustDynamicBone_R = bustDynamicBone_R;
		string name = ((human.sex != 0) ? name_baseBoneRoot_M : name_baseBoneRoot_F);
		string name2 = ((human.sex != 0) ? name_bodyMeshRoot_M : name_bodyMeshRoot_F);
		string name3 = ((human.sex != 0) ? name_mnpbMeshRoot_M : name_mnpbMeshRoot_F);
		AnimatedBoneRoot = human.transform.Find(name);
		Transform transform = human.transform.Find(name2);
		Transform transform2 = human.transform.Find(name3);
		AttachBoneWeight.Attach(AnimatedBoneRoot.gameObject, transform.gameObject, true);
		AttachBoneWeight.Attach(AnimatedBoneRoot.gameObject, transform2.gameObject, true);
	}

	public void Load(GameObject obj, BodyParameter bodyParam)
	{
		base.obj = obj;
		anime = obj.GetComponentInChildren<Animator>();
		if (human.sex == SEX.FEMALE)
		{
			rend_nipL = Transform_Utility.FindTransform(obj.transform, name_nipL).GetComponent<Renderer>();
			rend_nipR = Transform_Utility.FindTransform(obj.transform, name_nipR).GetComponent<Renderer>();
			rend_underhair = Transform_Utility.FindTransform(obj.transform, name_underhair_F).GetComponent<Renderer>();
			rend_skin = Transform_Utility.FindTransform(obj.transform, name_skin_F).GetComponent<Renderer>();
			rend_nail = Transform_Utility.FindTransform(obj.transform, name_nail_F).GetComponent<Renderer>();
			for (int i = 0; i < rend_nail.materials.Length; i++)
			{
				if (rend_nail.materials[i].name.IndexOf("cf_M_nail_iro") == 0)
				{
					manicureMaterialNo = i;
					break;
				}
			}
			if (rend_nail.materials.Length == 2)
			{
				nailMaterialNo = (manicureMaterialNo + 1) % 2;
			}
			for (int j = 0; j < 4; j++)
			{
				obj_underhair_sub[j] = Transform_Utility.FindTransform(obj.transform, name_underhair_sub[j]).gameObject;
			}
			info.InitShapeInfo(CustomDataManager.BodyShapes_Female, CustomDataManager.BodyCategory_Female, obj.transform);
			base.Setup_MaterialAndTexture(rend_skin, "cf_m_body");
			if (nailRendTex != null)
			{
                UnityEngine.Object.Destroy(nailRendTex);
			}
			if (nailBaseTex == null)
			{
				nailBaseTex = NailMaterial.mainTexture;
			}
            //int width = NailMaterial.mainTexture.width;
            //int height = NailMaterial.mainTexture.height;
            //nailRendTex = new RenderTexture(width, height, 0);
            //nailRendTex.wrapMode = nailBaseTex.wrapMode;
            //nailRendTex.useMipMap = true;
        }
        else if (human.sex == SEX.MALE)
		{
			rend_skin = Transform_Utility.FindTransform(obj.transform, name_skin_M).GetComponent<Renderer>();
			rend_underhair = Transform_Utility.FindTransform(obj.transform, name_underhair_M).GetComponent<Renderer>();
			info.InitShapeInfo(CustomDataManager.BodyShapes_Male, CustomDataManager.BodyCategory_Male, obj.transform);
			Setup_MaterialAndTexture(rend_skin, "cm_m_body");
		}
		string name = ((human.sex != 0) ? name_tongue_M : name_tongue_F);
		rend_tongue = Transform_Utility.FindTransform(obj.transform, name).GetComponent<Renderer>();
		SetShowTongue(showTongue);
		Load(bodyParam);
	}

	public void Load(BodyParameter bodyParam)
	{
		ShapeApply();
		ChangeNip();
		ChangeNail();
		ChangeManicure();
		ChangeUnderHair();
		LoadSkin();
		UpdateCustomHighlightMaterial();
	}

	public override void ShapeApply()
	{
		human.BodyShapeInfoApply();
		if (bustDynamicBone_L != null)
		{
			bustDynamicBone_L.ResetPosition();
		}
		if (bustDynamicBone_R != null)
		{
			bustDynamicBone_R.ResetPosition();
		}
		if (bustSoft != null)
		{
			bustSoft.ReCalc();
		}
		if (bustWeight != null)
		{
			bustWeight.ReCalc();
		}
		base.IsShapeChange = false;
		human.OnShapeApplied();
	}

	public override void SetShape(int category, float value)
	{
		human.customParam.body.shapeVals[category] = value;
		base.IsShapeChange = true;
	}

	public override float GetShape(int category)
	{
		return human.customParam.body.shapeVals[category];
	}

	public void ChangeSunburn()
	{
		CombineTextureData sunburn = CustomDataManager.GetSunburn(human.customParam.body.sunburnID);
		sunburnTex = ((sunburn != null) ? AssetBundleLoader.LoadAsset<Texture2D>(sunburn.assetbundleDir, sunburn.assetbundleName, sunburn.textureName) : null);
		if (sunburn != null)
		{
			sunburn.isNew = false;
		}
	}

	public void ChangeTattoo()
	{
		int tattooID = human.customParam.body.tattooID;
		CombineTextureData combineTextureData = ((human.sex != 0) ? CustomDataManager.GetBodyTattoo_Male(tattooID) : CustomDataManager.GetBodyTattoo_Female(tattooID));
		tattooTex = ((combineTextureData != null) ? AssetBundleLoader.LoadAsset<Texture2D>(combineTextureData.assetbundleDir, combineTextureData.assetbundleName, combineTextureData.textureName) : null);
		if (combineTextureData != null)
		{
			combineTextureData.isNew = false;
		}
	}

	public void ChangeUnderHair()
	{
		if (human.sex == SEX.FEMALE)
		{
			BodyParameter body = human.customParam.body;
			UnderhairData underhair = CustomDataManager.GetUnderhair(body.underhairID);
			AssetBundleController assetBundleController = AssetBundleController.New_OpenFromFile(GlobalData.assetBundlePath, underhair.assetbundleName);
			if (underhair != null)
			{
				underhair.isNew = false;
			}
			if (underhair.prefab != "-")
			{
				rend_underhair.enabled = true;
				rend_underhair.material = assetBundleController.LoadAsset<Material>(underhair.prefab);
			}
			else
			{
				rend_underhair.enabled = false;
			}
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject = obj_underhair_sub[i];
				if (underhair.sub == i)
				{
					gameObject.SetActive(true);
					string assetName = underhair.prefab.Remove(underhair.prefab.Length - 1) + "1";
					Material material = assetBundleController.LoadAsset<Material>(assetName);
					Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
					Renderer[] array = componentsInChildren;
					foreach (Renderer renderer in array)
					{
						renderer.material = material;
					}
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
			assetBundleController.Close();
		}
		ChangeUnderHairColor();
	}

	public void ShowUnderHair(bool show)
	{
		rend_underhair.enabled = show;
	}

	public void ShowUnderHair3D(bool show)
	{
		if (human.sex == SEX.MALE)
		{
			return;
		}
		BodyParameter body = human.customParam.body;
		UnderhairData underhair = CustomDataManager.GetUnderhair(body.underhairID);
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject = obj_underhair_sub[i];
			if (underhair.sub == i)
			{
				gameObject.SetActive(show);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void ChangeUnderHairColor()
	{
		BodyParameter body = human.customParam.body;
		body.underhairColor.SetToMaterial(rend_underhair.material);
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject = obj_underhair_sub[i];
			if (gameObject != null && gameObject.activeSelf)
			{
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
				Renderer[] array = componentsInChildren;
				foreach (Renderer renderer in array)
				{
					body.underhairColor.SetToMaterial(renderer.material);
				}
			}
		}
	}

	public void ChangeNip()
	{
		if (human.sex == SEX.FEMALE)
		{
			BodyParameter body = human.customParam.body;
			PrefabData nip = CustomDataManager.GetNip(body.nipID);
			if (nip != null)
			{
				nip.isNew = false;
			}
			Material material = AssetBundleLoader.LoadAsset<Material>(nip.assetbundleDir, nip.assetbundleName, nip.prefab);
			rend_nipL.material = material;
			rend_nipR.material = material;
			rend_nipL.material.renderQueue = 2451;
			rend_nipR.material.renderQueue = 2451;
			nipBaseTex = material.mainTexture;
			CreateNipTex();
			ChangeNipColor();
		}
	}

	private void CreateNipTex()
	{
		Material material = rend_nipL.material;
		if (nipRendTex != null)
		{
            UnityEngine.Object.Destroy(nipRendTex);
		}
		int width = material.mainTexture.width;
		int height = material.mainTexture.height;
		nipRendTex = new RenderTexture(width, height, 0);
		nipRendTex.wrapMode = material.mainTexture.wrapMode;
		nipRendTex.useMipMap = true;
	}

	public void ChangeNipColor()
	{
		BodyParameter body = human.customParam.body;
		body.nipColor.SetToMaterial(rend_nipL.material);
		body.nipColor.SetToMaterial(rend_nipR.material);
		ChangeNipTexture();
	}

	private void ChangeNipTexture()
	{
		BodyParameter body = human.customParam.body;
		Material material = new Material(CustomDataManager.hsvOffset);
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = true;
		Graphics.SetRenderTarget(nipRendTex);
		GL.Clear(false, true, Color.white);
		Graphics.SetRenderTarget(null);
		material.SetTexture("_MainTex", nipBaseTex);
		material.SetFloat("_OffsetH", body.nipColor.offset_h);
		material.SetFloat("_OffsetS", body.nipColor.offset_s);
		material.SetFloat("_OffsetV", body.nipColor.offset_v);
		Graphics.Blit(nipBaseTex, nipRendTex, material, 0);
		GL.sRGBWrite = sRGBWrite;
		rend_nipL.material.mainTexture = nipRendTex;
		rend_nipR.material.mainTexture = nipRendTex;
        UnityEngine.Object.Destroy(material);
    }

	public void ChangeNail()
	{
		if (human.sex == SEX.FEMALE)
		{
			BodyParameter body = human.customParam.body;
			ChangeNailTex();
			body.nailColor.SetToMaterial(NailMaterial);
		}
	}

	private void ChangeNailTex()
	{
		BodyParameter body = human.customParam.body;
		Material nailMaterial = NailMaterial;
		Material material = new Material(CustomDataManager.hsvOffset);
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = true;
		Graphics.SetRenderTarget(nailRendTex);
		GL.Clear(false, true, Color.white);
		Graphics.SetRenderTarget(null);
		material.SetTexture("_MainTex", nailBaseTex);
		material.SetFloat("_OffsetH", body.nailColor.offset_h);
		material.SetFloat("_OffsetS", body.nailColor.offset_s);
		material.SetFloat("_OffsetV", body.nailColor.offset_v);
		Graphics.Blit(nailBaseTex, nailRendTex, material, 0);
		GL.sRGBWrite = sRGBWrite;
		nailMaterial.mainTexture = nailRendTex;
        UnityEngine.Object.Destroy(material);
    }

	public void ChangeManicure()
	{
		if (human.sex == SEX.FEMALE)
		{
			BodyParameter body = human.customParam.body;
			Material manicureMaterial = ManicureMaterial;
			ColorParameter_PBR1 manicureColor = body.manicureColor;
			manicureMaterial.SetColor("_Color", manicureColor.mainColor1);
			manicureMaterial.SetFloat("_Metallic", manicureColor.specular1);
			manicureMaterial.SetFloat("_Glossiness", manicureColor.smooth1);
			body.manicureColor.SetToMaterial(ManicureMaterial);
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
		BodyParameter body = human.customParam.body;
		CombineTextureData bodyTattoo_Female = CustomDataManager.GetBodyTattoo_Female(body.tattooID);
		float value = body.sunburnColor_A;
		Color tattooColor = body.tattooColor;
		Material material = new Material(CustomDataManager.skinBlendShader_Body);
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = true;
		Graphics.SetRenderTarget(skinTex);
		GL.Clear(false, true, Color.white);
		Graphics.SetRenderTarget(null);
		if (sunburnTex == null)
		{
			value = 0f;
		}
		if (tattooTex == null)
		{
			tattooColor.a = 0f;
		}
		material.SetTexture("_BaseTex", skinBaseTex);
		material.SetFloat("_OffsetH", body.skinColor.offset_h);
		material.SetFloat("_OffsetS", body.skinColor.offset_s);
		material.SetFloat("_OffsetV", body.skinColor.offset_v);
		material.SetTexture("_SunburnTex", sunburnTex);
		material.SetFloat("_SunburnH", body.sunburnColor_H);
		material.SetFloat("_SunburnS", body.sunburnColor_S);
		material.SetFloat("_SunburnV", body.sunburnColor_V);
		material.SetFloat("_SunburnA", value);
		material.SetTexture("_TattooTex", tattooTex);
		if (tattooTex != null)
		{
			SetTattooOffsetAndTiling(material, "_TattooTex", 1024, 1024, tattooTex.width, tattooTex.height, bodyTattoo_Female.pos.x, bodyTattoo_Female.pos.y);
		}
		material.SetColor("_TattooColor", tattooColor);
		Graphics.Blit(skinBaseTex, skinTex, material, 0);
		GL.sRGBWrite = sRGBWrite;
		skinMaterial.mainTexture = skinTex;
		ChangeBumpRate();
        UnityEngine.Object.Destroy(material);
    }

	public void RendSkinTexture_Male()
	{
		BodyParameter body = human.customParam.body;
		CombineTextureData bodyTattoo_Male = CustomDataManager.GetBodyTattoo_Male(body.tattooID);
		Color tattooColor = body.tattooColor;
		Material material = new Material(CustomDataManager.skinBlendShader_Male);
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = true;
		Graphics.SetRenderTarget(skinTex);
		GL.Clear(false, true, Color.white);
		Graphics.SetRenderTarget(null);
		if (tattooTex == null)
		{
			tattooColor.a = 0f;
		}
		material.SetTexture("_BaseTex", skinBaseTex);
		material.SetFloat("_OffsetH", body.skinColor.offset_h);
		material.SetFloat("_OffsetS", body.skinColor.offset_s);
		material.SetFloat("_OffsetV", body.skinColor.offset_v);
		material.SetTexture("_TattooTex", tattooTex);
		if (tattooTex != null)
		{
			SetTattooOffsetAndTiling(material, "_TattooTex", 1024, 1024, tattooTex.width, tattooTex.height, bodyTattoo_Male.pos.x, bodyTattoo_Male.pos.y);
		}
		material.SetColor("_TattooColor", tattooColor);
		Graphics.Blit(skinMaterial.mainTexture, skinTex, material, 0);
		GL.sRGBWrite = sRGBWrite;
		skinMaterial.mainTexture = skinTex;
		ChangeBumpRate();
        UnityEngine.Object.Destroy(material);
    }

	public void ChangeBumpRate()
	{
		skinMaterial.SetFloat("_BumpScale", human.customParam.body.detailWeight);
	}

	public void ChangeSkinMaterial()
	{
		BodyParameter body = human.customParam.body;
		PrefabData prefabData = null;
		if (human.sex == SEX.FEMALE)
		{
			prefabData = CustomDataManager.GetBodySkin_Female(body.bodyID);
		}
		else if (human.sex == SEX.MALE)
		{
			prefabData = CustomDataManager.GetBodySkin_Male(body.bodyID);
		}
		if (prefabData != null)
		{
			prefabData.isNew = false;
		}
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(prefabData.assetbundleDir, prefabData.assetbundleName);
		Material material = assetBundleController.LoadAsset<Material>(prefabData.prefab);
		skinBaseTex = material.mainTexture;
		assetBundleController.Close();
		skinMaterial.CopyPropertiesFromMaterial(material);
		human.UpdateSkinMaterial();
		human.wears.ChangeBodyMaterial(base.Rend_skin);
		Resources.UnloadUnusedAssets();
	}

	public void LoadSkin()
	{
		ChangeSkinMaterial();
		ChangeSunburn();
		ChangeTattoo();
		RendSkinTexture();
		ChangeBumpRate();
	}

	public override void CreateCustomHighlightMaterial()
	{
		CreateCustomHighlightMaterial(ref customHighlightMat_Skin, rend_skin);
		human.wears.ChangeBodyMaterial(rend_skin);
	}

	public override void DeleteCustomHighlightMaterial()
	{
		DeleteCustomHighlightMaterial(ref customHighlightMat_Skin, rend_skin);
		human.wears.ChangeBodyMaterial(rend_skin);
	}

	public void SetShowTongue(bool show)
	{
		showTongue = show;
		if (rend_tongue != null)
		{
			rend_tongue.enabled = show;
		}
	}

	public void ShowNip(bool show)
	{
		if (rend_nipL != null)
		{
			rend_nipL.enabled = show;
		}
		if (rend_nipR != null)
		{
			rend_nipR.enabled = show;
		}
	}
}
