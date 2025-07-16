using UnityEngine;

[ExecuteInEditMode]
public class SunFogController : MonoBehaviour
{
	[Space]
	[Header("Sun")]
	[Space]
	[Range(0.0001f, 0.01f)]
	public float _SunSize = 0.00118f;

	[Range(0.0001f, 0.01f)]
	public float _SunEdge = 0.001f;

	public Color _SunColor;

	[Range(0f, 30f)]
	public float _SunIntensity = 1f;

	[Space]
	[Header("SunFog")]
	[Space]
	[Range(0.1f, 10f)]
	public float _SunFogIntensity = 1f;

	[Range(1f, 500f)]
	public float _SunFogPower = 24f;

	[Space]
	[Header("Sky")]
	[Space]
	public Color _SkyTopColor;

	[Range(1f, 10f)]
	public float _SkyTopPower = 1.3f;

	[Range(-1f, 1f)]
	public float _HorizonOffset;

	public Color _SkyColor;

	[Range(0.1f, 30f)]
	public float _SkyIntensity = 1f;

	[Range(0.1f, 100f)]
	[Space]
	[Header("Fog")]
	[Space]
	public float _FogPower = 1f;

	public float _FogHeight = -120.5f;

	public float _FogFade = 96.7f;

	[Range(1E-05f, 1f)]
	public float _HeightDensity = 0.001f;

	[Range(0.1f, 50f)]
	public float _HeightFogPower = 3f;

	[Range(0f, 1f)]
	public float _MaxHeightDensity = 3f;

	public float _FogStartDistance;

	public float _FogEndDistance = 5000f;

	[Space]
	[Header("Cloud")]
	[Space]
	public Cubemap _Cloud;

	[Range(0f, 1f)]
	public float _CloudOpacity = 1f;

	public Color _CloudColor = Color.white;

	[Range(0f, 10f)]
	public float _CloudIntensity = 1f;

	public Vector4 _CloudUVOffset;

	public Transform sun;

	private void Update()
	{
	}

	private void Awake()
	{
		SetValues();
	}

	private void SetValues()
	{
		_FogEndDistance = Mathf.Max(_FogStartDistance, _FogEndDistance);
		Shader.SetGlobalFloat("SunPower", _SunFogPower);
		Shader.SetGlobalFloat("SunSize", _SunSize);
		Shader.SetGlobalFloat("SunEdge", _SunEdge);
		Shader.SetGlobalColor("SunColor", _SunColor * _SunFogIntensity);
		Shader.SetGlobalFloat("SunIntensity", _SunIntensity);
		Shader.SetGlobalFloat("SunFogIntensity", _SunFogIntensity);
		Shader.SetGlobalColor("SkyColor", _SkyColor * _SkyIntensity);
		Shader.SetGlobalColor("SkyTopColor", _SkyTopColor);
		Shader.SetGlobalFloat("HorizonOffset", _HorizonOffset);
		Shader.SetGlobalFloat("SkyTopPower", _SkyTopPower);
		Shader.SetGlobalFloat("FogHeight", _FogHeight);
		Shader.SetGlobalFloat("FogFade", _FogFade);
		Shader.SetGlobalFloat("HeightDensity", _HeightDensity);
		Shader.SetGlobalFloat("HeightFogPower", _HeightFogPower);
		Shader.SetGlobalFloat("MaxHeightDensity", _MaxHeightDensity);
		Shader.SetGlobalFloat("FogStartDepth", _FogStartDistance);
		Shader.SetGlobalFloat("FogEndDepth", _FogEndDistance);
		Shader.SetGlobalFloat("FogPower", _FogPower);
		Shader.SetGlobalTexture("Cloud", _Cloud);
		Shader.SetGlobalFloat("CloudOpacity", _CloudOpacity);
		Shader.SetGlobalColor("CloudColor", _CloudColor * _CloudIntensity);
		Shader.SetGlobalVector("CloudUVOffset", _CloudUVOffset);
		Shader.SetGlobalVector("SunDir", -sun.transform.forward);
	}

	private void OnValidate()
	{
		SetValues();
	}
}
