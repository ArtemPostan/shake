using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[PostProcess(typeof(AmbientControlRenderer), PostProcessEvent.AfterStack, "Custom/AmbientControl", true)]
public sealed class AmbientControl : PostProcessEffectSettings
{
	[ColorUsage(false, true)]
	[Tooltip("SkyColor")]
	public ColorParameter SkyColor = new ColorParameter
	{
		value = new Color(0.212f, 0.227f, 0.259f)
	};

	[ColorUsage(false, true)]
	[Tooltip("EquatorColor")]
	public ColorParameter EquatorColor = new ColorParameter
	{
		value = new Color(0.114f, 0.125f, 0.133f)
	};

	[ColorUsage(false, true)]
	[Tooltip("GroundColor")]
	public ColorParameter GroundColor = new ColorParameter
	{
		value = new Color(0.047f, 0.043f, 0.035f)
	};

	[Header("角色边缘光")]
	[ColorUsage(false, true)]
	[Tooltip("角色边缘光颜色")]
	public ColorParameter edgeColor = new ColorParameter
	{
		value = new Color(0.5f, 0.5f, 0.5f)
	};

	[Header("环境")]
	public BoolParameter directionalLight = new BoolParameter
	{
		value = true
	};

	public FloatParameter bgmVolumeGain = new FloatParameter
	{
		value = 0f
	};
}
