using System;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class AcceCopyHelperUI : MonoBehaviour
{
	[SerializeField]
	private Toggle[] dstSlots = new Toggle[10];

	[SerializeField]
	private Toggle[] srcSlots = new Toggle[10];

	[SerializeField]
	private Toggle reverse;

	[SerializeField]
	private Button buttons_slotCopy;

	[SerializeField]
	private Button buttons_posCopy;

	[SerializeField]
	private Button buttons_posCopyRevH;

	[SerializeField]
	private Button buttons_posCopyRevV;

	[SerializeField]
	private AccessoryCustomEdit acceEdit;

	private Human human;

	public void SetHuman(Human human)
	{
		this.human = human;
		SetAccessoryNames();
	}

	private void OnEnable()
	{
		SetAccessoryNames();
	}

	public void SetAccessoryNames()
	{
		if (!(human == null))
		{
			for (int i = 0; i < 10; i++)
			{
				AccessoryData accessoryData = human.accessories.GetAccessoryData(human.customParam.acce, i);
				string text = (i + 1).ToString("00:");
				text = ((accessoryData == null) ? (text + "なし") : (text + accessoryData.name));
				dstSlots[i].GetComponentInChildren<Text>().text = text;
				srcSlots[i].GetComponentInChildren<Text>().text = text;
			}
		}
	}

	private void Update()
	{
		int dst;
		int src;
		CheckToggles(out dst, out src);
		AccessoryData accessoryData = ((src < 0) ? null : human.accessories.GetAccessoryData(human.customParam.acce, src));
		AccessoryData accessoryData2 = ((dst < 0) ? null : human.accessories.GetAccessoryData(human.customParam.acce, dst));
		bool interactable = dst != -1 && src != -1 && (accessoryData != null || accessoryData2 != null);
		bool interactable2 = accessoryData != null && accessoryData2 != null;
		buttons_slotCopy.interactable = interactable;
		buttons_posCopy.interactable = interactable2;
		buttons_posCopyRevH.interactable = interactable2;
		buttons_posCopyRevV.interactable = interactable2;
	}

	private void CheckToggles(out int dst, out int src)
	{
		dst = -1;
		src = -1;
		for (int i = 0; i < 10; i++)
		{
			if (dstSlots[i].isOn)
			{
				dst = i;
			}
			if (srcSlots[i].isOn)
			{
				src = i;
			}
		}
	}

	public void Button_CopySlot()
	{
		AccessoryParameter acce = human.customParam.acce;
		int dst;
		int src;
		CheckToggles(out dst, out src);
		acce.slot[dst].Copy(acce.slot[src]);
		if (reverse.isOn)
		{
			acce.slot[dst].nowAttach = Accessories.GetReverseAttach(acce.slot[dst].nowAttach);
		}
		human.accessories.AccessoryInstantiate(acce, dst, false, null);
		Resources.UnloadUnusedAssets();
		SetAccessoryNames();
		SystemSE.Play(SystemSE.SE.YES);
		Update_OtherUI();
	}

	public void Button_CopyPos()
	{
		AccessoryParameter acce = human.customParam.acce;
		int dst;
		int src;
		CheckToggles(out dst, out src);
		acce.slot[dst].addPos = acce.slot[src].addPos;
		acce.slot[dst].addRot = acce.slot[src].addRot;
		acce.slot[dst].addScl = acce.slot[src].addScl;
		human.accessories.UpdatePosition(acce, dst);
		SystemSE.Play(SystemSE.SE.YES);
		Update_OtherUI();
	}

	public void Button_CopyPosRev_H()
	{
		AccessoryParameter acce = human.customParam.acce;
		int dst;
		int src;
		CheckToggles(out dst, out src);
		acce.slot[dst].addPos = acce.slot[src].addPos;
		acce.slot[dst].addRot = acce.slot[src].addRot;
		acce.slot[dst].addScl = acce.slot[src].addScl;
		acce.slot[dst].addRot.y = Mathf.DeltaAngle(0f, acce.slot[dst].addRot.y + 180f);
		human.accessories.UpdatePosition(acce, dst);
		SystemSE.Play(SystemSE.SE.YES);
		Update_OtherUI();
	}

	public void Button_CopyPosRev_V()
	{
		AccessoryParameter acce = human.customParam.acce;
		int dst;
		int src;
		CheckToggles(out dst, out src);
		acce.slot[dst].addPos = acce.slot[src].addPos;
		acce.slot[dst].addRot = acce.slot[src].addRot;
		acce.slot[dst].addScl = acce.slot[src].addScl;
		acce.slot[dst].addRot.x = Mathf.DeltaAngle(0f, acce.slot[dst].addRot.x + 180f);
		human.accessories.UpdatePosition(acce, dst);
		SystemSE.Play(SystemSE.SE.YES);
		Update_OtherUI();
	}

	private void Update_OtherUI()
	{
		acceEdit.UpdateUI_NowTab();
		HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
		if (hWearAcceChangeUI != null)
		{
			hWearAcceChangeUI.CheckShowUI();
		}
	}
}
