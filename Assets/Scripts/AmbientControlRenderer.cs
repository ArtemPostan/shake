using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public sealed class AmbientControlRenderer : PostProcessEffectRenderer<AmbientControl>
{
	public override void Render(PostProcessRenderContext context)
	{
		RenderSettings.ambientSkyColor = base.settings.SkyColor.value;
		RenderSettings.ambientEquatorColor = base.settings.EquatorColor.value;
		RenderSettings.ambientGroundColor = base.settings.GroundColor.value;
		Shader.SetGlobalColor("EdgeColor", base.settings.edgeColor.value);
		if ((bool)RenderSettings.sun)
		{
			Shader.SetGlobalVector("SunDir", -RenderSettings.sun.transform.forward);
			RenderSettings.sun.enabled = base.settings.directionalLight;
		}
		AudioManager.BGMAttenuation = base.settings.bgmVolumeGain.value;
		context.command.Blit(context.source, context.destination);
	}
}
