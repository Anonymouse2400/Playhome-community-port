using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000105 RID: 261
public class BackGroundChangeUI : MonoBehaviour
{
    // Token: 0x06000552 RID: 1362 RVA: 0x00021F34 File Offset: 0x00020334
    private void Awake()
    {
        this.illCam = UnityEngine.Object.FindObjectOfType<IllusionCamera>();
        this.cam = this.illCam.GetComponent<Camera>();
        this.invoke = false;
        for (int i = 0; i < this.toggles.Length; i++)
        {
            this.toggles[i].onValueChanged.AddListener(new UnityAction<bool>(this.OnChangeToggle));
        }
        this.SetupMap();
        this.colorPicker.Setup("背景色", ConfigData.clearColor, false, new Action<Color>(this.OnChangeColor));
        this.SetupSkyBox();
        this.SetupPictures();
        this.toggles[(int)this.nowType].isOn = true;
        this.ChangeTab();
        this.invoke = true;
    }

    // Token: 0x06000553 RID: 1363 RVA: 0x00021FF0 File Offset: 0x000203F0
    private void SetupMap()
    {
        List<string> list = new List<string>();
        list.Add("夫婦の寝室");
        list.Add("律子の部屋");
        list.Add("明子の部屋");
        list.Add("リビング");
        list.Add("風呂");
        list.Add("和室");
        list.Add("洗面所");
        list.Add("玄関(内)");
        list.Add("トイレ");
        list.Add("玄関(外)");
        this.mapDropDown.options.Clear();
        this.mapDropDown.captionText.text = "マップ";
        this.mapDropDown.AddOptions(list);
        this.mapDropDown.onValueChanged.AddListener(new UnityAction<int>(this.OnChangeMap));
        for (int i = 0; i < this.mapTimes.Length; i++)
        {
            this.mapTimes[i].onValueChanged.AddListener(new UnityAction<bool>(this.OnChangeMapTime));
        }
    }

    // Token: 0x06000554 RID: 1364 RVA: 0x000220F8 File Offset: 0x000204F8
    private void SetupSkyBox()
    {
        this.skyboxList.Clear();
        this.skyboxDropDown.options.Clear();
        TextAsset textAsset = AssetBundleLoader.LoadAsset<TextAsset>(GlobalData.assetBundlePath, "skybox", "skybox_list");
        CustomDataListLoader customDataListLoader = new CustomDataListLoader();
        customDataListLoader.Load(textAsset);
        for (int i = 0; i < customDataListLoader.GetDataNum(); i++)
        {
            string data = customDataListLoader.GetData(0, i);
            string data2 = customDataListLoader.GetData(1, i);
            this.skyboxList.Add(data2);
            this.skyboxDropDown.options.Add(new Dropdown.OptionData(data));
        }
        this.skyboxDropDown.onValueChanged.AddListener(new UnityAction<int>(this.OnChangeSkybox));
    }

    // Token: 0x06000555 RID: 1365 RVA: 0x000221AC File Offset: 0x000205AC
    private void SetupPictures()
    {
        this.pictureButton_next.onClick.AddListener(delegate
        {
            this.PicturePageMove(1);
        });
        this.pictureButton_prev.onClick.AddListener(delegate
        {
            this.PicturePageMove(-1);
        });
        this.pictureButton_next5.onClick.AddListener(delegate
        {
            this.PicturePageMove(5);
        });
        this.pictureButton_prev5.onClick.AddListener(delegate
        {
            this.PicturePageMove(-5);
        });
        for (int i = 0; i < this.pictureButtons.Length; i++)
        {
            int no = i;
            this.pictureButtons[i].onClick.AddListener(delegate
            {
                this.OnClickPicture(no);
            });
        }
        this.pictures = Directory.GetFiles("UserData/Background", "*.png", SearchOption.AllDirectories);
        this.PicturePageMove(0);
    }

    // Token: 0x06000556 RID: 1366 RVA: 0x00022294 File Offset: 0x00020694
    private void PicturePage()
    {
        int num = this.nowPicturePage;
        for (int i = 0; i < 9; i++)
        {
            int num2 = num * 9 + i;
            if (num2 < this.pictures.Length)
            {
                this.LoadImage_Thumbs(this.pictureButtons[i].image, this.pictures[num2]);
                this.pictureButtons[i].interactable = true;
            }
            else
            {
                this.pictureButtons[i].image.sprite = null;
                this.pictureButtons[i].interactable = false;
            }
        }
    }

    // Token: 0x06000557 RID: 1367 RVA: 0x00022320 File Offset: 0x00020720
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
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                array = null;
            }
            binaryReader.Close();
        }
        if (array == null)
        {
            return null;
        }
        Texture2D texture2D = new Texture2D(1, 1);
        texture2D.LoadImage(array);
        array = null;
        return texture2D;
    }

    // Token: 0x06000558 RID: 1368 RVA: 0x000223C0 File Offset: 0x000207C0
    private void LoadImage_Thumbs(Image image, string file)
    {
        Texture2D texture2D = BackGroundChangeUI.LoadPNG(file);
        TextureScale.Bilinear(texture2D, 64, 40);
        Vector2 vector = new Vector2(64f, 40f);
        Rect rect = new Rect(Vector2.zero, vector);
        Vector2 vector2 = vector * 0.5f;
        if (texture2D != null)
        {
            image.sprite = Sprite.Create(texture2D, rect, vector2, 100f, 0U, SpriteMeshType.FullRect);
        }
        else
        {
            image.sprite = null;
        }
    }

    // Token: 0x06000559 RID: 1369 RVA: 0x00022438 File Offset: 0x00020838
    private void LoadImage_Raw(Image image, string file)
    {
        Texture2D texture2D = BackGroundChangeUI.LoadPNG(file);
        Vector2 vector = new Vector2((float)texture2D.width, (float)texture2D.height);
        Rect rect = new Rect(Vector2.zero, vector);
        Vector2 vector2 = vector * 0.5f;
        if (texture2D != null)
        {
            image.sprite = Sprite.Create(texture2D, rect, vector2, 100f, 0U, SpriteMeshType.FullRect);
        }
        else
        {
            image.sprite = null;
        }
    }

    // Token: 0x0600055A RID: 1370 RVA: 0x000224A8 File Offset: 0x000208A8
    private void PicturePageMove(int move)
    {
        int num = this.pictures.Length / 9;
        if (num > 0 && this.pictures.Length % 9 == 0)
        {
            num--;
        }
        num++;
        this.nowPicturePage = (num + this.nowPicturePage + move) % num;
        this.picturePageText.text = this.nowPicturePage + 1 + " / " + num;
        this.PicturePage();
    }

    // Token: 0x0600055B RID: 1371 RVA: 0x00022520 File Offset: 0x00020920
    private void OnClickPicture(int no)
    {
        int num = this.nowPicturePage * 9 + no;
        bool flag;
        if (no < this.pictures.Length)
        {
            this.LoadImage_Raw(this.backImage, this.pictures[num]);
            flag = true;
        }
        else
        {
            flag = false;
        }
        this.backImage.enabled = flag;
    }

    // Token: 0x0600055C RID: 1372 RVA: 0x00022574 File Offset: 0x00020974
    private void Update()
    {
        if (this.changeToggle)
        {
            this.ChangeTab();
            this.changeToggle = false;
        }
        if (this.changeMapTime)
        {
            this.ChangeMap();
            this.changeMapTime = false;
        }
        if (this.nowType == BackGroundChangeUI.TYPE.PICTURE)
        {
            this.UpdatePicture();
        }
        else
        {
            this.backImage.enabled = false;
        }
    }

    // Token: 0x0600055D RID: 1373 RVA: 0x000225D4 File Offset: 0x000209D4
    private void UpdatePicture()
    {
        if (!this.backImage.enabled || this.backImage.sprite.texture == null)
        {
            return;
        }
        bool isOn = this.pictureToggle_fitW.isOn;
        bool isOn2 = this.pictureToggle_fitH.isOn;
        int width = this.backImage.sprite.texture.width;
        int height = this.backImage.sprite.texture.height;
        Vector2 vector = new Vector2((float)width, (float)height);
        if (isOn && isOn2)
        {
            vector.x = (float)Screen.width;
            vector.y = (float)Screen.height;
        }
        else if (isOn)
        {
            float num = vector.y / vector.x;
            vector.x = (float)Screen.width;
            vector.y = vector.x * num;
        }
        else if (isOn2)
        {
            float num2 = vector.x / vector.y;
            vector.y = (float)Screen.height;
            vector.x = vector.y * num2;
        }
        this.backImage.rectTransform.sizeDelta = vector;
    }

    // Token: 0x0600055E RID: 1374 RVA: 0x0002270C File Offset: 0x00020B0C
    private void ChangeTab()
    {
        for (int i = 0; i < this.toggles.Length; i++)
        {
            this.mains[i].SetActive(this.toggles[i].isOn);
            if (this.toggles[i].isOn)
            {
                this.nowType = (BackGroundChangeUI.TYPE)i;
            }
        }
        if (this.nowType == BackGroundChangeUI.TYPE.OBJ)
        {
            this.cam.clearFlags = CameraClearFlags.Skybox;
            this.ChangeMap();
        }
        else if (this.nowType == BackGroundChangeUI.TYPE.SKYBOX)
        {
            this.cam.clearFlags = CameraClearFlags.Skybox;
            this.cam.backgroundColor = ConfigData.clearColor;
            this.OnChangeSkybox(this.skyboxDropDown.value);
        }
        else if (this.nowType == BackGroundChangeUI.TYPE.PICTURE)
        {
            this.cam.clearFlags = CameraClearFlags.Color;
            this.cam.backgroundColor = ConfigData.clearColor;
            this.backImage.enabled = this.backImage.sprite != null;
        }
        else if (this.nowType == BackGroundChangeUI.TYPE.COLOR)
        {
            this.cam.clearFlags = CameraClearFlags.Color;
            this.cam.backgroundColor = ConfigData.clearColor;
        }
        if (this.nowType != BackGroundChangeUI.TYPE.PICTURE)
        {
            this.backImage.enabled = false;
        }
        if (this.map != null)
        {
            this.map.gameObject.SetActive(this.nowType == BackGroundChangeUI.TYPE.OBJ);
        }
        bool flag = this.map != null && this.map.gameObject.activeInHierarchy;
        this.defRefProbe.gameObject.SetActive(!flag);
        if (this.nowType != BackGroundChangeUI.TYPE.OBJ)
        {
            this.noMapLight.gameObject.SetActive(true);
            this.noMapLight.color = Color.HSVToRGB(0f, 0f, 0.8235294f);
            this.noMapLight.intensity = 1f;
            this.lightCtrl.SetupLight(this.noMapLight.transform, false);
        }
        else
        {
            this.noMapLight.gameObject.SetActive(false);
        }
    }

    // Token: 0x0600055F RID: 1375 RVA: 0x0002292C File Offset: 0x00020D2C
    private void ChangeMap()
    {
        int value = this.mapDropDown.value;
        int num = 0;
        for (int i = 0; i < this.mapTimes.Length; i++)
        {
            if (this.mapTimes[i].isOn)
            {
                num = i;
                break;
            }
        }
        string[] array = new string[] { "bedroom", "ritsuko_room", "akiko_room", "living", "bathroom", "japanese", "poweder", "entrance", "toilet", "yard" };
        string[] array2 = new string[] { "day", "evening", "night_light", "night_dark" };
        string text = array[value] + "_" + array2[num];
        if (this.map != null)
        {
            UnityEngine.Object.Destroy(this.map.gameObject);
        }
        string text2 = "map/" + text;
        AssetBundleController assetBundleController = new AssetBundleController();
        assetBundleController.OpenFromFile(GlobalData.assetBundlePath, text2);
        GameObject gameObject = assetBundleController.LoadAndInstantiate<GameObject>(text);
        this.map = gameObject.GetComponent<Map>();
        this.map.name = text;
        LightMapControl componentInChildren = this.map.GetComponentInChildren<LightMapControl>();
        componentInChildren.Apply();
        assetBundleController.Close(false);
        this.lightCtrl.SetupLight(this.map.lightRoot, false);
        Resources.UnloadUnusedAssets();
    }

    // Token: 0x06000560 RID: 1376 RVA: 0x00022AB0 File Offset: 0x00020EB0
    private void OnChangeToggle(bool flag)
    {
        if (!this.invoke)
        {
            return;
        }
        if (flag)
        {
            this.changeToggle = true;
            SystemSE.Play(SystemSE.SE.CHOICE);
        }
    }

    // Token: 0x06000561 RID: 1377 RVA: 0x00022AD4 File Offset: 0x00020ED4
    private void OnChangeSkybox(int no)
    {
        if (no >= 0 && no < this.skyboxList.Count)
        {
            string text = this.skyboxList[no];
            Material material = AssetBundleLoader.LoadAsset<Material>(GlobalData.assetBundlePath, "skybox", text);
            Skybox component = this.illCam.GetComponent<Skybox>();
            component.material = material;
            Resources.UnloadUnusedAssets();
        }
    }

    // Token: 0x06000562 RID: 1378 RVA: 0x00022B30 File Offset: 0x00020F30
    private void OnChangeMap(int no)
    {
        this.ChangeMap();
    }

    // Token: 0x06000563 RID: 1379 RVA: 0x00022B38 File Offset: 0x00020F38
    private void OnChangeMapTime(bool flag)
    {
        if (!this.invoke)
        {
            return;
        }
        if (flag)
        {
            this.changeMapTime = true;
            SystemSE.Play(SystemSE.SE.CHOICE);
        }
    }

    // Token: 0x06000564 RID: 1380 RVA: 0x00022B59 File Offset: 0x00020F59
    private void OnChangeColor(Color color)
    {
        ConfigData.clearColor = color;
        this.cam.backgroundColor = color;
    }

    // Token: 0x0400060D RID: 1549
    [SerializeField]
    private Light noMapLight;

    // Token: 0x0400060E RID: 1550
    [SerializeField]
    private LightController lightCtrl;

    // Token: 0x0400060F RID: 1551
    [SerializeField]
    private Toggle[] toggles;

    // Token: 0x04000610 RID: 1552
    [SerializeField]
    private GameObject[] mains;

    // Token: 0x04000611 RID: 1553
    [SerializeField]
    private Dropdown mapDropDown;

    // Token: 0x04000612 RID: 1554
    [SerializeField]
    private Toggle[] mapTimes;

    // Token: 0x04000613 RID: 1555
    [SerializeField]
    private Dropdown skyboxDropDown;

    // Token: 0x04000614 RID: 1556
    [SerializeField]
    private ColorChangeButton colorPicker;

    // Token: 0x04000615 RID: 1557
    [SerializeField]
    private Button[] pictureButtons;

    // Token: 0x04000616 RID: 1558
    [SerializeField]
    private Text picturePageText;

    // Token: 0x04000617 RID: 1559
    [SerializeField]
    private Button pictureButton_next;

    // Token: 0x04000618 RID: 1560
    [SerializeField]
    private Button pictureButton_prev;

    // Token: 0x04000619 RID: 1561
    [SerializeField]
    private Button pictureButton_next5;

    // Token: 0x0400061A RID: 1562
    [SerializeField]
    private Button pictureButton_prev5;

    // Token: 0x0400061B RID: 1563
    [SerializeField]
    private Toggle pictureToggle_fitW;

    // Token: 0x0400061C RID: 1564
    [SerializeField]
    private Toggle pictureToggle_fitH;

    // Token: 0x0400061D RID: 1565
    [SerializeField]
    private Image backImage;

    // Token: 0x0400061E RID: 1566
    [SerializeField]
    private ReflectionProbe defRefProbe;

    // Token: 0x0400061F RID: 1567
    private bool changeToggle;

    // Token: 0x04000620 RID: 1568
    private bool changeMapTime;

    // Token: 0x04000621 RID: 1569
    private bool invoke = true;

    // Token: 0x04000622 RID: 1570
    private BackGroundChangeUI.TYPE nowType = BackGroundChangeUI.TYPE.SKYBOX;

    // Token: 0x04000623 RID: 1571
    private IllusionCamera illCam;

    // Token: 0x04000624 RID: 1572
    private Camera cam;

    // Token: 0x04000625 RID: 1573
    private Map map;

    // Token: 0x04000626 RID: 1574
    private List<string> skyboxList = new List<string>();

    // Token: 0x04000627 RID: 1575
    private string[] pictures;

    // Token: 0x04000628 RID: 1576
    private int nowPicturePage;

    // Token: 0x02000106 RID: 262
    public enum TYPE
    {
        // Token: 0x0400062A RID: 1578
        OBJ,
        // Token: 0x0400062B RID: 1579
        SKYBOX,
        // Token: 0x0400062C RID: 1580
        PICTURE,
        // Token: 0x0400062D RID: 1581
        COLOR,
        // Token: 0x0400062E RID: 1582
        NUM
    }
}
