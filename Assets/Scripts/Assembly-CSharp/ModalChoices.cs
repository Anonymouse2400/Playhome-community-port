using System;
using UnityEngine;
using UnityEngine.UI;

public class ModalChoices : ModalUI
{
	private class Set
	{
		public ModalChoices owner;

		public int no;

		public Action act;

		public Set(ModalChoices owner, int no, Action[] acts)
		{
			this.owner = owner;
			this.no = no;
			act = null;
			if (no < acts.Length)
			{
				act = acts[no];
			}
		}

		public void Choice()
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			if (act != null)
			{
				act();
			}
			owner.End();
		}
	}

	public Canvas myCanvas;

	public Text text;

	public GameObject choiceOriginal;

	public Transform choicesRoot;

	private Set[] sets;

	protected override void Awake()
	{
		base.Awake();
	}

	public void SetUp(string text, string[] texts, Action[] acts)
	{
		this.text.text = text;
		sets = new Set[acts.Length];
		for (int i = 0; i < texts.Length; i++)
		{
			sets[i] = new Set(this, i, acts);
			GameObject gameObject = UnityEngine.Object.Instantiate(choiceOriginal);
			gameObject.SetActive(true);
			gameObject.transform.SetParent(choicesRoot, false);
			gameObject.GetComponentInChildren<Text>().text = texts[i];
			gameObject.GetComponentInChildren<Button>().onClick.AddListener(sets[i].Choice);
		}
	}
}
