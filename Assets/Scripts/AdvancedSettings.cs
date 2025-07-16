using System;
using UnityEngine;

[Serializable]
public class AdvancedSettings
{
	[Tooltip("[Optimization] Enables material's GPU Instancing property")]
	public bool enableGPUInstancing;

	[Tooltip("Index for objects with multiple materials")]
	public int subMaterialIndex;

	[Tooltip("[Optimization] Check this box if the scale of your model is globalScale (1,1,1)")]
	public bool unitLocalScale;

	[Tooltip("[Optimization] Angular velocity threshold value before which the effects will not be rendered.")]
	public float AngularVelocityCutoff;
}
