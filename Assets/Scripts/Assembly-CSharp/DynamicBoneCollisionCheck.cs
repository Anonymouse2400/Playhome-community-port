using System;
using UnityEngine;

public class DynamicBoneCollisionCheck : MonoBehaviour
{
	public void SetupCollision()
	{
		DynamicBone[] componentsInChildren = GetComponentsInChildren<DynamicBone>();
		DynamicBone_Ver01[] componentsInChildren2 = GetComponentsInChildren<DynamicBone_Ver01>();
		DynamicBone_Ver02[] componentsInChildren3 = GetComponentsInChildren<DynamicBone_Ver02>();
		DynamicBoneCollider[] array = UnityEngine.Object.FindObjectsOfType<DynamicBoneCollider>();
		DynamicBone[] array2 = componentsInChildren;
		foreach (DynamicBone dynamicBone in array2)
		{
			dynamicBone.m_Colliders.Clear();
			DynamicBoneCollider[] array3 = array;
			foreach (DynamicBoneCollider item in array3)
			{
				dynamicBone.m_Colliders.Add(item);
			}
		}
		DynamicBone_Ver01[] array4 = componentsInChildren2;
		foreach (DynamicBone_Ver01 dynamicBone_Ver in array4)
		{
			dynamicBone_Ver.m_Colliders.Clear();
			DynamicBoneCollider[] array5 = array;
			foreach (DynamicBoneCollider item2 in array5)
			{
				dynamicBone_Ver.m_Colliders.Add(item2);
			}
		}
		DynamicBone_Ver02[] array6 = componentsInChildren3;
		foreach (DynamicBone_Ver02 dynamicBone_Ver2 in array6)
		{
			dynamicBone_Ver2.Colliders.Clear();
			DynamicBoneCollider[] array7 = array;
			foreach (DynamicBoneCollider item3 in array7)
			{
				dynamicBone_Ver2.Colliders.Add(item3);
			}
		}
	}
}
