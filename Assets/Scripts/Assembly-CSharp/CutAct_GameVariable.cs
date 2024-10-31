using System;
using UnityEngine;

public class CutAct_GameVariable : CutAction
{
	public string type;

	public string val;

	public CutAct_GameVariable(CutScene cutScene)
		: base(cutScene, CUTACT.GAMEVARIABLE)
	{
	}

	public CutAct_GameVariable(CutScene cutScene, float time, string type, string val)
		: base(cutScene, CUTACT.GAMEVARIABLE, time)
	{
		this.type = type;
		this.val = val;
	}

	public override object Clone()
	{
		return new CutAct_GameVariable(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		element.GetVal(ref type, "type", 0);
		element.GetVal(ref val, "val", 0);
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("type", type);
		element.AddAttribute("val", val);
	}

	public override void Action(bool skip)
	{
		if (type.Equals("progress", StringComparison.OrdinalIgnoreCase))
		{
			bool flag = false;
			string[] names = Enum.GetNames(typeof(GamePlayData.PROGRESS));
			for (int i = 0; i < names.Length; i++)
			{
				if (val.Equals(names[i], StringComparison.OrdinalIgnoreCase))
				{
					GlobalData.PlayData.Progress = (GamePlayData.PROGRESS)i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Debug.LogWarning("不明な値:" + val);
			}
		}
		else
		{
			Debug.LogWarning("不明な変数:" + type);
		}
	}
}
