using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class Breakable : MonoBehaviour
{
	[SerializeField]
	private int health;

	[SerializeField]
	private float breakDelay;

	[SerializeField]
	private GameObject breakParts;

	[SerializeField]
	private bool shake = true;

	[SerializeField]
	private Renderer renderer;

	[SerializeField]
	private Color hitColor;

	[SerializeField]
	private float hitIntensity;

	[Space]
	[Header("Boom")]
	[Space]
	[SerializeField]
	private bool boom;

	[SerializeField]
	private float boomRadius;

	[SerializeField]
	private float boomForce;

	[SerializeField]
	private int boomHurt = 15;

	private float hitEmiValue;

	private bool breaked;

	[SerializeField]
	private Rigidbody rb;

	[SerializeField]
	private float boomYVelocity;

	private float yVelocityLastFrame;

	private int MeleeWeaponAttackID = -1;

	private void Start()
	{
		breakDelay += UnityEngine.Random.Range(-0.3f, 0.3f);
	}

	private void Update()
	{
		if (hitEmiValue > 0f && renderer != null)
		{
			hitEmiValue -= Time.deltaTime * 7f;
			hitEmiValue = Mathf.Clamp01(hitEmiValue);
			renderer.material.SetColor("_EmissionColor", hitColor * hitIntensity * hitEmiValue);
		}
		if (health <= 0)
		{
			if (hitEmiValue <= 0f)
			{
				hitEmiValue = 1f;
			}
			breakDelay -= Time.deltaTime;
			if (breakDelay <= 0f)
			{
				Break();
			}
		}
	}

	public void Hurt(int hurt)
	{
		hitEmiValue = 1f;
		if (health >= 0)
		{
			health -= hurt;
		}
		else
		{
			breakDelay -= 0.8f;
		}
	}

	private void Break()
	{
		if (!breaked)
		{
			if (shake)
			{
				ProCamera2DShake.Instance.Shake("KillShake");
			}
			breaked = true;
			Object.Instantiate(breakParts, base.transform.position, base.transform.rotation);
			if (boom)
			{
				GameManager.Instance.BoomManager.Boom(base.transform.position, boomRadius, boomForce, boomHurt, 3);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void FixedUpdate()
	{
		if (rb != null)
		{
			yVelocityLastFrame = rb.velocity.y;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (((1 << other.gameObject.layer) & (int)GameManager.Instance.LayerManager.HurtLayer) != 0)
		{
			if (other.gameObject.tag == "Sword")
			{
				MeleeWeaponDamage component = other.gameObject.GetComponent<MeleeWeaponDamage>();
				if (component != null && MeleeWeaponAttackID != component.AttackID)
				{
					MeleeWeaponAttackID = component.AttackID;
					Hurt(component.attackDamage);
				}
			}
			else
			{
				Hurt(999999);
				breakDelay = 0f;
			}
		}
		else if (rb != null && yVelocityLastFrame <= 0f - Mathf.Abs(boomYVelocity))
		{
			Hurt(999999);
			breakDelay = 0f;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (((1 << collision.gameObject.layer) & (int)GameManager.Instance.LayerManager.HurtLayer) != 0)
		{
			Hurt(999999);
			breakDelay = 0f;
		}
		else if (rb != null && yVelocityLastFrame <= 0f - Mathf.Abs(boomYVelocity))
		{
			Hurt(999999);
			breakDelay = 0f;
		}
	}
}
