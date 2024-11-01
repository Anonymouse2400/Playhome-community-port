using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	[AddComponentMenu("AVPro Video/Cubemap Cube (VR)", 400)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	public class CubemapCube : MonoBehaviour
	{
		private Mesh _mesh;

		protected MeshRenderer _renderer;

		[SerializeField]
		protected Material _material;

		[SerializeField]
		private MediaPlayer _mediaPlayer;

		[SerializeField]
		private float expansion_coeff = 1.01f;

		private Texture _texture;

		private bool _verticalFlip;

		private int _textureWidth;

		private int _textureHeight;

		private static int _propApplyGamma;

		private static int _propUseYpCbCr;

		private const string PropChromaTexName = "_ChromaTex";

		private static int _propChromaTex;

		public MediaPlayer Player
		{
			get
			{
				return _mediaPlayer;
			}
			set
			{
				_mediaPlayer = value;
			}
		}

		private void Awake()
		{
			if (_propApplyGamma == 0)
			{
				_propApplyGamma = Shader.PropertyToID("_ApplyGamma");
			}
			if (_propUseYpCbCr == 0)
			{
				_propUseYpCbCr = Shader.PropertyToID("_UseYpCbCr");
			}
			if (_propChromaTex == 0)
			{
				_propChromaTex = Shader.PropertyToID("_ChromaTex");
			}
		}

		private void Start()
		{
			if (_mesh == null)
			{
				_mesh = new Mesh();
				_mesh.MarkDynamic();
				MeshFilter component = GetComponent<MeshFilter>();
				if (component != null)
				{
					component.mesh = _mesh;
				}
				_renderer = GetComponent<MeshRenderer>();
				if (_renderer != null)
				{
					_renderer.material = _material;
				}
				BuildMesh();
			}
		}

		private void OnDestroy()
		{
			if (_mesh != null)
			{
				MeshFilter component = GetComponent<MeshFilter>();
				if (component != null)
				{
					component.mesh = null;
				}
				Object.Destroy(_mesh);
				_mesh = null;
			}
			if (_renderer != null)
			{
				_renderer.material = null;
				_renderer = null;
			}
		}

		private void LateUpdate()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			Texture texture = null;
			bool flag = false;
			if (_mediaPlayer != null && _mediaPlayer.Control != null)
			{
				if (_mediaPlayer.TextureProducer != null)
				{
					texture = _mediaPlayer.TextureProducer.GetTexture();
					flag = _mediaPlayer.TextureProducer.RequiresVerticalFlip();
					if (_texture != texture || _verticalFlip != flag || (texture != null && (_textureWidth != texture.width || _textureHeight != texture.height)))
					{
						_texture = texture;
						if (texture != null)
						{
							UpdateMeshUV(texture.width, texture.height, flag);
						}
					}
					if (_renderer.material.HasProperty(_propApplyGamma) && _mediaPlayer.Info != null)
					{
						Helper.SetupGammaMaterial(_renderer.material, _mediaPlayer.Info.PlayerSupportsLinearColorSpace());
					}
					if (_renderer.material.HasProperty(_propUseYpCbCr) && _mediaPlayer.TextureProducer.GetTextureCount() == 2)
					{
						_renderer.material.EnableKeyword("USE_YPCBCR");
						_renderer.material.SetTexture(_propChromaTex, _mediaPlayer.TextureProducer.GetTexture(1));
					}
				}
				_renderer.material.mainTexture = _texture;
			}
			else
			{
				_renderer.material.mainTexture = null;
			}
		}

		private void BuildMesh()
		{
			Vector3 vector = new Vector3(-0.5f, -0.5f, -0.5f);
			Vector3[] array = new Vector3[24]
			{
				new Vector3(0f, -1f, 0f) - vector,
				new Vector3(0f, 0f, 0f) - vector,
				new Vector3(0f, 0f, -1f) - vector,
				new Vector3(0f, -1f, -1f) - vector,
				new Vector3(0f, 0f, 0f) - vector,
				new Vector3(-1f, 0f, 0f) - vector,
				new Vector3(-1f, 0f, -1f) - vector,
				new Vector3(0f, 0f, -1f) - vector,
				new Vector3(-1f, 0f, 0f) - vector,
				new Vector3(-1f, -1f, 0f) - vector,
				new Vector3(-1f, -1f, -1f) - vector,
				new Vector3(-1f, 0f, -1f) - vector,
				new Vector3(-1f, -1f, 0f) - vector,
				new Vector3(0f, -1f, 0f) - vector,
				new Vector3(0f, -1f, -1f) - vector,
				new Vector3(-1f, -1f, -1f) - vector,
				new Vector3(0f, -1f, -1f) - vector,
				new Vector3(0f, 0f, -1f) - vector,
				new Vector3(-1f, 0f, -1f) - vector,
				new Vector3(-1f, -1f, -1f) - vector,
				new Vector3(-1f, -1f, 0f) - vector,
				new Vector3(-1f, 0f, 0f) - vector,
				new Vector3(0f, 0f, 0f) - vector,
				new Vector3(0f, -1f, 0f) - vector
			};
			Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.right), Vector3.one);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = matrix4x.MultiplyPoint(array[i]);
			}
			_mesh.vertices = array;
			_mesh.triangles = new int[36]
			{
				0, 1, 2, 0, 2, 3, 4, 5, 6, 4,
				6, 7, 8, 9, 10, 8, 10, 11, 12, 13,
				14, 12, 14, 15, 16, 17, 18, 16, 18, 19,
				20, 21, 22, 20, 22, 23
			};
			_mesh.normals = new Vector3[24]
			{
				new Vector3(-1f, 0f, 0f),
				new Vector3(-1f, 0f, 0f),
				new Vector3(-1f, 0f, 0f),
				new Vector3(-1f, 0f, 0f),
				new Vector3(0f, -1f, 0f),
				new Vector3(0f, -1f, 0f),
				new Vector3(0f, -1f, 0f),
				new Vector3(0f, -1f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(0f, 1f, 0f),
				new Vector3(0f, 1f, 0f),
				new Vector3(0f, 1f, 0f),
				new Vector3(0f, 1f, 0f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0f, 0f, -1f),
				new Vector3(0f, 0f, -1f),
				new Vector3(0f, 0f, -1f),
				new Vector3(0f, 0f, -1f)
			};
			UpdateMeshUV(512, 512, false);
		}

		private void UpdateMeshUV(int textureWidth, int textureHeight, bool flipY)
		{
			_textureWidth = textureWidth;
			_textureHeight = textureHeight;
			_verticalFlip = flipY;
			float num = textureWidth;
			float num2 = textureHeight;
			float num3 = num / 3f;
			float num4 = Mathf.Floor((expansion_coeff * num3 - num3) / 2f);
			float num5 = num4 / num;
			float num6 = num4 / num2;
			Vector2[] array = new Vector2[24]
			{
				new Vector2(1f / 3f + num5, 1f - num6),
				new Vector2(2f / 3f - num5, 1f - num6),
				new Vector2(2f / 3f - num5, 0.5f + num6),
				new Vector2(1f / 3f + num5, 0.5f + num6),
				new Vector2(1f / 3f + num5, 0.5f - num6),
				new Vector2(2f / 3f - num5, 0.5f - num6),
				new Vector2(2f / 3f - num5, num6),
				new Vector2(1f / 3f + num5, num6),
				new Vector2(num5, 1f - num6),
				new Vector2(1f / 3f - num5, 1f - num6),
				new Vector2(1f / 3f - num5, 0.5f + num6),
				new Vector2(num5, 0.5f + num6),
				new Vector2(2f / 3f + num5, 0.5f - num6),
				new Vector2(1f - num5, 0.5f - num6),
				new Vector2(1f - num5, num6),
				new Vector2(2f / 3f + num5, num6),
				new Vector2(num5, num6),
				new Vector2(num5, 0.5f - num6),
				new Vector2(1f / 3f - num5, 0.5f - num6),
				new Vector2(1f / 3f - num5, num6),
				new Vector2(1f - num5, 1f - num6),
				new Vector2(1f - num5, 0.5f + num6),
				new Vector2(2f / 3f + num5, 0.5f + num6),
				new Vector2(2f / 3f + num5, 1f - num6)
			};
			if (flipY)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].y = 1f - array[i].y;
				}
			}
			_mesh.uv = array;
			_mesh.UploadMeshData(false);
		}
	}
}
