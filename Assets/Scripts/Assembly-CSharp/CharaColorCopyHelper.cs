using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class CharaColorCopyHelper : MonoBehaviour
{
	private enum TAB
	{
		HAIR = 0,
		SKIN = 1,
		NUM = 2
	}

	private static readonly string[,] NamesF = new string[2, 4]
	{
		{ "髪", "眉", "まつげ", "陰毛" },
		{ "肌", "日焼け", "乳首", "爪" }
	};

	private static readonly string[,] NamesM = new string[2, 4]
	{
		{ "髪", "眉", "ヒゲ", "陰毛" },
		{ "肌", "日焼け", "乳首", "爪" }
	};

	public MoveableUI moveable;

	[SerializeField]
	private EditMode editMode;

	[SerializeField]
	private Toggle[] tabs = new Toggle[2];

	[SerializeField]
	private Toggle[] fromToggles = new Toggle[4];

	[SerializeField]
	private Toggle[] toToggles = new Toggle[4];

	private int tab = -1;

	private int from = -1;

	private bool playSE = true;

	private Human human;

	public void Setup(Human human)
	{
		this.human = human;
		tabs[1].gameObject.SetActive(human.sex == SEX.FEMALE);
		for (int i = 0; i < fromToggles.Length; i++)
		{
			fromToggles[i].onValueChanged.AddListener(OnCheckTab);
		}
		for (int j = 0; j < toToggles.Length; j++)
		{
			toToggles[j].onValueChanged.AddListener(OnCheck);
		}
		for (int k = 0; k < tabs.Length; k++)
		{
			tabs[k].onValueChanged.AddListener(OnCheckTab);
		}
	}

	private void Update()
	{
		playSE = false;
		int num = -1;
		for (int i = 0; i < tabs.Length; i++)
		{
			if (tabs[i].isOn)
			{
				num = i;
				break;
			}
		}
		if (num != tab)
		{
			ChangeTab(num);
		}
		for (int j = 0; j < fromToggles.Length; j++)
		{
			if (fromToggles[j].isOn)
			{
				from = j;
				break;
			}
		}
		for (int k = 0; k < toToggles.Length; k++)
		{
			toToggles[k].gameObject.SetActive(k != from);
		}
		playSE = true;
	}

	private void ChangeTab(int newTab)
	{
		bool flag = playSE;
		playSE = false;
		tab = newTab;
		string[,] array = ((human.sex != 0) ? NamesM : NamesF);
		for (int i = 0; i < fromToggles.Length; i++)
		{
			fromToggles[i].GetComponentInChildren<Text>().text = array[tab, i];
		}
		fromToggles[0].isOn = true;
		for (int j = 0; j < toToggles.Length; j++)
		{
			toToggles[j].GetComponentInChildren<Text>().text = array[tab, j];
			toToggles[j].isOn = false;
		}
		playSE = flag;
	}

	public void Button_Copy()
	{
		if (tab == 0)
		{
			for (int i = 0; i < toToggles.Length; i++)
			{
				if (toToggles[i].gameObject.activeSelf && toToggles[i].isOn)
				{
					Copy_Hair(i);
				}
			}
		}
		else if (tab == 1)
		{
			for (int j = 0; j < toToggles.Length; j++)
			{
				if (toToggles[j].gameObject.activeSelf && toToggles[j].isOn)
				{
					Copy_Skin(j);
				}
			}
		}
		if (playSE)
		{
			SystemSE.Play(SystemSE.SE.YES);
		}
	}

	private void OnCheck(bool check)
	{
		SystemSE.SE se = (check ? SystemSE.SE.YES : SystemSE.SE.NO);
		if (playSE)
		{
			SystemSE.Play(se);
		}
	}

	private void OnCheckTab(bool check)
	{
		if (playSE && check)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
	}

	private void Copy_Hair(int to)
	{
		Color color = Color.white;
		if (from == 0)
		{
			color = human.customParam.hair.parts[0].hairColor.mainColor;
		}
		else if (from == 1)
		{
			color = human.customParam.head.eyeBrowColor.mainColor1;
		}
		else if (from == 2)
		{
			color = ((human.sex != 0) ? human.customParam.head.beardColor : human.customParam.head.eyeLashColor.mainColor1);
		}
		else if (from == 3)
		{
			color = human.customParam.body.underhairColor.mainColor;
		}
		switch (to)
		{
		case 0:
		{
			for (int i = 0; i < human.customParam.hair.parts.Length; i++)
			{
				human.customParam.hair.parts[i].hairColor.mainColor = color;
			}
			human.hairs.ChangeColor(human.customParam.hair);
			break;
		}
		case 1:
			human.customParam.head.eyeBrowColor.mainColor1 = color;
			human.head.ChangeEyebrowColor();
			break;
		case 2:
			if (human.sex == SEX.FEMALE)
			{
				human.customParam.head.eyeLashColor.mainColor1 = color;
				human.head.ChangeEyelashColor();
			}
			else
			{
				human.customParam.head.beardColor = color;
				human.head.ChangeBeardColor();
			}
			break;
		case 3:
			human.customParam.body.underhairColor.mainColor = color;
			human.body.ChangeUnderHairColor();
			break;
		}
		editMode.ChangedColor();
	}

	private void Copy_Skin(int to)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (from == 0)
		{
			num = human.customParam.body.skinColor.offset_h;
			num2 = human.customParam.body.skinColor.offset_s;
			num3 = human.customParam.body.skinColor.offset_v;
		}
		else if (from == 1)
		{
			num = human.customParam.body.sunburnColor_H;
			num2 = human.customParam.body.sunburnColor_S;
			num3 = human.customParam.body.sunburnColor_V;
		}
		else if (from == 2)
		{
			num = human.customParam.body.nipColor.offset_h;
			num2 = human.customParam.body.nipColor.offset_s;
			num3 = human.customParam.body.nipColor.offset_v;
		}
		else if (from == 3)
		{
			num = human.customParam.body.nailColor.offset_h;
			num2 = human.customParam.body.nailColor.offset_s;
			num3 = human.customParam.body.nailColor.offset_v;
		}
		switch (to)
		{
		case 0:
			human.customParam.body.skinColor.offset_h = num;
			human.customParam.body.skinColor.offset_s = num2;
			human.customParam.body.skinColor.offset_v = num3;
			ChangeSkin();
			break;
		case 1:
			human.customParam.body.sunburnColor_H = num;
			human.customParam.body.sunburnColor_S = num2;
			human.customParam.body.sunburnColor_V = num3;
			ChangeSkin();
			break;
		case 2:
			human.customParam.body.nipColor.offset_h = num;
			human.customParam.body.nipColor.offset_s = num2;
			human.customParam.body.nipColor.offset_v = num3;
			human.body.ChangeNipColor();
			break;
		case 3:
			human.customParam.body.nailColor.offset_h = num;
			human.customParam.body.nailColor.offset_s = num2;
			human.customParam.body.nailColor.offset_v = num3;
			human.body.ChangeNail();
			break;
		}
		editMode.ChangedColor();
	}

	private void ChangeSkin()
	{
		human.body.RendSkinTexture();
		human.UpdateSkinMaterial();
		human.wears.ChangeBodyMaterial(human.body.Rend_skin);
		human.head.RendSkinTexture();
	}
}
