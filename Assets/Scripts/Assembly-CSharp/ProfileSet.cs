using System;
using UnityEngine.PostProcessing;

[Serializable]
public class ProfileSet
{
	public PostProcessingProfile profile;

	public float vignetteIntensity;

	public float chromaticAberrationIntensity;

	public float grainIntensity;

	public float grainSize;

	public float bloomIntensity;

	public void Setup()
	{
		bloomIntensity = profile.bloom.settings.bloom.intensity;
		vignetteIntensity = profile.vignette.settings.intensity;
		chromaticAberrationIntensity = profile.chromaticAberration.settings.intensity;
		grainIntensity = profile.grain.settings.intensity;
		grainSize = profile.grain.settings.size;
	}
}
