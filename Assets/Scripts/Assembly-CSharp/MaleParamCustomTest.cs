using System;
using Character;
using UnityEngine;

[RequireComponent(typeof(Male))]
public class MaleParamCustomTest : ParamCustomTestBase
{
    public ParamCustomTestBase.ItemSelectionSet sel_heads = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008BB RID: 2235
    public ParamCustomTestBase.ItemSelectionSet sel_skin_face = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008BC RID: 2236
    public ParamCustomTestBase.ItemSelectionSet sel_skin_body = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008BD RID: 2237
    public ParamCustomTestBase.ItemSelectionSet sel_eye = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008BE RID: 2238
    public ParamCustomTestBase.ItemSelectionSet sel_eyebrow = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008BF RID: 2239
    public ParamCustomTestBase.ItemSelectionSet sel_beard = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008C0 RID: 2240
    public ParamCustomTestBase.ItemSelectionSet sel_bodyTattoo = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008C1 RID: 2241
    public ParamCustomTestBase.ItemSelectionSet sel_faceTattoo = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008C2 RID: 2242
    public ParamCustomTestBase.ItemSelectionSet sel_hairSelect = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008C3 RID: 2243
    public ParamCustomTestBase.ItemSelectionSet sel_wear_tops = new ParamCustomTestBase.ItemSelectionSet();

    // Token: 0x040008C4 RID: 2244
    public ParamCustomTestBase.ItemSelectionSet sel_wear_shoes = new ParamCustomTestBase.ItemSelectionSet();

    public Male male { get; private set; }

	public override Human GetHuman()
	{
		return male;
	}

	private void Awake()
	{
		male = GetComponent<Male>();
		Setup();
	}

	private void Start()
	{
		male.Apply();
	}

	private void Setup()
	{
		sel_heads.Setup(CustomDataManager.Heads_Male);
		sel_skin_face.Setup(CustomDataManager.FaceSkins_Male);
		sel_skin_body.Setup(CustomDataManager.BodySkins_Male);
		sel_eye.Setup(CustomDataManager.Eye_Male);
		sel_eyebrow.Setup(CustomDataManager.Eyebrow_Male);
		sel_beard.Setup(CustomDataManager.Beard);
		sel_bodyTattoo.Setup(CustomDataManager.BodyTattoo_Male);
		sel_faceTattoo.Setup(CustomDataManager.FaceTattoo_Male);
		sel_hairSelect.Setup(CustomDataManager.Hair_Male);
		sel_wear_tops.Setup(CustomDataManager.GetWearDictionary_Male(WEAR_TYPE.TOP));
		sel_wear_shoes.Setup(CustomDataManager.GetWearDictionary_Male(WEAR_TYPE.SHOES));
		SetupAcce();
	}
}
