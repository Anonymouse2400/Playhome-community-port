using System;
using System.Collections.Generic;
using UnityEngine;

public class MapPlane : MonoBehaviour
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

	public Vector3 planePos;

	public Vector3 planeRot;

	public int setLayer = 16;

	private List<Data> dataList = new List<Data>();

	private Plane plane;

	private IllusionCamera cam;

	private void Awake()
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer rend in array)
		{
			dataList.Add(new Data(rend));
		}
	}

	private void Start()
	{
		cam = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		CalcPlane();
	}

	private void LateUpdate()
	{
		if (!(cam == null))
		{
			if (ConfigData.autoHideObstacle && !plane.GetSide(cam.transform.position) && plane.GetSide(cam.Target.transform.position))
			{
				Hide();
			}
			else
			{
				Show();
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		float num = 2f;
		Quaternion quaternion = base.transform.rotation * Quaternion.Euler(planeRot);
		Vector3 vector = quaternion * Vector3.forward;
		Vector3 pos = base.transform.TransformPoint(planePos);
		Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, quaternion, Vector3.one);
		Matrix4x4 matrix4x2 = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
		Gizmos.matrix = matrix4x2 * matrix4x;
		Gizmos.DrawCube(Vector3.zero, new Vector3(num, num, 0f));
		Gizmos.DrawLine(Vector3.zero, new Vector3(0f, 0f, num));
		Gizmos.matrix = Matrix4x4.identity;
	}

	private void OnValidate()
	{
		CalcPlane();
	}

	public void Show()
	{
		foreach (Data data in dataList)
		{
			data.Show();
		}
	}

	public void Hide()
	{
		foreach (Data data in dataList)
		{
			data.Hide(setLayer);
		}
	}

	private void CalcPlane()
	{
		Quaternion quaternion = base.transform.rotation * Quaternion.Euler(planeRot);
		Vector3 inNormal = quaternion * Vector3.forward;
		Vector3 inPoint = base.transform.TransformPoint(planePos);
		plane = new Plane(inNormal, inPoint);
	}
}
