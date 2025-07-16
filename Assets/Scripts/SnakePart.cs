using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Combat))]
public class SnakePart : MonoBehaviour
{
	[HideInInspector]
	public SnakePart lastPart;

	[HideInInspector]
	public SnakePart nextPart;

	[SerializeField]
	private float checkHeadRange = 0.7f;

	private SnakeHead head;

	private Vector3 targetPosition;

	private PlayerControl playerController;

	private bool actived;

	private bool isHead;

	private bool removed;

	private Transform pointer;

	private Rigidbody rb;

	private Vector3 velocityTemp;

	private Collider[] colTemp;

	private Combat combat;

	public bool Actived => actived;

	public bool Removed => removed;

	public bool IsHead => isHead;

	public Combat Combat => combat;

	public void ActivePart(SnakeHead _head)
	{
		head = _head;
		playerController = head.GetComponent<PlayerControl>();
		actived = true;
		base.gameObject.layer = LayerMask.NameToLayer("Player");
		if (combat == null)
		{
			combat = GetComponent<Combat>();
		}
		combat.Active();
		if (!isHead)
		{
			AudioManager.PlaySFXAtPosition("YesSir", base.transform.position);
			AudioManager.PlaySFXAtPosition("Rescue", base.transform.position);
			DataManager.CountFriendlyRescued();
		}
	}

	public void SetTarget(Vector3 target)
	{
		targetPosition = Vector3.Lerp(targetPosition, target, Time.deltaTime * 10f);
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		if (combat == null)
		{
			combat = GetComponent<Combat>();
		}
		if (GetComponent<SnakeHead>() != null)
		{
			isHead = true;
		}
		colTemp = new Collider[1];
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		Remove();
	}

	public void Insert(SnakePart _part)
	{
		_part.lastPart = this;
		_part.nextPart = nextPart;
		if (nextPart != null)
		{
			nextPart.lastPart = _part;
		}
		nextPart = _part;
		_part.ActivePart(head);
	}

	private void Update()
	{
		if (pointer == null && GameManager.Instance != null)
		{
			pointer = GameManager.Instance.LevelManager.Pointer;
		}
	}

	private void FixedUpdate()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		if (!actived && !removed && Physics.OverlapSphereNonAlloc(base.transform.position, 1.3f, colTemp, 1 << LayerMask.NameToLayer("Player")) > 0)
		{
			colTemp[0].GetComponent<SnakePart>().Insert(this);
		}
		if (removed || (!(head == null) && head.Part.Combat.IsDead))
		{
			return;
		}
		velocityTemp = rb.velocity;
		velocityTemp.x = 0f;
		velocityTemp.z = 0f;
		if (!isHead && head != null && actived && !CheckheadAround())
		{
			if (Vector3.Distance(targetPosition, base.transform.position) < playerController.MaxSpeed * Time.fixedDeltaTime)
			{
				velocityTemp = (targetPosition - base.transform.position) / Time.fixedDeltaTime;
			}
			else
			{
				velocityTemp = (targetPosition - base.transform.position).normalized * playerController.MaxSpeed;
			}
			velocityTemp.y = rb.velocity.y;
			rb.velocity = velocityTemp;
		}
	}

	private bool CheckheadAround()
	{
		if (!isHead && Vector3.Distance(head.transform.position, base.transform.position) < checkHeadRange)
		{
			return true;
		}
		return false;
	}

	public void Remove()
	{
		if (!removed && !isHead)
		{
			removed = true;
			if (lastPart != null)
			{
				lastPart.nextPart = nextPart;
			}
			if (nextPart != null)
			{
				nextPart.lastPart = lastPart;
			}
		}
		bool isHead2 = isHead;
	}
}
