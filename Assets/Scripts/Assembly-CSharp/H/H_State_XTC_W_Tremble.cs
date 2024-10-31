namespace H
{
	internal class H_State_XTC_W_Tremble : H_State_TrembleBase
	{
		public H_State_XTC_W_Tremble(H_Members members)
			: base(members)
		{
		}

		protected override string GetAnimeName_Base()
		{
			return "XTC_W_Base";
		}

		protected override string GetAnimeName_TrembleA()
		{
			return "XTC_W_TrembleA";
		}

		protected override string GetAnimeName_TrembleB()
		{
			return "XTC_W_TrembleB";
		}

		protected override H_STATE GetNextState()
		{
			return H_STATE.IN_XTC_AFTER_WAIT;
		}
	}
}
