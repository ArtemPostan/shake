using System;
using UnityEngine;

[ExecuteInEditMode]
public class SunFog : MonoBehaviour
{
	public Material mat;

	[Space]
	[Header("Sun")]
	[Space]
	[Range(0.001f, 0.01f)]
	public float _SunSize = 0.00118f;

	[Range(0.001f, 0.01f)]
	public float _SunEdge = 0.001f;

	public Color _SunColor;

	[Range(0.1f, 30f)]
	public float _SunIntensity = 1f;

	[Space]
	[Header("SunFog")]
	[Space]
	[Range(0.1f, 10f)]
	public float _SunFogIntensity = 1f;

	[Range(3f, 500f)]
	public float _SunFogPower = 24f;

	[Space]
	[Header("Sky")]
	[Space]
	public Color _SkyColor;

	[Range(0.1f, 10f)]
	public float _SkyIntensity = 1f;

	public Color _SkyTopColor;

	[Range(0.1f, 2f)]
	public float _SkyTopPower = 1.3f;

	[Range(0.1f, 10f)]
	[Space]
	[Header("Fog")]
	[Space]
	public float _FogPower = 1f;

	public float _FogHeight = -120.5f;

	public float _FogFade = 96.7f;

	[Range(0.01f, 1f)]
	public float _HeightDensity = 0.098f;

	[Range(0f, 1f)]
	public float _FogStartDepth;

	[Range(0f, 1f)]
	public float _FogEndDepth = 1f;

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

	private Camera cam;

	private float Angle;

	private float Hipo;

	private Vector3 Sc_Center;

	private Vector3 Sc_CameraPosition;

	private Vector3 X_Vector;

	private Vector3 Y_Vector;

	private void Start()
	{
		SetValues();
	}

	private void Update()
	{
	}

	private void Calculate()
	{
		Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width, 0f));
		Ray ray2 = cam.ScreenPointToRay(new Vector3(0f, Screen.height));
		Ray ray3 = cam.ScreenPointToRay(new Vector3(0f, 0f));
		Angle = Vector3.Angle(to: cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)).direction, from: ray.direction);
		Hipo = cam.farClipPlane / Mathf.Cos((float)Math.PI * Angle / 180f);
		X_Vector = Hipo * ray.direction - Hipo * ray3.direction;
		Y_Vector = Hipo * ray2.direction - Hipo * ray3.direction;
		Sc_Center = Hipo * ray3.direction;
		Sc_CameraPosition = base.transform.position;
	}

	private void SetValues()
	{
		mat.SetFloat("_SunPower", _SunFogPower);
		mat.SetFloat("_SunSize", _SunSize);
		mat.SetFloat("_SunEdge", _SunEdge);
		mat.SetColor("_SunColor", _SunColor * _SunFogIntensity);
		mat.SetFloat("_SunIntensity", _SunIntensity);
		mat.SetFloat("_SunFogIntensity", _SunFogIntensity);
		mat.SetColor("_SkyColor", _SkyColor * _SkyIntensity);
		mat.SetColor("_SkyTopColor", _SkyTopColor);
		mat.SetFloat("_SkyTopPower", _SkyTopPower);
		mat.SetFloat("_FogHeight", _FogHeight);
		mat.SetFloat("_FogFade", _FogFade);
		mat.SetFloat("_HeightDensity", _HeightDensity);
		mat.SetFloat("_FogStartDepth", _FogStartDepth);
		mat.SetFloat("_FogEndDepth", _FogEndDepth);
		mat.SetFloat("_FogPower", _FogPower);
		mat.SetTexture("_Cloud", _Cloud);
		mat.SetFloat("_CloudOpacity", _CloudOpacity);
		mat.SetColor("_CloudColor", _CloudColor * _CloudIntensity);
		mat.SetVector("_CloudUVOffset", _CloudUVOffset);
	}

	private void OnValidate()
	{
		SetValues();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (cam == null)
		{
			cam = base.gameObject.GetComponent<Camera>();
			cam.depthTextureMode = DepthTextureMode.Depth;
		}
		Calculate();
		mat.SetVector("_Vector_X", new Vector4(X_Vector.x, X_Vector.y, X_Vector.z, 0f));
		mat.SetVector("_Vector_Y", new Vector4(Y_Vector.x, Y_Vector.y, Y_Vector.z, 0f));
		mat.SetVector("_Screem_Center", new Vector4(Sc_Center.x, Sc_Center.y, Sc_Center.z, 0f));
		mat.SetVector("_CameraPosition", new Vector4(Sc_CameraPosition.x, Sc_CameraPosition.y, Sc_CameraPosition.z, 0f));
		Graphics.Blit(source, destination, mat);
	}
}
