using System;
using UnityEngine;

public class CutActionEdit_Camera : CutActionEdit
{
	private CutAct_Camera actCamera;

	[SerializeField]
	private Camera cam;

	public override void Setup(CutAction act)
	{
		actCamera = act as CutAct_Camera;
	}

	public void SaveNowCamera()
	{
		actCamera.pos = cam.transform.position;
		actCamera.rot = cam.transform.rotation;
		actCamera.fov = cam.fieldOfView;
	}
}
