using System;
using UnityEngine;

namespace H
{
	public class H_InputBase : MonoBehaviour
	{
		protected H_STATE state;

		protected float pose;

		protected float stroke;

		protected float speed;

		protected H_Members members;

		public float Pose
		{
			get
			{
				return pose;
			}
			set
			{
				pose = value;
			}
		}

		public float Stroke
		{
			get
			{
				return 0f - stroke;
			}
			set
			{
				stroke = 0f - value;
			}
		}

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		public virtual void SetMembers(H_Members members)
		{
			this.members = members;
			state = members.StateMgr.NowStateID;
		}

		public virtual void Update()
		{
			if (members != null && state != members.StateMgr.NowStateID)
			{
				ChangeState(members.StateMgr.NowStateID);
			}
		}

		protected virtual void ChangeState(H_STATE state)
		{
			this.state = state;
		}
	}
}
