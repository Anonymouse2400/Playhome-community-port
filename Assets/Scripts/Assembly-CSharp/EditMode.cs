using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditMode : MonoBehaviour
{
	private enum TAB
	{
		NONE = -1,
		HAIR = 0,
		FACE = 1,
		BODY = 2,
		WEAR = 3,
		ACCE = 4,
		DATA = 5,
		NUM = 6
	}

	public string path_FemaleBody = "FemaleBody";

	public string path_MaleBody = "MaleBody";

	[SerializeField]
	private Canvas ui_canvas;

	[SerializeField]
	private FaceCustomEdit face;

	[SerializeField]
	private BodyCustomEdit body;

	[SerializeField]
	private HairCustomEdit hair;

	[SerializeField]
	private WearCustomEdit wear;

	[SerializeField]
	private AccessoryCustomEdit acce;

	[SerializeField]
	private DataCustomEdit data;

	[SerializeField]
	private ToggleButton[] toggles = new ToggleButton[6];

	[SerializeField]
	private GameObject[] tabMains = new GameObject[6];

	[SerializeField]
	private Button buttonOriginal;

	[SerializeField]
	private PresetListToggle presetListToggleOriginal;

	[SerializeField]
	private ItemChangeToggle itemChangeToggleOriginal;

	[SerializeField]
	private MoveableThumbnailSelectUI selectOriginal;

	[SerializeField]
	private DropDownUI dropdownUIOriginal;

	[SerializeField]
	private ColorChangeButton colorChangeButtonOriginal;

	[SerializeField]
	private SwitchUI switchUIOriginal;

	[SerializeField]
	private InputSliderUI inputSliderUIOriginal;

	[SerializeField]
	private RectTransform spaceOriginal;

	[SerializeField]
	private Text labelOriginal;

	[SerializeField]
	private MoveableGuideDriveUI guideDriveUI;

	[SerializeField]
	private CharaColorCopyHelper colorCopyHelper;

	private Human human;

	public List<CustomSelectSet> thumnbs_hair_set = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_hair_back = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_hair_side = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_hair_front = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_tops = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_bottoms = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_bra = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_shorts = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_swimwear = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_swimtops = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_swimbottoms = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_glove = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_panst = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_socks = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_wear_shoes = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_eyebrow = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_eyelash = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_eye = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_eyehighlight = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_nip = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_underhair = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_sunburn = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_bodyTattoo = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_faceType = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_faceSkin = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_faceDetail = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_bodySkin = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_bodyDetail = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_mole = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_eyeshadow = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_cheek = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_lip = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_faceTattoo = new List<CustomSelectSet>();

	public List<CustomSelectSet> thumnbs_beard = new List<CustomSelectSet>();

	public List<CustomSelectSet>[] thumnbs_acce = new List<CustomSelectSet>[12];

	private SmallThumnbnailRects smallRect = new SmallThumnbnailRects();

	[SerializeField]
	private CoordinateCapture coordinateCapture;

	[SerializeField]
	private Canvas backCardFrameCanvas;

	[SerializeField]
	private Canvas frontCardFrameCanvas;

	public Transform moveableRoot;

	public MoveableColorCustomUI colorUI;

	[SerializeField]
	private MoveableUI[] moveableUIs;

	[SerializeField]
	private MoveableThumbnailSelectUI itemSelectUI;

	private TAB nowTab = TAB.NONE;

	private GameControl gameCtrl;

	private bool show = true;

	public SEX Sex
	{
		get
		{
			return human.sex;
		}
	}

	private GameControl GameCtrl
	{
		get
		{
			if (gameCtrl == null)
			{
				gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
			}
			return gameCtrl;
		}
	}

	public void Setup(Human human, EditScene editScene)
	{
		this.human = human;
		coordinateCapture.SetHuman(human);
		SetupThumbs();
		EditEquipShow equipShow = ((!(editScene != null)) ? null : editScene.equipShow);
		hair.Setup(human, equipShow);
		face.Setup(human, equipShow);
		body.Setup(human, equipShow);
		wear.Setup(human, equipShow);
		acce.Setup(human, equipShow);
		data.Setup(human, equipShow);
		IllusionCamera illusionCamera = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
		backCardFrameCanvas.worldCamera = illusionCamera.GetComponent<Camera>();
		frontCardFrameCanvas.worldCamera = Transform_Utility.FindComponent<Camera>("UICamera");
		for (int i = 0; i < moveableUIs.Length; i++)
		{
			moveableUIs[i].transform.SetParent(moveableRoot, false);
		}
		colorCopyHelper.Setup(human);
	}

	private void Update()
	{
		ui_canvas.enabled = show && !GameCtrl.IsHideUI;
		TAB tAB = TAB.NONE;
		for (int i = 0; i < toggles.Length; i++)
		{
			if (toggles[i].Value)
			{
				tAB = (TAB)i;
			}
		}
		if (nowTab != tAB)
		{
			ChangeTab(tAB);
		}
	}

	private void ChangeTab(TAB tab)
	{
		nowTab = tab;
		for (int i = 0; i < toggles.Length; i++)
		{
			tabMains[i].SetActive(toggles[i].Value);
		}
		SystemSE.Play(SystemSE.SE.CHOICE);
	}

	private void SetupThumbs()
	{
		if (human.sex == SEX.FEMALE)
		{
			SetupThumbs_HairBack(ref thumnbs_hair_set, CustomDataManager.Hair_b, "thumnbnail/thumbnail_hairset", string.Empty, smallRect.hair_set, true);
			SetupThumbs_HairBack(ref thumnbs_hair_back, CustomDataManager.Hair_b, "thumnbnail/thumbnail_hairback", string.Empty, smallRect.hair_back, false);
			SetupThumbs(ref thumnbs_hair_side, CustomDataManager.Hair_s, "thumnbnail/thumbnail_hairside", string.Empty, smallRect.hair_side);
			SetupThumbs(ref thumnbs_hair_front, CustomDataManager.Hair_f, "thumnbnail/thumbnail_hairfront", string.Empty, smallRect.hair_front);
		}
		else
		{
			SetupThumbs_HairBack(ref thumnbs_hair_set, CustomDataManager.Hair_Male, "thumnbnail/thumbnail_hairmale", string.Empty, smallRect.hair_male, true);
		}
		if (human.sex == SEX.FEMALE)
		{
			SetupThumbs(ref thumnbs_wear_tops, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.TOP), "thumnbnail/thumbnail_weartop", string.Empty, smallRect.wear_tops);
			SetupThumbs(ref thumnbs_wear_bottoms, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.BOTTOM), "thumnbnail/thumbnail_wearbot", string.Empty, smallRect.wear_bottoms);
			SetupThumbs(ref thumnbs_wear_bra, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.BRA), "thumnbnail/thumbnail_wearbra", string.Empty, smallRect.wear_bra);
			SetupThumbs(ref thumnbs_wear_shorts, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.SHORTS), "thumnbnail/thumbnail_wearshorts", string.Empty, smallRect.wear_shorts);
			SetupThumbs(ref thumnbs_wear_swimwear, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.SWIM), "thumnbnail/thumbnail_wearswim", "swimtop|swimbot", smallRect.wear_swimwear);
			SetupThumbs(ref thumnbs_wear_swimtops, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.SWIM_TOP), "thumnbnail/thumbnail_wearswimtop", string.Empty, smallRect.wear_swimtops);
			SetupThumbs(ref thumnbs_wear_swimbottoms, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.SWIM_BOTTOM), "thumnbnail/thumbnail_wearswimbot", string.Empty, smallRect.wear_swimbottoms);
			SetupThumbs(ref thumnbs_wear_glove, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.GLOVE), "thumnbnail/thumbnail_wearglove", string.Empty, smallRect.wear_glove);
			SetupThumbs(ref thumnbs_wear_panst, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.PANST), "thumnbnail/thumbnail_wearpanst", string.Empty, smallRect.wear_panst);
			SetupThumbs(ref thumnbs_wear_socks, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.SOCKS), "thumnbnail/thumbnail_wearsocks", string.Empty, smallRect.wear_socks);
			SetupThumbs(ref thumnbs_wear_shoes, CustomDataManager.GetWearDictionary_Female(WEAR_TYPE.SHOES), "thumnbnail/thumbnail_wearshoes", string.Empty, smallRect.wear_shoes);
		}
		else
		{
			SetupThumbs(ref thumnbs_wear_tops, CustomDataManager.GetWearDictionary_Male(WEAR_TYPE.TOP), "thumnbnail/thumbnail_wearmaletop", string.Empty, smallRect.wear_tops);
			SetupThumbs(ref thumnbs_wear_shoes, CustomDataManager.GetWearDictionary_Male(WEAR_TYPE.SHOES), "thumnbnail/thumbnail_wearmaleshoes", string.Empty, new Rect(0f, 0f, 256f, 256f));
		}
		if (human.sex == SEX.FEMALE)
		{
			SetupThumbs_Head(ref thumnbs_faceType, CustomDataManager.Heads_Female, "thumnbnail/thumnbs_head", smallRect.faceType);
		}
		else
		{
			SetupThumbs_Head(ref thumnbs_faceType, CustomDataManager.Heads_Male, "thumnbnail/thumnbs_head_m", smallRect.faceType);
		}
		if (human.sex == SEX.FEMALE)
		{
			SetupThumbs(ref thumnbs_faceSkin, CustomDataManager.FaceSkins_Female, "thumnbnail/thumnbs_faceskin", smallRect.faceSkin);
			SetupThumbs(ref thumnbs_bodySkin, CustomDataManager.BodySkins_Female, "thumnbnail/thumnbs_bodyskin", smallRect.bodySkin);
			SetupThumbs_Tex(ref thumnbs_sunburn, CustomDataManager.Sunburn, "thumnbnail/thumnbs_sunburn", smallRect.sunburn);
			SetupThumbs_Tex(ref thumnbs_bodyTattoo, CustomDataManager.BodyTattoo_Female, "thumnbnail/thumnbs_bodytattoo_f", smallRect.bodytattoo);
			SetupThumbs(ref thumnbs_faceDetail, CustomDataManager.FaceDetails_Female, "thumnbnail/thumnbs_facedetail_f", smallRect.faceSkin);
			SetupThumbs(ref thumnbs_bodyDetail, CustomDataManager.BodyDetails_Female, "thumnbnail/thumnbs_bodydetail_f", smallRect.bodySkin);
			SetupThumbs(ref thumnbs_eyelash, CustomDataManager.Eyelash, "thumnbnail/thumnbs_eyelash", smallRect.eyelash);
			SetupThumbs(ref thumnbs_eyehighlight, CustomDataManager.Eyehighlight, "thumnbnail/thumnbs_eyehighlight", smallRect.eyehighlight);
			SetupThumbs(ref thumnbs_eyebrow, CustomDataManager.Eyebrow_Female, "thumnbnail/thumnbs_eyebrow", smallRect.eyebrow);
			SetupThumbs(ref thumnbs_eye, CustomDataManager.Eye_Female, "thumnbnail/thumnbs_eye", smallRect.eye);
			SetupThumbs_Tex(ref thumnbs_mole, CustomDataManager.Mole, "thumnbnail/thumnbs_mole", smallRect.mole);
			SetupThumbs_Tex(ref thumnbs_eyeshadow, CustomDataManager.EyeShadow, "thumnbnail/thumnbs_eyeshadow", smallRect.eyeshadow);
			SetupThumbs_Tex(ref thumnbs_cheek, CustomDataManager.Cheek, "thumnbnail/thumnbs_cheek", smallRect.cheek);
			SetupThumbs_Tex(ref thumnbs_lip, CustomDataManager.Lip, "thumnbnail/thumnbs_lip", smallRect.lip);
			SetupThumbs_Tex(ref thumnbs_faceTattoo, CustomDataManager.FaceTattoo_Female, "thumnbnail/thumnbs_faceTattoo", smallRect.faceTattoo);
			SetupThumbs(ref thumnbs_nip, CustomDataManager.Nip, "thumnbnail/thumnbs_nip", smallRect.nip);
			SetupThumbs(ref thumnbs_underhair, CustomDataManager.Underhair, "thumnbnail/thumnbs_underhair", smallRect.underhair);
		}
		else
		{
			SetupThumbs(ref thumnbs_faceSkin, CustomDataManager.FaceSkins_Male, "thumnbnail/thumnbs_faceskin_m", smallRect.faceSkin);
			SetupThumbs(ref thumnbs_bodySkin, CustomDataManager.BodySkins_Male, "thumnbnail/thumnbs_bodyskin_m", smallRect.bodySkin);
			SetupThumbs_Tex(ref thumnbs_bodyTattoo, CustomDataManager.BodyTattoo_Male, "thumnbnail/thumnbs_bodytattoo_m", smallRect.bodytattoo);
			SetupThumbs(ref thumnbs_faceDetail, CustomDataManager.FaceDetails_Male, "thumnbnail/thumnbs_facedetail_m", smallRect.faceSkin);
			SetupThumbs(ref thumnbs_bodyDetail, CustomDataManager.BodyDetails_Male, "thumnbnail/thumnbs_bodydetail_m", smallRect.bodySkin);
			SetupThumbs(ref thumnbs_eyebrow, CustomDataManager.Eyebrow_Male, "thumnbnail/thumnbs_eyebrow_m", smallRect.eyebrow);
			SetupThumbs(ref thumnbs_eye, CustomDataManager.Eye_Male, "thumnbnail/thumnbs_eye_m", smallRect.eye);
			SetupThumbs_Tex(ref thumnbs_faceTattoo, CustomDataManager.FaceTattoo_Male, "thumnbnail/thumnbs_faceTattoo_m", smallRect.faceTattoo);
			SetupThumbs(ref thumnbs_beard, CustomDataManager.Beard, "thumnbnail/thumnbs_beard", smallRect.beard);
		}
		string[] array = new string[12]
		{
			"thumnbnail/thumbnail_accehead", "thumnbnail/thumbnail_acceear", "thumnbnail/thumbnail_accemegane", "thumnbnail/thumbnail_acceface", "thumnbnail/thumbnail_acceneck", "thumnbnail/thumbnail_acceshoulder", "thumnbnail/thumbnail_accebreast", "thumnbnail/thumbnail_accewaist", "thumnbnail/thumbnail_acceback", "thumnbnail/thumbnail_accearm",
			"thumnbnail/thumbnail_accehand", "thumnbnail/thumbnail_acceleg"
		};
		for (int i = 0; i < 12; i++)
		{
			thumnbs_acce[i] = new List<CustomSelectSet>();
			ACCESSORY_TYPE type = (ACCESSORY_TYPE)i;
			SetupThumbs_Acce(ref thumnbs_acce[i], CustomDataManager.GetAccessoryDictionary(type), array[i], string.Empty, smallRect.acce[i]);
		}
	}

	private static void SetupThumbs<_T>(ref List<CustomSelectSet> list, Dictionary<int, _T> dictionary, string searchPath, string ignore, Rect smallRect) where _T : PrefabData
	{
		string fileName = Path.GetFileName(searchPath);
		string[] files = Directory.GetFiles("abdata/thumnbnail", fileName + "*", SearchOption.TopDirectoryOnly);
		string[] array = files;
		foreach (string text in array)
		{
			if (!Path.HasExtension(text) && (ignore.Length <= 0 || !Regex.IsMatch(text, ignore, RegexOptions.IgnoreCase)))
			{
				string assetBundle = "thumnbnail/" + Path.GetFileNameWithoutExtension(text);
				SetupThumbs(ref list, dictionary, assetBundle, smallRect);
			}
		}
	}

	private static void SetupThumbs<_T>(ref List<CustomSelectSet> list, Dictionary<int, _T> dictionary, string assetBundle, Rect smallRect) where _T : PrefabData
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", assetBundle);
		List<_T> list2 = new List<_T>();
		foreach (KeyValuePair<int, _T> item in dictionary)
		{
			list2.Add(item.Value);
		}
		list2.Sort((_T a, _T b) => a.order - b.order);
		foreach (_T item2 in list2)
		{
			list.Add(CreateData(item2, smallRect, assetBundleController));
		}
		assetBundleController.Close();
	}

	private static void SetupThumbs_HairBack(ref List<CustomSelectSet> list, Dictionary<int, BackHairData> dictionary, string searchPath, string ignore, Rect smallRect, bool isSet)
	{
		string fileName = Path.GetFileName(searchPath);
		string[] files = Directory.GetFiles("abdata/thumnbnail", fileName + "*", SearchOption.TopDirectoryOnly);
		string[] array = files;
		foreach (string text in array)
		{
			if (!Path.HasExtension(text) && (ignore.Length <= 0 || !Regex.IsMatch(text, ignore, RegexOptions.IgnoreCase)))
			{
				string assetBundle = "thumnbnail/" + Path.GetFileNameWithoutExtension(text);
				SetupThumbs_HairBack(ref list, dictionary, assetBundle, smallRect, isSet);
			}
		}
	}

	private static void SetupThumbs_HairBack(ref List<CustomSelectSet> list, Dictionary<int, BackHairData> dictionary, string assetBundle, Rect smallRect, bool isSet)
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", assetBundle);
		foreach (KeyValuePair<int, BackHairData> item in dictionary)
		{
			if (item.Value.isSet == isSet)
			{
				list.Add(CreateData(item.Value, smallRect, assetBundleController));
			}
		}
		assetBundleController.Close();
	}

	private static void SetupThumbs_Tex<_T>(ref List<CustomSelectSet> list, Dictionary<int, _T> dictionary, string searchPath, string ignore, Rect smallRect) where _T : CombineTextureData
	{
		string fileName = Path.GetFileName(searchPath);
		string[] files = Directory.GetFiles("abdata/thumnbnail", fileName + "*", SearchOption.TopDirectoryOnly);
		string[] array = files;
		foreach (string text in array)
		{
			if (!Path.HasExtension(text) && (ignore.Length <= 0 || !Regex.IsMatch(text, ignore, RegexOptions.IgnoreCase)))
			{
				string assetBundle = "thumnbnail/" + Path.GetFileNameWithoutExtension(text);
				SetupThumbs_Tex(ref list, dictionary, assetBundle, smallRect);
			}
		}
	}

	private static void SetupThumbs_Tex<_T>(ref List<CustomSelectSet> list, Dictionary<int, _T> dictionary, string assetBundle, Rect smallRect) where _T : CombineTextureData
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", assetBundle);
		foreach (KeyValuePair<int, _T> item in dictionary)
		{
			list.Add(CreateData(item.Value, smallRect, assetBundleController));
		}
		assetBundleController.Close();
	}

	private static void SetupThumbs_Head<_T>(ref List<CustomSelectSet> list, Dictionary<int, _T> dictionary, string searchPath, string ignore, Rect smallRect) where _T : HeadData
	{
		string fileName = Path.GetFileName(searchPath);
		string[] files = Directory.GetFiles("abdata/thumnbnail", fileName + "*", SearchOption.TopDirectoryOnly);
		string[] array = files;
		foreach (string text in array)
		{
			if (!Path.HasExtension(text) && (ignore.Length <= 0 || !Regex.IsMatch(text, ignore, RegexOptions.IgnoreCase)))
			{
				string assetBundle = "thumnbnail/" + Path.GetFileNameWithoutExtension(text);
				SetupThumbs_Head(ref list, dictionary, assetBundle, smallRect);
			}
		}
	}

	private static void SetupThumbs_Head<_T>(ref List<CustomSelectSet> list, Dictionary<int, _T> dictionary, string assetBundle, Rect smallRect) where _T : HeadData
	{
		AssetBundleController assetBundleController = new AssetBundleController();
		assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", assetBundle);
		foreach (KeyValuePair<int, _T> item in dictionary)
		{
			list.Add(CreateData(item.Value, smallRect, assetBundleController));
		}
		assetBundleController.Close();
	}

	private static void SetupThumbs_Acce<_T>(ref List<CustomSelectSet> list, Dictionary<int, _T> dictionary, string searchPath, string ignore, Rect smallRect) where _T : AccessoryData
	{
		string fileName = Path.GetFileName(searchPath);
		string[] files = Directory.GetFiles("abdata/thumnbnail", fileName + "*", SearchOption.TopDirectoryOnly);
		string[] array = files;
		foreach (string text in array)
		{
			if (Path.HasExtension(text) || (ignore.Length > 0 && Regex.IsMatch(text, ignore, RegexOptions.IgnoreCase)))
			{
				continue;
			}
			string assetBundleName = "thumnbnail/" + Path.GetFileNameWithoutExtension(text);
			AssetBundleController assetBundleController = new AssetBundleController();
			assetBundleController.OpenFromFile(Application.persistentDataPath + "/abdata", assetBundleName);
			foreach (KeyValuePair<int, _T> item in dictionary)
			{
				list.Add(CreateData(item.Value, smallRect, assetBundleController));
			}
			assetBundleController.Close();
		}
	}

	private static CustomSelectSet CreateData(PrefabData item, Rect smallRect, AssetBundleController abc)
	{
		Vector2 vector = new Vector2(256f, 256f);
		Texture2D texture = abc.LoadAsset<Texture2D>(item.prefab);
		Sprite thumbnail_L = Sprite.Create(texture, new Rect(Vector2.zero, vector), vector * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		Sprite thumbnail_S = Sprite.Create(texture, smallRect, smallRect.size * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		return new CustomSelectSet(item.id, item.name_LineFeed, thumbnail_S, thumbnail_L, item.isNew);
	}

	private static CustomSelectSet CreateData(CombineTextureData item, Rect smallRect, AssetBundleController abc)
	{
		Vector2 vector = new Vector2(256f, 256f);
		Texture2D texture = abc.LoadAsset<Texture2D>(item.textureName);
		Sprite thumbnail_L = Sprite.Create(texture, new Rect(Vector2.zero, vector), vector * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		Sprite thumbnail_S = Sprite.Create(texture, smallRect, smallRect.size * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		return new CustomSelectSet(item.id, item.name_LineFeed, thumbnail_S, thumbnail_L, item.isNew);
	}

	private static CustomSelectSet CreateData(HeadData item, Rect smallRect, AssetBundleController abc)
	{
		Vector2 vector = new Vector2(256f, 256f);
		Texture2D texture = abc.LoadAsset<Texture2D>(item.path);
		Sprite thumbnail_L = Sprite.Create(texture, new Rect(Vector2.zero, vector), vector * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		Sprite thumbnail_S = Sprite.Create(texture, smallRect, smallRect.size * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		return new CustomSelectSet(item.id, item.name_LineFeed, thumbnail_S, thumbnail_L, item.isNew);
	}

	private static CustomSelectSet CreateData(AccessoryData item, Rect smallRect, AssetBundleController abc)
	{
		Vector2 vector = new Vector2(256f, 256f);
		string prefab_F = item.prefab_F;
		Texture2D texture = abc.LoadAsset<Texture2D>(prefab_F);
		Sprite thumbnail_L = Sprite.Create(texture, new Rect(Vector2.zero, vector), vector * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		Sprite thumbnail_S = Sprite.Create(texture, smallRect, smallRect.size * 0.5f, 100f, 0u, SpriteMeshType.FullRect);
		return new CustomSelectSet(item.id, item.name_LineFeed, thumbnail_S, thumbnail_L, item.isNew);
	}

	public Button CreateButton(GameObject parent, string title, UnityAction onClick)
	{
		Button button = UnityEngine.Object.Instantiate(buttonOriginal);
		button.gameObject.SetActive(true);
		button.transform.SetParent(parent.transform, false);
		button.onClick.AddListener(onClick);
		button.gameObject.GetComponentInChildren<Text>().text = title;
		return button;
	}

	public MoveableThumbnailSelectUI CreateSelect(GameObject parent)
	{
		MoveableThumbnailSelectUI moveableThumbnailSelectUI = UnityEngine.Object.Instantiate(selectOriginal);
		moveableThumbnailSelectUI.gameObject.SetActive(true);
		moveableThumbnailSelectUI.transform.SetParent(parent.transform, false);
		moveableThumbnailSelectUI.Close();
		return moveableThumbnailSelectUI;
	}

	public ItemChangeToggle CreateItemToggle(GameObject parent)
	{
		ItemChangeToggle itemChangeToggle = UnityEngine.Object.Instantiate(itemChangeToggleOriginal);
		itemChangeToggle.gameObject.SetActive(true);
		itemChangeToggle.transform.SetParent(parent.transform, false);
		return itemChangeToggle;
	}

	public PresetListToggle CreatePresetListToggle(GameObject parent)
	{
		PresetListToggle presetListToggle = UnityEngine.Object.Instantiate(presetListToggleOriginal);
		presetListToggle.gameObject.SetActive(true);
		presetListToggle.transform.SetParent(parent.transform, false);
		return presetListToggle;
	}

	public ItemSelectUISets CreateItemSelectUISets(GameObject parent, string title, List<CustomSelectSet> setDatas, UnityAction<CustomSelectSet> act)
	{
		return new ItemSelectUISets(this, parent, itemSelectUI, title, setDatas, act);
	}

	public PresetSelectUISets CreatePresetSelectUISets(GameObject parent, string title, List<CustomSelectSet> setDatas, UnityAction<CustomSelectSet> act)
	{
		return new PresetSelectUISets(this, parent, itemSelectUI, title, setDatas, act);
	}

	public DropDownUI CreateDropDownUI(GameObject parent, string title, List<Dropdown.OptionData> options, UnityAction<int> act)
	{
		DropDownUI dropDownUI = UnityEngine.Object.Instantiate(dropdownUIOriginal);
		dropDownUI.gameObject.SetActive(true);
		dropDownUI.transform.SetParent(parent.transform, false);
		dropDownUI.SetTitle(title);
		dropDownUI.SetList(options);
		dropDownUI.AddOnValueChange(act);
		return dropDownUI;
	}

	public ColorChangeButton CreateColorChangeButton(GameObject parent, string title, Color color, bool hasAlpha, Action<Color> act)
	{
		ColorChangeButton colorChangeButton = UnityEngine.Object.Instantiate(colorChangeButtonOriginal);
		colorChangeButton.colorUI = colorUI;
		colorChangeButton.gameObject.SetActive(true);
		colorChangeButton.transform.SetParent(parent.transform, false);
		colorChangeButton.Setup(title, color, hasAlpha, act);
		return colorChangeButton;
	}

	public SwitchUI CreateSwitchUI(GameObject parent, string title, bool flag, UnityAction<bool> act)
	{
		SwitchUI switchUI = UnityEngine.Object.Instantiate(switchUIOriginal);
		switchUI.gameObject.SetActive(true);
		switchUI.transform.SetParent(parent.transform, false);
		switchUI.Setup(title, flag, act);
		return switchUI;
	}

	public InputSliderUI CreateInputSliderUI(GameObject parent, string title, float min, float max, bool hasDef, float defVal, UnityAction<float> onChange)
	{
		InputSliderUI inputSliderUI = UnityEngine.Object.Instantiate(inputSliderUIOriginal);
		inputSliderUI.gameObject.SetActive(true);
		inputSliderUI.transform.SetParent(parent.transform, false);
		inputSliderUI.Setup(title, min, max, hasDef, defVal, onChange);
		return inputSliderUI;
	}

	public RectTransform CreateSpace(GameObject parent)
	{
		RectTransform rectTransform = UnityEngine.Object.Instantiate(spaceOriginal);
		rectTransform.gameObject.SetActive(true);
		rectTransform.transform.SetParent(parent.transform, false);
		return rectTransform;
	}

	public Text CreateLabel(GameObject parent, string text)
	{
		Text text2 = UnityEngine.Object.Instantiate<Text>(labelOriginal);
		((Component)(object)text2).gameObject.SetActive(true);
		((Component)(object)text2).transform.SetParent(parent.transform, false);
		((Component)(object)text2).GetComponentInChildren<Text>().text = text;
		return text2;
	}

	public MoveableGuideDriveUI ShowMoveableGuideDriveUI(string title, Vector3 pos, Vector3 eul, Vector3 scl, UnityAction<MoveableUI.STATE> onChangeState, UnityAction<Vector3, Vector3, Vector3> onMove)
	{
		guideDriveUI.gameObject.SetActive(true);
		guideDriveUI.Setup(title, pos, eul, scl, onChangeState, onMove);
		guideDriveUI.Open();
		return guideDriveUI;
	}

	public void HideMoveableGuideDriveUI(Transform target)
	{
		guideDriveUI.Close();
		guideDriveUI.gameObject.SetActive(false);
	}

	public void HideMoveableGuideDriveUI_WithoutSE(Transform target)
	{
		guideDriveUI.gameObject.SetActive(false);
		guideDriveUI.Close();
	}

	public void ChangeFaceType(int no)
	{
		human.customParam.head.headID = no;
		human.Apply();
	}

	public void LoadedHuman()
	{
		face.LoadedHuman();
		body.LoadedHuman();
		wear.LoadedHuman();
		HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
		if (hWearAcceChangeUI != null)
		{
			hWearAcceChangeUI.CheckShowUI();
		}
	}

	public void LoadedCoordinate()
	{
		wear.LoadedCoordinate();
		HWearAcceChangeUI hWearAcceChangeUI = UnityEngine.Object.FindObjectOfType<HWearAcceChangeUI>();
		if (hWearAcceChangeUI != null)
		{
			hWearAcceChangeUI.CheckShowUI();
		}
	}

	public void ChangedColor()
	{
		hair.ChangedColor();
		face.ChangedColor();
		body.ChangedColor();
	}

	public void RecordCustomData()
	{
		CustomParameter customParameter = null;
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			customParameter = GlobalData.PlayData.GetHeroineCustomParam(female.HeroineID);
		}
		else
		{
			Male male = human as Male;
			customParameter = GlobalData.PlayData.GetMaleCustomParam(male.MaleID);
		}
		if (customParameter != null)
		{
			customParameter.Copy(human.customParam);
		}
	}

	public void RevertCustomData()
	{
		CustomParameter customParameter = null;
		if (human.sex == SEX.FEMALE)
		{
			Female female = human as Female;
			customParameter = GlobalData.PlayData.GetHeroineCustomParam(female.HeroineID);
		}
		else
		{
			Male male = human as Male;
			customParameter = GlobalData.PlayData.GetMaleCustomParam(male.MaleID);
		}
		if (customParameter != null)
		{
			human.customParam.Copy(customParameter);
		}
		human.Apply();
	}

	public void ShowUI(bool show)
	{
		this.show = show;
		ui_canvas.enabled = show && !GameCtrl.IsHideUI;
	}
}
