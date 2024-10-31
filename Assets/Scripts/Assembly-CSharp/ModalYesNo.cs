using System;
using UnityEngine;
using UnityEngine.UI;

public class ModalYesNo : ModalUI
{
   
    public Canvas myCanvas;

	public Text text;

	private Action yesAct;

	private Action noAct;

    //public AudioClip yesSE;

    //public AudioClip noSE;

    //protected override void Awake()
    //{
    //    base.Awake();

   // }

    public void Yes()
	{
		
		
			yesAct();
		
		//End();
	}

	public void No()
	{
		
		
			noAct();
		
		//End();
	}

	public void SetUp(string text, Action yesAct, Action noAct)
	{
		this.text.text = text;
		this.yesAct = yesAct;
		this.noAct = noAct;
	}

    private void Update()
    {
    }
    
}
