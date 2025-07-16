using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostManager : MonoBehaviour
{
	public PostProcessVolume hurtVolume;

	private float hurtValue;

	public float hurtCoolSpeed = 6f;

	private void Start()
	{
		hurtValue = 0f;
		//UpdateHurtPost();
	}

	private void Update()
	{
		if (hurtValue > 0f)
		{
			hurtValue = Mathf.Clamp01(hurtValue - Time.unscaledDeltaTime * hurtCoolSpeed);
			UpdateHurtPost();
		}
	}

	private void InitPostSettings()
	{
		hurtValue = 0f;
	}

	public void StartHurt()
	{
		hurtValue = Mathf.Clamp01(hurtValue + 0.3f);
	}

	public void UpdateHurtPost()
	{
		//hurtVolume.weight = hurtValue;
	}
}
