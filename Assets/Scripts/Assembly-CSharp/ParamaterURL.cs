using System.Collections.Generic;

public class ParamaterURL
{
	private string baseURL;

	private Dictionary<string, string> paramater = new Dictionary<string, string>();

	public ParamaterURL(string baseURL)
	{
		this.baseURL = baseURL;
	}

	public void Add(string key, string val)
	{
		paramater.Add(key, val);
	}

	public string Get()
	{
		string text = baseURL;
		if (paramater.Count > 0)
		{
			text += "?";
		}
		int num = 0;
		foreach (KeyValuePair<string, string> item in paramater)
		{
			if (num != 0)
			{
				text += "&";
			}
			text = text + item.Key + "=" + item.Value;
			num++;
		}
		return text;
	}
}
