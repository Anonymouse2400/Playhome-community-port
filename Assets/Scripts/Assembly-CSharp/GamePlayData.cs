using System;
using System.IO;
using Character;
using UnityEngine;

public class GamePlayData
{
	public enum PROGRESS
	{
		FIRST_RITSUKO = 0,
		FIRST_AKIKO = 1,
		FIRST_YUKIKO = 2,
		RESIST = 3,
		FLIPFLOP_YUKIKO = 4,
		FLIPFLOP_AKIKO = 5,
		FLIPFLOP_RITSUKO = 6,
		FLOP = 7,
		END_YUKIKO_1 = 8,
		END_SISTERS = 9,
		END_YUKIKO_2 = 10,
		ALL_FREE = 11
	}

	private SaveHeader header = new SaveHeader(3);

	private PROGRESS progress;

	public CustomParameter custom_ritsuko;

	public CustomParameter custom_akiko;

	public CustomParameter custom_yukiko;

	public CustomParameter custom_hero;

	public CustomParameter custom_kouichi;

	public CustomParameter custom_h_maleMobA;

	public CustomParameter custom_h_maleMobB;

	public CustomParameter custom_h_maleMobC;

	public Personality[] personality = new Personality[3];

	public MALE_ID lastSelectMale;

	public HEROINE lastSelectFemale;

	public VISITOR lastSelectVisitor = VISITOR.NONE;

	public int lastSelectMap;

	public int lastSelectTimeZone;

	public bool unlockWeaknessRecovery;

	public bool unlockShowHitArea;

	public bool unlockFastXtc;

	public bool readAllFreeMessage;

	public SaveHeader Header
	{
		get
		{
			return header;
		}
	}

	public PROGRESS Progress
	{
		get
		{
			return progress;
		}
		set
		{
			SetProgress(value);
		}
	}

	public GamePlayData()
	{
		for (int i = 0; i < personality.Length; i++)
		{
			personality[i] = new Personality();
		}
		Clear();
	}

	public void Clear()
	{
		progress = PROGRESS.FIRST_RITSUKO;
		for (int i = 0; i < personality.Length; i++)
		{
			personality[i].Init();
		}
		lastSelectMale = MALE_ID.HERO;
		lastSelectFemale = HEROINE.RITSUKO;
		lastSelectVisitor = VISITOR.NONE;
		lastSelectMap = 0;
		lastSelectTimeZone = 0;
		unlockWeaknessRecovery = false;
		unlockShowHitArea = false;
		unlockFastXtc = false;
		readAllFreeMessage = false;
	}

	public void Start()
	{
		Clear();
		TextAsset text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "MaleA");
		custom_hero = new CustomParameter(SEX.MALE);
		custom_hero.Load(text, false, true);
		text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "FemaleA");
		custom_ritsuko = new CustomParameter(SEX.FEMALE);
		custom_ritsuko.Load(text, true, false);
		personality[0] = new Personality();
		personality[0].feelAnus = true;
		personality[0].expFeelAnus = 999;
		text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "FemaleB");
		custom_akiko = new CustomParameter(SEX.FEMALE);
		custom_akiko.Load(text, true, false);
		personality[1] = new Personality();
		personality[1].indecentLanguage = true;
		personality[1].expIndecentLanguage = 999;
		text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "FemaleC");
		custom_yukiko = new CustomParameter(SEX.FEMALE);
		custom_yukiko.Load(text, true, false);
		personality[2] = new Personality();
		personality[2].feelVagina = true;
		personality[2].expFeelVagina = 999;
		personality[2].vaginaVirgin = false;
		personality[2].analVirgin = true;
		text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "MaleB");
		custom_kouichi = new CustomParameter(SEX.MALE);
		custom_kouichi.Load(text, false, true);
		text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "MobA");
		custom_h_maleMobA = new CustomParameter(SEX.MALE);
		custom_h_maleMobA.Load(text, false, true);
		text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "MobB");
		custom_h_maleMobB = new CustomParameter(SEX.MALE);
		custom_h_maleMobB.Load(text, false, true);
		text = AssetBundleLoader.LoadAsset<TextAsset>(Application.persistentDataPath + "/abdata", "def_card", "MobC");
		custom_h_maleMobC = new CustomParameter(SEX.MALE);
		custom_h_maleMobC.Load(text, false, true);
	}

	public void Save(string path, string comment)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = File.OpenWrite(path);
		}
		catch (Exception ex)
		{
			MonoBehaviour.print("ファイルが開けません:" + path + " " + ex);
			return;
		}
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		header.version = 3;
		header.comment = comment;
		header.SetNowTime();
		header.Save(binaryWriter);
		binaryWriter.Write((int)progress);
		custom_hero.Save(binaryWriter);
		custom_kouichi.Save(binaryWriter);
		custom_h_maleMobA.Save(binaryWriter);
		custom_h_maleMobB.Save(binaryWriter);
		custom_h_maleMobC.Save(binaryWriter);
		binaryWriter.Write(3);
		custom_ritsuko.Save(binaryWriter);
		custom_akiko.Save(binaryWriter);
		custom_yukiko.Save(binaryWriter);
		for (int i = 0; i < personality.Length; i++)
		{
			personality[i].Save(binaryWriter);
		}
		binaryWriter.Write((int)lastSelectMale);
		binaryWriter.Write((int)lastSelectFemale);
		binaryWriter.Write((int)lastSelectVisitor);
		binaryWriter.Write(lastSelectMap);
		binaryWriter.Write(lastSelectTimeZone);
		binaryWriter.Write(unlockWeaknessRecovery);
		binaryWriter.Write(unlockShowHitArea);
		binaryWriter.Write(unlockFastXtc);
		binaryWriter.Write(readAllFreeMessage);
		fileStream.Close();
	}

	public void Load(string path)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = File.OpenRead(path);
		}
		catch
		{
			return;
		}
		if (fileStream == null)
		{
			return;
		}
		BinaryReader binaryReader = new BinaryReader(fileStream);
		if (!header.Load(binaryReader))
		{
			fileStream.Close();
			return;
		}
		Clear();
		progress = (PROGRESS)binaryReader.ReadInt32();
		custom_hero.Load(binaryReader);
		custom_kouichi.Load(binaryReader);
		custom_h_maleMobA.Load(binaryReader);
		custom_h_maleMobB.Load(binaryReader);
		custom_h_maleMobC.Load(binaryReader);
		int num = binaryReader.ReadInt32();
		custom_ritsuko.Load(binaryReader);
		custom_akiko.Load(binaryReader);
		custom_yukiko.Load(binaryReader);
		if (num < 3)
		{
		}
		for (int i = 0; i < personality.Length; i++)
		{
			personality[i].Load(binaryReader, header.version);
		}
		lastSelectMale = (MALE_ID)binaryReader.ReadInt32();
		lastSelectFemale = (HEROINE)binaryReader.ReadInt32();
		lastSelectVisitor = (VISITOR)binaryReader.ReadInt32();
		lastSelectMap = binaryReader.ReadInt32();
		lastSelectTimeZone = binaryReader.ReadInt32();
		if (header.version >= 2)
		{
			unlockWeaknessRecovery = binaryReader.ReadBoolean();
			unlockShowHitArea = binaryReader.ReadBoolean();
			unlockFastXtc = binaryReader.ReadBoolean();
			readAllFreeMessage = binaryReader.ReadBoolean();
		}
		fileStream.Close();
	}

	public Personality GetHeroinePersonality(HEROINE id)
	{
		if (id >= HEROINE.RITSUKO && id < HEROINE.NUM)
		{
			return personality[(int)id];
		}
		return null;
	}

	public CustomParameter GetHeroineCustomParam(HEROINE heroineID)
	{
		switch (heroineID)
		{
		case HEROINE.RITSUKO:
			return custom_ritsuko;
		case HEROINE.AKIKO:
			return custom_akiko;
		case HEROINE.YUKIKO:
			return custom_yukiko;
		default:
			return null;
		}
	}

	public CustomParameter GetMaleCustomParam(MALE_ID maleID)
	{
		switch (maleID)
		{
		case MALE_ID.HERO:
			return custom_hero;
		case MALE_ID.KOUICHI:
			return custom_kouichi;
		case MALE_ID.MOB_A:
			return custom_h_maleMobA;
		case MALE_ID.MOB_B:
			return custom_h_maleMobB;
		case MALE_ID.MOB_C:
			return custom_h_maleMobC;
		default:
			return null;
		}
	}

	public bool IsHeroineFloped(HEROINE heroineID)
	{
		return personality[(int)heroineID].IsFloped(heroineID);
	}

	public int GetBadgeNum()
	{
		int num = 0;
		for (int i = 0; i < personality.Length; i++)
		{
			if (personality[i].feelVagina)
			{
				num++;
			}
			if (personality[i].feelAnus)
			{
				num++;
			}
			if (personality[i].indecentLanguage)
			{
				num++;
			}
			if (personality[i].likeFeratio)
			{
				num++;
			}
			if (personality[i].likeSperm)
			{
				num++;
			}
		}
		return num;
	}

	public bool IsFlopFromBadgeNum()
	{
		for (int i = 0; i < 3; i++)
		{
			int num = 0;
			if (personality[i].feelVagina)
			{
				num++;
			}
			if (personality[i].feelAnus)
			{
				num++;
			}
			if (personality[i].indecentLanguage)
			{
				num++;
			}
			if (personality[i].likeFeratio)
			{
				num++;
			}
			if (personality[i].likeSperm)
			{
				num++;
			}
			if (num < 3)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsAllFreeFromBadgeNum()
	{
		for (int i = 0; i < personality.Length; i++)
		{
			if (!personality[i].feelVagina)
			{
				return false;
			}
			if (!personality[i].feelAnus)
			{
				return false;
			}
			if (!personality[i].indecentLanguage)
			{
				return false;
			}
			if (!personality[i].likeFeratio)
			{
				return false;
			}
			if (!personality[i].likeSperm)
			{
				return false;
			}
		}
		return true;
	}

	public void SetProgress(PROGRESS next)
	{
		if (next >= progress)
		{
			progress = next;
		}
		else
		{
			Debug.LogWarning("進捗が戻ろうとした");
		}
	}
}
