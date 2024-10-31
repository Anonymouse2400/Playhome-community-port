using System;
using System.IO;
using UnityEngine;

namespace Character
{
	public class WearCustom : ParameterBase
	{
		public WEAR_TYPE type = WEAR_TYPE.NUM;

		public int id = -1;

		public ColorParameter_PBR2 color;

		public WearCustom(SEX sex, WEAR_TYPE type, int id)
			: base(sex)
		{
			this.type = type;
			this.id = id;
			color = new ColorParameter_PBR2();
		}

		public WearCustom(SEX sex, WEAR_TYPE type, int id, Color color)
			: base(sex)
		{
			this.type = type;
			this.id = id;
			this.color = new ColorParameter_PBR2();
		}

		public WearCustom(WearCustom copy)
			: base(copy.sex)
		{
			Copy(copy);
		}

		public void Init()
		{
			id = -1;
			color = new ColorParameter_PBR2();
		}

		public void Save(BinaryWriter writer)
		{
			writer.Write((int)type);
			Write(writer, id);
			if (color != null)
			{
				color.Save(writer);
			}
			else
			{
				writer.Write(0);
			}
		}

		public void Load(BinaryReader reader, SEX sex, CUSTOM_DATA_VERSION version)
		{
			type = (WEAR_TYPE)reader.ReadInt32();
			Read(reader, ref id);
			color.Load(reader, version);
		}

		public void Copy(WearCustom source)
		{
			type = source.type;
			id = source.id;
			if (source.color != null)
			{
				if (color == null)
				{
					color = new ColorParameter_PBR2();
				}
				color.Copy(source.color);
			}
			else
			{
				color = null;
			}
		}

		public void CheckHasData()
		{
			if (!CustomDataManager.HasWearData(sex, type, id))
			{
				id = CustomDataManager.GetWearFirstData(sex, type, id);
				color = new ColorParameter_PBR2();
			}
		}
	}
}
