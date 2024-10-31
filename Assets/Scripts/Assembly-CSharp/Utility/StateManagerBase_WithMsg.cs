using System;
using System.Collections.Generic;

namespace Utility
{
	public abstract class StateManagerBase_WithMsg<T_ID, T_CLASS, T_MSG> where T_CLASS : StateBase_WithMsg<T_MSG>
	{
		protected Dictionary<T_ID, T_CLASS> states = new Dictionary<T_ID, T_CLASS>();

		protected T_ID nowStateID;

		protected T_ID prevStateID;

		protected T_CLASS nowStateClass;

		public T_ID NowStateID
		{
			get
			{
				return nowStateID;
			}
		}

		public T_ID PrevStateID
		{
			get
			{
				return prevStateID;
			}
		}

		public StateManagerBase_WithMsg()
		{
		}

		public void Main()
		{
			nowStateClass.Main();
		}

		public void SetState(T_ID t_id, T_CLASS t_class)
		{
			states.Add(t_id, t_class);
		}

		public void ChangeState(T_ID next, T_MSG msg)
		{
			if (nowStateClass != null)
			{
				nowStateClass.Out();
			}
			prevStateID = nowStateID;
			nowStateID = next;
			nowStateClass = states[nowStateID];
			nowStateClass.In(msg);
		}
	}
}
