using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMove : EnemyRobot
{
	[Space]
	[Header("Area")]
	[Space]
	[SerializeField]
	private float moveAreaRadius;

	[SerializeField]
	private float bufferZone;

	[Space]
	[Header("Movement")]
	[Space]
	[SerializeField]
	private float speed;

	[SerializeField]
	private float directionSwitchSpeed = 3f;

	[Space]
	[Header("Tracing")]
	[Space]
	[SerializeField]
	private float farDistance;

	[SerializeField]
	private float nearDistance;

	[SerializeField]
	private Combat combat;

	private Rigidbody rb;

	private Vector3 centerPoint;

	private Vector3 toMoveVector;

	private Vector3 destination;

	private Vector3 limitedDestination;

	private float outRangeState;

	private float tracingState = 1f;

	private float distance;

	private Vector3 currentMoveVelocity;

	[Space]
	[Header("Debug")]
	[Space]
	[SerializeField]
	private bool debug = true;

	[SerializeField]
	private Color moveAreaColor = Color.white;

	[SerializeField]
	private Color bufferZoneColor = Color.white;

	[SerializeField]
	private Color farColor = Color.white;

	[SerializeField]
	private Color nearColor = Color.white;

	private Vector3 velocityTemp;

	private ObsticleChecker obsticleChecker;

	[SerializeField]
	private Animator animator;

	protected override void Start()
	{
		base.Start();
		rb = GetComponent<Rigidbody>();
		centerPoint = base.transform.position;
		combat = GetComponent<Combat>();
		obsticleChecker = GetComponent<ObsticleChecker>();
	}

	public override void Boot()
	{
		base.Boot();
	}

	protected void FixedUpdate()
	{
		if (!combat.IsDead)
		{
			if (target != null && booted && !paused && !obsticleChecker.Obsticle)
			{
				rb.constraints = RigidbodyConstraints.FreezeRotation;
				distance = (target.position - base.transform.position).magnitude;
				if (distance < nearDistance)
				{
					tracingState = Mathf.Lerp(tracingState, -1f, directionSwitchSpeed * Time.fixedDeltaTime);
				}
				else if (distance > farDistance && !obsticleChecker.cliff)
				{
					tracingState = Mathf.Lerp(tracingState, 1f, directionSwitchSpeed * Time.fixedDeltaTime);
				}
				else
				{
					tracingState = Mathf.Lerp(tracingState, 0f, directionSwitchSpeed * Time.fixedDeltaTime);
				}
				destination = base.transform.position + tracingState * (target.position - base.transform.position).normalized * speed * Time.fixedDeltaTime;
				if ((destination - centerPoint).magnitude > (base.transform.position - centerPoint).magnitude)
				{
					limitedDestination = centerPoint + (destination - centerPoint).normalized * (base.transform.position - centerPoint).magnitude;
				}
				else
				{
					limitedDestination = destination;
				}
				outRangeState = Mathf.Lerp(0f, 1f, Mathf.Clamp01(((base.transform.position - centerPoint).magnitude - moveAreaRadius) / bufferZone));
				destination = Vector3.Lerp(destination, limitedDestination, outRangeState);
				toMoveVector = destination - base.transform.position;
				if (Time.fixedDeltaTime != 0f)
				{
					currentMoveVelocity = toMoveVector / Time.fixedDeltaTime;
				}
				currentMoveVelocity.y = rb.velocity.y;
				rb.velocity = currentMoveVelocity;
			}
			else
			{
				rb.constraints = RigidbodyConstraints.FreezeRotation;
				currentMoveVelocity = Vector3.zero;
				currentMoveVelocity.y = rb.velocity.y;
				rb.velocity = currentMoveVelocity;
			}
			if (animator != null)
			{
				animator.SetFloat("Speed", Mathf.Clamp01(rb.velocity.magnitude / 3f));
			}
		}
		else
		{
			rb.constraints = RigidbodyConstraints.None;
			if (animator != null)
			{
				animator.SetFloat("Speed", 1f);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isEditor && debug)
		{
			if (!Application.isPlaying)
			{
				centerPoint = base.transform.position;
			}
			Gizmos.color = moveAreaColor;
			Gizmos.DrawWireSphere(centerPoint, moveAreaRadius);
			Gizmos.color = bufferZoneColor;
			Gizmos.DrawWireSphere(centerPoint, moveAreaRadius + bufferZone);
			Gizmos.color = farColor;
			Gizmos.DrawWireSphere(base.transform.position, farDistance);
			Gizmos.color = nearColor;
			Gizmos.DrawWireSphere(base.transform.position, nearDistance);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(centerPoint, base.transform.position);
		}
	}
}
