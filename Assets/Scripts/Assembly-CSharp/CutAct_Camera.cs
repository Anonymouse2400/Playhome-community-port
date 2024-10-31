using System;
using UnityEngine;

public class CutAct_Camera : CutAction
{
	public Vector3 pos;

	public Quaternion rot;

	public float fov;

	public CutScene.FOCUS focusType;

	public Vector3 focusPos;

	public string focusChara;

	public CutAct_Camera(CutScene cutScene)
		: base(cutScene, CUTACT.CAMERA)
	{
	}

	public CutAct_Camera(CutScene cutScene, float time, Vector3 pos, Quaternion rot, float fov)
		: base(cutScene, CUTACT.CAMERA, time)
	{
		this.pos = pos;
		this.rot = rot;
		this.fov = fov;
	}

	public override object Clone()
	{
		return new CutAct_Camera(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		pos = TagTextUtility.Load_Vector3(element, "pos");
		Vector3 vec = Vector3.zero;
		if (TagTextUtility.Load_Vector3(ref vec, element, "euler"))
		{
			rot = Quaternion.Euler(vec);
		}
		else
		{
			TagTextUtility.Load_Quaternion(ref rot, element, "rot");
		}
		fov = -1f;
		element.GetVal(ref fov, "fov", 0);
		string str = string.Empty;
		TagTextUtility.Load_String(ref str, element, "focus");
		if (str == "POS")
		{
			focusType = CutScene.FOCUS.POS;
			TagTextUtility.Load_Vector3(ref focusPos, element, "focus", 1);
		}
		else if (str == "CHARA")
		{
			focusType = CutScene.FOCUS.CHARA;
			TagTextUtility.Load_String(ref focusChara, element, "focus", 1);
		}
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		TagTextUtility.Save_Vector3(element, "pos", pos);
		TagTextUtility.Save_Quaternion(element, "rot", rot);
		element.AddAttribute("fov", fov.ToString());
	}

	public void SetCamera()
	{
		cutScene.SetCamera(pos, rot, fov);
		if (focusType == CutScene.FOCUS.CHARA)
		{
			cutScene.SetFocus(focusChara);
		}
		else if (focusType == CutScene.FOCUS.POS)
		{
			cutScene.SetFocus(focusPos);
		}
	}

	public override void Action(bool skip)
	{
		SetCamera();
	}
}
