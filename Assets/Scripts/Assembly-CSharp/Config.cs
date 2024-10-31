using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
	private enum TAB
	{
		CAMERA = 0,
		SHOW = 1,
		POSTEFFECT = 2,
		AUDIO = 3,
		NUM = 4
	}

	private enum SAMPLE
	{
		NONE = -1,
		HERO = 0,
		KOUICHI = 1,
		MOB = 2,
		RITSUKO = 3,
		AKIKO = 4,
		YUKIKO = 5
	}

	[SerializeField]
	private Toggle[] tabs = new Toggle[4];

	[SerializeField]
	private GameObject[] tabMains = new GameObject[4];

	[SerializeField]
	private Text detailText;

	[SerializeField]
	private InputSliderUI sliderOriginal;

	[SerializeField]
	private SwitchUI switchOriginal;

	[SerializeField]
	private DropDownUI dropdownOriginal;

	[SerializeField]
	private ColorChangeButton colorChangeOriginal;

	[SerializeField]
	private GameObject spaceOriginal;

	private SwitchUI showFocusUI;

	private DropDownUI centerDrgaAct;

	private InputSliderUI cameraTurnSpeed;

	private InputSliderUI cameraMoveSpeed;

	private InputSliderUI mouseSensitive;

	private SwitchUI mouseRevV;

	private SwitchUI mouseRevH;

	private SwitchUI dragLock;

	private InputSliderUI keySensitive;

	private SwitchUI keyRevV;

	private SwitchUI keyRevH;

	private InputSliderUI parse;

	private SwitchUI h_camReset_Position;

	private SwitchUI h_camReset_Style;

	private SwitchUI showFPS;

	private SwitchUI autoHideObstacle;

	private SwitchUI showMob;

	private SwitchUI showMirror;

	private InputSliderUI backLight;

	private ColorChangeButton maleColor;

	private SwitchUI customHighlight;

	private SwitchUI h_action_continue;

	private DropDownUI dropThumbsCacheSize;

	private DropDownUI dropFlavor;

	private SwitchUI switchEyeAdaptation;

	private InputSliderUI sliderExposureCompensation;

	private SwitchUI switchBloom;

	private InputSliderUI sliderBloom;

	private SwitchUI switchDirt;

	private SwitchUI switchVignette;

	private InputSliderUI sliderVignette;

	private SwitchUI switchNoise;

	private InputSliderUI sliderNoise;

	private SwitchUI switchSSAO;

	private InputSliderUI sliderSSAOIntensity;

	private InputSliderUI sliderSSAORadius;

	private SwitchUI switchDOF;

	private InputSliderUI sliderDOF;

	private InputSliderUI masterVol;

	private InputSliderUI bgmVol;

	private InputSliderUI seVol;

	private InputSliderUI hseVol;

	private InputSliderUI envVol;

	private InputSliderUI mobVol;

	private SwitchUI reverb;

	private InputSliderUI voiceAllVol;

	private InputSliderUI ritsukoVol;

	private InputSliderUI ritsukoPitch;

	private InputSliderUI akikoVol;

	private InputSliderUI akikoPitch;

	private InputSliderUI yukikoVol;

	private InputSliderUI yukikoPitch;

	private InputSliderUI heroVol;

	private InputSliderUI heroPitch;

	private InputSliderUI kouichiVol;

	private InputSliderUI kouichiPitch;

	private GameControl gameCtrl;

	[SerializeField]
	private AudioSource sampleVoice;

	private SAMPLE nowPlaySample;

	private ImageEffectConfigChanger imageEffectChanger;

	private IllusionCamera illusionCamera;

	private readonly int[] ThumbsCacheSizeList = new int[4] { 0, 100, 500, 1000 };

	private readonly string[] ThumbsCacheNameList = new string[4] { "なし", "100MB", "500MB", "1GB" };

	private void Start()
	{
		gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
		imageEffectChanger = UnityEngine.Object.FindObjectOfType<ImageEffectConfigChanger>();
		illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		if (tabMains.Length != tabs.Length)
		{
			Debug.LogError("タブの数が合わない");
			return;
		}
		Transform parent = tabMains[0].transform.FindChild("Column_L");
		Transform parent2 = tabMains[0].transform.FindChild("Column_R");
		showFocusUI = CreateSwitchUI(parent, "注視点表示", "カメラの注視点の表示を設定します", ConfigData.showFocusUI);
		centerDrgaAct = CreateDropDownUI(parent, "中ドラッグ動作", "中ドラッグ時の動作を設定します\u3000もう片方が左右ボタンドラッグの動作になります", (int)ConfigData.centerDragMove, new string[2] { "上下左右移動", "前後左右移動" });
		cameraTurnSpeed = CreateInputSliderUI(parent, "回転速度", "カメラの回転速度を設定します", ConfigData.cameraTurnSpeed, 0.2f, 4f, true, 1f);
		cameraMoveSpeed = CreateInputSliderUI(parent, "移動速度", "カメラの移動速度を設定します", ConfigData.cameraMoveSpeed, 0.2f, 4f, true, 1f);
		mouseSensitive = CreateInputSliderUI(parent, "マウス感度", "マウスによるカメラ操作感度を設定します", ConfigData.mouseSensitive, 0.2f, 4f, true, 1f);
		mouseRevV = CreateSwitchUI(parent, "マウス上下反転", "マウスによるカメラ操作の上下を反転します", ConfigData.mouseRevV);
		mouseRevH = CreateSwitchUI(parent, "マウス左右反転", "マウスによるカメラ操作の左右を反転します", ConfigData.mouseRevH);
		dragLock = CreateSwitchUI(parent, "ドラッグ中のカーソル固定", "ドラッグ中にカーソル位置が移動しないようにします", ConfigData.dragLock);
		keySensitive = CreateInputSliderUI(parent, "キー感度", "キー入力によるカメラ操作感度を設定します", ConfigData.keySensitive, 0.2f, 4f, true, 1f);
		keyRevV = CreateSwitchUI(parent, "キー上下反転", "キー入力によるカメラ操作の上下を反転します", ConfigData.keyRevV);
		keyRevH = CreateSwitchUI(parent, "キー左右反転", "キー入力によるカメラ操作の左右を反転します", ConfigData.keyRevH);
		parse = CreateInputSliderUI(parent, "デフォルトパース", "基本となる視野角の角度を設定します", ConfigData.defParse, 10f, 90f, true, 25f);
		h_camReset_Position = CreateSwitchUI(parent2, "位置変更時のリセット", "H中の位置変更時にカメラをリセットします", ConfigData.h_camReset_position);
		h_camReset_Style = CreateSwitchUI(parent2, "体位変更時のリセット", "H中の体位変更時にカメラをリセットします", ConfigData.h_camReset_style);
		Transform parent3 = tabMains[1].transform.FindChild("Column_L");
		Transform parent4 = tabMains[1].transform.FindChild("Column_R");
		showFPS = CreateSwitchUI(parent3, "FPS表示", "1秒間のゲームの描画数を画面に表示します", ConfigData.showFPS);
		autoHideObstacle = CreateSwitchUI(parent3, "遮蔽物を自動非表示", "マップ上のオブジェクトが邪魔になる場合に自動的に非表示にします", ConfigData.autoHideObstacle);
		showMob = CreateSwitchUI(parent3, "観衆の表示", "観衆がいる場合の表示を切り替えます", ConfigData.showMob);
		showMirror = CreateSwitchUI(parent3, "鏡の映り込み", "鏡に実際の風景が映り込みます\u3000無効にすると処理負荷が軽減します", ConfigData.showMirror);
		backLight = CreateInputSliderUI(parent3, "バックライトの強さ", "常にカメラから見てキャラクターの後ろから当てるライトの強さを設定します", ConfigData.backLightIntensity, 0f, 1f, true, 0.8f);
		maleColor = CreateColorChangeButton(parent4, "男のシルエット表示時の色", "男の表示をシルエットのみにした場合の色を設定します", ConfigData.maleColor, true, delegate(Color color)
		{
			ConfigData.maleColor = color;
		});
		customHighlight = CreateSwitchUI(parent4, "キャラカスタム部位の表示", "カスタム時にスライダーが影響を与える部位をキャラに表示します", ConfigData.showCustomHighlight);
		h_action_continue = CreateSwitchUI(parent4, "H行為の継続", "同じタイプのH行為の場合、体位を変更しても行為を継続します", ConfigData.h_action_continue);
		int val2 = 0;
		for (int i = 0; i < ThumbsCacheSizeList.Length; i++)
		{
			if (ConfigData.thumbsCacheSizeMB == ThumbsCacheSizeList[i])
			{
				val2 = i;
				break;
			}
		}
		dropThumbsCacheSize = CreateDropDownUI(parent4, "サムネイルキャッシュサイズ", "アップローダーのサムネイル読み込みの高速化の為にPCに保存する最大容量を指定します", val2, ThumbsCacheNameList);
		Transform parent5 = tabMains[2].transform.FindChild("Column_L");
		Transform parent6 = tabMains[2].transform.FindChild("Column_R");
		string[] options = new string[3] { "オススメ", "薄め", "濃厚" };
		dropFlavor = CreateDropDownUI(parent5, "雰囲気", "ゲーム世界の色合いの雰囲気を設定します", ConfigData.postProcessFlavor - 1, options);
		CreateSpace(parent5);
		switchEyeAdaptation = CreateSwitchUI(parent5, "自動露光", "自動で暗いシーンを明るく、明るいシーンを暗く調整します", ConfigData.eyeAdaptationEnable);
		sliderExposureCompensation = CreateInputSliderUI(parent5, "明るさ", "自動露光の基準となる明るさを設定します", ConfigData.exposureCompensation, 0f, 1f, true, 0.5f);
		CreateSpace(parent5);
		switchBloom = CreateSwitchUI(parent5, "ブルーム", "明るい光の拡散を表現します", ConfigData.bloomEnable);
		sliderBloom = CreateInputSliderUI(parent5, "強さ", "ブルームの強さを調整します", ConfigData.bloomRate, 0f, 2f, true, 1f);
		switchDirt = CreateSwitchUI(parent5, "レンズの汚れ", "ブルーム効果にレンズの汚れを加味します", ConfigData.lensDirtEnable);
		CreateSpace(parent5);
		switchVignette = CreateSwitchUI(parent5, "ビネット", "画面端を暗くします", ConfigData.vignetteEnable);
		sliderVignette = CreateInputSliderUI(parent5, "強さ", "ビネットの強さを調整します", ConfigData.vignetteRate, 0f, 2f, true, 1f);
		switchNoise = CreateSwitchUI(parent6, "ノイズ", "画面にノイズエフェクトを施します", ConfigData.noiseEnable);
		sliderNoise = CreateInputSliderUI(parent6, "強さ", "ノイズの強さを調整します", ConfigData.noiseRate, 0f, 2f, true, 1f);
		CreateSpace(parent6);
		switchSSAO = CreateSwitchUI(parent6, "SSAO", "物体の陰影を強調します", ConfigData.ssaoEnable);
		sliderSSAOIntensity = CreateInputSliderUI(parent6, "強さ", "SSAOの強さを調節します", ConfigData.ssaoIntensity, 0f, 2f, true, 1f);
		sliderSSAORadius = CreateInputSliderUI(parent6, "範囲", "SSAOの範囲を調節します", ConfigData.ssaoRadius, 0f, 2f, true, 1f);
		CreateSpace(parent6);
		switchDOF = CreateSwitchUI(parent6, "被写界深度", "カメラの注視点から離れた物体はぼけるようになります", ConfigData.dofEnable);
		sliderDOF = CreateInputSliderUI(parent6, "強さ", "被写界深度の強さを調整します", ConfigData.dofRate, 0f, 1f, true, 0.7f);
		Transform parent7 = tabMains[3].transform.FindChild("Column_L");
		masterVol = CreateInputSliderUI(parent7, "音量:マスター", "すべての音の音量を調整します", ConfigData.volume_master, 0f, 1f, false, 0.75f);
		CreateSpace(parent7);
		bgmVol = CreateInputSliderUI(parent7, "音量:BGM", "BGMの音量を設定します", ConfigData.volume_bgm, 0f, 1f, false, 0.3f);
		seVol = CreateInputSliderUI(parent7, "音量:システム音", "システム音の音量を設定します", ConfigData.volume_system, 0f, 1f, false, 1f);
		hseVol = CreateInputSliderUI(parent7, "音量:効果音", "効果音の音量を設定します", ConfigData.volume_se, 0f, 1f, false, 1f);
		envVol = CreateInputSliderUI(parent7, "音量:環境音", "環境音の音量を設定します", ConfigData.volume_env, 0f, 1f, false, 1f);
		CreateSpace(parent7);
		reverb = CreateSwitchUI(parent7, "音響効果", "反響などの音響効果の有無を設定します", ConfigData.reverb_flag);
		Transform parent8 = tabMains[3].transform.FindChild("Column_R");
		voiceAllVol = CreateInputSliderUI(parent8, "音量:音声", "全員の音声の音量を調整します", ConfigData.volume_voiceAll, 0f, 1f, false, 1f);
		CreateSpace(parent8);
		ritsukoVol = CreateInputSliderUI(parent8, "音量:律子", "律子の音量を設定します", ConfigData.volume_voiceRitsuko, 0f, 1f, false, 1f);
		ritsukoPitch = CreateInputSliderUI(parent8, "高低:律子", "律子の声の高低を設定します", ConfigData.pitch_voiceRitsuko, -1f, 1f, true);
		akikoVol = CreateInputSliderUI(parent8, "音量:明子", "明子の音量を設定します", ConfigData.volume_voiceAkiko, 0f, 1f, false, 1f);
		akikoPitch = CreateInputSliderUI(parent8, "高低:明子", "明子の声の高低を設定します", ConfigData.pitch_voiceAkiko, -1f, 1f, true);
		yukikoVol = CreateInputSliderUI(parent8, "音量:雪子", "雪子の音量を設定します", ConfigData.volume_voiceYukiko, 0f, 1f, false, 1f);
		yukikoPitch = CreateInputSliderUI(parent8, "高低:雪子", "雪子の声の高低を設定します", ConfigData.pitch_voiceYukiko, -1f, 1f, true);
		heroVol = CreateInputSliderUI(parent8, "音量:主人公", "主人公の音量を設定します", ConfigData.volume_voiceHero, 0f, 1f, false, 1f);
		heroPitch = CreateInputSliderUI(parent8, "高低:主人公", "主人公の声の高低を設定します", ConfigData.pitch_voiceHero, -1f, 1f, true);
		kouichiVol = CreateInputSliderUI(parent8, "音量:広一", "広一の音量を設定します", ConfigData.volume_voiceKouichi, 0f, 1f, false, 1f);
		kouichiPitch = CreateInputSliderUI(parent8, "高低:広一", "広一の声の高低を設定します", ConfigData.pitch_voiceKouichi, -1f, 1f, true);
		mobVol = CreateInputSliderUI(parent8, "音量:観衆", "観衆の音量を設定します", ConfigData.volume_voiceMob, 0f, 1f, false, 1f);
		ritsukoVol.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.RITSUKO);
		});
		ritsukoPitch.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.RITSUKO);
		});
		akikoVol.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.AKIKO);
		});
		akikoPitch.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.AKIKO);
		});
		yukikoVol.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.YUKIKO);
		});
		yukikoPitch.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.YUKIKO);
		});
		heroVol.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.HERO);
		});
		heroPitch.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.HERO);
		});
		kouichiVol.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.KOUICHI);
		});
		kouichiPitch.AddOnChangeAction(delegate
		{
			VoiceOnChange(SAMPLE.KOUICHI);
		});
		for (int j = 0; j < tabs.Length; j++)
		{
			tabMains[j].SetActive(false);
		}
		nowPlaySample = SAMPLE.NONE;
	}

	private void OnEnable()
	{
		UpdateFromShortCut();
	}

	private void Update()
	{
		for (int i = 0; i < tabs.Length; i++)
		{
			tabMains[i].SetActive(tabs[i].isOn);
		}
		ConfigData.showFocusUI = showFocusUI.Value;
		ConfigData.centerDragMove = (IllusionCamera.CenterDragMove)centerDrgaAct.Value;
		ConfigData.cameraTurnSpeed = cameraTurnSpeed.Value;
		ConfigData.cameraMoveSpeed = cameraMoveSpeed.Value;
		ConfigData.mouseSensitive = mouseSensitive.Value;
		ConfigData.mouseRevV = mouseRevV.Value;
		ConfigData.mouseRevH = mouseRevH.Value;
		ConfigData.dragLock = dragLock.Value;
		ConfigData.keySensitive = keySensitive.Value;
		ConfigData.keyRevV = keyRevV.Value;
		ConfigData.keyRevH = keyRevH.Value;
		ConfigData.showFPS = showFPS.Value;
		ConfigData.autoHideObstacle = autoHideObstacle.Value;
		ConfigData.showMob = showMob.Value;
		ConfigData.showMirror = showMirror.Value;
		ConfigData.backLightIntensity = backLight.Value;
		ConfigData.showCustomHighlight = customHighlight.Value;
		ConfigData.h_camReset_position = h_camReset_Position.Value;
		ConfigData.h_camReset_style = h_camReset_Style.Value;
		ConfigData.h_action_continue = h_action_continue.Value;
		ConfigData.thumbsCacheSizeMB = ThumbsCacheSizeList[dropThumbsCacheSize.Value];
		Update_PostEffects();
		ConfigData.volume_master = masterVol.Value;
		ConfigData.volume_bgm = bgmVol.Value;
		ConfigData.volume_system = seVol.Value;
		ConfigData.volume_se = hseVol.Value;
		ConfigData.volume_env = envVol.Value;
		ConfigData.reverb_flag = reverb.Value;
		ConfigData.volume_voiceAll = voiceAllVol.Value;
		ConfigData.volume_voiceRitsuko = ritsukoVol.Value;
		ConfigData.volume_voiceAkiko = akikoVol.Value;
		ConfigData.volume_voiceYukiko = yukikoVol.Value;
		ConfigData.volume_voiceHero = heroVol.Value;
		ConfigData.volume_voiceKouichi = kouichiVol.Value;
		ConfigData.volume_voiceMob = mobVol.Value;
		ConfigData.pitch_voiceRitsuko = ritsukoPitch.Value;
		ConfigData.pitch_voiceAkiko = akikoPitch.Value;
		ConfigData.pitch_voiceYukiko = yukikoPitch.Value;
		ConfigData.pitch_voiceHero = heroPitch.Value;
		ConfigData.pitch_voiceKouichi = kouichiPitch.Value;
		UpdateVoice();
		if (ConfigData.defParse != parse.Value)
		{
			ConfigData.defParse = parse.Value;
			if (illusionCamera != null)
			{
				illusionCamera.SetParse(ConfigData.defParse, true);
			}
		}
		Update_Input();
	}

	private void Update_PostEffects()
	{
		bool flag = false;
		if (ConfigData.postProcessFlavor != dropFlavor.Value + 1)
		{
			flag = flag || true;
		}
		if (ConfigData.eyeAdaptationEnable != switchEyeAdaptation.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.exposureCompensation != sliderExposureCompensation.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.bloomEnable != switchBloom.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.bloomRate != sliderBloom.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.lensDirtEnable != switchDirt.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.vignetteEnable != switchVignette.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.vignetteRate != sliderVignette.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.noiseEnable != switchNoise.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.noiseRate != sliderNoise.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.ssaoEnable != switchSSAO.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.ssaoIntensity != sliderSSAOIntensity.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.ssaoRadius != sliderSSAORadius.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.dofEnable != switchDOF.Value)
		{
			flag = flag || true;
		}
		if (ConfigData.dofRate != sliderDOF.Value)
		{
			flag = flag || true;
		}
		if (flag)
		{
			ConfigData.postProcessFlavor = dropFlavor.Value + 1;
			ConfigData.eyeAdaptationEnable = switchEyeAdaptation.Value;
			ConfigData.exposureCompensation = sliderExposureCompensation.Value;
			ConfigData.bloomEnable = switchBloom.Value;
			ConfigData.bloomRate = sliderBloom.Value;
			ConfigData.lensDirtEnable = switchDirt.Value;
			ConfigData.vignetteEnable = switchVignette.Value;
			ConfigData.vignetteRate = sliderVignette.Value;
			ConfigData.noiseEnable = switchNoise.Value;
			ConfigData.noiseRate = sliderNoise.Value;
			ConfigData.ssaoEnable = switchSSAO.Value;
			ConfigData.ssaoIntensity = sliderSSAOIntensity.Value;
			ConfigData.ssaoRadius = sliderSSAORadius.Value;
			ConfigData.dofEnable = switchDOF.Value;
			ConfigData.dofRate = sliderDOF.Value;
			if (imageEffectChanger != null)
			{
				imageEffectChanger.ChangeConfig();
			}
		}
	}

	private void UpdateVoice()
	{
		if (nowPlaySample == SAMPLE.RITSUKO)
		{
			sampleVoice.volume = ConfigData.VolumeVoice_Ritsuko();
			sampleVoice.pitch = ConfigData.PitchVoice_Ritsuko();
		}
		else if (nowPlaySample == SAMPLE.AKIKO)
		{
			sampleVoice.volume = ConfigData.VolumeVoice_Akiko();
			sampleVoice.pitch = ConfigData.PitchVoice_Akiko();
		}
		else if (nowPlaySample == SAMPLE.YUKIKO)
		{
			sampleVoice.volume = ConfigData.VolumeVoice_Yukiko();
			sampleVoice.pitch = ConfigData.PitchVoice_Yukiko();
		}
		else if (nowPlaySample == SAMPLE.HERO)
		{
			sampleVoice.volume = ConfigData.VolumeVoice_Hero();
			sampleVoice.pitch = ConfigData.PitchVoice_Hero();
		}
		else if (nowPlaySample == SAMPLE.KOUICHI)
		{
			sampleVoice.volume = ConfigData.VolumeVoice_Kouichi();
			sampleVoice.pitch = ConfigData.PitchVoice_Kouichi();
		}
		else if (nowPlaySample == SAMPLE.MOB)
		{
			sampleVoice.volume = ConfigData.VolumeVoice_Mob();
			sampleVoice.pitch = 1f;
		}
	}

	private void Update_Input()
	{
		if ((!(EventSystem.current != null) || !(EventSystem.current.currentSelectedGameObject != null)) && Input.GetKeyDown(KeyCode.F1))
		{
			Close();
		}
	}

	private SwitchUI CreateSwitchUI(Transform parent, string title, string detail, bool val)
	{
		SwitchUI switchUI = UnityEngine.Object.Instantiate(switchOriginal);
		switchUI.SetTitle(title);
		switchUI.SetValue(val);
		EventTrigger eventTrigger = switchUI.GetEventTrigger();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(delegate
		{
			SetDetailText(detail);
		});
		eventTrigger.triggers.Add(entry);
		switchUI.transform.SetParent(parent, false);
		return switchUI;
	}

	private InputSliderUI CreateInputSliderUI(Transform parent, string title, string detail, float val, float min, float max, bool hasDef = false, float defVal = 0f)
	{
		InputSliderUI inputSliderUI = UnityEngine.Object.Instantiate(sliderOriginal);
		inputSliderUI.SetTitle(title);
		inputSliderUI.Setup(min, max, hasDef, defVal);
		inputSliderUI.SetValue(val);
		EventTrigger eventTrigger = inputSliderUI.GetEventTrigger();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(delegate
		{
			SetDetailText(detail);
		});
		eventTrigger.triggers.Add(entry);
		inputSliderUI.transform.SetParent(parent, false);
		return inputSliderUI;
	}

	private DropDownUI CreateDropDownUI(Transform parent, string title, string detail, int val, string[] options)
	{
		DropDownUI dropDownUI = UnityEngine.Object.Instantiate(dropdownOriginal);
		dropDownUI.SetTitle(title);
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		foreach (string text in options)
		{
			list.Add(new Dropdown.OptionData(text));
		}
		dropDownUI.SetList(list);
		dropDownUI.SetValue(val);
		EventTrigger eventTrigger = dropDownUI.GetEventTrigger();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(delegate
		{
			SetDetailText(detail);
		});
		eventTrigger.triggers.Add(entry);
		dropDownUI.transform.SetParent(parent, false);
		return dropDownUI;
	}

	private ColorChangeButton CreateColorChangeButton(Transform parent, string title, string detail, Color color, bool hasAlpha, Action<Color> act)
	{
		ColorChangeButton colorChangeButton = UnityEngine.Object.Instantiate(colorChangeOriginal);
		colorChangeButton.Setup(title, color, hasAlpha, act);
		EventTrigger eventTrigger = colorChangeButton.GetEventTrigger();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(delegate
		{
			SetDetailText(detail);
		});
		eventTrigger.triggers.Add(entry);
		colorChangeButton.transform.SetParent(parent, false);
		return colorChangeButton;
	}

	private GameObject CreateSpace(Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(spaceOriginal);
		gameObject.transform.SetParent(parent, false);
		return gameObject;
	}

	public void SetDetailText(string str)
	{
		detailText.text = str;
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
		gameCtrl.audioCtrl.Play2DSE(gameCtrl.audioCtrl.systemSE_close);
	}

	private void VoiceOnChange(SAMPLE chara)
	{
		bool flag = false;
		if (!sampleVoice.isPlaying)
		{
			flag = true;
		}
		else if (nowPlaySample != chara)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		sampleVoice.Stop();
		AudioClip audioClip = null;
		string text = "SystemVoice/VolCheck/VolCheck_";
		if (chara == SAMPLE.HERO)
		{
			text += "M00";
		}
		else if (chara == SAMPLE.KOUICHI)
		{
			text += "M01";
		}
		else if (chara >= SAMPLE.RITSUKO)
		{
			string[] array = new string[2] { "A", "B" };
			int num = (GlobalData.flipflop ? UnityEngine.Random.Range(0, 2) : 0);
			int num2 = (int)(chara - 3);
			HEROINE heroineID = (HEROINE)num2;
			if (GlobalData.PlayData != null)
			{
				num = (GlobalData.PlayData.IsHeroineFloped(heroineID) ? 1 : 0);
			}
			string text2 = text;
			text = text2 + "F" + num2.ToString("00") + "_" + array[num];
		}
		audioClip = Resources.Load<AudioClip>(text);
		if (audioClip == null)
		{
			Debug.LogError("オーディオクリップがない");
			return;
		}
		nowPlaySample = chara;
		UpdateVoice();
		sampleVoice.PlayOneShot(audioClip);
	}

	public void UpdateFromShortCut()
	{
		if (showFocusUI != null && autoHideObstacle != null)
		{
			showFocusUI.SetValue(ConfigData.showFocusUI);
			autoHideObstacle.SetValue(ConfigData.autoHideObstacle);
		}
	}
}
