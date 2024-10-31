using UnityEngine;
using UnityEngine.UI;

public class UploaderScene : MonoBehaviour
{
	public enum MODE
	{
		UPLOAD = 0,
		DOWNLOAD = 1,
		NUM = 2
	}

	[SerializeField]
	private Color[] bg_colors = new Color[2];

	[SerializeField]
	private Image bg;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
