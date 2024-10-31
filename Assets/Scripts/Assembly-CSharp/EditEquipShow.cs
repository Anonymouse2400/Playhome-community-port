using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class EditEquipShow : MonoBehaviour
{
	public enum WEARSHOW
	{
		AUTO = 0,
		ALL = 1,
		UNDERWEAR = 2,
		NUDE = 3,
		NUM = 4
	}

	public enum ACCESHOW
	{
		ON = 0,
		OFF = 1,
		NUM = 2
	}

	[SerializeField]
	private Toggle[] wearToggles = new Toggle[4];

	[SerializeField]
	private Toggle[] acceToggles = new Toggle[2];

	private bool invoke = true;

	private Human human;

	private WEARSHOW auto = WEARSHOW.ALL;

	public WEARSHOW wearShow { get; private set; }

	public bool acceShow { get; private set; }

	public void Setup(Human human)
	{
		this.human = human;
		wearShow = WEARSHOW.AUTO;
		acceShow = true;
		wearToggles[2].interactable = human.sex == SEX.FEMALE;
	}

	private void Start()
	{
		invoke = false;
		wearToggles[0].isOn = true;
		acceToggles[0].isOn = true;
		invoke = true;
	}

	private void Update()
	{
	}

	public void ChangeWearShow(int no)
	{
		if (invoke)
		{
			wearShow = (WEARSHOW)no;
			WEARSHOW show = wearShow;
			if (wearShow == WEARSHOW.AUTO)
			{
				show = auto;
			}
			Calc(show);
		}
	}

	private void Calc(WEARSHOW show)
	{
		if (human.sex == SEX.MALE)
		{
			Male male = human as Male;
			MALE_SHOW show2 = ((show != WEARSHOW.ALL) ? MALE_SHOW.NUDE : MALE_SHOW.CLOTHING);
			male.ChangeMaleShow(show2);
			return;
		}
		switch (show)
		{
		case WEARSHOW.ALL:
		{
			for (int j = 0; j < 14; j++)
			{
				human.wears.ChangeShow((WEAR_SHOW_TYPE)j, WEAR_SHOW.ALL);
			}
			human.CheckShow();
			break;
		}
		case WEARSHOW.UNDERWEAR:
			human.wears.ChangeShow(WEAR_SHOW_TYPE.TOPUPPER, WEAR_SHOW.HIDE);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.TOPLOWER, WEAR_SHOW.HIDE);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.BOTTOM, WEAR_SHOW.HIDE);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.BRA, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SHORTS, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMUPPER, WEAR_SHOW.HIDE);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIMLOWER, WEAR_SHOW.HIDE);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPUPPER, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_TOPLOWER, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SWIM_BOTTOM, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.GLOVE, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.PANST, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SOCKS, WEAR_SHOW.ALL);
			human.wears.ChangeShow(WEAR_SHOW_TYPE.SHOES, WEAR_SHOW.ALL);
			human.CheckShow();
			break;
		case WEARSHOW.NUDE:
		{
			for (int i = 0; i < 14; i++)
			{
				human.wears.ChangeShow((WEAR_SHOW_TYPE)i, WEAR_SHOW.HIDE);
			}
			human.CheckShow();
			break;
		}
		}
	}

	public void ChangeAcceShow(int no)
	{
		if (invoke)
		{
			acceShow = no == 0;
			for (int i = 0; i < 10; i++)
			{
				human.accessories.SetShow(i, acceShow);
			}
		}
	}

	public void SetAuto(WEARSHOW auto)
	{
		this.auto = auto;
		if (wearShow == WEARSHOW.AUTO)
		{
			Calc(auto);
		}
	}
}
