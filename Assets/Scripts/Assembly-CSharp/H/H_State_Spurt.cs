using UnityEngine;

namespace H
{
	internal class H_State_Spurt : H_State
	{
		public struct SpurtMsg : Message
		{
			public XTC_TYPE xtcType;

			public SpurtMsg(XTC_TYPE type)
			{
				xtcType = type;
			}
		}

		private XTC_TYPE xtcType;

		private float timer;

		private float seTimer = -1f;

		private float maxSpeed = 4f;

		private float minSpeed = 0.5f;

		public H_State_Spurt(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.param.manyEjaculation = members.MaleGageVal >= 1f;
			xtcType = ((SpurtMsg)(object)msg).xtcType;
			members.param.xtcType = xtcType;
			members.PlayAnime("Spurt", 0.2f);
			members.SetSpeed(4f);
			H_Voice.TYPE voice = ((xtcType != XTC_TYPE.XTC_F) ? H_Voice.TYPE.XTC_OMEN_M : H_Voice.TYPE.XTC_OMEN_F);
			members.VoiceExpression(voice);
			timer = 0f;
			seTimer = -1f;
		}

		public override void Main()
		{
			timer += Time.deltaTime;
			if (members.CheckEndVoice())
			{
				Next();
				return;
			}
			float speed = members.input.Speed;
			speed = ((!(speed > 0f)) ? Mathf.Lerp(1f, minSpeed, 0f - speed) : Mathf.Lerp(1f, maxSpeed, speed));
			UpdateSE(speed);
		}

		public override void Out()
		{
			members.SetSpeed(1f);
		}

		private void Next()
		{
			H_STATE[] array = new H_STATE[5]
			{
				H_STATE.NUM,
				H_STATE.IN_EJA_IN,
				H_STATE.OUT_EJA_IN,
				H_STATE.XTC_F_IN,
				H_STATE.XTC_W_IN
			};
			ChangeState(array[(int)xtcType], null);
		}

		public override void OnInput(H_INPUT input)
		{
			if (input == H_INPUT.CLICK_PAD)
			{
				Next();
			}
		}

		private void UpdateSE(float speed)
		{
			Female female = members.GetFemale(0);
			float delay = members.h_scene.se.Delay;
			float num = female.body.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime + delay;
			float num2 = seTimer;
			if (seTimer == -1f || num % 1f < num2 % 1f)
			{
				if (members.StyleData.type == H_StyleData.TYPE.INSERT)
				{
					members.h_scene.se.Play_Piston(female, speed);
				}
				else if (members.StyleData.type == H_StyleData.TYPE.PETTING && ((uint)members.StyleData.detailFlag & 0x10u) != 0)
				{
					members.h_scene.se.Play_Crotch(female, speed);
				}
			}
			seTimer = num;
		}
	}
}
