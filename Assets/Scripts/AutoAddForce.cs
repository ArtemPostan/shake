using UnityEngine;

public class AutoAddForce : MonoBehaviour
{
	public Vector3 forceOffset;

	public float force;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.AddForce((UnityEngine.Random.insideUnitSphere + base.transform.TransformDirection(forceOffset)).normalized * force);
		rb.angularVelocity = UnityEngine.Random.insideUnitSphere * force * 0.1f;
	}

	private void Update()
	{
	}
}
