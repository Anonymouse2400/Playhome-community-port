using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_XTC_W_In : H_State
	{
		private bool ejaculated;

		private float ejaculationDelay = 0.5f;

		private float timer;

		public H_State_XTC_W_In(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.param.continuanceXTC_F++;
			members.AddCountXTC();
			CheckChangePersonalityState();
			members.PlayAnime("XTC_W_In", 0.2f);
			members.VoiceExpression(H_Voice.TYPE.XTC_F);
			members.MaleExpression(H_Expression_Male.TYPE.EJACULATION);
			ejaculated = false;
		}

		public override void Main()
		{
			if (!ejaculated && timer >= ejaculationDelay)
			{
				Ejaculation();
			}
			else
			{
				timer += Time.deltaTime;
			}
			if (members.CheckEndAnime("XTC_W_In"))
			{
				if (!ejaculated)
				{
					Ejaculation();
				}
				CheckUrine();
				members.FemaleGageVal = members.FemaleGageStartVal();
				members.MaleGageVal = 0f;
				ChangeState(H_STATE.XTC_W_TREMBLE, null);
			}
		}

		public override void Out()
		{
		}

		private void Ejaculation()
		{
			List<Male> males = members.GetMales();
			int noInsMale = members.StyleData.noInsMale;
			for (int i = 0; i < males.Count; i++)
			{
				if ((noInsMale & (1 << i)) != 0)
				{
					males[i].Ejaculation(members.param.manyEjaculation);
				}
			}
			members.MaleGageVal = 0f;
			ejaculated = true;
		}
	}
}
