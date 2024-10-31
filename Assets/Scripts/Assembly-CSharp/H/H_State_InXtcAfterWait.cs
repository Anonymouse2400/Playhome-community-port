namespace H
{
	internal class H_State_InXtcAfterWait : H_State_AfterBase
	{
		public H_State_InXtcAfterWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			base.In(msg);
			members.PlayAnime("XTC_After", 0.5f);
		}

		public override void Main()
		{
			UpdateVoice();
		}

		public override void OnInput(H_INPUT input)
		{
			OnInput_Inserted(input);
		}
	}
}
