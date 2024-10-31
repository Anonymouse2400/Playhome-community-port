using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Map : MonoBehaviour
{
	[SerializeField]
	private TextAsset dataText;

	public Transform lightRoot;

	public Material skyMaterial;

	public float ambientIntensity = 1f;

	public MapData data = new MapData();

	private MirrorReflection[] mirrors;

	private Material[] mirrorMaterials;

	private GameControl gameCtrl;

	private List<Terrain> terrains = new List<Terrain>();

	private List<Renderer> renderers = new List<Renderer>();

	private bool show = true;

	private AudioReverbZone[] reverbs;

	private void Awake()
	{
		LoadSetup();
		reverbs = GetComponentsInChildren<AudioReverbZone>();
		for (int i = 0; i < reverbs.Length; i++)
		{
			reverbs[i].enabled = ConfigData.reverb_flag;
		}
	}

	private void Start()
	{
		Skybox skybox = UnityEngine.Object.FindObjectOfType<Skybox>();
		if (skybox != null)
		{
			skybox.material = skyMaterial;
		}
		LightMapControl componentInChildren = GetComponentInChildren<LightMapControl>();
		if (componentInChildren != null)
		{
			componentInChildren.Apply();
		}
		gameCtrl = UnityEngine.Object.FindObjectOfType<GameControl>();
		mirrors = GetComponentsInChildren<MirrorReflection>();
		if (mirrors.Length > 0)
		{
			mirrorMaterials = new Material[mirrors.Length];
			for (int i = 0; i < mirrorMaterials.Length; i++)
			{
				mirrorMaterials[i] = mirrors[i].GetComponent<Renderer>().material;
			}
		}
		RenderSettings.ambientIntensity = ambientIntensity;
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j].enabled && componentsInChildren[j].shadowCastingMode != ShadowCastingMode.ShadowsOnly)
			{
				renderers.Add(componentsInChildren[j]);
			}
			for (int k = 0; k < data.noRecieveShadows.Count; k++)
			{
				if (componentsInChildren[j].name == data.noRecieveShadows[k])
				{
					componentsInChildren[j].receiveShadows = false;
					break;
				}
			}
		}
		Terrain[] componentsInChildren2 = GetComponentsInChildren<Terrain>();
		for (int l = 0; l < componentsInChildren2.Length; l++)
		{
			terrains.Add(componentsInChildren2[l]);
		}
		ChangeShow();
	}

	private void Update()
	{
		if (mirrors != null)
		{
			for (int i = 0; i < mirrors.Length; i++)
			{
				if (mirrors[i].enabled != ConfigData.showMirror)
				{
					mirrors[i].enabled = ConfigData.showMirror;
					mirrors[i].GetComponent<Renderer>().material = ((!ConfigData.showMirror) ? gameCtrl.mirrorDummy : mirrorMaterials[i]);
				}
			}
		}
		for (int j = 0; j < reverbs.Length; j++)
		{
			reverbs[j].enabled = ConfigData.reverb_flag;
		}
		UpdateShortCutKey();
	}

	private void UpdateShortCutKey()
	{
		if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			GlobalData.showMap = !GlobalData.showMap;
			ChangeShow();
			SystemSE.SE se = (GlobalData.showMap ? SystemSE.SE.YES : SystemSE.SE.NO);
			SystemSE.Play(se);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			ConfigData.autoHideObstacle = !ConfigData.autoHideObstacle;
			SystemSE.SE se2 = (ConfigData.autoHideObstacle ? SystemSE.SE.YES : SystemSE.SE.NO);
			SystemSE.Play(se2);
			Config config = UnityEngine.Object.FindObjectOfType<Config>();
			if ((bool)config)
			{
				config.UpdateFromShortCut();
			}
		}
	}

	private void ChangeShow()
	{
		for (int i = 0; i < renderers.Count; i++)
		{
			renderers[i].enabled = GlobalData.showMap;
		}
		for (int j = 0; j < terrains.Count; j++)
		{
			terrains[j].enabled = GlobalData.showMap;
		}
	}

	private void LoadSetup()
	{
		TagText tagText = new TagText();
		tagText.Load_TextAsset(dataText);
		for (int i = 0; i < tagText.ElementNum; i++)
		{
			if (tagText.Elements[i].Tag == "Map")
			{
				tagText.Elements[i].GetVal(ref data.name, "name", 0);
				tagText.Elements[i].GetVal(ref data.order, "order", 0);
				string val = string.Empty;
				tagText.Elements[i].GetVal(ref val, "foot", 0);
				if (val.Equals("BARE", StringComparison.OrdinalIgnoreCase))
				{
					data.foot = MapData.FOOT.BARE;
				}
				else if (val.Equals("SOCKS", StringComparison.OrdinalIgnoreCase))
				{
					data.foot = MapData.FOOT.SOCKS;
				}
				else if (val.Equals("SHOES", StringComparison.OrdinalIgnoreCase))
				{
					data.foot = MapData.FOOT.SHOES;
				}
				tagText.Elements[i].GetVal(ref data.selectPos.x, "selectPos", 0);
				tagText.Elements[i].GetVal(ref data.selectPos.y, "selectPos", 1);
				tagText.Elements[i].GetVal(ref data.selectPos.z, "selectPos", 2);
				tagText.Elements[i].GetVal(ref data.selectYaw, "selectYaw", 0);
				tagText.Elements[i].GetVal(ref data.mob, "mob", 0);
			}
			else if (tagText.Elements[i].Tag == "H_Pos")
			{
				string val2 = string.Empty;
				Vector3 vec = Vector3.zero;
				float val3 = 0f;
				tagText.Elements[i].GetVal(ref val2, "type", 0);
				tagText.Elements[i].GetVal(ref val3, "yaw", 0);
				TagTextUtility.Load_Vector3(ref vec, tagText.Elements[i], "pos");
				H_Pos h_Pos = new H_Pos(vec, val3);
				if (val2.Equals("floor", StringComparison.OrdinalIgnoreCase))
				{
					data.h_pos.floor.Add(h_Pos);
				}
				else if (val2.Equals("chair", StringComparison.OrdinalIgnoreCase))
				{
					data.h_pos.chair.Add(h_Pos);
				}
				else if (val2.Equals("wall", StringComparison.OrdinalIgnoreCase))
				{
					data.h_pos.wall.Add(h_Pos);
				}
				else if (val2.Equals("special", StringComparison.OrdinalIgnoreCase))
				{
					data.h_pos.special.Add(h_Pos);
				}
				else if (val2.Equals("5P_Resist", StringComparison.OrdinalIgnoreCase))
				{
					data.h_pos.five_Resist.Add(h_Pos);
				}
				else if (val2.Equals("5P_Flop", StringComparison.OrdinalIgnoreCase))
				{
					data.h_pos.five_Flop.Add(h_Pos);
				}
				else if (val2.Equals("5P_Weakness", StringComparison.OrdinalIgnoreCase))
				{
					data.h_pos.five_Weakness.Add(h_Pos);
				}
				TagText.Attribute attribute = tagText.Elements[i].GetAttribute("watchType");
				if (attribute != null)
				{
					for (int j = 0; j < attribute.vals.Count; j++)
					{
						string val4 = string.Empty;
						tagText.Elements[i].GetVal(ref val4, "watchType", j);
						Vector3 vec2 = Vector3.zero;
						TagTextUtility.Load_Vector3(ref vec2, tagText.Elements[i], "watchPos", j * 3);
						float val5 = 0f;
						tagText.Elements[i].GetVal(ref val5, "watchYaw", j);
						h_Pos.AddWathPos(val4, vec2, val5);
					}
				}
			}
			else
			{
				if (!(tagText.Elements[i].Tag == "NoReceiveShadow"))
				{
					continue;
				}
				TagText.Attribute attribute2 = tagText.Elements[i].GetAttribute("renderer");
				if (attribute2 != null)
				{
					for (int k = 0; k < attribute2.vals.Count; k++)
					{
						data.noRecieveShadows.Add(attribute2.vals[k]);
					}
				}
			}
		}
	}
}
