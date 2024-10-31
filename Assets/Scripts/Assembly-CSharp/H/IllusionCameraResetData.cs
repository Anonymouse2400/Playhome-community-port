using System;
using UnityEngine;

namespace H
{
	public class IllusionCameraResetData
	{
		public Vector3 pos;

		public Vector3 eul;

		public float dis;

		public void ResetCamera(IllusionCamera camera, Transform charaRoot)
		{
			Vector3 focus = charaRoot.TransformPoint(pos);
			Vector3 rotate = charaRoot.rotation.eulerAngles + eul;
			float num = 25f / ConfigData.defParse;
			camera.Set(focus, rotate, dis * num, ConfigData.defParse);
		}
	}
}
