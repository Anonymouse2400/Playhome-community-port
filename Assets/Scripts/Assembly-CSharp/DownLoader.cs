using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DownLoader : LoaderBase
{
	private enum HAIR_TYPE
	{
		SHORT = 0,
		SEMI_LONG = 1,
		LONG = 2,
		PONY = 3,
		TWIN = 4,
		OTHER = 5,
		NUM = 6
	}

	private enum CHECK_STEP
	{
		CHECK_NO = 0,
		CHECK_LIST_WAIT = 1,
		CHECK_THUMBS_WAIT = 2
	}

	private enum SORT
	{
		NEW = 0,
		OLD = 1,
		RANK_TOTAL = 2,
		RANK_WEEK = 3,
		NUM = 4
	}

	private enum USER_FILTERING
	{
		ALL = 0,
		ME = 1,
		NOT_ME = 2,
		SPECIFIC_USER = 3
	}

	private const string MSG_GetAllList = "キャラリスト取得中...";

	private const string MSG_GetThumbs = "サムネイル取得中...";

	private const string MSG_DownLoad = "ダウンロード中...";

	private const string MSG_Delete = "削除中...";

	private CHECK_STEP check;

	[SerializeField]
	private Toggle[] cards;

	[SerializeField]
	private Text serverNumText;

	[SerializeField]
	private Text cacheSizeText;

	[SerializeField]
	private Button toUploaderButton;

	[SerializeField]
	private Text handleName;

	[SerializeField]
	private Text uploadNumText;

	[SerializeField]
	private Text comment;

	[SerializeField]
	private Button pageTop;

	[SerializeField]
	private Button pagePrev;

	[SerializeField]
	private Button pageNext;

	[SerializeField]
	private Button pageLast;

	[SerializeField]
	private Text pageText;

	[SerializeField]
	private InputField pageInput;

	[SerializeField]
	private Dropdown dateSortDropDown;

	[SerializeField]
	private Dropdown handleNameDropDown;

	[SerializeField]
	private Toggle maleToggle;

	[SerializeField]
	private Toggle femaleToggle;

	[SerializeField]
	private Toggle[] heightToggles = new Toggle[3];

	[SerializeField]
	private Toggle[] bustToggles = new Toggle[3];

	[SerializeField]
	private Toggle[] hairToggles = new Toggle[6];

	[SerializeField]
	private Button clearCheckButton;

	[SerializeField]
	private Button sameUserOtherCharaButton;

	[SerializeField]
	private Button downloadButton;

	[SerializeField]
	private Button deleteButton;

	[SerializeField]
	private Sprite noCardSprite;

	[SerializeField]
	private Sprite loadingSprite;

	[SerializeField]
	private Button deleteCachFileButton;

	private List<DLDataHeader> serverDatas = new List<DLDataHeader>();

	private Dictionary<string, int> userUploadNum = new Dictionary<string, int>();

	private List<DLDataHeader> showDatas = new List<DLDataHeader>();

	private DLDataHeader selData;

	private int selectCard = -1;

	private int nowPage = -1;

	private Dictionary<int, ThumbsCacheData> thumbsCache = new Dictionary<int, ThumbsCacheData>();

	private int cacheSize;

	private bool changeFilter;

	private string filterHandleName;

	private Dictionary<int, string> downloaded = new Dictionary<int, string>();

	private int memoryMaxCacheSizeMB = 1000;

	private int hardCacheSplitSizeMB = 100;

	private void Awake()
	{
		serverNumText.text = string.Empty;
		cacheSizeText.text = "ｷｬｯｼｭ:0MB";
		toUploaderButton.onClick.AddListener(SwapToUploader);
		for (int i = 0; i < cards.Length; i++)
		{
			int no = i;
			cards[i].onValueChanged.AddListener(delegate(bool flag)
			{
				OnCardToggle(no, flag);
			});
		}
		pageTop.onClick.AddListener(OnClickPageTop);
		pagePrev.onClick.AddListener(OnClickPagePrev);
		pageNext.onClick.AddListener(OnClickPageNext);
		pageLast.onClick.AddListener(OnClickPageLast);
		pageInput.onEndEdit.AddListener(OnPageInput);
		SetupDropdownOptions(dateSortDropDown, new string[4] { "新しい順", "古い順", "ランキング（総合）", "ランキング（週間）" });
		dateSortDropDown.onValueChanged.AddListener(delegate
		{
			OnChangeFilter();
		});
		handleNameDropDown.onValueChanged.AddListener(delegate
		{
			OnChangeFilter();
		});
		maleToggle.onValueChanged.AddListener(delegate
		{
			OnChangeFilter();
		});
		femaleToggle.onValueChanged.AddListener(delegate
		{
			OnChangeFilter();
		});
		for (int j = 0; j < heightToggles.Length; j++)
		{
			heightToggles[j].onValueChanged.AddListener(delegate
			{
				OnChangeFilter();
			});
		}
		for (int k = 0; k < bustToggles.Length; k++)
		{
			bustToggles[k].onValueChanged.AddListener(delegate
			{
				OnChangeFilter();
			});
		}
		for (int l = 0; l < hairToggles.Length; l++)
		{
			hairToggles[l].onValueChanged.AddListener(delegate
			{
				OnChangeFilter();
			});
		}
		downloadButton.onClick.AddListener(OnDownLoad);
		deleteButton.onClick.AddListener(OnDelete);
		handleNameDropDown.onValueChanged.AddListener(OnChangeUser);
		sameUserOtherCharaButton.onClick.AddListener(OnSameUserOtherChara);
		deleteCachFileButton.onClick.AddListener(OnDeleteThumbsCache);
		clearCheckButton.onClick.AddListener(OnClearCheck);
		LoadThumbsCache();
	}

	private void OnEnable()
	{
		GetAllList();
	}

	private void OnDestroy()
	{
		SaveThumbsCache();
	}

	private void Update()
	{
		if (check == CHECK_STEP.CHECK_NO)
		{
			if (changeFilter)
			{
				ChangeFilter();
			}
			downloadButton.interactable = selData != null;
			deleteButton.interactable = selData != null && selData.UserID == upDown.UserID;
			sameUserOtherCharaButton.interactable = selData != null || handleNameDropDown.value == 3;
		}
	}

	private static void SetupDropdownOptions(Dropdown dropDown, string[] names)
	{
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
		for (int i = 0; i < names.Length; i++)
		{
			list.Add(new Dropdown.OptionData(names[i]));
		}
		dropDown.options = list;
	}

	private void SetupHandleNameDropDown()
	{
		int num = ((filterHandleName == null) ? 3 : 4);
		string[] array = new string[num];
		array[0] = "全ての投稿者";
		array[1] = "自分";
		array[2] = "自分以外";
		if (num == 4)
		{
			array[3] = filterHandleName;
		}
		SetupDropdownOptions(handleNameDropDown, array);
	}

	private void GetAllList()
	{
		Debug.Log("GetAllList");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("mode", 7);
		netWWW.Post("http://up.illusion.jp/phome_upload/chara/master/unity/getData.php", "キャラリスト取得中...", wWWForm, GetAllList_Complete, GetAllList_Faild);
		check = CHECK_STEP.CHECK_LIST_WAIT;
	}

	private void GetAllList_Complete(WWW www)
	{
		serverDatas.Clear();
		userUploadNum.Clear();
		string[] array = www.text.Split('\n');
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < array.Length && !(array[i] == string.Empty); i++)
		{
			DLDataHeader dLDataHeader = AnalyzeListData(array[i]);
			if (dLDataHeader != null)
			{
				serverDatas.Add(dLDataHeader);
				if (dLDataHeader.Sex == 0)
				{
					num++;
				}
				else
				{
					num2++;
				}
				if (userUploadNum.ContainsKey(dLDataHeader.HandleName))
				{
					userUploadNum[dLDataHeader.HandleName]++;
				}
				else
				{
					userUploadNum.Add(dLDataHeader.HandleName, 1);
				}
			}
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		for (int j = 0; j < serverDatas.Count; j++)
		{
			DLDataHeader dLDataHeader2 = serverDatas[j];
			if (!dictionary.ContainsKey(dLDataHeader2.HandleName))
			{
				dictionary.Add(dLDataHeader2.HandleName, dLDataHeader2.UserID);
			}
			if (!dictionary2.ContainsKey(dLDataHeader2.UserID))
			{
				dictionary2.Add(dLDataHeader2.UserID, dLDataHeader2.HandleName);
			}
		}
		SetupHandleNameDropDown();
		serverNumText.text = "男:" + num + " / 女:" + num2 + "\n総キャラ数:" + serverDatas.Count;
		Text text = serverNumText;
		text.text = text.text + "\nハンドルネーム:" + dictionary.Count;
		check = CHECK_STEP.CHECK_NO;
		ChangeFilter();
		ChangePage(0, true);
	}

	private DLDataHeader AnalyzeListData(string n)
	{
		string[] array = n.Split('\t');
		if (string.Empty == array[0])
		{
			return null;
		}
		if (string.Empty == array[2])
		{
			return null;
		}
		if (string.Empty == array[3])
		{
			return null;
		}
		if (string.Empty == array[4])
		{
			return null;
		}
		if (string.Empty == array[5])
		{
			return null;
		}
		if (string.Empty == array[6])
		{
			return null;
		}
		if (string.Empty == array[11])
		{
			return null;
		}
		if (string.Empty == array[15])
		{
			return null;
		}
		int num = int.Parse(array[6]);
		if (num > 0)
		{
			return null;
		}
		DLDataHeader dLDataHeader = new DLDataHeader();
		dLDataHeader.UID = int.Parse(array[0]);
		dLDataHeader.BustSize = int.Parse(array[2]);
		dLDataHeader.BustSize = Mathf.Clamp(dLDataHeader.BustSize, 0, 2);
		dLDataHeader.BHType = int.Parse(array[3]);
		dLDataHeader.BHType = Mathf.Clamp(dLDataHeader.BHType, 0, 5);
		dLDataHeader.Sex = int.Parse(array[4]);
		dLDataHeader.Sex = Mathf.Clamp(dLDataHeader.Sex, 0, 1);
		dLDataHeader.Height = int.Parse(array[5]);
		dLDataHeader.Height = Mathf.Clamp(dLDataHeader.Height, 0, 2);
		dLDataHeader.__State = num;
		dLDataHeader.DLCount = int.Parse(array[11]);
		dLDataHeader.HandleName = Encoding.UTF8.GetString(Convert.FromBase64String(array[12]));
		dLDataHeader.Comment = Encoding.UTF8.GetString(Convert.FromBase64String(array[13]));
		dLDataHeader.UserID = array[14];
		dLDataHeader.WeekCount = int.Parse(array[15]);
		return dLDataHeader;
	}

	private void GetAllList_Faild(WWW www)
	{
		upDown.ShowMessage("データ一覧の取得に失敗しました");
		upDown.ChangeMode(UpDown.MODE.ERROR);
	}

	private void ChangeFilter()
	{
		showDatas.Clear();
		USER_FILTERING value = (USER_FILTERING)handleNameDropDown.value;
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		if (maleToggle.isOn || femaleToggle.isOn)
		{
			num = 0;
			num |= (maleToggle.isOn ? 1 : 0);
			num |= (femaleToggle.isOn ? 2 : 0);
		}
		if (CheckToggles(heightToggles))
		{
			num2 = TogglesFlag(heightToggles);
		}
		if (CheckToggles(bustToggles))
		{
			num3 = TogglesFlag(bustToggles);
		}
		if (CheckToggles(hairToggles))
		{
			num4 = TogglesFlag(hairToggles);
		}
		for (int i = 0; i < serverDatas.Count; i++)
		{
			DLDataHeader dLDataHeader = serverDatas[i];
			bool flag = true;
			if (value == USER_FILTERING.ME && dLDataHeader.UserID != upDown.UserID)
			{
				flag = false;
			}
			else if (value == USER_FILTERING.NOT_ME && dLDataHeader.UserID == upDown.UserID)
			{
				flag = false;
			}
			else if (value == USER_FILTERING.SPECIFIC_USER && dLDataHeader.HandleName != filterHandleName)
			{
				flag = false;
			}
			if ((num & (1 << dLDataHeader.Sex)) == 0)
			{
				flag = false;
			}
			if ((num2 != -1 || num3 != -1 || num4 != -1) && dLDataHeader.Sex == 0)
			{
				flag = false;
			}
			if (dLDataHeader.Sex == 1 && (num2 & (1 << dLDataHeader.Height)) == 0)
			{
				flag = false;
			}
			else if (dLDataHeader.Sex == 1 && (num3 & (1 << dLDataHeader.BustSize)) == 0)
			{
				flag = false;
			}
			else if (dLDataHeader.Sex == 1 && (num4 & (1 << dLDataHeader.BHType)) == 0)
			{
				flag = false;
			}
			if (flag)
			{
				showDatas.Add(dLDataHeader);
			}
		}
		if (dateSortDropDown.value == 0)
		{
			showDatas.Reverse();
		}
		else if (dateSortDropDown.value != 1)
		{
			if (dateSortDropDown.value == 2)
			{
				showDatas.Sort((DLDataHeader a, DLDataHeader b) => b.DLCount - a.DLCount);
			}
			else if (dateSortDropDown.value == 3)
			{
				showDatas.Sort((DLDataHeader a, DLDataHeader b) => b.WeekCount - a.WeekCount);
			}
		}
		changeFilter = false;
		ChangePage(0, true);
	}

	private static bool CheckToggles(Toggle[] toggles)
	{
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].isOn)
			{
				return true;
			}
		}
		return false;
	}

	private static int TogglesFlag(Toggle[] toggles)
	{
		int num = 0;
		for (int i = 0; i < toggles.Length; i++)
		{
			num |= (toggles[i].isOn ? (1 << i) : 0);
		}
		return num;
	}

	private void ChangePage(int newPage, bool forceUpdate)
	{
		if (nowPage != newPage || forceUpdate)
		{
			int num = PageNum();
			nowPage = Mathf.Clamp(newPage, 0, num - 1);
			UpdateThumbs(true);
			pageInput.text = (nowPage + 1).ToString();
			pageText.text = " / " + num;
			if (selectCard != -1)
			{
				cards[selectCard].isOn = false;
				selectCard = -1;
				selData = null;
			}
		}
	}

	private void UpdateThumbs(bool requestServer)
	{
		if (nowPage == -1)
		{
			return;
		}
		int num = nowPage * cards.Length;
		int num2 = Mathf.Min(showDatas.Count, num + cards.Length);
		int num3 = num2 - num;
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Length; i++)
		{
			if (num + i < num2)
			{
				cards[i].interactable = true;
				int uID = showDatas[num + i].UID;
				if (!UpdateCardTexture_FromCache(i, uID))
				{
					list.Add(uID);
					cards[i].image.sprite = loadingSprite;
				}
			}
			else
			{
				cards[i].interactable = false;
				cards[i].image.sprite = noCardSprite;
			}
		}
		if (requestServer && list.Count > 0)
		{
			GetThumbs(list);
		}
	}

	private void GetThumbs(List<int> requestIDs)
	{
		check = CHECK_STEP.CHECK_THUMBS_WAIT;
		string text = string.Empty;
		for (int i = 0; i < requestIDs.Count; i++)
		{
			text += requestIDs[i];
			text += "\n";
		}
		text = text.TrimEnd('\n');
		Debug.Log("GetThumbs");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("mode", 9);
		wWWForm.AddField("uid", text);
		netWWW.Post("http://up.illusion.jp/phome_upload/chara/master/unity/getData.php", "サムネイル取得中...", wWWForm, delegate(WWW www)
		{
			GetThumbs_Complete(www, requestIDs);
		}, delegate(WWW www)
		{
			upDown.PostError(www, "サムネイルの取得に失敗しました");
		});
	}

	private void GetThumbs_Complete(WWW www, List<int> requestIDs)
	{
		RecordReceiveThumbs(requestIDs, www.text);
		check = CHECK_STEP.CHECK_NO;
		UpdateThumbs(false);
	}

	private void RecordReceiveThumbs(List<int> requestIDs, string strData)
	{
		string[] array = strData.Split('\n');
		if (requestIDs.Count != array.Length)
		{
			Debug.LogError("請求したサムネイルと帰ってきたサムネイルの数が合いません");
			return;
		}
		for (int i = 0; i < requestIDs.Count; i++)
		{
			int id = requestIDs[i];
			byte[] data = Convert.FromBase64String(array[i]);
			RecordThumbs(id, data);
		}
		UpdateCacheSizeText();
	}

	private void UpdateCacheSizeText()
	{
		float a = (float)cacheSize * 0.001f * 0.001f;
		a = Mathf.Min(a, ConfigData.thumbsCacheSizeMB);
		cacheSizeText.text = "ｷｬｯｼｭ:" + a.ToString("0.00") + "MB";
	}

	private Texture2D PNGtoTex(byte[] data)
	{
		Texture2D texture2D = new Texture2D(0, 0, TextureFormat.ARGB32, false);
		texture2D.LoadImage(data);
		return texture2D;
	}

	private bool UpdateCardTexture_FromCache(int cardNo, int dataID)
	{
		byte[] array = null;
		Toggle toggle = cards[cardNo];
		Image image = toggle.image;
		if ((UnityEngine.Object)(object)image.sprite != null && (UnityEngine.Object)(object)image.sprite != (UnityEngine.Object)(object)noCardSprite && (UnityEngine.Object)(object)image.sprite != (UnityEngine.Object)(object)loadingSprite)
		{
			if (image.sprite.texture != null)
			{
				UnityEngine.Object.Destroy(image.sprite.texture);
			}
			UnityEngine.Object.Destroy((UnityEngine.Object)(object)image.sprite);
		}
		array = GetThumbsCach(dataID);
		if (array != null)
		{
			Vector2 vector = new Vector2(0f, 1f);
			Texture2D texture2D = PNGtoTex(array);
			Rect rect = new Rect(0f, 0f, texture2D.width, texture2D.height);
			Vector2 pivot = new Vector2(0f, 1f);
			image.sprite = Sprite.Create(texture2D, rect, pivot);
			return true;
		}
		image.sprite = null;
		return false;
	}

	private void RecordThumbs(int id, byte[] data)
	{
		if (!thumbsCache.ContainsKey(id))
		{
			long ticks = DateTime.Now.Ticks;
			RecordThumbs(id, new ThumbsCacheData(id, data, ticks));
		}
	}

	private void RecordThumbs(int id, ThumbsCacheData cache)
	{
		if (!thumbsCache.ContainsKey(id))
		{
			thumbsCache.Add(id, cache);
			cacheSize += cache.data.Length;
			int num = memoryMaxCacheSizeMB * 1024 * 1024;
			if (cacheSize > num)
			{
				CacheReduction();
			}
		}
	}

	private void CacheReduction()
	{
		int num = memoryMaxCacheSizeMB * 1024 * 1024;
		int num2 = (int)((float)num * 0.9f);
		List<ThumbsCacheData> list = new List<ThumbsCacheData>();
		foreach (ThumbsCacheData value in thumbsCache.Values)
		{
			list.Add(value);
		}
		list.Sort((ThumbsCacheData a, ThumbsCacheData b) => (int)(b.time - a.time));
		cacheSize = 0;
		thumbsCache.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			RecordThumbs(list[i].cardID, list[i]);
			if (cacheSize > num2)
			{
				break;
			}
		}
	}

	private byte[] GetThumbsCach(int cardID)
	{
		ThumbsCacheData value = null;
		if (thumbsCache.TryGetValue(cardID, out value))
		{
			long ticks = DateTime.Now.Ticks;
			value.time = ticks;
			return value.data;
		}
		return null;
	}

	private void SaveThumbsCache()
	{
		if (ConfigData.thumbsCacheSizeMB <= 0)
		{
			return;
		}
		string path = "./UserData/Thumbs/cache.dat";
		string directoryName = Path.GetDirectoryName(path);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		int num = ConfigData.thumbsCacheSizeMB * 1000 * 1000;
		int num2 = (int)((float)num * 0.9f);
		List<ThumbsCacheData> list = new List<ThumbsCacheData>();
		foreach (ThumbsCacheData value in thumbsCache.Values)
		{
			list.Add(value);
		}
		list.Sort((ThumbsCacheData a, ThumbsCacheData b) => (int)(b.time - a.time));
		int num3 = hardCacheSplitSizeMB * 1000 * 1000;
		List<int> list2 = new List<int>();
		list2.Add(0);
		int num4 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			int num5 = num4;
			num4 += list[i].data.Length;
			if (num4 > num2)
			{
				list.RemoveRange(i, list.Count - i);
				break;
			}
			if (num4 % num3 < num5 % num3)
			{
				list2.Add(i);
			}
		}
		list2.Add(list.Count);
		for (int j = 0; j < list2.Count - 1; j++)
		{
			int num6 = list2[j];
			int num7 = list2[j + 1] - num6;
			SaveThumbsCache_Data(list, num6, num7, j);
		}
	}

	private void SaveThumbsCache_Data(List<ThumbsCacheData> datas, int start, int num, int dataNo)
	{
		if (num == 0)
		{
			return;
		}
		string path = "./UserData/Thumbs/cache" + dataNo + ".dat";
		using (FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(output))
			{
				binaryWriter.Write(num);
				for (int i = 0; i < num; i++)
				{
					int index = start + i;
					datas[index].Save(binaryWriter);
				}
			}
		}
	}

	private void LoadThumbsCache()
	{
		int num = 0;
		while (true)
		{
			string path = "./UserData/Thumbs/cache" + num + ".dat";
			if (!File.Exists(path))
			{
				break;
			}
			using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					int num2 = binaryReader.ReadInt32();
					for (int i = 0; i < num2; i++)
					{
						ThumbsCacheData thumbsCacheData = new ThumbsCacheData();
						thumbsCacheData.Load(binaryReader);
						RecordThumbs(thumbsCacheData.cardID, thumbsCacheData);
					}
				}
			}
			num++;
		}
		UpdateCacheSizeText();
	}

	private void DeleteThumbsCache()
	{
		string[] files = Directory.GetFiles("./UserData/Thumbs/", "*.dat");
		string[] array = files;
		foreach (string path in array)
		{
			File.Delete(path);
		}
		cacheSize = 0;
		thumbsCache.Clear();
		UpdateCacheSizeText();
	}

	private void SelectCard(int no)
	{
		selectCard = no;
		selData = null;
		if (no != -1)
		{
			int num = no + nowPage * cards.Length;
			if (num >= 0 && num < showDatas.Count)
			{
				selData = showDatas[num];
			}
		}
		string text = string.Empty;
		if (selData != null)
		{
			int value = 0;
			userUploadNum.TryGetValue(selData.HandleName, out value);
			text = value.ToString();
		}
		handleName.text = ((selData == null) ? string.Empty : selData.HandleName);
		uploadNumText.text = text;
		comment.text = ((selData == null) ? string.Empty : selData.Comment);
	}

	private int PageNum()
	{
		int num = ((showDatas.Count % cards.Length != 0) ? 1 : 0);
		int a = showDatas.Count / cards.Length + num;
		return Mathf.Max(a, 1);
	}

	private void DownLoad()
	{
		if (selData == null)
		{
			return;
		}
		if (downloaded.ContainsKey(selData.UID))
		{
			ShowMessage("既にダウンロード済みです\u3000ファイル名:" + downloaded[selData.UID]);
			return;
		}
		Debug.Log("DownLoad");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("mode", 10);
		wWWForm.AddField("pid", selData.UID);
		if (selData.UserID != upDown.UserID)
		{
			wWWForm.AddField("dlflag", 1);
		}
		netWWW.Post("http://up.illusion.jp/phome_upload/chara/master/unity/getData.php", "ダウンロード中...", wWWForm, delegate(WWW www)
		{
			DownLoad_Complete(www, selData);
		}, delegate(WWW www)
		{
			upDown.PostError(www, "ダウンロードに失敗しました");
		});
	}

	private void DownLoad_Complete(WWW www, DLDataHeader selData)
	{
		string[] array = new string[2] { "./UserData/Chara/Male/", "./UserData/Chara/Female/" };
		string[] array2 = new string[2] { "charaM_", "charaF_" };
		string text = array2[selData.Sex] + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
		string path = array[selData.Sex] + text;
		byte[] buffer = Convert.FromBase64String(www.text);
		using (FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(output))
			{
				binaryWriter.Write(buffer);
			}
		}
		downloaded.Add(selData.UID, text);
		if (ConfigData.downloadCmpMsg)
		{
			string text2 = "ダウンロードが完了しました\nファイル名:" + text;
			upDown.GC.CreateModalMessageUI(text2, DownLoadCmpMsg, true);
		}
	}

	private void Delete()
	{
		if (selData != null)
		{
			string userID = upDown.UserID;
			Debug.Log("Delete");
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("mode", 8);
			wWWForm.AddField("pid", selData.UID);
			wWWForm.AddField("uid", userID);
			netWWW.Post("http://up.illusion.jp/phome_upload/chara/master/unity/getData.php", "削除中...", wWWForm, delegate(WWW www)
			{
				Delete_Complete(www);
			}, delegate(WWW www)
			{
				upDown.PostError(www, "削除に失敗しました");
			});
		}
	}

	private void Delete_Complete(WWW www)
	{
		ShowMessage("削除が完了しました");
		GetAllList();
	}

	private void DownLoadCmpMsg(bool doNotAgain)
	{
		if (doNotAgain)
		{
			ConfigData.downloadCmpMsg = false;
		}
	}

	private void OnCardToggle(int no, bool flag)
	{
		if (flag && no != selectCard)
		{
			SystemSE.SE sE = SystemSE.SE.CHOICE;
			SelectCard(no);
		}
		else if (!flag && no == selectCard)
		{
			SystemSE.SE sE = SystemSE.SE.NO;
			SelectCard(-1);
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void SwapToUploader()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		upDown.ChangeMode(UpDown.MODE.UPLOAD);
	}

	private void OnClickPageTop()
	{
		if (nowPage != 0)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			ChangePage(0, false);
		}
	}

	private void OnClickPagePrev()
	{
		if (nowPage > 0)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			ChangePage(nowPage - 1, false);
		}
	}

	private void OnClickPageNext()
	{
		if (nowPage < PageNum() - 1)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			ChangePage(nowPage + 1, false);
		}
	}

	private void OnClickPageLast()
	{
		if (nowPage != PageNum() - 1)
		{
			SystemSE.Play(SystemSE.SE.CHOICE);
			ChangePage(PageNum() - 1, false);
		}
	}

	private void OnChangeFilter()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		changeFilter = true;
	}

	private void OnDownLoad()
	{
		SystemSE.Play(SystemSE.SE.YES);
		DownLoad();
	}

	private void OnDelete()
	{
		SystemSE.Play(SystemSE.SE.YES);
		string text = "選択中のカードを削除します\nよろしいですか？";
		upDown.GC.CreateModalYesNoUI(text, Delete);
	}

	private void OnSameUserOtherChara()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		if (selData != null && handleNameDropDown.value != 3)
		{
			filterHandleName = selData.HandleName;
			SetupHandleNameDropDown();
			handleNameDropDown.value = 3;
			changeFilter = true;
		}
		else if (handleNameDropDown.value == 3)
		{
			filterHandleName = null;
			SetupHandleNameDropDown();
			handleNameDropDown.value = 0;
			changeFilter = true;
		}
	}

	private void OnChangeUser(int no)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		Text componentInChildren = sameUserOtherCharaButton.GetComponentInChildren<Text>();
		componentInChildren.text = ((no != 3) ? "この投稿者のキャラをみる" : "全ての投稿者のキャラをみる");
	}

	private void OnPageInput(string str)
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		try
		{
			int num = int.Parse(str);
			ChangePage(num - 1, false);
		}
		catch
		{
			pageInput.text = (nowPage + 1).ToString();
		}
	}

	private void OnDeleteThumbsCache()
	{
		SystemSE.Play(SystemSE.SE.YES);
		string text = "サムネイルキャッシュを削除します\nよろしいですか？";
		upDown.GC.CreateModalYesNoUI(text, DeleteThumbsCache);
	}

	private void OnClearCheck()
	{
		SystemSE.Play(SystemSE.SE.CHOICE);
		maleToggle.isOn = false;
		femaleToggle.isOn = false;
		for (int i = 0; i < heightToggles.Length; i++)
		{
			heightToggles[i].isOn = false;
		}
		for (int j = 0; j < bustToggles.Length; j++)
		{
			bustToggles[j].isOn = false;
		}
		for (int k = 0; k < hairToggles.Length; k++)
		{
			hairToggles[k].isOn = false;
		}
	}
}
