using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Apply To Mesh", 300)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	public class ApplyToMesh : MonoBehaviour
	{
		[Header("Media Source")]
		[SerializeField]
		private MediaPlayer _media;

		[Tooltip("Default texture to display when the video texture is preparing")]
		[SerializeField]
		private Texture2D _defaultTexture;

		[Space(8f)]
		[Header("Renderer Target")]
		[SerializeField]
		private Renderer _mesh;

		[SerializeField]
		private string _texturePropertyName = "_MainTex";

		[SerializeField]
		private Vector2 _offset = Vector2.zero;

		[SerializeField]
		private Vector2 _scale = Vector2.one;

		private bool _isDirty;

		private Texture _lastTextureApplied;

		private static int _propStereo;

		private static int _propAlphaPack;

		private static int _propApplyGamma;

		private const string PropChromaTexName = "_ChromaTex";

		private static int _propChromaTex;

		private const string PropUseYpCbCrName = "_UseYpCbCr";

		private static int _propUseYpCbCr;

		public MediaPlayer Player
		{
			get
			{
				return _media;
			}
			set
			{
				if (_media != value)
				{
					_media = value;
					_isDirty = true;
				}
			}
		}

		public Texture2D DefaultTexture
		{
			get
			{
				return _defaultTexture;
			}
			set
			{
				if (_defaultTexture != value)
				{
					_defaultTexture = value;
					_isDirty = true;
				}
			}
		}

		public Renderer MeshRenderer
		{
			get
			{
				return _mesh;
			}
			set
			{
				if (_mesh != value)
				{
					_mesh = value;
					_isDirty = true;
				}
			}
		}

		public string TexturePropertyName
		{
			get
			{
				return _texturePropertyName;
			}
			set
			{
				if (_texturePropertyName != value)
				{
					_texturePropertyName = value;
					_isDirty = true;
				}
			}
		}

		public Vector2 Offset
		{
			get
			{
				return _offset;
			}
			set
			{
				if (_offset != value)
				{
					_offset = value;
					_isDirty = true;
				}
			}
		}

		public Vector2 Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				if (_scale != value)
				{
					_scale = value;
					_isDirty = true;
				}
			}
		}

		public void ForceUpdate()
		{
			_isDirty = true;
			LateUpdate();
		}

		private void Awake()
		{
			if (_propStereo == 0 || _propAlphaPack == 0)
			{
				_propStereo = Shader.PropertyToID("Stereo");
				_propAlphaPack = Shader.PropertyToID("AlphaPack");
				_propApplyGamma = Shader.PropertyToID("_ApplyGamma");
			}
			if (_propChromaTex == 0)
			{
				_propChromaTex = Shader.PropertyToID("_ChromaTex");
			}
			if (_propUseYpCbCr == 0)
			{
				_propUseYpCbCr = Shader.PropertyToID("_UseYpCbCr");
			}
		}

		private void LateUpdate()
		{
			bool flag = false;
			if (_media != null && _media.TextureProducer != null)
			{
				Texture texture = _media.TextureProducer.GetTexture();
				if (texture != null)
				{
					if (texture != _lastTextureApplied)
					{
						_isDirty = true;
					}
					if (_isDirty)
					{
						int textureCount = _media.TextureProducer.GetTextureCount();
						for (int i = 0; i < textureCount; i++)
						{
							texture = _media.TextureProducer.GetTexture(i);
							if (texture != null)
							{
								ApplyMapping(texture, _media.TextureProducer.RequiresVerticalFlip(), i);
							}
						}
					}
					flag = true;
				}
			}
			if (!flag)
			{
				if (_defaultTexture != _lastTextureApplied)
				{
					_isDirty = true;
				}
				if (_isDirty)
				{
					ApplyMapping(_defaultTexture, false);
				}
			}
		}

		private void ApplyMapping(Texture texture, bool requiresYFlip, int plane = 0)
		{
			if (!(_mesh != null))
			{
				return;
			}
			_isDirty = false;
			Material[] materials = _mesh.materials;
			if (materials == null)
			{
				return;
			}
			foreach (Material material in materials)
			{
				if (!((Object)(object)material != null))
				{
					continue;
				}
				switch (plane)
				{
				case 0:
					material.SetTexture(_texturePropertyName, texture);
					_lastTextureApplied = texture;
					if (texture != null)
					{
						if (requiresYFlip)
						{
							material.SetTextureScale(_texturePropertyName, new Vector2(_scale.x, 0f - _scale.y));
							material.SetTextureOffset(_texturePropertyName, Vector2.up + _offset);
						}
						else
						{
							material.SetTextureScale(_texturePropertyName, _scale);
							material.SetTextureOffset(_texturePropertyName, _offset);
						}
					}
					break;
				case 1:
					if (material.HasProperty(_propUseYpCbCr) && material.HasProperty(_propChromaTex))
					{
						material.EnableKeyword("USE_YPCBCR");
						material.SetTexture(_propChromaTex, texture);
						if (requiresYFlip)
						{
							material.SetTextureScale("_ChromaTex", new Vector2(_scale.x, 0f - _scale.y));
							material.SetTextureOffset("_ChromaTex", Vector2.up + _offset);
						}
						else
						{
							material.SetTextureScale("_ChromaTex", _scale);
							material.SetTextureOffset("_ChromaTex", _offset);
						}
					}
					break;
				}
				if (_media != null)
				{
					if (material.HasProperty(_propStereo))
					{
						Helper.SetupStereoMaterial(material, _media.m_StereoPacking, _media.m_DisplayDebugStereoColorTint);
					}
					if (material.HasProperty(_propAlphaPack))
					{
						Helper.SetupAlphaPackedMaterial(material, _media.m_AlphaPacking);
					}
					if (material.HasProperty(_propApplyGamma) && _media.Info != null)
					{
						Helper.SetupGammaMaterial(material, _media.Info.PlayerSupportsLinearColorSpace());
					}
				}
			}
		}

		private void OnEnable()
		{
			if (_mesh == null)
			{
				_mesh = GetComponent<MeshRenderer>();
				if (_mesh == null)
				{
					Debug.LogWarning("[AVProVideo] No mesh renderer set or found in gameobject");
				}
			}
			_isDirty = true;
			if (_mesh != null)
			{
				LateUpdate();
			}
		}

		private void OnDisable()
		{
			ApplyMapping(_defaultTexture, false);
		}
	}
}
