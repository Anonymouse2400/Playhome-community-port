using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomDataListLoader
{
	private char[] separator = new char[1] { '\t' };

	private List<string> attributesList = new List<string>();

	private Dictionary<string, int> attributeDictionary = new Dictionary<string, int>();

	private List<string[]> datas = new List<string[]>();

	public void SetSeparator(char separator)
	{
		this.separator = new char[1] { separator };
	}

	public void SetSeparator(char[] separator)
	{
		this.separator = separator;
	}

	public bool Load(TextAsset text)
	{
		StringReader stringReader = new StringReader(text.text);
		bool result = Load(stringReader);
		stringReader.Close();
		return result;
	}

	public bool Load(string fullPath)
	{
		FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
		if (fileStream == null)
		{
			MonoBehaviour.print("読み込み失敗 : " + fullPath);
			return false;
		}
		StreamReader streamReader = new StreamReader(fileStream);
		bool result = Load(streamReader);
		streamReader.Close();
		return result;
	}

	private bool Load(TextReader reader)
	{
		string text = null;
		text = reader.ReadLine();
		if (text == null)
		{
			return false;
		}
		string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			attributesList.Add(array[i]);
			attributeDictionary.Add(array[i], i);
		}
		int count = attributesList.Count;
		while ((text = reader.ReadLine()) != null)
		{
			if (text.Length == 0)
			{
				continue;
			}
			array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != count)
			{
				Debug.LogWarning("リストが不完全:" + text);
				continue;
			}
			string[] array2 = new string[count];
			for (int j = 0; j < count; j++)
			{
				array2[j] = array[j];
			}
			datas.Add(array2);
		}
		reader.Close();
		return true;
	}

	public int GetAttributeNo(string attribute)
	{
		if (attributeDictionary.ContainsKey(attribute))
		{
			return attributeDictionary[attribute];
		}
		return -1;
	}

	public string GetData(string attribute, int y)
	{
		int attributeNo = GetAttributeNo(attribute);
		if (attributeNo == -1)
		{
			Debug.LogError("不明");
			return string.Empty;
		}
		return GetData(attributeNo, y);
	}

	public string GetData(int x, int y)
	{
		return datas[y][x];
	}

	public int GetDataNum()
	{
		return datas.Count;
	}
}
