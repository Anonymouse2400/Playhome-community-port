using System;
using Character;
using UnityEngine;
using Utility;

// Token: 0x02000112 RID: 274
public abstract class CharaShapeCustomBase
{
    // Token: 0x060005DD RID: 1501 RVA: 0x00024C68 File Offset: 0x00023068
    public CharaShapeCustomBase(Human human)
    {
        this.human = human;
        this.customHighlightMaterialPath = ((human.sex != SEX.FEMALE) ? CharaShapeCustomBase.customHighlightMaterialPath_M : CharaShapeCustomBase.customHighlightMaterialPath_F);
        this.customHighlightMaterialName = ((human.sex != SEX.FEMALE) ? CharaShapeCustomBase.customHighlightMaterialName_M : CharaShapeCustomBase.customHighlightMaterialName_F);
    }

    // Token: 0x170000A5 RID: 165
    // (get) Token: 0x060005DE RID: 1502 RVA: 0x00024CC2 File Offset: 0x000230C2
    // (set) Token: 0x060005DF RID: 1503 RVA: 0x00024CCA File Offset: 0x000230CA
    public string customHighlightMaterialPath { get; protected set; }

    // Token: 0x170000A6 RID: 166
    // (get) Token: 0x060005E0 RID: 1504 RVA: 0x00024CD3 File Offset: 0x000230D3
    // (set) Token: 0x060005E1 RID: 1505 RVA: 0x00024CDB File Offset: 0x000230DB
    public string customHighlightMaterialName { get; protected set; }

    // Token: 0x170000A7 RID: 167
    // (get) Token: 0x060005E2 RID: 1506 RVA: 0x00024CE4 File Offset: 0x000230E4
    public GameObject Obj
    {
        get
        {
            return this.obj;
        }
    }

    // Token: 0x170000A8 RID: 168
    // (get) Token: 0x060005E3 RID: 1507 RVA: 0x00024CEC File Offset: 0x000230EC
    public ShapeInfoBase Info
    {
        get
        {
            return this.info;
        }
    }

    // Token: 0x170000A9 RID: 169
    // (get) Token: 0x060005E4 RID: 1508 RVA: 0x00024CF4 File Offset: 0x000230F4
    // (set) Token: 0x060005E5 RID: 1509 RVA: 0x00024CFC File Offset: 0x000230FC
    public bool IsShapeChange { get; protected set; }

    // Token: 0x170000AA RID: 170
    // (get) Token: 0x060005E6 RID: 1510 RVA: 0x00024D05 File Offset: 0x00023105
    public Material SkinMaterial
    {
        get
        {
            return this.skinMaterial;
        }
    }

    // Token: 0x170000AB RID: 171
    // (get) Token: 0x060005E7 RID: 1511 RVA: 0x00024D0D File Offset: 0x0002310D
    public Renderer Rend_skin
    {
        get
        {
            return this.rend_skin;
        }
    }

    // Token: 0x170000AC RID: 172
    // (get) Token: 0x060005E8 RID: 1512 RVA: 0x00024D15 File Offset: 0x00023115
    public Material CustomHighlightMat_Skin
    {
        get
        {
            return this.customHighlightMat_Skin;
        }
    }

    // Token: 0x170000AD RID: 173
    // (get) Token: 0x060005E9 RID: 1513 RVA: 0x00024D1D File Offset: 0x0002311D
    public Animator Anime
    {
        get
        {
            return this.anime;
        }
    }

    // Token: 0x060005EA RID: 1514 RVA: 0x00024D25 File Offset: 0x00023125
    public void Update()
    {
        if (this.IsShapeChange)
        {
            this.ShapeApply();
        }
    }

    // Token: 0x060005EB RID: 1515
    public abstract void ShapeApply();

    // Token: 0x060005EC RID: 1516
    public abstract void SetShape(int category, float value);

    // Token: 0x060005ED RID: 1517
    public abstract float GetShape(int category);

    // Token: 0x060005EE RID: 1518 RVA: 0x00024D38 File Offset: 0x00023138
    protected void Setup_MaterialAndTexture(Renderer rend_skin, string name_skinMaterial)
    {
        Setup_Material(rend_skin, name_skinMaterial);
        //Setup_Texture();
    }

    // Token: 0x060005EF RID: 1519 RVA: 0x00024D48 File Offset: 0x00023148
    private void Setup_Material(Renderer rend_skin, string name_skinMaterial)
    {
        int num = -1;
        for (int i = 0; i < rend_skin.sharedMaterials.Length; i++)
        {
            if (rend_skin.sharedMaterials[i].name.ToLower().IndexOf(name_skinMaterial.ToLower()) == 0)
            {
                num = i;
                this.skinMaterial = new Material(rend_skin.sharedMaterials[i]);
                this.skinMaterial.name = name_skinMaterial + "_CustomMaterial";
                break;
            }
        }
        if (num != -1)
        {
            this.rend_skin = rend_skin;
            this.skinMaterialNo = num;
            MaterialUtility.SwapSharedMaterials(rend_skin, num, this.skinMaterial);
        }
    }

    // Token: 0x060005F0 RID: 1520 RVA: 0x00024DE4 File Offset: 0x000231E4
    // private void Setup_Texture()
    // {
    //int width = skinMaterial.mainTexture.width;
    //int height = skinMaterial.mainTexture.height;
    //skinBaseTex = skinMaterial.mainTexture;
    //skinTex = new RenderTexture(width, height, 0);
    //skinTex.wrapMode = skinBaseTex.wrapMode;
    //skinTex.useMipMap = true;
    //Texture texture = skinMaterial.GetTexture("_BumpMap");
    //width = texture.width;
    //height = texture.height;
    //normalTex = new RenderTexture(width, height, 0);
    //normalTex.wrapMode = texture.wrapMode;
    // normalTex.useMipMap = true;
    //}

    // Token: 0x060005F1 RID: 1521 RVA: 0x00024EA0 File Offset: 0x000232A0
    protected void SetTattooOffsetAndTiling(Material mat, string propertyName, int baseW, int baseH, int texW, int texH, float offsetPx, float offsetPy)
    {
        float num = (float)baseW / (float)texW;
        float num2 = (float)baseH / (float)texH;
        float num3 = -(offsetPx / (float)baseW) * num;
        float num4 = -(((float)baseH - offsetPy - (float)texH) / (float)baseH) * num2;
        mat.SetTextureOffset(propertyName, new Vector2(num3, num4));
        mat.SetTextureScale(propertyName, new Vector2(num, num2));
    }

    // Token: 0x060005F2 RID: 1522 RVA: 0x00024EF5 File Offset: 0x000232F5
    public virtual void CreateCustomHighlightMaterial()
    {
        this.CreateCustomHighlightMaterial(ref this.customHighlightMat_Skin, this.rend_skin);
    }

    // Token: 0x060005F3 RID: 1523 RVA: 0x00024F09 File Offset: 0x00023309
    public virtual void DeleteCustomHighlightMaterial()
    {
        this.DeleteCustomHighlightMaterial(ref this.customHighlightMat_Skin, this.rend_skin);
    }

    // Token: 0x060005F4 RID: 1524 RVA: 0x00024F20 File Offset: 0x00023320
    protected void CreateCustomHighlightMaterial(ref Material customHighlightMat, Renderer rend)
    {
        int num = -1;
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            if (rend.sharedMaterials[i].name.IndexOf(this.customHighlightMaterialName) == 0)
            {
                num = i;
            }
        }
        if (num == -1)
        {
            if (customHighlightMat == null)
            {
                Material material = Resources.Load<Material>(customHighlightMaterialPath + customHighlightMaterialName);
                customHighlightMat = new Material(material);
            }
            MaterialUtility.AddSharedMaterials(rend, customHighlightMat);
        }
    }

    // Token: 0x060005F5 RID: 1525 RVA: 0x00024FA4 File Offset: 0x000233A4
    protected void DeleteCustomHighlightMaterial(ref Material customHighlightMat, Renderer rend)
    {
        if (customHighlightMat != null)
        {
            Material[] sharedMaterials = rend.sharedMaterials;
            int num = -1;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i].name.IndexOf(this.customHighlightMaterialName) == 0)
                {
                    num = i;
                }
            }
            if (num != -1)
            {
                Material[] array = new Material[sharedMaterials.Length - 1];
                int num2 = 0;
                for (int j = 0; j < sharedMaterials.Length; j++)
                {
                    if (j != num)
                    {
                        array[num2] = sharedMaterials[j];
                        num2++;
                    }
                }
                rend.sharedMaterials = array;
            }
            UnityEngine.Object.Destroy(customHighlightMat);
            customHighlightMat = null;
        }
    }

    // Token: 0x060005F6 RID: 1526 RVA: 0x00025049 File Offset: 0x00023449
    public virtual void UpdateCustomHighlightMaterial()
    {
        if (this.customHighlightMat_Skin)
        {
            this.CreateCustomHighlightMaterial();
        }
    }

    // Token: 0x060005F7 RID: 1527 RVA: 0x00025061 File Offset: 0x00023461
    public static string GetCustomHighlightMaterialPath(SEX sex)
    {
        return (sex != SEX.FEMALE) ? CharaShapeCustomBase.customHighlightMaterialPath_M : CharaShapeCustomBase.customHighlightMaterialPath_F;
    }

    // Token: 0x060005F8 RID: 1528 RVA: 0x00025078 File Offset: 0x00023478
    public static string GetCustomHighlightMaterialName(SEX sex)
    {
        return (sex != SEX.FEMALE) ? CharaShapeCustomBase.customHighlightMaterialName_M : CharaShapeCustomBase.customHighlightMaterialName_F;
    }

    // Token: 0x040006AC RID: 1708
    protected static readonly string customHighlightMaterialPath_F = "Custom Point F/Materials/";

    // Token: 0x040006AD RID: 1709
    public static readonly string customHighlightMaterialName_F = "cf_M_point";

    // Token: 0x040006AE RID: 1710
    protected static readonly string customHighlightMaterialPath_M = "Custom Point M/Materials/";

    // Token: 0x040006AF RID: 1711
    public static readonly string customHighlightMaterialName_M = "cm_M_point";

    // Token: 0x040006B2 RID: 1714
    protected Human human;

    // Token: 0x040006B3 RID: 1715
    protected GameObject obj;

    // Token: 0x040006B4 RID: 1716
    protected ShapeInfoBase info;

    // Token: 0x040006B6 RID: 1718
    protected Texture skinBaseTex;

    // Token: 0x040006B7 RID: 1719
    protected Texture skinNormalTex;

    // Token: 0x040006B8 RID: 1720
    protected RenderTexture skinTex;

    // Token: 0x040006B9 RID: 1721
    protected RenderTexture normalTex;

    // Token: 0x040006BA RID: 1722
    protected Material skinMaterial;

    // Token: 0x040006BB RID: 1723
    protected Renderer rend_skin;

    // Token: 0x040006BC RID: 1724
    protected int skinMaterialNo;

    // Token: 0x040006BD RID: 1725
    protected Material customHighlightMat_Skin;

    // Token: 0x040006BE RID: 1726
    protected Animator anime;
}
