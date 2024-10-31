using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo
{
	[ExecuteInEditMode]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	[AddComponentMenu("AVPro Video/Display uGUI", 200)]
	public class DisplayUGUI : MaskableGraphic
	{
		[SerializeField]
		public MediaPlayer _mediaPlayer;

		[SerializeField]
		public Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

		[SerializeField]
		public bool _setNativeSize;

		[SerializeField]
		public ScaleMode _scaleMode = ScaleMode.ScaleToFit;

		[SerializeField]
		public bool _noDefaultDisplay = true;

		[SerializeField]
		public bool _displayInEditor = true;

		[SerializeField]
		public Texture _defaultTexture;

		private int _lastWidth;

		private int _lastHeight;

		private bool _flipY;

		private Texture _lastTexture;

		private static Shader _shaderStereoPacking;

		private static Shader _shaderAlphaPacking;

		private static int _propAlphaPack;

		private static int _propVertScale;

		private static int _propStereo;

		private static int _propApplyGamma;

		private static int _propUseYpCbCr;

		private const string PropChromaTexName = "_ChromaTex";

		private static int _propChromaTex;

		private bool _userMaterial = true;

		private Material _material;

		private List<UIVertex> _vertices = new List<UIVertex>(4);

		private static List<int> QuadIndices = new List<int>(new int[6] { 0, 1, 2, 2, 3, 0 });

		public override Texture mainTexture
		{
			get
			{
				Texture result = Texture2D.whiteTexture;
				if (HasValidTexture())
				{
					result = _mediaPlayer.TextureProducer.GetTexture();
				}
				else if (_noDefaultDisplay)
				{
					result = null;
				}
				else if (_defaultTexture != null)
				{
					result = _defaultTexture;
				}
				return result;
			}
		}

		public MediaPlayer CurrentMediaPlayer
		{
			get
			{
				return _mediaPlayer;
			}
			set
			{
				if (_mediaPlayer != value)
				{
					_mediaPlayer = value;
					SetMaterialDirty();
				}
			}
		}

		public Rect uvRect
		{
			get
			{
				return m_UVRect;
			}
			set
			{
				if (!(m_UVRect == value))
				{
					m_UVRect = value;
					SetVerticesDirty();
				}
			}
		}

		protected override void Awake()
		{
			if (_propAlphaPack == 0)
			{
				_propStereo = Shader.PropertyToID("Stereo");
				_propAlphaPack = Shader.PropertyToID("AlphaPack");
				_propVertScale = Shader.PropertyToID("_VertScale");
				_propApplyGamma = Shader.PropertyToID("_ApplyGamma");
				_propUseYpCbCr = Shader.PropertyToID("_UseYpCbCr");
				_propChromaTex = Shader.PropertyToID("_ChromaTex");
			}
			if (_shaderAlphaPacking == null)
			{
				_shaderAlphaPacking = Shader.Find("AVProVideo/UI/Transparent Packed");
				if (_shaderAlphaPacking == null)
				{
					Debug.LogWarning("[AVProVideo] Missing shader AVProVideo/UI/Transparent Packed");
				}
			}
			if (_shaderStereoPacking == null)
			{
				_shaderStereoPacking = Shader.Find("AVProVideo/UI/Stereo");
				if (_shaderStereoPacking == null)
				{
					Debug.LogWarning("[AVProVideo] Missing shader AVProVideo/UI/Stereo");
				}
			}
			base.Awake();
		}

		protected override void Start()
		{
			_userMaterial = (UnityEngine.Object)(object)m_Material != null;
			base.Start();
		}

		protected override void OnDestroy()
		{
			if ((UnityEngine.Object)(object)_material != null)
			{
				material = null;
				UnityEngine.Object.Destroy((UnityEngine.Object)(object)_material);
				_material = null;
			}
			base.OnDestroy();
		}

		private Shader GetRequiredShader()
		{
			Shader shader = null;
			switch (_mediaPlayer.m_StereoPacking)
			{
			case StereoPacking.TopBottom:
			case StereoPacking.LeftRight:
				shader = _shaderStereoPacking;
				break;
			}
			switch (_mediaPlayer.m_AlphaPacking)
			{
			case AlphaPacking.TopBottom:
			case AlphaPacking.LeftRight:
				shader = _shaderAlphaPacking;
				break;
			}
			if (shader == null && _mediaPlayer.Info != null && QualitySettings.activeColorSpace == ColorSpace.Linear && !_mediaPlayer.Info.PlayerSupportsLinearColorSpace())
			{
				shader = _shaderAlphaPacking;
			}
			if (shader == null && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTextureCount() == 2)
			{
				shader = _shaderAlphaPacking;
			}
			return shader;
		}

		public bool HasValidTexture()
		{
			return _mediaPlayer != null && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTexture() != null;
		}

		private void UpdateInternalMaterial()
		{
			if (!(_mediaPlayer != null))
			{
				return;
			}
			Shader shader = null;
			if ((UnityEngine.Object)(object)_material != null)
			{
				shader = _material.shader;
			}
			Shader requiredShader = GetRequiredShader();
			if (shader != requiredShader)
			{
				if ((UnityEngine.Object)(object)_material != null)
				{
					material = null;
					UnityEngine.Object.Destroy((UnityEngine.Object)(object)_material);
					_material = null;
				}
				if (requiredShader != null)
				{
					_material = new Material(requiredShader);
				}
			}
			material = _material;
		}

		private void LateUpdate()
		{
			if (_setNativeSize)
			{
				SetNativeSize();
			}
			if (_lastTexture != mainTexture)
			{
				_lastTexture = mainTexture;
				SetVerticesDirty();
			}
			if (HasValidTexture() && mainTexture != null && (mainTexture.width != _lastWidth || mainTexture.height != _lastHeight))
			{
				_lastWidth = mainTexture.width;
				_lastHeight = mainTexture.height;
				SetVerticesDirty();
			}
			if (!_userMaterial && Application.isPlaying)
			{
				UpdateInternalMaterial();
			}
			if ((UnityEngine.Object)(object)material != null && _mediaPlayer != null)
			{
				if (material.HasProperty(_propUseYpCbCr) && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTextureCount() == 2)
				{
					material.EnableKeyword("USE_YPCBCR");
					material.SetTexture(_propChromaTex, _mediaPlayer.TextureProducer.GetTexture(1));
				}
				if (material.HasProperty(_propAlphaPack))
				{
					Helper.SetupAlphaPackedMaterial(material, _mediaPlayer.m_AlphaPacking);
					if (_flipY && _mediaPlayer.m_AlphaPacking != 0)
					{
						material.SetFloat(_propVertScale, -1f);
					}
					else
					{
						material.SetFloat(_propVertScale, 1f);
					}
				}
				if (material.HasProperty(_propStereo))
				{
					Helper.SetupStereoMaterial(material, _mediaPlayer.m_StereoPacking, _mediaPlayer.m_DisplayDebugStereoColorTint);
				}
				if (material.HasProperty(_propApplyGamma) && _mediaPlayer.Info != null)
				{
					Helper.SetupGammaMaterial(material, _mediaPlayer.Info.PlayerSupportsLinearColorSpace());
				}
			}
			SetMaterialDirty();
		}

		[ContextMenu("Set Native Size")]
		public override void SetNativeSize()
		{
			Texture texture = mainTexture;
			if (!(texture != null))
			{
				return;
			}
			int num = Mathf.RoundToInt((float)texture.width * uvRect.width);
			int num2 = Mathf.RoundToInt((float)texture.height * uvRect.height);
			if (_mediaPlayer != null)
			{
				if (_mediaPlayer.m_AlphaPacking == AlphaPacking.LeftRight || _mediaPlayer.m_StereoPacking == StereoPacking.LeftRight)
				{
					num /= 2;
				}
				else if (_mediaPlayer.m_AlphaPacking == AlphaPacking.TopBottom || _mediaPlayer.m_StereoPacking == StereoPacking.TopBottom)
				{
					num2 /= 2;
				}
			}
			base.rectTransform.anchorMax = base.rectTransform.anchorMin;
			base.rectTransform.sizeDelta = new Vector2(num, num2);
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			_OnFillVBO(_vertices);
			vh.AddUIVertexStream(_vertices, QuadIndices);
		}

		[Obsolete("This method is not called from Unity 5.2 and above")]
		protected override void OnFillVBO(List<UIVertex> vbo)
		{
			_OnFillVBO(vbo);
		}

		private void _OnFillVBO(List<UIVertex> vbo)
		{
			_flipY = false;
			if (HasValidTexture())
			{
				_flipY = _mediaPlayer.TextureProducer.RequiresVerticalFlip();
			}
			Rect uVRect = m_UVRect;
			Vector4 drawingDimensions = GetDrawingDimensions(_scaleMode, ref uVRect);
			vbo.Clear();
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = color;
			simpleVert.position = new Vector2(drawingDimensions.x, drawingDimensions.y);
			simpleVert.uv0 = new Vector2(uVRect.xMin, uVRect.yMin);
			if (_flipY)
			{
				simpleVert.uv0 = new Vector2(uVRect.xMin, 1f - uVRect.yMin);
			}
			vbo.Add(simpleVert);
			simpleVert.position = new Vector2(drawingDimensions.x, drawingDimensions.w);
			simpleVert.uv0 = new Vector2(uVRect.xMin, uVRect.yMax);
			if (_flipY)
			{
				simpleVert.uv0 = new Vector2(uVRect.xMin, 1f - uVRect.yMax);
			}
			vbo.Add(simpleVert);
			simpleVert.position = new Vector2(drawingDimensions.z, drawingDimensions.w);
			simpleVert.uv0 = new Vector2(uVRect.xMax, uVRect.yMax);
			if (_flipY)
			{
				simpleVert.uv0 = new Vector2(uVRect.xMax, 1f - uVRect.yMax);
			}
			vbo.Add(simpleVert);
			simpleVert.position = new Vector2(drawingDimensions.z, drawingDimensions.y);
			simpleVert.uv0 = new Vector2(uVRect.xMax, uVRect.yMin);
			if (_flipY)
			{
				simpleVert.uv0 = new Vector2(uVRect.xMax, 1f - uVRect.yMin);
			}
			vbo.Add(simpleVert);
		}

		private Vector4 GetDrawingDimensions(ScaleMode scaleMode, ref Rect uvRect)
		{
			Vector4 result = Vector4.zero;
			if (mainTexture != null)
			{
				Vector4 zero = Vector4.zero;
				Vector2 vector = new Vector2(mainTexture.width, mainTexture.height);
				if (_mediaPlayer != null)
				{
					if (_mediaPlayer.m_AlphaPacking == AlphaPacking.LeftRight || _mediaPlayer.m_StereoPacking == StereoPacking.LeftRight)
					{
						vector.x /= 2f;
					}
					else if (_mediaPlayer.m_AlphaPacking == AlphaPacking.TopBottom || _mediaPlayer.m_StereoPacking == StereoPacking.TopBottom)
					{
						vector.y /= 2f;
					}
				}
				Rect pixelAdjustedRect = GetPixelAdjustedRect();
				int num = Mathf.RoundToInt(vector.x);
				int num2 = Mathf.RoundToInt(vector.y);
				Vector4 vector2 = new Vector4(zero.x / (float)num, zero.y / (float)num2, ((float)num - zero.z) / (float)num, ((float)num2 - zero.w) / (float)num2);
				if (vector.sqrMagnitude > 0f)
				{
					switch (scaleMode)
					{
					case ScaleMode.ScaleToFit:
					{
						float num7 = vector.x / vector.y;
						float num8 = pixelAdjustedRect.width / pixelAdjustedRect.height;
						if (num7 > num8)
						{
							float height = pixelAdjustedRect.height;
							pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num7);
							pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * base.rectTransform.pivot.y;
						}
						else
						{
							float width = pixelAdjustedRect.width;
							pixelAdjustedRect.width = pixelAdjustedRect.height * num7;
							pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * base.rectTransform.pivot.x;
						}
						break;
					}
					case ScaleMode.ScaleAndCrop:
					{
						float num3 = vector.x / vector.y;
						float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
						if (num4 > num3)
						{
							float num5 = num3 / num4;
							uvRect = new Rect(0f, (1f - num5) * 0.5f, 1f, num5);
						}
						else
						{
							float num6 = num4 / num3;
							uvRect = new Rect(0.5f - num6 * 0.5f, 0f, num6, 1f);
						}
						break;
					}
					}
				}
				result = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector2.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector2.y, pixelAdjustedRect.x + pixelAdjustedRect.width * vector2.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector2.w);
			}
			return result;
		}
	}
}
