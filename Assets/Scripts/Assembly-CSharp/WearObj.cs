using System;
using Character;
using UnityEngine;

public class WearObj
{
	public static readonly string[,] ShowRoots = new string[14, 2]
	{
		{ "N_top_a", "N_top_b" },
		{ "N_bot_d", "N_bot_n" },
		{ "N_bot_d", "N_bot_n" },
		{ "cf_O_bra_d", "cf_O_bra_n" },
		{ "cf_O_shorts_d", "cf_O_shorts_n" },
		{ "N_top_a", "N_top_b" },
		{ "N_bot_a", "N_bot_b" },
		{ "N_top_a", "N_top_b" },
		{ "N_bot_d", "N_bot_n" },
		{ "N_bot_a", "N_bot_b" },
		{ null, null },
		{ "N_panst_d", "N_panst_n" },
		{ null, null },
		{ null, null }
	};

	public GameObject obj;

	public MaterialCustoms materialCustom;

	public bool enableColorCustom;

	public bool hasSecondColor;

	public bool hasSecondSpecular;

	public UVNormalBlend uv_normal;

	private WearParameter wearParam;

	private WEAR_TYPE type;

	private int showUpper;

	private int showLower;

	private GameObject upperAll;

	private GameObject upperHalf;

	private GameObject lowerAll;

	private GameObject lowerHalf;

	public WearLiquidObj liquid;

	public int ShowUpperNum
	{
		get
		{
			return showUpper;
		}
	}

	public int ShowLowerNum
	{
		get
		{
			return showLower;
		}
	}

	public WearObj(WearParameter wearParam, WEAR_TYPE type, GameObject obj, WearData data)
	{
		this.obj = obj;
		this.wearParam = wearParam;
		this.type = type;
		uv_normal = obj.GetComponent<UVNormalBlend>();
		SetupMaterials(data);
		SetupShow();
	}

	private void SetupMaterials(WearData data)
	{
		int num = (int)type;
		materialCustom = obj.GetComponent<MaterialCustoms>();
		enableColorCustom = materialCustom != null;
		hasSecondColor = false;
		hasSecondSpecular = false;
		if (enableColorCustom)
		{
			for (int i = 0; i < materialCustom.datas.Length; i++)
			{
				if (materialCustom.datas[i].param.name.IndexOf("Sub") >= 0)
				{
					hasSecondColor = true;
				}
				if (materialCustom.datas[i].param.name.IndexOf("SubMetallic") >= 0)
				{
					hasSecondColor = true;
					hasSecondSpecular = true;
					break;
				}
			}
			if (wearParam.wears[num] == null)
			{
				wearParam.wears[num].color = new ColorParameter_PBR2();
			}
		}
		else
		{
			wearParam.wears[num].color = null;
		}
	}

	private void SetupShow()
	{
		if (type == WEAR_TYPE.TOP)
		{
			showUpper = SetupShow(WEAR_SHOW_TYPE.TOPUPPER);
			showLower = SetupShow(WEAR_SHOW_TYPE.TOPLOWER);
		}
		else if (type == WEAR_TYPE.SWIM)
		{
			showUpper = SetupShow(WEAR_SHOW_TYPE.SWIMUPPER);
			showLower = SetupShow(WEAR_SHOW_TYPE.SWIMLOWER);
		}
		else if (type == WEAR_TYPE.SWIM_TOP)
		{
			showUpper = SetupShow(WEAR_SHOW_TYPE.SWIM_TOPUPPER);
			showLower = SetupShow(WEAR_SHOW_TYPE.SWIM_TOPLOWER);
		}
		else
		{
			WEAR_SHOW_TYPE showType = Wears.WeatToShowType[(int)type];
			showUpper = SetupShow(showType);
			showLower = 0;
		}
	}

	private int SetupShow(WEAR_SHOW_TYPE showType)
	{
		if (type == WEAR_TYPE.GLOVE || type == WEAR_TYPE.SOCKS || type == WEAR_TYPE.SHOES)
		{
			return 1;
		}
		Transform transform = null;
		Transform transform2 = null;
		string text = ShowRoots[(int)showType, 0];
		string text2 = ShowRoots[(int)showType, 1];
		if (text != null)
		{
			transform = Transform_Utility.FindTransform(obj.transform, text);
		}
		if (text2 != null)
		{
			transform2 = Transform_Utility.FindTransform(obj.transform, text2);
		}
		if (transform == null && transform2 != null)
		{
			Debug.LogError("着衣がないのに\u3000半脱ぎがある？");
		}
		int num = 0;
		if (transform != null)
		{
			num++;
		}
		if (transform2 != null)
		{
			num++;
		}
		if (Wears.IsLower(showType))
		{
			lowerAll = ((!(transform != null)) ? null : transform.gameObject);
			lowerHalf = ((!(transform2 != null)) ? null : transform2.gameObject);
		}
		else
		{
			upperAll = ((!(transform != null)) ? null : transform.gameObject);
			upperHalf = ((!(transform2 != null)) ? null : transform2.gameObject);
		}
		if (num == 0 && type == WEAR_TYPE.PANST)
		{
			return 1;
		}
		return num;
	}

	public void UpdateColorCustom()
	{
		int num = (int)type;
		if (enableColorCustom)
		{
			if (wearParam.wears[num].color == null)
			{
				wearParam.wears[num].color = new ColorParameter_PBR2(materialCustom);
			}
			else
			{
				wearParam.wears[num].color.SetMaterialCustoms(materialCustom);
			}
		}
	}

	public bool HasLowerShow()
	{
		return showLower != 0;
	}

	public bool HasUpperHalf()
	{
		return upperHalf != null;
	}

	public bool HasLowerHalf()
	{
		return lowerHalf != null;
	}

	public bool HasShow(WEAR_SHOW_TYPE showType)
	{
		if (Wears.IsLower(showType))
		{
			return showLower != 0;
		}
		return showUpper != 0;
	}

	public void ChangeShow_Upper(WEAR_SHOW show)
	{
		if (showUpper != 0)
		{
			if (upperAll != null)
			{
				upperAll.SetActive(show == WEAR_SHOW.ALL);
			}
			if (upperHalf != null)
			{
				upperHalf.SetActive(show == WEAR_SHOW.HALF);
			}
		}
	}

	public void ChangeShow_Lower(WEAR_SHOW show)
	{
		if (showLower != 0)
		{
			if (lowerAll != null)
			{
				lowerAll.SetActive(show == WEAR_SHOW.ALL);
			}
			if (lowerHalf != null)
			{
				lowerHalf.SetActive(show == WEAR_SHOW.HALF);
			}
		}
	}

	public void SetLiquidShow(int[] sperms)
	{
		bool flag = upperAll != null && upperAll.activeSelf;
		bool flag2 = upperHalf != null && upperHalf.activeSelf;
		bool flag3 = lowerAll != null && lowerAll.activeSelf;
		bool flag4 = lowerHalf != null && lowerHalf.activeSelf;
		if (lowerAll == null)
		{
			flag3 = flag;
			flag4 = flag2;
		}
		if (liquid != null)
		{
			liquid.SetShow(flag, flag2, flag3, flag4, sperms);
		}
	}
}
