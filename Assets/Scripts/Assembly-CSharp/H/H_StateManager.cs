using System;
using Utility;

namespace H
{
	public class H_StateManager : StateManagerBase_WithMsg<H_STATE, H_State, H_State.Message>
	{
		public H_StateManager(H_Members members)
		{
			states[H_STATE.START] = new H_State_Start(members);
			states[H_STATE.PRE_INSERT_WAIT] = new H_State_PreInsertWait(members);
			states[H_STATE.PRE_TOUCH_WAIT] = new H_State_PreTouchWait(members);
			states[H_STATE.INSERT] = new H_State_Insert(members);
			states[H_STATE.INSERTED_WAIT] = new H_State_InsertedWait(members);
			states[H_STATE.LOOP] = new H_State_Loop(members);
			states[H_STATE.SPURT] = new H_State_Spurt(members);
			states[H_STATE.IN_EJA_IN] = new H_State_InEjaIn(members);
			states[H_STATE.IN_EJA_TREMBLE] = new H_State_InEjaTremble(members);
			states[H_STATE.OUT_EJA_IN] = new H_State_OutEjaIn(members);
			states[H_STATE.OUT_EJA_TREMBLE] = new H_State_OutEjaTremble(members);
			states[H_STATE.XTC_F_IN] = new H_State_XTC_F_In(members);
			states[H_STATE.XTC_F_TREMBLE] = new H_State_XTC_F_Tremble(members);
			states[H_STATE.XTC_W_IN] = new H_State_XTC_W_In(members);
			states[H_STATE.XTC_W_TREMBLE] = new H_State_XTC_W_Tremble(members);
			states[H_STATE.IN_XTC_AFTER_WAIT] = new H_State_InXtcAfterWait(members);
			states[H_STATE.OUT_XTC_AFTER_WAIT] = new H_State_OutXtcAfterWait(members);
			states[H_STATE.EXTRACT] = new H_State_Extract(members);
			states[H_STATE.EXTRACTED_WAIT] = new H_State_ExtractedWait(members);
			states[H_STATE.COUGH] = new H_State_Cough(members);
			states[H_STATE.COUGH_WAIT] = new H_State_CoughWait(members);
			states[H_STATE.VOMIT] = new H_State_Vomit(members);
			states[H_STATE.VOMIT_WAIT] = new H_State_VomitWait(members);
			states[H_STATE.SHOW_MOUTH_LIQUID] = new H_State_ShowMouthLiquid(members);
			states[H_STATE.DRINK] = new H_State_Drink(members);
			states[H_STATE.DRINK_WAIT] = new H_State_DrinkWait(members);
			states[H_STATE.EXIT] = new H_State_Exit(members);
		}

		public void OnInput(H_INPUT input)
		{
			if (nowStateClass != null)
			{
				nowStateClass.OnInput(input);
			}
		}
	}
}
