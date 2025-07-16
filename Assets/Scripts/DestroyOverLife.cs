using UnityEngine;

public class DestroyOverLife : MonoBehaviour
{
	public float life;

	public bool scaleToZero;

	public float scaleToZeroTime;

	private float timer;

	private float scaleTimer;

	private Vector3 startScale;

	private void Start()
	{
		timer = life;
		if (scaleToZero)
		{
			scaleTimer = scaleToZeroTime;
			startScale = base.transform.localScale;
		}
	}

	private void Update()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
			return;
		}
		if (!scaleToZero)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		scaleTimer -= Time.deltaTime;
		if (scaleTimer < 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			base.transform.localScale = Vector3.Lerp(Vector3.zero, startScale, scaleTimer / scaleToZeroTime);
		}
	}
}
