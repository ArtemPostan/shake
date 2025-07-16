using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsSource : PooledBehaviour
{
	[HideInInspector]
	public Pool<SoundEffectsSource> master;

	private AudioSource _audioSource;

	public AudioSource audioSource
	{
		get
		{
			if (_audioSource == null)
			{
				_audioSource = GetComponent<AudioSource>();
			}
			return _audioSource;
		}
	}

	public void PlayClip(AudioClip clip)
	{
		audioSource.PlayOneShot(clip);
	}

	public override void Activate()
	{
		base.gameObject.SetActive(value: true);
	}

	public override void Deactivate()
	{
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (!audioSource.isPlaying)
		{
			master.Recycle(this);
		}
	}

	public override void Reset()
	{
	}
}
