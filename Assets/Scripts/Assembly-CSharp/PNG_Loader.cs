using System;
using System.IO;

public static class PNG_Loader
{
	public static byte[] Load(BinaryReader reader)
	{
		long num = 0L;
		byte[] array = new byte[8];
		byte[] array2 = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
		array = reader.ReadBytes(8);
		for (int i = 0; i < 8; i++)
		{
			if (array[i] != array2[i])
			{
				reader.BaseStream.Seek(0L, SeekOrigin.Begin);
				return null;
			}
		}
		string text;
		do
		{
			int val = reader.ReadInt32();
			val = RevEndian(val);
			char[] array3 = new char[4];
			array3 = reader.ReadChars(4);
			byte[] array4 = reader.ReadBytes(val);
			int num2 = reader.ReadInt32();
			text = new string(array3);
		}
		while (!(text == "IEND"));
		num = reader.BaseStream.Position;
		reader.BaseStream.Seek(0L, SeekOrigin.Begin);
		if (num > 0)
		{
			return reader.ReadBytes((int)num);
		}
		return null;
	}

	public static long CheckSize(BinaryReader reader)
	{
		long num = 0L;
		byte[] array = new byte[8];
		byte[] array2 = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
		array = reader.ReadBytes(8);
		for (int i = 0; i < 8; i++)
		{
			if (array[i] != array2[i])
			{
				reader.BaseStream.Seek(0L, SeekOrigin.Begin);
				return 0L;
			}
		}
		string text;
		do
		{
			int val = reader.ReadInt32();
			val = RevEndian(val);
			char[] array3 = new char[4];
			array3 = reader.ReadChars(4);
			byte[] array4 = reader.ReadBytes(val);
			int num2 = reader.ReadInt32();
			text = new string(array3);
		}
		while (!(text == "IEND"));
		num = reader.BaseStream.Position;
		reader.BaseStream.Seek(0L, SeekOrigin.Begin);
		return num;
	}

	public static long CheckSize(byte[] data)
	{
		long num = 0L;
		using (MemoryStream input = new MemoryStream(data))
		{
			using (BinaryReader reader = new BinaryReader(input))
			{
				return CheckSize(reader);
			}
		}
	}

	private static int RevEndian(int val)
	{
		byte[] bytes = BitConverter.GetBytes(val);
		Array.Reverse(bytes);
		return BitConverter.ToInt32(bytes, 0);
	}
}
