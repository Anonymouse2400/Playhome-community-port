using System;
using System.Collections.Generic;
using UnityEngine;

namespace H
{
	internal class H_State_Insert : H_State
	{
		private float timer;

		private bool inserted;

		public H_State_Insert(H_Members members)
			: base(members)
		{
		}

		public override void In(Message msg)
		{
			inserted = false;
			members.param.anyAction = true;
			if (((uint)members.StyleData.detailFlag & 0x1000u) != 0)
			{
				Insert();
				ChangeState(H_STATE.INSERTED_WAIT);
			}
			else
			{
				timer = 0f;
				members.PlayAnime("Insert", 0.2f);
			}
		}

		public override void Main()
		{
			if (!inserted)
			{
				timer += Time.deltaTime;
				if (timer >= members.StyleData.insertDelay)
				{
					Insert();
				}
			}
			else if (members.CheckEndAnime("Insert"))
			{
				ChangeState(H_STATE.INSERTED_WAIT);
			}
		}

		private void Insert()
		{
			members.param.mouth = members.StyleData.GetInsertedMouth();
			members.VoiceExpression(H_Voice.TYPE.INSERT);
			members.EnableTinIK = true;
			List<Female> females = members.GetFemales();
			bool flag = (members.StyleData.detailFlag & 0x10) != 0;
			bool flag2 = (members.StyleData.detailFlag & 0x20) != 0;
			for (int i = 0; i < females.Count; i++)
			{
				if (flag)
				{
					females[i].personality.insertVagina = true;
					if (females[i].personality.vaginaVirgin)
					{
						females[i].personality.LostVaginaVirgin();
						females[i].SetVirginBlood(true);
					}
				}
				if (flag2)
				{
					females[i].personality.insertAnal = true;
					if (females[i].personality.analVirgin)
					{
						females[i].personality.LostAnalVirgin();
					}
				}
			}
			members.MaleExpression(H_Expression_Male.TYPE.NORMAL);
			inserted = true;
		}
	}
}
