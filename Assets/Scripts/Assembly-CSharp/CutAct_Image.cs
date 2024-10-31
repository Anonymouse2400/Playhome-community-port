using System;

public class CutAct_Image : CutAction
{
	private string file;

	private bool show;

	public CutAct_Image(CutScene cutScene)
		: base(cutScene, CUTACT.IMAGE)
	{
	}

	public CutAct_Image(CutScene cutScene, float time, string file, bool show)
		: base(cutScene, CUTACT.IMAGE, time)
	{
		this.file = file;
		this.show = show;
	}

	public override object Clone()
	{
		return new CutAct_Image(cutScene);
	}

	public override void Load(TagText.Element element, int order)
	{
		base.Load(element, order);
		file = TagTextUtility.Load_String(element, "file");
		show = true;
		TagTextUtility.Load_Bool(ref show, element, "show");
	}

	public override void Save(TagText.Element element)
	{
		base.Save(element);
		element.AddAttribute("file", file);
		element.AddAttribute("show", show.ToString());
	}

	public override void Action(bool skip)
	{
		if (file != null && file.Length > 0)
		{
			cutScene.SetImage(file);
		}
		cutScene.ShowImage(show);
	}
}
