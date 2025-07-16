using UnityEngine;

public class HideIfSinglePlayer : MonoBehaviour
{
	public GameObject target;

	public bool inverse;

	private void Start()
	{
		if ((bool)target && (bool)GameManager.Instance && GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.single)
		{
			target.SetActive(inverse);
		}
	}

	private void Update()
	{
	}
}
