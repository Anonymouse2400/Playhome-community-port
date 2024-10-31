using System;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Card_Data
{
	private enum STATE
	{
		NO_TEX = 0,
		LOADING = 1,
		LOAD_END = 2,
		HAS_TEX = 3
	}

	public string file;

	public byte[] buf;

	public CharaInfo charaInfo = new CharaInfo();

	public bool show;

	public bool isSelect;

	private bool isChangeTex;

	private Texture2D tex;

	private Texture2D noLoadTex;

	private Thread thread;

	private STATE state;

	public bool HasTex
	{
		get
		{
			return state == STATE.HAS_TEX;
		}
	}

	public Card_Data(string file, Texture2D noLoadTex)
	{
		this.file = file;
		tex = null;
		this.noLoadTex = noLoadTex;
		charaInfo.Load(file);
		state = STATE.NO_TEX;
	}

	public void Delete()
	{
		buf = null;
		if (thread != null && thread.IsAlive)
		{
			thread.Abort();
		}
		if (tex != null)
		{
			UnityEngine.Object.Destroy(tex);
			tex = null;
		}
	}

	private void LoadedTex()
	{
		if (state == STATE.NO_TEX && tex == null && (thread == null || !thread.IsAlive))
		{
			state = STATE.LOADING;
			thread = new Thread(LoadPNG);
			thread.Start();
		}
	}

	private void LoadPNG()
	{
		buf = null;
		FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
		if (fileStream == null)
		{
			state = STATE.NO_TEX;
			return;
		}
		using (BinaryReader binaryReader = new BinaryReader(fileStream))
		{
			try
			{
				buf = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				buf = null;
			}
			binaryReader.Close();
		}
		state = STATE.LOAD_END;
	}

	private void CreateTex()
	{
		if (state == STATE.LOAD_END)
		{
			tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			tex.LoadImage(buf);
			buf = null;
			state = STATE.HAS_TEX;
			if ((float)tex.width > CardFileList.PNG_Size.x || (float)tex.height > CardFileList.PNG_Size.y)
			{
				TextureScale.Bilinear(tex, (int)CardFileList.PNG_Size.x, (int)CardFileList.PNG_Size.y);
			}
			isChangeTex = true;
		}
	}

	private void ForcedLoadPNG()
	{
		if (thread != null && thread.IsAlive)
		{
			thread.Abort();
		}
		buf = null;
		FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
		if (fileStream == null)
		{
			state = STATE.NO_TEX;
			return;
		}
		using (BinaryReader binaryReader = new BinaryReader(fileStream))
		{
			try
			{
				buf = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				buf = null;
			}
			binaryReader.Close();
		}
		tex = new Texture2D(1, 1);
		tex.LoadImage(buf);
		if ((float)tex.width > CardFileList.PNG_Size.x || (float)tex.height > CardFileList.PNG_Size.y)
		{
			TextureScale.Bilinear(tex, (int)CardFileList.PNG_Size.x, (int)CardFileList.PNG_Size.y);
		}
		buf = null;
		state = STATE.HAS_TEX;
		isChangeTex = true;
	}

	private void ReleaseTex()
	{
		if (state != 0 && !isSelect)
		{
			state = STATE.NO_TEX;
			buf = null;
			if (thread != null && thread.IsAlive)
			{
				thread.Abort();
			}
			if (tex != null)
			{
				UnityEngine.Object.Destroy(tex);
				tex = null;
				isChangeTex = true;
			}
		}
	}

	public Texture2D Texture(bool forcedLoad = false)
	{
		if (tex != null)
		{
			return tex;
		}
		if (forcedLoad)
		{
			ForcedLoadPNG();
			return tex;
		}
		return noLoadTex;
	}

	public void UpdateSprite(Image image)
	{
		if (show || isSelect)
		{
			LoadedTex();
		}
		else
		{
			ReleaseTex();
		}
		if (state == STATE.LOAD_END)
		{
			CreateTex();
		}
		if (isChangeTex)
		{
			Texture2D texture2D = Texture();
			Vector2 pNG_Size = CardFileList.PNG_Size;
			if (texture2D != null)
			{
				pNG_Size.x = texture2D.width;
				pNG_Size.y = texture2D.height;
			}
			Rect rect = new Rect(Vector2.zero, pNG_Size);
			Vector2 pivot = pNG_Size * 0.5f;
			image.sprite = Sprite.Create(Texture(), rect, pivot, 100f, 0u, SpriteMeshType.FullRect);
			isChangeTex = false;
		}
	}

	public void ChangeTex()
	{
		isChangeTex = true;
	}
}
