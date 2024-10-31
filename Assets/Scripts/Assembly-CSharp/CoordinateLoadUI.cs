using System;
using UnityEngine;
using UnityEngine.UI;

public class CoordinateLoadUI : LoadUI
{
	[SerializeField]
	private Image cardImage;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Toggle checkWear;

	[SerializeField]
	private Toggle checkAccessory;

	[SerializeField]
	private Button buttonAll;

	[SerializeField]
	private Button buttonWear;

	[SerializeField]
	private Button buttonAcce;

	[SerializeField]
	private Button buttonNone;

	[SerializeField]
	private Button buttonLoad;

	[SerializeField]
	private Button buttonCancel;

	private string file;

	private Action<int> loadAct;

	private void Awake()
	{
		buttonAll.onClick.AddListener(ShortCut_All);
		buttonNone.onClick.AddListener(ShortCut_None);
		buttonWear.onClick.AddListener(ShortCut_Wear);
		buttonAcce.onClick.AddListener(ShortCut_Acce);
		buttonLoad.onClick.AddListener(Load);
		buttonCancel.onClick.AddListener(Cancel);
	}

	public override void Setup(Sprite thumbnail, string name, string file, Action<int> loadAct)
	{
		cardImage.sprite = thumbnail;
		nameText.text = name;
		this.file = file;
		this.loadAct = loadAct;
		base.gameObject.SetActive(true);
	}

	private void Update()
	{
		buttonLoad.interactable = checkWear.isOn || checkAccessory.isOn;
	}

	private void ShortCut_All()
	{
		checkWear.isOn = true;
		checkAccessory.isOn = true;
	}

	private void ShortCut_None()
	{
		checkWear.isOn = false;
		checkAccessory.isOn = false;
	}

	private void ShortCut_Wear()
	{
		checkWear.isOn = true;
		checkAccessory.isOn = false;
	}

	private void ShortCut_Acce()
	{
		checkWear.isOn = false;
		checkAccessory.isOn = true;
	}

	private int CalcFilter()
	{
		int num = 0;
		if (checkWear.isOn)
		{
			num |= 8;
		}
		if (checkAccessory.isOn)
		{
			num |= 0x10;
		}
		return num;
	}

	private void Load()
	{
		int obj = CalcFilter();
		loadAct(obj);
		base.gameObject.SetActive(false);
	}

	private void Cancel()
	{
		base.gameObject.SetActive(false);
	}
}
