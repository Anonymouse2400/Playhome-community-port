using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ListDataLoader
{
	private char[] separator = new char[1] { '\t' };

	private StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries;

	private List<List<string>> list = new List<List<string>>();

	public int X_Num { get; private set; }

	public int Y_Num { get; private set; }

	public string this[int y, int x]
	{
		get
		{
			return Get(x, y);
		}
	}

	public ListDataLoader()
	{
		X_Num = -1;
		Y_Num = -1;
	}

	public ListDataLoader(char separator, StringSplitOptions splitOptions)
	{
		X_Num = -1;
		Y_Num = -1;
		SetSeparator(separator, splitOptions);
	}

	public ListDataLoader(char[] separator, StringSplitOptions splitOptions)
	{
		X_Num = -1;
		Y_Num = -1;
		SetSeparator(separator, splitOptions);
	}

	public void SetSeparator(char separator, StringSplitOptions splitOptions)
	{
		this.separator = new char[1] { separator };
		this.splitOptions = splitOptions;
	}

	public void SetSeparator(char[] separator, StringSplitOptions splitOptions)
	{
		this.separator = separator;
		this.splitOptions = splitOptions;
	}

	public void SetList(List<List<string>> setList)
	{
		list = setList;
	}

	public bool Load_Text(TextAsset text, int x = -1, int y = -1)
	{
		StringReader stringReader = new StringReader(text.text);
		bool result = Load_Text(stringReader, x, y);
		stringReader.Close();
		return result;
	}

	public bool Load_Text(string fullPath, int x = -1, int y = -1)
	{
		FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
		if (fileStream == null)
		{
			MonoBehaviour.print("読み込み失敗 : " + fullPath);
			return false;
		}
		StreamReader streamReader = new StreamReader(fileStream);
		bool result = Load_Text(streamReader, x, y);
		streamReader.Close();
		return result;
	}

	private bool Load_Text(TextReader reader, int x = -1, int y = -1)
	{
		X_Num = x;
		Y_Num = y;
		bool flag = X_Num == 0;
		int num = -1;
		string text = null;
		while ((text = reader.ReadLine()) != null)
		{
			string[] array = text.Split(separator, splitOptions);
			int num2 = Math.Max(0, x);
			if (num2 == 0)
			{
				num2 = array.Length;
			}
			List<string> list = new List<string>();
			for (int i = 0; i < num2; i++)
			{
				if (i < array.Length)
				{
					list.Add(array[i]);
				}
				else
				{
					list.Add(string.Empty);
				}
			}
			if (flag)
			{
				if (num == -1)
				{
					num = num2;
				}
				else if (num != num2)
				{
					flag = false;
				}
			}
			this.list.Add(list);
			if (y > 0 && this.list.Count >= y)
			{
				break;
			}
		}
		Y_Num = this.list.Count;
		if (flag)
		{
			X_Num = num;
		}
		reader.Close();
		return true;
	}

	public bool Save_Text(string fullPath, int x = -1, int y = -1)
	{
		FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
		if (fileStream == null)
		{
			MonoBehaviour.print("読み込み失敗 : " + fullPath);
			return false;
		}
		StreamWriter streamWriter = new StreamWriter(fileStream);
		bool result = Save_Text(streamWriter, x, y);
		streamWriter.Close();
		fileStream.Close();
		return result;
	}

	private bool Save_Text(TextWriter writer, int x = -1, int y = -1)
	{
		X_Num = x;
		Y_Num = y;
		if (Y_Num == -1)
		{
			Y_Num = list.Count;
		}
		for (int i = 0; i < Y_Num; i++)
		{
			int num = ((X_Num != -1) ? X_Num : list[i].Count);
			for (int j = 0; j < num; j++)
			{
				writer.Write(list[i][j]);
				if (j != num - 1)
				{
					writer.Write(separator[0]);
				}
			}
			if (i != Y_Num - 1)
			{
				writer.Write("\n");
			}
		}
		writer.Close();
		return true;
	}

	public string Get(int x, int y)
	{
		if (y < list.Count && x < list[y].Count)
		{
			return list[y][x];
		}
		return string.Empty;
	}

	public int GetInt(int x, int y)
	{
		return int.Parse(Get(x, y));
	}

	public float GetFloat(int x, int y)
	{
		return float.Parse(Get(x, y));
	}

	public bool GetBool(int x, int y)
	{
		string text = Get(x, y);
		return !(text.ToLower() == "false") && !(text == "0");
	}
}
