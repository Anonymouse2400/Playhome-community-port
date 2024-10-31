using System;
using UnityEngine;

public class MirrorSet : MonoBehaviour
{
	[SerializeField]
	private Renderer mirror;

	[SerializeField]
	private Renderer dummy;

	private bool show = true;

	private void Awake()
	{
		ShowMirror(ConfigData.showMirror);
	}

	private void Update()
	{
		if (show != ConfigData.showMirror)
		{
			ShowMirror(ConfigData.showMirror);
		}
	}

	public void ShowMirror(bool flag)
	{
		show = flag;
		mirror.enabled = show;
		dummy.enabled = !show;
	}
}
