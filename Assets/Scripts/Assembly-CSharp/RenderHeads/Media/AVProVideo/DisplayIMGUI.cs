using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Display IMGUI", 200)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	[ExecuteInEditMode]
	public class DisplayIMGUI : MonoBehaviour
	{
		public MediaPlayer _mediaPlayer;

		public bool _displayInEditor = true;

		public ScaleMode _scaleMode = ScaleMode.ScaleToFit;

		public Color _color = Color.white;

		public bool _alphaBlend;

		[SerializeField]
		private bool _useDepth;

		public int _depth;

		public bool _fullScreen = true;

		[Range(0f, 1f)]
		public float _x;

		[Range(0f, 1f)]
		public float _y;

		[Range(0f, 1f)]
		public float _width = 1f;

		[Range(0f, 1f)]
		public float _height = 1f;

		private static int _propAlphaPack;

		private static int _propVertScale;

		private static int _propApplyGamma;

		private static Shader _shaderAlphaPacking;

		private Material _material;

		private void Awake()
		{
			if (_propAlphaPack == 0)
			{
				_propAlphaPack = Shader.PropertyToID("AlphaPack");
				_propVertScale = Shader.PropertyToID("_VertScale");
				_propApplyGamma = Shader.PropertyToID("_ApplyGamma");
			}
		}

		private void Start()
		{
			if (!_useDepth)
			{
				base.useGUILayout = false;
			}
			if (_shaderAlphaPacking == null)
			{
				_shaderAlphaPacking = Shader.Find("AVProVideo/IMGUI/Texture Transparent");
				if (_shaderAlphaPacking == null)
				{
					Debug.LogWarning("[AVProVideo] Missing shader AVProVideo/IMGUI/Transparent Packed");
				}
			}
		}

		private void OnDestroy()
		{
			if (_material != null)
			{
                global::UnityEngine.Object.Destroy(_material);
				_material = null;
			}
		}

		private Shader GetRequiredShader()
		{
			Shader shader = null;
			switch (_mediaPlayer.m_AlphaPacking)
			{
			case AlphaPacking.TopBottom:
			case AlphaPacking.LeftRight:
				shader = _shaderAlphaPacking;
				break;
			}
			if (shader == null && _mediaPlayer.Info != null && QualitySettings.activeColorSpace == ColorSpace.Linear && _mediaPlayer.Info.PlayerSupportsLinearColorSpace())
			{
				shader = _shaderAlphaPacking;
			}
			if (shader == null && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTextureCount() == 2)
			{
				shader = _shaderAlphaPacking;
			}
			return shader;
		}

		private void Update()
		{
			if (!(_mediaPlayer != null))
			{
				return;
			}
			Shader shader = null;
			if (_material != null)
			{
				shader = _material.shader;
			}
			Shader requiredShader = GetRequiredShader();
			if (shader != requiredShader)
			{
				if (_material != null)
				{
                    global::UnityEngine.Object.Destroy(_material);
					_material = null;
				}
				if (requiredShader != null)
				{
					_material = new Material(requiredShader);
				}
			}
			if (_material != null)
			{
				if (_material.HasProperty(_propAlphaPack))
				{
					Helper.SetupAlphaPackedMaterial(_material, _mediaPlayer.m_AlphaPacking);
				}
				if (_material.HasProperty(_propApplyGamma) && _mediaPlayer.Info != null)
				{
					Helper.SetupGammaMaterial(_material, !_mediaPlayer.Info.PlayerSupportsLinearColorSpace());
				}
			}
		}

		private void OnGUI()
		{
			if (_mediaPlayer == null)
			{
				return;
			}
			bool flag = false;
			Texture texture = null;
			if (_displayInEditor)
			{
			}
			if (_mediaPlayer.Info != null && !_mediaPlayer.Info.HasVideo())
			{
				texture = null;
			}
			if (_mediaPlayer.TextureProducer != null)
			{
				if (_mediaPlayer.TextureProducer.GetTexture() != null)
				{
					texture = _mediaPlayer.TextureProducer.GetTexture();
					flag = _mediaPlayer.TextureProducer.RequiresVerticalFlip();
				}
				if (_mediaPlayer.TextureProducer.GetTextureCount() == 2 && _material != null)
				{
					Texture texture2 = _mediaPlayer.TextureProducer.GetTexture(1);
					_material.SetTexture("_ChromaTex", texture2);
					_material.EnableKeyword("USE_YPCBCR");
				}
			}
			if (!(texture != null) || (_alphaBlend && !(_color.a > 0f)))
			{
				return;
			}
			GUI.depth = _depth;
			GUI.color = _color;
			Rect rect = GetRect();
			if (_material != null)
			{
				if (flag)
				{
					_material.SetFloat(_propVertScale, -1f);
				}
				else
				{
					_material.SetFloat(_propVertScale, 1f);
				}
				Helper.DrawTexture(rect, texture, _scaleMode, _mediaPlayer.m_AlphaPacking, _material);
			}
			else
			{
				if (flag)
				{
					GUIUtility.ScaleAroundPivot(new Vector2(1f, -1f), new Vector2(0f, rect.y + rect.height / 2f));
				}
				GUI.DrawTexture(rect, texture, _scaleMode, _alphaBlend);
			}
		}

		public Rect GetRect()
		{
			return (!_fullScreen) ? new Rect(_x * (float)(Screen.width - 1), _y * (float)(Screen.height - 1), _width * (float)Screen.width, _height * (float)Screen.height) : new Rect(0f, 0f, Screen.width, Screen.height);
		}
	}
}
