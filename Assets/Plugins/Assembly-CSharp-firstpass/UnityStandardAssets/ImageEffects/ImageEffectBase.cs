using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		public Shader shader;

		private Material m_Material;

		protected Material material
		{
			get
			{
				if ((UnityEngine.Object)(object)m_Material == null)
				{
					m_Material = new Material(shader);
					((UnityEngine.Object)(object)m_Material).hideFlags = HideFlags.HideAndDontSave;
				}
				return m_Material;
			}
		}

		protected virtual void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
			}
			else if (!shader || !shader.isSupported)
			{
				base.enabled = false;
			}
		}

		protected virtual void OnDisable()
		{
			if ((bool)(UnityEngine.Object)(object)m_Material)
			{
                UnityEngine.Object.DestroyImmediate((UnityEngine.Object)(object)m_Material);
			}
		}
	}
}
