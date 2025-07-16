using UnityEngine;

public class TopCamera : MonoBehaviour
{
	[HideInInspector]
	public Material blackMaterial;

	[HideInInspector]
	public float brightness;

	private Camera camera;

	public Camera Camera => camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	private void Update()
	{
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (blackMaterial != null)
		{
			blackMaterial.SetFloat("_Brightness", brightness);
			Graphics.Blit(source, destination, blackMaterial);
		}
	}
}
