using UnityEngine;

public class Bullet : MonoBehaviour
{
	public enum bulletTypes
	{
		normal,
		boom
	}

	private int team;

	private Vector3 direction;

	private float speed;

	private RaycastHit[] hit;

	private float distance;

	private LayerMask playerLayer;

	private LayerMask unsavedBroLayer;

	private LayerMask enemyLayer;

	private LayerMask wallLayer;

	private LayerMask groundLayer;

	private LayerMask rigidBodyLayer;

	private LayerMask deadBodyLayer;

	private LayerMask hitAbleLayer;

	private LayerMask breakableLayer;

	private LayerMask hurtableLayer;

	private float life;

	private int layerTemp;

	private int damage;

	private bool needDestroy;

	private ParticleSystem wallParticleTemp;

	private ParticleSystem hitParticleTemp;

	private float sphereCastRadius = 0.16f;

	private bool back;

	private Combat fromCombat;

	private void Start()
	{
	}

	private void Update()
	{
		life -= Time.deltaTime;
		if (life <= 0f)
		{
			wallParticleTemp.transform.position = base.transform.position;
			wallParticleTemp.Emit(1);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		distance = speed * Time.deltaTime;
		int num = Physics.SphereCastNonAlloc(base.transform.position, sphereCastRadius, direction, hit, distance, hitAbleLayer);
		if (num > 0)
		{
			needDestroy = true;
			int num2 = -1;
			float num3 = distance;
			for (int i = 0; i < num; i++)
			{
				if (hit[i].distance < num3)
				{
					num3 = hit[i].distance;
					num2 = i;
				}
			}
			layerTemp = hit[num2].collider.gameObject.layer;
			if (((1 << layerTemp) & (int)hurtableLayer) != 0)
			{
				Combat component = hit[num2].collider.gameObject.GetComponent<Combat>();
				if ((bool)component && component != fromCombat)
				{
					EmitParticleAtPoint(hit[num2].point, hit[num2].normal);
					component.Hurt(damage, team, hit[num2].point);
					AudioManager.PlaySFXAtPosition("BulletHit_Flesh", hit[num2].point);
					if (((1 << layerTemp) & (int)deadBodyLayer) != 0 && hit[num2].collider.attachedRigidbody != null)
					{
						hit[num2].collider.attachedRigidbody.AddForceAtPosition(direction * 40f, hit[num2].point);
					}
				}
				else
				{
					needDestroy = false;
				}
			}
			else if (((1 << layerTemp) & (int)rigidBodyLayer) != 0)
			{
				EmitHitParticle(hit[num2].point);
				if (hit[num2].collider.attachedRigidbody != null)
				{
					hit[num2].collider.attachedRigidbody.AddForceAtPosition(direction * 800f, hit[num2].point);
				}
				AudioManager.PlaySFXAtPosition("BulletHit_Wall", hit[num2].point);
			}
			else if (((1 << layerTemp) & (int)breakableLayer) != 0)
			{
				EmitHitParticle(hit[num2].point);
				hit[num2].collider.gameObject.GetComponent<Breakable>().Hurt(damage);
				AudioManager.PlaySFXAtPosition("BulletHit_Fragile", hit[num2].point);
			}
			else if (((1 << layerTemp) & (int)wallLayer) != 0)
			{
				EmitHitParticle(hit[num2].point);
				AudioManager.PlaySFXAtPosition("BulletHit_Wall", hit[num2].point);
			}
			else if (((1 << layerTemp) & (int)GameManager.Instance.LayerManager.HurtLayer) != 0 && hit[num2].collider.gameObject.tag == "Sword")
			{
				needDestroy = false;
				if (!back)
				{
					BulletBack();
				}
			}
			if (needDestroy)
			{
				distance = hit[num2].distance;
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		base.transform.position += direction * distance;
	}

	private void BulletBack()
	{
		back = true;
		fromCombat = null;
		direction = -direction;
		speed *= 4f;
		damage = 25;
		team = 0;
		EmitHitParticle(base.transform.position);
		AudioManager.PlaySFXAtPosition("BulletHit_Back", base.transform.position);
	}

	private void EmitHitParticle(Vector3 _hitPoint)
	{
		wallParticleTemp.transform.position = _hitPoint;
		wallParticleTemp.Emit(1);
	}

	public void Init(Vector3 _startPoint, Vector3 _direction, float _speed, int _damage, float _life, int _team, Combat _fromCombat)
	{
		life = _life;
		base.transform.position = _startPoint;
		direction = _direction;
		base.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
		speed = _speed;
		team = _team;
		fromCombat = _fromCombat;
		if (team == 1 && GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.single)
		{
			speed *= 1.2f;
		}
		damage = _damage;
		hit = new RaycastHit[2];
		playerLayer = GameManager.Instance.LayerManager.PlayerLayer;
		enemyLayer = GameManager.Instance.LayerManager.EnemyLayer;
		wallLayer = ((int)GameManager.Instance.LayerManager.WallLayer | (int)GameManager.Instance.LayerManager.GroundLayer);
		rigidBodyLayer = GameManager.Instance.LayerManager.RigidBodyLayer;
		unsavedBroLayer = GameManager.Instance.LayerManager.UnsavedBroLayer;
		deadBodyLayer = GameManager.Instance.LayerManager.DeadBodyLayer;
		breakableLayer = GameManager.Instance.LayerManager.BreakableLayer;
		if (_team != 1 && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
		{
			base.transform.localScale = base.transform.lossyScale * 0.3f;
			sphereCastRadius *= 0.01f;
			speed *= 4f;
			hurtableLayer = ((int)unsavedBroLayer | (int)enemyLayer | (int)deadBodyLayer);
			hitAbleLayer = ((int)enemyLayer | (int)wallLayer | (int)unsavedBroLayer | (int)rigidBodyLayer | (int)deadBodyLayer | (int)breakableLayer);
			if (!GameManager.Instance.CameraManager.FpsCameraArm.Ads)
			{
				direction = 12f * direction + UnityEngine.Random.insideUnitSphere;
				direction.Normalize();
			}
			else
			{
				direction = 200f * direction + UnityEngine.Random.insideUnitSphere;
				direction.Normalize();
			}
		}
		else if (team != 1 && !FriendlyDamageSwitch.FriendlyDamageOn)
		{
			hurtableLayer = ((int)enemyLayer | (int)deadBodyLayer);
			hitAbleLayer = ((int)enemyLayer | (int)wallLayer | (int)rigidBodyLayer | (int)deadBodyLayer | (int)breakableLayer);
		}
		else
		{
			hurtableLayer = ((int)playerLayer | (int)unsavedBroLayer | (int)enemyLayer | (int)deadBodyLayer);
			hitAbleLayer = ((int)playerLayer | (int)enemyLayer | (int)wallLayer | (int)unsavedBroLayer | (int)rigidBodyLayer | (int)deadBodyLayer | (int)breakableLayer);
		}
		hitAbleLayer = ((int)hitAbleLayer | (int)GameManager.Instance.LayerManager.HurtLayer);
		wallParticleTemp = GameManager.Instance.ParticleManager.HitWallParticleEmitter;
		hitParticleTemp = GameManager.Instance.ParticleManager.HitParticleEmitter;
	}

	private void EmitParticleAtPoint(Vector3 point, Vector3 normal)
	{
		hitParticleTemp.transform.position = point;
		hitParticleTemp.transform.rotation = Quaternion.LookRotation(normal);
		hitParticleTemp.Emit(1);
	}
}
