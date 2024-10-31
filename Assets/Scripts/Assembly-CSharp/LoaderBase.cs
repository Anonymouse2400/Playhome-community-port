using System;
using UnityEngine;

public class LoaderBase : MonoBehaviour
{
	[SerializeField]
	protected NetWWW netWWW;

	[SerializeField]
	protected UpDown upDown;

	protected void PostError(WWW www, string msg)
	{
		upDown.PostError(www, msg);
	}

	public void ShowMessage(string message)
	{
		upDown.ShowMessage(message);
	}
}
