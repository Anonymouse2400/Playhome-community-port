using System;
using UnityEngine;

public class AttachWear : MonoBehaviour
{
	public GameObject baseBoneRoot;

	private void Awake()
	{
		if (baseBoneRoot == null)
		{
			return;
		}
		AttachBoneWeight.Attach(baseBoneRoot, base.gameObject, true);
		DynamicBone[] componentsInChildren = GetComponentsInChildren<DynamicBone>(true);
		DynamicBoneCollider[] componentsInChildren2 = baseBoneRoot.GetComponentsInChildren<DynamicBoneCollider>(true);
		DynamicBone[] array = componentsInChildren;
		foreach (DynamicBone dynamicBone in array)
		{
			string text = dynamicBone.m_Root.name;
			Transform root = Transform_Utility.FindTransform(baseBoneRoot.transform, text);
			dynamicBone.m_Root = root;
			dynamicBone.m_Colliders.Clear();
			DynamicBoneCollider[] array2 = componentsInChildren2;
			foreach (DynamicBoneCollider item in array2)
			{
				dynamicBone.m_Colliders.Add(item);
			}
		}
	}
}
