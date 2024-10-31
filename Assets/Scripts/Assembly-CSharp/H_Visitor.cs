using System;
using System.Collections.Generic;
using Character;
using H;
using UnityEngine;
using Utility;

public class H_Visitor
{
	private H_Scene h_scene;

	private Human human;

	private Female female;

	private Dictionary<string, H_PoseData> poseDatas;

	private H_Pos h_pos;

	private int posNo;

	private PosYaw lastPosYaw;

	private int updateLookDelay = -1;

	private float voiceTimer;

	private float voiceTimeMin = 10f;

	private float voiceTimeMax = 60f;

	private float expressionTimer;

	private float expressionTimeMin = 5f;

	private float expressionTimeMax = 10f;

	private H_VisitorVoice voice;

	private H_VoiceLog log = new H_VoiceLog();

	private H_VisitorExpression expression;

	public bool LockPos { get; set; }

	public H_Visitor(H_Scene h_scene, HEROINE heroineID, CustomParameter param, RuntimeAnimatorController runtimeAC, H_VisitorVoice visitorVoice, H_VisitorExpression visitorExpression)
	{
		this.h_scene = h_scene;
		female = ResourceUtility.CreateInstance<Female>("FemaleBody");
		female.SetHeroineID(heroineID);
		female.Load(param);
		female.Apply();
		female.body.Anime.runtimeAnimatorController = runtimeAC;
		female.personality.H_In();
		human = female;
		Foot(h_scene.map.data.foot);
		voice = visitorVoice;
		expression = visitorExpression;
		NextVoiceTime();
		LockPos = false;
	}

	public H_Visitor(H_Scene h_scene, MALE_ID maleID, CustomParameter param, RuntimeAnimatorController runtimeAC)
	{
		this.h_scene = h_scene;
		Male male = ResourceUtility.CreateInstance<Male>("MaleBody");
		male.SetMaleID(maleID);
		male.Load(param);
		male.Apply();
		male.body.Anime.runtimeAnimatorController = runtimeAC;
		human = male;
		Foot(h_scene.map.data.foot);
		voice = null;
		expression = null;
		LockPos = false;
	}

	public void LoadAC(SEX sex)
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", "h/h_visitor");
		RuntimeAnimatorController runtimeAnimatorController = null;
		runtimeAnimatorController = ((sex != 0) ? assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_Visitor_M") : assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_Visitor"));
		human.body.Anime.runtimeAnimatorController = runtimeAnimatorController;
		human.body.Anime.speed = 1f;
		assetBundleController.Close();
	}

	public void SwapHuman(Human newHuman, AssetBundleController abc, H_Members members, bool setOnly)
	{
		human = newHuman;
		female = human as Female;
		if (!setOnly)
		{
			human.GagShow = true;
			LoadAC(human.sex);
			voice.Voice(female, log, abc, H_VisitorVoice.TYPE.BREATH, members);
			Expression(H_VisitorExpression.TYPE.BREATH);
		}
	}

	public void Update()
	{
		int count = h_scene.mainMembers.GetFemales().Count;
		if (1 == 0 || count > 1)
		{
			return;
		}
		if (updateLookDelay >= 0)
		{
			if (updateLookDelay == 0)
			{
				Transform tgt = h_scene.mainMembers.CaclVisitorLookPos();
				human.ChangeNeckLook(LookAtRotator.TYPE.TARGET, tgt, false);
				human.ChangeEyeLook(LookAtRotator.TYPE.TARGET, tgt, false);
				updateLookDelay = -1;
			}
			updateLookDelay--;
		}
		human.body.Anime.SetFloat("Height", human.customParam.body.GetHeight());
		if (female != null)
		{
			voiceTimer -= Time.deltaTime;
			if (voiceTimer <= 0f)
			{
				h_scene.VisitorVoiceExpression(H_VisitorVoice.TYPE.GENERAL);
			}
			log.Update();
			if (!female.IsVoicePlaying())
			{
				h_scene.VisitorVoiceExpression(H_VisitorVoice.TYPE.BREATH);
			}
			if (expressionTimer <= 0f)
			{
				Expression(H_VisitorExpression.TYPE.BREATH);
			}
			else
			{
				expressionTimer -= Time.deltaTime;
			}
		}
	}

	public bool VoiceExpression(H_VisitorVoice.TYPE type, AssetBundleController abc, H_Members members)
	{
		if (voice == null)
		{
			return false;
		}
		H_VisitorVoice.Data data = voice.Voice(female, log, abc, type, members);
		if (data != null)
		{
			expression.ChangeExpression(female, data.file);
			NextVoiceTime();
			NextExpressionTime();
			return true;
		}
		return false;
	}

	public void Expression(H_VisitorExpression.TYPE type)
	{
		expression.ChangeExpression(female, type);
		NextExpressionTime();
	}

	public void RandomPose(Dictionary<string, H_PoseData> poseDatas, H_Pos h_pos)
	{
		this.poseDatas = poseDatas;
		this.h_pos = h_pos;
		if (h_pos.watchPos.Count != 0)
		{
			posNo = UnityEngine.Random.Range(0, h_pos.watchPos.Count);
			ChangePose();
		}
	}

	public void SetPose(Dictionary<string, H_PoseData> poseDatas, H_Pos h_pos, int no)
	{
		this.poseDatas = poseDatas;
		this.h_pos = h_pos;
		if (h_pos.watchPos.Count != 0)
		{
			posNo = no % h_pos.watchPos.Count;
			ChangePose();
		}
	}

	public void ChangePose()
	{
		if (female != null)
		{
			ChangePose_F();
		}
		else
		{
			ChangePose_M();
		}
	}

	private void ChangePose_F()
	{
		if (!LockPos)
		{
			WatchPos watchPos = h_pos.watchPos[posNo];
			string[] array = new string[3] { "Ritsuko", "Akiko", "Yukiko" };
			string text = array[(int)female.HeroineID];
			string[] array2 = new string[3] { "Stand", "Floor", "Chair" };
			string text2 = ((!GlobalData.PlayData.IsHeroineFloped(female.HeroineID)) ? "Resist" : "Flop");
			string key = text + text2 + array2[(int)watchPos.type];
			SetPos(watchPos);
			H_PoseData data = poseDatas[key];
			Pose(data);
		}
		updateLookDelay = 1;
	}

	private void ChangePose_M()
	{
		if (!LockPos)
		{
			WatchPos pos = h_pos.watchPos[posNo];
			SetPos(pos);
		}
		updateLookDelay = 1;
	}

	private void SetPos(WatchPos watchPos)
	{
		lastPosYaw = watchPos.pos;
		human.transform.position = watchPos.pos.pos;
		human.transform.rotation = Quaternion.Euler(0f, watchPos.pos.yaw, 0f);
	}

	public void NextPos()
	{
		if (h_pos.watchPos.Count != 0)
		{
			posNo = (posNo + 1) % h_pos.watchPos.Count;
			WatchPos watchPos = h_pos.watchPos[posNo];
			ChangePose();
		}
	}

	public void PrevPos()
	{
		if (h_pos.watchPos.Count != 0)
		{
			posNo = (h_pos.watchPos.Count + posNo - 1) % h_pos.watchPos.Count;
			WatchPos watchPos = h_pos.watchPos[posNo];
			ChangePose();
		}
	}

	private void Pose(H_PoseData data)
	{
		PlayAnime(data.id, 0f);
		SetIK(human, data.ikDatas);
	}

	private void SetIK(Human human, IK_DataList dataList)
	{
		human.ik.ClearIK();
		if (dataList == null)
		{
			return;
		}
		foreach (IK_Data ikData in dataList.ikDatas)
		{
			SetIK(ikData, 2f);
		}
	}

	private void SetIK(IK_Data data, float speed)
	{
		bool flag = false;
		Human humanFromString = GetHumanFromString(data.ikChara);
		Transform iKTargetFromString = GetIKTargetFromString(data.targetChara);
		if (humanFromString == null)
		{
			flag = true;
		}
		if (iKTargetFromString == null)
		{
			flag = true;
		}
		if (!flag)
		{
			Transform transform = Transform_Utility.FindTransform(iKTargetFromString, data.targetPart);
			if (transform != null)
			{
				humanFromString.ik.SetIK(data.ikPart, transform, speed);
			}
			else
			{
				Debug.LogError("不明なターゲット:" + data.targetPart);
			}
		}
	}

	private Human GetHumanFromString(string str)
	{
		if (human.sex == SEX.FEMALE && str == "f0")
		{
			return human;
		}
		if (human.sex == SEX.MALE && str == "m0")
		{
			return human;
		}
		return null;
	}

	private Transform GetIKTargetFromString(string str)
	{
		if (human.sex == SEX.FEMALE && str == "f0")
		{
			return human.body.Anime.transform;
		}
		if (human.sex == SEX.MALE && str == "m0")
		{
			return human.body.Anime.transform;
		}
		return null;
	}

	public void PlayAnime(string name, float transitionDuration)
	{
		Animator anime = human.body.Anime;
		if (transitionDuration <= 0f)
		{
			anime.Play(name, 0);
		}
		else
		{
			anime.CrossFadeInFixedTime(name, transitionDuration, 0);
		}
	}

	public void Foot(MapData.FOOT foot)
	{
		human.Foot(foot);
	}

	public Human GetHuman()
	{
		return human;
	}

	public Female GetFemale()
	{
		return female;
	}

	public bool IsFemale()
	{
		return female != null;
	}

	public H_Pos GetH_Pos()
	{
		return h_pos;
	}

	public PosYaw GetLastPosYaw()
	{
		return lastPosYaw;
	}

	private void NextVoiceTime()
	{
		voiceTimer = UnityEngine.Random.Range(voiceTimeMin, voiceTimeMax);
	}

	private void NextExpressionTime()
	{
		expressionTimer = UnityEngine.Random.Range(expressionTimeMin, expressionTimeMax);
	}
}
