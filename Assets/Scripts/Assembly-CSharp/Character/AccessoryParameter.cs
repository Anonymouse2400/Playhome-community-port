using System.IO;

namespace Character
{
	public class AccessoryParameter : ParameterBase
	{
		public AccessoryCustom[] slot = new AccessoryCustom[10];

		public AccessoryParameter(SEX sex)
			: base(sex)
		{
			for (int i = 0; i < slot.Length; i++)
			{
				slot[i] = new AccessoryCustom(sex);
			}
		}

		public AccessoryParameter(AccessoryParameter copy)
			: base(copy.sex)
		{
			for (int i = 0; i < copy.slot.Length; i++)
			{
				slot[i] = new AccessoryCustom(copy.slot[i]);
			}
		}

		public void Init()
		{
			for (int i = 0; i < slot.Length; i++)
			{
				slot[i].Init();
			}
		}

		public void Save(BinaryWriter writer, SEX sex)
		{
			for (int i = 0; i < slot.Length; i++)
			{
				slot[i].Save(writer, sex);
			}
		}

		public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
		{
			for (int i = 0; i < slot.Length; i++)
			{
				slot[i].Load(reader, sex, version);
			}
		}

		public void Copy(AccessoryParameter source)
		{
			sex = source.sex;
			for (int i = 0; i < slot.Length; i++)
			{
				slot[i].Copy(source.slot[i]);
			}
		}

		public void CheckHasData()
		{
			for (int i = 0; i < slot.Length; i++)
			{
				slot[i].CheckHasData();
			}
		}
	}
}
