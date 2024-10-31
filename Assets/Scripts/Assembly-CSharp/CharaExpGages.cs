using System;
using Character;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class CharaExpGages : MonoBehaviour
{
	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Image[] gages = new Image[5];

	[SerializeField]
	private Image[] unlocks = new Image[5];

	[SerializeField]
	private float K = 10f;

	[SerializeField]
	private float minSpeed = 0.01f;

	[SerializeField]
	private Text vaginaText;

	[SerializeField]
	private Text analText;

	private Personality personality;

	private bool virgineChanged;

	private float virginDelay = 1f;

	public void Setup(HEROINE heroineID, Personality personality)
	{
		nameText.text = Female.HeroineName(heroineID);
		this.personality = personality;
		gages[0].fillAmount = personality.ExpFeelVaginaRate;
		gages[1].fillAmount = personality.ExpFeelAnusRate;
		gages[2].fillAmount = personality.ExpIndecentLanguageRate;
		gages[3].fillAmount = personality.ExpLikeFeratioRate;
		gages[4].fillAmount = personality.ExpLikeSpermRate;
		for (int i = 0; i < gages.Length; i++)
		{
			bool active = gages[i].fillAmount >= 1f;
			unlocks[i].gameObject.SetActive(active);
		}
		SetVergineText();
		personality.AdjustmentExp();
		if (personality.expFeelVagina >= 15)
		{
			personality.feelVagina = true;
		}
		if (personality.expFeelAnus >= 15)
		{
			personality.feelAnus = true;
		}
		if (personality.expIndecentLanguage >= 21)
		{
			personality.indecentLanguage = true;
		}
		if (personality.expLikeFeratio >= 15)
		{
			personality.likeFeratio = true;
		}
		if (personality.expLikeSperm >= 15)
		{
			personality.likeSperm = true;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		gages[0].fillAmount = Tween.Spring(gages[0].fillAmount, personality.ExpFeelVaginaRate, K, minSpeed);
		gages[1].fillAmount = Tween.Spring(gages[1].fillAmount, personality.ExpFeelAnusRate, K, minSpeed);
		gages[2].fillAmount = Tween.Spring(gages[2].fillAmount, personality.ExpIndecentLanguageRate, K, minSpeed);
		gages[3].fillAmount = Tween.Spring(gages[3].fillAmount, personality.ExpLikeFeratioRate, K, minSpeed);
		gages[4].fillAmount = Tween.Spring(gages[4].fillAmount, personality.ExpLikeSpermRate, K, minSpeed);
		for (int i = 0; i < gages.Length; i++)
		{
			bool active = gages[i].fillAmount >= 1f;
			unlocks[i].gameObject.SetActive(active);
		}
		if (!virgineChanged)
		{
			virginDelay -= Time.deltaTime;
			if (virginDelay <= 0f)
			{
				SetVergineText();
				virgineChanged = true;
			}
		}
	}

	private void SetVergineText()
	{
		vaginaText.text = ((!personality.vaginaVirgin) ? "性器非処女" : "性器処女");
		analText.text = ((!personality.analVirgin) ? "アナル非処女" : "アナル処女");
	}
}
