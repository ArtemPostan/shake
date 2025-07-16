using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(CinemachineBrain))]
public class SideCamera : MonoBehaviour
{
	private Camera _camera;

	private CinemachineBrain _brain;

	[SerializeField]
	private CinemachineVirtualCameraBase[] vCams;

	[SerializeField]
	private int index;

	[SerializeField]
	private int targetDisplay;

	private float cd = 0.2f;

	private float lastChange = float.MinValue;

	private CinemachineVirtualCameraBase currentCam
	{
		get
		{
			if (index < 0)
			{
				index = 0;
			}
			else if (index >= vCams.Length)
			{
				index = vCams.Length - 1;
			}
			return vCams[index];
		}
	}

	private Camera camera
	{
		get
		{
			if (!_camera)
			{
				_camera = GetComponent<Camera>();
			}
			return _camera;
		}
	}

	private CinemachineBrain brain
	{
		get
		{
			if (!_brain)
			{
				_brain = GetComponent<CinemachineBrain>();
			}
			return _brain;
		}
	}

	private float timeSinceLastChange => Time.unscaledTime - lastChange;

	private void Awake()
	{
		OnValidate();
		RefreshBrainTarget();
	}

	private void OnValidate()
	{
		camera.targetDisplay = targetDisplay;
		RefreshBrainTarget();
	}

	private void Update()
	{
		HandleInput();
	}

	private void NextCam()
	{
		index++;
		if (index >= vCams.Length)
		{
			index = 0;
		}
		RefreshBrainTarget();
	}

	private void PrevCam()
	{
		index--;
		if (index < 0)
		{
			index = vCams.Length - 1;
		}
		RefreshBrainTarget();
	}

	public void HandleInput()
	{
		if (timeSinceLastChange > cd)
		{
			float axis = UnityEngine.Input.GetAxis("CamSelect");
			if (axis > 0f)
			{
				NextCam();
				lastChange = Time.unscaledTime;
			}
			else if (axis < 0f)
			{
				PrevCam();
				lastChange = Time.unscaledTime;
			}
		}
	}

	public void RefreshBrainTarget()
	{
		brain.SetCameraOverride(0, currentCam, currentCam, 1f, 0f);
	}
}
