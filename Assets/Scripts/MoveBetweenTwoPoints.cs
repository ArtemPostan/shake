using UnityEngine;

public class MoveBetweenTwoPoints : PeriodicalBehaviour
{
	public Vector3 posA;

	public Vector3 posB;

	public Transform target;

	public Rigidbody2D rigidbody;

	public AnimationCurve curve;

	public float recalibrateThreshold = 1f;

	public Vector3 posAGlobal => base.transform.position + posA;

	public Vector3 posBGlobal => base.transform.position + posB;

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(posAGlobal, 0.1f);
		Gizmos.DrawCube(posBGlobal, Vector3.one * 0.1f);
		Gizmos.DrawLine(posAGlobal, posBGlobal);
	}

	protected override void Update()
	{
		base.Update();
		if (!useFixedTime)
		{
			float t = curve.Evaluate(base.t01);
			Vector3 b = base.transform.position + Vector3.Lerp(posA, posB, t);
			float t2 = curve.Evaluate(base.t01 + Time.deltaTime / period);
			Vector2 a = base.transform.position + Vector3.Lerp(posA, posB, t2) - b;
			if (base.timer.paused)
			{
				a = Vector2.zero;
			}
			if (rigidbody != null)
			{
				rigidbody.velocity = a / Time.deltaTime;
			}
			else
			{
				Recalibrate();
			}
			if ((base.transform.position - b).magnitude > recalibrateThreshold)
			{
				Recalibrate();
			}
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (useFixedTime)
		{
			float t = curve.Evaluate(base.t01);
			Vector3 b = base.transform.position + Vector3.Lerp(posA, posB, t);
			float t2 = curve.Evaluate(base.t01 + Time.fixedDeltaTime / period);
			Vector2 a = base.transform.position + Vector3.Lerp(posA, posB, t2) - b;
			if (base.timer.paused)
			{
				a = Vector2.zero;
			}
			if (rigidbody != null)
			{
				rigidbody.velocity = a / Time.fixedDeltaTime;
			}
			else
			{
				Recalibrate();
			}
			if ((base.transform.position - b).magnitude > recalibrateThreshold)
			{
				Recalibrate();
			}
		}
	}

	public override void Tick(int i)
	{
		base.Tick(i);
		Recalibrate();
	}

	public void Recalibrate()
	{
		float t = curve.Evaluate(base.t01);
		Vector3 vector = base.transform.position + Vector3.Lerp(posA, posB, t);
		if (rigidbody != null)
		{
			rigidbody.MovePosition(vector);
		}
		else if (target != null)
		{
			target.position = vector;
		}
	}

	private void Start()
	{
		Recalibrate();
	}
}
