using System;
using UnityEngine;

public class H_Map : MonoBehaviour
{
	[SerializeField]
	private H_Scene h_scene;

	[SerializeField]
	private Transform place;

	[SerializeField]
	private Transform timeZone;

	[SerializeField]
	private ToggleButton originalToggle;

	private void Start()
	{
		CreateToggleButton("夫婦の寝室", place);
		CreateToggleButton("律子の部屋", place);
		CreateToggleButton("明子の部屋", place);
		CreateToggleButton("リビング", place);
		CreateToggleButton("風呂", place);
		CreateToggleButton("和室", place);
		CreateToggleButton("洗面所", place);
		CreateToggleButton("玄関(内)", place);
		CreateToggleButton("トイレ", place);
		CreateToggleButton("玄関(外)", place);
		RadioButtonGroup radioButtonGroup = SetupRadioButtonGroup(place.gameObject, false);
		radioButtonGroup.Value = GlobalData.PlayData.lastSelectMap;
		radioButtonGroup.action.AddListener(ChangeMap);
		CreateToggleButton("昼", timeZone);
		CreateToggleButton("夕", timeZone);
		CreateToggleButton("夜(点灯)", timeZone);
		CreateToggleButton("夜(消灯)", timeZone);
		RadioButtonGroup radioButtonGroup2 = SetupRadioButtonGroup(timeZone.gameObject, false);
		radioButtonGroup2.Value = GlobalData.PlayData.lastSelectTimeZone;
		radioButtonGroup2.action.AddListener(ChangeTimeZone);
	}

	private ToggleButton CreateToggleButton(string title, Transform parent)
	{
		ToggleButton toggleButton = UnityEngine.Object.Instantiate(originalToggle);
		toggleButton.SetText(title, title);
		toggleButton.transform.SetParent(parent, false);
		return toggleButton;
	}

	private RadioButtonGroup SetupRadioButtonGroup(GameObject go, bool allowOff)
	{
		RadioButtonGroup radioButtonGroup = go.GetComponent<RadioButtonGroup>();
		if (radioButtonGroup == null)
		{
			radioButtonGroup = go.AddComponent<RadioButtonGroup>();
		}
		radioButtonGroup.AllowOFF = allowOff;
		ToggleButton[] componentsInChildren = go.GetComponentsInChildren<ToggleButton>();
		radioButtonGroup.SetToggleButtons(componentsInChildren);
		return radioButtonGroup;
	}

	private void ChangeMap(int no)
	{
		if (GlobalData.PlayData.lastSelectMap != no)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			GlobalData.PlayData.lastSelectMap = no;
			Change(false);
		}
	}

	private void ChangeTimeZone(int no)
	{
		if (GlobalData.PlayData.lastSelectTimeZone != no)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			GlobalData.PlayData.lastSelectTimeZone = no;
			Change(true);
		}
	}

	private void Change(bool changeTimeOnly)
	{
		h_scene.LoadMap(changeTimeOnly);
		Resources.UnloadUnusedAssets();
	}
}
