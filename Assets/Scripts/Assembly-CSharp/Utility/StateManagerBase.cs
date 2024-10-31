using System;
using System.Collections.Generic;

namespace Utility
{
	public abstract class StateManagerBase<T_ID, T_CLASS> where T_CLASS : StateBase
	{
		protected Dictionary<T_ID, T_CLASS> states = new Dictionary<T_ID, T_CLASS>();

		protected T_ID nowStateID;

		protected T_CLASS nowStateClass;

		public T_ID NowStateID
		{
			get
			{
				return nowStateID;
			}
		}

		public StateManagerBase()
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

		public void ChangeState(T_ID next)
		{
			if (nowStateClass != null)
			{
				nowStateClass.Out();
			}
			nowStateID = next;
			nowStateClass = states[nowStateID];
			nowStateClass.In();
		}
	}
}
