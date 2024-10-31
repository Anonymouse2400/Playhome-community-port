using System;
using UnityEngine;

public class AnimeParamBlink : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private string paramName;

	[SerializeField]
	private AnimationCurve normalCurves;

	[SerializeField]
	private AnimationCurve rareCurves;

	[SerializeField]
	private float rareProbability = 10f;

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private float waitMin = 2f;

	[SerializeField]
	private float waitMax = 5f;

	private float timer;

	private float blinkEvaluate;

	private AnimationCurve useCurve;

	private int paramID;

	private float limitMin;

	private float limitMax = 1f;

	public float LimitMin
	{
		get
		{
			return limitMin;
		}
		set
		{
			limitMin = value;
			UpdateParam();
		}
	}

	public float LimitMax
	{
		get
		{
			return limitMax;
		}
		set
		{
			limitMax = value;
			UpdateParam();
		}
	}

	public bool Hold { get; set; }

	private void Awake()
	{
		paramID = Animator.StringToHash(paramName);
		timer = UnityEngine.Random.Range(waitMin, waitMax);
		blinkEvaluate = 0f;
		useCurve = normalCurves;
	}

	private void Start()
	{
		animator.SetFloat(paramID, limitMax);
	}

	private void Update()
	{
		if (timer > 0f)
		{
			if (!Hold)
			{
				timer -= Time.deltaTime;
			}
			if (timer <= 0f)
			{
				useCurve = ((!(UnityEngine.Random.Range(0f, 100f) < rareProbability)) ? normalCurves : rareCurves);
				blinkEvaluate = 0f;
			}
		}
		else
		{
			float t = 1f;
			if (blinkEvaluate < 1f)
			{
				blinkEvaluate += Time.deltaTime * speed;
				t = useCurve.Evaluate(blinkEvaluate);
			}
			else
			{
				timer = UnityEngine.Random.Range(waitMin, waitMax);
			}
			t = Mathf.Lerp(limitMin, limitMax, t);
			animator.SetFloat(paramID, t);
		}
	}

	private void UpdateParam()
	{
		float t = 1f;
		if (blinkEvaluate < 1f)
		{
			t = useCurve.Evaluate(blinkEvaluate);
		}
		t = Mathf.Lerp(limitMin, limitMax, t);
		animator.SetFloat(paramID, t);
	}
}
