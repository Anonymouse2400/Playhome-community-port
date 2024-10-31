namespace H
{
	internal class H_State_OutXtcAfterWait : H_State_AfterBase
	{
		public H_State_OutXtcAfterWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			base.In(msg);
			members.PlayAnime("OutEja_Base", 0.5f);
		}

		public override void Main()
		{
			UpdateVoice();
		}

		public override void OnInput(H_INPUT input)
		{
			OnInput_Extracted(input);
		}
	}
}
