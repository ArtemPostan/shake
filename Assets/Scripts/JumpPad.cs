using UnityEngine;

public class JumpPad : MonoBehaviour
{
	public float addSpeed;

	public SphereCollider sphereCollider;

	private float startRadius;

	public float radiusGain = 0.15f;

	private Rigidbody rb;

	private void Start()
	{
		startRadius = sphereCollider.radius;
	}

	private void Update()
	{
		sphereCollider.radius = Mathf.MoveTowards(sphereCollider.radius, startRadius, 1f * Time.deltaTime);
	}

	public void OnTriggerEnter(Collider other)
	{
		rb = other.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.velocity += Vector3.up * addSpeed;
		}
		sphereCollider.radius = startRadius + radiusGain;
	}
}
