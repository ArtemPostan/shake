using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[PostProcess(typeof(SunFogControllerPostProcessingStackRenderer), PostProcessEvent.BeforeTransparent, "Custom/SunFog", true)]
public class SunFogControllerPostProcessingStack : PostProcessEffectSettings
{
	[Space]
	[Header("Sun")]
	[Space]
	[Range(0.0001f, 0.01f)]
	public FloatParameter _SunSize = new FloatParameter
	{
		value = 0.00118f
	};

	[Range(0.0001f, 0.01f)]
	public FloatParameter _SunEdge = new FloatParameter
	{
		value = 0.001f
	};

	public ColorParameter _SunColor = new ColorParameter
	{
		value = Color.white
	};

	[UnityEngine.Rendering.PostProcessing.Min(0f)]
	public FloatParameter _SunIntensity = new FloatParameter
	{
		value = 1f
	};

	[Space]
	[Header("SunFog")]
	[Space]
	[UnityEngine.Rendering.PostProcessing.Min(0f)]
	public FloatParameter _SunFogIntensity = new FloatParameter
	{
		value = 1f
	};

	[UnityEngine.Rendering.PostProcessing.Min(0.01f)]
	public FloatParameter _SunFogPower = new FloatParameter
	{
		value = 24f
	};

	[Space]
	[Header("Sky")]
	[Space]
	public ColorParameter _SkyTopColor = new ColorParameter
	{
		value = Color.white
	};

	[Range(1f, 10f)]
	public FloatParameter _SkyTopPower = new FloatParameter
	{
		value = 1.3f
	};

	[Range(-1f, 1f)]
	public FloatParameter _HorizonOffset = new FloatParameter
	{
		value = 0f
	};

	public ColorParameter _SkyColor = new ColorParameter
	{
		value = Color.white
	};

	[Range(0.1f, 30f)]
	public FloatParameter _SkyIntensity = new FloatParameter
	{
		value = 1f
	};

	[Range(0.1f, 100f)]
	[Space]
	[Header("Fog")]
	[Space]
	public FloatParameter _FogPower = new FloatParameter
	{
		value = 1f
	};

	public FloatParameter _FogHeight = new FloatParameter
	{
		value = -120.5f
	};

	public FloatParameter _FogFade = new FloatParameter
	{
		value = 96.7f
	};

	[Range(1E-05f, 1f)]
	public FloatParameter _HeightDensity = new FloatParameter
	{
		value = 0.001f
	};

	[Range(0.1f, 50f)]
	public FloatParameter _HeightFogPower = new FloatParameter
	{
		value = 3f
	};

	[Range(0f, 1f)]
	public FloatParameter _MaxHeightDensity = new FloatParameter
	{
		value = 3f
	};

	public FloatParameter _FogStartDistance = new FloatParameter
	{
		value = 0f
	};

	public FloatParameter _FogEndDistance = new FloatParameter
	{
		value = 5000f
	};

	public ColorParameter _FogDarkColor = new ColorParameter
	{
		value = Color.black
	};

	[UnityEngine.Rendering.PostProcessing.Min(0.001f)]
	public FloatParameter _FogDarkPower = new FloatParameter
	{
		value = 1f
	};

	[Range(0f, 1f)]
	public FloatParameter _FogMinDarkness = new FloatParameter
	{
		value = 1f
	};
}
