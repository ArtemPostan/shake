using UnityEngine;

public class SetActiveByLevelUnlock : MonoBehaviour
{
	public int levelIndex;

	private void Start()
	{
		if (!DataManager.EverEnteredLevel("Level" + levelIndex.ToString()))
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
	}
}
