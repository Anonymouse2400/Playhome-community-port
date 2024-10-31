using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class NetWWW : MonoBehaviour
{
	public delegate void ActWWW(WWW www);

	public float timeOut = 10f;

	[SerializeField]
	private GameObject connectingRoot;

	[SerializeField]
	private Text message;

	public bool NowConnecting { get; private set; }

	private void Awake()
	{
		connectingRoot.SetActive(NowConnecting);
	}

	public WWW Get(string url, string msg, ActWWW completeAct = null, ActWWW faildAct = null)
	{
		WWW wWW = new WWW(url);
		StartCoroutine(Wait(wWW, msg, completeAct, faildAct));
		return wWW;
	}

	public WWW Post(string url, string msg, WWWForm form, ActWWW completeAct = null, ActWWW faildAct = null)
	{
		WWW wWW = new WWW(url, form);
		StartCoroutine(Wait(wWW, msg, completeAct, faildAct));
		return wWW;
	}

	public WWW Post(string url, string msg, Dictionary<string, string> post, ActWWW completeAct = null, ActWWW faildAct = null)
	{
		WWWForm wWWForm = new WWWForm();
		foreach (KeyValuePair<string, string> item in post)
		{
			wWWForm.AddField(item.Key, item.Value);
		}
		return Post(url, msg, wWWForm, completeAct, faildAct);
	}

	private IEnumerator Wait(WWW www, string msg, ActWWW completeAct = null, ActWWW faildAct = null)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();
		NowConnecting = true;
		connectingRoot.SetActive(NowConnecting);
		if ((Object)(object)message != null)
		{
			message.text = msg;
		}
		bool isTimeOut = false;
		float timer = 0f;
		while (!www.isDone)
		{
			if (timer > timeOut)
			{
				isTimeOut = true;
				break;
			}
			timer += Time.deltaTime;
			yield return null;
		}
		bool success = !isTimeOut && string.IsNullOrEmpty(www.error);
		NowConnecting = false;
		connectingRoot.SetActive(NowConnecting);
		sw.Stop();
		float sec = (float)sw.ElapsedMilliseconds * 0.001f;
		UnityEngine.Debug.Log("通信時間:" + sec);
		if (!success)
		{
			if (faildAct != null)
			{
				faildAct(www);
				www.Dispose();
			}
		}
		else if (completeAct != null)
		{
			completeAct(www);
		}
	}
}
