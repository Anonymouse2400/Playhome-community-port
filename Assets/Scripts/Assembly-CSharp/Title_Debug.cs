using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Title_Debug : MonoBehaviour
{
	private Canvas canvas;

	private GameControl _gameCtrl;

	private GameControl GameCtrl
	{
		get
		{
			if (_gameCtrl == null)
			{
				_gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
			}
			return _gameCtrl;
		}
	}

	private void Start()
	{
		canvas = GetComponent<Canvas>();
	}

	private void Update()
	{
		if ((!(EventSystem.current != null) || !(EventSystem.current.currentSelectedGameObject != null)) && Input.GetKeyDown(KeyCode.Delete))
		{
			canvas.enabled = !canvas.enabled;
		}
	}

	public void SceneChange_H()
	{
		GameCtrl.ChangeScene("H", string.Empty);
	}

	public void SceneChange_ADV(string message)
	{
		GameCtrl.ChangeScene("ADVScene", message);
	}
}
