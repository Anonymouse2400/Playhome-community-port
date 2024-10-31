using System;
using UnityEngine;

public class GameFirstSetup : MonoBehaviour
{
	[SerializeField]
	private Shader multiplayBlendShader_2;

	[SerializeField]
	private Shader multiplayBlendShader_3;

	[SerializeField]
	private Shader multiplayBlendShader_6;

	[SerializeField]
	private Shader normalBlendShader_2;

	[SerializeField]
	private Shader normalAddShader_2;

	[SerializeField]
	private Shader hsvOffset;

	[SerializeField]
	private Shader skinBlendShader_Body;

	[SerializeField]
	private Shader skinBlendShader_Face;

	[SerializeField]
	private Shader skinBlendShader_Male;

	private void Awake()
	{
		CustomDataManager.multiplayBlendShader_2 = multiplayBlendShader_2;
		CustomDataManager.multiplayBlendShader_3 = multiplayBlendShader_3;
		CustomDataManager.multiplayBlendShader_6 = multiplayBlendShader_6;
		CustomDataManager.normalBlendShader_2 = normalBlendShader_2;
		CustomDataManager.normalAddShader_2 = normalAddShader_2;
		CustomDataManager.hsvOffset = hsvOffset;
		CustomDataManager.skinBlendShader_Body = skinBlendShader_Body;
		CustomDataManager.skinBlendShader_Face = skinBlendShader_Face;
		CustomDataManager.skinBlendShader_Male = skinBlendShader_Male;
		CustomDataManager.Setup();
		GlobalData.PlayData = new GamePlayData();
		GlobalData.PlayData.Start();
		ColorPaletteData.Load();
		GlobalData.Load();
		ConfigData.Load();
		MaterialCustomData.Load();
	}

	private void OnDestroy()
	{
		CustomDataManager.SaveIsNewData();
		GlobalData.Save();
		ConfigData.Save();
		MaterialCustomData.Save();
		ColorPaletteData.Save();
	}
}
