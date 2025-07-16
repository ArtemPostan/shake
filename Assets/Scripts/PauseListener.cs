using UnityEngine;

public class PauseListener : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			LevelManager.Paused = !LevelManager.Paused;
		}
	}
}
