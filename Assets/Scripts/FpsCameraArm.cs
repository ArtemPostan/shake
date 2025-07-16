using UnityEngine;

public class FpsCameraArm : MonoBehaviour
{
	[SerializeField]
	private Camera virtualCamera;

	[SerializeField]
	private Transform gunPoint;

	[SerializeField]
	private Transform normalGunTransform;

	[SerializeField]
	private Transform adsgunTransform;

	private float startFov;

	[SerializeField]
	private float adsFovGain = 5f;

	[SerializeField]
	private float adsTime = 0.2f;

	private bool ads;

	private float adsMoveSpeed;

	private float fovSpeed;

	private float adsSensitivity;

	private float sensitivitySpeed;

	private float sensitivity;

	[SerializeField]
	private Transform aimMarker;

	public GunRecoil recoil;

	public Camera VirtualCamera => virtualCamera;

	public float AdsTime => adsTime;

	public bool Ads => ads;

	private bool holdToAim => DataManager.data.holdToAim;

	private float normalSensitivity => DataManager.data.mouseSensitivity / 20f;

	public float Sensitivity => sensitivity;

	private void Awake()
	{
		VirtualCamera.enabled = false;
		startFov = virtualCamera.fieldOfView;
	}

	private void Update()
	{
		if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
		{
			if (ads)
			{
				if (aimMarker.gameObject.activeInHierarchy)
				{
					aimMarker.gameObject.SetActive(value: false);
				}
			}
			else if (!aimMarker.gameObject.activeInHierarchy)
			{
				aimMarker.gameObject.SetActive(value: true);
			}
		}
		else if (aimMarker.gameObject.activeInHierarchy)
		{
			aimMarker.gameObject.SetActive(value: false);
		}
		adsMoveSpeed = Vector3.Distance(normalGunTransform.position, adsgunTransform.position) / adsTime;
		fovSpeed = adsFovGain / adsTime;
		adsSensitivity = normalSensitivity;
		sensitivitySpeed = (normalSensitivity - adsSensitivity) / adsTime;
		if (!holdToAim && Input.GetMouseButtonDown(1) && !LevelManager.Paused)
		{
			ads = !ads;
		}
		else if (holdToAim)
		{
			ads = Input.GetMouseButton(1);
		}
		if (LevelManager.instance.Player.Combat.Gun.IsMeleeWeapon)
		{
			ads = false;
		}
		if (ads)
		{
			gunPoint.position = Vector3.MoveTowards(gunPoint.position, adsgunTransform.position, adsMoveSpeed * Time.deltaTime);
			VirtualCamera.fieldOfView = Mathf.MoveTowards(VirtualCamera.fieldOfView, startFov - adsFovGain, fovSpeed * Time.deltaTime);
			sensitivity = Mathf.MoveTowards(sensitivity, adsSensitivity, sensitivitySpeed * Time.deltaTime);
		}
		else
		{
			gunPoint.position = Vector3.MoveTowards(gunPoint.position, normalGunTransform.position, adsMoveSpeed * Time.deltaTime);
			VirtualCamera.fieldOfView = Mathf.MoveTowards(VirtualCamera.fieldOfView, startFov, fovSpeed * Time.deltaTime);
			sensitivity = Mathf.MoveTowards(sensitivity, normalSensitivity, sensitivitySpeed * Time.deltaTime);
		}
		sensitivity = Mathf.Clamp(sensitivity, normalSensitivity, adsSensitivity);
	}

	private void Start()
	{
		GameManager.Instance.CameraManager.SetFpsCamera(this);
	}
}
