using UnityEngine;

public class Marker : MonoBehaviour
{
	public Vector3 keepRotation;

	public Vector3 localOffset;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(keepRotation);
		base.transform.position = base.transform.parent.position + localOffset;
	}
}
