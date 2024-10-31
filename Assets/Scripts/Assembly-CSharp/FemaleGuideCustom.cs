using System;
using System.Collections.Generic;
using UnityEngine;

public class FemaleGuideCustom : MonoBehaviour
{
	[Serializable]
	public class MoveParam
	{
		public enum DRIVE
		{
			POS_X = 0,
			POS_Y = 1,
			POS_Z = 2,
			ROT_X = 3,
			ROT_Y = 4,
			ROT_Z = 5,
			SCL_X = 6,
			SCL_Y = 7,
			SCL_Z = 8
		}

		public enum TYPE
		{
			POS = 0,
			ROT = 1,
			SCL = 2
		}

		public int shapeID = -1;

		public DRIVE drive;

		public float val;

		public float Rate { get; private set; }

		public TYPE Type { get; private set; }

		public void Setup()
		{
			CalcRate();
			if (drive == DRIVE.POS_X || drive == DRIVE.POS_Y || drive == DRIVE.POS_Z)
			{
				Type = TYPE.POS;
			}
			if (drive == DRIVE.ROT_X || drive == DRIVE.ROT_Y || drive == DRIVE.ROT_Z)
			{
				Type = TYPE.ROT;
			}
			if (drive == DRIVE.SCL_X || drive == DRIVE.SCL_Y || drive == DRIVE.SCL_Z)
			{
				Type = TYPE.SCL;
			}
		}

		private void CalcRate()
		{
			if (val != 0f)
			{
				Rate = 1f / val;
			}
			else
			{
				Rate = 0f;
			}
		}

		public void CustomMovePos(CharaShapeCustomBase custom, Vector3 move)
		{
			float num = move[(int)drive % 3];
			float value = custom.GetShape(shapeID) + num * Rate;
			custom.SetShape(shapeID, Mathf.Clamp01(value));
		}

		public void CustomMoveRot(CharaShapeCustomBase custom, Vector3 move)
		{
			float target = move[(int)drive % 3];
			float value = custom.GetShape(shapeID) + Mathf.DeltaAngle(0f, target) * Rate;
			custom.SetShape(shapeID, Mathf.Clamp01(value));
		}

		public void CustomMoveScl(CharaShapeCustomBase custom, Vector3 move)
		{
			float num = move[(int)drive % 3];
			float value = custom.GetShape(shapeID) + num * Rate;
			custom.SetShape(shapeID, Mathf.Clamp01(value));
		}
	}

	[Serializable]
	public class MovePoint
	{
		public string transformName = string.Empty;

		public MoveParam[] moveParams;

		private Transform trans;

		private GuideDriveManager drive;

		private CharaShapeCustomBase custom;

		private List<MoveParam> params_pos = new List<MoveParam>();

		private List<MoveParam> params_rot = new List<MoveParam>();

		private List<MoveParam> params_scl = new List<MoveParam>();

		public void Setup(GuidesManager guidesManager, CharaShapeCustomBase custom, Transform root)
		{
			MoveParam[] array = moveParams;
			foreach (MoveParam moveParam in array)
			{
				moveParam.Setup();
			}
			this.custom = custom;
			trans = Transform_Utility.FindTransform(root, transformName);
			if (trans != null)
			{
				drive = guidesManager.Add(trans, false);
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				MoveParam[] array2 = moveParams;
				foreach (MoveParam moveParam2 in array2)
				{
					if (moveParam2.Type == MoveParam.TYPE.POS)
					{
						params_pos.Add(moveParam2);
						flag = true;
					}
					if (moveParam2.Type == MoveParam.TYPE.ROT)
					{
						params_rot.Add(moveParam2);
						flag2 = true;
					}
					if (moveParam2.Type == MoveParam.TYPE.SCL)
					{
						params_scl.Add(moveParam2);
						flag3 = true;
					}
				}
				bool pos = drive.enablePos;
				bool rot = drive.enableRot;
				bool scl = drive.enableScl;
				if (flag)
				{
					drive.delegateMovePos = DelegateMovePos;
				}
				else
				{
					pos = false;
				}
				if (flag2)
				{
					drive.delegateMoveRot = DelegateMoveRot;
				}
				else
				{
					rot = false;
				}
				if (flag3)
				{
					drive.delegateMoveScl = DelegateMoveScl;
				}
				else
				{
					scl = false;
				}
				drive.SetEnables(pos, rot, scl);
			}
			else
			{
				Debug.LogError("参照ボーンなし：" + transformName);
			}
		}

		private void DelegateMovePos(Vector3 move)
		{
			foreach (MoveParam params_po in params_pos)
			{
				params_po.CustomMovePos(custom, move);
			}
		}

		private void DelegateMoveRot(Quaternion move)
		{
			foreach (MoveParam item in params_rot)
			{
				item.CustomMoveRot(custom, move.eulerAngles);
			}
		}

		private void DelegateMoveScl(Vector3 move)
		{
			foreach (MoveParam item in params_scl)
			{
				item.CustomMoveScl(custom, move);
			}
		}
	}

	public Female female;

	public GuidesManager guidesManager;

	public MovePoint[] facePoints;

	public MovePoint[] bodyPoints;

	private bool setuped;

	private void Start()
	{
	}

	private void Update()
	{
		if (!setuped)
		{
			Setup();
			setuped = true;
		}
	}

	private void Setup()
	{
		MovePoint[] array = facePoints;
		foreach (MovePoint movePoint in array)
		{
			movePoint.Setup(guidesManager, female.head, female.transform);
		}
		MovePoint[] array2 = bodyPoints;
		foreach (MovePoint movePoint2 in array2)
		{
			movePoint2.Setup(guidesManager, female.body, female.transform);
		}
	}
}
