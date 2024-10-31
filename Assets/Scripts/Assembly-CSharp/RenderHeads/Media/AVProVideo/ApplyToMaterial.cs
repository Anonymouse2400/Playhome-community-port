using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Apply To Material", 300)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	public class ApplyToMaterial : MonoBehaviour
	{
		public Vector2 _offset = Vector2.zero;

		public Vector2 _scale = Vector2.one;

		public Material _material;

		public string _texturePropertyName;

		public MediaPlayer _media;

		public Texture2D _defaultTexture;

		private Texture _originalTexture;

		private Vector2 _originalScale = Vector2.one;

		private Vector2 _originalOffset = Vector2.zero;

		private static int _propStereo;

		private static int _propAlphaPack;

		private static int _propApplyGamma;

		private const string PropChromaTexName = "_ChromaTex";

		private static int _propChromaTex;

		private const string PropUseYpCbCrName = "_UseYpCbCr";

		private static int _propUseYpCbCr;

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
				int textureCount = _media.TextureProducer.GetTextureCount();
				for (int i = 0; i < textureCount; i++)
				{
					Texture texture = _media.TextureProducer.GetTexture(i);
					if (texture != null)
					{
						ApplyMapping(texture, _media.TextureProducer.RequiresVerticalFlip(), i);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				ApplyMapping(_defaultTexture, false);
			}
		}

		private void ApplyMapping(Texture texture, bool requiresYFlip, int plane = 0)
		{
			if (!((Object)(object)_material != null))
			{
				return;
			}
			switch (plane)
			{
			case 0:
				if (string.IsNullOrEmpty(_texturePropertyName))
				{
					_material.mainTexture = texture;
					if (texture != null)
					{
						if (requiresYFlip)
						{
							_material.mainTextureScale = new Vector2(_scale.x, 0f - _scale.y);
							_material.mainTextureOffset = Vector2.up + _offset;
						}
						else
						{
							_material.mainTextureScale = _scale;
							_material.mainTextureOffset = _offset;
						}
					}
					break;
				}
				_material.SetTexture(_texturePropertyName, texture);
				if (texture != null)
				{
					if (requiresYFlip)
					{
						_material.SetTextureScale(_texturePropertyName, new Vector2(_scale.x, 0f - _scale.y));
						_material.SetTextureOffset(_texturePropertyName, Vector2.up + _offset);
					}
					else
					{
						_material.SetTextureScale(_texturePropertyName, _scale);
						_material.SetTextureOffset(_texturePropertyName, _offset);
					}
				}
				break;
			case 1:
				if (_material.HasProperty(_propUseYpCbCr))
				{
					_material.EnableKeyword("USE_YPCBCR");
				}
				if (!_material.HasProperty(_propChromaTex))
				{
					break;
				}
				_material.SetTexture(_propChromaTex, texture);
				if (texture != null)
				{
					if (requiresYFlip)
					{
						_material.SetTextureScale("_ChromaTex", new Vector2(_scale.x, 0f - _scale.y));
						_material.SetTextureOffset("_ChromaTex", Vector2.up + _offset);
					}
					else
					{
						_material.SetTextureScale("_ChromaTex", _scale);
						_material.SetTextureOffset("_ChromaTex", _offset);
					}
				}
				break;
			}
			if (_media != null)
			{
				if (_material.HasProperty(_propStereo))
				{
					Helper.SetupStereoMaterial(_material, _media.m_StereoPacking, _media.m_DisplayDebugStereoColorTint);
				}
				if (_material.HasProperty(_propAlphaPack))
				{
					Helper.SetupAlphaPackedMaterial(_material, _media.m_AlphaPacking);
				}
				if (_material.HasProperty(_propApplyGamma) && _media.Info != null)
				{
					Helper.SetupGammaMaterial(_material, _media.Info.PlayerSupportsLinearColorSpace());
				}
			}
		}

		private void OnEnable()
		{
			if (string.IsNullOrEmpty(_texturePropertyName))
			{
				_originalTexture = _material.mainTexture;
				_originalScale = _material.mainTextureScale;
				_originalOffset = _material.mainTextureOffset;
			}
			else
			{
				_originalTexture = _material.GetTexture(_texturePropertyName);
				_originalScale = _material.GetTextureScale(_texturePropertyName);
				_originalOffset = _material.GetTextureOffset(_texturePropertyName);
			}
			LateUpdate();
		}

		private void OnDisable()
		{
			if (string.IsNullOrEmpty(_texturePropertyName))
			{
				_material.mainTexture = _originalTexture;
				_material.mainTextureScale = _originalScale;
				_material.mainTextureOffset = _originalOffset;
			}
			else
			{
				_material.SetTexture(_texturePropertyName, _originalTexture);
				_material.SetTextureScale(_texturePropertyName, _originalScale);
				_material.SetTextureOffset(_texturePropertyName, _originalOffset);
			}
		}
	}
}
