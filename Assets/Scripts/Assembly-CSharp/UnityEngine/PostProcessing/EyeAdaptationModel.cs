using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class EyeAdaptationModel : PostProcessingModel
	{
		public enum EyeAdaptationType
		{
			Progressive = 0,
			Fixed = 1
		}

		[Serializable]
		public struct Settings
		{
			[Range(1f, 99f)]
			[Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float lowPercent;

			[Range(1f, 99f)]
			[Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float highPercent;

			[Min(0f)]
			[Tooltip("Minimum average luminance to consider for auto exposure.")]
			public float minLuminance;

			[Min(0f)]
			[Tooltip("Maximum average luminance to consider for auto exposure.")]
			public float maxLuminance;

			[Min(0f)]
			[Tooltip("Exposure bias. Use this to control the global exposure of the scene.")]
			public float exposureCompensation;

			[Tooltip("Use \"Progressive\" if you want the auto exposure to be animated. Use \"Fixed\" otherwise.")]
			public EyeAdaptationType adaptationType;

			[Min(0f)]
			[Tooltip("Adaptation speed from a dark to a light environment.")]
			public float speedUp;

			[Min(0f)]
			[Tooltip("Adaptation speed from a light to a dark environment.")]
			public float speedDown;

			[Range(-16f, -1f)]
			[Tooltip("Lower bound for the brightness range of the generated histogram (Log2).")]
			public int logMin;

			[Range(1f, 16f)]
			[Tooltip("Upper bound for the brightness range of the generated histogram (Log2).")]
			public int logMax;

			public static Settings defaultSettings
			{
				get
				{
					Settings result = default(Settings);
					result.lowPercent = 65f;
					result.highPercent = 95f;
					result.minLuminance = 0.03f;
					result.maxLuminance = 2f;
					result.exposureCompensation = 0.5f;
					result.adaptationType = EyeAdaptationType.Progressive;
					result.speedUp = 2f;
					result.speedDown = 1f;
					result.logMin = -8;
					result.logMax = 4;
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
