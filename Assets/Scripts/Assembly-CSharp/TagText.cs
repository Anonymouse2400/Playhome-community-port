using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class TagText
{
	public class Attribute
	{
		public string name;

		public string valOriginal;

		public List<string> vals = new List<string>();

		public int ValsNum
		{
			get
			{
				return vals.Count;
			}
		}

		public Attribute(string name, string valOriginal)
		{
			Setup(name, valOriginal);
		}

		public void Setup(string name, string valOriginal)
		{
			if (valOriginal != null)
			{
				this.name = name;
				this.valOriginal = valOriginal;
				string[] array = valOriginal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					vals.Add(array[i]);
				}
			}
		}

		public void Add(string valOriginal)
		{
			this.valOriginal = this.valOriginal + "\n" + valOriginal;
			string[] array = valOriginal.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				vals.Add(array[i]);
			}
		}

		public bool GetVal(ref string val, int id)
		{
			if (id >= 0 && id < vals.Count)
			{
				val = vals[id];
				return true;
			}
			return false;
		}

		public bool GetVal(ref int val, int id)
		{
			if (id >= 0 && id < vals.Count)
			{
				try
				{
					val = int.Parse(vals[id]);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.Message + " " + vals[id]);
				}
				return true;
			}
			return false;
		}

		public bool GetVal(ref float val, int id)
		{
			if (id >= 0 && id < vals.Count)
			{
				try
				{
					val = float.Parse(vals[id]);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.Message + " " + vals[id]);
				}
				return true;
			}
			return false;
		}

		public bool GetVal(ref bool val, int id)
		{
			if (id >= 0 && id < vals.Count)
			{
				try
				{
					val = bool.Parse(vals[id]);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.Message + " " + vals[id]);
				}
				return true;
			}
			return false;
		}
	}

	public class Element
	{
		private Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();

		public string Tag { get; private set; }

		public Dictionary<string, Attribute> Attributes
		{
			get
			{
				return attributes;
			}
		}

		public Element()
		{
		}

		public Element(string tag)
		{
			Tag = tag;
		}

		public int Load(string tag, string[] strs, int i, string fileName)
		{
			Tag = tag;
			for (i++; i < strs.Length; i++)
			{
				string text = strs[i];
				if (text == string.Empty)
				{
					return i;
				}
				int num = text.Length + 1;
				for (int j = 0; j < separator.Length; j++)
				{
					int num2 = text.IndexOf(separator[j]);
					if (num2 != -1)
					{
						num = Mathf.Min(num, num2);
					}
				}
				if (num >= text.Length)
				{
					Debug.LogWarning("不明な内容 " + fileName + " " + i + "行目 " + text);
				}
				else
				{
					string name = text.Remove(num);
					string val = text.Remove(0, num + 1);
					AddAttribute(name, val);
				}
			}
			return i;
		}

		public void AddAttribute(string name, string val)
		{
			Attribute attribute = null;
			if (attributes.ContainsKey(name))
			{
				attribute = attributes[name];
				attribute.Add(val);
			}
			else
			{
				attribute = new Attribute(name, val);
			}
			attributes[name] = attribute;
		}

		public Attribute GetAttribute(string attribute)
		{
			if (attributes.ContainsKey(attribute))
			{
				return attributes[attribute];
			}
			return null;
		}

		public bool GetVal(ref string val, string attribute, int id)
		{
			if (attributes.ContainsKey(attribute))
			{
				return attributes[attribute].GetVal(ref val, id);
			}
			return false;
		}

		public bool GetVal(ref int val, string attribute, int id)
		{
			if (attributes.ContainsKey(attribute))
			{
				return attributes[attribute].GetVal(ref val, id);
			}
			return false;
		}

		public bool GetVal(ref float val, string attribute, int id)
		{
			if (attributes.ContainsKey(attribute))
			{
				return attributes[attribute].GetVal(ref val, id);
			}
			return false;
		}

		public bool GetVal(ref bool val, string attribute, int id)
		{
			if (attributes.ContainsKey(attribute))
			{
				return attributes[attribute].GetVal(ref val, id);
			}
			return false;
		}
	}

	private static readonly char[] separator = new char[2] { ':', ',' };

	private List<Element> elements = new List<Element>();

	public int ElementNum
	{
		get
		{
			return elements.Count;
		}
	}

	public List<Element> Elements
	{
		get
		{
			return elements;
		}
	}

	public TagText()
	{
	}

	public TagText(string file)
	{
		Load_Text(file);
	}

	public TagText(TextAsset text)
	{
		Load_Data(text.text, text.name);
	}

	public TagText(string assetbundleDir, string assetbundleName, string name)
	{
		Load_AssetBundle(assetbundleDir, assetbundleName, name);
	}

	public void Load_Resource(string file)
	{
		TextAsset textAsset = Resources.Load(file) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogWarning(file + "は開けませんでした");
		}
		else
		{
			Load_Data(textAsset.text, file);
		}
	}

	public void Load_AssetBundle(string assetbundleDir, string assetbundleName, string name)
	{
		TextAsset textAsset = AssetBundleLoader.LoadAsset<TextAsset>(assetbundleDir, assetbundleName, name);
		if (textAsset == null)
		{
			Debug.LogWarning(name + "は開けませんでした");
			return;
		}
		string fileName = Application.persistentDataPath + "/abdata" + "," + name;
		Load_Data(textAsset.text, fileName);
	}

	public void Load_TextAsset(TextAsset text)
	{
		Load_Data(text.text, text.name);
	}

	public void Load_Text(string file)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
		FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
		StreamReader streamReader = new StreamReader(fileStream);
		Load_Data(streamReader.ReadToEnd(), fileNameWithoutExtension);
		streamReader.Close();
		fileStream.Close();
	}

	private void Load_Data(string txtStr, string fileName)
	{
		Regex regex = new Regex("\\[(?<tag>.+)\\]", RegexOptions.IgnoreCase | RegexOptions.Singleline);
		string[] array = new string[1] { "\r\n" };
		string[] array2 = txtStr.Split(array, StringSplitOptions.None);
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			if (!(text == string.Empty) && text.IndexOf("//") != 0)
			{
				Match match = regex.Match(text);
				if (match.Success)
				{
					string value = match.Groups["tag"].Value;
					Element element = new Element();
					i = element.Load(value, array2, i, fileName);
					elements.Add(element);
				}
			}
		}
	}

	public void AddElement(Element element)
	{
		elements.Add(element);
	}

	public void Save(string file)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
		FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
		StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
		foreach (Element element in elements)
		{
			streamWriter.WriteLine("[" + element.Tag + "]");
			foreach (KeyValuePair<string, Attribute> attribute in element.Attributes)
			{
				streamWriter.Write(attribute.Key);
				streamWriter.Write(":");
				streamWriter.Write(attribute.Value.valOriginal);
				streamWriter.WriteLine();
			}
			streamWriter.WriteLine();
		}
		streamWriter.Close();
		fileStream.Close();
	}
}
