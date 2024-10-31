namespace H
{
	internal class H_State_OutEjaTremble : H_State_TrembleBase
	{
		public H_State_OutEjaTremble(H_Members members)
			: base(members)
		{
		}

		protected override string GetAnimeName_Base()
		{
			return "OutEja_Base";
		}

		protected override string GetAnimeName_TrembleA()
		{
			return "OutEja_TrembleA";
		}

		protected override string GetAnimeName_TrembleB()
		{
			return "OutEja_TrembleB";
		}

		protected override H_STATE GetNextState()
		{
			return H_STATE.OUT_XTC_AFTER_WAIT;
		}
	}
}
