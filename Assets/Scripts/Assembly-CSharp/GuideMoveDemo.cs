using System;
using UnityEngine;

public class GuideMoveDemo : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;

	[SerializeField]
	private GuideDriveManager guideOriginal;

	[SerializeField]
	private GuidesManager guidesManager;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void CreateObj()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(obj);
		guidesManager.Add(gameObject.transform);
	}
}
