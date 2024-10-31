using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace H
{
	public enum H_STATE
	{
		START = 0,
		PRE_INSERT_WAIT = 1,
		PRE_TOUCH_WAIT = 2,
		INSERT = 3,
		INSERTED_WAIT = 4,
		LOOP = 5,
		SPURT = 6,
		IN_EJA_IN = 7,
		IN_EJA_TREMBLE = 8,
		OUT_EJA_IN = 9,
		OUT_EJA_TREMBLE = 10,
		XTC_F_IN = 11,
		XTC_F_TREMBLE = 12,
		XTC_W_IN = 13,
		XTC_W_TREMBLE = 14,
		IN_XTC_AFTER_WAIT = 15,
		OUT_XTC_AFTER_WAIT = 16,
		EXTRACT = 17,
		EXTRACTED_WAIT = 18,
		COUGH = 19,
		COUGH_WAIT = 20,
		SHOW_MOUTH_LIQUID = 21,
		VOMIT = 22,
		VOMIT_WAIT = 23,
		DRINK = 24,
		DRINK_WAIT = 25,
		EXIT = 26,
		NUM = 27
	}
	public abstract class H_State : StateBase_WithMsg<H_State.Message>
	{
		public interface Message
		{
		}

		public class BlendMessage : Message
		{
			public float blend;

			public BlendMessage(float blend)
			{
				this.blend = blend;
			}

			public static float GetBlend(Message msg, float defBlend)
			{
				BlendMessage blendMessage = msg as BlendMessage;
				return (blendMessage == null) ? defBlend : blendMessage.blend;
			}
		}

		protected H_Members members;

		protected H_State(H_Members members)
		{
			this.members = members;
		}

		public virtual void OnInput(H_INPUT input)
		{
		}

		protected void ChangeState(H_STATE next, Message msg)
		{
			members.StateMgr.ChangeState(next, msg);
		}

		protected void ChangeState(H_STATE next)
		{
			members.StateMgr.ChangeState(next, null);
		}

		protected bool CheckChangePersonalityState()
		{
			if (members.param.continuanceXTC_F >= 3)
			{
				Female female = members.GetFemale(0);
				if (female.IsFloped())
				{
					if (!female.personality.ahe)
					{
						female.personality.ahe = true;
						return true;
					}
				}
				else if (!female.personality.weakness)
				{
					female.personality.weakness = true;
					members.h_scene.StyleChangeUI.UpdateList();
					return true;
				}
			}
			return false;
		}

		protected void CheckWeaknessStyle()
		{
			H_Scene h_scene = members.h_scene;
			H_StyleData styleData = members.StyleData;
			if (members.GetFemale(0).personality.weakness && styleData.state != H_StyleData.STATE.WEAKNESS)
			{
				H_StyleData h_StyleData = null;
				string[] array = styleData.id.Split(new char[1] { '_' }, StringSplitOptions.RemoveEmptyEntries);
				string text = "02";
				string id = array[0] + "_" + array[1] + "_" + text + "_" + array[3];
				h_scene.ChangeStyle(id);
			}
		}

		protected void CheckUrine()
		{
			List<Female> females = members.GetFemales();
			for (int i = 0; i < females.Count; i++)
			{
				float num = 0.1f;
				if (females[i].personality.weakness)
				{
					num += 0.25f;
				}
				float num2 = UnityEngine.Random.Range(0f, 1f);
				if (num2 <= num)
				{
					females[i].urineParticle.Play();
					members.VoiceExpression(i, H_Voice.TYPE.INCONTINENCE);
				}
			}
		}
	}
}
