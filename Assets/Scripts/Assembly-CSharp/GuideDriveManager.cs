using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuideDriveManager : MonoBehaviour
{
	public struct CursorCheckData
	{
		public GuideDrive drive;

		public float distance;

		public Vector3 pos;
	}

	public delegate void DelegateMovePos(Vector3 move);

	public delegate void DelegateMoveRot(Quaternion move);

	public delegate void DelegateMoveScl(Vector3 move);

	public delegate void DelegateSetTrans(Vector3 pos, Quaternion rot, Vector3 scl);

	private GuideDrive[] drives;

	[SerializeField]
	private float checkDis = 100f;

	[SerializeField]
	private float scaleRate = 1f;

	[SerializeField]
	private GuideDrive_Inactive sphere;

	public Transform posRoot;

	public Transform rotRoot;

	public Transform sclRoot;

	[SerializeField]
	private GuideDrive_Pos[] drivePos;

	[SerializeField]
	private GuideDrive_Pos[] drivePlane;

	[SerializeField]
	private GuideDrive_Rot[] driveRot;

	[SerializeField]
	private GuideDrive_Scl[] driveScl;

	private Transform target;

	private GuideDrive movingDrive;

	private GuidesManager manager;

	private bool isActive;

	public DelegateMovePos delegateMovePos;

	public DelegateMoveRot delegateMoveRot;

	public DelegateMoveScl delegateMoveScl;

	public DelegateSetTrans delegateSetTrans;

	public bool MoveInitiative = true;

	public bool enablePos { get; private set; }

	public bool enableRot { get; private set; }

	public bool enableScl { get; private set; }

	public Transform Target
	{
		get
		{
			return target;
		}
	}

	public bool IsActive
	{
		get
		{
			return isActive;
		}
		set
		{
			if (isActive != value)
			{
				isActive = value;
				ChangeActiveObj();
			}
		}
	}

	public GuideDrive MovingDrive
	{
		get
		{
			return movingDrive;
		}
	}

	private void Awake()
	{
		drives = GetComponentsInChildren<GuideDrive>();
		isActive = false;
	}

	public void SetManager(GuidesManager manager)
	{
		this.manager = manager;
	}

	private void Start()
	{
		ChangeActiveObj();
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		Update_PlanePos();
		ScaleCalc();
		if (movingDrive != null)
		{
			InvokeTransform();
		}
		if (!MoveInitiative)
		{
			base.transform.position = target.position;
			rotRoot.rotation = target.transform.rotation;
		}
	}

	public CursorCheckData CursorCheck()
	{
		CursorCheckData result = default(CursorCheckData);
		result.distance = float.MaxValue;
		result.drive = null;
		result.pos = Vector3.zero;
		if ((bool)EventSystem.current && EventSystem.current.IsPointerOverGameObject())
		{
			return result;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float num = float.MaxValue;
		RaycastHit hitInfo;
		if (IsActive)
		{
			if (movingDrive == null)
			{
				GuideDrive guideDrive = null;
				Vector3 pos = Vector3.zero;
				GuideDrive[] array = drives;
				foreach (GuideDrive guideDrive2 in array)
				{
					if (guideDrive2.GetComponent<Collider>().Raycast(ray, out hitInfo, checkDis) && hitInfo.distance < num)
					{
						guideDrive = guideDrive2;
						num = hitInfo.distance;
						pos = hitInfo.point;
					}
				}
				if (guideDrive != null)
				{
					result.distance = num;
					result.drive = guideDrive;
					result.pos = pos;
					return result;
				}
			}
		}
		else if (sphere.GetComponent<Collider>().Raycast(ray, out hitInfo, checkDis))
		{
			result.distance = hitInfo.distance;
			result.drive = sphere;
			result.pos = hitInfo.point;
			return result;
		}
		return result;
	}

	public void SetOutCursor()
	{
		sphere.OnCursor = false;
		GuideDrive[] array = drives;
		foreach (GuideDrive guideDrive in array)
		{
			guideDrive.OnCursor = false;
		}
	}

	public void SetOnCursor(GuideDrive drive)
	{
		SetOutCursor();
		drive.OnCursor = true;
	}

	private void InvokeTransform()
	{
		if (manager != null)
		{
			if (target != null)
			{
				manager.OnChangeTransform(target.transform);
			}
			else
			{
				manager.OnChangeTransform(rotRoot.transform);
			}
		}
	}

	public void OnMoveStart(GuideDrive drive)
	{
		movingDrive = drive;
		manager.activeGuide = this;
	}

	public void OnMoveEnd(GuideDrive drive)
	{
		manager.activeGuide = null;
		movingDrive = null;
	}

	public void SetTrans(Vector3 pos, Quaternion rot, Vector3 scl, bool invoke)
	{
		base.transform.position = pos;
		rotRoot.rotation = rot;
		sclRoot.rotation = rot;
		if (MoveInitiative && target != null)
		{
			target.transform.position = pos;
			target.transform.rotation = rot;
			target.transform.localScale = scl;
		}
		if (invoke && delegateSetTrans != null)
		{
			delegateSetTrans(pos, rot, scl);
		}
	}

	public void SetLocalTrans(Vector3 pos, Quaternion rot, Vector3 scl, bool invoke)
	{
		bool flag = false;
		if (MoveInitiative && target != null)
		{
			target.transform.localPosition = pos;
			target.transform.localRotation = rot;
			target.transform.localScale = scl;
			flag = true;
		}
		if (flag)
		{
			base.transform.position = target.transform.position;
			base.transform.localRotation = Quaternion.identity;
			rotRoot.rotation = target.transform.rotation;
			sclRoot.rotation = target.transform.rotation;
		}
		else
		{
			base.transform.localPosition = pos;
			base.transform.localRotation = Quaternion.identity;
			rotRoot.localRotation = rot;
			sclRoot.localRotation = rot;
		}
		if (invoke && delegateSetTrans != null)
		{
			delegateSetTrans(base.transform.position, rotRoot.rotation, scl);
		}
	}

	public void SetTrans(Transform trans, bool invoke)
	{
		SetTrans(trans.position, trans.rotation, trans.localScale, invoke);
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
		SetTrans(target.transform, false);
	}

	private void ChangeActiveObj()
	{
		ChangeEnable();
		bool flag = enablePos | enableRot | enableScl;
		sphere.gameObject.SetActive(!isActive && flag);
		if (isActive)
		{
			manager.Activate(this);
			InvokeTransform();
		}
	}

	private void ChangeEnable()
	{
		GuideDrive_Pos[] array = drivePos;
		foreach (GuideDrive_Pos guideDrive_Pos in array)
		{
			guideDrive_Pos.gameObject.SetActive(isActive && enablePos);
		}
		GuideDrive_Pos[] array2 = drivePlane;
		foreach (GuideDrive_Pos guideDrive_Pos2 in array2)
		{
			guideDrive_Pos2.gameObject.SetActive(isActive && enablePos);
		}
		GuideDrive_Rot[] array3 = driveRot;
		foreach (GuideDrive_Rot guideDrive_Rot in array3)
		{
			guideDrive_Rot.gameObject.SetActive(isActive && enableRot);
		}
		GuideDrive_Scl[] array4 = driveScl;
		foreach (GuideDrive_Scl guideDrive_Scl in array4)
		{
			guideDrive_Scl.gameObject.SetActive(isActive && enableScl);
		}
	}

	public void DriveMovePosition(Vector3 move)
	{
		if (delegateMovePos != null)
		{
			delegateMovePos(move);
		}
		if (MoveInitiative)
		{
			base.transform.position += move;
			if (target != null)
			{
				target.position = base.transform.position;
			}
		}
	}

	public void DriveMoveRotation(Quaternion move)
	{
		if (delegateMoveRot != null)
		{
			delegateMoveRot(move);
		}
		if (MoveInitiative)
		{
			rotRoot.rotation *= move;
			sclRoot.rotation *= move;
			if (target != null)
			{
				target.rotation = rotRoot.rotation;
			}
		}
	}

	public void DriveMoveScale(Vector3 move)
	{
		if (delegateMoveScl != null)
		{
			delegateMoveScl(move);
		}
		if (MoveInitiative && target != null)
		{
			target.localScale += move;
		}
	}

	private void ScaleCalc()
	{
		float num = Vector3.Distance(base.transform.position, Camera.main.transform.position);
		float num2 = num * scaleRate;
		base.transform.localScale = new Vector3(num2, num2, num2);
	}

	public void SetEnables(bool pos, bool rot, bool scl)
	{
		enablePos = pos;
		enableRot = rot;
		enableScl = scl;
		ChangeEnable();
	}

	public void Update_PlanePos()
	{
		float num = 0.05f;
		Vector3 vector = Camera.main.transform.position - base.transform.position;
		GuideDrive_Pos[] array = drivePlane;
		foreach (GuideDrive_Pos guideDrive_Pos in array)
		{
			Vector3 localPosition = guideDrive_Pos.transform.localPosition;
			if (guideDrive_Pos.moveType == GuideDrive_Pos.MoveType.XY)
			{
				localPosition.x = ((!(vector.x >= 0f)) ? (0f - num) : num);
				localPosition.y = ((!(vector.y >= 0f)) ? (0f - num) : num);
			}
			else if (guideDrive_Pos.moveType == GuideDrive_Pos.MoveType.XZ)
			{
				localPosition.x = ((!(vector.x >= 0f)) ? (0f - num) : num);
				localPosition.z = ((!(vector.z >= 0f)) ? (0f - num) : num);
			}
			else if (guideDrive_Pos.moveType == GuideDrive_Pos.MoveType.YZ)
			{
				localPosition.y = ((!(vector.y >= 0f)) ? (0f - num) : num);
				localPosition.z = ((!(vector.z >= 0f)) ? (0f - num) : num);
			}
			guideDrive_Pos.transform.localPosition = localPosition;
		}
	}

	public void SetDelegate(DelegateMovePos pos, DelegateMoveRot rot, DelegateMoveScl scl, DelegateSetTrans trans)
	{
		delegateMovePos = pos;
		delegateMoveRot = rot;
		delegateMoveScl = scl;
		delegateSetTrans = trans;
	}

	public void SetSphereColor(Color color)
	{
		sphere.SetBaseColor(color);
	}
}
