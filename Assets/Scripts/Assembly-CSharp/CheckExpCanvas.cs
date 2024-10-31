using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class CheckExpCanvas : MonoBehaviour
{
	private enum STEP
	{
		START = 0,
		EXP = 1,
		MSG = 2,
		END = 3
	}

	private enum NEXT
	{
		CLOSE = 0,
		FLIP_FLOP = 1,
		FINAL = 2
	}

	[SerializeField]
	private CharaExpGages original;

	private float timer;

	private NEXT next;

	private List<string> unlockMSGs = new List<string>();

	private bool isAdd;

	private STEP step = STEP.EXP;

	private SelectScene selectScene;

	private void Start()
	{
		selectScene = UnityEngine.Object.FindObjectOfType<SelectScene>();
		isAdd = false;
		next = NEXT.CLOSE;
		step = STEP.START;
		if (GlobalData.PlayData.Progress != GamePlayData.PROGRESS.ALL_FREE)
		{
			bool flag = GlobalData.PlayData.IsFlopFromBadgeNum();
			for (int i = 0; i < 3; i++)
			{
				if (GlobalData.PlayData.personality[i].IsRequireAdjustment())
				{
					CharaExpGages charaExpGages = UnityEngine.Object.Instantiate(original);
					charaExpGages.gameObject.SetActive(true);
					charaExpGages.transform.SetParent(base.transform, false);
					charaExpGages.Setup((HEROINE)i, GlobalData.PlayData.personality[i]);
					isAdd = true;
				}
			}
			bool flag2 = GlobalData.PlayData.IsFlopFromBadgeNum();
			bool flag3 = GlobalData.PlayData.IsAllFreeFromBadgeNum();
			if (!flag && flag2)
			{
				next = NEXT.FLIP_FLOP;
			}
			else if (flag3)
			{
				next = NEXT.FINAL;
			}
		}
		else
		{
			for (int j = 0; j < 3; j++)
			{
				GlobalData.PlayData.personality[j].AdjustmentExp_Free();
			}
		}
		int badgeNum = GlobalData.PlayData.GetBadgeNum();
		if (!GlobalData.PlayData.unlockWeaknessRecovery && badgeNum >= 6)
		{
			GlobalData.PlayData.unlockWeaknessRecovery = true;
			string item = "脱力回復機能をアンロックしました。\nH中に脱力状態になった場合\n画面右上の回復ボタンで脱力状態から回復できます。";
			unlockMSGs.Add(item);
		}
		if (!GlobalData.PlayData.unlockShowHitArea && badgeNum >= 9)
		{
			GlobalData.PlayData.unlockShowHitArea = true;
			string item2 = "快感エリア可視化機能をアンロックしました。\nHパッド内に快感エリアがある場合に\nその場所が表示されます。";
			unlockMSGs.Add(item2);
		}
		if (!GlobalData.PlayData.readAllFreeMessage && GlobalData.PlayData.Progress >= GamePlayData.PROGRESS.ALL_FREE)
		{
			GlobalData.PlayData.readAllFreeMessage = true;
			unlockMSGs.Add("ゲームをクリアしました。\n以降はフリーモードになります。");
			unlockMSGs.Add("キャラの状態に関係なく\nすべての体位が選択できるようになりました。");
			unlockMSGs.Add("男の切り替えがアンロックされました。\n選択画面の「男」から\n主人公と広一を切り替えることが出来ます。");
			unlockMSGs.Add("状態変更がアンロックされました。\n選択画面の「状態」から\nキャラの状態を自由に変更出来ます。");
		}
		NextStep();
	}

	private void Update()
	{
		if (step == STEP.EXP)
		{
			if (timer < 1f)
			{
				timer += Time.deltaTime;
			}
			else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				NextStep();
			}
		}
	}

	private void NextStep()
	{
		if (step == STEP.START)
		{
			if (isAdd)
			{
				step = STEP.EXP;
			}
			else if (unlockMSGs.Count > 0)
			{
				SystemSE.Play(SystemSE.SE.YES);
				step = STEP.MSG;
				ShowMessage();
			}
			else
			{
				End();
			}
		}
		else if (step == STEP.EXP)
		{
			if (unlockMSGs.Count > 0)
			{
				SystemSE.Play(SystemSE.SE.YES);
				step = STEP.MSG;
				ShowMessage();
			}
			else
			{
				End();
			}
		}
		else if (step == STEP.MSG)
		{
			unlockMSGs.RemoveAt(0);
			if (unlockMSGs.Count > 0)
			{
				ShowMessage();
			}
			else
			{
				End();
			}
		}
	}

	private void ShowMessage()
	{
		GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
		gameControl.CreateModalMessageUI(unlockMSGs[0], ReadMessage);
	}

	private void ReadMessage(bool doNotAgain)
	{
		NextStep();
	}

	private void End()
	{
		if (next == NEXT.FLIP_FLOP)
		{
			GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
			gameControl.ChangeScene("ADVScene", "adv/adv_01_00,ADV_Script_01_00");
		}
		else if (next == NEXT.FINAL)
		{
			GameControl gameControl2 = UnityEngine.Object.FindObjectOfType<GameControl>();
			gameControl2.ChangeScene("ADVScene", "adv/adv_02_00,ADV_Script_02_00");
		}
		base.gameObject.SetActive(false);
		selectScene.UpdateState();
	}
}
