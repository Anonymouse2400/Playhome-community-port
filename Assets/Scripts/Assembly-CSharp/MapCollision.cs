using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MapCollision : MonoBehaviour
{
	private class Data
	{
		private Renderer rend;

		private int layer;

		public Data(Renderer rend)
		{
			this.rend = rend;
			layer = rend.gameObject.layer;
		}

		public void Show()
		{
			rend.gameObject.layer = layer;
		}

		public void Hide(int setLayer)
		{
			rend.gameObject.layer = setLayer;
		}
	}

	private Collider[] cols;

	private Plane plane;

	private IllusionCamera cam;

	private int hitCnt;

	private bool show;

	private List<Data> dataList = new List<Data>();

	public int setLayer = 16;

	private void Awake()
	{
		cols = base.gameObject.GetComponents<Collider>();
		if (cols == null)
		{
			MonoBehaviour.print("当たり判定がない");
		}
		hitCnt = 0;
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer rend in array)
		{
			dataList.Add(new Data(rend));
		}
		show = true;
	}

	private void Start()
	{
		cam = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		hitCnt = 0;
	}

	private void Update()
	{
		ShowCalc();
	}

	public void Show()
	{
		if (show)
		{
			return;
		}
		show = true;
		foreach (Data data in dataList)
		{
			data.Show();
		}
	}

	public void Hide()
	{
		if (!show)
		{
			return;
		}
		show = false;
		foreach (Data data in dataList)
		{
			data.Hide(setLayer);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == null)
		{
			MonoBehaviour.print("other.gameObject == null");
		}
		if (cam == null)
		{
			MonoBehaviour.print("cam == null");
			return;
		}
		if (cam.gameObject == null)
		{
			MonoBehaviour.print("cam.gameObject == null");
		}
		if (!(other.gameObject == null) && !(cam.gameObject == null) && other.gameObject == cam.gameObject)
		{
			hitCnt = 1;
			ShowCalc();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject == null)
		{
			MonoBehaviour.print("other.gameObject == null");
		}
		if (cam.gameObject == null)
		{
			MonoBehaviour.print("cam.gameObject == null");
		}
		if (!(other.gameObject == null) && !(cam.gameObject == null) && other.gameObject == cam.gameObject)
		{
			hitCnt = 0;
			ShowCalc();
		}
	}

	private void ShowCalc()
	{
		bool flag = true;
		bool flag2 = !(cam != null) || ConfigData.autoHideObstacle;
		bool flag3 = flag;
		if (flag2 && hitCnt != 0)
		{
			flag3 = false;
		}
		if (flag3)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
}
