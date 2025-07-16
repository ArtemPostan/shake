using UnityEngine;

public class SightToggle : MonoBehaviour
{
	public GameObject gunWithoutSight;

	public GameObject gunWithSight;

	private bool useSight;

	private void Start()
	{
		useSight = false;
		gunWithoutSight.SetActive(value: true);
		gunWithSight.SetActive(value: false);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.T))
		{
			useSight = !useSight;
			if (useSight)
			{
				gunWithoutSight.SetActive(value: false);
				gunWithSight.SetActive(value: true);
			}
			else
			{
				gunWithoutSight.SetActive(value: true);
				gunWithSight.SetActive(value: false);
			}
		}
	}
}
