using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ThumbnailSelectUI : MonoBehaviour
{
	[Serializable]
	private class CustomSelEvent : UnityEvent<CustomSelectSet>
	{
	}

	[SerializeField]
	private RectTransform contentRoot;

	[SerializeField]
	private ThumbnailSelectCell cellOriginal;

	[SerializeField]
	private GameObject bigThumbsRoot;

	[SerializeField]
	private Image bigThumbsImage;

	[SerializeField]
	private Text bigThumbsText;

	[SerializeField]
	private List<CustomSelectSet> datas = new List<CustomSelectSet>();

	private ThumbnailSelectCell[] cells;

	private int overlapID = -1;

	[SerializeField]
	private CustomSelEvent onSelectAction;

	[SerializeField]
	private ScrollRect scrollRect;

	private bool invoke = true;

	public int SelectNo { get; private set; }

	private void Awake()
	{
		SelectNo = -1;
		bigThumbsRoot.SetActive(false);
	}

	private void SetupCells()
	{
		DeleteCells();
		if (datas != null && datas.Count != 0)
		{
			cells = new ThumbnailSelectCell[datas.Count];
			for (int i = 0; i < datas.Count; i++)
			{
				bool isNew = datas[i].isNew;
				ThumbnailSelectCell thumbnailSelectCell = UnityEngine.Object.Instantiate(cellOriginal);
				thumbnailSelectCell.gameObject.SetActive(true);
				thumbnailSelectCell.Setup(this, i, datas[i].name, datas[i].thumbnail_S, isNew);
				thumbnailSelectCell.transform.SetParent(contentRoot, false);
				cells[i] = thumbnailSelectCell;
			}
			Vector2 anchoredPosition = contentRoot.anchoredPosition;
			anchoredPosition.y = 0f;
			contentRoot.anchoredPosition = anchoredPosition;
			scrollRect.Rebuild(CanvasUpdate.PostLayout);
		}
	}

	private void DeleteCells()
	{
		if (cells != null)
		{
			for (int i = 0; i < cells.Length; i++)
			{
				UnityEngine.Object.Destroy(cells[i].gameObject);
			}
		}
		cells = null;
	}

	public void SetDatas(List<CustomSelectSet> setDatas)
	{
		datas = setDatas;
		SetupCells();
		SelectNo = -1;
		bigThumbsRoot.SetActive(false);
	}

	private void Update()
	{
	}

	public void OnEnterCell(int no)
	{
		if (datas != null && overlapID != no)
		{
			overlapID = no;
			bigThumbsImage.sprite = datas[no].thumbnail_L;
			bigThumbsText.text = datas[no].name;
			bool active = (UnityEngine.Object)(object)bigThumbsImage.sprite != null;
			bigThumbsRoot.SetActive(active);
		}
	}

	public void OnExitCell(int no)
	{
		if (overlapID == no)
		{
			overlapID = -1;
			bigThumbsRoot.SetActive(false);
		}
	}

	public void OnSelect(int no)
	{
		SelectNo = no;
		if (invoke)
		{
			if (onSelectAction != null)
			{
				onSelectAction.Invoke(GetSelectedData());
			}
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (no >= 0)
			{
				datas[no].isNew = false;
			}
		}
	}

	public CustomSelectSet GetData(int listNo)
	{
		if (datas == null)
		{
			return null;
		}
		if (listNo >= 0 && listNo < datas.Count)
		{
			return datas[listNo];
		}
		return null;
	}

	public CustomSelectSet GetSelectedData()
	{
		if (datas == null)
		{
			return null;
		}
		if (SelectNo >= 0 && SelectNo < datas.Count)
		{
			return datas[SelectNo];
		}
		return null;
	}

	public void SetSelectedNo(int no)
	{
		invoke = false;
		SelectNo = no;
		for (int i = 0; i < cells.Length; i++)
		{
			cells[i].SetToggle(no == i);
		}
		invoke = true;
	}

	public void SetSelectedFromDataID(int id)
	{
		invoke = false;
		SelectNo = -1;
		if (datas != null && cells != null)
		{
			for (int i = 0; i < datas.Count; i++)
			{
				if (datas[i].id == id)
				{
					SelectNo = i;
					break;
				}
			}
			for (int j = 0; j < cells.Length; j++)
			{
				cells[j].SetToggle(SelectNo == j);
			}
		}
		invoke = true;
	}

	public void AddOnSelectAction(UnityAction<CustomSelectSet> act)
	{
		onSelectAction.AddListener(act);
	}

	public void RemoveAllListenersOnSelectAction()
	{
		onSelectAction.RemoveAllListeners();
	}

	public List<CustomSelectSet> GetDatas()
	{
		return datas;
	}

	public void UpdateEnables()
	{
		if (cells != null)
		{
			for (int i = 0; i < cells.Length; i++)
			{
				cells[i].gameObject.SetActive(!datas[i].hide);
				cells[i].SetInteractable(datas[i].enable);
			}
		}
	}
}
