using UnityEngine;

public abstract class PooledBehaviour : MonoBehaviour
{
	public abstract void Reset();

	public abstract void Activate();

	public abstract void Deactivate();
}
