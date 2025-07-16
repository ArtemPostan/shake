using UnityEngine;

public class ringRotateLerp : MonoBehaviour
{
	public float lerpSpeed = 10f;

	private Transform parent;

	private Vector3 offset;

	private void Start()
	{
		parent = base.transform.parent;
		offset = base.transform.localPosition;
		base.transform.SetParent(null);
	}

	private void Update()
	{
		base.transform.position = parent.TransformPoint(offset);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, parent.rotation, lerpSpeed * Time.unscaledDeltaTime);
	}
}
