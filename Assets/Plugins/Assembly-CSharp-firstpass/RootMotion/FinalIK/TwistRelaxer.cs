using UnityEngine;

namespace RootMotion.FinalIK
{
	public class TwistRelaxer : MonoBehaviour
	{
		[Tooltip("The weight of relaxing the twist of this Transform")]
		[Range(0f, 1f)]
		public float weight = 1f;

		[Tooltip("If 0.5, this Transform will be twisted half way from parent to child. If 1, the twist angle will be locked to the child and will rotate with along with it.")]
		[Range(0f, 1f)]
		public float parentChildCrossfade = 0.5f;

		[Tooltip("The parent Transform, does not need to be the actual transform.parent.")]
		public Transform parent;

		[Tooltip("The child Transform, does not need to be the direct child, you can skip bones in the hierarchy.")]
		public Transform child;

		[Tooltip("The local axis of this Transform that it will be twisted around (the axis pointing towards the parent).")]
		public Vector3 twistAxis = Vector3.right;

		[Tooltip("Another axis, orthogonal to twistAxis.")]
		public Vector3 axis = Vector3.forward;

		private Vector3 axisRelativeToParentDefault;

		private Vector3 axisRelativeToChildDefault;

		public void Relax()
		{
			if (!(weight <= 0f))
			{
				Vector3 a = parent.rotation * axisRelativeToParentDefault;
				Vector3 b = child.rotation * axisRelativeToChildDefault;
				Vector3 vector = Vector3.Slerp(a, b, parentChildCrossfade);
				Quaternion rotation = Quaternion.LookRotation(base.transform.rotation * axis, base.transform.rotation * twistAxis);
				vector = Quaternion.Inverse(rotation) * vector;
				float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				Quaternion rotation2 = child.rotation;
				base.transform.rotation = Quaternion.AngleAxis(num * weight, base.transform.rotation * twistAxis) * base.transform.rotation;
				child.rotation = rotation2;
			}
		}

		private void Start()
		{
			Vector3 vector = base.transform.rotation * axis;
			axisRelativeToParentDefault = Quaternion.Inverse(parent.rotation) * vector;
			axisRelativeToChildDefault = Quaternion.Inverse(child.rotation) * vector;
		}

		private void LateUpdate()
		{
			Relax();
		}
	}
}
