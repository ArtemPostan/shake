using UnityEngine;

public class PlayBGM : MonoBehaviour
{
	public AudioClip bgmClip;

	public bool playOnStart = true;

	public void Invoke()
	{
		AudioManager.PlayBGM(bgmClip);
	}

	private void Start()
	{
		if (playOnStart)
		{
			Invoke();
		}
	}
}
