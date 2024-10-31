using System;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
	private const int CELL_NUM = 50;

	[SerializeField]
	private ColorPicker picker;

	[SerializeField]
	private ColorPaletteCell cellOriginal;

	[SerializeField]
	private RectTransform paletteArea;

	private ColorPaletteCell[] cells = new ColorPaletteCell[50];

	private void Start()
	{
		for (int i = 0; i < ColorPaletteData.colors.Length; i++)
		{
			ColorPaletteCell colorPaletteCell = UnityEngine.Object.Instantiate(cellOriginal);
			colorPaletteCell.gameObject.SetActive(true);
			colorPaletteCell.Setup(i, ColorPaletteData.colors[i], OnSave, OnLoad);
			colorPaletteCell.transform.SetParent(paletteArea, false);
			cells[i] = colorPaletteCell;
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < ColorPaletteData.colors.Length; i++)
		{
			if (cells[i] != null)
			{
				cells[i].SetColor(ColorPaletteData.colors[i]);
			}
		}
	}

	private void Update()
	{
	}

	private void OnSave(int id)
	{
		ColorPaletteData.colors[id] = picker.EditColor;
		cells[id].SetColor(ColorPaletteData.colors[id]);
	}

	private void OnLoad(int id)
	{
		picker.EditColor = ColorPaletteData.colors[id];
	}
}
