using System;
using Character;
using UnityEngine;

[RequireComponent(typeof(Female))]
public class FemaleParamCustomTest : ParamCustomTestBase
{
	protected Female m_female;

	public ItemSelectionSet sel_heads = new ItemSelectionSet();

	public ItemSelectionSet sel_skin_face = new ItemSelectionSet();

	public ItemSelectionSet sel_skin_body = new ItemSelectionSet();

	public ItemSelectionSet sel_eye = new ItemSelectionSet();

	public ItemSelectionSet sel_eyebrow = new ItemSelectionSet();

	public ItemSelectionSet sel_eyelash = new ItemSelectionSet();

	public ItemSelectionSet sel_eyeHighlight = new ItemSelectionSet();

	public ItemSelectionSet sel_nip = new ItemSelectionSet();

	public ItemSelectionSet sel_underhair = new ItemSelectionSet();

	public ItemSelectionSet sel_sunbarn = new ItemSelectionSet();

	public ItemSelectionSet sel_bodyTattoo = new ItemSelectionSet();

	public ItemSelectionSet sel_faceTattoo = new ItemSelectionSet();

	public ItemSelectionSet sel_mole = new ItemSelectionSet();

	public ItemSelectionSet sel_cheek = new ItemSelectionSet();

	public ItemSelectionSet sel_lip = new ItemSelectionSet();

	public ItemSelectionSet sel_eyeshadow = new ItemSelectionSet();

	public ItemSelectionSet sel_hairSelect_F = new ItemSelectionSet();

	public ItemSelectionSet sel_hairSelect_S = new ItemSelectionSet();

	public ItemSelectionSet sel_hairSelect_B = new ItemSelectionSet();

	public ItemSelectionSet[] sel_wears = new ItemSelectionSet[11];

	public Female female
	{
		get
		{
			if (m_female == null)
			{
				m_female = GetComponent<Female>();
			}
			return m_female;
		}
	}

	public override Human GetHuman()
	{
		return female;
	}

	private void Awake()
	{
		m_female = GetComponent<Female>();
		Setup();
	}

	private void Start()
	{
		female.Apply();
	}

	private void Setup()
	{
		sel_heads.Setup(CustomDataManager.Heads_Female);
		sel_skin_face.Setup(CustomDataManager.FaceSkins_Female);
		sel_skin_body.Setup(CustomDataManager.BodySkins_Female);
		sel_eye.Setup(CustomDataManager.Eye_Female);
		sel_eyebrow.Setup(CustomDataManager.Eyebrow_Female);
		sel_eyelash.Setup(CustomDataManager.Eyelash);
		sel_eyeHighlight.Setup(CustomDataManager.Eyehighlight);
		sel_nip.Setup(CustomDataManager.Nip);
		sel_underhair.Setup(CustomDataManager.Underhair);
		sel_sunbarn.Setup(CustomDataManager.Sunburn);
		sel_bodyTattoo.Setup(CustomDataManager.BodyTattoo_Female);
		sel_faceTattoo.Setup(CustomDataManager.FaceTattoo_Female);
		sel_mole.Setup(CustomDataManager.Mole);
		sel_cheek.Setup(CustomDataManager.Cheek);
		sel_lip.Setup(CustomDataManager.Lip);
		sel_eyeshadow.Setup(CustomDataManager.EyeShadow);
		sel_hairSelect_F.Setup(CustomDataManager.Hair_f);
		sel_hairSelect_S.Setup(CustomDataManager.Hair_s);
		sel_hairSelect_B.Setup(CustomDataManager.Hair_b);
		for (WEAR_TYPE wEAR_TYPE = WEAR_TYPE.TOP; wEAR_TYPE < WEAR_TYPE.NUM; wEAR_TYPE++)
		{
			int num = (int)wEAR_TYPE;
			sel_wears[num] = new ItemSelectionSet();
			sel_wears[num].Setup(CustomDataManager.GetWearDictionary_Female(wEAR_TYPE));
		}
		SetupAcce();
	}
}
