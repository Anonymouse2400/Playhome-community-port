namespace H
{
	internal class H_State_XTC_F_In : H_State
	{
		public H_State_XTC_F_In(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.param.continuanceXTC_F++;
			members.AddCountXTC();
			CheckChangePersonalityState();
			members.PlayAnime("XTC_F_In", 0.2f);
			members.VoiceExpression(H_Voice.TYPE.XTC_F);
		}

		public override void Main()
		{
			if (members.CheckEndAnime("XTC_F_In"))
			{
				CheckUrine();
				members.FemaleGageVal = members.FemaleGageStartVal();
				ChangeState(H_STATE.XTC_F_TREMBLE, null);
			}
		}

		public override void Out()
		{
		}
	}
}
