using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Update Stereo Material", 400)]
	[HelpURL("http://renderheads.com/product/avpro-video/")]
	public class UpdateStereoMaterial : MonoBehaviour
	{
		[Header("Stereo camera")]
		public Camera _camera;

		[Header("Rendering elements")]
		public MeshRenderer _renderer;

		public Graphic _uGuiComponent;

		public Material _material;

		private int _cameraPositionId;

		private int _viewMatrixId;

		private void Awake()
		{
			_cameraPositionId = Shader.PropertyToID("_cameraPosition");
			_viewMatrixId = Shader.PropertyToID("_ViewMatrix");
			if (_camera == null)
			{
				Debug.LogWarning("[AVProVideo] No camera set for UpdateStereoMaterial component. If you are rendering in stereo then it is recommended to set this.");
			}
		}

		private void SetupMaterial(Material m, Camera camera)
		{
			m.SetVector(_cameraPositionId, camera.transform.position);
			m.SetMatrix(_viewMatrixId, camera.worldToCameraMatrix.transpose);
		}

		private void LateUpdate()
		{
			Camera camera = _camera;
			if (camera == null)
			{
				camera = Camera.main;
			}
			if (_renderer == null && (Object)(object)_material == null)
			{
				_renderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			if (camera != null)
			{
				if (_renderer != null)
				{
					SetupMaterial(_renderer.material, camera);
				}
				if ((Object)(object)_material != null)
				{
					SetupMaterial(_material, camera);
				}
				if (_uGuiComponent != null)
				{
					SetupMaterial(_uGuiComponent.material, camera);
				}
			}
		}
	}
}
