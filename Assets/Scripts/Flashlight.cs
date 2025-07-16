using UnityEngine;

public class Flashlight : MonoBehaviour
{
	public Light flashLight;

	private void Start()
	{
		if (GameManager.Instance.LevelManager.levelInfo.needFlashlight)
		{
			flashLight.enabled = true;
		}
		else
		{
			flashLight.enabled = false;
		}
	}

	private void Update()
	{
	}
}
