using FlamingCore;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
	[SerializeField]
	private float speed;

	[SerializeField]
	private float fastSpeed = 10f;

	[SerializeField]
	private float maxSpeed;

	[SerializeField]
	private float jumpSpeed = 2f;

	[SerializeField]
	private FpsCameraArm fpsCameraArm;

	public Transform sideCameraTransform;

	private Rigidbody rb;

	private Vector3 moveDirection;

	private Vector3 velocityTemp;

	private Transform pointer;

	private Combat combat;

	private float fpsPitch;

	public Transform groundChecker;

	public LayerMask groundLayer;

	private bool checkGround;

	public bool floatingMode;

	public bool dashMode;

	public float dashTime = 0.15f;

	private float dashTimer;

	public float dashCoolTime = 0.5f;

	private float dashCoolTimer;

	public float dashSpeed = 20f;

	private Vector3 dashDirection;

	public Transform firstPersonCameraTransform => fpsCameraArm.transform;

	public float MaxSpeed => maxSpeed;

	public Combat Combat => combat;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		combat = GetComponent<Combat>();
	}

	private void Start()
	{
		pointer = GameManager.Instance.LevelManager.Pointer;
	}

	private void Update()
	{
		if (!combat.IsDead && Time.timeScale != 0f)
		{
			switch (GameManager.Instance.LevelManager.game3CType)
			{
			case LevelManager.game3Ctypes.topDown:
				UpdateTopDownMovement();
				break;
			case LevelManager.game3Ctypes.fps:
				UpdateFpsView();
				UpdateFpsMovement();
				break;
			}
		}
	}

	private void FixedUpdate()
	{
		checkGround = CheckGround();
		if (!checkGround && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
		{
			rb.AddForce(-Physics.gravity * 0.3f, ForceMode.Acceleration);
		}
	}

	private bool CheckGround()
	{
		Ray ray = default(Ray);
		ray.direction = Vector3.down;
		ray.origin = groundChecker.position;
		return Physics.Raycast(ray, 0.3f, groundLayer);
	}

	private void UpdateDash(Vector3 directionInput)
	{
		if (!dashMode)
		{
			return;
		}
		if (dashCoolTimer > 0f)
		{
			dashCoolTimer -= Time.deltaTime;
		}
		if (dashTimer > 0f)
		{
			dashTimer -= Time.deltaTime;
		}
		if (Input.GetMouseButtonDown(1) && dashCoolTimer <= 0f)
		{
			dashCoolTimer = dashCoolTime;
			dashTimer = dashTime;
			dashDirection = directionInput;
			if (dashDirection.magnitude < 0.9f)
			{
				dashDirection = base.transform.forward;
			}
			AudioManager.PlaySFXAtPosition("Dash", base.transform.position);
		}
		if (dashTimer > 0f)
		{
			rb.velocity = dashDirection * dashSpeed;
		}
	}

	private void UpdateTopDownMovement()
	{
		Vector3 a = Vector3.zero;
		if (UnityEngine.Input.GetKey(KeyCode.A))
		{
			a += Vector3.left;
		}
		if (UnityEngine.Input.GetKey(KeyCode.D))
		{
			a += Vector3.right;
		}
		if (UnityEngine.Input.GetKey(KeyCode.W))
		{
			a += Vector3.forward;
		}
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			a += Vector3.back;
		}
		a = a.normalized;
		float d = speed;
		if (dashMode)
		{
			d = fastSpeed;
		}
		velocityTemp = (Quaternion.Euler(0f, GameManager.Instance.CameraManager.TopDownCameraArm.transform.rotation.eulerAngles.y, 0f) * a).normalized * d;
		velocityTemp.y = rb.velocity.y;
		rb.velocity = velocityTemp;
		UnityEngine.Debug.DrawLine(base.transform.position, base.transform.position + rb.velocity);
		Vector3 normalized = FCTool.Vector3YToZero(pointer.transform.position - base.transform.position).normalized;
		UpdateDash(normalized);
	}

	private void UpdateFpsMovement()
	{
		Vector3 a = Vector3.zero;
		if (UnityEngine.Input.GetKey(KeyCode.A))
		{
			a += Vector3.left;
		}
		if (UnityEngine.Input.GetKey(KeyCode.D))
		{
			a += Vector3.right;
		}
		if (UnityEngine.Input.GetKey(KeyCode.W))
		{
			a += Vector3.forward;
		}
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			a += Vector3.back;
		}
		a = a.normalized;
		float num = speed;
		if (dashMode)
		{
			num = fastSpeed;
		}
		if ((bool)GameManager.Instance.CameraManager.FpsCameraArm)
		{
			if (GameManager.Instance.CameraManager.FpsCameraArm.Ads)
			{
				num *= 0.7f;
			}
			velocityTemp = (FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.forward).normalized * a.z + FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.right).normalized * a.x).normalized * num;
			velocityTemp.y = rb.velocity.y;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && checkGround)
		{
			velocityTemp.y = jumpSpeed;
		}
		if (UnityEngine.Input.GetKey(KeyCode.Space) && floatingMode && velocityTemp.y < 0f)
		{
			velocityTemp.y = 0f;
		}
		rb.velocity = velocityTemp;
		UpdateDash((FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.forward).normalized * a.z + FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.right).normalized * a.x).normalized);
	}

	private void UpdateFpsView()
	{
		base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y + UnityEngine.Input.GetAxis("mouse x") * fpsCameraArm.Sensitivity, 0f);
		fpsPitch += UnityEngine.Input.GetAxis("mouse y") * (0f - fpsCameraArm.Sensitivity);
		fpsPitch = Mathf.Clamp(fpsPitch, -89.9f, 89.9f);
		firstPersonCameraTransform.localRotation = Quaternion.Euler(fpsPitch, 0f, 0f);
	}

	private Vector3 Vector3YToZero(Vector3 v3)
	{
		return new Vector3(v3.x, 0f, v3.z);
	}
}
