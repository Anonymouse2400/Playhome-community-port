using System;
using System.Collections.Generic;
using UnityEngine;

public class AttachBoneWeight
{
	public static void Attach(GameObject baseBoneRoot, GameObject attachMeshRoot, bool includeInactive)
	{
		Dictionary<string, Transform> bones = new Dictionary<string, Transform>();
		ListupBones(ref bones, baseBoneRoot.transform, includeInactive);
		SetupRenderers(bones, attachMeshRoot, includeInactive);
	}

	private static void ListupBones(ref Dictionary<string, Transform> bones, Transform trans, bool includeInactive)
	{
		if (trans == null || trans.gameObject == null)
		{
			MonoBehaviour.print("無効な骨");
			return;
		}
		bool flag = trans.gameObject.activeSelf && trans.gameObject.activeInHierarchy;
		if ((includeInactive || flag) && !bones.ContainsKey(trans.name))
		{
			bones.Add(trans.name, trans);
		}
		for (int i = 0; i < trans.childCount; i++)
		{
			ListupBones(ref bones, trans.GetChild(i), includeInactive);
		}
	}

	private static void SetupRenderers(Dictionary<string, Transform> bones, GameObject attachObj, bool includeInactive)
	{
		SkinnedMeshRenderer[] componentsInChildren = attachObj.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive);
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer renderer in array)
		{
			SetupRenderer(bones, renderer);
		}
	}

	private static void SetupRenderer(Dictionary<string, Transform> bones, SkinnedMeshRenderer renderer)
	{
		string name = renderer.rootBone.name;
		if (!bones.ContainsKey(name))
		{
			Debug.LogWarning("ルートボーンが見つかりません:" + name);
			return;
		}
		Transform rootBone = bones[name];
		renderer.rootBone = rootBone;
		Transform[] array = new Transform[renderer.bones.Length];
		for (int i = 0; i < renderer.bones.Length; i++)
		{
			Transform transform = renderer.bones[i];
			string name2 = transform.name;
			if (bones.ContainsKey(name2))
			{
				array[i] = bones[name2];
				continue;
			}
			Transform parent = renderer.bones[i].parent;
			if (parent != null && bones.ContainsKey(parent.name))
			{
				transform.SetParent(bones[parent.name]);
			}
			Debug.LogWarning("アタッチするボーンが見つかりません:" + name2);
			array[i] = null;
		}
		renderer.bones = array;
	}
}
