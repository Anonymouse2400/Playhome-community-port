namespace H
{
	internal class H_State_ExtractedWait : H_State_AfterBase
	{
		public H_State_ExtractedWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			base.In(msg);
			if (members.StyleData.type == H_StyleData.TYPE.INSERT)
			{
				members.PlayAnime("OutEja_Base", 0.5f);
			}
			else
			{
				members.PlayAnime("ExtractedWait", 0.5f);
			}
		}

		public override void Main()
		{
			UpdateVoice();
		}

		public override void Out()
		{
		}

		public override void OnInput(H_INPUT input)
		{
			OnInput_Extracted(input);
		}
	}
}
