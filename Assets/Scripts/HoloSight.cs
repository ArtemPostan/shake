using UnityEngine;

[ExecuteInEditMode]
public class HoloSight : MonoBehaviour
{
	public Transform sightMarker;

	public Renderer renderer;

	private MaterialPropertyBlock prop;

	private void Start()
	{
	}

	[ExecuteInEditMode]
	private void Update()
	{
		if (prop == null)
		{
			prop = new MaterialPropertyBlock();
		}
		if ((bool)sightMarker && (bool)renderer)
		{
			prop.SetVector("_SightForward", sightMarker.forward);
			prop.SetVector("_SightUp", sightMarker.up);
			prop.SetVector("_SightRight", sightMarker.right);
			prop.SetVector("_SightCenterPos", sightMarker.position);
			renderer.SetPropertyBlock(prop);
		}
	}
}
