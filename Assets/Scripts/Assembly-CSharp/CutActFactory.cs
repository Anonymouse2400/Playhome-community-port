using System;
using System.Collections.Generic;

public class CutActFactory : FactoryBase<string, CutAction>
{
	private CutScene scene;

	private Dictionary<string, CutAction> actDic = new Dictionary<string, CutAction>();

	public CutActFactory(CutScene scene)
	{
		this.scene = scene;
	}
}
