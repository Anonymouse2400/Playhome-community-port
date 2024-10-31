using System;

public abstract class CutAction : FactoryProduct
{
	protected CutScene cutScene;

	public float time;

	public int order;

	public CUTACT Type { get; protected set; }

	public CutAction(CutScene cutScene, CUTACT type)
	{
		this.cutScene = cutScene;
		Type = type;
		order = -1;
	}

	public CutAction(CutScene cutScene, CUTACT type, float time)
	{
		this.cutScene = cutScene;
		this.time = time;
		Type = type;
		order = -1;
	}

	public virtual void Load(TagText.Element element, int order)
	{
		this.order = order;
		element.GetVal(ref time, "time", 0);
	}

	public virtual void Save(TagText.Element element)
	{
		element.AddAttribute("time", time.ToString());
	}

	public virtual void Action(bool skip)
	{
	}
}
