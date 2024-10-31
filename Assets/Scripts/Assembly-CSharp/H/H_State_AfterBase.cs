using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace H
{
	internal abstract class H_State_AfterBase : H_State
	{
		protected bool[] breath;

		private int visitorVoiceProbability = 3;

		public H_State_AfterBase(H_Members members)
			: base(members)
		{
			breath = new bool[members.GetFemales().Count];
			for (int i = 0; i < breath.Length; i++)
			{
				breath[i] = false;
			}
		}

		public override void In(Message msg)
		{
			List<Female> females = members.GetFemales();
			if (breath.Length != females.Count)
			{
				breath = new bool[females.Count];
			}
			bool flag = VisitorVoice();
			for (int i = 0; i < females.Count; i++)
			{
				bool flag2 = false;
				if (!flag)
				{
					flag2 = members.VoiceExpression(i, H_Voice.TYPE.XTC_AFTER_TALK);
				}
				if (!flag2)
				{
					flag2 = members.VoiceExpression(i, H_Voice.TYPE.XTC_AFTER_BREATH);
					breath[i] = true;
				}
			}
			members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
		}

		private bool VisitorVoice()
		{
			if (members.h_scene.visitor != null && members.h_scene.visitor.GetHuman().sex == SEX.FEMALE && UnityEngine.Random.Range(0, visitorVoiceProbability) == 0)
			{
				return members.h_scene.VisitorVoiceExpression(H_VisitorVoice.TYPE.AFTER);
			}
			return false;
		}

		public override void Main()
		{
			UpdateVoice();
		}

		protected void UpdateVoice()
		{
			List<Female> females = members.GetFemales();
			for (int i = 0; i < females.Count; i++)
			{
				if (!females[i].IsVoicePlaying())
				{
					H_Voice.TYPE voice = ((!breath[i]) ? H_Voice.TYPE.XTC_AFTER_BREATH : H_Voice.TYPE.BREATH);
					breath[i] = true;
					members.VoiceExpression(i, voice);
				}
			}
		}

		protected void OnInput_Extracted(H_INPUT input)
		{
			if (input == H_INPUT.CLICK_PAD)
			{
				CheckWeaknessStyle();
				if (members.StyleData.type == H_StyleData.TYPE.SERVICE && !members.StyleData.IsInMouth())
				{
					ChangeState(H_STATE.LOOP, null);
				}
				else
				{
					ChangeState(H_STATE.INSERT, null);
				}
			}
		}

		protected void OnInput_Inserted(H_INPUT input)
		{
			switch (input)
			{
			case H_INPUT.CLICK_PAD:
				CheckWeaknessStyle();
				ChangeState(H_STATE.LOOP, null);
				break;
			case H_INPUT.EXTRACT:
				CheckWeaknessStyle();
				ChangeState(H_STATE.EXTRACT, null);
				break;
			}
		}
	}
}
