using System;
using System.IO;
using Character;
using UnityEngine;

public class Personality
{
	public enum STATE
	{
		FIRST = 0,
		RESIST = 1,
		FLOP = 2,
		FLIP_FLOP = 3,
		LAST_EVENT_SISTERS = 4,
		LAST_EVENT_YUKIKO_1 = 5,
		LAST_EVENT_YUKIKO_2 = 6,
		NUM = 7
	}

	public const int ExpMax_FeelVagina = 15;

	public const int ExpMax_FeelAnus = 15;

	public const int ExpMax_IndecentLanguage = 21;

	public const int ExpMax_LikeFeratio = 15;

	public const int ExpMax_LikeSperm = 15;

	public const int ExpMax_Add = 3;

	public bool vaginaVirgin = true;

	public bool analVirgin = true;

	public bool feelVagina;

	public bool feelAnus;

	public bool indecentLanguage;

	public bool likeFeratio;

	public bool likeSperm;

	public int expFeelVagina;

	public int expFeelAnus;

	public int expIndecentLanguage;

	public int expLikeFeratio;

	public int expLikeSperm;

	private int addFeelVagina;

	private int addFeelAnus;

	private int addIndecentLanguage;

	private int addLikeFeratio;

	private int addLikeSperm;

	public STATE state;

	public GAG_ITEM gagItem;

	public int continuousInsVagina;

	public int continuousInsAnal;

	public bool weakness;

	public bool ahe;

	public int xtc_count_vagina;

	public int xtc_count_anal;

	public int eja_count_vagina;

	public int eja_count_anal;

	public int eja_count;

	public int spermInCntV;

	public int spermInCntA;

	private bool lostVaginaVirgin;

	private bool lostAnalVirgin;

	public bool insertVagina;

	public bool insertAnal;

	public float ExpFeelVaginaRate
	{
		get
		{
			return (!feelVagina) ? Rate(expFeelVagina, 15) : 1f;
		}
	}

	public float ExpFeelAnusRate
	{
		get
		{
			return (!feelAnus) ? Rate(expFeelAnus, 15) : 1f;
		}
	}

	public float ExpIndecentLanguageRate
	{
		get
		{
			return (!indecentLanguage) ? Rate(expIndecentLanguage, 21) : 1f;
		}
	}

	public float ExpLikeFeratioRate
	{
		get
		{
			return (!likeFeratio) ? Rate(expLikeFeratio, 15) : 1f;
		}
	}

	public float ExpLikeSpermRate
	{
		get
		{
			return (!likeSperm) ? Rate(expLikeSperm, 15) : 1f;
		}
	}

	public void Init()
	{
		vaginaVirgin = true;
		analVirgin = true;
		feelVagina = false;
		feelAnus = false;
		indecentLanguage = false;
		likeFeratio = false;
		likeSperm = false;
		expFeelVagina = 0;
		expFeelAnus = 0;
		expIndecentLanguage = 0;
		expLikeFeratio = 0;
		expLikeSperm = 0;
		addFeelVagina = 0;
		addFeelAnus = 0;
		addIndecentLanguage = 0;
		addLikeFeratio = 0;
		addLikeSperm = 0;
		state = STATE.FIRST;
		weakness = false;
		ahe = false;
		gagItem = GAG_ITEM.NONE;
		continuousInsVagina = 0;
		continuousInsAnal = 0;
		xtc_count_vagina = 0;
		xtc_count_anal = 0;
		eja_count_vagina = 0;
		eja_count_anal = 0;
		eja_count = 0;
		spermInCntV = 0;
		spermInCntA = 0;
		lostVaginaVirgin = false;
		lostAnalVirgin = false;
		insertVagina = false;
		insertAnal = false;
	}

	public bool IsRequireAdjustment()
	{
		bool flag = false;
		flag |= !feelVagina && (float)addFeelVagina > 0f;
		flag |= !feelAnus && (float)addFeelAnus > 0f;
		flag |= !indecentLanguage && (float)addIndecentLanguage > 0f;
		flag |= !likeFeratio && (float)addLikeFeratio > 0f;
		flag |= !likeSperm && (float)addLikeSperm > 0f;
		return flag | (lostVaginaVirgin | lostAnalVirgin);
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write(vaginaVirgin);
		writer.Write(analVirgin);
		writer.Write(feelVagina);
		writer.Write(feelAnus);
		writer.Write(indecentLanguage);
		writer.Write(likeFeratio);
		writer.Write(likeSperm);
		writer.Write(expFeelVagina);
		writer.Write(expFeelAnus);
		writer.Write(expIndecentLanguage);
		writer.Write(expLikeFeratio);
		writer.Write(expLikeSperm);
		writer.Write(addFeelVagina);
		writer.Write(addFeelAnus);
		writer.Write(addIndecentLanguage);
		writer.Write(addLikeFeratio);
		writer.Write(addLikeSperm);
		writer.Write((int)state);
		writer.Write((int)gagItem);
		writer.Write(continuousInsVagina);
		writer.Write(continuousInsAnal);
	}

	public void Load(BinaryReader reader, int version)
	{
		vaginaVirgin = reader.ReadBoolean();
		analVirgin = reader.ReadBoolean();
		feelVagina = reader.ReadBoolean();
		feelAnus = reader.ReadBoolean();
		indecentLanguage = reader.ReadBoolean();
		likeFeratio = reader.ReadBoolean();
		likeSperm = reader.ReadBoolean();
		expFeelVagina = reader.ReadInt32();
		expFeelAnus = reader.ReadInt32();
		expIndecentLanguage = reader.ReadInt32();
		expLikeFeratio = reader.ReadInt32();
		expLikeSperm = reader.ReadInt32();
		addFeelVagina = reader.ReadInt32();
		addFeelAnus = reader.ReadInt32();
		addIndecentLanguage = reader.ReadInt32();
		addLikeFeratio = reader.ReadInt32();
		addLikeSperm = reader.ReadInt32();
		state = (STATE)reader.ReadInt32();
		if (version >= 1)
		{
			gagItem = (GAG_ITEM)reader.ReadInt32();
		}
		if (version >= 3)
		{
			continuousInsVagina = reader.ReadInt32();
			continuousInsAnal = reader.ReadInt32();
		}
	}

	private static float Rate(int now, int max)
	{
		if (max == 0)
		{
			return 1f;
		}
		return Mathf.Clamp01((float)now / (float)max);
	}

	public void AdjustmentExp()
	{
		expFeelVagina = Mathf.Min(expFeelVagina + addFeelVagina, 15);
		expFeelAnus = Mathf.Min(expFeelAnus + addFeelAnus, 15);
		expIndecentLanguage = Mathf.Min(expIndecentLanguage + addIndecentLanguage, 21);
		expLikeFeratio = Mathf.Min(expLikeFeratio + addLikeFeratio, 15);
		expLikeSperm = Mathf.Min(expLikeSperm + addLikeSperm, 15);
		addFeelVagina = 0;
		addFeelAnus = 0;
		addIndecentLanguage = 0;
		addLikeFeratio = 0;
		addLikeSperm = 0;
		if (lostVaginaVirgin)
		{
			vaginaVirgin = false;
			lostVaginaVirgin = false;
		}
		if (lostAnalVirgin)
		{
			analVirgin = false;
			lostAnalVirgin = false;
		}
	}

	public void AdjustmentExp_Free()
	{
		addFeelVagina = 0;
		addFeelAnus = 0;
		addIndecentLanguage = 0;
		addLikeFeratio = 0;
		addLikeSperm = 0;
		lostVaginaVirgin = false;
		lostAnalVirgin = false;
	}

	public void H_In()
	{
		weakness = false;
		ahe = false;
		xtc_count_vagina = 0;
		xtc_count_anal = 0;
		eja_count_vagina = 0;
		eja_count_anal = 0;
		eja_count = 0;
		spermInCntV = 0;
		spermInCntA = 0;
	}

	public void AddFeelVagina()
	{
		if (!feelVagina)
		{
			addFeelVagina = Mathf.Min(addFeelVagina + 1, 3);
		}
	}

	public void AddFeelAnus()
	{
		if (!feelAnus)
		{
			addFeelAnus = Mathf.Min(addFeelAnus + 1, 3);
		}
	}

	public void AddIndecentLanguage()
	{
		if (!indecentLanguage)
		{
			addIndecentLanguage = Mathf.Min(addIndecentLanguage + 1, 3);
		}
	}

	public void AddLikeFeratio()
	{
		if (!likeFeratio)
		{
			addLikeFeratio = Mathf.Min(addLikeFeratio + 1, 3);
		}
	}

	public void AddLikeSperm()
	{
		if (!likeSperm)
		{
			addLikeSperm = Mathf.Min(addLikeSperm + 1, 3);
		}
	}

	public void LostVaginaVirgin()
	{
		lostVaginaVirgin = true;
	}

	public void LostAnalVirgin()
	{
		lostAnalVirgin = true;
	}

	public bool IsLostVaginaVirgin()
	{
		return lostVaginaVirgin;
	}

	public bool IsLostAnalVirgin()
	{
		return lostAnalVirgin;
	}

	public bool IsFloped(HEROINE heroine)
	{
		return IsFloped(state, heroine);
	}

	public static bool IsFloped(STATE checkState, HEROINE heroine)
	{
		if (checkState == STATE.FLIP_FLOP)
		{
			return heroine != HEROINE.AKIKO;
		}
		return checkState > STATE.RESIST;
	}
}
