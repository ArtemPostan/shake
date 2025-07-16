using Com.LuisPedroFonseca.ProCamera2D;
using System;
using UnityEngine;

public class Combat : MonoBehaviour
{
	public delegate void KillAllDelegate();

	[SerializeField]
	private int maxHealth = 10;

	[SerializeField]
	private int team;

	public float deadBodyStayTime = 4f;

	private Rigidbody rb;

	private SnakePart part;

	private bool dead;

	private bool actived;

	public Animator animator;

	private float animatorSpeed;

	[SerializeField]
	private Gun gunPfb;

	private Gun gun;

	[SerializeField]
	private Transform topDownGunParent;

	[SerializeField]
	private Transform fpsGunParent;

	[SerializeField]
	private GameObject rope;

	[SerializeField]
	private GameObject ring;

	[SerializeField]
	private ParticleSystem footParticle;

	[SerializeField]
	private ParticleSystem bloodParticle;

	private int health;

	private ParticleSystem.EmissionModule partEmi;

	private bool inited;

	private bool gameStarted;

	public Transform headPoint;

	public float headPointRadius = 0.3f;

	public ObsticleChecker obsticleChecker;

	public static KillAllDelegate KillAllStaticEvent;

	private bool isBlack;

	[HideInInspector]
	public bool ignoreExplosion;

	[SerializeField]
	private HeadFly headFly;

	private int MeleeWeaponAttackID = -1;

	public SnakePart Part => part;

	public bool IsDead => dead;

	public bool Actived => actived;

	public Gun Gun => gun;

	public float HealthPercent => Mathf.Clamp01((float)health / (float)maxHealth);

	private void Awake()
	{
		Init();
		KillAllStaticEvent = (KillAllDelegate)Delegate.Combine(KillAllStaticEvent, new KillAllDelegate(KillIfNotHead));
	}

	private void OnDestroy()
	{
		KillAllStaticEvent = (KillAllDelegate)Delegate.Remove(KillAllStaticEvent, new KillAllDelegate(KillIfNotHead));
		if (headFly != null)
		{
			UnityEngine.Object.Destroy(headFly.gameObject);
		}
	}

	private void Init()
	{
		if (!inited)
		{
			inited = true;
			health = maxHealth;
			part = GetComponent<SnakePart>();
			rb = GetComponent<Rigidbody>();
			if (rope != null)
			{
				rope.SetActive(value: true);
			}
			ChangeGun(gunPfb);
			if (!IsHead() && team != 1 && ring != null)
			{
				ring.SetActive(value: false);
			}
			rb.constraints = RigidbodyConstraints.None;
			rb.angularVelocity = UnityEngine.Random.insideUnitSphere * 2000f;
			if (bloodParticle != null && footParticle != null)
			{
				partEmi = bloodParticle.emission;
				partEmi.rateOverTime = 0f;
			}
		}
	}

	public void Active()
	{
		if (!actived)
		{
			Init();
			rb.constraints = RigidbodyConstraints.FreezeRotation;
			if (rope != null)
			{
				rope.SetActive(value: false);
			}
			gun.gameObject.SetActive(value: true);
			gun.Active = true;
			if (ring != null)
			{
				ring.SetActive(value: true);
			}
			actived = true;
			SaveCount();
		}
	}

	private void Update()
	{
		if (!gameStarted)
		{
			if (GameManager.Instance.LevelManager.GameState == LevelManager.gameStates.playing)
			{
				gameStarted = true;
				BirthCount();
			}
			return;
		}
		if (dead && !IsHead())
		{
			deadBodyStayTime -= Time.deltaTime;
			if (deadBodyStayTime <= 0f)
			{
				GameManager.Instance.ParticleManager.DieParticleEmitter.transform.position = base.transform.position;
				GameManager.Instance.ParticleManager.DieParticleEmitter.Emit(1);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		if (team == 1)
		{
			return;
		}
		if (actived && !dead)
		{
			if (rb.velocity.magnitude > 0.5f && animatorSpeed < 1f)
			{
				animatorSpeed = Mathf.Clamp01(animatorSpeed + Time.deltaTime * 4f);
			}
			else
			{
				animatorSpeed = Mathf.Clamp01(animatorSpeed - Time.deltaTime * 4f);
			}
		}
		else
		{
			if (!actived)
			{
				animatorSpeed = 1f;
			}
			if (dead)
			{
				animatorSpeed = 0.5f;
			}
		}
		animator.SetFloat("Speed", animatorSpeed);
	}

	public void Hurt(int hurt, int _team, Vector3 _hurtPoint)
	{
		if (!dead)
		{
			if (_team == team)
			{
				return;
			}
			health -= hurt;
			if ((bool)headPoint && Vector3.Distance(_hurtPoint, headPoint.position) <= headPointRadius)
			{
				health -= hurt;
			}
			if (IsHead())
			{
				GameManager.Instance.PostManager.StartHurt();
			}
			if (health <= 0)
			{
				if (IsHead())
				{
					GameManager.Instance.PostManager.StartHurt();
					GameManager.Instance.LevelManager.Defeat(DefeatType.died);
				}
				Die(_team);
			}
		}
		else
		{
			deadBodyStayTime -= 0.8f;
		}
	}

	public bool IsHead()
	{
		if (part != null)
		{
			return part.IsHead;
		}
		return false;
	}

	private void Die(int _killerTeam)
	{
		if (dead)
		{
			return;
		}
		DeadCount();
		gun.Active = false;
		gun.gameObject.SetActive(value: false);
		dead = true;
		AudioManager.PlaySFXAtPosition("Death_Flesh", base.transform.position);
		AudioManager.PlaySFXAtPosition("Scream", base.transform.position);
		if (team != 1)
		{
			if (IsHead())
			{
				DataManager.CountPlayerDead();
			}
			DataManager.CountFriendlyDead();
			if (_killerTeam != 1)
			{
				DataManager.CountFriendlyKilled();
				if (Part != null && !Part.Actived)
				{
					DataManager.CountFriendlyKilledUnsaved();
				}
			}
			if (GameManager.Instance.LevelManager.GameState == LevelManager.gameStates.success && GameManager.Instance.LevelManager.TeamBroCount == 0)
			{
				DataManager.SetAchievement("killAllFriendliesWhenFinished");
			}
		}
		else
		{
			DataManager.CountEnemyKilled();
		}
		if (ring != null)
		{
			ring.SetActive(value: false);
		}
		if (rope != null)
		{
			rope.SetActive(value: false);
		}
		base.gameObject.layer = LayerMask.NameToLayer("DeadBody");
		ProCamera2DShake.Instance.Shake("KillShake");
		if (part != null)
		{
			part.Remove();
		}
		rb.constraints = RigidbodyConstraints.None;
		GetComponent<Collider>().material = GameManager.Instance.PhysicsManager.deadBodyMaterial;
		rb.AddForce((UnityEngine.Random.insideUnitSphere + Vector3.up * 3f).normalized * 100f);
		rb.angularVelocity = UnityEngine.Random.insideUnitSphere * 100f;
		if (IsHead())
		{
			rb.mass = 0.5f;
		}
		GameManager.Instance.ParticleManager.DieParticleEmitter.transform.position = base.transform.position;
		GameManager.Instance.ParticleManager.DieParticleEmitter.Emit(1);
		if (bloodParticle != null && footParticle != null)
		{
			partEmi = bloodParticle.emission;
			partEmi.rateOverTime = 20f;
			partEmi = footParticle.emission;
			partEmi.rateOverDistance = 0f;
			bloodParticle.Play();
		}
	}

	private void DeadCount()
	{
		if (!IsHead() && team == 0)
		{
			GameManager.Instance.LevelManager.KillABro(actived);
		}
		else if (team == 1)
		{
			GameManager.Instance.LevelManager.KillAEnemy();
		}
	}

	public void ChangeGun(Gun _gunPfb)
	{
		if ((bool)gun)
		{
			UnityEngine.Object.Destroy(gun.gameObject);
		}
		gun = UnityEngine.Object.Instantiate(_gunPfb);
		gun.ownerCombat = this;
		if (IsHead() && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
		{
			gun.transform.SetParent(fpsGunParent);
		}
		else
		{
			gun.transform.SetParent(topDownGunParent);
		}
		gun.transform.localPosition = Vector3.zero;
		gun.transform.localRotation = Quaternion.Euler(Vector3.zero);
		if (!IsHead() && team != 1 && !actived)
		{
			gun.gameObject.SetActive(value: false);
		}
		else
		{
			gun.Active = true;
		}
		if (obsticleChecker != null)
		{
			gun.obsticleChecker = obsticleChecker;
		}
	}

	private void BirthCount()
	{
		if (!IsHead() && team == 0)
		{
			GameManager.Instance.LevelManager.CountABro();
		}
	}

	private void SaveCount()
	{
		if (!IsHead() && team == 0)
		{
			GameManager.Instance.LevelManager.SaveABro();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (((1 << other.gameObject.layer) & (int)GameManager.Instance.LayerManager.HurtLayer) == 0)
		{
			return;
		}
		if (other.gameObject.tag == "Sword")
		{
			MeleeWeaponDamage component = other.gameObject.GetComponent<MeleeWeaponDamage>();
			if (component != null && MeleeWeaponAttackID != component.AttackID)
			{
				MeleeWeaponAttackID = component.AttackID;
				Hurt(component.attackDamage, component.team, other.transform.position);
				Vector3 position;
				if ((bool)headPoint)
				{
					position = headPoint.position;
				}
				else
				{
					position = base.transform.position;
					position.y = Camera.main.transform.position.y;
				}
				UnityEngine.Object.Instantiate(GameManager.Instance.ParticleManager.meleeHitBloodPfb, position, Quaternion.LookRotation(component.attackRight * component.transform.right, Vector3.up));
				if ((bool)headFly && health <= 0)
				{
					headFly.Fly(component.attackRight * component.transform.right);
				}
			}
		}
		else
		{
			Hurt(25, 3, other.transform.position);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (((1 << collision.gameObject.layer) & (int)GameManager.Instance.LayerManager.HurtLayer) != 0)
		{
			Hurt(25, 3, collision.transform.position);
		}
	}

	private void OnDrawGizmos()
	{
		if (headPoint != null)
		{
			Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.3f);
			Gizmos.DrawSphere(headPoint.position, headPointRadius);
		}
	}

	private void KillIfNotHead()
	{
		if (!IsHead() && !isBlack && base.transform != null)
		{
			Hurt(99999, 0, base.transform.position);
		}
	}

	public static void KillAllEnemies()
	{
		KillAllStaticEvent?.Invoke();
	}

	public void SetAsBlack()
	{
		isBlack = true;
	}
}
