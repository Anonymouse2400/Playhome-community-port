using System;
using System.IO;
using UnityEngine;

namespace Character
{
	public abstract class ParameterBase
	{
		protected SEX sex;

		public SEX Sex
		{
			get
			{
				return sex;
			}
		}

		protected ParameterBase(SEX sex)
		{
			this.sex = sex;
		}

		protected void Write(BinaryWriter writer, int val)
		{
			writer.Write(val);
		}

		protected void Read(BinaryReader reader, ref int val)
		{
			val = reader.ReadInt32();
		}

		protected void Write(BinaryWriter writer, float val)
		{
			writer.Write(val);
		}

		protected void Read(BinaryReader reader, ref float val)
		{
			val = reader.ReadSingle();
		}

		protected void Write(BinaryWriter writer, bool val)
		{
			writer.Write(val);
		}

		protected void Read(BinaryReader reader, ref bool val)
		{
			val = reader.ReadBoolean();
		}

		protected void Write(BinaryWriter writer, Color color)
		{
			writer.Write(color.r);
			writer.Write(color.g);
			writer.Write(color.b);
			writer.Write(color.a);
		}

		protected void Read(BinaryReader reader, ref Color color)
		{
			color.r = reader.ReadSingle();
			color.g = reader.ReadSingle();
			color.b = reader.ReadSingle();
			color.a = reader.ReadSingle();
		}

		protected void Write(BinaryWriter writer, Vector3 vec)
		{
			writer.Write(vec.x);
			writer.Write(vec.y);
			writer.Write(vec.z);
		}

		protected void Read(BinaryReader reader, ref Vector3 vec)
		{
			vec.x = reader.ReadSingle();
			vec.y = reader.ReadSingle();
			vec.z = reader.ReadSingle();
		}

		protected void Write(BinaryWriter writer, float[] vals)
		{
			writer.Write(vals.Length);
			for (int i = 0; i < vals.Length; i++)
			{
				Write(writer, vals[i]);
			}
		}

		protected void Read(BinaryReader reader, ref float[] vals)
		{
			int num = reader.ReadInt32();
			vals = new float[num];
			for (int i = 0; i < vals.Length; i++)
			{
				vals[i] = reader.ReadSingle();
			}
		}
	}
}
