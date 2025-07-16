using UnityEngine;

public class FollowerChanger : MonoBehaviour
{
	public GameObject enemyPfb;

	private void Start()
	{
		if (GameManager.Instance != null && GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.single)
		{
			Object.Instantiate(enemyPfb, base.transform.position, Quaternion.Euler(Vector3.zero));
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
	}
}
