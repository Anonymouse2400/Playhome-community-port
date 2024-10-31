using System;
using UnityEngine;

namespace Utility
{
	public static class MaterialUtility
	{
		public static void SwapMaterials(Renderer rend, int no, Material mat)
		{
			Material[] materials = rend.materials;
			materials[no] = mat;
			rend.materials = materials;
		}

		public static void SwapSharedMaterials(Renderer rend, int no, Material mat)
		{
			Material[] sharedMaterials = rend.sharedMaterials;
			sharedMaterials[no] = mat;
			rend.sharedMaterials = sharedMaterials;
		}

		public static void AddSharedMaterials(Renderer rend, Material addMat)
		{
			Material[] sharedMaterials = rend.sharedMaterials;
			Material[] array = new Material[sharedMaterials.Length + 1];
			sharedMaterials.CopyTo(array, 0);
			int num = array.Length - 1;
			array[num] = addMat;
			rend.sharedMaterials = array;
		}
	}
}
