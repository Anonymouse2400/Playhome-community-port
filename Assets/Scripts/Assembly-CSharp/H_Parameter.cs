using System;
using Character;
using H;

public class H_Parameter
{
	public enum DETAIL
	{
		NO = 0,
		COUGH = 1,
		SHOW_ORAL = 2,
		VOMIT = 4,
		DRINK = 8
	}

	public H_MOUTH mouth;

	public H_StyleData style;

	public H_SPEED speed = H_SPEED.NORMAL;

	public XTC_TYPE xtcType;

	public bool manyEjaculation;

	public bool hitEnableStyle;

	public bool hit;

	public int continuanceXTC_F;

	public string mapName = string.Empty;

	public bool anyAction;

	public DETAIL detail;

	public bool maleHateEvent;

	public int swapVisitor;

	public void Init()
	{
		mouth = H_MOUTH.FREE;
		style = null;
		speed = H_SPEED.NORMAL;
		xtcType = XTC_TYPE.NONE;
		manyEjaculation = false;
		hitEnableStyle = false;
		hit = false;
		continuanceXTC_F = 0;
		mapName = string.Empty;
		anyAction = false;
		detail = DETAIL.NO;
		swapVisitor = 0;
	}

	public bool UseMouth(SEX sex)
	{
		if (style == null)
		{
			return false;
		}
		if (sex == SEX.FEMALE)
		{
			return style.type != H_StyleData.TYPE.PETTING && mouth != H_MOUTH.FREE;
		}
		return style.type == H_StyleData.TYPE.PETTING && mouth != H_MOUTH.FREE;
	}
}
