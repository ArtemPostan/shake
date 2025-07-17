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

	private Joystick fireJoystick;

	public bool IsMeleeWeapon => isMeleeWeapon;

	private void Awake()
	{
		if ((bool)muzzleLight)
		{
			lightIntensity = muzzleLight.intensity;
		}
	}

    private void Start()
    {
        fireJoystick = GameManager.Instance.UIManager.FireJoystickUI.GetComponent<Joystick>();
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
                    realShootTime *= 0.5f;

                muzzleParticle = overrideMuzzleParticle ? overrideMuzzleParticle : GameManager.Instance.ParticleManager.ShootParticleEmitter;
                player = GameManager.Instance.LevelManager.Player;

                if (startActive)
                    Active = true;
            }
            return;
        }

        if (team == 1)
        {
            float range = GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps ? autoShootRange * 3f : autoShootRange;
            if (obsticleChecker != null)
                obsticleChecker.checkDistance = range;

            canShoot = Vector3.Distance(transform.position, player.transform.position) < range;
            if (obsticleChecker != null && obsticleChecker.Obsticle)
                canShoot = false;
        }

        if (shootTimer < realShootTime)
            shootTimer += Time.deltaTime;

        // Ввод выстрела:
        bool shouldShoot = false;
        Vector3 shootDirection = transform.forward;

        if (team == 1)
        {
            // ВРАГ — стреляет в игрока
            Vector3 toPlayer = player.transform.position - transform.position;
            shootDirection = FCTool.Vector3YToZero(toPlayer).normalized;
        }
        else if (!GameManager.Instance.isMobile)
        {
            // ИГРОК на ПК — стреляет в сторону курсора
            bool flag = (autoTrigger && Input.GetMouseButton(0)) || (!autoTrigger && Input.GetMouseButtonDown(0));
            shouldShoot = flag;

            if (GameManager.Instance.LevelManager.game3CType != LevelManager.game3Ctypes.fps)
            {
                Vector3 pointerDir = FCTool.Vector3YToZero(LevelManager.instance.Pointer.position - transform.position);
                shootDirection = pointerDir.normalized;
            }
            else
            {
                shootDirection = GameManager.Instance.CameraManager.FpsCameraArm.transform.forward;
            }
        }
        else
        {
            Vector3 inputDir = new Vector3(fireJoystick.Horizontal, 0f, fireJoystick.Vertical);

            if (inputDir.magnitude > 0.2f && LevelManager.instance.Pointer != null)
            {
                shouldShoot = true;

                Vector3 toPointer = LevelManager.instance.Pointer.position - transform.position;
                shootDirection = FCTool.Vector3YToZero(toPointer).normalized;
            }
        }


        // Выполняем выстрел
        if ((canShoot || (shouldShoot && team != 1 && !player.Combat.IsDead)) && Active && shootTimer >= realShootTime)
        {
            ShootOneBullet(shootDirection);
            shootTimer = 0f;
            realShootTime = shootTime;
            if (team == 1)
                realShootTime += UnityEngine.Random.Range(-0.05f, 0.05f);
        }

        // Освещение вспышки
        if (muzzleLight)
        {
            muzzleLight.intensity = lightValue * lightIntensity;
            if (lightValue > 0f)
            {
                lightValue -= 8f * Time.deltaTime;
                lightValue = Mathf.Clamp01(lightValue);
            }
        }

        // ADS анимация
        if (animator)
        {
            if (team != 1 && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
            {
                FpsCameraArm cam = GameManager.Instance.CameraManager.FpsCameraArm;
                adsValue = Mathf.MoveTowards(adsValue, cam.Ads ? 1f : 0f, Time.deltaTime / cam.AdsTime);
            }
            else
            {
                adsValue = 0f;
            }

            animator.SetFloat("ads", adsValue);
        }
    }


    public void ShootOneBullet(Vector3 direction)
    {
        lightValue = 1f;

        if (recoil)
            recoil.Recoil();

        if (string.IsNullOrEmpty(overrideSFX))
            AudioManager.PlaySFXAtPosition("Fire", transform.position, 0.8f, -0.1f, 0.1f);
        else
            AudioManager.PlaySFXAtPosition(overrideSFX, transform.position, 0.8f, -0.1f, 0.1f);

        if (isMeleeWeapon)
        {
            meleeWeaponControl.Attack();
            return;
        }

        for (int i = 0; i < muzzles.Length; i++)
        {
            muzzleParticle.transform.position = muzzles[i].position;
            muzzleParticle.Emit(1);

            Vector3 startPoint = muzzles[i].position;
            Vector3 dir = direction;

            if (team != 1 && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
            {
                startPoint = GameManager.Instance.CameraManager.FpsCameraArm.transform.TransformPoint(0f, 0f, 0.6f);
                dir = GameManager.Instance.CameraManager.FpsCameraArm.transform.forward;
            }

            if (GameManager.Instance.LevelManager.game3CType != LevelManager.game3Ctypes.fps)
            {
                dir = FCTool.Vector3YToZero(dir);
            }

            Instantiate(bullet).Init(startPoint, dir, speed, damage, bulletLife, team, ownerCombat);
        }
    }

}
