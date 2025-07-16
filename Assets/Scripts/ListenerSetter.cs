using UnityEngine;

public class ListenerSetter : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if ((bool)GameManager.Instance && (bool)GameManager.Instance.LevelManager && (bool)GameManager.Instance.LevelManager.Player)
		{
			base.transform.position = GameManager.Instance.LevelManager.Player.transform.position;
		}
		base.transform.rotation = Camera.main.transform.rotation;
		if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.topDown)
		{
			base.transform.position += Vector3.up * 2f;
		}
	}
}
