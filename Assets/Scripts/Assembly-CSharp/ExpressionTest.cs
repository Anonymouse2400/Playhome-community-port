using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Human))]
public class ExpressionTest : MonoBehaviour
{
	private class Data
	{
		public string name;

		public string anime;

		public Data(string name, string anime)
		{
			this.name = name;
			this.anime = anime;
		}
	}

	[SerializeField]
	private TextAsset eyeList;

	[SerializeField]
	private TextAsset mouthList;

	[SerializeField]
	private float duration = 0.2f;

	[SerializeField]
	[Range(0f, 1f)]
	private float openEye = 1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float openMouth;

	[SerializeField]
	private Transform lookTarget;

	private List<Data> eyeDatas = new List<Data>();

	private List<Data> mouthDatas = new List<Data>();

	private Human human;

	private void Start()
	{
		Setup(eyeDatas, eyeList);
		Setup(mouthDatas, mouthList);
		human = GetComponent<Human>();
	}

	private void LateUpdate()
	{
		human.blink.enabled = false;
		human.OpenEye(openEye);
		human.OpenMouth(openMouth);
		if (lookTarget != null)
		{
			human.ChangeEyeLook(LookAtRotator.TYPE.TARGET, lookTarget, false);
			human.ChangeNeckLook(LookAtRotator.TYPE.TARGET, lookTarget, false);
		}
		else
		{
			human.ChangeEyeLook(LookAtRotator.TYPE.NO, null, false);
			human.ChangeNeckLook(LookAtRotator.TYPE.NO, null, false);
		}
	}

	private static void Setup(List<Data> datas, TextAsset text)
	{
		CustomDataListLoader customDataListLoader = new CustomDataListLoader();
		customDataListLoader.Load(text);
		int attributeNo = customDataListLoader.GetAttributeNo("anime");
		int attributeNo2 = customDataListLoader.GetAttributeNo("name");
		for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
		{
			string data = customDataListLoader.GetData(attributeNo, i);
			string data2 = customDataListLoader.GetData(attributeNo2, i);
			datas.Add(new Data(data2, data));
		}
	}

	public void ChangeEye(int no)
	{
		if (no >= 0 && no < eyeDatas.Count)
		{
			human.ExpressionPlay(0, eyeDatas[no].anime, duration);
		}
	}

	public void ChangeMouth(int no)
	{
		if (no >= 0 && no < mouthDatas.Count)
		{
			human.ExpressionPlay(1, mouthDatas[no].anime, duration);
		}
	}

	public string[] GetEyeNames()
	{
		string[] array = new string[eyeDatas.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = eyeDatas[i].name;
		}
		return array;
	}

	public string[] GetMouthNames()
	{
		string[] array = new string[mouthDatas.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = mouthDatas[i].name;
		}
		return array;
	}
}
