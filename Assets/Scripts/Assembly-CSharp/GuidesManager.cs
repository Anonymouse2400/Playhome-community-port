using System;
using System.Collections.Generic;
using UnityEngine;

public class GuidesManager : MonoBehaviour
{
	[SerializeField]
	private GuideDriveManager original;

	private List<GuideDriveManager> guides = new List<GuideDriveManager>();

	[SerializeField]
	private IllusionCamera illusionCamera;

	private Action<GuideDriveManager> onChangeActive;

	private Action<Transform> onChangeMove;

	public GuideDriveManager activeGuide { get; set; }

	private void Start()
	{
		GuideDriveManager[] componentsInChildren = base.gameObject.GetComponentsInChildren<GuideDriveManager>();
		GuideDriveManager[] array = componentsInChildren;
		foreach (GuideDriveManager guideDriveManager in array)
		{
			guideDriveManager.SetManager(this);
			guides.Add(guideDriveManager);
		}
	}

	private void Update()
	{
		float num = float.MaxValue;
		GuideDriveManager guideDriveManager = null;
		GuideDrive guideDrive = null;
		Vector3 clickPos = Vector3.zero;
		foreach (GuideDriveManager guide in guides)
		{
			GuideDriveManager.CursorCheckData cursorCheckData = guide.CursorCheck();
			if (cursorCheckData.distance < num)
			{
				num = cursorCheckData.distance;
				guideDriveManager = guide;
				guideDrive = cursorCheckData.drive;
				clickPos = cursorCheckData.pos;
			}
		}
		foreach (GuideDriveManager guide2 in guides)
		{
			if (guide2 == guideDriveManager)
			{
				guide2.SetOnCursor(guideDrive);
			}
			else
			{
				guide2.SetOutCursor();
			}
		}
		if (Input.GetMouseButtonDown(0) && guideDriveManager != null)
		{
			if (guideDriveManager.IsActive && guideDriveManager.MovingDrive == null)
			{
				guideDrive.OnMoveStart(clickPos);
			}
			else
			{
				guideDriveManager.IsActive = true;
			}
		}
		illusionCamera.enabled = activeGuide == null;
	}

	public void Add(GuideDriveManager guide, bool setParent)
	{
		guide.SetManager(this);
		if (setParent)
		{
			guide.transform.SetParent(base.transform, true);
		}
		guides.Add(guide);
	}

	public GuideDriveManager Add(Transform target, bool guideMoveInitiative = true)
	{
		GuideDriveManager guideDriveManager = UnityEngine.Object.Instantiate(original);
		guideDriveManager.gameObject.SetActive(true);
		guideDriveManager.MoveInitiative = guideMoveInitiative;
		guideDriveManager.SetTarget(target);
		Add(guideDriveManager, true);
		return guideDriveManager;
	}

	public void Del(GuideDriveManager guide)
	{
		Remove(guide);
		UnityEngine.Object.Destroy(guide.gameObject);
	}

	public void Del(Transform target)
	{
		foreach (GuideDriveManager guide in guides)
		{
			if (guide.Target == target)
			{
				Remove(guide);
				UnityEngine.Object.Destroy(guide.gameObject);
				break;
			}
		}
	}

	public void Remove(GuideDriveManager guide)
	{
		if (guide.IsActive && onChangeActive != null)
		{
			onChangeActive(null);
		}
		guides.Remove(guide);
	}

	public void Activate(GuideDriveManager guide)
	{
		foreach (GuideDriveManager guide2 in guides)
		{
			if (guide2 != guide)
			{
				guide2.IsActive = false;
			}
		}
		guide.IsActive = true;
		if (onChangeActive != null)
		{
			onChangeActive(guide);
		}
	}

	public void Activate(Transform target)
	{
		GuideDriveManager guideDriveManager = null;
		foreach (GuideDriveManager guide in guides)
		{
			if (guide.Target == target)
			{
				guideDriveManager = guide;
			}
			else
			{
				guide.IsActive = false;
			}
		}
		guideDriveManager.IsActive = true;
		if (onChangeActive != null)
		{
			onChangeActive(guideDriveManager);
		}
	}

	public void Set_OnChangeActions(Action<GuideDriveManager> onChangeActive, Action<Transform> onChangeMove)
	{
		this.onChangeActive = onChangeActive;
		this.onChangeMove = onChangeMove;
	}

	public void OnChangeTransform(Transform transform)
	{
		if (onChangeMove != null)
		{
			onChangeMove(transform);
		}
	}
}
