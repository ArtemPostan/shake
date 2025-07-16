using UnityEngine;

public class WideScreen : MonoBehaviour
{
	private void Start()
	{
		Resolution currentResolution = Screen.currentResolution;
		Rect rect = new Rect(0f, (1f - 9f * (float)currentResolution.width / (21f * (float)currentResolution.height)) / 2f, 1f, 9f * (float)currentResolution.width / (21f * (float)currentResolution.height));
		GetComponent<Camera>().rect = rect;
	}

	private void OnEnable()
	{
		Resolution currentResolution = Screen.currentResolution;
		Rect rect = new Rect(0f, (1f - 9f * (float)currentResolution.width / (21f * (float)currentResolution.height)) / 2f, 1f, 9f * (float)currentResolution.width / (21f * (float)currentResolution.height));
		GetComponent<Camera>().rect = rect;
	}

	private void OnDisable()
	{
		Rect rect = new Rect(0f, 0f, 1f, 1f);
		GetComponent<Camera>().rect = rect;
	}

	private void Update()
	{
	}
}
