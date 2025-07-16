using UnityEngine;

public class EnemyRobot : MonoBehaviour
{
	[Header("Robot")]
	[Space]
	public Transform target;

	public bool autoBoot;

	protected bool booted;

	[HideInInspector]
	public bool paused;

	protected virtual void Start()
	{
		if (autoBoot)
		{
			Boot();
		}
	}

	protected virtual void Update()
	{
		if (target == null && GameManager.Instance.LevelManager.Player != null)
		{
			target = GameManager.Instance.LevelManager.Player.transform;
		}
	}

	public virtual void Boot()
	{
		booted = true;
	}
}
