using System;
using System.IO;
using UnityEngine;

namespace SEXY
{
	public static class PngAssist
	{
		public static bool CheckPngData(Stream st, ref long size, bool skip)
		{
			if (st == null)
			{
				return false;
			}
			size = 0L;
			long position = st.Position;
			byte[] array = new byte[8];
			byte[] array2 = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
			st.Read(array, 0, 8);
			for (int i = 0; i < 8; i++)
			{
				if (array[i] != array2[i])
				{
					st.Seek(position, SeekOrigin.Begin);
					return false;
				}
			}
			int num = 0;
			int num2 = 0;
			bool flag = true;
			while (flag)
			{
				byte[] array3 = new byte[4];
				st.Read(array3, 0, 4);
				Array.Reverse(array3);
				num = BitConverter.ToInt32(array3, 0);
				byte[] array4 = new byte[4];
				st.Read(array4, 0, 4);
				num2 = BitConverter.ToInt32(array4, 0);
				if (num2 == 1145980233)
				{
					flag = false;
				}
				st.Seek(num + 4, SeekOrigin.Current);
			}
			size = st.Position - position;
			if (!skip)
			{
				st.Seek(position, SeekOrigin.Begin);
			}
			return true;
		}

		public static bool CheckPngData(BinaryReader reader, ref long size, bool skip)
		{
			if (reader == null)
			{
				return false;
			}
			return CheckPngData(reader.BaseStream, ref size, skip);
		}

		public static bool CheckPngData(byte[] data, ref long size)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(data, 0, data.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return CheckPngData(memoryStream, ref size, false);
			}
		}

		public static byte[] LoadPngData(string fullpath)
		{
			using (FileStream input = new FileStream(fullpath, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader reader = new BinaryReader(input))
				{
					return LoadPngData(reader);
				}
			}
		}

		public static byte[] LoadPngData(BinaryReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			long size = 0L;
			CheckPngData(reader.BaseStream, ref size, false);
			if (size == 0)
			{
				return null;
			}
			return reader.ReadBytes((int)size);
		}

		public static Sprite LoadSpriteFromFile(string path, int width, int height, Vector2 pivot)
		{
			if (!File.Exists(path))
			{
				Debug.LogError(path + " が存在しない");
				return null;
			}
			using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					byte[] data = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
					Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
					if (null == texture2D)
					{
						return null;
					}
					texture2D.LoadImage(data);
					if (width == 0 || height == 0)
					{
						width = texture2D.width;
						height = texture2D.height;
					}
					return Sprite.Create(texture2D, new Rect(0f, 0f, width, height), pivot, 100f, 0u, SpriteMeshType.FullRect);
				}
			}
		}

		public static Texture2D ChangeTextureFromByte(byte[] data, int width = 0, int height = 0)
		{
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
			if (null == texture2D)
			{
				return null;
			}
			texture2D.LoadImage(data);
			return texture2D;
		}

		public static Sprite LoadSpriteFromFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				long size = 0L;
				CheckPngData(fileStream, ref size, false);
				if (size == 0)
				{
					return null;
				}
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					byte[] data = binaryReader.ReadBytes((int)size);
					int width = 0;
					int height = 0;
					Texture2D texture2D = ChangeTextureFromPngByte(data, ref width, ref height);
					if (null == texture2D)
					{
						return null;
					}
					return Sprite.Create(texture2D, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect);
				}
			}
		}

		public static Texture2D ChangeTextureFromPngByte(byte[] data, ref int width, ref int height)
		{
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
			if (null == texture2D)
			{
				return null;
			}
			texture2D.LoadImage(data);
			width = texture2D.width;
			height = texture2D.height;
			return texture2D;
		}

		public static Texture2D LoadTexture(string _path)
		{
			using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read))
			{
				long size = 0L;
				CheckPngData(fileStream, ref size, false);
				if (size == 0)
				{
					return null;
				}
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					byte[] data = binaryReader.ReadBytes((int)size);
					int width = 0;
					int height = 0;
					return ChangeTextureFromPngByte(data, ref width, ref height);
				}
			}
		}
	}
}
