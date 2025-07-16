using UnityEngine;

public class CarControl : MonoBehaviour
{
	public Vector3 carRotation;

	public float lerpSpeed;

	private float lerpValue;

	private bool ads;

	private void Start()
	{
	}

	private void Update()
	{
		ads = GameManager.Instance.CameraManager.FpsCameraArm.Ads;
		lerpValue = Mathf.MoveTowards(lerpValue, ads ? 1 : 0, lerpSpeed * Time.deltaTime);
		base.transform.localRotation = Quaternion.Euler(Vector3.Lerp(carRotation, Vector3.zero, lerpValue));
	}
}
