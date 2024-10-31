using System;
using System.Collections;
using System.IO;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class DataCustomEdit : MonoBehaviour
{
	public enum TAB
	{
		NONE = -1,
		CHARA = 0,
		CHARA_CAPTURE = 1,
		WEAR = 2,
		NUM = 3
	}

	[SerializeField]
	private ToggleButton toggleChara;

	[SerializeField]
	private ToggleButton toggleWear;

	[SerializeField]
	private GameObject[] tabs = new GameObject[3];

	[SerializeField]
	private EditMode editMode;

	private EditEquipShow equipShow;

	private Human human;

	private TAB nowTab = TAB.NONE;

	[SerializeField]
	private GameObject cardRoot;

	[SerializeField]
	private Image cardBack;

	[SerializeField]
	private Image cardFront;

	[SerializeField]
	private Text backCardSelText;

	[SerializeField]
	private Text frontCardSelText;

	[SerializeField]
	private CoordinateCapture coordinateCapture;

	[SerializeField]
	private CardFileList caraList;

	[SerializeField]
	private CardFileList equipList;

	[SerializeField]
	private CharaSaveUI saveUI;

	private string[] list_Back;

	private string[] list_Front;

	private int backCard;

	private int frontCard;

	private Texture2D captureTex;

	private string charaOverwriteFile;

	public void Setup(Human human, EditEquipShow equipShow)
	{
		this.human = human;
		this.equipShow = equipShow;
		caraList.Seup(CardFileList.TYPE.CHARA, human.sex);
		equipList.Seup(CardFileList.TYPE.COORDINATE, human.sex);
		list_Back = Directory.GetFiles("UserData/CardFrame/Back", "*.png", SearchOption.AllDirectories);
		Array.Sort(list_Back);
		list_Front = Directory.GetFiles("UserData/CardFrame/Front", "*.png", SearchOption.AllDirectories);
		Array.Sort(list_Front);
		GameObject[] array = tabs;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(false);
		}
		SetCard_Back(backCard);
		SetCard_Front(frontCard);
	}

	private void OnEnable()
	{
		if (equipShow != null)
		{
			equipShow.SetAuto(EditEquipShow.WEARSHOW.ALL);
		}
	}

	private void OnDestroy()
	{
		if (captureTex != null)
		{
			UnityEngine.Object.Destroy(captureTex);
			captureTex = null;
		}
	}

	private void Update()
	{
	}

	public void ChangeToggles(int no)
	{
		switch (no)
		{
		case 0:
			ChangeTab(TAB.CHARA);
			break;
		case 1:
			ChangeTab(TAB.WEAR);
			break;
		default:
			ChangeTab(TAB.NONE);
			break;
		}
	}

	public void ChangeTab(TAB tab)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		nowTab = tab;
		for (int i = 0; i < 3; i++)
		{
			tabs[i].SetActive(nowTab == (TAB)i);
		}
		cardRoot.SetActive(tabs[1].activeInHierarchy);
	}

	public void Chara_Save(string file)
	{
		if (file == string.Empty)
		{
			charaOverwriteFile = string.Empty;
			ChangeTab(TAB.CHARA_CAPTURE);
			return;
		}
		GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
		string[] choices = new string[3] { "カードを撮影して上書き", "カードをそのままに上書き", "キャンセル" };
		Action[] acts = new Action[3]
		{
			delegate
			{
				Chara_SaveOverwriteCapture(file);
			},
			delegate
			{
				CharaOverwriteSaveExe(file);
			},
			null
		};
		gameControl.CreateModalChoices("上書きしますか？", choices, acts);
	}

	public void Chara_Load(string file, int filter)
	{
		LOAD_MSG lOAD_MSG = human.Load(file, filter);
		editMode.LoadedHuman();
		GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
		LOAD_MSG lOAD_MSG2 = (LOAD_MSG)12;
		if ((lOAD_MSG & LOAD_MSG.ISOMERISM) != 0)
		{
			gameControl.CreateModalMessageUI("異性のデータでした。\nカードファイルを入れるフォルダをご確認ください");
		}
		else if ((lOAD_MSG & lOAD_MSG2) != 0 && ConfigData.anotherGameCardMessage)
		{
			string text = (((lOAD_MSG & LOAD_MSG.VER_SEXYBEACH) == 0) ? "ハニーセレクト" : "セクシービーチ");
			string text2 = text + "で作成されたデータをロードしました。\n";
			text2 += "ゲーム毎の表現方法の違いにより、色の設定等が一部引き継げない場合や\n";
			text2 += "同じ設定でも見た目が異なってしまう場合があります。\n";
			text2 += "大変お手数ですが、再度設定をお願い致します。\n";
			gameControl.CreateModalMessageUI(text2, LoadMsgOK_Version, true);
		}
	}

	private void LoadMsgOK_Version(bool doNotAgain)
	{
		if (doNotAgain)
		{
			ConfigData.anotherGameCardMessage = false;
		}
	}

	public void Coordinate_Save(string file)
	{
		if (file == string.Empty)
		{
			CoordinateSaveName();
			return;
		}
		GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
		gameControl.CreateModalYesNoUI("上書きしますか？", delegate
		{
			CoordinateSaveExe(file);
		});
	}

	public void Coordinate_Load(string file, int filter)
	{
		human.LoadCoordinate(file, filter);
		editMode.LoadedCoordinate();
	}

	private void SetCard_Back(int no)
	{
		string file = string.Empty;
		string text = "カードなし";
		if (list_Back.Length > 0)
		{
			backCard = (list_Back.Length + no) % list_Back.Length;
			file = list_Back[backCard];
			text = backCard + "/" + list_Back.Length;
		}
		else
		{
			cardBack.enabled = false;
		}
		LoadImage(cardBack, file);
		backCardSelText.text = text;
	}

	public void MoveCard_Back(int move)
	{
		SetCard_Back(backCard + move);
	}

	public void SwitchCard_Back(bool flag)
	{
		cardBack.enabled = list_Back.Length > 0 && flag;
	}

	private void SetCard_Front(int no)
	{
		string empty = string.Empty;
		string text = "カードなし";
		if (list_Front.Length > 0)
		{
			frontCard = (list_Front.Length + no) % list_Front.Length;
			empty = list_Front[frontCard];
			LoadImage(cardFront, empty);
			text = frontCard + "/" + list_Front.Length;
		}
		else
		{
			cardFront.enabled = false;
		}
		frontCardSelText.text = text;
	}

	public void MoveCard_Front(int move)
	{
		SetCard_Front(frontCard + move);
	}

	public void SwitchCard_Front(bool flag)
	{
		cardFront.enabled = list_Front.Length > 0 && flag;
	}

	private static Texture2D LoadPNG(string file)
	{
		byte[] array = null;
		FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
		if (fileStream == null)
		{
			return null;
		}
		using (BinaryReader binaryReader = new BinaryReader(fileStream))
		{
			try
			{
				array = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				array = null;
			}
			binaryReader.Close();
		}
		if (array == null)
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2D.LoadImage(array);
		array = null;
		return texture2D;
	}

	private void LoadImage(Image image, string file)
	{
		Texture2D texture2D = LoadPNG(file);
		Vector2 vector = new Vector2(texture2D.width, texture2D.height);
		Rect rect = new Rect(Vector2.zero, vector);
		Vector2 pivot = vector * 0.5f;
		if (texture2D != null)
		{
			image.sprite = Sprite.Create(texture2D, rect, pivot, 100f, 0u, SpriteMeshType.FullRect);
		}
		else
		{
			image.sprite = null;
		}
	}

	public void CharaCaptureButton()
	{
		SystemSE.Play(SystemSE.SE.YES);
		StartCoroutine(ReadPixEndFrame());
	}

	private IEnumerator ReadPixEndFrame()
	{
		editMode.moveableRoot.gameObject.SetActive(false);
		ShortCut_Help shortcutHelp = UnityEngine.Object.FindObjectOfType<ShortCut_Help>();
		if ((bool)shortcutHelp)
		{
			shortcutHelp.gameObject.SetActive(false);
		}
		yield return new WaitForEndOfFrame();
		int saveWidth = 252;
		int saveHeight = 352;
		float aspect = (float)saveWidth / (float)saveHeight;
		float w = (float)Screen.height * aspect;
		Texture2D tex2D = new Texture2D((int)w, Screen.height, TextureFormat.RGB24, false);
		tex2D.filterMode = FilterMode.Point;
		tex2D.wrapMode = TextureWrapMode.Clamp;
		tex2D.ReadPixels(new Rect
		{
			x = (float)Screen.width * 0.5f - w * 0.5f,
			y = 0f,
			width = w,
			height = Screen.height
		}, 0, 0, false);
		tex2D.Apply();
		TextureScale.Bilinear(tex2D, saveWidth, saveHeight);
		editMode.moveableRoot.gameObject.SetActive(true);
		if ((bool)shortcutHelp)
		{
			shortcutHelp.gameObject.SetActive(true);
		}
		captureTex = tex2D;
		CharaSaveName();
	}

	private void CharaSaveName()
	{
		if (charaOverwriteFile == string.Empty)
		{
			saveUI.Save_New(captureTex, CharaSaveFile, CharaSaveCancel);
			return;
		}
		saveUI.Save_Overwrite(captureTex, delegate
		{
			CharaSaveExe(charaOverwriteFile);
		}, CharaSaveCancel);
	}

	private void CharaSaveCancel()
	{
		if (captureTex != null)
		{
			UnityEngine.Object.Destroy(captureTex);
			captureTex = null;
		}
	}

	private void CharaSaveFile(string name)
	{
		string file = "UserData\\Chara\\";
		file += ((human.sex != 0) ? "male\\" : "female\\");
		file = file + name + ".png";
        //string text = Directory.GetCurrentDirectory() + file;
        string text = Application.persistentDataPath + file;
        if (text.Length >= 246)
		{
			GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
			gameControl.CreateModalMessageUI("名前が長すぎます", delegate
			{
				CharaSaveName();
			});
		}
		else if (File.Exists(file))
		{
			GameControl gameControl2 = UnityEngine.Object.FindObjectOfType<GameControl>();
			gameControl2.CreateModalYesNoUI("同名ファイルがあります\n上書きしますか？", delegate
			{
				CharaSaveExe(file);
			}, CharaSaveName);
		}
		else
		{
			CharaSaveExe(file);
		}
		charaOverwriteFile = string.Empty;
	}

	private void CharaSaveExe(string path)
	{
		human.Save(path, captureTex);
		if (captureTex != null)
		{
			UnityEngine.Object.Destroy(captureTex);
			captureTex = null;
		}
		toggleChara.Value = false;
		caraList.Replacement(true);
	}

	private void CharaOverwriteSaveExe(string path)
	{
		Texture2D texture2D = LoadPNG(path);
		human.Save(path, texture2D);
		if (texture2D != null)
		{
			UnityEngine.Object.Destroy(texture2D);
			captureTex = null;
		}
		toggleChara.Value = false;
		caraList.Replacement(true);
	}

	private void Chara_SaveOverwriteCapture(string file)
	{
		charaOverwriteFile = file;
		ChangeTab(TAB.CHARA_CAPTURE);
	}

	private void CoordinateSaveName()
	{
		GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
		gameControl.CreateModalInputStringUI(true, "保存するファイル名を入力してください", CoordinateSaveFile, null, false, string.Empty);
	}

	private void CoordinateSaveFile(string name)
	{
		string file = "UserData\\coordinate\\";
		file += ((human.sex != 0) ? "male\\" : "female\\");
		file = file + name + ".png";
        //string text = Directory.GetCurrentDirectory() + file;
        string text = Application.persistentDataPath + file;
        if (text.Length >= 246)
		{
			GameControl gameControl = UnityEngine.Object.FindObjectOfType<GameControl>();
			gameControl.CreateModalMessageUI("名前が長すぎます", delegate
			{
				CoordinateSaveName();
			});
		}
		else if (File.Exists(file))
		{
			GameControl gameControl2 = UnityEngine.Object.FindObjectOfType<GameControl>();
			gameControl2.CreateModalYesNoUI("同名ファイルがあります\n上書きしますか？", delegate
			{
				CoordinateSaveExe(file);
			}, CoordinateSaveName);
		}
		else
		{
			CoordinateSaveExe(file);
		}
	}

	private void CoordinateSaveExe(string path)
	{
		coordinateCapture.Save(path);
		toggleWear.Value = false;
		equipList.Replacement(true);
	}
}
