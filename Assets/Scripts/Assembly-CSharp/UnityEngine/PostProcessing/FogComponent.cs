using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		private static class Uniforms
		{
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			internal static readonly int _Density = Shader.PropertyToID("_Density");

			internal static readonly int _Start = Shader.PropertyToID("_Start");

			internal static readonly int _End = Shader.PropertyToID("_End");

			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}

		private const string k_ShaderString = "Hidden/Post FX/Fog";

		public override bool active
		{
			get
			{
				return base.model.enabled && context.isGBufferAvailable && !context.interrupted;
			}
		}

		public override string GetName()
		{
			return "Fog";
		}

		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.BeforeImageEffectsOpaque;
		}

		public override void Init(PostProcessingContext pcontext, FogModel pmodel)
		{
			base.Init(pcontext, pmodel);
			FogModel.Settings settings = base.model.settings;
			RenderSettings.fog = base.model.enabled;
			RenderSettings.fogColor = settings.color;
			RenderSettings.fogMode = settings.mode;
			RenderSettings.fogDensity = settings.density;
			RenderSettings.fogStartDistance = settings.start;
			RenderSettings.fogEndDistance = settings.end;
		}

		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			material.SetColor(Uniforms._FogColor, settings.color);
			material.SetFloat(Uniforms._Density, settings.density);
			material.SetFloat(Uniforms._Start, settings.start);
			material.SetFloat(Uniforms._End, settings.end);
			switch (settings.mode)
			{
			case FogMode.Linear:
				material.EnableKeyword("FOG_LINEAR");
				break;
			case FogMode.Exponential:
				material.EnableKeyword("FOG_EXP");
				break;
			case FogMode.ExponentialSquared:
				material.EnableKeyword("FOG_EXP2");
				break;
			}
			RenderTextureFormat format = ((!context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			cb.GetTemporaryRT(Uniforms._TempRT, context.width, context.height, 24, FilterMode.Bilinear, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, Uniforms._TempRT);
			cb.Blit(Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, settings.excludeSkybox ? 1 : 0);
			cb.ReleaseTemporaryRT(Uniforms._TempRT);
		}
	}
}
