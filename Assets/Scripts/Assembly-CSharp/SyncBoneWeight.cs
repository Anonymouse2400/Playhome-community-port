using System;
using UnityEngine;

public class SyncBoneWeight : MonoBehaviour
{
	public GameObject baseBoneRoot;

	public GameObject attachMeshRoot;

	private void Awake()
	{
		AttachBoneWeight.Attach(baseBoneRoot, attachMeshRoot, true);
	}
}
