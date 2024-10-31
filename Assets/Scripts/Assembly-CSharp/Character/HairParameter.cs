using System;
using System.IO;

namespace Character
{
	public class HairParameter : ParameterBase
	{
		public HairPartParameter[] parts;

		public HairParameter(SEX sex)
			: base(sex)
		{
			parts = new HairPartParameter[3];
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i] = new HairPartParameter(sex);
			}
		}

		public HairParameter(HairParameter copy)
			: base(copy.sex)
		{
			parts = new HairPartParameter[3];
			Copy(copy);
		}

		public void Init()
		{
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i].Init();
			}
		}

		public void Copy(HairParameter copy)
		{
			if (parts == null)
			{
				parts = new HairPartParameter[3];
			}
			for (int i = 0; i < parts.Length; i++)
			{
				if (parts[i] == null)
				{
					parts[i] = new HairPartParameter(copy.parts[i]);
				}
				else
				{
					parts[i].Copy(copy.parts[i]);
				}
			}
		}

		public void Save(BinaryWriter writer, SEX sex)
		{
			writer.Write(parts.Length);
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i].Save(writer, sex);
			}
		}

		public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
		{
			int num = reader.ReadInt32();
			parts = new HairPartParameter[num];
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i] = new HairPartParameter(sex);
				parts[i].Load(reader, sex, version);
			}
		}
	}
}
