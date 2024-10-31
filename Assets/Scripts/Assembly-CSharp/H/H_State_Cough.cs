namespace H
{
	internal class H_State_Cough : H_State
	{
		public H_State_Cough(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.PlayAnime("Cough", 0.2f);
			members.param.detail |= H_Parameter.DETAIL.COUGH;
			members.param.mouth = H_MOUTH.FREE;
			members.VoiceExpression(H_Voice.TYPE.COUGH);
		}

		public override void Main()
		{
			if (members.CheckEndVoice())
			{
				ChangeState(H_STATE.COUGH_WAIT);
			}
		}

		public override void Out()
		{
		}
	}
}
