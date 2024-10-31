using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Utility;

namespace H
{
	public class H_Members
    {
		private H_StateManager stateMgr;

		public H_InputBase input;

		public H_Parameter param;

		private List<Male> males = new List<Male>();

		private List<Female> females = new List<Female>();

		private List<Human> humans = new List<Human>();

		private Animator mapIK;

		private List<H_VoiceLog> voiceLogs = new List<H_VoiceLog>();

		private PosYaw posFloor = new PosYaw();

		private PosYaw posWall = new PosYaw();

		private PosYaw posChair = new PosYaw();

		private PosYaw posSpecial = new PosYaw();

		private PosYaw posFiveR = new PosYaw();

		private PosYaw posFiveF = new PosYaw();

		private PosYaw posFiveW = new PosYaw();

		public int floorPosNo;

		public int wallPosNo;

		public int chairPosNo;

		public int specialPosNo;

		public int fiveR_PosNo;

		public int fiveF_PosNo;

		public int fiveW_PosNo;

		private IK_DataList ikDataList;

		private H_Item itemObj;

		private PosYaw lastPosYaw;

		public H_Scene h_scene { get; private set; }

		public H_StateManager StateMgr
		{
			get
			{
				return stateMgr;
			}
		}

		public float FemaleGageVal { get; set; }

		public float MaleGageVal { get; set; }

		public H_StyleData StyleData { get; private set; }

		public H_PoseData PoseData { get; private set; }

		public Transform Transform { get; private set; }

		public bool EnableTinIK { get; set; }

		public GameObject visitorLookPos { get; private set; }

		public H_Members(H_Scene h_scene)
		{
			GameObject gameObject = new GameObject("H_Pos");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			visitorLookPos = new GameObject("VisitorLoopPos");
			visitorLookPos.transform.SetParent(gameObject.transform, false);
			visitorLookPos.transform.localPosition = new Vector3(0f, 1f, 0f);
			this.h_scene = h_scene;
			Transform = gameObject.transform;
			stateMgr = new H_StateManager(this);
			param = new H_Parameter();
			param.mapName = h_scene.map.name;
			mapIK = h_scene.CreateMapIK();
			mapIK.transform.SetParent(gameObject.transform, false);
		}

		public void Update(bool enableProcess)
		{
			SetAnimeParam();
			stateMgr.Main();
			for (int i = 0; i < voiceLogs.Count; i++)
			{
				voiceLogs[i].Update();
			}
			Update_EyeLook();
			Update_Female();
			Update_Pos();
			for (int j = 0; j < males.Count; j++)
			{
				males[j].ik.TinEnable = EnableTinIK;
				males[j].ik.MouthEnable = EnableTinIK;
			}
		}

		public void LateUpdate()
		{
			for (int i = 0; i < females.Count; i++)
			{
				females[i].ik.FBIK.UpdateSolverExternal();
			}
			for (int j = 0; j < males.Count; j++)
			{
				males[j].ik.FBIK.UpdateSolverExternal();
				males[j].ik.CalcMouth();
				males[j].ik.CalcTin();
			}
			for (int k = 0; k < females.Count; k++)
			{
				females[k].ik.FBIK.UpdateSolverExternal();
			}
		}

		private void Update_Pos()
		{
			if (StyleData != null)
			{
				if (StyleData.position == H_StyleData.POSITION.FIVE_RESIST)
				{
					posFiveR.pos = Transform.position;
					posFiveR.yaw = Transform.rotation.eulerAngles.y;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_FLOP)
				{
					posFiveF.pos = Transform.position;
					posFiveF.yaw = Transform.rotation.eulerAngles.y;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_WEAKNESS)
				{
					posFiveW.pos = Transform.position;
					posFiveW.yaw = Transform.rotation.eulerAngles.y;
				}
				else if (StyleData.position == H_StyleData.POSITION.SPECIAL)
				{
					posSpecial.pos = Transform.position;
					posSpecial.yaw = Transform.rotation.eulerAngles.y;
				}
				else if (StyleData.position == H_StyleData.POSITION.WALL)
				{
					posWall.pos = Transform.position;
					posWall.yaw = Transform.rotation.eulerAngles.y;
				}
				else if (StyleData.position == H_StyleData.POSITION.CHAIR)
				{
					posChair.pos = Transform.position;
					posChair.yaw = Transform.rotation.eulerAngles.y;
				}
				else
				{
					posFloor.pos = Transform.position;
					posFloor.yaw = Transform.rotation.eulerAngles.y;
				}
			}
		}

		private void Update_EyeLook()
		{
			Vector3 facePos = males[0].FacePos;
			Vector3 facePos2 = females[0].FacePos;
			Vector3 position = h_scene.IllCam.transform.position;
			bool flag = males[0].MaleShow >= MALE_SHOW.CLOTHING && males[0].MaleShow <= MALE_SHOW.ONECOLOR;
			for (int i = 0; i < males.Count; i++)
			{
				males[i].EyeLook.TargetPos = facePos2;
			}
			Vector3 targetPos = ((!flag) ? position : facePos);
			for (int j = 0; j < females.Count; j++)
			{
				females[j].EyeLook.TargetPos = targetPos;
			}
		}

		private void Update_Female()
		{
			for (int i = 0; i < females.Count; i++)
			{
				Update_Female(females[i]);
			}
		}

		private void Update_Female(Female female)
		{
			Update_FemaleFlush(female);
			Update_FemaleTear(female);
			Update_FemaleSmooth(female);
			Update_FemaleNip(female);
		}

		private void Update_FemaleFlush(Female female)
		{
			float value = female.FlushRate;
			int num = female.personality.xtc_count_vagina + female.personality.xtc_count_anal;
			if (female.personality.weakness || female.personality.ahe)
			{
				value = 1f;
			}
			else if (female.HeroineID == HEROINE.RITSUKO || female.HeroineID == HEROINE.AKIKO)
			{
				if (!female.IsFloped())
				{
					if (num == 1)
					{
						value = 0.25f;
					}
					else if (num >= 2)
					{
						value = 0.5f;
					}
				}
				else if (num > 0)
				{
					value = 0.5f;
				}
			}
			else if (female.HeroineID == HEROINE.YUKIKO)
			{
				if (!female.IsFloped())
				{
					if (num == 1)
					{
						value = 0.25f;
					}
					else if (num >= 2)
					{
						value = 0.5f;
					}
				}
				else if (num > 0)
				{
					value = 0.5f;
				}
			}
			if (param.style != null && param.style.type != H_StyleData.TYPE.SERVICE)
			{
				value = ((!param.hit) ? 0f : 0.3f);
			}
			value = Mathf.Clamp01(value);
			if (value != female.FlushRate)
			{
				female.SetFlush(value);
			}
		}

		private void Update_FemaleTear(Female female)
		{
			float num = 0f;
			int num2 = female.personality.eja_count_vagina + female.personality.eja_count_anal;
			if (female.HeroineID == HEROINE.RITSUKO)
			{
				if (female.personality.state == Personality.STATE.FIRST)
				{
					num = Mathf.Max(num, 0.33f);
				}
				if (female.personality.IsLostVaginaVirgin() || female.personality.IsLostAnalVirgin())
				{
					num = Mathf.Max(num, 1f);
				}
				if (!female.IsFloped())
				{
					if (num2 == 2)
					{
						num = Mathf.Max(num, 0.33f);
					}
					else if (num2 >= 3)
					{
						num = Mathf.Max(num, 0.66f);
					}
				}
			}
			if (female.HeroineID == HEROINE.AKIKO)
			{
				if (female.personality.IsLostVaginaVirgin() || female.personality.IsLostAnalVirgin())
				{
					num = Mathf.Max(num, 1f);
				}
				if (!female.IsFloped())
				{
					if (num2 == 2)
					{
						num = Mathf.Max(num, 0.33f);
					}
					else if (num2 >= 3)
					{
						num = Mathf.Max(num, 0.66f);
					}
				}
			}
			if (num > female.TearsRate)
			{
				female.SetTear(num);
			}
		}

		private void Update_FemaleSmooth(Female female)
		{
			float num = 0f;
			int num2 = female.personality.xtc_count_vagina + female.personality.xtc_count_anal;
			if (female.HeroineID == HEROINE.RITSUKO || female.HeroineID == HEROINE.AKIKO)
			{
				if (female.personality.state == Personality.STATE.FIRST)
				{
					num = 0f;
				}
				else if (!female.IsFloped())
				{
					num = ((num2 <= 0) ? 0f : 0.1f);
				}
				else if (num2 > 0)
				{
					num = ((num2 != 1) ? 0.2f : 0.1f);
				}
			}
			else if (female.HeroineID == HEROINE.YUKIKO)
			{
				if (female.personality.state == Personality.STATE.FIRST)
				{
					num = ((num2 <= 0) ? 0f : 0.1f);
				}
				else if (!female.IsFloped())
				{
					num = ((num2 <= 0) ? 0f : 0.1f);
				}
				else if (num2 > 0)
				{
					num = ((num2 != 1) ? 0.2f : 0.1f);
				}
			}
			if (num > female.SkinSmoothAdd)
			{
				female.SetSkinSmoothAdd(num);
			}
		}

		private void Update_FemaleNip(Female female)
		{
			float num = 0f;
			int num2 = female.personality.xtc_count_vagina + female.personality.xtc_count_anal;
			if (female.HeroineID == HEROINE.RITSUKO)
			{
				if (female.personality.state == Personality.STATE.FIRST)
				{
					num = 0f;
				}
				else if (num2 == 1)
				{
					num = 0.1f;
				}
				else if (num2 >= 2)
				{
					num = 0.2f;
				}
			}
			else if (female.HeroineID == HEROINE.AKIKO)
			{
				if (num2 == 1)
				{
					num = 0.1f;
				}
				else if (num2 >= 2)
				{
					num = 0.2f;
				}
			}
			else if (female.HeroineID == HEROINE.YUKIKO)
			{
				if (female.personality.state == Personality.STATE.FIRST)
				{
					num = 0f;
				}
				else if (num2 == 1)
				{
					num = 0.2f;
				}
				else if (num2 >= 2)
				{
					num = 0.4f;
				}
			}
			if (num > female.NipAdd)
			{
				female.SetNipAdd(num);
			}
		}

		public Female AddFemale(HEROINE heroineID)
		{
			CustomParameter heroineCustomParam = GlobalData.PlayData.GetHeroineCustomParam(heroineID);
			Female female = ResourceUtility.CreateInstance<Female>("FemaleBody");
			female.SetHeroineID(heroineID);
			female.Load(heroineCustomParam);
			female.Apply();
			AddChara(female);
			female.personality.H_In();
			Personality.STATE state = female.personality.state;
			if (state == Personality.STATE.LAST_EVENT_SISTERS || state == Personality.STATE.LAST_EVENT_YUKIKO_2)
			{
				param.maleHateEvent = true;
			}
			return female;
		}

		public Female AddFemaleFromVisitor(H_Visitor femaleVisitor)
		{
			Female female = femaleVisitor.GetFemale();
			femaleVisitor.LockPos = true;
			AddChara(female);
			Personality.STATE state = female.personality.state;
			if (state == Personality.STATE.LAST_EVENT_SISTERS || state == Personality.STATE.LAST_EVENT_YUKIKO_2)
			{
				param.maleHateEvent = true;
			}
			return female;
		}

		public Male AddMale(MALE_ID maleID)
		{
			CustomParameter maleCustomParam = GlobalData.PlayData.GetMaleCustomParam(maleID);
			Male male = ResourceUtility.CreateInstance<Male>("MaleBody");
			male.SetMaleID(maleID);
			male.Load(maleCustomParam);
			male.Apply();
			this.AddChara(male);
			return male;
		}

		private void AddChara(Female female)
		{
			females.Add(female);
			humans.Add(female);
			female.transform.SetParent(Transform, false);
			female.transform.localPosition = Vector3.zero;
			female.transform.localRotation = Quaternion.identity;
			female.ik.ClearIK();
			female.ChangeNeckLook(LookAtRotator.TYPE.NO, null, false);
			female.ChangeEyeLook(LookAtRotator.TYPE.TARGET, female.FacePos, false);
			voiceLogs.Add(new H_VoiceLog());
		}

		private void AddChara(Male male)
		{
			males.Add(male);
			humans.Add(male);
			male.transform.SetParent(Transform, false);
			male.transform.localPosition = Vector3.zero;
			male.transform.localRotation = Quaternion.identity;
			male.ChangeEyeLook(LookAtRotator.TYPE.TARGET, male.FacePos, false);
		}

		private void DelChara(Female female)
		{
			females.Remove(female);
			humans.Remove(female);
            UnityEngine.Object.Destroy(female.gameObject);
		}

		private void DelChara(Male male)
		{
			males.Remove(male);
			humans.Remove(male);
            UnityEngine.Object.Destroy(male.gameObject);
		}

		private void RemoveChara(Female female)
		{
			if (h_scene.visitor != null && h_scene.visitor.GetFemale() == female)
			{
				h_scene.visitor.LockPos = false;
				h_scene.visitor.LoadAC(SEX.FEMALE);
				h_scene.visitor.GetHuman().VoiceShutUp();
				h_scene.visitor.Expression(H_VisitorExpression.TYPE.BREATH);
			}
			female.transform.SetParent(null, false);
			female.ik.ClearIK();
			females.Remove(female);
			humans.Remove(female);
		}

		private void SetInput(H_InputBase input)
		{
			this.input = input;
		}

		public void PlayAnime(string name, float transitionDuration)
		{
			foreach (Human human in humans)
			{
				PlayAnime(human.body.Anime, name, transitionDuration);
			}
			if (mapIK.runtimeAnimatorController != null)
			{
				PlayAnime(mapIK, name, transitionDuration);
			}
			if (itemObj != null)
			{
				PlayAnime(itemObj.animator, name, transitionDuration);
			}
			if (StyleData != null)
			{
				SetStyleIK(name);
			}
		}

		private static void PlayAnime(Animator anm, string name, float transitionDuration)
		{
			for (int i = 0; i < anm.layerCount; i++)
			{
				if (transitionDuration <= 0f)
				{
					anm.Play(name, i);
				}
				else
				{
					anm.CrossFadeInFixedTime(name, transitionDuration, i);
				}
			}
		}

		public void SetLoopPose(float pose)
		{
			foreach (Human human in humans)
			{
				human.body.Anime.SetFloat(H_Scene.AnmParamID_Pose, pose);
			}
			if (mapIK.runtimeAnimatorController != null)
			{
				mapIK.SetFloat(H_Scene.AnmParamID_Pose, pose);
			}
		}

		public void SetLoopStroke(float stroke)
		{
			foreach (Human human in humans)
			{
				human.body.Anime.SetFloat(H_Scene.AnmParamID_Stroke, stroke);
			}
			if (mapIK.runtimeAnimatorController != null)
			{
				mapIK.SetFloat(H_Scene.AnmParamID_Stroke, stroke);
			}
		}

		public void SetAnimeParam()
		{
			bool flag = StyleData != null && (StyleData.detailFlag & H_StyleData.DetailMasking_Bust) != 0;
			for (int i = 0; i < females.Count; i++)
			{
				Female female = females[i];
				float height = female.customParam.body.GetHeight();
				float bustSize = female.customParam.body.GetBustSize();
				if (female.gameObject.activeInHierarchy)
				{
					female.body.Anime.SetFloat(H_Scene.AnmParamID_Height, height);
					if (flag)
					{
						female.body.Anime.SetFloat(H_Scene.AnmParamID_Bust, bustSize);
					}
				}
			}
			float height2 = females[0].customParam.body.GetHeight();
			float bustSize2 = females[0].customParam.body.GetBustSize();
			for (int j = 0; j < males.Count; j++)
			{
				Male male = males[j];
				if (male.gameObject.activeInHierarchy)
				{
					male.body.Anime.SetFloat(H_Scene.AnmParamID_Height, height2);
					if (flag)
					{
						male.body.Anime.SetFloat(H_Scene.AnmParamID_Bust, bustSize2);
					}
				}
			}
			if (mapIK.runtimeAnimatorController != null)
			{
				mapIK.SetFloat(H_Scene.AnmParamID_Height, height2);
			}
		}

		public void SetSpeed(float val)
		{
			if (val < 0.75f)
			{
				param.speed = H_SPEED.SLOW;
			}
			else if (val > 2f)
			{
				param.speed = H_SPEED.FAST;
			}
			else
			{
				param.speed = H_SPEED.NORMAL;
			}
			foreach (Human human in humans)
			{
				human.body.Anime.speed = val;
			}
			if (mapIK.runtimeAnimatorController != null)
			{
				mapIK.speed = val;
			}
		}

		public bool CheckEndAnime()
		{
			return GetAnimeNormalizedTime() >= 1f;
		}

		public bool CheckEndAnime(string name)
		{
			return CheckAnimeName(name) && GetAnimeNormalizedTime() >= 1f;
		}

		public bool CheckAnimeName(string name)
		{
			return humans.Count > 0 && humans[0].body.Anime.GetCurrentAnimatorStateInfo(0).IsName(name);
		}

		public float GetAnimeNormalizedTime()
		{
			return (humans.Count <= 0) ? 0f : humans[0].body.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}

		public bool CheckEndVoice()
		{
			foreach (Female female in females)
			{
				if (female.IsVoicePlaying())
				{
					return false;
				}
			}
			return true;
		}

		public void OnInput(H_INPUT input)
		{
			stateMgr.OnInput(input);
		}

		public bool ChangeMap(MapData mapData, bool changeTimeOnly)
		{
			bool result = false;
			H_PosSet(ref posFloor, ref floorPosNo, mapData.h_pos.floor);
			H_PosSet(ref posWall, ref wallPosNo, mapData.h_pos.wall);
			H_PosSet(ref posChair, ref chairPosNo, mapData.h_pos.chair);
			H_PosSet(ref posSpecial, ref specialPosNo, mapData.h_pos.special);
			H_PosSet(ref posFiveR, ref fiveR_PosNo, mapData.h_pos.five_Resist);
			H_PosSet(ref posFiveF, ref fiveF_PosNo, mapData.h_pos.five_Flop);
			H_PosSet(ref posFiveW, ref fiveW_PosNo, mapData.h_pos.five_Weakness);
			if (StyleData != null && !h_scene.CheckEnableStyle(StyleData.id))
			{
				string id = AlternativeStyle(StyleData.type, StyleData.state, StyleData.detailFlag);
				h_scene.ChangeStyle(id);
				result = true;
			}
			if (!changeTimeOnly)
			{
				SetDataPos();
			}
			Foot(mapData);
			return result;
		}

		private static string AlternativeStyle(H_StyleData.TYPE type, H_StyleData.STATE state, int detail)
		{
			string result = string.Empty;
			bool flag = (detail & 0x20) != 0;
			string[] array = new string[4] { "00", "00", "01", "02" };
			switch (type)
			{
			case H_StyleData.TYPE.INSERT:
				result = "HS";
				result = (flag ? (result + "_01_" + array[(int)state] + "_00") : (result + "_00_" + array[(int)state] + "_00"));
				break;
			case H_StyleData.TYPE.SERVICE:
				result = "HH_01_" + array[(int)state] + "_00";
				break;
			case H_StyleData.TYPE.PETTING:
				result = "HA";
				result = (flag ? (result + "_01_" + array[(int)state] + "_00") : (result + "_00_" + array[(int)state] + "_00"));
				break;
			}
			return result;
		}

		private void Foot(MapData mapData)
		{
			for (int i = 0; i < humans.Count; i++)
			{
				humans[i].Foot(mapData.foot);
			}
		}

		private static void H_PosSet(ref PosYaw posYaw, ref int no, List<H_Pos> list)
		{
			if (list.Count > 0)
			{
				no %= list.Count;
				posYaw.Copy(list[no].pos);
			}
			else
			{
				posYaw.Zero();
			}
		}

		public void GetNowDataPosNo(out int no, out H_Pos h_pos)
		{
			no = -1;
			h_pos = null;
			if (StyleData != null)
			{
				if (StyleData.position == H_StyleData.POSITION.FIVE_RESIST)
				{
					h_pos = NowPos(ref fiveR_PosNo, h_scene.map.data.h_pos.five_Resist);
					no = fiveR_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_FLOP)
				{
					h_pos = NowPos(ref fiveF_PosNo, h_scene.map.data.h_pos.five_Flop);
					no = fiveF_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_WEAKNESS)
				{
					h_pos = NowPos(ref fiveW_PosNo, h_scene.map.data.h_pos.five_Weakness);
					no = fiveW_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.SPECIAL)
				{
					h_pos = NowPos(ref specialPosNo, h_scene.map.data.h_pos.special);
					no = specialPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.WALL)
				{
					h_pos = NowPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					no = wallPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.CHAIR)
				{
					h_pos = NowPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					no = chairPosNo;
				}
				else
				{
					h_pos = NowPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					no = floorPosNo;
				}
			}
			else if (PoseData != null)
			{
				if (PoseData.position == H_PoseData.POSITION.WALL)
				{
					h_pos = NowPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					no = wallPosNo;
				}
				else if (PoseData.position == H_PoseData.POSITION.CHAIR)
				{
					h_pos = NowPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					no = chairPosNo;
				}
				else
				{
					h_pos = NowPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					no = floorPosNo;
				}
			}
		}

		public H_Pos SetDataPos()
		{
			H_Pos h_Pos = null;
			if (StyleData != null)
			{
				int num = -1;
				if (StyleData.position == H_StyleData.POSITION.FIVE_RESIST)
				{
					h_Pos = NowPos(ref fiveR_PosNo, h_scene.map.data.h_pos.five_Resist);
					num = fiveR_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_FLOP)
				{
					h_Pos = NowPos(ref fiveF_PosNo, h_scene.map.data.h_pos.five_Flop);
					num = fiveF_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_WEAKNESS)
				{
					h_Pos = NowPos(ref fiveW_PosNo, h_scene.map.data.h_pos.five_Weakness);
					num = fiveW_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.SPECIAL)
				{
					h_Pos = NowPos(ref specialPosNo, h_scene.map.data.h_pos.special);
					num = specialPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.WALL)
				{
					h_Pos = NowPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					num = wallPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.CHAIR)
				{
					h_Pos = NowPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					num = chairPosNo;
				}
				else
				{
					h_Pos = NowPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					num = floorPosNo;
				}
				h_scene.SetH_Light(num);
			}
			else if (PoseData != null)
			{
				int num2 = -1;
				if (PoseData.position == H_PoseData.POSITION.WALL)
				{
					h_Pos = NowPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					num2 = wallPosNo;
				}
				else if (PoseData.position == H_PoseData.POSITION.CHAIR)
				{
					h_Pos = NowPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					num2 = chairPosNo;
				}
				else
				{
					h_Pos = NowPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					num2 = floorPosNo;
				}
				h_scene.SetH_Light(num2);
			}
			if (h_Pos != null)
			{
				SetH_Pos(h_Pos);
			}
			return h_Pos;
		}

		private void SetH_Pos(H_Pos h_pos)
		{
			lastPosYaw = h_pos.pos;
			Vector3 getPos = Vector3.zero;
			Quaternion getRot = Quaternion.identity;
			h_pos.Get(out getPos, out getRot);
			Transform.position = getPos;
			Transform.rotation = getRot;
		}

		public H_Pos NextPos()
		{
			H_Pos h_Pos = null;
			if (StyleData != null)
			{
				int num = -1;
				if (StyleData.position == H_StyleData.POSITION.FIVE_RESIST)
				{
					h_Pos = NextPos(ref specialPosNo, h_scene.map.data.h_pos.five_Resist);
					num = fiveR_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_FLOP)
				{
					h_Pos = NextPos(ref specialPosNo, h_scene.map.data.h_pos.five_Flop);
					num = fiveF_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_WEAKNESS)
				{
					h_Pos = NextPos(ref specialPosNo, h_scene.map.data.h_pos.five_Weakness);
					num = fiveW_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.SPECIAL)
				{
					h_Pos = NextPos(ref specialPosNo, h_scene.map.data.h_pos.special);
					num = specialPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.WALL)
				{
					h_Pos = NextPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					num = wallPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.CHAIR)
				{
					h_Pos = NextPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					num = chairPosNo;
				}
				else
				{
					h_Pos = NextPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					num = floorPosNo;
				}
				h_scene.SetH_Light(num);
			}
			else
			{
				int num2 = -1;
				if (PoseData.position == H_PoseData.POSITION.WALL)
				{
					h_Pos = NextPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					num2 = wallPosNo;
				}
				else if (PoseData.position == H_PoseData.POSITION.CHAIR)
				{
					h_Pos = NextPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					num2 = chairPosNo;
				}
				else
				{
					h_Pos = NextPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					num2 = floorPosNo;
				}
				h_scene.SetH_Light(num2);
			}
			if (h_Pos != null)
			{
				SetH_Pos(h_Pos);
			}
			return h_Pos;
		}

		public H_Pos PrevPos()
		{
			H_Pos h_Pos = null;
			if (StyleData != null)
			{
				int num = -1;
				if (StyleData.position == H_StyleData.POSITION.FIVE_RESIST)
				{
					h_Pos = PrevPos(ref specialPosNo, h_scene.map.data.h_pos.five_Resist);
					num = fiveR_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_FLOP)
				{
					h_Pos = PrevPos(ref specialPosNo, h_scene.map.data.h_pos.five_Flop);
					num = fiveF_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.FIVE_WEAKNESS)
				{
					h_Pos = PrevPos(ref specialPosNo, h_scene.map.data.h_pos.five_Weakness);
					num = fiveW_PosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.SPECIAL)
				{
					h_Pos = PrevPos(ref specialPosNo, h_scene.map.data.h_pos.special);
					num = specialPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.WALL)
				{
					h_Pos = PrevPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					num = wallPosNo;
				}
				else if (StyleData.position == H_StyleData.POSITION.CHAIR)
				{
					h_Pos = PrevPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					num = chairPosNo;
				}
				else
				{
					h_Pos = PrevPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					num = floorPosNo;
				}
				h_scene.SetH_Light(num);
			}
			else
			{
				int num2 = -1;
				if (PoseData.position == H_PoseData.POSITION.WALL)
				{
					h_Pos = PrevPos(ref wallPosNo, h_scene.map.data.h_pos.wall);
					num2 = wallPosNo;
				}
				else if (PoseData.position == H_PoseData.POSITION.CHAIR)
				{
					h_Pos = PrevPos(ref chairPosNo, h_scene.map.data.h_pos.chair);
					num2 = chairPosNo;
				}
				else
				{
					h_Pos = PrevPos(ref floorPosNo, h_scene.map.data.h_pos.floor);
					num2 = floorPosNo;
				}
				h_scene.SetH_Light(num2);
			}
			if (h_Pos != null)
			{
				SetH_Pos(h_Pos);
			}
			return h_Pos;
		}

		private H_Pos NextPos(ref int no, List<H_Pos> list)
		{
			if (list.Count > 0)
			{
				no = (no + 1) % list.Count;
				return list[no];
			}
			return null;
		}

		private H_Pos PrevPos(ref int no, List<H_Pos> list)
		{
			if (list.Count > 0)
			{
				no = (list.Count + no - 1) % list.Count;
				return list[no];
			}
			return null;
		}

		private H_Pos NowPos(ref int no, List<H_Pos> list)
		{
			if (list.Count > 0)
			{
				no %= list.Count;
				return list[no];
			}
			return null;
		}

		public void ChangeStyle(H_StyleData data)
		{
			PoseData = null;
			if (itemObj != null)
			{
               UnityEngine.Object.Destroy(itemObj.gameObject);
				itemObj = null;
			}
			if (((uint)data.detailFlag & 0x400u) != 0)
			{
				if (((uint)data.detailFlag & 0x20u) != 0)
				{
					GameObject gameObject = AssetBundleLoader.LoadAndInstantiate<GameObject>(GlobalData.assetBundlePath, "h/h_item", "p_item_analvibe");
					Transform target = Transform_Utility.FindTransform(females[0].body.Anime.transform, "k_f_ana_00");
					itemObj = gameObject.GetComponent<H_Item>();
					itemObj.SetTarget(target);
				}
				else
				{
					GameObject gameObject2 = AssetBundleLoader.LoadAndInstantiate<GameObject>(GlobalData.assetBundlePath, "h/h_item", "p_item_vibe_01");
					Transform target2 = Transform_Utility.FindTransform(females[0].body.Anime.transform, "k_f_kokan_00");
					itemObj = gameObject2.GetComponent<H_Item>();
					itemObj.SetTarget(target2);
				}
			}
			bool set = (data.detailFlag & 0x800) != 0;
			for (int i = 0; i < males.Count; i++)
			{
				males[i].ChangeRestrict(set);
			}
			H_StyleData.MEMBER member = data.member;
			MemberAdjust(data.member);
			string[] array = new string[4] { "M", "N", "O", "P" };
			string[] array2 = new string[2] { "F", "G" };
			AssetBundleController assetBundleController = new AssetBundleController(false);
			assetBundleController.OpenFromFile(GlobalData.assetBundlePath, data.assetBundle);
			for (int j = 0; j < males.Count; j++)
			{
				males[j].body.Anime.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_" + data.id + "_" + array[j]);
			}
			for (int k = 0; k < females.Count; k++)
			{
				females[k].body.Anime.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_" + data.id + "_" + array2[k]);
			}
			mapIK.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_" + data.id + "_H");
			if (itemObj != null)
			{
				itemObj.animator.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_" + data.id + "_I");
			}
			assetBundleController.Close();
			Resources.UnloadUnusedAssets();
			bool flag = StyleData != null && StyleData.position == data.position;
			StyleData = data;
			if (!flag)
			{
				H_Pos h_pos = SetDataPos();
				h_scene.VisitorPos(h_pos);
				h_scene.CharaMove.SetDef(Transform.position, Transform.rotation);
			}
			if (StyleData.hasLight)
			{
				h_scene.SetLightDir(StyleData.lightEuler);
			}
			param.mouth = H_MOUTH.FREE;
			param.style = data;
			param.continuanceXTC_F = 0;
			for (int l = 0; l < females.Count; l++)
			{
				bool flag2 = true;
				if (data.type != H_StyleData.TYPE.PETTING)
				{
					flag2 = (data.detailFlag & H_StyleData.DetailMasking_UseMouth) == 0;
				}
				females[l].ChangeShowGag(flag2);
			}
			if (females[0].personality.state == Personality.STATE.FIRST)
			{
				VoiceExpression(H_Voice.TYPE.BREATH);
			}
			else
			{
				int num = UnityEngine.Random.Range(0, females.Count);
				for (int m = 0; m < females.Count; m++)
				{
					H_Voice.TYPE voice = ((m == num) ? H_Voice.TYPE.CHANGE_STYLE : H_Voice.TYPE.BREATH);
					VoiceExpression(m, voice);
				}
			}
			Wear();
			bool flag3 = (StyleData.detailFlag & 0x180) != 0;
			for (int n = 0; n < females.Count; n++)
			{
				females[n].body.bustDynamicBone_L.enabled = !flag3;
				females[n].body.bustDynamicBone_R.enabled = !flag3;
			}
		}

		private void MemberAdjust(H_StyleData.MEMBER memberType)
		{
			int[] array = new int[5] { 1, 2, 3, 4, 1 };
			int[] array2 = new int[5] { 1, 1, 1, 1, 2 };
			int num = array[(int)memberType];
			int num2 = array2[(int)memberType];
			while (males.Count > num)
			{
				int index = males.Count - 1;
				DelChara(males[index]);
			}
			while (females.Count > num2)
			{
				int index2 = females.Count - 1;
				RemoveChara(females[index2]);
			}
			while (males.Count < num)
			{
				MALE_ID maleID = (MALE_ID)((males.Count - 1) % 3 + 2);
				Male male = AddMale(maleID);
				male.Foot(h_scene.map.data.foot);
			}
			while (females.Count < num2 && GlobalData.PlayData.lastSelectVisitor >= VISITOR.RITSUKO && GlobalData.PlayData.lastSelectVisitor < (VISITOR)3)
			{
				AddFemaleFromVisitor(h_scene.visitor);
			}
		}

		public void StartPose(H_PoseData data)
		{
			PoseData = data;
			MemberAdjust(data.members);
			AssetBundleController assetBundleController = new AssetBundleController(false);
			assetBundleController.OpenFromFile(GlobalData.assetBundlePath, "h/h_in");
			for (int i = 0; i < males.Count; i++)
			{
				males[i].body.Anime.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_IN_M");
			}
			for (int j = 0; j < females.Count; j++)
			{
				females[j].body.Anime.runtimeAnimatorController = assetBundleController.LoadAsset<RuntimeAnimatorController>("AC_IN_F");
			}
			assetBundleController.Close();
			foreach (Male male in males)
			{
				male.gameObject.SetActive(PoseData.showMale);
			}
			string id = PoseData.id;
			if (PoseData.members == H_StyleData.MEMBER.M1F2)
			{
				for (int k = 0; k < males.Count; k++)
				{
					PlayAnime(males[k].body.Anime, id, 0f);
				}
				for (int l = 0; l < females.Count; l++)
				{
					PlayAnime(females[l].body.Anime, id + "_" + l.ToString("00"), 0f);
				}
			}
			else
			{
				PlayAnime(id, 0f);
			}
			SetIK(PoseData.ikDatas);
			StateMgr.ChangeState(H_STATE.START, null);
		}

		public void SetStyleIK(string animeName)
		{
			IK_DataList iK_DataList = null;
			iK_DataList = ((!StyleData.alternativeIK.ContainsKey(animeName)) ? StyleData.baseIK : StyleData.alternativeIK[animeName]);
			if (iK_DataList != ikDataList)
			{
				SetIK(iK_DataList);
			}
		}

		public void ClearIK()
		{
			foreach (Human human in humans)
			{
				human.ik.ClearIK();
			}
			ikDataList = null;
		}

		public void SetIK(IK_DataList dataList)
		{
			ClearIK();
			if (dataList == null)
			{
				return;
			}
			ikDataList = dataList;
			foreach (IK_Data ikData in dataList.ikDatas)
			{
				SetIK(ikData, 2f);
			}
		}

		public void SetIK(IK_Data data, float speed)
		{
			bool flag = false;
			Human humanFromString = GetHumanFromString(data.ikChara);
			Transform iKTargetFromString = GetIKTargetFromString(data.targetChara);
			if (humanFromString == null)
			{
				flag = true;
				Debug.LogWarning("不明なikChara:" + data.ikChara);
			}
			if (iKTargetFromString == null)
			{
				flag = true;
				Debug.LogWarning("不明なtargetChara:" + data.targetChara);
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

		public void Expression(H_Expression.TYPE type)
		{
			for (int i = 0; i < females.Count; i++)
			{
				h_scene.Expression(females[i], type, param);
			}
		}

		public void VoiceExpression(H_Voice.TYPE voice)
		{
			for (int i = 0; i < females.Count; i++)
			{
				VoiceExpression(females[i], voiceLogs[i], voice);
			}
		}

		public void MaleExpression(H_Expression_Male.TYPE type)
		{
			for (int i = 0; i < males.Count; i++)
			{
				h_scene.Expression(males[i], type, param);
			}
		}

		public bool VoiceExpression(int femaleNo, H_Voice.TYPE voice)
		{
			return VoiceExpression(females[femaleNo], voiceLogs[femaleNo], voice);
		}

		private bool VoiceExpression(Female female, H_VoiceLog voiceLog, H_Voice.TYPE voice)
		{
			H_Voice.Data data = h_scene.Voice(female, voiceLog, voice, this);
			if (data != null)
			{
				if (data.priority > 0)
				{
					voiceLog.AddPriorityTalk(data.File);
				}
				else
				{
					voiceLog.AddPant(data.File);
				}
				ExpressionFromVoice(female, data);
				return true;
			}
			female.ExpressionPlay(1, "Mouth_Def", 0.2f);
			return false;
		}

		private void ExpressionFromVoice(Female female, H_Voice.Data voice)
		{
			if (h_scene.Expression(female, voice.File, param) == null)
			{
				H_Expression.TYPE type = VoiceToExpressionType(female, voice);
				h_scene.Expression(female, type, param);
			}
		}

		private H_Expression.TYPE VoiceToExpressionType(Female female, H_Voice.Data voice)
		{
			if (voice.type == H_Voice.TYPE.BREATH)
			{
				return H_Expression.TYPE.BREATH;
			}
			if (voice.type == H_Voice.TYPE.START)
			{
				return H_Expression.TYPE.TALK;
			}
			if (voice.type == H_Voice.TYPE.LEAVING)
			{
				return H_Expression.TYPE.TALK;
			}
			if (voice.type == H_Voice.TYPE.LEAVING)
			{
				H_Expression.TYPE result = H_Expression.TYPE.INSERT;
				if (((uint)param.style.detailFlag & 4u) != 0)
				{
					result = H_Expression.TYPE.INSERT_FELLATIO;
				}
				else if (((uint)param.style.detailFlag & 8u) != 0)
				{
					result = H_Expression.TYPE.INSERT_IRRUMATIO;
				}
				return result;
			}
			if (voice.type == H_Voice.TYPE.ACT)
			{
				return VoiceToExpressionType_Act(voice);
			}
			if (voice.type == H_Voice.TYPE.XTC_OMEN_F || voice.type == H_Voice.TYPE.XTC_OMEN_M)
			{
				return VoiceToExpressionType_Spurt(voice);
			}
			if (voice.type == H_Voice.TYPE.XTC_F)
			{
				return H_Expression.TYPE.XTC;
			}
			if (voice.type == H_Voice.TYPE.XTC_M)
			{
				return VoiceToExpressionType_XTC_M(female, voice);
			}
			if (voice.type == H_Voice.TYPE.COUGH)
			{
				return H_Expression.TYPE.COUGH;
			}
			if (voice.type == H_Voice.TYPE.DRINK)
			{
				return H_Expression.TYPE.DRINK;
			}
			if (voice.type == H_Voice.TYPE.VOMIT)
			{
				return H_Expression.TYPE.VOMIT;
			}
			if (voice.type == H_Voice.TYPE.SHOW_ORAL)
			{
				return H_Expression.TYPE.SHOW_ORAL;
			}
			if (voice.type == H_Voice.TYPE.FALL_LIQUID)
			{
				return H_Expression.TYPE.PANT_TALK_LO;
			}
			if (voice.type == H_Voice.TYPE.INCONTINENCE)
			{
				return H_Expression.TYPE.PANT_TALK_LO;
			}
			if (voice.type == H_Voice.TYPE.XTC_AFTER_TALK)
			{
				return H_Expression.TYPE.TALK;
			}
			if (voice.type == H_Voice.TYPE.XTC_AFTER_BREATH)
			{
				if (param.xtcType == XTC_TYPE.EJA_IN)
				{
					return H_Expression.TYPE.INEJA_AFTER_BREATH;
				}
				if (param.xtcType == XTC_TYPE.EJA_OUT)
				{
					return H_Expression.TYPE.OUTEJA_AFTER_BREATH;
				}
				return H_Expression.TYPE.XTC_AFTER_BREATH;
			}
			if (voice.type == H_Voice.TYPE.EXIT)
			{
				return H_Expression.TYPE.TALK;
			}
			if (voice.type == H_Voice.TYPE.ACT_TALK)
			{
				return (param.speed <= H_SPEED.NORMAL) ? H_Expression.TYPE.PANT_TALK_LO : H_Expression.TYPE.PANT_TALK_HI;
			}
			if (voice.type == H_Voice.TYPE.CHANGE_STYLE)
			{
				return H_Expression.TYPE.TALK;
			}
			Debug.LogWarning("表情条件が不確定");
			return H_Expression.TYPE.BREATH;
		}

		private H_Expression.TYPE VoiceToExpressionType_Act(H_Voice.Data voice)
		{
			bool flag = param.speed > H_SPEED.NORMAL;
			bool hit = param.hit;
			if (voice.kind == H_Voice.KIND.TALK)
			{
				return (!flag) ? H_Expression.TYPE.PANT_TALK_LO : H_Expression.TYPE.PANT_TALK_HI;
			}
			if (((uint)param.style.detailFlag & 8u) != 0)
			{
				if (flag)
				{
					return (!hit) ? H_Expression.TYPE.IRRUMATIO_HI_NORMAL : H_Expression.TYPE.IRRUMATIO_HI_HIT;
				}
				return (!hit) ? H_Expression.TYPE.IRRUMATIO_LO_NORMAL : H_Expression.TYPE.IRRUMATIO_LO_HIT;
			}
			if (((uint)param.style.detailFlag & 4u) != 0)
			{
				if (flag)
				{
					return (!hit) ? H_Expression.TYPE.FELLATIO_HI_NORMAL : H_Expression.TYPE.FELLATIO_HI_HIT;
				}
				return (!hit) ? H_Expression.TYPE.FELLATIO_LO_NORMAL : H_Expression.TYPE.FELLATIO_LO_HIT;
			}
			if (param.style.type != H_StyleData.TYPE.PETTING && ((uint)param.style.detailFlag & 2u) != 0)
			{
				if (flag)
				{
					return (!hit) ? H_Expression.TYPE.LICK_HI_NORMAL : H_Expression.TYPE.LICK_HI_HIT;
				}
				return (!hit) ? H_Expression.TYPE.LICK_LO_NORMAL : H_Expression.TYPE.LICK_LO_HIT;
			}
			if (param.style.type == H_StyleData.TYPE.SERVICE)
			{
				return (!flag) ? H_Expression.TYPE.JOB_LO : H_Expression.TYPE.JOB_HI;
			}
			if (flag)
			{
				return (!hit) ? H_Expression.TYPE.PANT_HI_NORMAL : H_Expression.TYPE.PANT_HI_HIT;
			}
			return (!hit) ? H_Expression.TYPE.PANT_LO_NORMAL : H_Expression.TYPE.PANT_LO_HIT;
		}

		public static H_Expression.TYPE VoiceToExpressionType_Act(H_Parameter h_param, H_Voice.KIND voiceKind)
		{
			bool flag = h_param.speed > H_SPEED.NORMAL;
			bool hit = h_param.hit;
			H_StyleData style = h_param.style;
			if (voiceKind == H_Voice.KIND.TALK)
			{
				return (!flag) ? H_Expression.TYPE.PANT_TALK_LO : H_Expression.TYPE.PANT_TALK_HI;
			}
			if (((uint)style.detailFlag & 8u) != 0)
			{
				if (flag)
				{
					return (!hit) ? H_Expression.TYPE.IRRUMATIO_HI_NORMAL : H_Expression.TYPE.IRRUMATIO_HI_HIT;
				}
				return (!hit) ? H_Expression.TYPE.IRRUMATIO_LO_NORMAL : H_Expression.TYPE.IRRUMATIO_LO_HIT;
			}
			if (((uint)style.detailFlag & 4u) != 0)
			{
				if (flag)
				{
					return (!hit) ? H_Expression.TYPE.FELLATIO_HI_NORMAL : H_Expression.TYPE.FELLATIO_HI_HIT;
				}
				return (!hit) ? H_Expression.TYPE.FELLATIO_LO_NORMAL : H_Expression.TYPE.FELLATIO_LO_HIT;
			}
			if (style.type != H_StyleData.TYPE.PETTING && ((uint)style.detailFlag & 2u) != 0)
			{
				if (flag)
				{
					return (!hit) ? H_Expression.TYPE.LICK_HI_NORMAL : H_Expression.TYPE.LICK_HI_HIT;
				}
				return (!hit) ? H_Expression.TYPE.LICK_LO_NORMAL : H_Expression.TYPE.LICK_LO_HIT;
			}
			if (style.type == H_StyleData.TYPE.SERVICE)
			{
				return (!flag) ? H_Expression.TYPE.JOB_LO : H_Expression.TYPE.JOB_HI;
			}
			if (flag)
			{
				return (!hit) ? H_Expression.TYPE.PANT_HI_NORMAL : H_Expression.TYPE.PANT_HI_HIT;
			}
			return (!hit) ? H_Expression.TYPE.PANT_LO_NORMAL : H_Expression.TYPE.PANT_LO_HIT;
		}

		private H_Expression.TYPE VoiceToExpressionType_Spurt(H_Voice.Data voice)
		{
			if (voice.kind == H_Voice.KIND.TALK)
			{
				return H_Expression.TYPE.PANT_TALK_HI;
			}
			if (((uint)param.style.detailFlag & 8u) != 0)
			{
				return H_Expression.TYPE.IRRUMATIO_HI_HIT;
			}
			if (((uint)param.style.detailFlag & 4u) != 0)
			{
				return H_Expression.TYPE.FELLATIO_HI_HIT;
			}
			if (param.style.type != H_StyleData.TYPE.PETTING && ((uint)param.style.detailFlag & 2u) != 0)
			{
				return H_Expression.TYPE.LICK_HI_HIT;
			}
			if (param.style.type == H_StyleData.TYPE.SERVICE)
			{
				return H_Expression.TYPE.JOB_HI;
			}
			return H_Expression.TYPE.PANT_HI_HIT;
		}

		private H_Expression.TYPE VoiceToExpressionType_XTC_M(Female female, H_Voice.Data voice)
		{
			bool likeSperm = female.personality.likeSperm;
			if (param.xtcType == XTC_TYPE.EJA_OUT)
			{
				return (!likeSperm) ? H_Expression.TYPE.EJACULATION_OUT_NORMAL : H_Expression.TYPE.EJACULATION_OUT_LIKE;
			}
			if ((param.style.detailFlag & H_StyleData.DetailMasking_InMouth) != 0)
			{
				return (!likeSperm) ? H_Expression.TYPE.EJACULATION_MOUTH_NORMAL : H_Expression.TYPE.EJACULATION_MOUTH_LIKE;
			}
			return (!likeSperm) ? H_Expression.TYPE.EJACULATION_IN_NORMAL : H_Expression.TYPE.EJACULATION_IN_LIKE;
		}

		public void Wear()
		{
			if (PoseData != null || StyleData == null)
			{
				return;
			}
			foreach (Female female in females)
			{
				WEAR_SHOW show = WEAR_SHOW.ALL;
				WEAR_SHOW show2 = WEAR_SHOW.ALL;
				WEAR_SHOW show3 = WEAR_SHOW.ALL;
				WEAR_SHOW show4 = WEAR_SHOW.ALL;
				WEAR_SHOW show5 = WEAR_SHOW.ALL;
				if (StyleData.type == H_StyleData.TYPE.INSERT)
				{
					show2 = WEAR_SHOW.HALF;
					show4 = WEAR_SHOW.HALF;
					show5 = WEAR_SHOW.HALF;
					if (((uint)StyleData.detailFlag & 0x20u) != 0)
					{
						show4 = WEAR_SHOW.HIDE;
					}
				}
				else if (StyleData.type == H_StyleData.TYPE.PETTING)
				{
					if (((uint)StyleData.detailFlag & 0x10u) != 0)
					{
						show2 = WEAR_SHOW.HALF;
						show4 = WEAR_SHOW.HALF;
						show5 = WEAR_SHOW.HALF;
					}
					else if (((uint)StyleData.detailFlag & 0x20u) != 0)
					{
						show2 = WEAR_SHOW.HALF;
						show4 = WEAR_SHOW.HIDE;
						show5 = WEAR_SHOW.HALF;
					}
				}
				else if (StyleData.type == H_StyleData.TYPE.SERVICE)
				{

					if (((uint)StyleData.detailFlag & 0x80u) != 0)
					{
						show = WEAR_SHOW.HALF;
						show3 = WEAR_SHOW.HALF;
						show2 = WEAR_SHOW.HALF;
					}
					else
					{
						show2 = WEAR_SHOW.HALF;
					}
				}
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.TOPUPPER, show);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.TOPLOWER, show2);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.BOTTOM, show2);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.BRA, show3);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.SHORTS, show4);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.SWIMUPPER, show3);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.SWIMLOWER, show4);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.SWIM_TOPUPPER, show);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.SWIM_TOPLOWER, show2);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.SWIM_BOTTOM, show2);
				female.wears.ChangeShow_StripOnly(WEAR_SHOW_TYPE.PANST, show5);
				female.CheckShow();
			}
		}

		public Human GetHumanFromString(string str)
		{
			if (str.IndexOf("f") == 0)
			{
				int index = int.Parse(str.Remove(0, 1));
				return females[index];
			}
			if (str.IndexOf("m") == 0)
			{
				int index2 = int.Parse(str.Remove(0, 1));
				return males[index2];
			}
			return null;
		}

		public Transform GetIKTargetFromString(string str)
		{
			if (str.IndexOf("f") == 0)
			{
				int index = int.Parse(str.Remove(0, 1));
				return females[index].body.Anime.transform;
			}
			if (str.IndexOf("m") == 0)
			{
				int index2 = int.Parse(str.Remove(0, 1));
				return males[index2].body.Anime.transform;
			}
			if (str.IndexOf("n") == 0)
			{
				return mapIK.transform;
			}
			if (str.IndexOf("i") == 0)
			{
				return itemObj.transform;
			}
			return null;
		}

		public bool HasHitArea()
		{
			if (StyleData == null)
			{
				return false;
			}
			if (StyleData.type == H_StyleData.TYPE.SERVICE)
			{
				return true;
			}
			for (int i = 0; i < females.Count; i++)
			{
				if (HasHitArea(females[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool HasHitArea(Female female)
		{
			if (StyleData == null)
			{
				return false;
			}
			if (female.personality.feelVagina && ((uint)StyleData.detailFlag & 0x10u) != 0)
			{
				return true;
			}
			if (female.personality.feelAnus && ((uint)StyleData.detailFlag & 0x20u) != 0)
			{
				return true;
			}
			return false;
		}

		public void AddGage(bool addFemale, bool addMale)
		{
			if (!h_scene.IsOverlapMode())
			{
				bool flag = StyleData.type == H_StyleData.TYPE.INSERT || StyleData.type == H_StyleData.TYPE.PETTING;
				bool flag2 = StyleData.type == H_StyleData.TYPE.SERVICE;
				float num = ((!flag || !param.hit) ? 1f : h_scene.hitSpeedRate);
				float num2 = ((!flag2 || !param.hit) ? 1f : h_scene.hitSpeedRate);
				if (addFemale && !h_scene.femaleGageLock.isOn)
				{
					FemaleGageVal += h_scene.femaleGageSpeed * num * Time.deltaTime;
					FemaleGageVal = Mathf.Clamp01(FemaleGageVal);
				}
				if (addMale && !h_scene.maleGageLock.isOn)
				{
					MaleGageVal += h_scene.maleGageSpeed * num2 * Time.deltaTime;
					MaleGageVal = Mathf.Clamp01(MaleGageVal);
				}
			}
		}

		public void AddCountXTC()
		{
			for (int i = 0; i < females.Count; i++)
			{
				if (param.xtcType == XTC_TYPE.EJA_IN || param.xtcType == XTC_TYPE.EJA_OUT || param.xtcType == XTC_TYPE.XTC_W)
				{
					females[i].personality.eja_count++;
				}
				if (((uint)param.style.detailFlag & 0x10u) != 0)
				{
					if (param.xtcType == XTC_TYPE.EJA_IN || param.xtcType == XTC_TYPE.EJA_OUT || param.xtcType == XTC_TYPE.XTC_W)
					{
						females[i].personality.eja_count_vagina++;
					}
					if (param.xtcType == XTC_TYPE.XTC_F || param.xtcType == XTC_TYPE.XTC_W)
					{
						females[i].personality.xtc_count_vagina++;
						females[i].personality.AddFeelVagina();
					}
					if (param.xtcType == XTC_TYPE.EJA_IN || param.xtcType == XTC_TYPE.XTC_W)
					{
						females[i].personality.spermInCntV++;
					}
				}
				else if (((uint)param.style.detailFlag & 0x20u) != 0)
				{
					if (param.xtcType == XTC_TYPE.EJA_IN || param.xtcType == XTC_TYPE.EJA_OUT || param.xtcType == XTC_TYPE.XTC_W)
					{
						females[i].personality.eja_count_anal++;
					}
					if (param.xtcType == XTC_TYPE.XTC_F || param.xtcType == XTC_TYPE.XTC_W)
					{
						females[i].personality.xtc_count_anal++;
						females[i].personality.AddFeelAnus();
					}
					if (param.xtcType == XTC_TYPE.EJA_IN || param.xtcType == XTC_TYPE.XTC_W)
					{
						females[i].personality.spermInCntA++;
					}
				}
				if (param.xtcType == XTC_TYPE.EJA_IN || param.xtcType == XTC_TYPE.XTC_W)
				{
					females[i].personality.AddLikeSperm();
				}
				if (param.style.type != H_StyleData.TYPE.PETTING && (((uint)param.style.detailFlag & 2u) != 0 || (param.style.detailFlag & H_StyleData.DetailMasking_InMouth) != 0))
				{
					females[i].personality.AddLikeFeratio();
				}
			}
			h_scene.ExitEnable = true;
		}

		public void SwapFemale(int no, Female female)
		{
			females[no] = female;
			female.transform.SetParent(Transform, false);
			female.transform.localPosition = Vector3.zero;
			female.transform.localRotation = Quaternion.identity;
			female.ChangeNeckLook(LookAtRotator.TYPE.NO, null, false);
			female.ChangeEyeLook(LookAtRotator.TYPE.TARGET, female.FacePos, false);
			int swapVisitor = param.swapVisitor;
			param.Init();
			param.mapName = h_scene.map.name;
			param.swapVisitor = swapVisitor;
			voiceLogs[no].Clear();
			humans.Clear();
			for (int i = 0; i < females.Count; i++)
			{
				humans.Add(females[i]);
			}
			for (int j = 0; j < males.Count; j++)
			{
				humans.Add(males[j]);
			}
			FemaleGageVal = FemaleGageStartVal();
		}

		public void SwapFemale01()
		{
			if (females.Count < 2)
			{
				Debug.LogError("スワップ不可");
				return;
			}
			Female value = females[0];
			females[0] = females[1];
			females[1] = value;
			H_VoiceLog value2 = voiceLogs[0];
			voiceLogs[0] = voiceLogs[1];
			voiceLogs[1] = value2;
		}

		public float FemaleGageStartVal()
		{

			int value = females[0].personality.xtc_count_vagina + females[0].personality.xtc_count_anal;
			value = Mathf.Clamp(value, 0, 2);
			return (float)value * (1f / 3f);
		}

		private void SetPosYaw(PosYaw pos_yaw)
		{
			lastPosYaw = pos_yaw;
			Transform.position = pos_yaw.pos;
			Transform.rotation = Quaternion.Euler(0f, pos_yaw.yaw, 0f);
		}

		public PosYaw GetLastPosYaw()
		{
			return lastPosYaw;
		}

		public Transform CaclVisitorLookPos()
		{
			Female female = h_scene.mainMembers.GetFemale(0);
			Vector3 position = female.HeadPosTrans.position;
			visitorLookPos.transform.position = position;
			return visitorLookPos.transform;
		}

		public Human GetHuman(int no)
		{
			return humans[no];
		}

		public List<Human> GetHumans()
		{
			return humans;
		}

		public Male GetMale(int no)
		{
			return males[no];
		}

		public List<Male> GetMales()
		{
			return males;
		}

		public Female GetFemale(int no)
		{
			return females[no];
		}

		public List<Female> GetFemales()
		{
			return females;
		}
	}
}
