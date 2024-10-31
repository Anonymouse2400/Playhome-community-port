using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Character;
using SEXY;
using UnityEngine;
using Utility;

public abstract class Human : MonoBehaviour
{
	public static readonly int AnmParam_Height = Animator.StringToHash("Height");

	public DefParam defParam;

	protected float eyeOpen = 1f;

	protected float mouthOpen;

	protected string eyeState = "Eye_Def";

	protected string mouthState = "Mouth_Def";

	protected string lipSyncState = string.Empty;

	protected float nowTearsRate;

	protected float nowFlushRate;

	protected LookAtRotator eyeLook;

	protected LookAtRotator.TYPE eyeLookType;

	protected Transform eyeLookTarget;

	protected bool eyeLookForce;

	protected LookAtRotator neckLook;

	protected LookAtRotator.TYPE neckLookType;

	protected Transform neckLookTarget;

	protected bool neckLookForce;

	protected Dictionary<string, HumanAttachItem> attachItem = new Dictionary<string, HumanAttachItem>();

	protected List<HumanAttachItem> restrictItems = new List<HumanAttachItem>();

	protected HumanAttachItem gagItem;

	protected bool gagShow = true;

	public SEX sex { get; protected set; }

	public CustomParameter customParam { get; protected set; }

	public Head head { get; protected set; }

	public Body body { get; protected set; }

	public Hairs hairs { get; protected set; }

	public Wears wears { get; protected set; }

	public Accessories accessories { get; protected set; }

	public AnimeLipSync lipSync { get; protected set; }

	public AnimeParamBlink blink { get; protected set; }

	public IK_Control ik { get; protected set; }

	public float TearsRate { get; protected set; }

	public float FlushRate { get; protected set; }

	public TONGUE_TYPE TongueType { get; protected set; }

	public LookAtRotator EyeLook
	{
		get
		{
			return eyeLook;
		}
	}

	public LookAtRotator NeckLook
	{
		get
		{
			return neckLook;
		}
	}

	public Transform HeadPosTrans { get; protected set; }

	public Vector3 FacePos
	{
		get
		{
			return HeadPosTrans.TransformPoint(new Vector3(0f, 0f, 0.08f));
		}
	}

	public Transform BrestPosTrans { get; protected set; }

	public Transform CrotchTrans { get; protected set; }

	public bool IsRestrict { get; protected set; }

	public GAG_ITEM GagType { get; protected set; }

	public bool GagShow
	{
		get
		{
			return gagShow;
		}
		set
		{
			ChangeShowGag(value);
		}
	}

	public bool Gag
	{
		get
		{
			return gagItem != null && gagShow;
		}
	}

	public void SetDefParam()
	{
		customParam.head.eyeScleraColorL = defParam.eyeScleraColor;
		customParam.head.eyeScleraColorL = defParam.eyeScleraColor;
		customParam.head.eyeIrisColorL = defParam.eyeIrisColor;
		customParam.head.eyeIrisColorR = defParam.eyeIrisColor;
		customParam.head.eyePupilDilationL = 0f;
		customParam.head.eyePupilDilationR = 0f;
		customParam.head.eyeEmissiveL = 0.5f;
		customParam.head.eyeEmissiveR = 0.5f;
	}

	public abstract void Save(string file);

	public abstract void Save(string file, Texture2D tex);

	public abstract LOAD_MSG Load(string file, int filter = -1);

	public abstract LOAD_MSG Load(TextAsset text, int filter = -1);

	public abstract void SaveCoordinate(string file);

	public abstract void SaveCoordinate(string file, Texture2D tex);

	public abstract LOAD_MSG LoadCoordinate(string file, int filter = -1);

	public abstract LOAD_MSG LoadCoordinate(TextAsset text, int filter = -1);

	public void Load(CustomParameter copy)
	{
		customParam.Copy(copy, -1);
		Apply();
	}

	protected LOAD_MSG Load(TextAsset text, bool female, bool male, int filter = -1)
	{
		if (text == null)
		{
			Debug.LogError("不明なバイナリカード");
			return LOAD_MSG.DO_NOT_LOAD;
		}

		using (MemoryStream input = new MemoryStream(text.bytes))
		{
			using (BinaryReader reader = new BinaryReader(input))
			{
				return Load(reader, female, male, filter);
			}
		}
	}

	protected LOAD_MSG Load(string file, bool female, bool male, int filter = -1)
	{
		LOAD_MSG lOAD_MSG = LOAD_MSG.PERFECT;
		using (FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read))
		{
			using (BinaryReader reader = new BinaryReader(input))
			{
				lOAD_MSG = Load(reader, female, male, filter);
				if ((lOAD_MSG & LOAD_MSG.DO_NOT_LOAD) != 0)
				{
					Debug.LogError("読み込みに失敗しました:" + file);
				}
			}
		}
		return lOAD_MSG;
	}

	protected LOAD_MSG Load(BinaryReader reader, bool female, bool male, int filter = -1)
	{
		LOAD_MSG lOAD_MSG = LOAD_MSG.PERFECT;
		bool flag = false;
		CustomParameter customParameter = new CustomParameter(customParam);
		try
		{
			long offset = PNG_Loader.CheckSize(reader);
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			string text = reader.ReadString();
			switch (text)
			{
			case "【HoneySelectCharaFemale】":
				Debug.Log("ハニーセレクト：女");
				lOAD_MSG |= LOAD_MSG.VER_HONEYSELECT;
				if (female)
				{
					reader.BaseStream.Seek(offset, SeekOrigin.Begin);
					CharFemaleFile charFemaleFile2 = new CharFemaleFile();
					flag = charFemaleFile2.Load(reader);
					customParameter.FromSexyData(charFemaleFile2);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			case "【PremiumResortCharaFemale】":
				Debug.Log("セクシービーチプレミアムリゾート：女");
				lOAD_MSG |= LOAD_MSG.VER_SEXYBEACH;
				if (female)
				{
					reader.BaseStream.Seek(0L, SeekOrigin.Begin);
					CharFemaleFile charFemaleFile = new CharFemaleFile();
					flag = charFemaleFile.LoadFromSBPR(reader);
					customParameter.FromSexyData(charFemaleFile);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			case "【PlayHome_Female】":
				if (female)
				{
					customParameter.Load(reader);
					flag = true;
				}
				else
				{
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
					Debug.LogWarning("異性データ");
				}
				break;
			case "【HoneySelectCharaMale】":
				Debug.Log("ハニーセレクト：男");
				lOAD_MSG |= LOAD_MSG.VER_HONEYSELECT;
				if (male)
				{
					reader.BaseStream.Seek(offset, SeekOrigin.Begin);
					CharMaleFile charMaleFile = new CharMaleFile();
					flag = charMaleFile.Load(reader, true);
					customParameter.FromSexyData(charMaleFile);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			case "【PremiumResortCharaMale】":
				Debug.Log("セクシービーチプレミアムリゾート：男");
				lOAD_MSG |= LOAD_MSG.VER_SEXYBEACH;
				if (male)
				{
					reader.BaseStream.Seek(0L, SeekOrigin.Begin);
					CharMaleFile charMaleFile2 = new CharMaleFile();
					flag = charMaleFile2.LoadFromSBPR(reader);
					customParameter.FromSexyData(charMaleFile2);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			case "【PlayHome_Male】":
				if (male)
				{
					customParameter.Load(reader);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			default:
				Debug.LogWarning("読めないセーブデータ:" + text);
				lOAD_MSG |= LOAD_MSG.DO_NOT_LOAD;
				break;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			lOAD_MSG |= LOAD_MSG.DO_NOT_LOAD;
			flag = false;
		}
		if (flag)
		{
			customParam.Copy(customParameter, filter);
			Apply();
		}
		return lOAD_MSG;
	}

	protected LOAD_MSG LoadCoordinate(TextAsset text, bool female, bool male, int filter = -1)
	{
		if (text == null)
		{
			Debug.LogError("不明なバイナリカード");
			return LOAD_MSG.DO_NOT_LOAD;
		}
		LOAD_MSG lOAD_MSG = LOAD_MSG.PERFECT;
		using (MemoryStream input = new MemoryStream(text.bytes))
		{
			using (BinaryReader reader = new BinaryReader(input))
			{
				return lOAD_MSG | LoadCoordinate(reader, female, male, filter);
			}
		}
	}

	protected LOAD_MSG LoadCoordinate(string file, bool female, bool male, int filter = -1)
	{
		LOAD_MSG lOAD_MSG = LOAD_MSG.PERFECT;
		using (FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read))
		{
			using (BinaryReader reader = new BinaryReader(input))
			{
				return lOAD_MSG | LoadCoordinate(reader, female, male, filter);
			}
		}
	}

	protected LOAD_MSG LoadCoordinate(BinaryReader reader, bool female, bool male, int filter = -1)
	{
		LOAD_MSG lOAD_MSG = LOAD_MSG.PERFECT;
		bool flag = false;
		CustomParameter customParameter = new CustomParameter(customParam);
		try
		{
			long offset = PNG_Loader.CheckSize(reader);
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			string text = reader.ReadString();
			switch (text)
			{
			case "【HoneySelectClothesFemale】":
				Debug.Log("ハニーセレクト：女");
				if (female)
				{
					reader.BaseStream.Seek(0L, SeekOrigin.Begin);
					CharFileInfoClothesFemale charFileInfoClothesFemale = new CharFileInfoClothesFemale();
					flag = charFileInfoClothesFemale.Load(reader);
					customParameter.FromSexyCoordinateData(charFileInfoClothesFemale);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			case "【PlayHome_FemaleCoordinate】":
				if (female)
				{
					customParameter.LoadCoordinate(reader);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			case "【HoneySelectClothesMale】":
				Debug.Log("ハニーセレクト：男");
				if (male)
				{
					reader.BaseStream.Seek(0L, SeekOrigin.Begin);
					CharFileInfoClothesMale charFileInfoClothesMale = new CharFileInfoClothesMale();
					flag = charFileInfoClothesMale.Load(reader);
					customParameter.FromSexyCoordinateData(charFileInfoClothesMale);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			case "【PlayHome_MaleCoordinate】":
				if (male)
				{
					customParameter.LoadCoordinate(reader);
					flag = true;
				}
				else
				{
					Debug.LogWarning("異性データ");
					lOAD_MSG |= LOAD_MSG.ISOMERISM;
				}
				break;
			default:
				Debug.LogWarning("読めないセーブデータ:" + text);
				lOAD_MSG |= LOAD_MSG.DO_NOT_LOAD;
				break;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			flag = false;
		}
		if (flag)
		{
			customParam.Copy(customParameter, filter);
			ApplyCoordinate();
		}
		return lOAD_MSG;
	}

	public abstract void Apply();

	public abstract void ApplyCoordinate();

	public abstract void ApplyHair();

	public abstract void ChangeHead();

	protected void ChangeHead(string hairParentName)
	{
		accessories.DetachHeadAccessory(customParam.acce);
		hairs.DetachHairs();
		HumanAttachItem humanAttachItem = DetachGag();
		head.Load(customParam.head);
		Transform parent = Transform_Utility.FindTransform(head.Obj.transform, hairParentName);
		hairs.AttachHairs(parent);
		accessories.AttachHeadAccessory(customParam.acce);
		lipSync = GetComponentInChildren<AnimeLipSync>();
		blink = GetComponentInChildren<AnimeParamBlink>();
		head.SetShowTongue(TongueType == TONGUE_TYPE.FACE);
		ReAttachGag();
	}

	protected virtual void Update()
	{
		head.Update();
		body.Update();
		UpdateVoiceVolume();
	}

	protected virtual void LateUpdate()
	{
		ik.Update();
	}

	protected IEnumerator ToCharaPNG(string file, string mark)
	{
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		tex.Apply();
		byte[] png_bytes = tex.EncodeToPNG();
		UnityEngine.Object.Destroy(tex);
		using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				binaryWriter.Write(png_bytes);
				binaryWriter.Write(mark);
				customParam.Save(binaryWriter);
				binaryWriter.Close();
			}
			fileStream.Close();
		}
	}

	protected void ToCharaPNG(string file, string mark, Texture2D tex)
	{
		byte[] buffer = tex.EncodeToPNG();
		using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				binaryWriter.Write(buffer);
				binaryWriter.Write(mark);
				customParam.Save(binaryWriter);
				binaryWriter.Close();
			}
			fileStream.Close();
		}
	}

	protected IEnumerator ToCoordinatePNG(string file, string mark)
	{
		yield return new WaitForEndOfFrame();
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		tex.Apply();
		byte[] png_bytes = tex.EncodeToPNG();
		UnityEngine.Object.Destroy(tex);
		using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				binaryWriter.Write(png_bytes);
				binaryWriter.Write(mark);
				customParam.SaveCoordinate(binaryWriter);
				binaryWriter.Close();
			}
			fileStream.Close();
		}
	}

	protected void ToCoordinatePNG(string file, string mark, Texture2D tex)
	{
		byte[] buffer = tex.EncodeToPNG();
		using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				binaryWriter.Write(buffer);
				binaryWriter.Write(mark);
				customParam.SaveCoordinate(binaryWriter);
				binaryWriter.Close();
			}
			fileStream.Close();
		}
	}

	public void PhonationVoice(AudioClip clip, bool loop)
	{
		if (lipSync == null)
		{
			lipSync = GetComponentInChildren<AnimeLipSync>();
		}
		UpdateVoiceVolume();
		lipSync.audio.Stop();
		lipSync.audio.clip = clip;
		lipSync.audio.loop = loop;
		lipSync.audio.Play();
	}

	public void SoundSpatialBlend(float blend)
	{
		lipSync.audio.spatialBlend = blend;
	}

	public void VoiceShutUp()
	{
		if (lipSync == null)
		{
			lipSync = GetComponentInChildren<AnimeLipSync>();
		}
		lipSync.audio.Stop();
	}

	private void UpdateVoiceVolume()
	{
		if (!(lipSync == null))
		{
			lipSync.audio.volume = GetVolume();
			lipSync.audio.pitch = GetVoicePitch();
		}
	}

	protected abstract float GetVolume();

	protected abstract float GetVoicePitch();

	public bool IsVoicePlaying()
	{
		if (lipSync != null && lipSync.audio.clip != null)
		{
			return lipSync.audio.isPlaying;
		}
		return false;
	}

	public bool IsLoopVoicePlaying()
	{
		if (lipSync != null && lipSync.audio.clip != null)
		{
			return lipSync.audio.isPlaying && lipSync.audio.loop;
		}
		return false;
	}

	public virtual void BodyShapeInfoApply()
	{
		for (int i = 0; i < customParam.body.shapeVals.Length; i++)
		{
			body.Info.ChangeValue(i, customParam.body.shapeVals[i]);
		}
		body.Info.Update();
	}

	public virtual void OnShapeApplied()
	{
	}

	public virtual void SetupDynamicBones()
	{
	}

	public virtual void UpdateSkinMaterial()
	{
		customParam.body.skinColor.SetToMaterial(body.SkinMaterial);
		customParam.body.skinColor.SetToMaterial(head.SkinMaterial);
	}

	protected virtual string MouthExpressionCheck(string name)
	{
		return name;
	}

	public void ExpressionPlay(int layer, string name, float fixedTime)
	{
		switch (layer)
		{
		case 0:
			eyeState = name;
			break;
		case 1:
			mouthState = name;
			name = MouthExpressionCheck(name);
			break;
		case 2:
			lipSyncState = name;
			break;
		}
		if (head.Anime != null)
		{
			if (fixedTime <= 0f)
			{
				head.Anime.Play(name, layer);
			}
			else
			{
				head.Anime.CrossFadeInFixedTime(name, fixedTime, layer);
			}
		}
	}

	public void OpenEye(float v)
	{
		eyeOpen = v;
		if ((bool)blink)
		{
			blink.LimitMax = v;
		}
	}

	public void OpenMouth(float v)
	{
		mouthOpen = v;
		if ((bool)lipSync)
		{
			lipSync.RelaxOpen = v;
		}
	}

	public virtual void SetFlush(float rate, bool force = false)
	{
		FlushRate = rate;
	}

	public virtual void SetTear(float rate, bool force = false)
	{
		TearsRate = rate;
	}

	public virtual void SetTongueType(TONGUE_TYPE type)
	{
		TongueType = type;
		head.SetShowTongue(type == TONGUE_TYPE.FACE);
		body.SetShowTongue(type == TONGUE_TYPE.BODY);
	}

	public abstract void CheckShow();

	public void ChangeEyeLook(LookAtRotator.TYPE type, Transform tgt, bool force)
	{
		eyeLookType = type;
		eyeLookTarget = tgt;
		eyeLookForce = force;
		if (eyeLook != null)
		{
			eyeLook.Change(type, tgt, force);
		}
	}

	public void ChangeEyeLook(LookAtRotator.TYPE type, Vector3 tgt, bool force)
	{
		eyeLookType = type;
		eyeLookTarget = null;
		eyeLookForce = force;
		if (eyeLook != null)
		{
			eyeLook.Change(type, tgt, force);
		}
	}

	public void ChangeNeckLook(LookAtRotator.TYPE type, Transform tgt, bool force)
	{
		neckLookType = type;
		neckLookTarget = tgt;
		neckLookForce = force;
		if (neckLook != null)
		{
			neckLook.Change(type, tgt, force);
		}
	}

	public void ChangeNeckLook(LookAtRotator.TYPE type, Vector3 tgt, bool force)
	{
		neckLookType = type;
		neckLookTarget = null;
		neckLookForce = force;
		if (neckLook != null)
		{
			neckLook.Change(type, tgt, force);
		}
	}

	public virtual void Foot(MapData.FOOT foot)
	{
	}

	protected void AddAttachItem(HumanAttachItem item)
	{
		attachItem.Add(item.name, item);
	}

	protected void DelAttachItem(string name)
	{
		if (attachItem.ContainsKey(name))
		{
            UnityEngine.Object.Destroy(attachItem[name].obj);
			attachItem.Remove(name);
		}
	}

	protected void ClearAttachItems()
	{
		foreach (HumanAttachItem value in attachItem.Values)
		{
			UnityEngine.Object.Destroy(value.obj);
		}
		attachItem.Clear();
	}

	protected HumanAttachItem GetAttachItems(string name)
	{
		if (attachItem.ContainsKey(name))
		{
			return attachItem[name];
		}
		return null;
	}

	public virtual void ChangeGagItem()
	{
	}

	public virtual void ChangeShowGag(bool flag)
	{
		gagShow = flag;
	}

	protected virtual HumanAttachItem DetachGag()
	{
		if (gagItem != null)
		{
			gagItem.obj.transform.SetParent(null);
		}
		return gagItem;
	}

	protected virtual void ReAttachGag()
	{
	}

	public void RePlayMouthExpression()
	{
		ExpressionPlay(1, mouthState, 0f);
		OpenMouth(mouthOpen);
	}

	public void ChangeRestrict(bool set)
	{
		IsRestrict = set;
		if (!IsRestrict)
		{
			for (int i = 0; i < restrictItems.Count; i++)
			{
				DelAttachItem(restrictItems[i].name);
			}
			restrictItems.Clear();
		}
		if (IsRestrict && restrictItems.Count == 0)
		{
			GameObject gameObject = ResourceUtility.CreateInstance<GameObject>("RestrictRope");
			GameObject gameObject2 = ResourceUtility.CreateInstance<GameObject>("RestrictRope");
			Transform parent = Transform_Utility.FindTransform(body.Anime.transform, "N_Wrist_L");
			Transform parent2 = Transform_Utility.FindTransform(body.Anime.transform, "N_Wrist_R");
			gameObject.transform.SetParent(parent, false);
			gameObject2.transform.SetParent(parent2, false);
			restrictItems.Add(new HumanAttachItem("RestrictRope_L", gameObject));
			restrictItems.Add(new HumanAttachItem("RestrictRope_R", gameObject2));
			for (int j = 0; j < restrictItems.Count; j++)
			{
				AddAttachItem(restrictItems[j]);
			}
		}
	}
}
