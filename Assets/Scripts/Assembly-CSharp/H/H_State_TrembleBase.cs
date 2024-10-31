using UnityEngine;

namespace H
{
	public abstract class H_State_TrembleBase : H_State
	{
		protected enum TREMBLE
		{
			A = 0,
			B = 1,
			NUM = 2
		}

		protected float next_timer;

		protected TREMBLE lastTremble;

		protected bool nowTrembling;

		protected int trembleCount;

		public H_State_TrembleBase(H_Members members)
			: base(members)
		{
		}

		protected abstract string GetAnimeName_Base();

		protected abstract string GetAnimeName_TrembleA();

		protected abstract string GetAnimeName_TrembleB();

		protected abstract H_STATE GetNextState();

		protected string GetAnimeName_Tremble(TREMBLE tremble)
		{
			return (tremble != 0) ? GetAnimeName_TrembleB() : GetAnimeName_TrembleA();
		}

		public override void In(Message msg)
		{
			trembleCount = 0;
			TrembleAnime(true);
		}

		public override void Main()
		{
			next_timer -= Time.deltaTime;
			if (next_timer <= 0f)
			{
				TrembleAnime(false);
			}
			if (members.StyleData.type == H_StyleData.TYPE.SERVICE)
			{
				if (trembleCount >= 5)
				{
					ChangeState(GetNextState(), null);
				}
			}
			else if (members.CheckEndVoice())
			{
				ChangeState(GetNextState(), null);
			}
		}

		public override void Out()
		{
		}

		protected void TrembleAnime(bool isFirst)
		{
			TREMBLE tremble;
			if (isFirst)
			{
				tremble = (TREMBLE)Random.Range(0, 2);
			}
			else
			{
				int num = (int)(lastTremble + 1);
				int num2 = 2;
				tremble = (TREMBLE)(num % num2);
			}
			members.PlayAnime(GetAnimeName_Tremble(tremble), 0.2f);
			lastTremble = tremble;
			next_timer = (float)trembleCount * 0.5f;
			trembleCount++;
			Debug.Log("ビクッ");
		}
	}
}
