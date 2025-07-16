using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
	public Transform gunModel;

	private Vector3 startPos;

	private Vector3 endPos;

	private Quaternion startLocalRotation;

	private Vector3 endLocalRotationEuler;

	public float backDistance = 0.2f;

	public float recoilSpeed = 15f;

	public float recoverSpeed = 6f;

	public Vector3 minRotation;

	public Vector3 maxRotation;

	private float recoilValue;

	private bool recoilling;

	private bool started;

	private float startRecoilValue;

	private Quaternion startRecoilRotation;

	public bool triggerCameraRecoil;

	public List<GunRecoil> subRecoils;

	public ParticleSystem shellParticle;

	public Animator animator;

	public Vector3 fpsOffset;

	private void Start()
	{
	}

	public void Recoil()
	{
		recoilling = true;
		endLocalRotationEuler = new Vector3(0f - UnityEngine.Random.Range(Mathf.Abs(minRotation.x), Mathf.Abs(maxRotation.x)), UnityEngine.Random.Range(minRotation.y, maxRotation.y), UnityEngine.Random.Range(minRotation.z, maxRotation.z));
		startRecoilValue = recoilValue;
		startRecoilRotation = gunModel.localRotation;
		if (triggerCameraRecoil)
		{
			GameManager.Instance.CameraManager.FpsCameraArm.recoil.Recoil();
		}
		if (subRecoils.Count > 0)
		{
			for (int i = 0; i < subRecoils.Count; i++)
			{
				subRecoils[i].Recoil();
			}
		}
		if ((bool)shellParticle)
		{
			shellParticle.Emit(1);
		}
	}

	private void Update()
	{
		if (!started)
		{
			if (!(GameManager.Instance != null) || !(GameManager.Instance.LevelManager != null))
			{
				return;
			}
			startPos = gunModel.localPosition;
			endPos = startPos + Vector3.back * backDistance;
			startLocalRotation = gunModel.localRotation;
			started = true;
			if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
			{
				startPos += fpsOffset;
				endPos += fpsOffset;
			}
		}
		if (recoilling)
		{
			recoilValue += Time.deltaTime * recoilSpeed;
			if (recoilValue >= 1f)
			{
				recoilValue = 1f;
				recoilling = false;
			}
		}
		else
		{
			recoilValue -= Time.deltaTime * recoverSpeed;
			if (recoilValue <= 0f)
			{
				recoilValue = 0f;
			}
		}
		if ((bool)animator)
		{
			animator.SetFloat("recoilValue", recoilValue);
		}
		UpdateGraphic();
	}

	private void UpdateGraphic()
	{
		gunModel.localPosition = Vector3.Lerp(startPos, endPos, recoilValue);
		if (recoilling)
		{
			gunModel.localRotation = Quaternion.Lerp(startRecoilRotation, Quaternion.Euler(endLocalRotationEuler), (recoilValue - startRecoilValue) / (1f - startRecoilValue));
		}
		else
		{
			gunModel.localRotation = Quaternion.Lerp(startLocalRotation, Quaternion.Euler(endLocalRotationEuler), recoilValue);
		}
	}
}
