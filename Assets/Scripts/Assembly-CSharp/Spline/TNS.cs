using System;
using UnityEngine;

namespace Spline
{
	internal class TNS : SNS
	{
		public void AddNode(Vector3 pos, float timePeriod)
		{
			if (nodeCount == 0)
			{
				maxDistance = 0f;
			}
			else
			{
				node[nodeCount - 1].distance = timePeriod;
				maxDistance += node[nodeCount - 1].distance;
			}
			node[nodeCount++].position = pos;
		}

		public override void BuildSpline()
		{
			Build();
			Smooth();
			Smooth();
			Smooth();
		}

		protected override void Smooth()
		{
			Smoothing();
			Constrain();
		}

		public void Constrain()
		{
			for (int i = 1; i < nodeCount - 1; i++)
			{
				float num = Vector3.Distance(node[i].position, node[i - 1].position) / node[i - 1].distance;
				float num2 = Vector3.Distance(node[i + 1].position, node[i].position) / node[i].distance;
				node[i].velocity *= 4f * num * num2 / ((num + num2) * (num + num2));
			}
		}

		protected void ConstrainLast()
		{
			int num = nodeCount - 2;
			float num2 = Vector3.Distance(node[num].position, node[num - 1].position) / node[num - 1].distance;
			float num3 = Vector3.Distance(node[num + 1].position, node[num].position) / node[num].distance;
			node[num].velocity *= 4f * num2 * num3 / ((num2 + num3) * (num2 + num3));
		}

		protected override void SmoothingLast()
		{
			node[nodeCount - 1].velocity = GetEndVelocity(nodeCount - 1);
			ConstrainLast();
		}

		public override float RenewMove(Vector3 pos, float time)
		{
			int prevNo;
			int nextNo;
			float rate;
			TimeState(time, out prevNo, out nextNo, out rate);
			RemoveHeadNode();
			AddNode(pos);
			BuildLast();
			SmoothingLast();
			SmoothingLast();
			SmoothingLast();
			return GetTime(prevNo - 1, rate);
		}
	}
}
