using System;
using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_ShowMouthLiquid : H_State
	{
		public H_State_ShowMouthLiquid(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			List<Female> females = members.GetFemales();
			members.param.detail |= H_Parameter.DETAIL.SHOW_ORAL;
			bool flag = false;
			if (females[0].IsFloped() ? (UnityEngine.Random.Range(0f, 1f) <= 0.7f) : (UnityEngine.Random.Range(0f, 1f) <= 0.3f))
			{
				members.PlayAnime("ShowMouthLiquid", 0.5f);
			}
			else
			{
				members.PlayAnime("ExtractedWait", 0.5f);
			}
			members.VoiceExpression(H_Voice.TYPE.SHOW_ORAL);
		}

		public override void Main()
		{
			UpdateVoice();
		}

		public override void Out()
		{
			if ((members.param.detail & H_Parameter.DETAIL.SHOW_ORAL) != 0)
			{
				members.param.detail ^= H_Parameter.DETAIL.SHOW_ORAL;
			}
		}

		private void UpdateVoice()
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

		public override void OnInput(H_INPUT input)
		{
			switch (input)
			{
			case H_INPUT.VOMIT:
				ChangeState(H_STATE.VOMIT, null);
				break;
			case H_INPUT.DRINK:
				ChangeState(H_STATE.DRINK, null);
				break;
			}
		}
	}
}
