using System.Collections;
using UnityEngine;

public class OrbitCam : MonoBehaviour
{
	public Transform target;

	public float distance = 5f;

	public float height = 5f;

	public float xSpeed = 120f;

	public float ySpeed = 120f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	public float distanceMin = 0.5f;

	public float distanceMax = 15f;

	public float smoothTime = 2f;

	public float groundHitPos = -4.2f;

	private float rotationYAxis;

	private float rotationXAxis;

	private float velocityX;

	private float velocityY;

	private Vector3 position;

	private bool snap1 = true;

	private bool snap2 = true;

	private bool snap3 = true;

	private float smoothdist;

	private Quaternion toRotation;

	private Vector3 negDistance;

	private float pan;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		rotationYAxis = eulerAngles.y;
		rotationXAxis = eulerAngles.x;
		smoothdist = distance;
	}

	private void LateUpdate()
	{
		if ((bool)target)
		{
			if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftShift))
			{
				velocityX += xSpeed * UnityEngine.Input.GetAxis("Mouse X") * 0.02f;
				velocityY += ySpeed * UnityEngine.Input.GetAxis("Mouse Y") * 0.02f;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) || UnityEngine.Input.GetKeyDown("[1]"))
			{
				StartCoroutine(SnappingTime(1));
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) || UnityEngine.Input.GetKeyDown("[2]"))
			{
				StartCoroutine(SnappingTime(2));
			}
			else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) || UnityEngine.Input.GetKeyDown("[3]"))
			{
				StartCoroutine(SnappingTime(3));
			}
			if (!snap1)
			{
				rotationYAxis = Mathf.Lerp(rotationYAxis, 0f, Time.deltaTime * smoothTime);
				rotationXAxis = Mathf.Lerp(rotationXAxis, 0f, Time.deltaTime * smoothTime);
			}
			else if (!snap2)
			{
				rotationYAxis = Mathf.Lerp(rotationYAxis, 90f, Time.deltaTime * smoothTime);
				rotationXAxis = Mathf.Lerp(rotationXAxis, 0f, Time.deltaTime * smoothTime);
			}
			else if (!snap3)
			{
				rotationYAxis = Mathf.Lerp(rotationYAxis, 90f, Time.deltaTime * smoothTime);
				rotationXAxis = Mathf.Lerp(rotationXAxis, 90f, Time.deltaTime * smoothTime);
			}
			else if (snap1 && snap2 && snap3)
			{
				rotationYAxis += velocityX;
				rotationXAxis -= velocityY;
			}
			yMinLimit = Mathf.Sin(-1.66f / distance) * 57.29578f;
			rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
			Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 0f);
			toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0f);
			smoothdist -= UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 5f;
			distance = Mathf.Lerp(distance, smoothdist, Time.deltaTime * smoothTime);
			if (Input.GetMouseButton(2))
			{
				height -= xSpeed * UnityEngine.Input.GetAxis("Mouse Y") * 0.005f;
				pan -= xSpeed * UnityEngine.Input.GetAxis("Mouse X") * 0.005f;
			}
			negDistance = new Vector3(pan, height, 0f - distance);
			position = toRotation * negDistance + target.position;
			base.transform.rotation = toRotation;
			base.transform.position = new Vector3(position.x, Mathf.Max(position.y, groundHitPos), position.z);
			velocityX = Mathf.Lerp(velocityX, 0f, Time.deltaTime * smoothTime);
			velocityY = Mathf.Lerp(velocityY, 0f, Time.deltaTime * smoothTime);
		}
	}

	private IEnumerator SnappingTime(int a)
	{
		if (a == 1)
		{
			snap1 = false;
			yield return new WaitForSeconds(0.5f);
			snap1 = true;
		}
		if (a == 2)
		{
			snap2 = false;
			yield return new WaitForSeconds(0.5f);
			snap2 = true;
		}
		if (a == 3)
		{
			snap3 = false;
			yield return new WaitForSeconds(0.5f);
			snap3 = true;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
