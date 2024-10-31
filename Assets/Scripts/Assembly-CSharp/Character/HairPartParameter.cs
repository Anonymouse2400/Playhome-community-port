using System.IO;
using UnityEngine;

namespace Character
{
	public class HairPartParameter : ParameterBase
	{
		public int ID;

		public ColorParameter_Hair hairColor;

		public ColorParameter_PBR1 acceColor;

		public HairPartParameter(SEX sex)
			: base(sex)
		{
			Init();
		}

		public HairPartParameter(HairPartParameter copy)
			: base(copy.sex)
		{
			Copy(copy);
		}

		public void Init()
		{
			ID = 0;
			hairColor = new ColorParameter_Hair();
			acceColor = null;
		}

		public void Copy(HairPartParameter copy)
		{
			ID = copy.ID;
			if (copy.hairColor != null)
			{
				hairColor = new ColorParameter_Hair(copy.hairColor);
			}
			else
			{
				hairColor = null;
			}
			if (copy.acceColor != null)
			{
				acceColor = new ColorParameter_PBR1(copy.acceColor);
			}
			else
			{
				acceColor = null;
			}
		}

		public void Save(BinaryWriter writer, SEX sex)
		{
			Write(writer, ID);
			if (hairColor != null)
			{
				hairColor.Save(writer);
			}
			else
			{
				writer.Write(0);
			}
			if (acceColor != null)
			{
				acceColor.Save(writer);
			}
			else
			{
				writer.Write(0);
			}
		}

		public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
		{
			Read(reader, ref ID);
			if (version <= CUSTOM_DATA_VERSION.DEBUG_03)
			{
				Color color = Color.white;
				Read(reader, ref color);
				if (version > CUSTOM_DATA_VERSION.DEBUG_00)
				{
					Read(reader, ref color);
				}
				return;
			}
			hairColor.Load(reader, version);
			if (acceColor == null)
			{
				acceColor = new ColorParameter_PBR1();
			}
			if (!acceColor.Load(reader, version))
			{
				acceColor = null;
			}
		}
	}
}
