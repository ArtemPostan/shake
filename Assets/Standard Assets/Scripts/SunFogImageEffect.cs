using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class SunFogImageEffect : MonoBehaviour
{
	private Camera cam;

	private static Material _mat;

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

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (cam == null)
		{
			cam = base.gameObject.GetComponent<Camera>();
			cam.depthTextureMode = DepthTextureMode.Depth;
		}
		Graphics.Blit(source, destination, mat);
	}
}
