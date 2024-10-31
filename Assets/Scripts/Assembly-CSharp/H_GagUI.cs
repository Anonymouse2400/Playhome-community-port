using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class H_GagUI : MonoBehaviour
{
	private H_Scene h_scene;

	[SerializeField]
	private ToggleButton toggle;

	[SerializeField]
	private GameObject root;

	[SerializeField]
	private RadioButtonGroup mainRadioGroup;

	[SerializeField]
	private RadioButtonGroup subRadioGroup;

	[SerializeField]
	private Text mainName;

	[SerializeField]
	private Text subName;

	private void Awake()
	{
		toggle.action.AddListener(OnToggle);
	}

	public void Setup(H_Scene h_scene)
	{
		this.h_scene = h_scene;
		mainRadioGroup.action.AddListener(ChangeMain);
		subRadioGroup.action.AddListener(ChangeSub);
		Female female = h_scene.mainMembers.GetFemale(0);
		Female female2 = null;
		if (h_scene.visitor != null)
		{
			female2 = h_scene.visitor.GetHuman() as Female;
		}
		subRadioGroup.gameObject.SetActive(female2 != null);
		mainName.text = Female.HeroineName(female.HeroineID);
		mainRadioGroup.Change((int)female.GagType, false);
		if (female2 != null)
		{
			subRadioGroup.Change((int)female2.GagType, false);
			subName.text = Female.HeroineName(female2.HeroineID);
		}
	}

	public void SetNameUI()
	{
		Female female = h_scene.mainMembers.GetFemale(0);
		Female female2 = null;
		if (h_scene.visitor != null)
		{
			female2 = h_scene.visitor.GetHuman() as Female;
		}
		mainName.text = Female.HeroineName(female.HeroineID);
		if (female2 != null)
		{
			subName.text = Female.HeroineName(female2.HeroineID);
		}
	}

	private void OnToggle(bool flag)
	{
		root.gameObject.SetActive(flag);
	}

	private void ChangeMain(int value)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		Female female = h_scene.mainMembers.GetFemale(0);
		female.personality.gagItem = (GAG_ITEM)value;
		female.ChangeGagItem();
	}

	private void ChangeSub(int value)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		Female female = null;
		if (h_scene.visitor != null)
		{
			female = h_scene.visitor.GetHuman() as Female;
		}
		if (female != null)
		{
			female.personality.gagItem = (GAG_ITEM)value;
			female.ChangeGagItem();
		}
	}
}
