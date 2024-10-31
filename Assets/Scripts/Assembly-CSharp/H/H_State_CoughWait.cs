namespace H
{
	internal class H_State_CoughWait : H_State_AfterBase
	{
		public H_State_CoughWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			base.In(msg);
			members.PlayAnime("CoughWait", 0.5f);
		}

		public override void Main()
		{
			UpdateVoice();
		}

		public override void Out()
		{
			if ((members.param.detail & H_Parameter.DETAIL.COUGH) != 0)
			{
				members.param.detail ^= H_Parameter.DETAIL.COUGH;
			}
		}

		public override void OnInput(H_INPUT input)
		{
			OnInput_Extracted(input);
		}
	}
}
