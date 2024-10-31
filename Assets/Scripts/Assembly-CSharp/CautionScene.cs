using System;
using UnityEngine;

public class CautionScene : Scene
{
	public string nextScene = "LogoScene";

	public string nextMessage = string.Empty;

	public float fadeInTime = 1f;

	public float fadeOutTime = 1f;

	public string bgm = string.Empty;

	private bool isFirstFrame = true;

	private Rigidbody[] rigids;

	[SerializeField]
	private Transform explosionTrans;

	[SerializeField]
	private float explosionForce = 1f;

	[SerializeField]
	private float explosionRadius = 1f;

	private bool broken;

	private void Start()
	{
		InScene(false);
		ScreenFade.StartFade(ScreenFade.TYPE.IN, base.GC.FadeColor, fadeInTime);
		isFirstFrame = true;
		if (bgm.Length > 0)
		{
			base.GC.audioCtrl.BGM_LoadAndPlay(bgm);
		}
		rigids = explosionTrans.GetComponentsInChildren<Rigidbody>();
	}

	private void Update()
	{
		if (isFirstFrame)
		{
			isFirstFrame = false;
		}
		else if (GetInput() && !broken)
		{
			Break();
			base.GC.ChangeScene(nextScene, nextMessage, fadeOutTime);
		}
	}

	private void Break()
	{
		if (explosionTrans == null)
		{
			explosionTrans = base.transform;
		}
		for (int i = 0; i < rigids.Length; i++)
		{
			rigids[i].isKinematic = false;
			rigids[i].AddExplosionForce(explosionForce, explosionTrans.position, explosionRadius);
		}
		broken = true;
	}

	private bool GetInput()
	{
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
			return true;
		}
		return false;
	}
}
