using System;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingConfig : MonoBehaviour
{
	private PostProcessingBehaviour postProcessingBehaviour;

	public bool bloom;

	public float bloomIntensity;

	private PostProcessingProfile profile;

	private void Start()
	{
		postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
		profile = UnityEngine.Object.Instantiate(postProcessingBehaviour.profile);
		profile.name = "InstantiateProfile";
		postProcessingBehaviour.profile = profile;
	}

	private void Update()
	{
	}
}
