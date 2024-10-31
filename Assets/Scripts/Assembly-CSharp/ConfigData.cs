using System;
using System.IO;
using Character;
using UnityEngine;

public static class ConfigData
{
	//public static readonly string path = Application.dataPath + "/../UserData/Save";

    public static readonly string path = Application.persistentDataPath + "/UserData/Save";

    public static readonly int version = 0;

	public static readonly float pitchRate = 0.06f;

	public static bool showFocusUI = true;

	public static float cameraTurnSpeed = 1f;

	public static float cameraMoveSpeed = 1f;

	public static float mouseSensitive = 1f;

	public static bool mouseRevV = false;

	public static bool mouseRevH = false;

	public static bool dragLock = false;

	public static IllusionCamera.CenterDragMove centerDragMove = IllusionCamera.CenterDragMove.XY;

	public static float keySensitive = 1f;

	public static bool keyRevV = false;

	public static bool keyRevH = false;

	public static float defParse = 25f;

	public static bool dropShadow = true;

	public static bool postEffect = true;

	public static bool h_camReset_position = true;

	public static bool h_camReset_style = true;

	public static bool showFPS = true;

	public static bool autoHideObstacle = true;

	public static bool showMob = true;

	public static bool showMirror = true;

	public static float backLightIntensity = 0.8f;

	public static Color maleColor = new Color(0.5f, 0.75f, 1f, 0.8f);

	public static Color clearColor = new Color(0.25f, 0.25f, 0.25f, 1f);

	public static bool showCustomHighlight = true;

	public static bool h_action_continue = false;

	public static int thumbsCacheSizeMB = 100;

	public static int postProcessFlavor = 1;

	public static bool eyeAdaptationEnable = false;

	public static float exposureCompensation = 0.5f;

	public static bool bloomEnable = true;

	public static float bloomRate = 1f;

	public static bool lensDirtEnable = false;

	public static bool vignetteEnable = true;

	public static float vignetteRate = 1f;

	public static bool noiseEnable = false;

	public static float noiseRate = 1f;

	public static bool ssaoEnable = true;

	public static float ssaoIntensity = 1f;

	public static float ssaoRadius = 1f;

	public static bool dofEnable = true;

	public static float dofRate = 0.7f;

	public static float volume_master = 0.75f;

	public static float volume_bgm = 0.3f;

	public static float volume_system = 1f;

	public static float volume_se = 1f;

	public static float volume_env = 1f;

	public static float volume_voiceAll = 1f;

	public static float volume_voiceRitsuko = 1f;

	public static float volume_voiceAkiko = 1f;

	public static float volume_voiceYukiko = 1f;

	public static float volume_voiceHero = 1f;

	public static float volume_voiceKouichi = 1f;

	public static float volume_voiceMob = 1f;

	public static bool reverb_flag = true;

	public static float pitch_voiceRitsuko = 0f;

	public static float pitch_voiceAkiko = 0f;

	public static float pitch_voiceYukiko = 0f;

	public static float pitch_voiceHero = 0f;

	public static float pitch_voiceKouichi = 0f;

	public static bool anotherGameCardMessage = true;

	public static bool downloadCmpMsg = true;

	public static void Reset()
	{
		showFPS = false;
		showFocusUI = true;
		cameraTurnSpeed = 1f;
		cameraMoveSpeed = 1f;
		mouseSensitive = 1f;
		mouseRevV = false;
		mouseRevH = false;
		dragLock = false;
		keySensitive = 1f;
		keyRevV = false;
		keyRevH = false;
		defParse = 25f;
		dropShadow = true;
		postEffect = true;
		autoHideObstacle = true;
		showMob = true;
		showMirror = true;
		backLightIntensity = 0.8f;
		maleColor = new Color(0.5f, 0.75f, 1f, 0.8f);
		clearColor = new Color(0.25f, 0.25f, 0.25f, 1f);
		showCustomHighlight = true;
		h_camReset_position = true;
		h_camReset_style = true;
		h_action_continue = false;
		thumbsCacheSizeMB = 100;
		postProcessFlavor = 1;
		eyeAdaptationEnable = false;
		exposureCompensation = 0.5f;
		bloomEnable = true;
		bloomRate = 1f;
		lensDirtEnable = false;
		vignetteEnable = true;
		vignetteRate = 1f;
		noiseEnable = false;
		noiseRate = 1f;
		ssaoEnable = true;
		ssaoIntensity = 1f;
		ssaoRadius = 1f;
		dofEnable = true;
		dofRate = 0.7f;
		volume_master = 0.75f;
		volume_bgm = 0.3f;
		volume_system = 1f;
		volume_se = 1f;
		volume_env = 1f;
		volume_voiceAll = 1f;
		volume_voiceRitsuko = 1f;
		volume_voiceAkiko = 1f;
		volume_voiceYukiko = 1f;
		volume_voiceHero = 1f;
		volume_voiceKouichi = 1f;
		volume_voiceMob = 1f;
		reverb_flag = true;
		pitch_voiceRitsuko = 0f;
		pitch_voiceAkiko = 0f;
		pitch_voiceYukiko = 0f;
		pitch_voiceHero = 0f;
		pitch_voiceKouichi = 0f;
		anotherGameCardMessage = true;
		downloadCmpMsg = true;
		if (QualitySettings.GetQualityLevel() == 0)
		{
			dropShadow = false;
			showMirror = false;
			ssaoEnable = false;
			dofEnable = false;
		}
		else if (QualitySettings.GetQualityLevel() == 1)
		{
			dropShadow = false;
		}
		else if (QualitySettings.GetQualityLevel() == 2)
		{
			dropShadow = true;
		}
	}

	public static void Save()
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		string text = path + "/Config";
		FileStream fileStream = new FileStream(text, FileMode.Create);
		StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.WriteLine(version);
		Write(streamWriter, "showFocusUI", showFocusUI);
		Write(streamWriter, "cameraTurnSpeed", cameraTurnSpeed);
		Write(streamWriter, "cameraMoveSpeed", cameraMoveSpeed);
		Write(streamWriter, "mouseSensitive", mouseSensitive);
		Write(streamWriter, "mouseRevV", mouseRevV);
		Write(streamWriter, "mouseRevH", mouseRevH);
		Write(streamWriter, "dragLock", dragLock);
		Write(streamWriter, "keySensitive", keySensitive);
		Write(streamWriter, "keyRevV", keyRevV);
		Write(streamWriter, "keyRevH", keyRevH);
		Write(streamWriter, "centerDragMove", (int)centerDragMove);
		Write(streamWriter, "defParse", defParse);
		Write(streamWriter, "dropShadow", dropShadow);
		Write(streamWriter, "postEffect", postEffect);
		Write(streamWriter, "h_camReset_position", h_camReset_position);
		Write(streamWriter, "h_camReset_style", h_camReset_style);
		Write(streamWriter, "showFPS", showFPS);
		Write(streamWriter, "autoHideObstacle", autoHideObstacle);
		Write(streamWriter, "showMob", showMob);
		Write(streamWriter, "showMirror", showMirror);
		Write(streamWriter, "maleColor", maleColor);
		Write(streamWriter, "backLightIntensity", backLightIntensity);
		Write(streamWriter, "showCustomHighlight", showCustomHighlight);
		Write(streamWriter, "clearColor", clearColor);
		Write(streamWriter, "h_action_continue", h_action_continue);
		Write(streamWriter, "thumbsCacheSize", thumbsCacheSizeMB);
		Write(streamWriter, "postProcessFlavor", postProcessFlavor);
		Write(streamWriter, "eyeAdaptationEnable", eyeAdaptationEnable);
		Write(streamWriter, "exposureCompensation", exposureCompensation);
		Write(streamWriter, "bloomEnable", bloomEnable);
		Write(streamWriter, "bloomRate", bloomRate);
		Write(streamWriter, "lensDirtEnable", lensDirtEnable);
		Write(streamWriter, "vignetteEnable", vignetteEnable);
		Write(streamWriter, "vignetteRate", vignetteRate);
		Write(streamWriter, "noiseEnable", noiseEnable);
		Write(streamWriter, "noiseRate", noiseRate);
		Write(streamWriter, "ssaoEnable", ssaoEnable);
		Write(streamWriter, "ssaoIntensity", ssaoIntensity);
		Write(streamWriter, "ssaoRadius", ssaoRadius);
		Write(streamWriter, "dofEnable", dofEnable);
		Write(streamWriter, "dofRate", dofRate);
		Write(streamWriter, "volume_master", volume_master);
		Write(streamWriter, "volume_bgm", volume_bgm);
		Write(streamWriter, "volume_system", volume_system);
		Write(streamWriter, "volume_se", volume_se);
		Write(streamWriter, "volume_env", volume_env);
		Write(streamWriter, "volume_voiceAll", volume_voiceAll);
		Write(streamWriter, "volume_voiceRitsuko", volume_voiceRitsuko);
		Write(streamWriter, "volume_voiceAkiko", volume_voiceAkiko);
		Write(streamWriter, "volume_voiceYukiko", volume_voiceYukiko);
		Write(streamWriter, "volume_voiceHero", volume_voiceHero);
		Write(streamWriter, "volume_voiceKouichi", volume_voiceKouichi);
		Write(streamWriter, "volume_voiceMob", volume_voiceMob);
		Write(streamWriter, "reverb_flag", reverb_flag);
		Write(streamWriter, "pitch_voiceRitsuko", pitch_voiceRitsuko);
		Write(streamWriter, "pitch_voiceAkiko", pitch_voiceAkiko);
		Write(streamWriter, "pitch_voiceYukiko", pitch_voiceYukiko);
		Write(streamWriter, "pitch_voiceHero", pitch_voiceHero);
		Write(streamWriter, "pitch_voiceKouichi", pitch_voiceKouichi);
		Write(streamWriter, "anotherGameCardMessage", anotherGameCardMessage);
		Write(streamWriter, "downloadCmpMsg", downloadCmpMsg);
		streamWriter.Close();
		fileStream.Close();
	}

	private static void Write(StreamWriter writer, string name, int val)
	{
		writer.WriteLine(name + ":" + val);
	}

	private static void Write(StreamWriter writer, string name, float val)
	{
		writer.WriteLine(name + ":" + val);
	}

	private static void Write(StreamWriter writer, string name, bool val)
	{
		writer.WriteLine(name + ":" + val);
	}

	private static void Write(StreamWriter writer, string name, Color val)
	{
		writer.WriteLine(name + ":" + val.r + "," + val.g + "," + val.b + "," + val.a);
	}

	public static void Load()
	{
		string text = path + "/Config";
		if (!File.Exists(text))
		{
			Reset();
			return;
		}
		FileStream fileStream = new FileStream(text, FileMode.Open);
		if (fileStream == null)
		{
			return;
		}
		StreamReader streamReader = new StreamReader(fileStream);
		int num = int.Parse(streamReader.ReadLine());
		if (num < 0 || num > version)
		{
			Debug.LogError("不明なバージョン：" + num);
			streamReader.Close();
			fileStream.Close();
			return;
		}
		char[] separator = new char[1] { ':' };
		string empty = string.Empty;
		while ((empty = streamReader.ReadLine()) != null)
		{
			string[] array = empty.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				switch (array[0])
				{
				case "showFocusUI":
					Read(ref showFocusUI, array[1]);
					break;
				case "cameraTurnSpeed":
					Read(ref cameraTurnSpeed, array[1]);
					break;
				case "cameraMoveSpeed":
					Read(ref cameraMoveSpeed, array[1]);
					break;
				case "mouseSensitive":
					Read(ref mouseSensitive, array[1]);
					break;
				case "mouseRevV":
					Read(ref mouseRevV, array[1]);
					break;
				case "mouseRevH":
					Read(ref mouseRevH, array[1]);
					break;
				case "dragLock":
					Read(ref dragLock, array[1]);
					break;
				case "keySensitive":
					Read(ref keySensitive, array[1]);
					break;
				case "keyRevV":
					Read(ref keyRevV, array[1]);
					break;
				case "keyRevH":
					Read(ref keyRevH, array[1]);
					break;
				case "centerDragMove":
				{
					int val = 0;
					Read(ref val, array[1]);
					centerDragMove = (IllusionCamera.CenterDragMove)val;
					break;
				}
				case "defParse":
					Read(ref defParse, array[1]);
					break;
				case "dropShadow":
					Read(ref dropShadow, array[1]);
					break;
				case "h_camReset_position":
					Read(ref h_camReset_position, array[1]);
					break;
				case "h_camReset_style":
					Read(ref h_camReset_style, array[1]);
					break;
				case "showFPS":
					Read(ref showFPS, array[1]);
					break;
				case "autoHideObstacle":
					Read(ref autoHideObstacle, array[1]);
					break;
				case "showMob":
					Read(ref showMob, array[1]);
					break;
				case "showMirror":
					Read(ref showMirror, array[1]);
					break;
				case "maleColor":
					Read(ref maleColor, array[1]);
					break;
				case "backLightIntensity":
					Read(ref backLightIntensity, array[1]);
					break;
				case "showCustomHighlight":
					Read(ref showCustomHighlight, array[1]);
					break;
				case "clearColor":
					Read(ref clearColor, array[1]);
					break;
				case "h_action_continue":
					Read(ref h_action_continue, array[1]);
					break;
				case "thumbsCacheSize":
					Read(ref thumbsCacheSizeMB, array[1]);
					break;
				case "postProcessFlavor":
					Read(ref postProcessFlavor, array[1]);
					break;
				case "eyeAdaptationEnable":
					Read(ref eyeAdaptationEnable, array[1]);
					break;
				case "exposureCompensation":
					Read(ref exposureCompensation, array[1]);
					break;
				case "bloomEnable":
					Read(ref bloomEnable, array[1]);
					break;
				case "bloomRate":
					Read(ref bloomRate, array[1]);
					break;
				case "lensDirtEnable":
					Read(ref lensDirtEnable, array[1]);
					break;
				case "vignetteEnable":
					Read(ref vignetteEnable, array[1]);
					break;
				case "vignetteRate":
					Read(ref vignetteRate, array[1]);
					break;
				case "noiseEnable":
					Read(ref noiseEnable, array[1]);
					break;
				case "noiseRate":
					Read(ref noiseRate, array[1]);
					break;
				case "ssaoEnable":
					Read(ref ssaoEnable, array[1]);
					break;
				case "ssaoIntensity":
					Read(ref ssaoIntensity, array[1]);
					break;
				case "ssaoRadius":
					Read(ref ssaoRadius, array[1]);
					break;
				case "dofEnable":
					Read(ref dofEnable, array[1]);
					break;
				case "dofRate":
					Read(ref dofRate, array[1]);
					break;
				case "volume_master":
					Read(ref volume_master, array[1]);
					break;
				case "volume_bgm":
					Read(ref volume_bgm, array[1]);
					break;
				case "volume_system":
					Read(ref volume_system, array[1]);
					break;
				case "volume_se":
					Read(ref volume_se, array[1]);
					break;
				case "volume_env":
					Read(ref volume_env, array[1]);
					break;
				case "volume_voiceAll":
					Read(ref volume_voiceAll, array[1]);
					break;
				case "volume_voiceRitsuko":
					Read(ref volume_voiceRitsuko, array[1]);
					break;
				case "volume_voiceAkiko":
					Read(ref volume_voiceAkiko, array[1]);
					break;
				case "volume_voiceYukiko":
					Read(ref volume_voiceYukiko, array[1]);
					break;
				case "volume_voiceHero":
					Read(ref volume_voiceHero, array[1]);
					break;
				case "volume_voiceKouichi":
					Read(ref volume_voiceKouichi, array[1]);
					break;
				case "volume_voiceMob":
					Read(ref volume_voiceMob, array[1]);
					break;
				case "pitch_voiceRitsuko":
					Read(ref pitch_voiceRitsuko, array[1]);
					break;
				case "pitch_voiceAkiko":
					Read(ref pitch_voiceAkiko, array[1]);
					break;
				case "pitch_voiceYukiko":
					Read(ref pitch_voiceYukiko, array[1]);
					break;
				case "pitch_voiceHero":
					Read(ref pitch_voiceHero, array[1]);
					break;
				case "pitch_voiceKouichi":
					Read(ref pitch_voiceKouichi, array[1]);
					break;
				case "reverb_flag":
					Read(ref reverb_flag, array[1]);
					break;
				case "anotherGameCardMessage":
					Read(ref anotherGameCardMessage, array[1]);
					break;
				case "downloadCmpMsg":
					Read(ref downloadCmpMsg, array[1]);
					break;
				}
			}
		}
		streamReader.Close();
		fileStream.Close();
	}

	private static void Read(ref bool val, string str)
	{
		val = bool.Parse(str);
	}

	private static void Read(ref float val, string str)
	{
		val = float.Parse(str);
	}

	private static void Read(ref int val, string str)
	{
		val = int.Parse(str);
	}

	private static void Read(ref Color val, string str)
	{
		string[] array = str.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 4)
		{
			val.r = float.Parse(array[0]);
			val.g = float.Parse(array[1]);
			val.b = float.Parse(array[2]);
			val.a = float.Parse(array[3]);
		}
		else
		{
			Debug.LogError("コンフィグの色設定が読み込めなかった:" + str);
		}
	}

	public static float VolumeBGM()
	{
		return volume_master * volume_bgm;
	}

	public static float VolumeSyetem()
	{
		return volume_master * volume_system;
	}

	public static float VolumeSoundEffect()
	{
		return volume_master * volume_se;
	}

	public static float VolumeEnv()
	{
		return volume_master * volume_env;
	}

	public static float VolumeVoice_Ritsuko()
	{
		return volume_master * volume_voiceAll * volume_voiceRitsuko;
	}

	public static float VolumeVoice_Akiko()
	{
		return volume_master * volume_voiceAll * volume_voiceAkiko;
	}

	public static float VolumeVoice_Yukiko()
	{
		return volume_master * volume_voiceAll * volume_voiceYukiko;
	}

	public static float VolumeVoice_Hero()
	{
		return volume_master * volume_voiceAll * volume_voiceHero;
	}

	public static float VolumeVoice_Kouichi()
	{
		return volume_master * volume_voiceAll * volume_voiceKouichi;
	}

	public static float VolumeVoice_Mob()
	{
		return volume_master * volume_voiceAll * volume_voiceMob;
	}

	public static float VolumeVoice_Heroine(HEROINE heroineID)
	{
		float num = 1f;
		switch (heroineID)
		{
		case HEROINE.RITSUKO:
			num = volume_voiceRitsuko;
			break;
		case HEROINE.AKIKO:
			num = volume_voiceAkiko;
			break;
		case HEROINE.YUKIKO:
			num = volume_voiceYukiko;
			break;
		}
		return volume_master * volume_voiceAll * num;
	}

	public static float PitchVoice_Ritsuko()
	{
		return 1f + pitch_voiceRitsuko * pitchRate;
	}

	public static float PitchVoice_Akiko()
	{
		return 1f + pitch_voiceAkiko * pitchRate;
	}

	public static float PitchVoice_Yukiko()
	{
		return 1f + pitch_voiceYukiko * pitchRate;
	}

	public static float PitchVoice_Hero()
	{
		return 1f + pitch_voiceHero * pitchRate;
	}

	public static float PitchVoice_Kouichi()
	{
		return 1f + pitch_voiceKouichi * pitchRate;
	}

	public static float PitchVoice_Heroine(HEROINE heroineID)
	{
		float num = 1f;
		switch (heroineID)
		{
		case HEROINE.RITSUKO:
			num = pitch_voiceRitsuko;
			break;
		case HEROINE.AKIKO:
			num = pitch_voiceAkiko;
			break;
		case HEROINE.YUKIKO:
			num = pitch_voiceYukiko;
			break;
		}
		return 1f + num * pitchRate;
	}
}
