using System;
using UnityEngine;

namespace Spline
{
	internal class SNS : RNS
	{
		public override void BuildSpline()
		{
			Build();
			Smooth();
			Smooth();
			Smooth();
		}

		protected virtual void Smooth()
		{
			Smoothing();
		}

		protected void Smoothing()
		{
			Vector3 velocity = GetStartVelocity(0);
			for (int i = 1; i < nodeCount - 1; i++)
			{
				Vector3 vector = GetEndVelocity(i) * node[i].distance + GetStartVelocity(i) * node[i - 1].distance;
				vector /= node[i - 1].distance + node[i].distance;
				node[i - 1].velocity = velocity;
				velocity = vector;
			}
			node[nodeCount - 1].velocity = GetEndVelocity(nodeCount - 1);
			node[nodeCount - 2].velocity = velocity;
		}

		protected virtual void SmoothingLast()
		{
			node[nodeCount - 1].velocity = GetEndVelocity(nodeCount - 1);
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
