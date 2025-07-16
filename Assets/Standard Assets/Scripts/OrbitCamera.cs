using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCameraBase))]
public class OrbitCamera : MonoBehaviour
{
	private CinemachineVirtualCameraBase _vcam;

	public Transform target;

	[Header("Configuration")]
	public float minPitchAngle;

	public float maxPitchAngle = 89f;

	public float minDistance = 1f;

	public float maxDistance = 10f;

	public float rotateSpeed = 100f;

	public float zoomSpeed = 10f;

	public float rotateAcceleration = 360f;

	public float zoomAcceleration = 10f;

	private float currentYawSpeed;

	private float currentPitchSpeed;

	private float currentZoomSpeed;

	[Header("Status")]
	public float yawAngle;

	public float pitchAngle = 45f;

	public float zoom = 10f;

	[Header("Controls")]
	public Vector2 axisRotation;

	public float axisZoom;

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
			HandleZoom();
			EvaluatePosition();
		}
	}

	private void HandleInput()
	{
		axisRotation.x = 0f - UnityEngine.Input.GetAxis("CamRotateX");
		axisRotation.y = UnityEngine.Input.GetAxis("CamRotateY");
		axisZoom = UnityEngine.Input.GetAxis("CamMovementAscend") - UnityEngine.Input.GetAxis("CamMovementDescend");
	}

	private void HandleRotation()
	{
		float num = axisRotation.x * rotateSpeed;
		float num2 = axisRotation.y * rotateSpeed;
		currentYawSpeed = Mathf.MoveTowards(currentYawSpeed, num, rotateAcceleration * Time.unscaledDeltaTime);
		currentPitchSpeed = Mathf.MoveTowards(currentPitchSpeed, num2, rotateAcceleration * Time.unscaledDeltaTime);
		yawAngle += currentYawSpeed * Time.unscaledDeltaTime;
		pitchAngle += currentPitchSpeed * Time.unscaledDeltaTime;
		if (pitchAngle > maxPitchAngle && currentPitchSpeed > 0f)
		{
			currentPitchSpeed = 0f;
			pitchAngle = maxPitchAngle;
		}
		if (pitchAngle < minPitchAngle && currentPitchSpeed < 0f)
		{
			currentPitchSpeed = 0f;
			pitchAngle = minPitchAngle;
		}
		if (yawAngle > 360f)
		{
			yawAngle -= 360f;
		}
		else if (yawAngle < -360f)
		{
			yawAngle += 360f;
		}
		if (pitchAngle > 360f)
		{
			pitchAngle -= 360f;
		}
		if (pitchAngle < -360f)
		{
			pitchAngle += 360f;
		}
	}

	private void HandleZoom()
	{
		float num = axisZoom * zoomSpeed;
		currentZoomSpeed = Mathf.MoveTowards(currentZoomSpeed, num, zoomAcceleration * Time.unscaledDeltaTime);
		zoom += currentZoomSpeed;
		if (zoom > maxDistance)
		{
			if (currentZoomSpeed > 0f)
			{
				currentZoomSpeed = 0f;
			}
			zoom = maxDistance;
		}
		if (zoom < minDistance)
		{
			if (zoomSpeed < 0f)
			{
				currentZoomSpeed = 0f;
			}
			zoom = minDistance;
		}
	}

	private void EvaluatePosition()
	{
		if ((bool)target)
		{
			Vector3 a = Quaternion.AngleAxis(yawAngle, Vector3.up) * (Quaternion.AngleAxis(pitchAngle, -Vector3.right) * Vector3.forward);
			Vector3 b = a * zoom;
			Quaternion rotation = Quaternion.LookRotation(-a, Vector3.up);
			base.transform.rotation = rotation;
			base.transform.position = target.position + b;
		}
	}
}
