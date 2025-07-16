using UnityEngine;

public class HeadFly : MonoBehaviour
{
	private bool fly;

	private Rigidbody rb;

	public Collider collider;

	[Range(0f, 1f)]
	public float chance = 0.3f;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Fly(Vector3 direction)
	{
		if (!fly && !(UnityEngine.Random.Range(0.1f, 1000f) > chance * 1000f))
		{
			fly = true;
			rb = base.gameObject.AddComponent<Rigidbody>();
			collider.enabled = true;
			rb.mass = 2f;
			base.transform.SetParent(null);
			rb.velocity = Vector3.up * UnityEngine.Random.Range(5, 12);
			rb.AddForceAtPosition(direction * 10f, base.transform.position + Vector3.down * 0.3f);
			rb.angularDrag = 2f;
		}
	}
}
