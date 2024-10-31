using System;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class PostProcessingChanger : MonoBehaviour
{
	private PostProcessingBehaviour postProcessingBehaviour;

	public ProfileSet[] profileSets;

	private PostProcessingProfile instantiateProfile;

	public int profileNo;

	public bool eyeAdaptationEnable;

	public float exposureCompensation = 0.55f;

	public bool bloomEnable;

	[Range(0f, 2f)]
	public float bloomRate = 1f;

	public bool lensDirtEnable;

	public Texture dirtTex;

	public bool vignetteEnable = true;

	[Range(0f, 2f)]
	public float vignetteRate = 1f;

	public bool noiseEnable;

	[Range(0f, 2f)]
	public float noiseRate = 1f;

	private bool isUpdate;

	private void Awake()
	{
		postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
		for (int i = 0; i < profileSets.Length; i++)
		{
			profileSets[i].Setup();
		}
		ChangeBaseProfile(profileNo);
	}

	public void ChangeBaseProfile(int no)
	{
        UnityEngine.Object.Destroy(instantiateProfile);
		instantiateProfile = UnityEngine.Object.Instantiate(profileSets[no].profile);
		instantiateProfile.name = "InstantiateProfile";
		postProcessingBehaviour.profile = instantiateProfile;
		SetChange(no);
	}

	private void SetChange(int no)
	{
		profileNo = no;
		instantiateProfile.bloom.enabled = bloomEnable;
		BloomModel.Settings settings = instantiateProfile.bloom.settings;
		settings.bloom.intensity = profileSets[no].bloomIntensity * bloomRate;
		settings.lensDirt.texture = ((!lensDirtEnable) ? null : dirtTex);
		instantiateProfile.bloom.settings = settings;
		instantiateProfile.eyeAdaptation.enabled = eyeAdaptationEnable;
		EyeAdaptationModel.Settings settings2 = instantiateProfile.eyeAdaptation.settings;
		settings2.exposureCompensation = exposureCompensation;
		instantiateProfile.eyeAdaptation.settings = settings2;
		instantiateProfile.vignette.enabled = vignetteEnable;
		VignetteModel.Settings settings3 = instantiateProfile.vignette.settings;
		settings3.intensity = profileSets[no].vignetteIntensity * vignetteRate;
		instantiateProfile.vignette.settings = settings3;
		instantiateProfile.chromaticAberration.enabled = noiseEnable;
		ChromaticAberrationModel.Settings settings4 = instantiateProfile.chromaticAberration.settings;
		settings4.intensity = profileSets[no].chromaticAberrationIntensity * noiseRate;
		instantiateProfile.chromaticAberration.settings = settings4;
		instantiateProfile.grain.enabled = noiseEnable;
		GrainModel.Settings settings5 = instantiateProfile.grain.settings;
		settings5.intensity = profileSets[no].grainIntensity * noiseRate;
		instantiateProfile.grain.settings = settings5;
	}

	private void OnValidate()
	{
		isUpdate = true;
	}

	private void Update()
	{
		if (isUpdate)
		{
			SetChange(profileNo);
		}
	}
}
