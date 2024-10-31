namespace H
{
	internal class H_State_Exit : H_State
	{
		private bool exit;

		public H_State_Exit(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			H_StyleData styleData = members.StyleData;
			if (styleData != null)
			{
				bool flag = false;
				if (styleData.type == H_StyleData.TYPE.PETTING)
				{
					flag = false;
				}
				else if (styleData.type == H_StyleData.TYPE.INSERT)
				{
					flag = true;
				}
				else if (styleData.type == H_StyleData.TYPE.SERVICE)
				{
					flag = (styleData.IsInMouth() ? true : false);
				}
				if (flag)
				{
					if (((uint)styleData.detailFlag & 0x1000u) != 0)
					{
						members.PlayAnime("InEja_Base", 0.5f);
					}
					else if (members.StateMgr.PrevStateID == H_STATE.PRE_INSERT_WAIT)
					{
						members.PlayAnime("PreInsertWait", 0.5f);
					}
					else
					{
						members.PlayAnime("OutEja_Base", 0.5f);
					}
				}
				else
				{
					members.PlayAnime("Wait", 0.5f);
				}
			}
			members.param.mouth = H_MOUTH.FREE;
			members.EnableTinIK = false;
			members.VoiceExpression(H_Voice.TYPE.EXIT);
			members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
			exit = false;
		}

		public override void Main()
		{
			if (!exit && members.CheckEndVoice())
			{
				members.h_scene.Exit();
				exit = true;
			}
		}

		public override void Out()
		{
		}
	}
}
