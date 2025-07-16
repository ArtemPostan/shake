using UnityEngine;

public class HideOnStart : MonoBehaviour
{
	private void Start()
	{
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
	}
}
