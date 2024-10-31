using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class UpLoader : LoaderBase
{
	private enum HASH_CHECK
	{
		OK = 0,
		SAME_CARD = 1,
		SAME_MY_CARD = 2
	}

	private const string MSG_Uploading = "アップロード中...";

	private const string MSG_CheckHash = "ハッシュ確認...";

	[SerializeField]
	private Button upDownloaderButton;

	[SerializeField]
	private Button checkRuleButton;

	[SerializeField]
	private GameObject ruleRoot;

	[SerializeField]
	private Button uploadButton;

	[SerializeField]
	private InputField handleNameInput;

	[SerializeField]
	private InputField commentInput;

	[SerializeField]
	private UploaderCardFileList charaList;

	[SerializeField]
	private string def_handleName = "名も無き紳士";

	private List<ServerHashInfo> hashInfos = new List<ServerHashInfo>();

	private void Awake()
	{
		upDownloaderButton.onClick.AddListener(SwapToDownloader);
		checkRuleButton.onClick.AddListener(OnCheckRule);
		uploadButton.onClick.AddListener(OnUpload);
		handleNameInput.text = GlobalData.uploaderHandleName;
		handleNameInput.onEndEdit.AddListener(OnHandleName);
	}

	private void Start()
	{
		if (!GlobalData.showUploaderRule)
		{
			ruleRoot.gameObject.SetActive(true);
			GlobalData.showUploaderRule = true;
		}
		CheckServerHash();
	}

	private void Update()
	{
		Card_Data selectedCard = charaList.GetSelectedCard();
		uploadButton.interactable = selectedCard != null && handleNameInput.text.Length > 0;
	}

	private void Upload()
	{
		Card_Data selectedCard = charaList.GetSelectedCard();
		if (selectedCard == null || selectedCard.charaInfo == null)
		{
			return;
		}
		string text = handleNameInput.text;
		if (text.Length == 0)
		{
			return;
		}
		string text2 = selectedCard.charaInfo.name;
		if (text2.Length > 16)
		{
			text2 = text2.Substring(0, 16);
		}
		byte[] array = LoadData(selectedCard.file);
		if (array == null)
		{
			return;
		}
		string value = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
		string value2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(commentInput.text));
		string value3 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text2));
		string hashAll = GetHashAll(array);
		string hashParam = GetHashParam(array);
		Debug.Log("Hash all:" + hashAll + " param:" + hashParam);
		CustomParameter customParameter = new CustomParameter(charaList.Sex);
		customParameter.Load(selectedCard.file, true, true);
		int value4 = (int)(customParameter.body.GetBustSize() / 0.333f);
		value4 = Mathf.Clamp(value4, 0, 2);
		int i = (int)((charaList.Sex == SEX.FEMALE) ? CustomDataManager.GetHair_Back(customParameter.hair.parts[0].ID).type : BackHairData.HAIR_TYPE.SHORT);
		int value5 = (int)(customParameter.body.GetHeight() / 0.333f);
		value5 = Mathf.Clamp(value5, 0, 2);
		int pid = -1;
		int i2 = ((charaList.Sex != SEX.MALE) ? 1 : 0);
		string userID = upDown.UserID;
		if (charaList.Sex == SEX.MALE)
		{
			i = 255;
			value5 = 255;
			value4 = 255;
		}
		if (customParameter.CheckWrongParam())
		{
			ShowMessage("不正なデータです\nファイルが破損しているか\n不正に改変されたデータの可能性があります\nこのデータはアップロード出来ません");
			return;
		}
		if (CheckUploadHash(hashAll, hashParam, ref pid) != 0)
		{
			ShowMessage("既に同じデータがアップロードされています");
			return;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("mode", 5);
		wWWForm.AddField("pid", pid);
		wWWForm.AddField("uid", userID);
		wWWForm.AddField("handlename", value);
		wWWForm.AddField("products_description", value2);
		wWWForm.AddField("basuto", value4);
		wWWForm.AddField("hair", i);
		wWWForm.AddField("sex", i2);
		wWWForm.AddField("shinchou", value5);
		wWWForm.AddField("title", value3);
		wWWForm.AddBinaryData("png", array);
		wWWForm.AddField("hash_png", hashAll);
		wWWForm.AddField("hash_param", hashParam);
		wWWForm.AddField("state", 0);
		wWWForm.AddField("seikaku", 0);
		wWWForm.AddField("resistH", 0);
		wWWForm.AddField("resistPain", 0);
		wWWForm.AddField("resistAnal", 0);
		netWWW.Post("http://up.illusion.jp/phome_upload/chara/master/unity/getData.php", "アップロード中...", wWWForm, delegate(WWW www)
		{
			Upload_Complete(www);
		}, delegate(WWW www)
		{
			upDown.PostError(www, "アップロードに失敗しました");
		});
	}

	private static byte[] LoadData(string file)
	{
		byte[] array = null;
		using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
		{
			array = new byte[fileStream.Length];
			using (BinaryReader binaryReader = new BinaryReader(fileStream))
			{
				return binaryReader.ReadBytes(array.Length);
			}
		}
	}

	private HASH_CHECK CheckUploadHash(string hashPng, string hashParam, ref int pid)
	{
		for (int i = 0; i < hashInfos.Count; i++)
		{
			if (hashInfos[i].hashpng == hashPng)
			{
				return HASH_CHECK.SAME_CARD;
			}
			if (hashInfos[i].hashparam == hashParam)
			{
				if (hashInfos[i].userId == upDown.UserID)
				{
					pid = hashInfos[i].cardID;
					return HASH_CHECK.SAME_MY_CARD;
				}
				return HASH_CHECK.SAME_CARD;
			}
		}
		return HASH_CHECK.OK;
	}

	private void Upload_Complete(WWW www)
	{
		CheckServerHash();
		ShowMessage("アップロードが完了しました");
	}

	private void CheckServerHash()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("mode", 0);
		netWWW.Post("http://up.illusion.jp/phome_upload/chara/master/unity/getData.php", "ハッシュ確認...", wWWForm, delegate(WWW www)
		{
			CheckServerHash_Complete(www);
		}, delegate(WWW www)
		{
			upDown.PostError(www, "ハッシュデータの取得に失敗しました");
		});
	}

	private void CheckServerHash_Complete(WWW www)
	{
		hashInfos.Clear();
		string[] array = www.text.Split('\n');
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (string.Empty == text)
			{
				break;
			}
			string[] array3 = text.Split('\t');
			if (array3.Length >= 3 && !(string.Empty == array3[0]) && !(string.Empty == array3[1]) && !(string.Empty == array3[2]))
			{
				int cardID = int.Parse(array3[0]);
				hashInfos.Add(new ServerHashInfo(cardID, array3[1], array3[2], array3[3]));
			}
		}
	}

	public static string GetHashAll(byte[] data)
	{
		byte[] hashBytes = CreateSha256(data, "CreateHash");
		return HashBytesToString(hashBytes);
	}

	public string GetHashParam(byte[] data)
	{
		int num = (int)PNG_Loader.CheckSize(data);
		int num2 = data.Length - num;
		byte[] array = new byte[num2];
		Buffer.BlockCopy(data, num, array, 0, num2);
		byte[] hashBytes = CreateSha256(array, "CreateHash");
		return HashBytesToString(hashBytes);
	}

	private static byte[] CreateSha256(byte[] data, string key)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(key);
		HMACSHA256 hMACSHA = new HMACSHA256(bytes);
		return hMACSHA.ComputeHash(data);
	}

	private static string HashBytesToString(byte[] hashBytes)
	{
		if (hashBytes == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in hashBytes)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private void SwapToDownloader()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		upDown.ChangeMode(UpDown.MODE.DOWNLOAD);
	}

	private void OnCheckRule()
	{
		SystemSE.Play(SystemSE.SE.YES);
		ruleRoot.gameObject.SetActive(true);
	}

	private void OnUpload()
	{
		SystemSE.Play(SystemSE.SE.YES);
		string text = "選択中のカードをアップロードします\nよろしいですか？";
		upDown.GC.CreateModalYesNoUI(text, Upload);
	}

	private void OnHandleName(string str)
	{
		GlobalData.uploaderHandleName = str;
	}
}
