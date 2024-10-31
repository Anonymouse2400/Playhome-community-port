using System;
using UnityEngine;
using UnityEngine.UI;

public class CharaLoadUI : LoadUI
{
	[SerializeField]
	private Image cardImage;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Toggle checkHair;

	[SerializeField]
	private Toggle checkFace;

	[SerializeField]
	private Toggle checkBody;

	[SerializeField]
	private Toggle checkWear;

	[SerializeField]
	private Toggle checkAccessory;

	[SerializeField]
	private Button buttonAll;

	[SerializeField]
	private Button buttonCoordinate;

	[SerializeField]
	private Button buttonHairFaceBody;

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
		buttonCoordinate.onClick.AddListener(ShortCut_Coordinate);
		buttonHairFaceBody.onClick.AddListener(ShortCut_Style);
		buttonNone.onClick.AddListener(ShortCut_None);
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
		buttonLoad.interactable = checkHair.isOn || checkFace.isOn || checkBody.isOn || checkWear.isOn || checkAccessory.isOn;
	}

	private void ShortCut_All()
	{
		checkHair.isOn = true;
		checkFace.isOn = true;
		checkBody.isOn = true;
		checkWear.isOn = true;
		checkAccessory.isOn = true;
	}

	private void ShortCut_Coordinate()
	{
		checkHair.isOn = false;
		checkFace.isOn = false;
		checkBody.isOn = false;
		checkWear.isOn = true;
		checkAccessory.isOn = true;
	}

	private void ShortCut_Style()
	{
		checkHair.isOn = true;
		checkFace.isOn = true;
		checkBody.isOn = true;
		checkWear.isOn = false;
		checkAccessory.isOn = false;
	}

	private void ShortCut_None()
	{
		checkHair.isOn = false;
		checkFace.isOn = false;
		checkBody.isOn = false;
		checkWear.isOn = false;
		checkAccessory.isOn = false;
	}

	private int CalcFilter()
	{
		int num = 0;
		if (checkHair.isOn)
		{
			num |= 1;
		}
		if (checkFace.isOn)
		{
			num |= 2;
		}
		if (checkBody.isOn)
		{
			num |= 4;
		}
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
