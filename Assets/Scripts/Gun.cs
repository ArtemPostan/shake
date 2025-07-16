using FlamingCore;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField]
	private int team;

	[SerializeField]
	private bool startActive;

	[SerializeField]
	private bool isMeleeWeapon;

	[SerializeField]
	private MeleeWeaponControl meleeWeaponControl;

	[SerializeField]
	private Transform[] muzzles;

	[SerializeField]
	private Bullet bullet;

	[SerializeField]
	private float bulletLife = 3.5f;

	[SerializeField]
	private float speed;

	[SerializeField]
	private int damage = 3;

	[SerializeField]
	private float shootTime;

	[SerializeField]
	private float autoShootRange;

	public ObsticleChecker obsticleChecker;

	[HideInInspector]
	public bool Active;

	private float realShootTime;

	private float shootTimer;

	private bool canShoot;

	private ParticleSystem muzzleParticle;

	public ParticleSystem overrideMuzzleParticle;

	public string overrideSFX;

	private bool inited;

	private PlayerControl player;

	public GunRecoil recoil;

	public bool autoTrigger = true;

	public Light muzzleLight;

	private float lightIntensity;

	private float lightValue;

	public Animator animator;

	private float adsValue;

	public Combat ownerCombat;

	public bool IsMeleeWeapon => isMeleeWeapon;

	private void Awake()
	{
		if ((bool)muzzleLight)
		{
			lightIntensity = muzzleLight.intensity;
		}
	}

	private void Update()
	{
		if (!inited)
		{
			if (GameManager.Instance.LevelManager.GameState == LevelManager.gameStates.playing)
			{
				inited = true;
				realShootTime = shootTime + UnityEngine.Random.Range(-0.02f, 0.02f);
				if (team == 1 && GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.single)
				{
					realShootTime *= 0.5f;
				}
				if ((bool)overrideMuzzleParticle)
				{
					muzzleParticle = overrideMuzzleParticle;
				}
				else
				{
					muzzleParticle = GameManager.Instance.ParticleManager.ShootParticleEmitter;
				}
				player = GameManager.Instance.LevelManager.Player;
				if (startActive)
				{
					Active = true;
				}
			}
		}
		else
		{
			if (team == 1)
			{
				float num = autoShootRange;
				if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
				{
					num = autoShootRange * 3f;
					if (obsticleChecker != null)
					{
						obsticleChecker.checkDistance = num;
					}
				}
				canShoot = (Vector3.Distance(base.transform.position, player.transform.position) < num);
				if (obsticleChecker != null && obsticleChecker.Obsticle)
				{
					canShoot = false;
				}
			}
			if (shootTimer < realShootTime)
			{
				shootTimer += Time.deltaTime;
			}
			bool flag = (autoTrigger && Input.GetMouseButton(0)) || (!autoTrigger && Input.GetMouseButtonDown(0));
			if ((canShoot || (flag && !player.Combat.IsDead && team != 1)) && Active && shootTimer >= realShootTime)
			{
				ShootOneBullet();
				shootTimer -= realShootTime;
				realShootTime = shootTime;
				if (team == 1)
				{
					realShootTime += UnityEngine.Random.Range(-0.05f, 0.05f);
				}
			}
		}
		if ((bool)muzzleLight)
		{
			muzzleLight.intensity = lightValue * lightIntensity;
			if (lightValue > 0f)
			{
				lightValue -= 8f * Time.deltaTime;
				lightValue = Mathf.Clamp01(lightValue);
			}
		}
		if (!animator)
		{
			return;
		}
		if (team != 1 && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
		{
			FpsCameraArm fpsCameraArm = GameManager.Instance.CameraManager.FpsCameraArm;
			if (fpsCameraArm.Ads)
			{
				adsValue = Mathf.MoveTowards(adsValue, 1f, Time.deltaTime / fpsCameraArm.AdsTime);
			}
			else
			{
				adsValue = Mathf.MoveTowards(adsValue, 0f, Time.deltaTime / fpsCameraArm.AdsTime);
			}
		}
		else
		{
			adsValue = 0f;
		}
		animator.SetFloat("ads", adsValue);
	}

	public void ShootOneBullet()
	{
		lightValue = 1f;
		if ((bool)recoil)
		{
			recoil.Recoil();
		}
		if (overrideSFX == "")
		{
			AudioManager.PlaySFXAtPosition("Fire", base.transform.position, 0.8f, -0.1f, 0.1f);
		}
		else
		{
			AudioManager.PlaySFXAtPosition(overrideSFX, base.transform.position, 0.8f, -0.1f, 0.1f);
		}
		if (isMeleeWeapon)
		{
			meleeWeaponControl.Attack();
			return;
		}
		for (int i = 0; i < muzzles.Length; i++)
		{
			muzzleParticle.transform.position = muzzles[i].position;
			muzzleParticle.Emit(1);
			Vector3 startPoint;
			Vector3 vector;
			if (team != 1 && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
			{
				startPoint = GameManager.Instance.CameraManager.FpsCameraArm.transform.TransformPoint(0f, 0f, 0.6f);
				vector = GameManager.Instance.CameraManager.FpsCameraArm.transform.forward;
			}
			else
			{
				startPoint = muzzles[i].position;
				vector = muzzles[i].forward;
				if (GameManager.Instance.LevelManager.game3CType != LevelManager.game3Ctypes.fps)
				{
					vector = FCTool.Vector3YToZero(vector);
				}
			}
			Object.Instantiate(bullet).Init(startPoint, vector, speed, damage, bulletLife, team, ownerCombat);
		}
	}
}
