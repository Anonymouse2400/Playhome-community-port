using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UpDown : Scene
{
	public enum MODE
	{
		WAIT = 0,
		CHECK = 1,
		DOWNLOAD = 2,
		UPLOAD = 3,
		ERROR = 4,
		NUM = 5
	}

	private enum CHECK_STEP
	{
		CHECK_WAIT = 0,
		CHECK_SERVER_WAIT = 1,
		CHECK_SERVER_COMPLETE = 2
	}

	private const string MSG_NetWorkCheck = "サーバー接続確認中...";

	private NetWWW netWWW;

	private MODE mode = MODE.CHECK;

	private CHECK_STEP checkStep;

	[SerializeField]
	private GameObject[] modeRoot = new GameObject[5];

	[SerializeField]
	private Button modeSwapButton;

	public const int LastVersion = 0;

	public string UserID { get; private set; }

	private void Awake()
	{
		netWWW = GetComponent<NetWWW>();
	}

	private void Start()
	{
		CreateUUID();
		UserID = GetUUID();
		InScene();
		mode = MODE.WAIT;
	}

	private void Update()
	{
		if (mode == MODE.WAIT && inFade == null)
		{
			ChangeMode(MODE.CHECK);
			NetWorkCheck();
		}
		if (mode == MODE.CHECK && checkStep == CHECK_STEP.CHECK_SERVER_COMPLETE)
		{
			ChangeMode(MODE.DOWNLOAD);
		}
	}

	public void NetWorkCheck()
	{
		Debug.Log("NetWorkCheck");
		checkStep = CHECK_STEP.CHECK_SERVER_WAIT;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("mode", 100);
		netWWW.Post("http://up.illusion.jp/phome_upload/chara/master/unity/getData.php", "サーバー接続確認中...", wWWForm, NetWorkCheck_Complete, delegate(WWW www)
		{
			PostError(www, "サーバーへの接続に失敗しました");
		});
	}

	public void PostError(WWW www, string msg)
	{
		ShowMessage(msg);
		ChangeMode(MODE.ERROR);
	}

	private void NetWorkCheck_Complete(WWW www)
	{
		if (www.text == "1")
		{
			Debug.Log("NetWorkCheck_Complete");
			checkStep = CHECK_STEP.CHECK_SERVER_COMPLETE;
		}
		else
		{
			ShowMessage("アップロードサービスは終了しました");
			ChangeMode(MODE.ERROR);
		}
	}

	public void ChangeMode(MODE next)
	{
		mode = next;
		for (int i = 0; i < 5; i++)
		{
			modeRoot[i].gameObject.SetActive(i == (int)next);
		}
		if (mode == MODE.CHECK)
		{
			modeSwapButton.gameObject.SetActive(false);
			checkStep = CHECK_STEP.CHECK_WAIT;
		}
		else if (mode == MODE.DOWNLOAD)
		{
			modeSwapButton.gameObject.SetActive(true);
			modeSwapButton.GetComponentInChildren<Text>().text = "アップローダー";
		}
		else if (mode == MODE.UPLOAD)
		{
			modeSwapButton.gameObject.SetActive(true);
			modeSwapButton.GetComponentInChildren<Text>().text = "ダウンローダー";
		}
		else if (mode == MODE.ERROR)
		{
			modeSwapButton.gameObject.SetActive(false);
		}
	}

	public void ShowMessage(string message)
	{
		base.GC.CreateModalMessageUI(message);
	}

	private static void CreateUUID()
	{
		string dataPath = Application.persistentDataPath;
		dataPath = Path.GetDirectoryName(dataPath);
		string path = dataPath + "/UserData/netUID.dat";
		if (File.Exists(path))
		{
			using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					string text = binaryReader.ReadString();
					if (text != string.Empty)
					{
						return;
					}
				}
			}
		}
		string directoryName = Path.GetDirectoryName(path);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		string value = Guid.NewGuid().ToString();
		using (FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(output))
			{
				binaryWriter.Write(value);
			}
		}
	}

	private static string GetUUID()
	{
		string dataPath = Application.persistentDataPath;
		dataPath = Path.GetDirectoryName(dataPath);
		string path = dataPath + "/UserData/netUID.dat";
		if (File.Exists(path))
		{
			using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					return binaryReader.ReadString();
				}
			}
		}
		return string.Empty;
	}

	public void OnTitleButton()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		base.GC.CreateModalYesNoUI("タイトルに戻ります\nよろしいですか？", ReturnTitle);
	}

	private void ReturnTitle()
	{
		base.GC.ChangeScene("TitleScene", string.Empty);
	}
}
