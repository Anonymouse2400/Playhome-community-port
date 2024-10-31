using System;
using System.Collections.Generic;
using System.IO;
using Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardFileList : MonoBehaviour
{
	public enum TYPE
	{
		CHARA = 0,
		COORDINATE = 1
	}

	[Serializable]
	public class SaveEvent : UnityEvent<string>
	{
	}

	[Serializable]
	public class LoadEvent : UnityEvent<string, int>
	{
	}

	public static Vector2 PNG_Size = new Vector2(252f, 352f);

	public Scrollbar scroll;

	public Toggle item_original;

	public RectTransform maskArea;

	public RectTransform contentArea;

	public int h_num = 3;

	public Texture2D noLoadTex;

	public int cacheNum = 100;

	public float scrollSpeed = 1f;

	public ToggleGroup group;

	public Image previewImage;

	public Text previewName;

	private List<Card_Data> datas = new List<Card_Data>();

	private List<Toggle> toggles = new List<Toggle>();

	private Vector2 itemSize;

	private int yNum;

	private int select = -1;

	public Button button_save_new;

	public Button button_save_overwrite;

	public Button button_load;

	public Button button_delete;

	[SerializeField]
	private string directory;

	private string path;

	[SerializeField]
	private LoadUI loadUI;

	[SerializeField]
	private Dropdown sortUI;

	[SerializeField]
	private SaveEvent onSave;

	[SerializeField]
	private LoadEvent onLoad;

	private TYPE dataType;

	private int sortType;

	public void Seup(TYPE type, SEX sex)
	{
		path = directory + ((sex != 0) ? "/Male/" : "/Female/");
		string[] files = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly);
		for (int i = 0; i < files.Length; i++)
		{
			datas.Add(new Card_Data(files[i], noLoadTex));
		}
		previewImage.transform.parent.gameObject.SetActive(false);
		button_save_new.interactable = true;
		button_save_overwrite.interactable = false;
		button_load.interactable = false;
		button_delete.interactable = false;
		for (int j = 0; j < datas.Count; j++)
		{
			Toggle toggle = UnityEngine.Object.Instantiate(item_original);
			toggle.gameObject.SetActive(true);
			RectTransform rectTransform = toggle.transform as RectTransform;
			toggle.transform.SetParent(contentArea, false);
			toggle.group = group;
			toggle.isOn = false;
			toggles.Add(toggle);
		}
		select = -1;
		dataType = type;
		if (dataType == TYPE.CHARA)
		{
			sortType = GlobalData.sortChara;
		}
		else if (dataType == TYPE.COORDINATE)
		{
			sortType = GlobalData.sortCoord;
		}
		Sort_WithoutSE(sortType);
		sortUI.value = sortType;
		sortUI.onValueChanged.AddListener(Sort);
		ListPlacement();
	}

	private void Update()
	{
		float y = maskArea.sizeDelta.y;
		float y2 = contentArea.sizeDelta.y;
		float num = y2 - y;
		float value = 1f;
		if (y2 > 0f)
		{
			value = y / y2;
		}
		scroll.size = Mathf.Clamp(value, 0.2f, 1f);
		Vector2 anchoredPosition = contentArea.anchoredPosition;
		anchoredPosition.y = num * scroll.value;
		contentArea.anchoredPosition = anchoredPosition;
		int num2 = -1;
		for (int i = 0; i < toggles.Count; i++)
		{
			RectTransform rectTransform = toggles[i].transform as RectTransform;
			float y3 = rectTransform.anchoredPosition.y;
			float y4 = rectTransform.sizeDelta.y;
			float num3 = 0f - anchoredPosition.y;
			float num4 = num3 - y;
			float num5 = y3;
			float num6 = y3 - y4;
			if (num6 <= num3 && num5 >= num4)
			{
				datas[i].show = true;
			}
			else
			{
				datas[i].show = false;
			}
			datas[i].isSelect = toggles[i].isOn;
			datas[i].UpdateSprite(toggles[i].image);
			if (toggles[i].isOn)
			{
				num2 = i;
			}
		}
		if (num2 == -1 && select >= datas.Count)
		{
			num2 = datas.Count - 1;
		}
		if (num2 != select)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			select = num2;
			ChangeSelect();
		}
	}

	private void ChangeSelect()
	{
		if (select != -1 && !toggles[select].isOn)
		{
			toggles[select].isOn = true;
		}
		previewImage.transform.parent.gameObject.SetActive(select != -1);
		if (select != -1)
		{
			Texture2D texture2D = datas[select].Texture(true);
			Vector2 pNG_Size = PNG_Size;
			if (texture2D != null)
			{
				pNG_Size.x = texture2D.width;
				pNG_Size.y = texture2D.height;
			}
			previewImage.sprite = Sprite.Create(texture2D, new Rect(Vector2.zero, pNG_Size), pNG_Size * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
			previewName.text = datas[select].charaInfo.name;
		}
		button_save_overwrite.interactable = select != -1;
		button_load.interactable = select != -1;
		button_delete.interactable = select != -1;
	}

	public void ScrollEvent(BaseEventData data)
	{
		if (yNum != 0)
		{
			float num = 1f / (float)yNum * scrollSpeed;
			float num2 = (0f - Input.mouseScrollDelta.y) * num;
			scroll.value += num2;
		}
	}

	public void Load()
	{
		if (select != -1)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			loadUI.Setup(previewImage.sprite, datas[select].charaInfo.name, datas[select].file, LoadExe);
		}
	}

	private void LoadExe(int filter)
	{
		onLoad.Invoke(datas[select].file, filter);
	}

	public void SaveNew()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		onSave.Invoke(string.Empty);
	}

	public void SaveOverwrite()
	{
		if (select != -1)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			onSave.Invoke(datas[select].file);
		}
	}

	public void Delete()
	{
		if (select != -1)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
			string text = datas[select].charaInfo.name + "を削除してもよろしいですか？";
			gameControl.CreateModalYesNoUI(text, DeleteExe);
		}
	}

	private void DeleteExe()
	{
		if (select != -1)
		{
			File.Delete(datas[select].file);
			datas[select].Delete();
			UnityEngine.Object.Destroy(toggles[select].gameObject);
			datas.RemoveAt(select);
			toggles.RemoveAt(select);
			ListPlacement();
			if (select >= datas.Count)
			{
				select = datas.Count - 1;
			}
			ChangeSelect();
		}
	}

	public void Sort(int type)
	{
		sortType = type;
		if (dataType == TYPE.CHARA)
		{
			GlobalData.sortChara = type;
		}
		else
		{
			GlobalData.sortCoord = type;
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
		Sort_WithoutSE(type);
	}

	public void Sort_WithoutSE(int type)
	{
		switch (type)
		{
		case 0:
			datas.Sort(Sort_Date);
			break;
		case 1:
			datas.Sort(Sort_Date);
			datas.Reverse();
			break;
		case 2:
			datas.Sort(Sort_Name);
			break;
		case 3:
			datas.Sort(Sort_Name);
			datas.Reverse();
			break;
		}
		ListPlacement();
		ChangeSelect();
	}

	private static int Sort_Name(Card_Data a, Card_Data b)
	{
		return string.Compare(a.charaInfo.name, b.charaInfo.name, false);
	}

	private static int Sort_Date(Card_Data a, Card_Data b)
	{
		return DateTime.Compare(a.charaInfo.time, b.charaInfo.time);
	}

	private void ListPlacement()
	{
		float num = maskArea.sizeDelta.x / (float)h_num;
		float num2 = num / PNG_Size.x;
		itemSize = PNG_Size * num2;
		yNum = datas.Count / h_num;
		if (datas.Count % h_num != 0)
		{
			yNum++;
		}
		Vector2 sizeDelta = contentArea.sizeDelta;
		sizeDelta.y = itemSize.y * (float)yNum;
		if (sizeDelta.y < maskArea.sizeDelta.y)
		{
			sizeDelta.y = maskArea.sizeDelta.y;
		}
		contentArea.sizeDelta = sizeDelta;
		float num3 = 0f;
		for (int i = 0; i < datas.Count; i++)
		{
			float x = itemSize.x * (float)(i % h_num);
			Toggle toggle = toggles[i];
			RectTransform rectTransform = toggle.transform as RectTransform;
			rectTransform.anchoredPosition = new Vector2(x, num3);
			rectTransform.localScale = Vector3.one;
			if (i % h_num == h_num - 1)
			{
				num3 -= itemSize.y;
			}
			Text componentInChildren = toggles[i].GetComponentInChildren<Text>();
			componentInChildren.text = datas[i].charaInfo.name;
			if (datas[i].HasTex)
			{
				datas[i].ChangeTex();
			}
		}
	}

	public void Replacement(bool noSelect)
	{
		for (int i = 0; i < datas.Count; i++)
		{
			datas[i].Delete();
		}
		datas.Clear();
		string[] files = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly);
		for (int j = 0; j < files.Length; j++)
		{
			datas.Add(new Card_Data(files[j], noLoadTex));
		}
		for (int k = 0; k < toggles.Count; k++)
		{
			UnityEngine.Object.Destroy(toggles[k].gameObject);
		}
		toggles.Clear();
		for (int l = 0; l < datas.Count; l++)
		{
			Toggle toggle = UnityEngine.Object.Instantiate(item_original);
			toggle.gameObject.SetActive(true);
			RectTransform rectTransform = toggle.transform as RectTransform;
			toggle.transform.SetParent(contentArea);
			toggle.group = group;
			toggle.isOn = false;
			toggles.Add(toggle);
		}
		if (noSelect)
		{
			select = -1;
		}
		else if (select >= datas.Count)
		{
			select = datas.Count - 1;
		}
		Sort_WithoutSE(sortType);
		ListPlacement();
		ChangeSelect();
	}
}
