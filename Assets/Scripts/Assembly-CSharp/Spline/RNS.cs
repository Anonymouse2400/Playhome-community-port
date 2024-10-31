using System;
using UnityEngine;

namespace Spline
{
	public class RNS
	{
		public struct splineData
		{
			public Vector3 position;

			public Vector3 velocity;

			public float distance;
		}

		public splineData[] node = new splineData[100];

		public float maxDistance;

		public int nodeCount;

		public void Init()
		{
			nodeCount = 0;
		}

		public void AddNode(Vector3 pos)
		{
			if (nodeCount == 0)
			{
				maxDistance = 0f;
			}
			else
			{
				node[nodeCount - 1].distance = Vector3_Length(node[nodeCount - 1].position - pos);
				maxDistance += node[nodeCount - 1].distance;
			}
			node[nodeCount++].position = pos;
		}

		protected void RemoveHeadNode()
		{
			if (nodeCount != 0)
			{
				maxDistance -= node[0].distance;
				for (int i = 0; i < nodeCount - 1; i++)
				{
					node[i] = node[i + 1];
				}
				nodeCount--;
			}
		}

		public virtual void BuildSpline()
		{
			Build();
		}

		protected void Build()
		{
			for (int i = 1; i < nodeCount - 1; i++)
			{
				Vector3 vector = Vector3.Normalize(node[i + 1].position - node[i].position);
				Vector3 vector2 = Vector3.Normalize(node[i - 1].position - node[i].position);
				node[i].velocity = vector - vector2;
				node[i].velocity.Normalize();
			}
			node[0].velocity = GetStartVelocity(0);
			node[nodeCount - 1].velocity = GetEndVelocity(nodeCount - 1);
		}

		protected void BuildLast()
		{
			int num = nodeCount - 2;
			node[num].velocity = Vector3.Normalize(node[num + 1].position - node[num].position) - Vector3.Normalize(node[num - 1].position - node[num].position);
			node[num].velocity.Normalize();
			node[nodeCount - 1].velocity = GetEndVelocity(nodeCount - 1);
		}

		public virtual float RenewMove(Vector3 pos, float time)
		{
			int prevNo;
			int nextNo;
			float rate;
			TimeState(time, out prevNo, out nextNo, out rate);
			RemoveHeadNode();
			AddNode(pos);
			BuildLast();
			return GetTime(prevNo - 1, rate);
		}

		public Vector3 GetPosition(float time)
		{
			if (time <= 0f)
			{
				return node[0].position;
			}
			if (time >= 1f)
			{
				return node[nodeCount - 1].position;
			}
			float num = time * maxDistance;
			float num2 = 0f;
			int i;
			for (i = 0; i < nodeCount && num2 + node[i].distance < num; i++)
			{
				num2 += node[i].distance;
			}
			if (i == nodeCount)
			{
				i = nodeCount - 1;
			}
			float num3 = num - num2;
			num3 /= node[i].distance;
			Vector3 startVel = node[i].velocity * node[i].distance;
			Vector3 endVel = node[i + 1].velocity * node[i].distance;
			return GetPositionOnCubic(node[i].position, startVel, node[i + 1].position, endVel, num3);
		}

		protected void TimeState(float time, out int prevNo, out int nextNo, out float rate)
		{
			float num = time * maxDistance;
			float num2 = 0f;
			int i;
			for (i = 0; i < nodeCount && num2 + node[i].distance < num; i++)
			{
				num2 += node[i].distance;
			}
			if (i == nodeCount)
			{
				i = nodeCount - 1;
			}
			float num3 = num - num2;
			num3 /= node[i].distance;
			prevNo = i;
			nextNo = i + 1;
			rate = num3;
		}

		protected float GetTime(int prevNo, float rate)
		{
			float num = 0f;
			for (int i = 0; i < prevNo; i++)
			{
				num += node[i].distance;
			}
			num += node[prevNo].distance * rate;
			return num / maxDistance;
		}

		protected Vector3 GetStartVelocity(int index)
		{
			if (node[index].distance == 0f)
			{
				return Vector3.zero;
			}
			Vector3 vector = 3f * (node[index + 1].position - node[index].position) / node[index].distance;
			return (vector - node[index + 1].velocity) * 0.5f;
		}

		protected Vector3 GetEndVelocity(int index)
		{
			if (node[index - 1].distance == 0f)
			{
				return Vector3.zero;
			}
			Vector3 vector = 3f * (node[index].position - node[index - 1].position) / node[index - 1].distance;
			return (vector - node[index - 1].velocity) * 0.5f;
		}

		protected static Vector3 GetPositionOnCubic(Vector3 startPos, Vector3 startVel, Vector3 endPos, Vector3 endVel, float time)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x.m00 = 2f;
			matrix4x.m01 = -2f;
			matrix4x.m02 = 1f;
			matrix4x.m03 = 1f;
			matrix4x.m10 = -3f;
			matrix4x.m11 = 3f;
			matrix4x.m12 = -2f;
			matrix4x.m13 = -1f;
			matrix4x.m20 = 0f;
			matrix4x.m21 = 0f;
			matrix4x.m22 = 1f;
			matrix4x.m23 = 0f;
			matrix4x.m30 = 1f;
			matrix4x.m31 = 0f;
			matrix4x.m32 = 0f;
			matrix4x.m33 = 0f;
			Matrix4x4 matrix4x2 = default(Matrix4x4);
			matrix4x2.m00 = startPos.x;
			matrix4x2.m01 = startPos.y;
			matrix4x2.m02 = startPos.z;
			matrix4x2.m03 = 1f;
			matrix4x2.m10 = endPos.x;
			matrix4x2.m11 = endPos.y;
			matrix4x2.m12 = endPos.z;
			matrix4x2.m13 = 1f;
			matrix4x2.m20 = startVel.x;
			matrix4x2.m21 = startVel.y;
			matrix4x2.m22 = startVel.z;
			matrix4x2.m23 = 1f;
			matrix4x2.m30 = endVel.x;
			matrix4x2.m31 = endVel.y;
			matrix4x2.m32 = endVel.z;
			matrix4x2.m33 = 1f;
			matrix4x2 = matrix4x * matrix4x2;
			Vector4 v = new Vector4(time * time * time, time * time, time, 1f);
			Matrix4x4 identity = Matrix4x4.identity;
			identity.SetRow(3, v);
			return (identity * matrix4x2).GetRow(3);
		}

		private float Vector3_Length(Vector3 v)
		{
			return v.magnitude;
		}
	}
}
