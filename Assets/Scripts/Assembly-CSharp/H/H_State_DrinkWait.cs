namespace H
{
	internal class H_State_DrinkWait : H_State_AfterBase
	{
		public H_State_DrinkWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			base.In(msg);
			members.PlayAnime("DrinkWait", 0.5f);
		}

		public override void Main()
		{
			UpdateVoice();
		}

		public override void Out()
		{
			if ((members.param.detail & H_Parameter.DETAIL.DRINK) != 0)
			{
				members.param.detail ^= H_Parameter.DETAIL.DRINK;
			}
		}

		public override void OnInput(H_INPUT input)
		{
			OnInput_Extracted(input);
		}
	}
}
