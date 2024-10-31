using System;
using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_InEjaIn : H_State
	{
		private bool ejaculated;

		private float ejaculationDelay = 0.5f;

		private float timer;

		public H_State_InEjaIn(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.param.continuanceXTC_F = 0;
			members.AddCountXTC();
			members.param.manyEjaculation = members.MaleGageVal >= 1f;
			members.PlayAnime("InEja_In", 0.2f);
			members.VoiceExpression(H_Voice.TYPE.XTC_M);
			members.MaleExpression(H_Expression_Male.TYPE.EJACULATION);
			ejaculated = false;
			timer = 0f;
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
			if (members.CheckEndAnime("InEja_In"))
			{
				if (!ejaculated)
				{
					Ejaculation();
				}
				ChangeState(H_STATE.IN_EJA_TREMBLE, null);
			}
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

		public override void Out()
		{
		}
	}
}
