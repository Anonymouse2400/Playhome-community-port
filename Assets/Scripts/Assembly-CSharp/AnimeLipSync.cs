using System;
using UnityEngine;

public class AnimeLipSync : MonoBehaviour
{
	public AudioSource audio;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private int layerNo;

	[SerializeField]
	private string blendName = "Blend";

	[SerializeField]
	private string speedName = "Speed";

	[SerializeField]
	private float lipSpeed = 1f;

	private AudioVolumeAnalyzer analyser;

	private float prevVol;

	[SerializeField]
	private float volMul = 1f;

	[SerializeField]
	private AnimationCurve volToMouth;

	[SerializeField]
	private LogLine volumeLine;

	[SerializeField]
	private LogLine blendLine;

	[SerializeField]
	private LogLine rawLine;

	public float RelaxOpen { get; set; }

	public bool Hold { get; set; }

	private void Awake()
	{
		analyser = new AudioVolumeAnalyzer();
		analyser.SetAudio(audio);
	}

	private void Update()
	{
		float num = 0f;
		float num2 = 0f;
		if (!Hold)
		{
			num = analyser.AnalyzeSound();
			num2 = analyser.AnalyzeSound(0f);
		}
		float time = num * volMul;
		float num3 = volToMouth.Evaluate(time);
		float num4 = Mathf.Abs(prevVol - num2);
		float value = Mathf.Clamp01(num4 * lipSpeed);
		animator.SetLayerWeight(layerNo, num3);
		animator.SetFloat(speedName, value);
		float value2 = Mathf.Lerp(0f, RelaxOpen, 1f - num3);
		animator.SetFloat(blendName, value2);
		float value3 = analyser.AnalyzeSound(0f);
		if ((bool)volumeLine)
		{
			volumeLine.Add(num);
		}
		if ((bool)blendLine)
		{
			blendLine.Add(num3);
		}
		if ((bool)rawLine)
		{
			rawLine.Add(value3);
		}
		prevVol = num2;
	}
}
