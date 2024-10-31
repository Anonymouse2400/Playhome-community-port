using System;
using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_InsertedWait : H_State
	{
		private const float leaveTime = 10f;

		private float leaveTimer;

		private bool leave;

		private bool endInVoice;

		public H_State_InsertedWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			float transitionDuration = 0.2f;
			if (((uint)members.StyleData.detailFlag & 0x1000u) != 0)
			{
				transitionDuration = 0f;
			}
			members.PlayAnime("InsertedWait", transitionDuration);
			leave = false;
			leaveTimer = 0f;
			endInVoice = false;
			members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
		}

		public override void Main()
		{
			leaveTimer += Time.deltaTime;
			if (!leave && leaveTimer >= 10f)
			{
				int femaleNo = UnityEngine.Random.Range(0, members.GetFemales().Count);
				members.VoiceExpression(femaleNo, H_Voice.TYPE.LEAVING);
				leave = true;
			}
			UpdateVoice();
			if (endInVoice && members.h_scene.MixCtrl.mode == MixController.MODE.FULL_AUTO)
			{
				ChangeState(H_STATE.LOOP);
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
					endInVoice = true;
				}
			}
		}

		public override void OnInput(H_INPUT input)
		{
			if (input == H_INPUT.CLICK_PAD)
			{
				ChangeState(H_STATE.LOOP);
			}
		}
	}
}
