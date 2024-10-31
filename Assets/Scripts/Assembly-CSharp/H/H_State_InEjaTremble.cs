namespace H
{
	internal class H_State_InEjaTremble : H_State_TrembleBase
	{
		public H_State_InEjaTremble(H_Members members)
			: base(members)
		{
		}

		protected override string GetAnimeName_Base()
		{
			return "InEja_Base";
		}

		protected override string GetAnimeName_TrembleA()
		{
			return "InEja_TrembleA";
		}

		protected override string GetAnimeName_TrembleB()
		{
			return "InEja_TrembleB";
		}

		protected override H_STATE GetNextState()
		{
			return H_STATE.IN_XTC_AFTER_WAIT;
		}
	}
}
