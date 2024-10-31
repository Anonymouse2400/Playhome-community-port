using System;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class IK_Control
{
	private FullBodyBipedIK fbik;

	private Transform tinRoot;

	private Transform tinTip;

	private Transform tinTarget;

	private Transform mouthTrans;

	private Transform mouthTarget;

	private float mouthRate;

	private float mouthSpeed;

	private bool mouthGoal;

	private List<IKBlendData> blends = new List<IKBlendData>();

	public FullBodyBipedIK FBIK
	{
		get
		{
			return fbik;
		}
	}

	public bool TinEnable { get; set; }

	public bool MouthEnable { get; set; }

	public IK_Control(FullBodyBipedIK fbik, Transform tinRoot, Transform tinTip, Transform mouthTrans)
	{
		this.fbik = fbik;
		this.tinRoot = tinRoot;
		this.tinTip = tinTip;
		this.mouthTrans = mouthTrans;
		fbik.solver.leftShoulderEffector.target = null;
		fbik.solver.rightShoulderEffector.target = null;
	}

	public void Update()
	{
		UpdateBlends();
	}

	private void UpdateBlends()
	{
		for (int i = 0; i < blends.Count; i++)
		{
			if (blends[i].Update(Time.deltaTime))
			{
				blends.RemoveAt(i);
				i--;
			}
		}
	}

	public void ClearIK()
	{
		fbik.solver.leftHandEffector.positionWeight = 0f;
		fbik.solver.leftHandEffector.rotationWeight = 0f;
		fbik.solver.leftArmChain.bendConstraint.weight = 0f;
		fbik.solver.rightHandEffector.positionWeight = 0f;
		fbik.solver.rightHandEffector.rotationWeight = 0f;
		fbik.solver.rightArmChain.bendConstraint.weight = 0f;
		fbik.solver.leftFootEffector.positionWeight = 0f;
		fbik.solver.leftFootEffector.rotationWeight = 0f;
		fbik.solver.leftLegChain.bendConstraint.weight = 0f;
		fbik.solver.rightFootEffector.positionWeight = 0f;
		fbik.solver.rightFootEffector.rotationWeight = 0f;
		fbik.solver.rightLegChain.bendConstraint.weight = 0f;
		fbik.solver.leftShoulderEffector.positionWeight = 0f;
		fbik.solver.rightShoulderEffector.positionWeight = 0f;
		fbik.solver.bodyEffector.positionWeight = 0f;
		tinTarget = null;
		mouthTarget = null;
		blends.Clear();
	}

	public void ClearIK(IK_Data.PART part)
	{
		if (part == IK_Data.PART.HAND_L)
		{
			fbik.solver.leftHandEffector.positionWeight = 0f;
			fbik.solver.leftHandEffector.rotationWeight = 0f;
			fbik.solver.leftArmChain.bendConstraint.weight = 0f;
		}
		if (part == IK_Data.PART.HAND_R)
		{
			fbik.solver.rightHandEffector.positionWeight = 0f;
			fbik.solver.rightHandEffector.rotationWeight = 0f;
			fbik.solver.rightArmChain.bendConstraint.weight = 0f;
		}
		if (part == IK_Data.PART.FOOT_L)
		{
			fbik.solver.leftFootEffector.positionWeight = 0f;
			fbik.solver.leftFootEffector.rotationWeight = 0f;
			fbik.solver.leftLegChain.bendConstraint.weight = 0f;
		}
		if (part == IK_Data.PART.FOOT_R)
		{
			fbik.solver.rightFootEffector.positionWeight = 0f;
			fbik.solver.rightFootEffector.rotationWeight = 0f;
			fbik.solver.rightLegChain.bendConstraint.weight = 0f;
		}
		if (part == IK_Data.PART.TIN)
		{
			tinTarget = null;
		}
	}

	public void SetIK(IK_Data.PART part, Transform target, float speed = 0f)
	{
		switch (part)
		{
		case IK_Data.PART.TIN:
			tinTarget = target;
			return;
		case IK_Data.PART.MOUTH:
			mouthTarget = target;
			mouthSpeed = speed;
			if (speed <= 0f)
			{
				mouthRate = 1f;
			}
			return;
		}
		IKEffector effector = GetEffector(part);
		effector.target = target;
		FBIKChain chain = GetChain(part);
		if (speed <= 0f)
		{
			effector.positionWeight = 1f;
			effector.rotationWeight = 1f;
			chain.bendConstraint.weight = 1f;
		}
		else
		{
			AddBlend(effector, chain, speed);
		}
	}

	private void AddBlend(IKEffector effector, FBIKChain chain, float speed)
	{
		int num = -1;
		for (int i = 0; i < blends.Count; i++)
		{
			if (blends[i].Check(effector, chain))
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			blends.Add(new IKBlendData(effector, chain, speed));
		}
		else
		{
			blends[num] = new IKBlendData(effector, chain, speed);
		}
	}

	private IKEffector GetEffector(IK_Data.PART part)
	{
		IKEffector[] array = new IKEffector[5]
		{
			fbik.solver.leftHandEffector,
			fbik.solver.rightHandEffector,
			fbik.solver.leftFootEffector,
			fbik.solver.rightFootEffector,
			null
		};
		return array[(int)part];
	}

	private FBIKChain GetChain(IK_Data.PART part)
	{
		FBIKChain[] array = new FBIKChain[5]
		{
			fbik.solver.leftArmChain,
			fbik.solver.rightArmChain,
			fbik.solver.leftLegChain,
			fbik.solver.rightLegChain,
			null
		};
		return array[(int)part];
	}

	public void CalcTin()
	{
		if (!(tinRoot == null) && !(tinTarget == null) && TinEnable)
		{
			Vector3 v = tinRoot.parent.InverseTransformPoint(tinTarget.position);
			float yaw;
			float pitch;
			VectorUtility.Vector3_ToYawPitch(v, out yaw, out pitch);
			tinRoot.localRotation = Quaternion.Euler(pitch, yaw, 0f);
			tinTip.position = tinTarget.position;
		}
	}

	public void SetMouthTrans(Transform mouth)
	{
		mouthTrans = mouth;
	}

	public void CalcMouth()
	{
		if (!(mouthTrans == null) && !(mouthTarget == null))
		{
			if (mouthRate < 1f)
			{
				mouthRate += Time.deltaTime * mouthSpeed;
				mouthRate = Mathf.Min(mouthRate, 1f);
			}
			float num = mouthRate;
			float num2 = 1f;
			float num3 = 1f;
			fbik.solver.leftShoulderEffector.target = null;
			fbik.solver.rightShoulderEffector.target = null;
			fbik.solver.leftShoulderEffector.positionWeight = num;
			fbik.solver.rightShoulderEffector.positionWeight = num;
			fbik.solver.bodyEffector.positionWeight = num;
			InverseTransformEffector(FullBodyBipedEffector.LeftShoulder, mouthTrans, mouthTarget.position, num);
			InverseTransformEffector(FullBodyBipedEffector.RightShoulder, mouthTrans, mouthTarget.position, num);
			InverseTransformEffector(FullBodyBipedEffector.Body, mouthTrans, mouthTarget.position, num);
			IKEffector bodyEffector = fbik.solver.bodyEffector;
			bodyEffector.position = Vector3.Lerp(new Vector3(bodyEffector.position.x, bodyEffector.bone.position.y, bodyEffector.position.z), bodyEffector.position, num2 * num);
			bodyEffector.position = Vector3.Lerp(new Vector3(bodyEffector.bone.position.x, bodyEffector.position.y, bodyEffector.bone.position.z), bodyEffector.position, num3 * num);
		}
	}

	private void InverseTransformEffector(FullBodyBipedEffector effector, Transform target, Vector3 targetPosition, float weight)
	{
		Vector3 vector = fbik.solver.GetEffector(effector).bone.position - target.position;
		fbik.solver.GetEffector(effector).position = Vector3.Lerp(fbik.solver.GetEffector(effector).bone.position, targetPosition + vector, weight);
	}
}
