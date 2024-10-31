using System;
namespace Utility
{
	public abstract class StateBase_WithMsg<T_MSG>
	{
		protected T_MSG defMsg;

		public virtual void In(T_MSG msg)
		{
		}

		public virtual void Main()
		{
		}

		public virtual void Out()
		{
		}
	}
}
