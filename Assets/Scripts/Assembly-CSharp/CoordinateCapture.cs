using System;
using Character;
using UnityEngine;

public class CoordinateCapture : MonoBehaviour
{
	[SerializeField]
	private Camera capCamera;

	[SerializeField]
	private int superSize = 2;

	[SerializeField]
	private int saveWidth = 252;

	[SerializeField]
	private int saveHeight = 352;

	[SerializeField]
	private Mannequin mannequin_F;

	[SerializeField]
	private Mannequin mannequin_M;

	private Mannequin mannequin;

	private Human human;

	private void Start()
	{
		capCamera.enabled = false;
	}

	private void OnDestroy()
	{
	}

	public Texture2D Capture()
	{
		capCamera.enabled = true;
		int num = saveWidth * superSize;
		int num2 = saveHeight * superSize;
		RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 24);
		capCamera.targetTexture = temporary;
		capCamera.Render();
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, num, num2), 0, 0);
		texture2D.Apply();
		TextureScale.Bilinear(texture2D, saveWidth, saveHeight);
		capCamera.targetTexture = null;
		RenderTexture.ReleaseTemporary(temporary);
		capCamera.enabled = false;
		return texture2D;
	}

	public void Save(string file)
	{
		mannequin.FromHuman(human);
		human.SaveCoordinate(file, Capture());
		mannequin.Strip();
	}

	public void SetHuman(Human human)
	{
		this.human = human;
		if (human.sex == SEX.MALE)
		{
			capCamera.transform.localPosition = new Vector3(0f, 0.92f, 4.4f);
			capCamera.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
			mannequin = mannequin_M;
			mannequin_M.gameObject.SetActive(true);
			mannequin_F.gameObject.SetActive(false);
		}
		else
		{
			capCamera.transform.localPosition = new Vector3(0f, 0.87f, 4.2999997f);
			capCamera.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
			mannequin = mannequin_F;
			mannequin_M.gameObject.SetActive(false);
			mannequin_F.gameObject.SetActive(true);
		}
	}
}
