using System.Collections.Generic;
using Character;
using UnityEngine;

namespace H
{
	internal class H_State_Loop : H_State
	{
		private float maxSpeed = 4f;

		private float minSpeed = 0.5f;

		private float maxExpressionTime = 7.5f;

		private float minExpressionTime = 2.5f;

		private H_SPEED checkSpeed = H_SPEED.NORMAL;

		private float seTimer = -1f;

		private float visitorVoiceTimer;

		private float visitorVoiceMin = 5f;

		private float visitorVoiceMax = 15f;

		private int visitorVoiceProbability = 3;

		private bool visitorVoicePlay;

		private float[] expressionTimers_F;

		private float[] expressionTimers_M;

		public H_State_Loop(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			float blend = BlendMessage.GetBlend(msg, 0.2f);
			members.param.anyAction = true;
			members.PlayAnime("Loop", blend);
			members.param.mouth = members.StyleData.GetInsertedMouth();
			members.EnableTinIK = true;
			members.VoiceExpression(H_Voice.TYPE.ACT);
			members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
			seTimer = -1f;
			In_Visitor();
			expressionTimers_F = new float[members.GetFemales().Count];
			for (int i = 0; i < expressionTimers_F.Length; i++)
			{
				expressionTimers_F[i] = NextExpressionTime();
			}
			expressionTimers_M = new float[members.GetMales().Count];
			for (int j = 0; j < expressionTimers_M.Length; j++)
			{
				expressionTimers_M[j] = NextExpressionTime();
			}
			CheckLostVirgin();
		}

		private void CheckLostVirgin()
		{
			H_StyleData styleData = members.StyleData;
			bool flag = false;
			if (styleData.type == H_StyleData.TYPE.INSERT)
			{
				flag = true;
			}
			else if (styleData.type == H_StyleData.TYPE.PETTING && ((uint)styleData.detailFlag & 0x400u) != 0)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			List<Female> females = members.GetFemales();
			bool flag2 = (members.StyleData.detailFlag & 0x10) != 0;
			bool flag3 = (members.StyleData.detailFlag & 0x20) != 0;
			for (int i = 0; i < females.Count; i++)
			{
				if (flag2)
				{
					females[i].personality.insertVagina = true;
					if (females[i].personality.vaginaVirgin)
					{
						females[i].personality.LostVaginaVirgin();
						females[i].SetVirginBlood(true);
					}
				}
				if (flag3)
				{
					females[i].personality.insertAnal = true;
					if (females[i].personality.analVirgin)
					{
						females[i].personality.LostAnalVirgin();
					}
				}
			}
		}

		private float NextExpressionTime()
		{
			return Random.Range(minExpressionTime, maxExpressionTime);
		}

		private void In_Visitor()
		{
			if (members.h_scene.visitor != null && members.h_scene.visitor.GetHuman().sex == SEX.FEMALE)
			{
				int num = Random.Range(0, visitorVoiceProbability);
				visitorVoicePlay = num == 0;
				if (visitorVoicePlay)
				{
					visitorVoiceTimer = Random.Range(visitorVoiceMin, visitorVoiceMax);
				}
			}
		}

		public override void Main()
		{
			float speed = members.input.Speed;
			speed = ((!(speed > 0f)) ? Mathf.Lerp(1f, minSpeed, 0f - speed) : Mathf.Lerp(1f, maxSpeed, speed));
			float pose = members.input.Pose;
			float stroke = members.input.Stroke;
			members.SetLoopPose(pose);
			members.SetLoopStroke(stroke);
			members.SetSpeed(speed);
			H_StyleData styleData = members.StyleData;
			bool addFemale = styleData.type == H_StyleData.TYPE.INSERT || styleData.type == H_StyleData.TYPE.PETTING;
			bool addMale = styleData.type == H_StyleData.TYPE.INSERT || styleData.type == H_StyleData.TYPE.SERVICE;
			members.AddGage(addFemale, addMale);
			if (members.FemaleGageVal >= 1f)
			{
				ChangeState(H_STATE.SPURT, new H_State_Spurt.SpurtMsg(XTC_TYPE.XTC_F));
				return;
			}
			bool forced = false;
			if (members.param.speed != H_SPEED.NORMAL && checkSpeed != members.param.speed)
			{
				forced = true;
				checkSpeed = members.param.speed;
			}
			UpdateVoice(forced);
			UpdateSE(speed);
			UpdateVisitor();
		}

		private void UpdateVoice(bool forced = false)
		{
			List<Female> females = members.GetFemales();
			for (int i = 0; i < females.Count; i++)
			{
				if (!females[i].IsVoicePlaying() || forced)
				{
					members.VoiceExpression(i, H_Voice.TYPE.ACT);
					expressionTimers_F[i] = NextExpressionTime();
				}
			}
			for (int j = 0; j < expressionTimers_F.Length; j++)
			{
				if (expressionTimers_F[j] <= 0f)
				{
					H_Expression.TYPE type = H_Members.VoiceToExpressionType_Act(members.param, H_Voice.KIND.PANT);
					members.Expression(type);
					expressionTimers_F[j] = NextExpressionTime();
				}
				else
				{
					expressionTimers_F[j] -= Time.deltaTime;
				}
			}
			for (int k = 0; k < expressionTimers_M.Length; k++)
			{
				if (expressionTimers_M[k] <= 0f)
				{
					members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
					expressionTimers_M[k] = NextExpressionTime();
				}
				else
				{
					expressionTimers_M[k] -= Time.deltaTime;
				}
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

		private void UpdateVisitor()
		{
			if (visitorVoicePlay)
			{
				visitorVoiceTimer -= Time.deltaTime;
				if (visitorVoiceTimer <= 0f)
				{
					members.h_scene.VisitorVoiceExpression(H_VisitorVoice.TYPE.ACT);
					visitorVoicePlay = false;
				}
			}
		}

		public override void Out()
		{
			members.SetSpeed(1f);
		}

		public override void OnInput(H_INPUT input)
		{
			switch (input)
			{
			case H_INPUT.EJA_IN:
				ChangeState(H_STATE.SPURT, new H_State_Spurt.SpurtMsg(XTC_TYPE.EJA_IN));
				break;
			case H_INPUT.EJA_OUT:
				ChangeState(H_STATE.SPURT, new H_State_Spurt.SpurtMsg(XTC_TYPE.EJA_OUT));
				break;
			case H_INPUT.XTC_F:
				ChangeState(H_STATE.SPURT, new H_State_Spurt.SpurtMsg(XTC_TYPE.XTC_F));
				break;
			case H_INPUT.XTC_W:
				ChangeState(H_STATE.SPURT, new H_State_Spurt.SpurtMsg(XTC_TYPE.XTC_W));
				break;
			}
		}
	}
}
