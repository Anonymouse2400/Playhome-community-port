using System;
using UnityEngine;

public class TagTextUtility
{
	public static float Load_Float(TagText.Attribute attribute, int id = 0)
	{
		if (id >= attribute.vals.Count)
		{
			return 0f;
		}
		return float.Parse(attribute.vals[id]);
	}

	public static float Load_Float(TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			return Load_Float(element.Attributes[attribute], id);
		}
		return 0f;
	}

	public static bool Load_Float(ref float val, TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			val = Load_Float(element.Attributes[attribute], id);
			return true;
		}
		return false;
	}

	public static bool Load_Bool(TagText.Attribute attribute, int id = 0)
	{
		if (id >= attribute.vals.Count)
		{
			return false;
		}
		bool result = false;
		try
		{
			result = bool.Parse(attribute.vals[id]);
		}
		catch (Exception ex)
		{
			Debug.LogError(attribute.vals[id] + ":" + ex);
		}
		return result;
	}

	public static bool Load_Bool(TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			return Load_Bool(element.Attributes[attribute], id);
		}
		return false;
	}

	public static bool Load_Bool(ref bool val, TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			val = Load_Bool(element.Attributes[attribute], id);
			return true;
		}
		return false;
	}

	public static string Load_String(TagText.Attribute attribute, int id = 0)
	{
		if (id == -1)
		{
			return attribute.valOriginal;
		}
		if (id < attribute.vals.Count)
		{
			return attribute.vals[id];
		}
		return string.Empty;
	}

	public static string Load_String(TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			return Load_String(element.Attributes[attribute], id);
		}
		return null;
	}

	public static bool Load_String(ref string str, TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			str = Load_String(element.Attributes[attribute], id);
			return true;
		}
		return false;
	}

	public static Vector3 Load_Vector3(TagText.Attribute attribute, int id = 0)
	{
		return new Vector3(Load_Float(attribute, id), Load_Float(attribute, id + 1), Load_Float(attribute, id + 2));
	}

	public static Vector3 Load_Vector3(TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			return Load_Vector3(element.Attributes[attribute], id);
		}
		return new Vector3(0f, 0f, 0f);
	}

	public static bool Load_Vector3(ref Vector3 vec, TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			vec = Load_Vector3(element.Attributes[attribute], id);
			return true;
		}
		return false;
	}

	public static void Save_Vector3(TagText.Element element, string attribute, Vector3 vec)
	{
		string val = vec.x + "," + vec.y + "," + vec.z;
		element.AddAttribute(attribute, val);
	}

	public static Vector2 Load_Vector2(TagText.Attribute attribute, int id = 0)
	{
		return new Vector2(Load_Float(attribute, id), Load_Float(attribute, id + 1));
	}

	public static Vector2 Load_Vector2(TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			return Load_Vector2(element.Attributes[attribute], id);
		}
		return new Vector2(0f, 0f);
	}

	public static bool Load_Vector2(ref Vector2 vec, TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			vec = Load_Vector2(element.Attributes[attribute], id);
			return true;
		}
		return false;
	}

	public static void Save_Vector2(TagText.Element element, string attribute, Vector2 vec)
	{
		string val = vec.x + "," + vec.y;
		element.AddAttribute(attribute, val);
	}

	public static Color Load_Color(TagText.Attribute attribute, int id = 0)
	{
		Color result = default(Color);
		result.r = Load_Float(attribute, id);
		result.g = Load_Float(attribute, id + 1);
		result.b = Load_Float(attribute, id + 2);
		if (attribute.vals.Count >= 4)
		{
			result.a = Load_Float(attribute, id + 3);
		}
		else
		{
			result.a = 1f;
		}
		return result;
	}

	public static Color Load_Color(TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			return Load_Color(element.Attributes[attribute], id);
		}
		return new Color(0f, 0f, 0f);
	}

	public static bool Load_Color(ref Color vec, TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			vec = Load_Color(element.Attributes[attribute], id);
			return true;
		}
		return false;
	}

	public static void Save_Color(TagText.Element element, string attribute, Color color)
	{
		string val = color.r + "," + color.g + "," + color.b + "," + color.a;
		element.AddAttribute(attribute, val);
	}

	public static Quaternion Load_Quaternion(TagText.Attribute attribute, int id = 0)
	{
		return new Quaternion(Load_Float(attribute, id), Load_Float(attribute, id + 1), Load_Float(attribute, id + 2), Load_Float(attribute, id + 3));
	}

	public static Quaternion Load_Quaternion(TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			return Load_Quaternion(element.Attributes[attribute], id);
		}
		return Quaternion.identity;
	}

	public static bool Load_Quaternion(ref Quaternion qtn, TagText.Element element, string attribute, int id = 0)
	{
		if (element.Attributes.ContainsKey(attribute))
		{
			qtn = Load_Quaternion(element.Attributes[attribute], id);
			return true;
		}
		return false;
	}

	public static void Save_Quaternion(TagText.Element element, string attribute, Quaternion qtn)
	{
		string val = qtn.x + "," + qtn.y + "," + qtn.z + "," + qtn.w;
		element.AddAttribute(attribute, val);
	}

	public static bool Check_Attribute(TagText.Element element, string attribute)
	{
		return element.Attributes.ContainsKey(attribute);
	}
}
