namespace H
{
	internal class H_State_Drink : H_State
	{
		public H_State_Drink(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.PlayAnime("Drink", 0.2f);
			members.param.detail |= H_Parameter.DETAIL.DRINK;
			members.param.mouth = H_MOUTH.FREE;
			members.VoiceExpression(H_Voice.TYPE.DRINK);
		}

		public override void Main()
		{
			if (members.CheckEndVoice())
			{
				ChangeState(H_STATE.DRINK_WAIT);
			}
		}

		public override void Out()
		{
		}
	}
}
