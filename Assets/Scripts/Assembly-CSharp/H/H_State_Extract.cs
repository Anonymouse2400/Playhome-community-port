using System;
using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_Extract : H_State
	{
		private float timer;

		private bool extracted;

		private float extractDelay;

		public H_State_Extract(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			members.PlayAnime("Extract", 0.2f);
			extractDelay = 2f;
			timer = 0f;
			extracted = false;
			members.EnableTinIK = false;
		}

		public override void Main()
		{
			timer += Time.deltaTime;
			if (!extracted)
			{
				if (timer >= extractDelay)
				{
					Extract();
				}
			}
			else if (members.CheckEndAnime("Extract"))
			{
				Next();
			}
		}

		private void Extract()
		{
			H_Expression.TYPE type = H_Expression.TYPE.EXTRACT;
			H_StyleData style = members.param.style;
			if (((uint)style.detailFlag & 4u) != 0)
			{
				type = H_Expression.TYPE.EXTRACT_FELLATIO;
			}
			else if (((uint)style.detailFlag & 8u) != 0)
			{
				type = H_Expression.TYPE.EXTRACT_IRRUMATIO;
			}
			members.param.mouth = H_MOUTH.FREE;
			members.Expression(type);
			bool flag = (style.detailFlag & 0x10) != 0;
			bool flag2 = (style.detailFlag & 0x20) != 0;
			List<Female> females = members.GetFemales();
			for (int i = 0; i < females.Count; i++)
			{
				bool flag3 = false;
				if (females[i].personality.spermInCntV > 0 && flag)
				{
					females[i].personality.spermInCntV = 0;
					females[i].dripParticleVagina.Play();
					flag3 = true;
				}
				if (females[i].personality.spermInCntA > 0 && flag2)
				{
					females[i].personality.spermInCntA = 0;
					females[i].dripParticleAnus.Play();
					flag3 = true;
				}
				if (flag3)
				{
					members.VoiceExpression(i, H_Voice.TYPE.FALL_LIQUID);
				}
			}
			extracted = true;
		}

		private void Next()
		{
			if (members.StyleData.type == H_StyleData.TYPE.SERVICE && members.StyleData.IsInMouth())
			{
				Female female = members.GetFemale(0);
				Personality personality = members.GetFemale(0).personality;
				if (personality.likeFeratio)
				{
					ChangeState(H_STATE.SHOW_MOUTH_LIQUID);
					return;
				}
				bool flag = false;
				if (female.IsFloped() ? (UnityEngine.Random.Range(0f, 1f) <= 0.3f) : (UnityEngine.Random.Range(0f, 1f) <= 0.7f))
				{
					ChangeState(H_STATE.COUGH);
				}
				else
				{
					ChangeState(H_STATE.EXTRACTED_WAIT);
				}
			}
			else
			{
				ChangeState(H_STATE.EXTRACTED_WAIT);
			}
		}

		public override void Out()
		{
		}
	}
}
