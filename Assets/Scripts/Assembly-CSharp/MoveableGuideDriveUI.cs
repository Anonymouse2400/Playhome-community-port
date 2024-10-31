using System;
using UnityEngine;
using UnityEngine.Events;

public class MoveableGuideDriveUI : MonoBehaviour
{
	[SerializeField]
	private MoveableUI moveable;

	[SerializeField]
	private GuideDriveUI guideDrive;

	public bool isShow
	{
		get
		{
			return moveable.isActiveAndEnabled;
		}
	}

	public bool isOpen
	{
		get
		{
			return moveable.State == MoveableUI.STATE.OPEN;
		}
	}

	public void Setup(string str, Vector3 pos, Vector3 eul, Vector3 scl, UnityAction<MoveableUI.STATE> moveableStateOnChange, UnityAction<Vector3, Vector3, Vector3> onMove)
	{
		moveable.SetTitle(str);
		moveable.AddOnChange(moveableStateOnChange);
		guideDrive.Setup(pos, eul, scl, onMove);
	}

	public void SetLimit(bool isLimitPos, Vector3 limitPosMin, Vector3 limitPosMax, bool isLimitScl, Vector3 limitSclMin, Vector3 limitSclMax)
	{
		guideDrive.SetLimit(isLimitPos, limitPosMin, limitPosMax, isLimitScl, limitSclMin, limitSclMax);
	}

	public void SetUnlimit()
	{
		guideDrive.SetUnlimit();
	}

	public void Open()
	{
		moveable.Open();
	}

	public void Close()
	{
		moveable.Close();
	}
}
