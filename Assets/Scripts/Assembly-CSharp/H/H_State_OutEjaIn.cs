using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_OutEjaIn : H_State
	{
		private bool ejaculated;

		private float ejaculationDelay = 0.5f;

		private float timer;

		public H_State_OutEjaIn(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.param.continuanceXTC_F = 0;
			members.AddCountXTC();
			members.param.manyEjaculation = members.MaleGageVal >= 1f;
			members.PlayAnime("OutEja_In", 0.2f);
			members.param.mouth = H_MOUTH.FREE;
			members.EnableTinIK = false;
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
			if (members.CheckEndAnime("OutEja_In"))
			{
				if (!ejaculated)
				{
					Ejaculation();
				}
				ChangeState(H_STATE.OUT_EJA_TREMBLE, null);
			}
		}

		private void Ejaculation()
		{
			List<Male> males = members.GetMales();
			for (int i = 0; i < males.Count; i++)
			{
				males[i].Ejaculation(members.param.manyEjaculation);
			}
			members.MaleGageVal = 0f;
			ejaculated = true;
		}

		public override void Out()
		{
		}
	}
}
