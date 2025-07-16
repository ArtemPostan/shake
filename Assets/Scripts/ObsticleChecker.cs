using UnityEngine;

public class ObsticleChecker : MonoBehaviour
{
	private LayerMask wallLayer;

	private RaycastHit[] hit;

	private Transform target;

	[HideInInspector]
	public bool Obsticle;

	public float checkDistance;

	[HideInInspector]
	public bool cliff;

	public CapsuleCollider collider;

	public float cliffCheckLength = 0.2f;

	public float cliffCheckAheadDist = 1f;

	private bool gameStarted;

	private Vector3 footPos;

	private void Start()
	{
		UpdateChecker();
	}

	private void UpdateChecker()
	{
		Vector3 max = collider.bounds.max;
		Vector3 min = collider.bounds.min;
		footPos = new Vector3((max.x + min.x) / 2f, min.y, (max.z + min.z) / 2f);
	}

	private void Update()
	{
		if (!gameStarted && GameManager.Instance.LevelManager.GameState == LevelManager.gameStates.playing)
		{
			gameStarted = true;
			target = GameManager.Instance.LevelManager.Player.transform;
			wallLayer = GameManager.Instance.LayerManager.WallLayer;
		}
		if (!gameStarted)
		{
			return;
		}
		if (Vector3.Distance(target.position, base.transform.position) < checkDistance)
		{
			Obsticle = Physics.Raycast(base.transform.position, target.position - base.transform.position, (target.position - base.transform.position).magnitude, wallLayer);
			if (Obsticle)
			{
				UnityEngine.Debug.DrawLine(base.transform.position, target.position, Color.red);
			}
			else
			{
				UnityEngine.Debug.DrawLine(base.transform.position, target.position, Color.green);
			}
		}
		else
		{
			Obsticle = true;
		}
		UpdateChecker();
		Vector3 normalized = (target.position - base.transform.position).normalized;
		Vector3 origin = footPos + normalized * (collider.bounds.extents.x + cliffCheckAheadDist);
		origin.y = footPos.y + 0.1f;
		normalized = Quaternion.Euler(0f, 30f, 0f) * normalized;
		Vector3 origin2 = footPos + normalized * (collider.bounds.extents.x + cliffCheckAheadDist);
		origin2.y = footPos.y + 0.1f;
		normalized = Quaternion.Euler(0f, -60f, 0f) * normalized;
		Vector3 origin3 = footPos + normalized * (collider.bounds.extents.x + cliffCheckAheadDist);
		origin3.y = footPos.y + 0.1f;
		cliff = (!Physics.Raycast(origin, Vector3.down, cliffCheckLength, wallLayer) || !Physics.Raycast(origin2, Vector3.down, cliffCheckLength, wallLayer) || !Physics.Raycast(origin3, Vector3.down, cliffCheckLength, wallLayer));
	}

	private void OnDrawGizmos()
	{
		if ((bool)collider)
		{
			Gizmos.color = Color.yellow;
			Vector3 max = collider.bounds.max;
			Vector3 min = collider.bounds.min;
			Vector3 vector = new Vector3((max.x + min.x) / 2f, min.y, (max.z + min.z) / 2f);
			Gizmos.DrawCube(vector, Vector3.one * 0.01f);
			Gizmos.color = Color.green;
			Gizmos.DrawCube(vector + (collider.bounds.extents.x + cliffCheckAheadDist) * base.transform.forward, Vector3.one * 0.01f);
			Vector3 vector2 = Quaternion.Euler(0f, 30f, 0f) * base.transform.forward;
			Gizmos.DrawCube(vector + (collider.bounds.extents.x + cliffCheckAheadDist) * vector2, Vector3.one * 0.01f);
			vector2 = Quaternion.Euler(0f, -60f, 0f) * vector2;
			Gizmos.DrawCube(vector + (collider.bounds.extents.x + cliffCheckAheadDist) * vector2, Vector3.one * 0.01f);
		}
	}
}
