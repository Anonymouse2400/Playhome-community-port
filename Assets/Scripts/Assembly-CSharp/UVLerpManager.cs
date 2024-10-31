using System;
using UnityEngine;

public class UVLerpManager
{
	private UVData targetData;

	private Mesh mesh_nipL;

	private Mesh mesh_nipR;

	private static readonly string[] uvName = new string[4] { "cf_O_tikubiL_00", "cf_O_tikubiR_00", "cf_O_tikubiL_01", "cf_O_tikubiR_01" };

	private int idx_L_Min;

	private int idx_R_Min = 2;

	private int idx_L_Max = 1;

	private int idx_R_Max = 3;

	public UVLerpManager(UVNormalBlend blend, UVData targetData)
	{
		this.targetData = targetData;
		for (int i = 0; i < targetData.data.Count; i++)
		{
			if (targetData.data[i].ObjectName == uvName[0])
			{
				idx_L_Min = i;
			}
			else if (targetData.data[i].ObjectName == uvName[1])
			{
				idx_R_Min = i;
			}
			else if (targetData.data[i].ObjectName == uvName[2])
			{
				idx_L_Max = i;
			}
			else if (targetData.data[i].ObjectName == uvName[3])
			{
				idx_R_Max = i;
			}
		}
		mesh_nipL = blend.GetClonedMesh(uvName[0]);
		mesh_nipR = blend.GetClonedMesh(uvName[1]);
	}

	public void Lerp(float rate)
	{
		UVData.Param param = targetData.data[idx_L_Min];
		UVData.Param param2 = targetData.data[idx_R_Min];
		UVData.Param param3 = targetData.data[idx_L_Max];
		UVData.Param param4 = targetData.data[idx_R_Max];
		Vector2[] array = new Vector2[param.UV.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Vector2.Lerp(param.UV[i], param3.UV[i], rate);
		}
		mesh_nipL.uv = array;
		Vector2[] array2 = new Vector2[param2.UV.Count];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = Vector2.Lerp(param2.UV[j], param4.UV[j], rate);
		}
		mesh_nipR.uv = array2;
	}
}
