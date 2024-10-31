using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MaterialCustoms : MonoBehaviour
{
	[Serializable]
	public class Parameter
	{
		public enum TYPE
		{
			COLOR = 0,
			FLOAT01 = 1,
			FLOAT11 = 2,
			ALPHA = 3
		}

		public string name;

		public TYPE type;

		public string[] materialNames;

		public string propertyName;

		public Parameter()
		{
		}

		public Parameter(string name, TYPE type, string[] materialNames, string propertyName)
		{
			this.name = name;
			this.type = type;
			this.materialNames = new string[materialNames.Length];
			for (int i = 0; i < materialNames.Length; i++)
			{
				this.materialNames[i] = materialNames[i];
			}
			this.propertyName = propertyName;
		}

		public Parameter(Parameter copy)
		{
			name = copy.name;
			type = copy.type;
			materialNames = new string[copy.materialNames.Length];
			for (int i = 0; i < materialNames.Length; i++)
			{
				materialNames[i] = copy.materialNames[i];
			}
			propertyName = copy.propertyName;
		}
	}

	public class Data_Base
	{
		public Parameter param;

		protected List<Material> materials = new List<Material>();

		protected int propertyID;

		public Data_Base(Parameter param, Renderer[] rends)
		{
			propertyID = Shader.PropertyToID(param.propertyName);
			this.param = param;
			for (int i = 0; i < rends.Length; i++)
			{
				for (int j = 0; j < rends[i].sharedMaterials.Length; j++)
				{
					string materialName = GetMaterialName(rends[i].sharedMaterials[j]);
					for (int k = 0; k < param.materialNames.Length; k++)
					{
						if (materialName == param.materialNames[k])
						{
							materials.Add(rends[i].materials[j]);
						}
					}
				}
			}
		}

		private string GetMaterialName(Material material)
		{
			string text = ((UnityEngine.Object)(object)material).name;
			int num = text.IndexOf(" (Instance)");
			if (num != -1)
			{
				text = text.Remove(num);
			}
			return text;
		}
	}

	public class Data_Color : Data_Base
	{
		protected Color value;

		public Color Value
		{
			get
			{
				return value;
			}
			set
			{
				SetValue(value);
			}
		}

		public Data_Color(Parameter param, Renderer[] rends)
			: base(param, rends)
		{
			if (materials.Count > 0)
			{
				if (materials[0].HasProperty(propertyID))
				{
					value = materials[0].GetColor(propertyID);
				}
				else
				{
					Debug.LogWarning("シェーダプロパティがない:" + base.param.propertyName + " " + ((UnityEngine.Object)(object)materials[0]).name);
				}
			}
		}

		public void SetValue(Color value)
		{
			this.value = value;
			for (int i = 0; i < materials.Count; i++)
			{
				materials[i].SetColor(propertyID, value);
			}
		}
	}

	public class Data_Float : Data_Base
	{
		protected float value;

		protected float min;

		protected float max = 1f;

		public float Value
		{
			get
			{
				return value;
			}
			set
			{
				SetValue(value);
			}
		}

		public float Min
		{
			get
			{
				return min;
			}
		}

		public float Max
		{
			get
			{
				return max;
			}
		}

		public Data_Float(Parameter param, Renderer[] rends, float min, float max)
			: base(param, rends)
		{
			this.min = min;
			this.max = max;
			if (materials.Count > 0)
			{
				if (materials[0].HasProperty(propertyID))
				{
					value = materials[0].GetFloat(propertyID);
				}
				else
				{
					Debug.LogWarning("シェーダプロパティがない:" + base.param.propertyName);
				}
			}
		}

		public void SetValue(float value)
		{
			this.value = value;
			for (int i = 0; i < materials.Count; i++)
			{
				materials[i].SetFloat(propertyID, value);
			}
		}
	}

	public class Data_Alpha : Data_Base
	{
		protected float value;

		public float Value
		{
			get
			{
				return value;
			}
			set
			{
				SetValue(value);
			}
		}

		public Data_Alpha(Parameter param, Renderer[] rends)
			: base(param, rends)
		{
			if (materials.Count > 0)
			{
				if (materials[0].HasProperty(propertyID))
				{
					value = materials[0].GetColor(propertyID).a;
				}
				else
				{
					Debug.LogWarning("シェーダプロパティがない:" + base.param.propertyName);
				}
			}
		}

		public void SetValue(float value)
		{
			this.value = value;
			for (int i = 0; i < materials.Count; i++)
			{
				Color color = materials[i].GetColor(propertyID);
				color.a = value;
				materials[i].SetColor(propertyID, color);
			}
		}
	}

	public Parameter[] parameters;

	public Data_Base[] datas;

	private void Awake()
	{
		Setup();
	}

	private void Setup()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		datas = new Data_Base[parameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			Parameter parameter = parameters[i];
			if (parameter.type == Parameter.TYPE.FLOAT01)
			{
				datas[i] = new Data_Float(parameter, componentsInChildren, 0f, 1f);
			}
			else if (parameter.type == Parameter.TYPE.FLOAT11)
			{
				datas[i] = new Data_Float(parameter, componentsInChildren, -1f, 1f);
			}
			else if (parameter.type == Parameter.TYPE.COLOR)
			{
				datas[i] = new Data_Color(parameter, componentsInChildren);
			}
			else if (parameter.type == Parameter.TYPE.ALPHA)
			{
				datas[i] = new Data_Alpha(parameter, componentsInChildren);
			}
		}
	}

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
		}
	}

	private void Update()
	{
	}

	public void SetColor(string name, Color value)
	{
		for (int i = 0; i < datas.Length; i++)
		{
			if (datas[i].param.type == Parameter.TYPE.COLOR && datas[i].param.name == name)
			{
				Data_Color data_Color = datas[i] as Data_Color;
				data_Color.SetValue(value);
			}
		}
	}

	public void SetFloat(string name, float value)
	{
		for (int i = 0; i < datas.Length; i++)
		{
			if (datas[i].param.name == name)
			{
				if (datas[i].param.type == Parameter.TYPE.FLOAT01 || datas[i].param.type == Parameter.TYPE.FLOAT01)
				{
					Data_Float data_Float = datas[i] as Data_Float;
					data_Float.SetValue(value);
				}
				else if (datas[i].param.type == Parameter.TYPE.ALPHA)
				{
					Data_Alpha data_Alpha = datas[i] as Data_Alpha;
					data_Alpha.SetValue(value);
				}
			}
		}
	}

	public void Load(string filePath)
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(filePath);
		int attributeNo = customDataListLoader.GetAttributeNo("Name");
		int attributeNo2 = customDataListLoader.GetAttributeNo("Type");
		int attributeNo3 = customDataListLoader.GetAttributeNo("MaterialName");
		int attributeNo4 = customDataListLoader.GetAttributeNo("PropertyName");
		parameters = new Parameter[customDataListLoader.GetDataNum()];
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			string data3 = customDataListLoader.GetData(attributeNo3, i);
			string data4 = customDataListLoader.GetData(attributeNo4, i);
			Parameter.TYPE type = Parameter.TYPE.FLOAT01;
			if (data2.Equals("Alpha", StringComparison.OrdinalIgnoreCase))
			{
				type = Parameter.TYPE.ALPHA;
			}
			else if (data2.Equals("Color", StringComparison.OrdinalIgnoreCase))
			{
				type = Parameter.TYPE.COLOR;
			}
			else if (data2.Equals("Float01", StringComparison.OrdinalIgnoreCase))
			{
				type = Parameter.TYPE.FLOAT01;
			}
			else if (data2.Equals("Float11", StringComparison.OrdinalIgnoreCase))
			{
				type = Parameter.TYPE.FLOAT11;
			}
			string[] materialNames = data3.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			parameters[i] = new Parameter(data, type, materialNames, data4);
		}
	}

	public void Save(string filePath)
	{
		FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
		if (fileStream == null)
		{
			MonoBehaviour.print("読み込み失敗 : " + filePath);
			return;
		}
		StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.Write("Name");
		streamWriter.Write("\t");
		streamWriter.Write("Type");
		streamWriter.Write("\t");
		streamWriter.Write("MaterialName");
		streamWriter.Write("\t");
		streamWriter.Write("PropertyName");
		streamWriter.WriteLine();
		if (parameters != null)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				Parameter parameter = parameters[i];
				string text = string.Empty;
				for (int j = 0; j < parameter.materialNames.Length; j++)
				{
					text += parameter.materialNames[j];
					if (j < parameter.materialNames.Length - 1)
					{
						text += ",";
					}
				}
				streamWriter.Write(parameter.name);
				streamWriter.Write("\t");
				streamWriter.Write(parameter.type);
				streamWriter.Write("\t");
				streamWriter.Write(text);
				streamWriter.Write("\t");
				streamWriter.Write(parameter.propertyName);
				streamWriter.Write("\t");
				streamWriter.WriteLine();
			}
		}
		streamWriter.Close();
		fileStream.Close();
	}

	public Color GetColor(string name)
	{
		for (int i = 0; i < datas.Length; i++)
		{
			if (datas[i].param.type == Parameter.TYPE.COLOR && datas[i].param.name == name)
			{
				Data_Color data_Color = datas[i] as Data_Color;
				return data_Color.Value;
			}
		}
		return Color.white;
	}

	public float GetFloat(string name)
	{
		for (int i = 0; i < datas.Length; i++)
		{
			if (datas[i].param.name == name)
			{
				if (datas[i].param.type == Parameter.TYPE.FLOAT01 || datas[i].param.type == Parameter.TYPE.FLOAT01)
				{
					Data_Float data_Float = datas[i] as Data_Float;
					return data_Float.Value;
				}
				if (datas[i].param.type == Parameter.TYPE.ALPHA)
				{
					Data_Alpha data_Alpha = datas[i] as Data_Alpha;
					return data_Alpha.Value;
				}
			}
		}
		return 0f;
	}
}
