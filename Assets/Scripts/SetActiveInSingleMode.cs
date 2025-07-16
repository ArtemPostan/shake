using UnityEngine;

public class SetActiveInSingleMode : MonoBehaviour
{
	public enum ObjectActiveType
	{
		RESCUE_AND_SINGLE,
		ONLY_IN_SINGLE
	}

	public ObjectActiveType activeType;

	private void Start()
	{
		if (activeType == ObjectActiveType.ONLY_IN_SINGLE && GameManager.Instance != null && GameManager.Instance.LevelManager.gameMode != LevelManager.gameModes.single)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnDrawGizmos()
	{
		if (activeType == ObjectActiveType.ONLY_IN_SINGLE)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(base.transform.position + Vector3.up * 2f, 0.3f);
		}
	}
}
