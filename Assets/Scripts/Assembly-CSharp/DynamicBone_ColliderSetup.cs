using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBone_ColliderSetup : MonoBehaviour
{
	public Transform dynamicBoneRoot;

	public Transform colliderRoot;

	private DynamicBone[] dynamicBones;

	private DynamicBoneCollider[] dynamicBoneColliders;

	private void Awake()
	{
		dynamicBones = dynamicBoneRoot.GetComponentsInChildren<DynamicBone>();
		dynamicBoneColliders = colliderRoot.GetComponentsInChildren<DynamicBoneCollider>();
		DynamicBone[] array = dynamicBones;
		foreach (DynamicBone dynamicBone in array)
		{
			dynamicBone.m_Colliders = new List<DynamicBoneCollider>(dynamicBoneColliders);
		}
	}
}
