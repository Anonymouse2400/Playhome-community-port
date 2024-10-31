using System;
using UnityEngine;

public class H_OptionUISE : MonoBehaviour
{
	public void ChangeRightupToggles(int no)
	{
		SystemSE.SE se = ((no == -1) ? SystemSE.SE.CLOSE : SystemSE.SE.OPEN);
		SystemSE.Play(se);
	}
}
