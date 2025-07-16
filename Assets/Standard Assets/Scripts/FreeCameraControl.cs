using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCameraBase))]
public class FreeCameraControl : MonoBehaviour
{
	private CinemachineVirtualCameraBase _vcam;

	[SerializeField]
	private float moveSpeed = 20f;

	[SerializeField]
	private float rotateSpeed = 100f;

	[SerializeField]
	private float maxXRotation = 89f;

	[SerializeField]
	private float minXRotation = -89f;

	[SerializeField]
	private float movementAcceleration = 20f;

	[SerializeField]
	private float rotateAcceleration = 100f;

	private Vector3 currentSpeed;

	private float pitchSpeed;

	private float yawSpeed;

	private float pitch;

	[Header("Controls")]
	public Vector2 axisA;

	public Vector2 axisB;

	public float axisAscend;

	private CinemachineVirtualCameraBase vcam
	{
		get
		{
			if (!_vcam)
			{
				_vcam = GetComponent<CinemachineVirtualCameraBase>();
			}
			return _vcam;
		}
	}

	private void LateUpdate()
	{
		if (CinemachineCore.Instance.IsLive(vcam))
		{
			HandleInput();
			HandleRotation();
			HandleMovement();
		}
	}

	private void HandleInput()
	{
		axisB.x = UnityEngine.Input.GetAxis("CamRotateX");
		axisB.y = UnityEngine.Input.GetAxis("CamRotateY");
		axisA.x = UnityEngine.Input.GetAxis("CamMovementX");
		axisA.y = UnityEngine.Input.GetAxis("CamMovementY");
		axisAscend = UnityEngine.Input.GetAxis("CamMovementAscend") - UnityEngine.Input.GetAxis("CamMovementDescend");
	}

	private void HandleRotation()
	{
		float target = axisB.x * rotateSpeed;
		float target2 = axisB.y * rotateSpeed;
		yawSpeed = Mathf.MoveTowards(yawSpeed, target, rotateAcceleration * Time.unscaledDeltaTime);
		pitchSpeed = Mathf.MoveTowards(pitchSpeed, target2, rotateAcceleration * Time.unscaledDeltaTime);
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.y += yawSpeed * Time.unscaledDeltaTime;
		eulerAngles.x += pitchSpeed * Time.unscaledDeltaTime;
		if (eulerAngles.x > 180f)
		{
			eulerAngles.x -= 360f;
		}
		if (eulerAngles.x < -180f)
		{
			eulerAngles.x += 360f;
		}
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, minXRotation, maxXRotation);
		base.transform.rotation = Quaternion.Euler(eulerAngles);
	}

	private void HandleMovement()
	{
		Vector3 up = Vector3.up;
		Vector3 vector = Vector3.ProjectOnPlane(base.transform.forward, up);
		Vector3 a = Quaternion.AngleAxis(90f, up) * vector;
		Vector3 target = (axisA.x * a + axisA.y * vector + axisAscend * up) * moveSpeed;
		currentSpeed = Vector3.MoveTowards(currentSpeed, target, movementAcceleration * Time.unscaledDeltaTime);
		base.transform.Translate(currentSpeed * Time.unscaledDeltaTime, Space.World);
	}
}
