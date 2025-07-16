using System.Collections.Generic;
using UnityEngine;

public class ShellDrop : MonoBehaviour
{
	public ParticleSystem part;

	public List<ParticleCollisionEvent> collisionEvents;

	public float velocityThreshold = 1f;

	private void Start()
	{
		part = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
	}

	private void OnParticleCollision(GameObject other)
	{
		if (part.GetCollisionEvents(other, collisionEvents) > 0 && collisionEvents[0].velocity.y < 0f - velocityThreshold)
		{
			AudioManager.PlaySFXAtPosition("ShellDrop", collisionEvents[0].intersection);
		}
	}
}
