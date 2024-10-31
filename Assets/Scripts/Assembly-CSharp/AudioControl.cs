using System;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
	[SerializeField]
	private AudioSource bgmSource;

	[SerializeField]
	private string bgmAssetBundle = "bgm";

	[SerializeField]
	private float bgmFadeSpeed = 2f;

	private BGM_Control bgmCtrl;

	private AssetBundleController bgmABC;

	[SerializeField]
	private SoundEffect se2D;

	[SerializeField]
	private SoundEffect se3D;

	public AudioClip systemSE_yes;

	public AudioClip systemSE_no;

	public AudioClip systemSE_choice;

	public AudioClip systemSE_open;

	public AudioClip systemSE_close;

	private AudioClip reserve2DClip;

	public string BGM_Name { get; private set; }

	public bool PlayingBGM
	{
		get
		{
			return bgmCtrl.IsPlaying;
		}
	}

	private void Awake()
	{
		bgmSource.loop = true;
		bgmCtrl = new BGM_Control(bgmSource);
		bgmABC = new AssetBundleController();
		bgmABC.OpenFromFile(GlobalData.assetBundlePath, "bgm");
	}

	private void OnDestroy()
	{
		bgmABC.Close();
	}

	private void Update()
	{
		UpdateBGM(Time.deltaTime);
		Update2DSE();
	}

	private void UpdateBGM(float updateTime = 0f)
	{
		bgmCtrl.Update();
	}

	private void Update2DSE()
	{
		if (reserve2DClip != null)
		{
			SoundEffect soundEffect = UnityEngine.Object.Instantiate(se2D);
			soundEffect.type = SoundEffect.TYPE.TYPE_SYSTEM;
			soundEffect.Play(reserve2DClip);
			soundEffect.transform.SetParent(base.transform);
			reserve2DClip = null;
		}
	}

	public void BGM_Load(string file)
	{
		if (!(BGM_Name == file))
		{
			AudioClip audioClip = bgmABC.LoadAsset<AudioClip>(file);
			if (audioClip == null)
			{
				Debug.LogError("BGMが読めなかった:" + file);
				return;
			}
			BGM_Name = file;
			BGM_Load(audioClip);
		}
	}

	private void BGM_Load(AudioClip clip)
	{
		bgmCtrl.ChangeClip(clip, bgmFadeSpeed);
	}

	private void BGM_LoadAndPlay(string file)
	{
		BGM_Name = file;
		AudioClip clip = bgmABC.LoadAsset<AudioClip>(file);
		if (bgmCtrl.IsPlaying)
		{
			bgmCtrl.ChangeClip(clip, bgmFadeSpeed);
			return;
		}
		bgmCtrl.ChangeClip(clip, 0f);
		bgmCtrl.Play(0f);
	}

	public void BGM_LoadAndPlay(string file, bool sameRestart = false, bool forcePlay = false)
	{
		if (!bgmSource.isPlaying || !(BGM_Name == file) || sameRestart)
		{
			BGM_Name = file;
			BGM_LoadAndPlay(file);
		}
	}

	public void BGM_Play()
	{
		bgmCtrl.Play(0f);
	}

	public void BGM_Stop()
	{
		bgmCtrl.Stop(bgmFadeSpeed);
	}

	public void Play2DSE(AudioClip clip)
	{
		reserve2DClip = clip;
	}

	public void Play3DSE(AudioClip clip, Vector3 pos)
	{
		SoundEffect soundEffect = UnityEngine.Object.Instantiate(se3D);
		soundEffect.Play(clip);
		soundEffect.transform.SetParent(base.transform);
		soundEffect.transform.position = pos;
	}
}
