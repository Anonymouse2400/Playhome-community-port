using System;
using UnityEngine;

public class CutSceneEdit : MonoBehaviour
{
	public CutScene cutScene;

	public CutSceneEdit_TimeBar timeBar;

	public CutSceneEdit_Command command;

	private void Start()
	{
		cutScene.IsPlay = false;
	}
}
