namespace H
{
	internal class H_State_VomitWait : H_State_AfterBase
	{
		public H_State_VomitWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			base.In(msg);
			members.PlayAnime("VomitWait", 0.5f);
		}

		public override void Main()
		{
			UpdateVoice();
		}

		public override void Out()
		{
			if ((members.param.detail & H_Parameter.DETAIL.VOMIT) != 0)
			{
				members.param.detail ^= H_Parameter.DETAIL.VOMIT;
			}
		}

		public override void OnInput(H_INPUT input)
		{
			OnInput_Extracted(input);
		}
	}
}
