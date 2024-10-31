using System;
using System.IO;
using UnityEngine;

namespace Character
{
	public abstract class ColorParameter_Base
	{
		public abstract COLOR_TYPE GetColorType();

		public abstract void Save(BinaryWriter writer);

		public abstract bool Load(BinaryReader reader, CUSTOM_DATA_VERSION version);

		protected void Save_ColorType(BinaryWriter writer)
		{
			writer.Write((int)GetColorType());
		}

		protected static COLOR_TYPE Load_ColorType(BinaryReader reader, CUSTOM_DATA_VERSION version)
		{
			return (COLOR_TYPE)reader.ReadInt32();
		}

		public abstract void SetToMaterial(Material material);

		protected void WriteColor(BinaryWriter writer, Color color)
		{
			writer.Write(color.r);
			writer.Write(color.g);
			writer.Write(color.b);
			writer.Write(color.a);
		}

		protected void ReadColor(BinaryReader reader, ref Color color)
		{
			color.r = reader.ReadSingle();
			color.g = reader.ReadSingle();
			color.b = reader.ReadSingle();
			color.a = reader.ReadSingle();
		}
	}
}
