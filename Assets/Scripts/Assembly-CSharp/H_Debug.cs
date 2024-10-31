using System;
using Character;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class H_Debug : MonoBehaviour
{
	[SerializeField]
	private Button buttonOriginal;

	[SerializeField]
	private H_Scene h_scene;

	[SerializeField]
	private Toggle mozToggle;

	[SerializeField]
	private Material[] mozMaterials;

	[SerializeField]
	private Button[] spermAddButtons;

	[SerializeField]
	private Button spermClearButton;

	private Canvas canvas;

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		mozToggle.isOn = mozMaterials[0].GetFloat("_White") > 0f;
		mozToggle.onValueChanged.AddListener(OnChangeMozToggle);
		for (int i = 0; i < spermAddButtons.Length; i++)
		{
			int no = i;
			spermAddButtons[i].onClick.AddListener(delegate
			{
				AddSperm(no);
			});
		}
		spermClearButton.onClick.AddListener(ClearSperm);
	}

	private void Update()
	{
		if ((!(EventSystem.current != null) || !(EventSystem.current.currentSelectedGameObject != null)) && Input.GetKeyDown(KeyCode.Delete))
		{
			canvas.enabled = !canvas.enabled;
		}
	}

	private void OnChangeMozToggle(bool flag)
	{
		float value = ((!flag) ? 0f : 1f);
		for (int i = 0; i < mozMaterials.Length; i++)
		{
			mozMaterials[i].SetFloat("_White", value);
		}
	}

	private void AddSperm(int no)
	{
		foreach (Female female in h_scene.mainMembers.GetFemales())
		{
			female.AddSperm((SPERM_POS)no);
		}
	}

	private void ClearSperm()
	{
		foreach (Female female in h_scene.mainMembers.GetFemales())
		{
			female.ClearSpermMaterials();
		}
	}
}
