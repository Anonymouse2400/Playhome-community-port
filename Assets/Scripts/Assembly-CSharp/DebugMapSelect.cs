using System;
using System.Collections.Generic;
using H;
using UnityEngine;
using UnityEngine.UI;

public class DebugMapSelect : MonoBehaviour
{
	private H_Scene h_scene;

	[SerializeField]
	private Dropdown dropdownPose;

	private static readonly string[] PoseNames = new string[36]
	{
		"律子初回H", "律子立ち抵抗", "律子床抵抗", "律子椅子抵抗", "律子立ち豹変", "律子床豹変", "律子椅子豹変", "明子初回H", "明子立ち抵抗", "明子床抵抗",
		"明子椅子抵抗", "明子立ち豹変", "明子床豹変", "明子椅子豹変", "雪子初回H", "雪子立ち抵抗", "雪子床抵抗", "雪子椅子抵抗", "雪子立ち豹変", "雪子床豹変",
		"雪子椅子豹変", "性器挑発A", "性器挑発B", "性器挑発C", "アナル挑発A", "アナル挑発B", "アナル挑発C", "奉仕挑発A", "奉仕挑発B", "奉仕挑発C",
		"律子豹変イベント", "明子豹変イベント", "雪子豹変イベント", "最終：雪子１", "最終：姉妹", "最終：雪子２"
	};

	private void Start()
	{
		h_scene = UnityEngine.Object.FindObjectOfType<H_Scene>();
		List<string> list = new List<string>();
		for (int i = 0; i < PoseNames.Length; i++)
		{
			list.Add(PoseNames[i]);
		}
		dropdownPose.value = -1;
		dropdownPose.ClearOptions();
		dropdownPose.AddOptions(list);
		dropdownPose.onValueChanged.AddListener(ChangePose);
	}

	private void Update()
	{
		bool active = h_scene.mainMembers.StateMgr.NowStateID == H_STATE.START;
		dropdownPose.gameObject.SetActive(active);
	}

	public void ChangePos(int no)
	{
		h_scene.mainMembers.floorPosNo = no;
		h_scene.mainMembers.wallPosNo = no;
		h_scene.mainMembers.chairPosNo = no;
		h_scene.mainMembers.specialPosNo = no;
		H_Pos h_pos = h_scene.mainMembers.SetDataPos();
		h_scene.VisitorPos(h_pos);
	}

	private void ChangePose(int no)
	{
		string[] array = new string[36]
		{
			"RitsukoFirstH", "RitsukoStandResist", "RitsukoLieResist", "RitsukoChairResist", "RitsukoStandFlip", "RitsukoLieFlip", "RitsukoChairFlip", "AkikoFirstH", "AkikoStandResist", "AkikoLieResist",
			"AkikoChairResist", "AkikoStandFlip", "AkikoLieFlip", "AkikoChairFlip", "YukikoFirstH", "YukikoStandResist", "YukikoLieResist", "YukikoChairResist", "YukikoStandFlip", "YukikoLieFlip",
			"YukikoChairFlip", "Excite_VaginaA", "Excite_VaginaB", "Excite_VaginaC", "Excite_AnalA", "Excite_AnalB", "Excite_AnalC", "Excite_ServiceA", "Excite_ServiceB", "Excite_ServiceC",
			"RitsukoFlipFlop", "AkikoFlipFlop", "YukikoFlipFlop", "FinalYukiko1", "FinalSisters", "FinalYukiko2"
		};
		h_scene.StartPose(0, array[no]);
		h_scene.mainMembers.SetDataPos();
	}
}
