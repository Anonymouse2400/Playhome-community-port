using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		[Serializable]
		public struct Settings
		{
			[Tooltip("Controls the color of that fog drawn in the scene.")]
			public Color color;

			[Tooltip("Controls the mathematical function determining the way fog accumulates with distance from the camera. Options are Linear, Exponential and Exponential Squared.")]
			public FogMode mode;

			[Tooltip("Controls the density of the fog effect in the Scene when using Exponential or Exponential Squared modes.")]
			public float density;

			[Tooltip("Controls the distance from the camera where the fog will start in the scene.")]
			public float start;

			[Tooltip("Controls the distance from the camera where the fog will completely obscure objects in the Scene.")]
			public float end;

			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;

			public static Settings defaultSettings
			{
				get
				{
					Settings result = default(Settings);
					result.color = new Color32(102, 108, 113, 154);
					result.mode = FogMode.Exponential;
					result.density = 0.001f;
					result.start = 0f;
					result.end = 600f;
					result.excludeSkybox = true;
					return result;
				}
			}
		}

		[SerializeField]
		private Settings m_Settings = Settings.defaultSettings;

		public Settings settings
		{
			get
			{
				return m_Settings;
			}
			set
			{
				m_Settings = value;
			}
		}

		public override void Reset()
		{
			m_Settings = Settings.defaultSettings;
		}
	}
}
