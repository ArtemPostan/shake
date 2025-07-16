using UnityEngine;

public class BlackTrigger : MonoBehaviour
{
	private bool triggered;

	public Black black;

	private bool canShow;

	private void Start()
	{
		if ((bool)GameManager.Instance && GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.single && DataManager.data.badAssFinished)
		{
			if (!DataManager.data.theMythDefeated && DataManager.data.firstPersonFinished)
			{
				canShow = (UnityEngine.Random.Range(0f, 100f) < 40f);
			}
			else
			{
				canShow = (UnityEngine.Random.Range(0f, 100f) < 10f);
			}
		}
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (canShow && !triggered && other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			black.gameObject.SetActive(value: true);
		}
	}
}
