using System;
using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueueRecords")]
public class SetRenderQueueRecords : MonoBehaviour
{
	private class Data
	{
		public string mesh;

		public int[] prioritys;

		public Data(string mesh, int[] prioritys)
		{
			this.mesh = mesh;
			this.prioritys = prioritys;
		}

		public Data(SetRenderQueue queue)
		{
			mesh = queue.gameObject.name;
			int[] array = queue.Get();
			prioritys = new int[array.Length];
			array.CopyTo(prioritys, 0);
		}
	}

	public TextAsset csv;
}
