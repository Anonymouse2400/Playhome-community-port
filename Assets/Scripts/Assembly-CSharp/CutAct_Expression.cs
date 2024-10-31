using System;
using UnityEngine;

public class CutAct_Expression : CutAction
{
	public string chara;

	public string eye;

	public float eyeOpen = 100f;

	public string mouth;

	public float mouthOpen;

	public float duration = 0.5f;

	public string look = string.Empty;

	public string flush = string.Empty;

	public string tear = string.Empty;

	public CutAct_Expression(CutScene cutScene)
		: base(cutScene, CUTACT.EXPRESSION)
	{
	}

	public override object Clone()
	{
		return new CutAct_Expression(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		element.GetVal(ref chara, "chara", 0);
		element.GetVal(ref eye, "eye", 0);
		element.GetVal(ref eyeOpen, "eye", 1);
		element.GetVal(ref mouth, "mouth", 0);
		element.GetVal(ref mouthOpen, "mouth", 1);
		element.GetVal(ref duration, "duration", 0);
		element.GetVal(ref look, "look", 0);
		element.GetVal(ref flush, "flush", 0);
		element.GetVal(ref tear, "tear", 0);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("chara", chara);
		element.AddAttribute("eye", eye + "," + eyeOpen);
		element.AddAttribute("mouth", mouth + "," + mouthOpen);
		element.AddAttribute("duration", duration.ToString());
		element.AddAttribute("look", look.ToString());
		element.AddAttribute("flush", flush);
		element.AddAttribute("tear", tear);
	}

	public override void Action(bool skip)
	{
		Human human = cutScene.GetHuman(chara);
		human.ExpressionPlay(0, eye, duration);
		human.ExpressionPlay(1, mouth, duration);
		if (human.blink != null)
		{
			human.blink.LimitMax = eyeOpen * 0.01f;
		}
		if (human.lipSync != null)
		{
			human.lipSync.RelaxOpen = mouthOpen * 0.01f;
		}
		if (flush.Length > 0)
		{
			human.SetFlush(float.Parse(flush) * 0.01f);
		}
		if (tear.Length > 0)
		{
			human.SetTear(float.Parse(tear) * 0.01f);
		}
		if (look.Length <= 0)
		{
			return;
		}
		if (look.Equals("カメラ") || look.Equals("Camera", StringComparison.OrdinalIgnoreCase))
		{
			human.ChangeEyeLook(LookAtRotator.TYPE.TARGET, cutScene.camera.transform, false);
			return;
		}
		Vector3 zero = Vector3.zero;
		if (look.IndexOf("右") != -1)
		{
			zero.y = 45f;
		}
		else if (look.IndexOf("左") != -1)
		{
			zero.y = -45f;
		}
		if (look.IndexOf("上") != -1)
		{
			zero.x = -45f;
		}
		else if (look.IndexOf("下") != -1)
		{
			zero.x = 45f;
		}
		if (zero.magnitude > 0f)
		{
			human.ChangeEyeLook(LookAtRotator.TYPE.DIRECTION, zero, false);
			return;
		}
		Human human2 = cutScene.GetHuman(look);
		if (human2 != null)
		{
			human.ChangeEyeLook(LookAtRotator.TYPE.TARGET, human2.HeadPosTrans, false);
		}
		else
		{
			human.ChangeEyeLook(LookAtRotator.TYPE.FORWARD, null, false);
		}
	}
}
