using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public sealed class SunFogControllerPostProcessingStackRenderer : PostProcessEffectRenderer<SunFogControllerPostProcessingStack>
{
	private static Material _mat;

	private Light sun;

	public static Material mat
	{
		get
		{
			if (_mat == null)
			{
				_mat = new Material(Shader.Find("JeffShader/SunFogV2"));
			}
			return _mat;
		}
	}

	public override void Render(PostProcessRenderContext context)
	{
		Shader.SetGlobalFloat("SunPower", base.settings._SunFogPower);
		Shader.SetGlobalFloat("SunSize", base.settings._SunSize);
		Shader.SetGlobalFloat("SunEdge", base.settings._SunEdge);
		Color a = base.settings._SunColor;
		Shader.SetGlobalColor("SunColor", a * base.settings._SunFogIntensity);
		Shader.SetGlobalFloat("SunIntensity", base.settings._SunIntensity);
		Shader.SetGlobalFloat("SunFogIntensity", base.settings._SunFogIntensity);
		Color a2 = base.settings._SkyColor;
		Shader.SetGlobalColor("SkyColor", a2 * base.settings._SkyIntensity);
		Shader.SetGlobalColor("SkyTopColor", base.settings._SkyTopColor);
		Shader.SetGlobalFloat("HorizonOffset", base.settings._HorizonOffset);
		Shader.SetGlobalFloat("SkyTopPower", base.settings._SkyTopPower);
		Shader.SetGlobalFloat("FogHeight", base.settings._FogHeight);
		Shader.SetGlobalFloat("FogFade", base.settings._FogFade);
		Shader.SetGlobalFloat("HeightDensity", base.settings._HeightDensity);
		Shader.SetGlobalFloat("HeightFogPower", base.settings._HeightFogPower);
		Shader.SetGlobalFloat("MaxHeightDensity", base.settings._MaxHeightDensity);
		Shader.SetGlobalFloat("FogStartDepth", base.settings._FogStartDistance);
		Shader.SetGlobalFloat("FogEndDepth", base.settings._FogEndDistance);
		Shader.SetGlobalFloat("FogPower", base.settings._FogPower);
		Shader.SetGlobalColor("SunFogDarkColor", base.settings._FogDarkColor);
		Shader.SetGlobalFloat("SunFogDarkPower", base.settings._FogDarkPower);
		Shader.SetGlobalFloat("SunFogMinDarkness", base.settings._FogMinDarkness);
		if (sun == null)
		{
			sun = RenderSettings.sun;
		}
		if (sun != null)
		{
			Shader.SetGlobalVector("SunDir", -sun.transform.forward);
			Shader.SetGlobalColor("SunLightColor", sun.color);
		}
		context.command.Blit(context.source, context.destination, mat, 0);
	}
}
