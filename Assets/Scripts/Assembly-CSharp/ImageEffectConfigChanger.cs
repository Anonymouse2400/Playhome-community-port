using System;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class ImageEffectConfigChanger : MonoBehaviour
{
	private PostProcessingBehaviour postProcessingBehaviour;

	private SSAOPro ssao;

	private DepthOfField dof;

	public ProfileSet[] profileSets;

	public Texture dirtTex;

	private int profileNo = -1;

	public bool dofChange = true;

	private PostProcessingProfile instantiateProfile;

	private void Start()
	{
		postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
		ssao = GetComponent<SSAOPro>();
		dof = GetComponent<DepthOfField>();
		for (int i = 0; i < profileSets.Length; i++)
		{
			profileSets[i].Setup();
		}
		ChangeConfig();
	}

	public void ChangeConfig()
	{
		if (profileNo != ConfigData.postProcessFlavor)
		{
			profileNo = ConfigData.postProcessFlavor;
            UnityEngine.Object.Destroy(instantiateProfile);
			instantiateProfile = UnityEngine.Object.Instantiate(profileSets[profileNo].profile);
			instantiateProfile.name = "InstantiateProfile";
			postProcessingBehaviour.profile = instantiateProfile;
		}
		instantiateProfile.bloom.enabled = ConfigData.bloomEnable;
		BloomModel.Settings settings = instantiateProfile.bloom.settings;
		settings.bloom.intensity = profileSets[profileNo].bloomIntensity * ConfigData.bloomRate;
		settings.lensDirt.texture = ((!ConfigData.lensDirtEnable) ? null : dirtTex);
		instantiateProfile.bloom.settings = settings;
		if (instantiateProfile.eyeAdaptation.enabled != ConfigData.eyeAdaptationEnable)
		{
			instantiateProfile.eyeAdaptation.enabled = ConfigData.eyeAdaptationEnable;
		}
		EyeAdaptationModel.Settings settings2 = instantiateProfile.eyeAdaptation.settings;
		settings2.exposureCompensation = 0.6f + Mathf.Lerp(-0.2f, 0.2f, ConfigData.exposureCompensation);
		instantiateProfile.eyeAdaptation.settings = settings2;
		postProcessingBehaviour.EyeAdaptation.ResetHistory();
		instantiateProfile.vignette.enabled = ConfigData.vignetteEnable;
		VignetteModel.Settings settings3 = instantiateProfile.vignette.settings;
		settings3.intensity = profileSets[profileNo].vignetteIntensity * ConfigData.vignetteRate;
		instantiateProfile.vignette.settings = settings3;
		instantiateProfile.chromaticAberration.enabled = ConfigData.noiseEnable;
		ChromaticAberrationModel.Settings settings4 = instantiateProfile.chromaticAberration.settings;
		settings4.intensity = profileSets[profileNo].chromaticAberrationIntensity * ConfigData.noiseRate;
		instantiateProfile.chromaticAberration.settings = settings4;
		instantiateProfile.grain.enabled = ConfigData.noiseEnable;
		GrainModel.Settings settings5 = instantiateProfile.grain.settings;
		settings5.intensity = profileSets[profileNo].grainIntensity * ConfigData.noiseRate;
		instantiateProfile.grain.settings = settings5;
		if (ssao != null)
		{
			ssao.enabled = ConfigData.ssaoEnable;
			ssao.Intensity = 1f * ConfigData.ssaoIntensity;
			ssao.Radius = 0.12f * ConfigData.ssaoRadius;
		}
		if (dof != null && dofChange)
		{
			dof.enabled = ConfigData.dofEnable;
			dof.aperture = Mathf.Lerp(0.6f, 0.9f, ConfigData.dofRate);
		}
	}
}
