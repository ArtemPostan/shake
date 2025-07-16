using UnityEngine;

public class LayerManager : MonoBehaviour
{
	[SerializeField]
	private LayerMask playerLayer;

	[SerializeField]
	private LayerMask wallLayer;

	[SerializeField]
	private LayerMask enemyLayer;

	[SerializeField]
	private LayerMask unsavedBroLayer;

	[SerializeField]
	private LayerMask groundLayer;

	[SerializeField]
	private LayerMask rigidBodyLayer;

	[SerializeField]
	private LayerMask deadBodyLayer;

	[SerializeField]
	private LayerMask breakableLayer;

	[SerializeField]
	private LayerMask hurtLayer;

	public LayerMask PlayerLayer => playerLayer;

	public LayerMask WallLayer => wallLayer;

	public LayerMask EnemyLayer => enemyLayer;

	public LayerMask UnsavedBroLayer => unsavedBroLayer;

	public LayerMask GroundLayer => groundLayer;

	public LayerMask RigidBodyLayer => rigidBodyLayer;

	public LayerMask DeadBodyLayer => deadBodyLayer;

	public LayerMask BreakableLayer => breakableLayer;

	public LayerMask HurtLayer => hurtLayer;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
