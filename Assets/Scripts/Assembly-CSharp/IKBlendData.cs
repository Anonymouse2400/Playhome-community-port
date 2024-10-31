using System;
using RootMotion.FinalIK;

internal class IKBlendData
{
	private IKEffector effector;

	private FBIKChain chain;

	private float speed;

	private float rate;

	public IKBlendData(IKEffector effector, FBIKChain chain, float speed)
	{
		this.effector = effector;
		this.chain = chain;
		this.speed = speed;
		rate = 0f;
		SetRate();
	}

	public bool Update(float time)
	{
		bool result = false;
		rate += time * speed;
		if (rate >= 1f)
		{
			rate = 1f;
			result = true;
		}
		SetRate();
		return result;
	}

	private void SetRate()
	{
		effector.positionWeight = rate;
		effector.rotationWeight = rate;
		chain.bendConstraint.weight = rate;
	}

	public bool Check(IKEffector effector, FBIKChain chain)
	{
		return this.effector == effector && this.chain == chain;
	}
}
