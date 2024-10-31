using System;
using Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HWearAcceChangeUI : MonoBehaviour
{
	[SerializeField]
	private ToggleButton mainToggle;

	[SerializeField]
	private GameObject hideableRoot;

	[SerializeField]
	private Button wearAll;

	[SerializeField]
	private Button wearHalf;

	[SerializeField]
	private Button wearNude;

	[SerializeField]
	private Button[] wears = new Button[14];

	[SerializeField]
	private Button acceOn;

	[SerializeField]
	private Button acceOff;

	[SerializeField]
	private Button[] acces = new Button[10];

	[SerializeField]
	private RadioButtonGroup switchRoot;

	[SerializeField]
	private ToggleButton switchFemale;

	[SerializeField]
	private ToggleButton switchVisitor;

	private bool invoke = true;

	private H_Scene h_scene;

	private Female female;

	private void Awake()
	{
		mainToggle.ActionAddListener(new UnityAction<bool>(OnChangeMainToggle));
		wearAll.onClick.AddListener(new UnityAction(ChangeAcceShow_All));
		wearHalf.onClick.AddListener(new UnityAction(ChangeAcceShow_Half));
		wearNude.onClick.AddListener(new UnityAction(ChangeWearShow_Nude));
		for (int i = 0; i < 14; i++)
		{
			WEAR_SHOW_TYPE id = (WEAR_SHOW_TYPE)i;
			wears[i].onClick.AddListener(delegate
			{
				ChangeWearShow(id);
			});
		}
		acceOn.onClick.AddListener(new UnityAction(ChangeAcceShow_AllOn));
		acceOff.onClick.AddListener(new UnityAction(ChangeAcceShow_AllOff));
		for (int j = 0; j < 10; j++)
		{
			int id2 = j;
			acces[j].onClick.AddListener(delegate
			{
				ChangeAcceShow(id2);
			});
		}
		hideableRoot.SetActive(false);
		switchRoot.action.AddListener(new UnityAction<int>(SwitchFemale));
	}

	public void Setup(H_Scene h_scene)
	{
		this.h_scene = h_scene;
		SetNameUI();
		switchRoot.Value = 0;
		SwitchFemale(0);
	}

	private void SetNameUI()
	{
		Female female = h_scene.mainMembers.GetFemale(0);
		Female female2 = null;
		if (h_scene.visitor != null)
		{
			female2 = h_scene.visitor.GetHuman() as Female;
		}
		string text = Female.HeroineName(female.HeroineID);
		switchFemale.SetText(text, text);
		switchRoot.gameObject.SetActive(female2 != null);
		if (h_scene.visitor != null && h_scene.visitor.IsFemale())
		{
			string text2 = Female.HeroineName(female2.HeroineID);
			switchVisitor.SetText(text2, text2);
		}
	}

	public void SwitchedFemaleVisitor()
	{
		SetNameUI();
		SwitchFemale(0);
	}

	private void SwitchFemale(int no)
	{
		switch (no)
		{
		case 0:
			female = h_scene.mainMembers.GetFemale(0);
			break;
		case 1:
			female = h_scene.visitor.GetHuman() as Female;
			break;
		}
		CheckShowUI();
	}

	public void CheckShowUI()
	{
		for (int i = 0; i < 14; i++)
		{
			WEAR_SHOW_TYPE type = (WEAR_SHOW_TYPE)i;
			wears[i].gameObject.SetActive(female.wears.IsEquiped(female.customParam, type));
		}
		for (int j = 0; j < 10; j++)
		{
			acces[j].gameObject.SetActive(female.accessories.IsAttachedAccessory(j));
		}
	}

	private void OnChangeMainToggle(bool change)
	{
		hideableRoot.SetActive(change);
		if (change)
		{
			CheckShowUI();
		}
	}

	private void ChangeAcceShow_All()
	{
		for (WEAR_SHOW_TYPE wEAR_SHOW_TYPE = WEAR_SHOW_TYPE.TOPUPPER; wEAR_SHOW_TYPE < WEAR_SHOW_TYPE.SHOES; wEAR_SHOW_TYPE++)
		{
			female.wears.ChangeShow(wEAR_SHOW_TYPE, WEAR_SHOW.ALL);
		}
		female.CheckShow();
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void ChangeAcceShow_Half()
	{
		for (WEAR_SHOW_TYPE wEAR_SHOW_TYPE = WEAR_SHOW_TYPE.TOPUPPER; wEAR_SHOW_TYPE < WEAR_SHOW_TYPE.SHOES; wEAR_SHOW_TYPE++)
		{
			female.wears.ChangeShow(wEAR_SHOW_TYPE, WEAR_SHOW.HALF);
		}
		female.CheckShow();
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void ChangeWearShow_Nude()
	{
		for (WEAR_SHOW_TYPE wEAR_SHOW_TYPE = WEAR_SHOW_TYPE.TOPUPPER; wEAR_SHOW_TYPE < WEAR_SHOW_TYPE.SHOES; wEAR_SHOW_TYPE++)
		{
			female.wears.ChangeShow(wEAR_SHOW_TYPE, WEAR_SHOW.HIDE);
		}
		female.CheckShow();
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void ChangeWearShow(WEAR_SHOW_TYPE type)
	{
        WEAR_SHOW show = this.female.wears.GetShow(type, true);
        WEAR_SHOW show2 = this.female.wears.GetShow(type, false);
        if (show == show2)
        {
            WEAR_SHOW wear_SHOW = this.Next(type, show2);
            this.female.wears.ChangeShow(type, wear_SHOW);
            this.female.CheckShow();
        }
        else
        {
            WEAR_SHOW_TYPE wearShowTypePair = Wears.GetWearShowTypePair(type);
            this.female.wears.ChangeShow(type, WEAR_SHOW.ALL);
            this.female.wears.ChangeShow(wearShowTypePair, WEAR_SHOW.ALL);
            this.female.CheckShow();
        }
        SystemSE.Play(SystemSE.SE.CHOICE);
    }

	private WEAR_SHOW Next(WEAR_SHOW_TYPE type, WEAR_SHOW now)
	{
		switch (female.wears.GetWearShowNum(type))
		{
		case 2:
		{
			int num = (int)(now + 1) % 3;
			now = (WEAR_SHOW)num;
			break;
		}
		case 1:
			now = ((now == WEAR_SHOW.ALL) ? WEAR_SHOW.HIDE : WEAR_SHOW.ALL);
			break;
		}
		return now;
	}

	private void ChangeAcceShow_AllOn()
	{
		for (int i = 0; i < 10; i++)
		{
			female.accessories.SetShow(i, true);
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void ChangeAcceShow_AllOff()
	{
		for (int i = 0; i < 10; i++)
		{
			female.accessories.SetShow(i, false);
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void ChangeAcceShow(int id)
	{
		bool show = !female.accessories.GetShow(id);
		female.accessories.SetShow(id, show);
		SystemSE.Play(SystemSE.SE.CHOICE);
	}
}
