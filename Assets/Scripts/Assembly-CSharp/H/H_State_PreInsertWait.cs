using System;
using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_PreInsertWait : H_State
	{
		private const float leaveTime = 10f;

		private float leaveTimer;

		private bool leave;

		private bool endInVoice;

		public H_State_PreInsertWait(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.param.mouth = H_MOUTH.FREE;
			if (((uint)members.StyleData.detailFlag & 0x1000u) != 0)
			{
				ChangeState(H_STATE.INSERT);
				return;
			}
			float blend = BlendMessage.GetBlend(msg, 0f);
			members.PlayAnime("PreInsertWait", blend);
			leaveTimer = 0f;
			leave = false;
			endInVoice = false;
			members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
			members.EnableTinIK = false;
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
				ChangeState(H_STATE.INSERT);
			}
		}

		private void UpdateVoice()
		{
			List<Female> females = members.GetFemales();
			for (int i = 0; i < females.Count; i++)
			{
				if (!females[i].IsVoicePlaying())
				{
					endInVoice = true;
					members.VoiceExpression(i, H_Voice.TYPE.BREATH);
				}
			}
		}

		public override void OnInput(H_INPUT input)
		{
			if (input == H_INPUT.CLICK_PAD)
			{
				ChangeState(H_STATE.INSERT);
			}
		}
	}
}
