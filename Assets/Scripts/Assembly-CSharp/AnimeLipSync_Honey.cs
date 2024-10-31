using System;
using UnityEngine;

public class AnimeLipSync_Honey : MonoBehaviour
{
	public AudioSource audio;

	public Animator animator;

	public string blendName = "Blend";

	public int layerNo;

	public Transform mouthH;

	public float mouthHMax = 0.5f;

	public float mouthHSpeed = 2f;

	private float mouthHCycle;

	private AudioVolumeAnalyzer analyser;

	private float prevVol;

	public float volMul = 1f;

	public AnimationCurve volToMouth;

	public LogLine volumeLine;

	public LogLine blendLine;

	public LogLine rawLine;

	private void Start()
	{
		analyser = new AudioVolumeAnalyzer();
		analyser.SetAudio(audio);
	}

	private void Update()
	{
		float num = analyser.AnalyzeSound();
		float time = num * volMul;
		float num2 = volToMouth.Evaluate(time);
		float value = analyser.AnalyzeSound(0f);
		animator.SetFloat(blendName, num2);
		animator.SetLayerWeight(layerNo, 0f);
		float num3 = Mathf.Abs(num2 - prevVol);
		mouthHCycle += num3 * (float)Math.PI * mouthHSpeed;
		float t = Mathf.Abs(Mathf.Sin(mouthHCycle)) * num2;
		t = Mathf.Lerp(1f, mouthHMax, t);
		mouthH.localScale = new Vector3(t, 1f, 1f);
		if ((bool)volumeLine)
		{
			volumeLine.Add(num);
		}
		if ((bool)blendLine)
		{
			blendLine.Add(num2);
		}
		if ((bool)rawLine)
		{
			rawLine.Add(value);
		}
		prevVol = num2;
	}
}
