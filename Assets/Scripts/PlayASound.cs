using UnityEngine;

public class PlayASound : MonoBehaviour
{
	public AudioClip clip;

	public void Invoke()
	{
		AudioManager.PlaySFXAtPosition(clip, base.transform.position);
	}
}
