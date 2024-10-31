using System;
using System.Collections.Generic;

namespace H
{
	internal class H_State_Start : H_State
	{
		public H_State_Start(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.VoiceExpression(H_Voice.TYPE.START);
			members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
		}

		public override void Main()
		{
			List<Female> females = members.GetFemales();
			for (int i = 0; i < females.Count; i++)
			{
				if (!females[i].IsVoicePlaying())
				{
					members.VoiceExpression(i, H_Voice.TYPE.BREATH);
				}
			}
		}

		public override void Out()
		{
		}
	}
}
