namespace H
{
	internal class H_State_Vomit : H_State
	{
		private float vomitDelay = 1f;

		private bool vomited;

		private float vomitTimer;

		public H_State_Vomit(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.PlayAnime("Vomit", 0.2f);
			members.param.detail |= H_Parameter.DETAIL.VOMIT;
			members.param.mouth = H_MOUTH.FREE;
			members.VoiceExpression(H_Voice.TYPE.VOMIT);
			vomited = false;
			vomitTimer = 0f;
		}

		public override void Main()
		{
			vomitTimer += 1f;
			if (!vomited && vomitTimer >= vomitDelay)
			{
				Vomit();
			}
			if (members.CheckEndVoice())
			{
				if (!vomited)
				{
					Vomit();
				}
				ChangeState(H_STATE.VOMIT_WAIT);
			}
		}

		public override void Out()
		{
		}

		private void Vomit()
		{
			Female female = members.GetFemale(0);
			female.dripParticleMouth.Play();
			vomited = true;
		}
	}
}
