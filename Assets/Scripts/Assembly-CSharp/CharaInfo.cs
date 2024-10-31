using System;
using System.IO;
using SEXY;
using UnityEngine;

public class CharaInfo
{
	public enum GAME
	{
		UNKNOWN = -1,
		SEXY_BEACH = 0,
		HONEY_SELECT = 1,
		PLAY_HOME = 2
	}

	public string name = "名無し";

	public DateTime time;

	public GAME game = GAME.UNKNOWN;

	public void Load(string fileName)
	{
		name = Path.GetFileNameWithoutExtension(fileName);
		using (FileStream input = new FileStream(fileName, FileMode.Open, FileAccess.Read))
		{
			using (BinaryReader reader = new BinaryReader(input))
			{
				try
				{
					Load(reader);
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
			}
		}
		time = File.GetLastWriteTime(fileName);
	}

	public void Load(BinaryReader reader)
	{
		long offset = PNG_Loader.CheckSize(reader);
		reader.BaseStream.Seek(offset, SeekOrigin.Begin);
		string text = reader.ReadString();
		if (text.IndexOf("【PlayHome") == 0)
		{
			game = GAME.PLAY_HOME;
		}
		else if (text.IndexOf("【HoneySelectChara") == 0 || text.IndexOf("【PremiumResortChara") == 0)
		{
			LoadSEXY(reader, text);
		}
		else if (text.IndexOf("【HoneySelectClothesFemale】") == 0)
		{
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			CharFileInfoClothesFemale charFileInfoClothesFemale = new CharFileInfoClothesFemale();
			charFileInfoClothesFemale.Load(reader, true);
			name = charFileInfoClothesFemale.comment;
		}
		else if (text.IndexOf("【HoneySelectClothesMale】") == 0)
		{
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			CharFileInfoClothesMale charFileInfoClothesMale = new CharFileInfoClothesMale();
			charFileInfoClothesMale.Load(reader, true);
			name = charFileInfoClothesMale.comment;
		}
	}

	private void LoadSEXY(BinaryReader reader, string mark)
	{
		int num = reader.ReadInt32();
		int num2 = reader.ReadInt32();
		BlockHeader[] array = new BlockHeader[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = new BlockHeader();
			array[i].LoadHeader(reader);
		}
		long position = reader.BaseStream.Position;
		BlockHeader[] array2 = array;
		foreach (BlockHeader blockHeader in array2)
		{
			if (blockHeader.tagName.StartsWith("プレビュー情報"))
			{
				if (mark.IndexOf("【HoneySelectChara") == 0)
				{
					reader.BaseStream.Seek(position + blockHeader.pos, SeekOrigin.Begin);
					Load_HoneyPreview(reader, blockHeader.version);
				}
				else if (mark.IndexOf("【PremiumResortChara") == 0)
				{
					reader.BaseStream.Seek(blockHeader.pos, SeekOrigin.Begin);
					Load_SBPRPreview(reader, blockHeader.version);
				}
				else
				{
					Debug.LogError(mark);
				}
			}
		}
	}

	private void Load_HoneyPreview(BinaryReader br, int saveVer)
	{
		game = GAME.HONEY_SELECT;
		if (saveVer >= 4)
		{
			int num = br.ReadInt32();
		}
		int num2 = br.ReadInt32();
		if (saveVer >= 2)
		{
			int num3 = br.ReadInt32();
			int num4 = br.ReadInt32();
			name = br.ReadString();
			int num5 = br.ReadInt32();
			int num6 = br.ReadInt32();
			int num7 = br.ReadInt32();
		}
		if (saveVer >= 4)
		{
			int num8 = br.ReadInt32();
			int num9 = br.ReadInt32();
			int num10 = br.ReadInt32();
			int num11 = br.ReadInt32();
		}
		if (saveVer >= 3)
		{
			int num12 = br.ReadInt32();
		}
	}

	private void Load_SBPRPreview(BinaryReader br, int saveVer)
	{
		game = GAME.SEXY_BEACH;
		br.BaseStream.Seek(1L, SeekOrigin.Current);
		if (saveVer >= 1)
		{
			int num = br.ReadInt32();
		}
		name = br.ReadString();
		byte b = br.ReadByte();
		int num2 = br.ReadInt32();
		byte b2 = br.ReadByte();
		byte b3 = br.ReadByte();
		int num3 = br.ReadInt32();
	}
}
