using System;
using Character;
using UnityEngine;

public class H_CharaMoveController : CharaMoveController
{
	[SerializeField]
	protected RadioButtonGroup switchRoot;

	[SerializeField]
	protected ToggleButton toggleFemale;

	[SerializeField]
	protected ToggleButton toggleVisitor;

	protected H_Scene h_scene;

	protected TARGET target;

	protected override void Awake()
	{
		base.Awake();
		switchRoot.action.AddListener(ChangeTrans);
	}

	public void Setup(H_Scene h_scene)
	{
		this.h_scene = h_scene;
		SetNameUI();
		invoke = false;
		speedSlider.SetValue(speedRate);
		invoke = true;
		ChangeTrans(0, false);
	}

	public void SetNameUI()
	{
		bool flag = h_scene.visitor != null;
		if (switchRoot != null)
		{
			switchRoot.gameObject.SetActive(flag);
		}
		if (flag)
		{
			string text = Female.HeroineName(h_scene.mainMembers.GetFemale(0).HeroineID);
			toggleFemale.SetText(text, text);
			string empty = string.Empty;
			if (h_scene.visitor.GetHuman().sex == SEX.FEMALE)
			{
				Female female = h_scene.visitor.GetHuman() as Female;
				empty = Female.HeroineName(female.HeroineID);
			}
			else
			{
				Male male = h_scene.visitor.GetHuman() as Male;
				empty = Male.MaleName(male.MaleID);
			}
			toggleVisitor.SetText(empty, empty);
		}
	}

	private void Update()
	{
		if (h_scene.visitor != null)
		{
			toggleVisitor.Interactable = !h_scene.visitor.LockPos;
			if (target == TARGET.VISITOR && h_scene.visitor.LockPos)
			{
				switchRoot.Change(0, false);
				ChangeTrans(0, false);
			}
		}
	}

	public void ChangeTrans()
	{
		ChangeTrans((int)target, false);
	}

	private void ChangeTrans(int no)
	{
		ChangeTrans(no, true);
	}

	private void ChangeTrans(int no, bool se)
	{
		if (se)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
		}
		PosYaw posYaw = null;
		if (no == 0)
		{
			moveTrans = h_scene.mainMembers.Transform;
			target = TARGET.MAIN;
			posYaw = h_scene.mainMembers.GetLastPosYaw();
		}
		else
		{
			moveTrans = h_scene.visitor.GetHuman().transform;
			target = TARGET.VISITOR;
			posYaw = h_scene.visitor.GetLastPosYaw();
		}
		Vector3 pos = Vector3.zero;
		Quaternion rot = Quaternion.identity;
		if (posYaw != null)
		{
			pos = posYaw.pos;
			rot = Quaternion.Euler(0f, posYaw.yaw, 0f);
		}
		SetDef(pos, rot);
	}

	public void Button_NextMapPos()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		if (target == TARGET.MAIN)
		{
			h_scene.NextMapPos_MainMember();
		}
		else
		{
			h_scene.NextMapPos_Visitor();
		}
		SetDef(moveTrans.position, moveTrans.rotation);
	}

	public void Button_PrevMapPos()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		if (target == TARGET.MAIN)
		{
			h_scene.PrevMapPos_MainMember();
		}
		else
		{
			h_scene.PrevMapPos_Visitor();
		}
		SetDef(moveTrans.position, moveTrans.rotation);
	}
}
